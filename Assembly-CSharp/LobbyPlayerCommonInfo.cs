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

	public bool IsRemoteControlled
	{
		get
		{
			return this.ControllingPlayerInfo != null;
		}
	}

	public bool IsSpectator
	{
		get
		{
			return this.TeamId == Team.Spectator;
		}
	}

	public CharacterType CharacterType
	{
		get
		{
			CharacterType result;
			if (this.CharacterInfo == null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyPlayerCommonInfo.get_CharacterType()).MethodHandle;
				}
				result = CharacterType.None;
			}
			else
			{
				result = this.CharacterInfo.CharacterType;
			}
			return result;
		}
	}

	public bool IsReady
	{
		get
		{
			if (this.ReadyState != ReadyState.Ready)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyPlayerCommonInfo.get_IsReady()).MethodHandle;
				}
				if (!this.IsAIControlled)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					return this.IsRemoteControlled;
				}
			}
			return true;
		}
	}

	public bool ReplacedWithBots { get; set; }

	public bool IsAIControlled
	{
		get
		{
			if (!this.IsNPCBot)
			{
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyPlayerCommonInfo.get_IsAIControlled()).MethodHandle;
				}
				if (!this.IsLoadTestBot)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					return this.ReplacedWithBots;
				}
			}
			return true;
		}
	}

	public bool IsHumanControlled
	{
		get
		{
			return !this.IsAIControlled;
		}
	}

	public bool IsNPCBot
	{
		get
		{
			return this.GameAccountType == PlayerGameAccountType.None;
		}
		set
		{
			if (value)
			{
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyPlayerCommonInfo.set_IsNPCBot(bool)).MethodHandle;
				}
				this.GameAccountType = PlayerGameAccountType.None;
			}
		}
	}

	public bool IsLoadTestBot
	{
		get
		{
			return this.GameAccountType == PlayerGameAccountType.LoadTest;
		}
		set
		{
			if (value)
			{
				this.GameAccountType = PlayerGameAccountType.LoadTest;
			}
		}
	}

	public void SetGameOption(LobbyGameplayOverrides gameplayOverrides)
	{
		if (this.IsLoadTestBot)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyPlayerCommonInfo.SetGameOption(LobbyGameplayOverrides)).MethodHandle;
			}
			if (gameplayOverrides.UseFakeGameServersForLoadTests)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				this.GameConnectionType = PlayerGameConnectionType.None;
			}
			else if (gameplayOverrides.UseFakeClientConnectionsForLoadTests)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				this.GameConnectionType = PlayerGameConnectionType.None;
			}
			else
			{
				this.GameConnectionType = PlayerGameConnectionType.RawSocket;
			}
		}
	}

	public void SetGameOption(PlayerGameOptionFlag flag, bool on)
	{
		if (on)
		{
			this.GameOptionFlags = this.GameOptionFlags.WithGameOption(flag);
		}
		else
		{
			this.GameOptionFlags = this.GameOptionFlags.WithoutGameOption(flag);
		}
	}
}
