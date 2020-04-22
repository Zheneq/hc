using System;
using System.Collections.Generic;

[Serializable]
public class LobbyPlayerCommonInfo
{
	public long AccountId;

	public int PlayerId;

	public int CustomGameVisualSlot;

	public string Handle;

	public int TitleID;

	public int TitleLevel;

	public int BannerID;

	public int EmblemID;

	public int RibbonID;

	public bool IsGameOwner;

	public bool IsReplayGenerator;

	public BotDifficulty Difficulty;

	public bool BotCanTaunt;

	public Team TeamId;

	public LobbyCharacterInfo CharacterInfo = new LobbyCharacterInfo();

	public List<LobbyCharacterInfo> RemoteCharacterInfos = new List<LobbyCharacterInfo>();

	public ReadyState ReadyState;

	public int ControllingPlayerId;

	public LobbyServerPlayerInfo ControllingPlayerInfo;

	public PlayerGameAccountType GameAccountType;

	public PlayerGameConnectionType GameConnectionType;

	public PlayerGameOptionFlag GameOptionFlags;

	public bool IsRemoteControlled => ControllingPlayerInfo != null;

	public bool IsSpectator => TeamId == Team.Spectator;

	public CharacterType CharacterType
	{
		get
		{
			int result;
			if (CharacterInfo == null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				result = 0;
			}
			else
			{
				result = (int)CharacterInfo.CharacterType;
			}
			return (CharacterType)result;
		}
	}

	public bool IsReady
	{
		get
		{
			int result;
			if (ReadyState != ReadyState.Ready)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!IsAIControlled)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					result = (IsRemoteControlled ? 1 : 0);
					goto IL_003b;
				}
			}
			result = 1;
			goto IL_003b;
			IL_003b:
			return (byte)result != 0;
		}
	}

	public bool ReplacedWithBots
	{
		get;
		set;
	}

	public bool IsAIControlled
	{
		get
		{
			int result;
			if (!IsNPCBot)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!IsLoadTestBot)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					result = (ReplacedWithBots ? 1 : 0);
					goto IL_003a;
				}
			}
			result = 1;
			goto IL_003a;
			IL_003a:
			return (byte)result != 0;
		}
	}

	public bool IsHumanControlled => !IsAIControlled;

	public bool IsNPCBot
	{
		get
		{
			return GameAccountType == PlayerGameAccountType.None;
		}
		set
		{
			if (!value)
			{
				return;
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
				GameAccountType = PlayerGameAccountType.None;
				return;
			}
		}
	}

	public bool IsLoadTestBot
	{
		get
		{
			return GameAccountType == PlayerGameAccountType.LoadTest;
		}
		set
		{
			if (value)
			{
				GameAccountType = PlayerGameAccountType.LoadTest;
			}
		}
	}

	public void SetGameOption(LobbyGameplayOverrides gameplayOverrides)
	{
		if (!IsLoadTestBot)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (gameplayOverrides.UseFakeGameServersForLoadTests)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						GameConnectionType = PlayerGameConnectionType.None;
						return;
					}
				}
			}
			if (gameplayOverrides.UseFakeClientConnectionsForLoadTests)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						GameConnectionType = PlayerGameConnectionType.None;
						return;
					}
				}
			}
			GameConnectionType = PlayerGameConnectionType.RawSocket;
			return;
		}
	}

	public void SetGameOption(PlayerGameOptionFlag flag, bool on)
	{
		if (on)
		{
			GameOptionFlags = GameOptionFlags.WithGameOption(flag);
		}
		else
		{
			GameOptionFlags = GameOptionFlags.WithoutGameOption(flag);
		}
	}
}
