using System;
using System.Threading.Tasks;
using Rs317.Sharp;

namespace Rs317.Extended
{
	public class ExtendedProgram : Program
	{
		public new static async Task Main(string[] args)
		{
			Console.WriteLine($"RS2 user client - release #{317} using Rs317.Sharp.Extended by Glader");

			try
			{
				ExtendedProgram program = new ExtendedProgram();
				await program.StartClient(0, 0, true);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Erorr: {exception.Message} \n\n Stack: {exception.StackTrace}");
			}
		}

		public override OpenTKClient CreateOpenTkClient(ClientConfiguration configuration, OpenTkImageProducerFactory imageProducerFactory)
		{
			return base.CreateOpenTkClient(configuration, imageProducerFactory);
		}
	}
}
