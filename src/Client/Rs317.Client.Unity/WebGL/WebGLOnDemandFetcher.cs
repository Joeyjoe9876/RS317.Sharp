using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class WebGLOnDemandFetcher : OnDemandFetcher
	{
		private ITaskDelayFactory TaskDelayFactory { get; }

		public WebGLOnDemandFetcher([NotNull] ITaskDelayFactory taskDelayFactory)
		{
			TaskDelayFactory = taskDelayFactory ?? throw new ArgumentNullException(nameof(taskDelayFactory));
		}

		public void Initialize(Archive streamLoader, IBaseClient client)
		{
			String[] strings = { "model_version", "anim_version", "midi_version", "map_version" };
			for(int i = 0; i < 4; i++)
			{
				byte[] abyte0 = streamLoader.decompressFile(strings[i]);
				int j = abyte0.Length / 2;
				Default317Buffer stream = new Default317Buffer(abyte0);
				versions[i] = new int[j];
				filePriorities[i] = new byte[j];
				for(int l = 0; l < j; l++)
					versions[i][l] = stream.getUnsignedLEShort();

			}

			String[] strings2 = { "model_crc", "anim_crc", "midi_crc", "map_crc" };
			for(int k = 0; k < 4; k++)
			{
				byte[] abyte1 = streamLoader.decompressFile(strings2[k]);
				int i1 = abyte1.Length / 4;
				Default317Buffer stream_1 = new Default317Buffer(abyte1);
				crcs[k] = new int[i1];
				for(int l1 = 0; l1 < i1; l1++)
					crcs[k][l1] = stream_1.getInt();

			}

			byte[] abyte2 = streamLoader.decompressFile("model_index");
			int j1 = versions[0].Length;
			modelIndices = new byte[j1];
			for(int k1 = 0; k1 < j1; k1++)
				if(k1 < abyte2.Length)
					modelIndices[k1] = abyte2[k1];
				else
					modelIndices[k1] = 0;

			MapIndexArchiveDeserializer mapIndexDeserializer = new MapIndexArchiveDeserializer(streamLoader);
			MapIndices = mapIndexDeserializer.Deserialize();

			abyte2 = streamLoader.decompressFile("anim_index");
			Default317Buffer stream2 = new Default317Buffer(abyte2);
			j1 = abyte2.Length / 2;
			frames = new int[j1];
			for(int j2 = 0; j2 < j1; j2++)
				frames[j2] = stream2.getUnsignedLEShort();

			abyte2 = streamLoader.decompressFile("midi_index");
			stream2 = new Default317Buffer(abyte2);
			j1 = abyte2.Length;
			musicPriorities = new int[j1];
			for(int k2 = 0; k2 < j1; k2++)
				musicPriorities[k2] = stream2.getUnsignedByte();

			running = true;
			clientInstance = client;
		}

		public async Task preloadRegionsAsync(bool flag)
		{
			int j = MapIndices.Count;
			for(int k = 0; k < j; k++)
				if(flag || MapIndices[k].isMembers)
				{
					setPriority((byte)2, 3, MapIndices[k].ObjectFileId);
					setPriority((byte)2, 3, MapIndices[k].TerrainId);

					if(k % 2 == 0)
						await TaskDelayFactory.Create(1);
				}
		}

		public IEnumerator RunCoroutine()
		{
			while(running)
			{
				onDemandCycle++;
				int i = 20;
				if(highestPriority == 0 && clientInstance.caches[0] != null)
					i = 50;

				yield return new WaitForSeconds((float)i / 1000.0f);

				waiting = true;
				for(int j = 0; j < 100; j++)
				{
					if(!waiting)
						break;
					waiting = false;
					checkReceived(false);
					handleFailed();
					if(uncompletedCount == 0 && j >= 5)
						break;
					method568();
					if(inputStream != null)
						readData();
				}

				bool flag = false;
				for(OnDemandData onDemandData = (OnDemandData)requested
						.peekFront();
					onDemandData != null;
					onDemandData = (OnDemandData)requested.getPrevious())
					if(onDemandData.incomplete)
					{
						flag = true;
						onDemandData.loopCycle++;
						if(onDemandData.loopCycle > 50)
						{
							onDemandData.loopCycle = 0;
							closeRequest(onDemandData);
						}
					}

				if(!flag)
				{
					for(OnDemandData onDemandData_1 = (OnDemandData)requested
							.peekFront();
						onDemandData_1 != null;
						onDemandData_1 = (OnDemandData)requested
							.getPrevious())
					{
						flag = true;
						onDemandData_1.loopCycle++;
						if(onDemandData_1.loopCycle > 50)
						{
							onDemandData_1.loopCycle = 0;
							closeRequest(onDemandData_1);
						}
					}

				}

				if(flag)
				{
					loopCycle++;
					if(loopCycle > 750)
					{
						try
						{
							socket.Close();
						}
						catch(Exception _ex)
						{
						}

						socket = null;
						inputStream = null;
						outputStream = null;
						expectedSize = 0;
					}
				}
				else
				{
					loopCycle = 0;
					statusString = "";
				}

				if(clientInstance.isLoggedIn && socket != null && outputStream != null
					&& (highestPriority > 0 || clientInstance.caches[0] == null))
				{
					writeLoopCycle++;
					if(writeLoopCycle > 500)
					{
						writeLoopCycle = 0;
						payload[0] = 0;
						payload[1] = 0;
						payload[2] = 0;
						payload[3] = 10;
						try
						{
							outputStream.Write(payload, 0, 4);
						}
						catch(IOException _ex)
						{
							loopCycle = 5000;
						}
					}
				}
			}
		}
	}
}
