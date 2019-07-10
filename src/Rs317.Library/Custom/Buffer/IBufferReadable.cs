using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface IBufferReadable : IBufferExposeable, IBufferSeekable
	{
		//Lowercase due to Java for now.
		int bitPosition { get; }

		void finishBitAccess();

		void generateKeys();

		byte get();

		int get3Bytes();

		byte getByteC();

		void getBytes(int startPos, int endPos, byte[] buf);

		byte getByteS();

		int getSignedLEShort();

		int getSignedLEShortA();

		int getInt();

		int getMEBInt();

		int getMESInt();

		long getLong();

		int getShort();

		int getSmartA();

		int getSmartB();

		String getString();

		int getUnsignedByte();

		int getUnsignedByteA();

		int getUnsignedByteC();

		int getUnsignedByteS();

		int getUnsignedLEShort();

		int getUnsignedLEShortA();

		int getUnsignedShort();

		int getUnsignedShortA();

		void initBitAccess();

		int readBits(int i);

		byte[] readBytes();

		void readBytes(int length, int startPosition, byte[] dest);
	}
}
