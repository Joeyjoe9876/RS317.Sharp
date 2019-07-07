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
				Application.SetCompatibleTextRenderingDefault(false);
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

				signlink.storeid = int.Parse(args[4]);
				Task clientRunningAwaitable = signlink.startpriv(IPAddress.Parse("127.0.0.1"));

				//Wait for signlink
				while (!signlink.IsSignLinkThreadActive)
					await Task.Delay(50)
						.ConfigureAwait(false);

				Client client1 = new Client(new ClientConfiguration(localWorldId, portOffset, membersWorld));
				client1.createClientFrame(765, 503);

				await clientRunningAwaitable
					.ConfigureAwait(false);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Erorr: {exception.Message} \n\n Stack: {exception.StackTrace}");
			}
		}
	}
}
