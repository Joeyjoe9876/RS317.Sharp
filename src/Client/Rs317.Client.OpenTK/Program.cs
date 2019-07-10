using System;
using System.Data;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			Console.WriteLine($"RS2 user client - release #{317} using Rs317.Sharp by Glader");

			try
			{
				Program program = new Program();
				await program.StartClient(0, 0, true);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Erorr: {exception.Message} \n\n Stack: {exception.StackTrace}");
			}
		}

		public async Task StartClient(int localWorldId, short portOffset, bool membersWorld)
		{
			Task clientRunningAwaitable = signlink.startpriv(IPAddress.Parse("127.0.0.1"));
			ClientConfiguration configuration = new ClientConfiguration(localWorldId, portOffset, membersWorld);

			//Wait for signlink
			while(!signlink.IsSignLinkThreadActive)
				await Task.Delay(50)
					.ConfigureAwait(false);

			OpenTkImageProducerFactory imageProducerFactory = new OpenTkImageProducerFactory();
			OpenTKClient client = CreateOpenTkClient(configuration, imageProducerFactory);

			using (OpenTKGameWindow gameWindow = new OpenTKGameWindow(765, 503, client, imageProducerFactory, client))
			{
				client.createClientFrame(765, 503);
				gameWindow.Run(20, 60);
			}

			await clientRunningAwaitable
				.ConfigureAwait(false);
		}

		public virtual OpenTKClient CreateOpenTkClient(ClientConfiguration configuration, OpenTkImageProducerFactory imageProducerFactory)
		{
			return new OpenTKClient(configuration, new OpenTKRsGraphicsContext(), imageProducerFactory, new DefaultBufferFactory());
		}
	}
}
