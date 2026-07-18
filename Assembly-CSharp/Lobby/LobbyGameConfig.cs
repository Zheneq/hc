using System;
using System.Collections.Generic;
using System.Linq;
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
				throw new Exception($"LobbyGameConfig for {GameType} has no SubTypes defined");
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
	public GameSubType InstanceSubType => GetSubType(InstanceSubTypeBit);

	public GameSubType GetSubType(ushort subtypeBit)
	{
		if (SubTypes.IsNullOrEmpty())
		{
			throw new Exception($"LobbyGameConfig for {GameType} has no SubTypes defined");
		}
		if (SubTypes.Count == 1)
		{
			return SubTypes.First();
		}
		if (subtypeBit == 0)
		{
			throw new Exception($"LobbyGameConfig instance created for a specific game of {GameType} " +
			                    $"but no subtype chosen (there are {SubTypes.Count} subtypes)");
		}
		IEnumerable<GameSubType> subTypes = GetSubTypes(subtypeBit);
		if (subTypes.Count() > 1)
		{
			throw new Exception($"LobbyGameConfig instance created for a specific game of {GameType} " +
			                    $"but multiple subtypes selected");
		}
		return subTypes.First();
	}

	[JsonIgnore]
	public bool DoAFKPlayersAbortPreLoadGames => InstanceSubType.HasMod(GameSubType.SubTypeMods.AFKPlayersAbortPreLoadGame);

	[JsonIgnore]
	public bool DoesSubGameTypeBlockQueueMMRUpdate => InstanceSubType.HasMod(GameSubType.SubTypeMods.BlockQueueMMRUpdate);

	[JsonIgnore]
	public bool DoesSubGameTypeOverrideFreelancerSelection => InstanceSubType.HasMod(GameSubType.SubTypeMods.OverrideFreelancerSelection);

	[JsonIgnore]
	public bool DoesSubGameTypeAllowPlayingLockedCharacters => InstanceSubType.HasMod(GameSubType.SubTypeMods.AllowPlayingLockedCharacters);

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
			throw new Exception($"There is no subtype in {GameType} that matches mask {subTypeMask:X}");
		}
	}

	[JsonIgnore]
	public int TotalPlayers => TeamAPlayers + TeamBPlayers;

	[JsonIgnore]
	public int TotalBots => TeamABots + TeamBBots;

	[JsonIgnore]
	public int TotalHumanPlayers => TotalPlayers - TotalBots;

	[JsonIgnore]
	public int MaxGroupSize => Math.Max(TeamAPlayers, TeamBPlayers);

	[JsonIgnore]
	public int TeamAHumanPlayers => TeamAPlayers - TeamABots;

	[JsonIgnore]
	public int TeamBHumanPlayers => TeamBPlayers - TeamBBots;

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
			throw new Exception($"Why does the {GameType} json config have no sub-types defined?");
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
					Log.Notice($"Override disabling {gameMapConfig.Map} in {GameType} {gameSubType.GetNameAsPayload().Term}");
				}
			}
		}
		return result;
	}

	public override string ToString()
	{
		string subType = HasSelectedSubType ? $" {InstanceSubType.GetNameAsPayload().Term}" : string.Empty;
		return $"{GameType}{subType} {RoomName} {Map}";
	}
}
