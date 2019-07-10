using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	/*int _skillId = inStream.getUnsignedByte();
	int _skillExp = inStream.getMESInt();
	int _skillLevel = inStream.getUnsignedByte();*/

	[WireDataContract]
	[GameServerPayload(RsServerNetworkOperationCode.SetSkillExperience)]
	public sealed class ServerSetSkillExperiencePayload : BaseGameServerPayload
	{
		//TODO: make this an enum.
		[WireMember(1)]
		public byte SkillId { get; private set; }

		[WireMember(2)]
		public int Experience { get; private set; }

		[WireMember(3)]
		public int Level { get; private set; }

		public ServerSetSkillExperiencePayload(byte skillId, int experience, int level)
		{
			SkillId = skillId;
			Experience = experience;
			Level = level;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		private ServerSetSkillExperiencePayload()
		{
			
		}
	}
}
