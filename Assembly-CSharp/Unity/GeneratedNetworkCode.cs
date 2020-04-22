using System.Runtime.InteropServices;
using UnityEngine.Networking;

namespace Unity
{
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public class GeneratedNetworkCode
	{
		public static void _ReadStructSyncListSparkTetherAgeInfo_None(NetworkReader reader, SyncListSparkTetherAgeInfo instance)
		{
			ushort num = reader.ReadUInt16();
			instance.Clear();
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				instance.AddInternal(instance.DeserializeItem(reader));
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}

		public static void _WriteStructSyncListSparkTetherAgeInfo_None(NetworkWriter writer, SyncListSparkTetherAgeInfo value)
		{
			ushort count = value.Count;
			writer.Write(count);
			for (ushort num = 0; num < count; num = (ushort)(num + 1))
			{
				value.SerializeItem(writer, value.GetItem(num));
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}

		public static void _ReadStructSyncListVisionProviderInfo_None(NetworkReader reader, SyncListVisionProviderInfo instance)
		{
			ushort num = reader.ReadUInt16();
			instance.Clear();
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				instance.AddInternal(instance.DeserializeItem(reader));
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}

		public static void _WriteStructSyncListVisionProviderInfo_None(NetworkWriter writer, SyncListVisionProviderInfo value)
		{
			ushort count = value.Count;
			writer.Write(count);
			for (ushort num = 0; num < count; num = (ushort)(num + 1))
			{
				value.SerializeItem(writer, value.GetItem(num));
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}

		public static void _ReadStructSyncListTempCoverInfo_None(NetworkReader reader, SyncListTempCoverInfo instance)
		{
			ushort num = reader.ReadUInt16();
			instance.Clear();
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				instance.AddInternal(instance.DeserializeItem(reader));
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}

		public static void _WriteStructSyncListTempCoverInfo_None(NetworkWriter writer, SyncListTempCoverInfo value)
		{
			ushort count = value.Count;
			writer.Write(count);
			for (ushort num = 0; num < count; num = (ushort)(num + 1))
			{
				value.SerializeItem(writer, value.GetItem(num));
			}
		}

		public static LocalizationArg_AbilityPing _ReadLocalizationArg_AbilityPing_None(NetworkReader reader)
		{
			LocalizationArg_AbilityPing localizationArg_AbilityPing = new LocalizationArg_AbilityPing();
			localizationArg_AbilityPing.m_characterType = (CharacterType)reader.ReadInt32();
			localizationArg_AbilityPing.m_abilityType = reader.ReadString();
			localizationArg_AbilityPing.m_abilityName = reader.ReadString();
			localizationArg_AbilityPing.m_isSelectable = reader.ReadBoolean();
			localizationArg_AbilityPing.m_remainingCooldown = (int)reader.ReadPackedUInt32();
			localizationArg_AbilityPing.m_isUlt = reader.ReadBoolean();
			localizationArg_AbilityPing.m_currentTechPoints = (int)reader.ReadPackedUInt32();
			localizationArg_AbilityPing.m_maxTechPoints = (int)reader.ReadPackedUInt32();
			return localizationArg_AbilityPing;
		}

		public static void _WriteLocalizationArg_AbilityPing_None(NetworkWriter writer, LocalizationArg_AbilityPing value)
		{
			writer.Write((int)value.m_characterType);
			writer.Write(value.m_abilityType);
			writer.Write(value.m_abilityName);
			writer.Write(value.m_isSelectable);
			writer.WritePackedUInt32((uint)value.m_remainingCooldown);
			writer.Write(value.m_isUlt);
			writer.WritePackedUInt32((uint)value.m_currentTechPoints);
			writer.WritePackedUInt32((uint)value.m_maxTechPoints);
		}

		public static GridPosProp _ReadGridPosProp_None(NetworkReader reader)
		{
			GridPosProp gridPosProp = new GridPosProp();
			gridPosProp.m_x = (int)reader.ReadPackedUInt32();
			gridPosProp.m_y = (int)reader.ReadPackedUInt32();
			gridPosProp.m_height = (int)reader.ReadPackedUInt32();
			return gridPosProp;
		}

		public static void _WriteGridPosProp_None(NetworkWriter writer, GridPosProp value)
		{
			writer.WritePackedUInt32((uint)value.m_x);
			writer.WritePackedUInt32((uint)value.m_y);
			writer.WritePackedUInt32((uint)value.m_height);
		}

		public static void _WriteMessage_Replay(NetworkWriter writer, Replay.Message value)
		{
			writer.Write(value.timestamp);
			writer.WriteBytesFull(value.data);
		}

		public static Replay.Message _ReadMessage_Replay(NetworkReader reader)
		{
			Replay.Message result = default(Replay.Message);
			result.timestamp = reader.ReadSingle();
			result.data = reader.ReadBytesAndSize();
			return result;
		}

		public static DisplayConsoleTextMessage _ReadDisplayConsoleTextMessage_None(NetworkReader reader)
		{
			DisplayConsoleTextMessage result = default(DisplayConsoleTextMessage);
			result.Term = reader.ReadString();
			result.Context = reader.ReadString();
			result.Token = reader.ReadString();
			result.Unlocalized = reader.ReadString();
			result.MessageType = (ConsoleMessageType)reader.ReadInt32();
			result.RestrictVisibiltyToTeam = (Team)reader.ReadInt32();
			result.SenderHandle = reader.ReadString();
			result.CharacterType = (CharacterType)reader.ReadInt32();
			return result;
		}

		public static void _WriteDisplayConsoleTextMessage_None(NetworkWriter writer, DisplayConsoleTextMessage value)
		{
			writer.Write(value.Term);
			writer.Write(value.Context);
			writer.Write(value.Token);
			writer.Write(value.Unlocalized);
			writer.Write((int)value.MessageType);
			writer.Write((int)value.RestrictVisibiltyToTeam);
			writer.Write(value.SenderHandle);
			writer.Write((int)value.CharacterType);
		}

		public static SinglePlayerScriptedChat _ReadSinglePlayerScriptedChat_None(NetworkReader reader)
		{
			SinglePlayerScriptedChat singlePlayerScriptedChat = new SinglePlayerScriptedChat();
			singlePlayerScriptedChat.m_text = reader.ReadString();
			singlePlayerScriptedChat.m_sender = (CharacterType)reader.ReadInt32();
			singlePlayerScriptedChat.m_displaySeconds = reader.ReadSingle();
			singlePlayerScriptedChat.m_audioEvent = reader.ReadString();
			return singlePlayerScriptedChat;
		}

		public static void _WriteSinglePlayerScriptedChat_None(NetworkWriter writer, SinglePlayerScriptedChat value)
		{
			writer.Write(value.m_text);
			writer.Write((int)value.m_sender);
			writer.Write(value.m_displaySeconds);
			writer.Write(value.m_audioEvent);
		}
	}
}
