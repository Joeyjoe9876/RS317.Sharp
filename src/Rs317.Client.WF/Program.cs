using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rs317.Sharp
{
	public static class Program
	{
		public static async Task Main(string[] args)
		{
			try
			{
				Console.WriteLine($"RS2 user client - release #{317}");

				args = new string[] { "0", "0", "highmem", "members", "0" };

				int localWorldId = int.Parse(args[0]);
				short portOffset = (short)int.Parse(args[1]);
				bool membersWorld;

				if(args[3] == "free")
					membersWorld = false;
				else if(args[3] == "members")
				{
					membersWorld = true;
				}
				else
				{
					Console.WriteLine("Usage: node-id, port-offset, [lowmem/highmem], [free/members], storeid");
					return;
				}

				await StartClient(localWorldId, portOffset, membersWorld);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Erorr: {exception.Message} \n\n Stack: {exception.StackTrace}");
			}
		}

		private static async Task StartClient(int localWorldId, short portOffset, bool membersWorld)
		{
			Application.SetCompatibleTextRenderingDefault(false);

			Task clientRunningAwaitable = signlink.startpriv(IPAddress.Parse("127.0.0.1"));
			ClientConfiguration configuration = new ClientConfiguration(localWorldId, portOffset, membersWorld);

			//Wait for signlink
			while (!signlink.IsSignLinkThreadActive)
				await Task.Delay(50)
					.ConfigureAwait(false);
			RsWinForm windowsFormApplication = new RsWinForm(765, 503);
			RsWinFormsClient client1 = new RsWinFormsClient(configuration, windowsFormApplication.CreateGraphics());
			windowsFormApplication.RegisterInputSubscriber(client1);
			client1.createClientFrame(765, 503);

			Application.Run(windowsFormApplication);

			await clientRunningAwaitable
				.ConfigureAwait(false);
		}
	}
}
