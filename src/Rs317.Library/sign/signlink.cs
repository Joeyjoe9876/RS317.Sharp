using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rs317.Sharp
{

	public sealed class signlink
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static void dnslookup(String s)
		{
			throw new NotImplementedException($"TODO: DNS requests are temporarily disabled.");
		}

		private static String findcachedir()
		{
			return "./cache/";
		}

		//Pulled from R2Sharp
		public static String findcachedirORIG()
		{
			String[] locations =
			{
				"c:/windows/", "c:/winnt/", "d:/windows/", "d:/winnt/", "e:/windows/", "e:/winnt/",
				"f:/windows/", "f:/winnt/", "c:/", "~/", "/tmp/", "", "c:/rscache", "/rscache"
			};
			if(storeid < 32 || storeid > 34)
				storeid = 32;
			String s = ".file_store_" + storeid;
			for(int i = 0; i < locations.Length; i++)
				try
				{
					String s1 = locations[i];
					if(s1.Length > 0)
					{
						if(!Directory.Exists(s1))
							continue;
					}

					if(File.Exists(s1 + s) || Directory.Exists(s1 + s))
						return s1 + s + "/";
				}
				catch(Exception _ex)
				{
					throw new InvalidOperationException($"Failed: {nameof(findcachedirORIG)}");
				}

			return null;
		}

		private static int getuid(String s)
		{
			try
			{
				string uidPath = Path.Combine(s, "uid.dat");
				bool exists = File.Exists(uidPath);
				using(FileStream uidFile = File.Open(uidPath, FileMode.OpenOrCreate))
				{
					if(!exists || uidFile.Length < 4L)
					{
						BinaryWriter dataoutputstream = new BinaryWriter(uidFile);
						dataoutputstream.Write((int)(StaticRandomGenerator.Next() * 99999999D));
						dataoutputstream.Close();
					}
				}
			}
			catch(Exception _ex)
			{
				throw new InvalidOperationException($"Failed to generate and write uid. Reason: {_ex.Message}", _ex);
			}

			try
			{
				BinaryReader datainputstream = new BinaryReader(File.OpenRead(s + "uid.dat"), Encoding.Default, false);
				int i = datainputstream.ReadInt32();
				datainputstream.Close();
				return i + 1;
			}
			catch(Exception _ex)
			{
				throw new InvalidOperationException($"Failed to read uid.");
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public static void midisave(byte[] abyte0, int i)
		{
			if(i > 0x1e8480)
				return;
			if(savereq != null)
			{
			}
			else
			{
				midipos = (midipos + 1) % 5;
				savelen = i;
				savebuf = abyte0;
				midiplay = true;
				savereq = "jingle" + midipos + ".mid";
			}
		}

		//TODO: Do we need to re-add the syncronization?
		public static async Task<TcpClient> openSocket(int port)
		{
			if(port < 0) throw new ArgumentOutOfRangeException(nameof(port));

			//I think the rest of the codebase assumes we DON'T actually
			//mantain a single tcp client state.

			TcpClient client = new TcpClient();

			await client.ConnectAsync(socketip, port)
				.ConfigureAwait(false);

			//TODO: Should we check if it's connected??
			return client;
		}

		//TODO: Add exception documentation
		/// <summary>
		/// 
		/// </summary>
		/// <exception cref=""></exception>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static Task<MemoryStream> openurl(String s)
		{
			if(string.IsNullOrEmpty(s)) throw new ArgumentException("Value cannot be null or empty.", nameof(s));

			throw new NotImplementedException($"TODO: Whenever 317refactor reimplements this in openJagGrabInputStream then implement");
			/*for(urlreq = s; urlreq != null;)
				try
				{
					Thread.sleep(50L);
				}
				catch(Exception _ex)
				{
				}
	
			if(urlstream == null)
				throw new IOException("could not open: " + s);
			else
				return urlstream;*/
		}

		public static void reporterror(String s)
		{
			if(shouldReportErrors)
				Console.WriteLine($"Error: {s}");
		}

		public static Task startpriv(IPAddress inetaddress)
		{
			if(IsSignLinkThreadActive)
				throw new InvalidOperationException($"Cannot call this method when thread is active.");

			savereq = null;
			urlreq = null;
			socketip = inetaddress;

			//TODO: This design is so shit, why would anyone do this.
			return Task.Factory.StartNew(async () => { new signlink().run(); }, TaskCreationOptions.LongRunning);
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public static void startThread(IRunnable runnable, int priority)
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(s => runnable.run()));

			/*Thread newThread = new Thread(new ThreadStart(runnable.run));
			newThread.IsBackground = true;
			newThread.Priority = priority;
			newThread.Start();*/
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public static bool wavereplay()
		{
			if(savereq != null)
			{
				return false;
			}
			else
			{
				savebuf = null;
				waveplay = true;
				savereq = "sound" + wavepos + ".wav";
				return true;
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public static bool wavesave(byte[] abyte0, int i)
		{
			if(i > 0x1e8480)
				return false;
			if(savereq != null)
			{
				return false;
			}
			else
			{
				wavepos = (wavepos + 1) % 5;
				savelen = i;
				savebuf = abyte0;
				waveplay = true;
				savereq = "sound" + wavepos + ".wav";
				return true;
			}
		}

		public static int clientversion = 317;

		public static int uid;

		public static int storeid = 32;
		public static FileStream cache_dat = null;
		public static FileStream[] cache_idx = new FileStream[5];
		public static bool sunjava;
		public static Object applet = null;
		public static bool IsSignLinkThreadActive { get; private set; }
		private static int threadliveid;
		private static IPAddress socketip;
		private static int socketreq;
		private static TcpClient socket = null;
		public static String dns = null;
		private static String urlreq = null;
		private static int savelen;
		private static String savereq = null;
		private static byte[] savebuf = null;
		private static bool midiplay;
		private static int midipos;
		public static String midi = null;
		public static int midiVolume;
		public static int midiFade;
		private static bool waveplay;
		private static int wavepos;
		public static int wavevol;
		public static bool shouldReportErrors = true;
		public static String errorname = "";

		private signlink()
		{
		}

		public void run()
		{
			String cacheDirectoryPath;
			try
			{
				cacheDirectoryPath = findcachedir();
				uid = getuid(cacheDirectoryPath);

				if(File.Exists($"{cacheDirectoryPath}main_file_cache.dat"))
				{
					//Just like RS2Sharp we skip the part where
					//it checks if the cache is too large.
					/*if(file.exists() && file.length() > 0x3200000L)
						file.delete();*/
				}

				cache_dat = new FileStream($"{cacheDirectoryPath}main_file_cache.dat", FileMode.Open);
				for(int j = 0; j < 5; j++)
					cache_idx[j] = new FileStream($"{cacheDirectoryPath}main_file_cache.idx{j}", FileMode.Open);

			}
			catch(Exception exception)
			{
				signlink.reporterror($"Failed to load cache. Reason: {exception.Message} \n StackTrack: {exception.StackTrace}");
				throw new InvalidOperationException($"Failed to load cache. Reason: {exception.Message} \n StackTrack: {exception.StackTrace}", exception);
			}
			finally
			{
				//Always indicate we are running, even if we're about to fail.
				IsSignLinkThreadActive = true;
			}
		}
	}
}
