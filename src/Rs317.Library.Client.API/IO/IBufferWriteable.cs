using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface IBufferWriteable : IBufferExposeable, IBufferSeekable
	{
		void generateKeys();

		void put(int i);

		void put24BitInt(int i);

		void putByteC(int i);

		void putBytes(byte[] buf, int length, int startPosition);

		void putByteS(int j);

		void putBytesA(int i, byte[] buf, int j);

		void putInt(int i);

		void putLEInt(int j);

		void putLEShort(int i);

		void putLEShortA(int j);

		void putLong(long l);

		void putOpcode(int i);

		void putShort(int i);

		void putShortA(int j);

		void putSizeByte(int i);

		void putString(String s);
	}
}
