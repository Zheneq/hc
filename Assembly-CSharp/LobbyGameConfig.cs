using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

[Serializable]
public class LobbyGameConfig
{
	public const int MaxEquipPoints = 0xA;
	public string Map;
	public List<GameSubType> SubTypes;
	public GameType GameType;
	public GameOptionFlag GameOptionFlags;
	public bool IsActive;
	public float GameServerShutdownTime = 300f;
	public ushort InstanceSubTypeBit;
	public string RoomName;
	public int TeamAPlayers;
	public int TeamBPlayers;
	public int TeamABots;
	public int TeamBBots;
	public int Spectators;
	public int ResolveTimeoutLimit;

	public LobbyGameConfig()
	{
		Map = string.Empty;
		InstanceSubTypeBit = 0;
		TeamAPlayers = 4;
		TeamBPlayers = 4;
		TeamABots = 0;
		TeamBBots = 0;
		Spectators = 0;
		ResolveTimeoutLimit = 0xA0;
		GameOptionFlags = GameOptionFlag.None;
	}

	public LobbyGameConfig Clone()
	{
		LobbyGameConfig lobbyGameConfig = (LobbyGameConfig)MemberwiseClone();
		lobbyGameConfig.SubTypes = new List<GameSubType>();
		foreach (GameSubType gameSubType in SubTypes)
		{
			lobbyGameConfig.SubTypes.Add(gameSubType.Clone());
		}
		return lobbyGameConfig;
	}

	public bool HasGameOption(GameOptionFlag gameOptionFlag)
	{
		return GameOptionFlags.HasGameOption(gameOptionFlag);
	}

	public void SetGameOption(GameOptionFlag flag, bool on)
	{
		GameOptionFlags = on
			? GameOptionFlags.WithGameOption(flag)
			: GameOptionFlags.WithoutGameOption(flag);
	}

	[JsonIgnore]
	public bool NeedsPreSelectedFreelancer
	{
		get
		{
			return SubTypes.Exists(p => p.NeedsPreSelectedFreelancer);
		}
	}

	[JsonIgnore]
	public bool HasSelectedSubType
	{
		get
		{
			if (SubTypes.IsNullOrEmpty())
			{
				throw new Exception(new StringBuilder().Append("LobbyGameConfig for ").Append(GameType).Append(" has no SubTypes defined").ToString());
			}
			if (SubTypes.Count == 1)
			{
				return true;
			}
			if (InstanceSubTypeBit == 0)
			{
				return false;
			}
			return GetSubTypes(InstanceSubTypeBit).Count() == 1;
		}
	}

	[JsonIgnore]
	public GameSubType InstanceSubType
	{
		get { return GetSubType(InstanceSubTypeBit); }
	}

	public GameSubType GetSubType(ushort subtypeBit)
	{
		if (SubTypes.IsNullOrEmpty())
		{
			throw new Exception(new StringBuilder().Append("LobbyGameConfig for ").Append(GameType).Append(" has no SubTypes defined").ToString());
		}
		if (SubTypes.Count == 1)
		{
			return SubTypes.First();
		}
		if (subtypeBit == 0)
		{
			throw new Exception(new StringBuilder().Append("LobbyGameConfig instance created for a specific game of ").Append(GameType).Append(" ").Append("but no subtype chosen (there are ").Append(SubTypes.Count).Append(" subtypes)").ToString());
		}
		IEnumerable<GameSubType> subTypes = GetSubTypes(subtypeBit);
		if (subTypes.Count() > 1)
		{
			throw new Exception(new StringBuilder().Append("LobbyGameConfig instance created for a specific game of ").Append(GameType).Append(" ").Append("but multiple subtypes selected").ToString());
		}
		return subTypes.First();
	}

	[JsonIgnore]
	public bool DoAFKPlayersAbortPreLoadGames
	{
		get { return InstanceSubType.HasMod(GameSubType.SubTypeMods.AFKPlayersAbortPreLoadGame); }
	}

