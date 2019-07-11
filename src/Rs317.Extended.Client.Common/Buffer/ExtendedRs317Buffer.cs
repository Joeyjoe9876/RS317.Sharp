using System;
using System.Collections.Generic;
using System.Text;
using Reinterpret.Net;
using Rs317.Sharp;

namespace Rs317.Extended
{
	/// <summary>
	/// The extended implementation of the Rs317 buffer.
	/// </summary>
	public sealed class ExtendedRs317Buffer : IBuffer
	{
		public byte[] buffer { get; }

		public int position { get; set; }

		public int bitPosition { get; }

		public ExtendedRs317Buffer(byte[] buffer)
		{
			if (buffer == null) throw new ArgumentNullException(nameof(buffer));

			this.buffer = buffer;
			bitPosition = 0;
			position = 0;
		}

		public void finishBitAccess()
		{
			return;
		}

		void IBufferReadable.generateKeys()
		{
			return;
		}

		public void put(int i)
		{
			buffer[position++] = (byte)i;
		}

		public void put24BitInt(int i)
		{
			//A 24bit int is stupid. Let's just use an int.
			putInt(i);
		}

		public void putByteC(int i)
		{
			//This specially handled byte stuff is super dumb.
			put(i);
		}

		public void putBytes(byte[] buf, int length, int startPosition)
		{
			System.Buffer.BlockCopy(buf, startPosition, buffer, position, length);
			position += length;
		}

		public void putByteS(int j)
		{
			//This specially handled byte stuff is super dumb.
			put(j);
		}

		//TODO: Rename base interface method.
		public void putBytesA(int startPosition, byte[] buf, int length)
		{
			putBytes(buf, length, startPosition);
		}

		public void putInt(int i)
		{
			i.Reinterpret(buffer, position);
			position += 4;
		}

		public void putLEInt(int j)
		{
			putInt(j);
		}

		public void putLEShort(int i)
		{
			//This is stupid, let's just use short.
			putShort(i);
		}

		public void putLEShortA(int j)
		{
			//This is stupid, let's just use short.
			putShort(j);
		}

		public void putLong(long l)
		{
			l.Reinterpret(buffer, position);
		}

		public void putOpcode(int i)
		{
			put(i);
		}

		public void putShort(int i)
		{
			((short) i).Reinterpret(buffer, position);
			position += 2;
		}

		public void putShortA(int j)
		{
			//This is stupid, let's just use short.
			putShort(j);
		}

		public void putSizeByte(int i)
		{
			buffer[position - i - 1] = (byte)i;
		}

		public void putString(string s)
		{
			//Let's send a short size, easier than null terminated
			putShort(s.Length);
			Encoding.ASCII.GetBytes(s, 0, s.Length, buffer, position);
			position += s.Length;
		}

		public byte get()
		{
			return buffer[position++];
		}

		public int get3Bytes()
		{
			//A 3 byte int is dumb, let's just read a normal int
			return getInt();
		}

		public byte getByteC()
		{
			return get();
		}

		public void getBytes(int startPos, int endPos, byte[] buf)
		{
			//What is this? It's riduclous....
			for(int k = (endPos + startPos) - 1; k >= endPos; k--)
				buf[k] = buffer[position++];
		}

		public byte getByteS()
		{
			return get();
		}

		public int getSignedLEShort()
		{
			return getShort();
		}

		public int getSignedLEShortA()
		{
			return getShort();
		}

		public int getInt()
		{
			position += 4;
			return buffer.Reinterpret<int>(position - 4);
		}

		public int getMEBInt()
		{
			//So dumb, let's just use int
			return getInt();
		}

		public int getMESInt()
		{
			//So dumb, let's just use int
			return getInt();
		}

		public long getLong()
		{
			position += 8;
			return buffer.Reinterpret<long>(position - 8);
		}

		public int getShort()
		{
			position += 2;
			return buffer.Reinterpret<short>(position - 2);
		}

		public int getSmartA()
		{
			//Absolutely ridiculous
			return getShort();
		}

		public int getSmartB()
		{
			//Absolutely ridiculous
			return getShort();
		}

		public string getString()
		{
			//First 2 bytes is size
			short size = (short) getShort();
			string resultingString = Encoding.ASCII.GetString(buffer, position, size);
			position += size;

			return resultingString;
		}

		public int getUnsignedByte()
		{
			return get();
		}

		public int getUnsignedByteA()
		{
			return get();
		}

		public int getUnsignedByteC()
		{
			return get();
		}

		public int getUnsignedByteS()
		{
			return get();
		}

		public int getUnsignedLEShort()
		{
			return getShort();
		}

		public int getUnsignedLEShortA()
		{
			return getShort();
		}

		public int getUnsignedShort()
		{
			return getShort();
		}

		public int getUnsignedShortA()
		{
			return getShort();
		}

		public void initBitAccess()
		{
			return;
		}

		public int readBits(int i)
		{
			if (i < 16)
				return get();
			else if (i >= 16 && i < 32)
				return getShort();
			else if (i >= 32 && i < 64)
				return getInt();

			throw new InvalidOperationException($"Failed to read bits size: {i}");
		}

		public byte[] readBytes()
		{
			throw new NotImplementedException($"Not implemented, only used in cache.");
		}

		public void readBytes(int length, int startPosition, byte[] dest)
		{
			System.Buffer.BlockCopy(buffer, position, dest, startPosition, length);
			position += length;
		}

		void IBufferWriteable.generateKeys()
		{
			return;
		}
	}
}
