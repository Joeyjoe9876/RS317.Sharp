
using System;

namespace Rs317.Sharp
{

	public sealed class VarBit
	{
		public static VarBit[] values;
		public int configId;
		public int leastSignificantBit;
		public int mostSignificantBit;

		public static void load(Archive archive)
		{
			Default317Buffer buffer = new Default317Buffer(archive.decompressFile("varbit.dat"));
			int count = buffer.getUnsignedLEShort();

			if(values == null)
			{
				values = new VarBit[count];
			}

			for(int i = 0; i < count; i++)
			{
				if(values[i] == null)
				{
					values[i] = new VarBit();
				}

				values[i].load(buffer);
			}

			if(buffer.position != buffer.buffer.Length)
				throw new InvalidOperationException("varbit load mismatch");

		}

		private void load(Default317Buffer buffer)
		{
			do
			{
				int opcode = buffer.getUnsignedByte();
				if(opcode == 0)
					return;
				if(opcode == 1)
				{
					configId = buffer.getUnsignedLEShort();
					leastSignificantBit = buffer.getUnsignedByte();
					mostSignificantBit = buffer.getUnsignedByte();
				}
				else if(opcode == 10)
					buffer.getString();
				else if(opcode == 2)
				{
				} // dummy
				else if(opcode == 3)
					buffer.getInt();
				else if(opcode == 4)
					buffer.getInt();
				else
					throw new InvalidOperationException($"Error unrecognised config code: {opcode}");
			} while(true);
		}
	}
}
