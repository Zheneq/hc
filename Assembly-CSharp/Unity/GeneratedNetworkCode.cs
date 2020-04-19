using System;
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
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				instance.AddInternal(instance.DeserializeItem(reader));
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GeneratedNetworkCode._ReadStructSyncListSparkTetherAgeInfo_None(NetworkReader, SyncListSparkTetherAgeInfo)).MethodHandle;
			}
		}

		public static void _WriteStructSyncListSparkTetherAgeInfo_None(NetworkWriter writer, SyncListSparkTetherAgeInfo value)
		{
			ushort count = value.Count;
			writer.Write(count);
			for (ushort num = 0; num < count; num += 1)
			{
				value.SerializeItem(writer, value.GetItem((int)num));
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GeneratedNetworkCode._WriteStructSyncListSparkTetherAgeInfo_None(NetworkWriter, SyncListSparkTetherAgeInfo)).MethodHandle;
			}
		}

		public static void _ReadStructSyncListVisionProviderInfo_None(NetworkReader reader, SyncListVisionProviderInfo instance)
		{
			ushort num = reader.ReadUInt16();
			instance.Clear();
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				instance.AddInternal(instance.DeserializeItem(reader));
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GeneratedNetworkCode._ReadStructSyncListVisionProviderInfo_None(NetworkReader, SyncListVisionProviderInfo)).MethodHandle;
			}
		}

		public static void _WriteStructSyncListVisionProviderInfo_None(NetworkWriter writer, SyncListVisionProviderInfo value)
		{
			ushort count = value.Count;
			writer.Write(count);
			for (ushort num = 0; num < count; num += 1)
			{
				value.SerializeItem(writer, value.GetItem((int)num));
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GeneratedNetworkCode._WriteStructSyncListVisionProviderInfo_None(NetworkWriter, SyncListVisionProviderInfo)).MethodHandle;
			}
		}

		public static void _ReadStructSyncListTempCoverInfo_None(NetworkReader reader, SyncListTempCoverInfo instance)
		{
			ushort num = reader.ReadUInt16();
			instance.Clear();
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				instance.AddInternal(instance.DeserializeItem(reader));
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GeneratedNetworkCode._ReadStructSyncListTempCoverInfo_None(NetworkReader, SyncListTempCoverInfo)).MethodHandle;
			}
		}

		public static void _WriteStructSyncListTempCoverInfo_None(NetworkWriter writer, SyncListTempCoverInfo value)
		{
			ushort count = value.Count;
			writer.Write(count);
			for (ushort num = 0; num < count; num += 1)
			{
				value.SerializeItem(writer, value.GetItem((int)num));
			}
		}

		public static LocalizationArg_AbilityPing _ReadLocalizationArg_AbilityPing_None(NetworkReader reader)
		{
			return new LocalizationArg_AbilityPing
			{
				m_characterType = (CharacterType)reader.ReadInt32(),
				m_abilityType = reader.ReadString(),
				m_abilityName = reader.ReadString(),
				m_isSelectable = reader.ReadBoolean(),
				m_remainingCooldown = (int)reader.ReadPackedUInt32(),
				m_isUlt = reader.ReadBoolean(),
				m_currentTechPoints = (int)reader.ReadPackedUInt32(),
				m_maxTechPoints = (int)reader.ReadPackedUInt32()
			};
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
			return new GridPosProp
			{
				m_x = (int)reader.ReadPackedUInt32(),
				m_y = (int)reader.ReadPackedUInt32(),
				m_height = (int)reader.ReadPackedUInt32()
			};
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
			return new Replay.Message
			{
				timestamp = reader.ReadSingle(),
				data = reader.ReadBytesAndSize()
			};
		}

		public static DisplayConsoleTextMessage _ReadDisplayConsoleTextMessage_None(NetworkReader reader)
		{
			return new DisplayConsoleTextMessage
			{
				Term = reader.ReadString(),
				Context = reader.ReadString(),
				Token = reader.ReadString(),
				Unlocalized = reader.ReadString(),
				MessageType = (ConsoleMessageType)reader.ReadInt32(),
				RestrictVisibiltyToTeam = (Team)reader.ReadInt32(),
				SenderHandle = reader.ReadString(),
				CharacterType = (CharacterType)reader.ReadInt32()
			};
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
			return new SinglePlayerScriptedChat
			{
				m_text = reader.ReadString(),
				m_sender = (CharacterType)reader.ReadInt32(),
				m_displaySeconds = reader.ReadSingle(),
				m_audioEvent = reader.ReadString()
			};
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