	[JsonIgnore]
	public bool DoesSubGameTypeBlockQueueMMRUpdate
	{
		get { return InstanceSubType.HasMod(GameSubType.SubTypeMods.BlockQueueMMRUpdate); }
	}

	[JsonIgnore]
	public bool DoesSubGameTypeOverrideFreelancerSelection
	{
		get { return InstanceSubType.HasMod(GameSubType.SubTypeMods.OverrideFreelancerSelection); }
	}

	[JsonIgnore]
	public bool DoesSubGameTypeAllowPlayingLockedCharacters
	{
		get { return InstanceSubType.HasMod(GameSubType.SubTypeMods.AllowPlayingLockedCharacters); }
	}

	[JsonIgnore]
	public double TurnTime
	{
		get
		{
			if (!SubTypes.IsNullOrEmpty()
			    && InstanceSubTypeBit != 0
			    && InstanceSubType.GameOverrides != null)
			{
				TimeSpan? turnTimeSpan = InstanceSubType.GameOverrides.TurnTimeSpan;
				if (turnTimeSpan != null)
				{
					return turnTimeSpan.Value.TotalSeconds;
				}
			}

			return 20.0;
		}
	}

	public IEnumerable<GameSubType> GetSubTypes(ushort subTypeMask)
	{
		ushort bit = 1;
		bool found = false;
		foreach (GameSubType gst in SubTypes)
		{
			if ((bit & subTypeMask) != 0)
			{
				found = true;
				yield return gst;
			}
			bit = (ushort)(bit << 1);
		}
		if (!found)
		{
			throw new Exception(new StringBuilder().Append("There is no subtype in ").Append(GameType).Append(" that matches mask ").AppendFormat("{0:X}", subTypeMask).ToString());
		}
	}

	[JsonIgnore]
	public int TotalPlayers
	{
		get { return TeamAPlayers + TeamBPlayers; }
	}

	[JsonIgnore]
	public int TotalBots
	{
		get { return TeamABots + TeamBBots; }
	}

	[JsonIgnore]
	public int TotalHumanPlayers
	{
		get { return TotalPlayers - TotalBots; }
	}

	[JsonIgnore]
	public int MaxGroupSize
	{
		get { return Math.Max(TeamAPlayers, TeamBPlayers); }
	}

	[JsonIgnore]
	public int TeamAHumanPlayers
	{
		get { return TeamAPlayers - TeamABots; }
	}

	[JsonIgnore]
	public int TeamBHumanPlayers
	{
		get { return TeamBPlayers - TeamBBots; }
	}

	public bool ApplyDisabledMaps(List<string> disabledMaps, LobbyGameConfig defaultGameConfig)
	{
		SubTypes = new List<GameSubType>();
		if (defaultGameConfig == null)
		{
			return false;
		}
		if (defaultGameConfig.SubTypes.IsNullOrEmpty())
		{
			IsActive = false;
			throw new Exception(new StringBuilder().Append("Why does the ").Append(GameType).Append(" json config have no sub-types defined?").ToString());
		}
		bool result = false;
		foreach (GameSubType gameSubType in defaultGameConfig.SubTypes)
		{
			GameSubType gameSubTypeClone = gameSubType.Clone();
			SubTypes.Add(gameSubTypeClone);
			foreach (GameMapConfig gameMapConfig in gameSubTypeClone.GameMapConfigs)
			{
				if (gameMapConfig.IsActive && disabledMaps.Contains(gameMapConfig.Map))
				{
					result = true;
					gameMapConfig.IsActive = false;
					Log.Notice(new StringBuilder().Append("Override disabling ").Append(gameMapConfig.Map).Append(" in ").Append(GameType).Append(" ").Append(gameSubType.GetNameAsPayload().Term).ToString());
				}
			}
		}
		return result;
	}

	public override string ToString()
	{
		string subType = HasSelectedSubType ? new StringBuilder().Append(" ").Append(InstanceSubType.GetNameAsPayload().Term).ToString() : string.Empty;
		return new StringBuilder().Append(GameType).Append(subType).Append(" ").Append(RoomName).Append(" ").Append(Map).ToString();
	}
}
