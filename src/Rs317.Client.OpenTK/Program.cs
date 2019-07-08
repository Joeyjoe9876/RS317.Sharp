using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	class Program
	{
		public static async Task Main(string[] args)
		{
			try
			{
				Console.WriteLine($"RS2 user client - release #{317} using Rs317.Sharp");
				await StartClient(0, 0, true);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Erorr: {exception.Message} \n\n Stack: {exception.StackTrace}");
			}
		}

		private static async Task StartClient(int localWorldId, short portOffset, bool membersWorld)
		{
			Task clientRunningAwaitable = signlink.startpriv(IPAddress.Parse("127.0.0.1"));
			ClientConfiguration configuration = new ClientConfiguration(localWorldId, portOffset, membersWorld);

			//Wait for signlink
			while(!signlink.IsSignLinkThreadActive)
				await Task.Delay(50)
					.ConfigureAwait(false);

			OpenTKGameWindow gameWindow = new OpenTKGameWindow(765, 503);
			OpenTKClient client = new OpenTKClient(configuration, new OpenTKRsGraphicsContext());
			client.createClientFrame(765, 503);
			gameWindow.Run(60);
			//RsWinForm windowsFormApplication = new RsWinForm(765, 503);
			//RsWinFormsClient client1 = new RsWinFormsClient(configuration, windowsFormApplication.CreateGraphics());
			//windowsFormApplication.RegisterInputSubscriber(client1);
			//client1.createClientFrame(765, 503);

			await clientRunningAwaitable
				.ConfigureAwait(false);
		}
	}
}
