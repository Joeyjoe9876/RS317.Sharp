using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class OpcodeEncryptingBufferWriterDecorator : IBufferWriteable
	{
		private IBufferWriteable DecoratedBufferWriter { get; }

		private ISAACRandomGenerator Encryptor { get; }

		public OpcodeEncryptingBufferWriterDecorator(IBufferWriteable decoratedBufferWriter, ISAACRandomGenerator encryptor)
		{
			DecoratedBufferWriter = decoratedBufferWriter ?? throw new ArgumentNullException(nameof(decoratedBufferWriter));
			Encryptor = encryptor ?? throw new ArgumentNullException(nameof(encryptor));
		}
		public byte[] buffer => DecoratedBufferWriter.buffer;

		public int position
		{
			get => DecoratedBufferWriter.position;
			set => DecoratedBufferWriter.position = value;
		}

		public void generateKeys()
		{
			DecoratedBufferWriter.generateKeys();
		}

		public void put(int i)
		{
			DecoratedBufferWriter.put(i);
		}

		public void put24BitInt(int i)
		{
			DecoratedBufferWriter.put24BitInt(i);
		}

		public void putByteC(int i)
		{
			DecoratedBufferWriter.putByteC(i);
		}

		public void putBytes(byte[] buf, int length, int startPosition)
		{
			DecoratedBufferWriter.putBytes(buf, length, startPosition);
		}

		public void putByteS(int j)
		{
			DecoratedBufferWriter.putByteS(j);
		}

		public void putBytesA(int i, byte[] buf, int j)
		{
			DecoratedBufferWriter.putBytesA(i, buf, j);
		}

		public void putInt(int i)
		{
			DecoratedBufferWriter.putInt(i);
		}

		public void putLEInt(int j)
		{
			DecoratedBufferWriter.putLEInt(j);
		}

		public void putLEShort(int i)
		{
			DecoratedBufferWriter.putLEShort(i);
		}

		public void putLEShortA(int j)
		{
			DecoratedBufferWriter.putLEShortA(j);
		}

		public void putLong(long l)
		{
			DecoratedBufferWriter.putLong(l);
		}

		public void putOpcode(int i)
		{
			DecoratedBufferWriter.putOpcode((byte)(i + Encryptor.value()));
		}

		public void putShort(int i)
		{
			DecoratedBufferWriter.putShort(i);
		}

		public void putShortA(int j)
		{
			DecoratedBufferWriter.putShortA(j);
		}

		public void putSizeByte(int i)
		{
			DecoratedBufferWriter.putSizeByte(i);
		}

		public void putString(string s)
		{
			DecoratedBufferWriter.putString(s);
		}
	}
}
