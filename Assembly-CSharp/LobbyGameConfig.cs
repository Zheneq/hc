using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LobbyGameConfig
{
	public const int MaxEquipPoints = 10;

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

	[JsonIgnore]
	public bool NeedsPreSelectedFreelancer
	{
		get
		{
			List<GameSubType> subTypes = SubTypes;
			if (_003C_003Ef__am_0024cache0 == null)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				_003C_003Ef__am_0024cache0 = ((GameSubType p) => p.NeedsPreSelectedFreelancer);
			}
			return subTypes.Exists(_003C_003Ef__am_0024cache0);
		}
	}

	[JsonIgnore]
	public bool HasSelectedSubType
	{
		get
		{
			if (SubTypes.IsNullOrEmpty())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						throw new Exception($"LobbyGameConfig for {GameType} has no SubTypes defined");
					}
				}
			}
			if (SubTypes.Count() == 1)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
			if (InstanceSubTypeBit == 0)
			{
				return false;
			}
			IEnumerable<GameSubType> subTypes = GetSubTypes(InstanceSubTypeBit);
			return subTypes.Count() == 1;
		}
	}

	[JsonIgnore]
	public GameSubType InstanceSubType => GetSubType(InstanceSubTypeBit);

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
			if (!SubTypes.IsNullOrEmpty())
			{
				while (true)
				{
					switch (2)
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
				if (InstanceSubTypeBit != 0)
				{
					if (InstanceSubType.GameOverrides != null)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						TimeSpan? turnTimeSpan = InstanceSubType.GameOverrides.TurnTimeSpan;
						if (turnTimeSpan.HasValue)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
								{
									TimeSpan? turnTimeSpan2 = InstanceSubType.GameOverrides.TurnTimeSpan;
									return turnTimeSpan2.Value.TotalSeconds;
								}
								}
							}
						}
					}
					return TimeSpan.FromSeconds(20.0).TotalSeconds;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			return 20.0;
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

	public LobbyGameConfig()
	{
		Map = string.Empty;
		InstanceSubTypeBit = 0;
		TeamAPlayers = 4;
		TeamBPlayers = 4;
		TeamABots = 0;
		TeamBBots = 0;
		Spectators = 0;
		ResolveTimeoutLimit = 160;
		GameOptionFlags = GameOptionFlag.None;
	}

	public LobbyGameConfig Clone()
	{
		LobbyGameConfig lobbyGameConfig = (LobbyGameConfig)MemberwiseClone();
		lobbyGameConfig.SubTypes = new List<GameSubType>();
		foreach (GameSubType subType in SubTypes)
		{
			lobbyGameConfig.SubTypes.Add(subType.Clone());
		}
		return lobbyGameConfig;
	}

	public bool HasGameOption(GameOptionFlag gameOptionFlag)
	{
		return GameOptionFlags.HasGameOption(gameOptionFlag);
	}

	public void SetGameOption(GameOptionFlag flag, bool on)
	{
		if (on)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					GameOptionFlags = GameOptionFlags.WithGameOption(flag);
					return;
				}
			}
		}
		GameOptionFlags = GameOptionFlags.WithoutGameOption(flag);
	}

	public GameSubType GetSubType(ushort subtypeBit)
	{
		if (SubTypes.IsNullOrEmpty())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					throw new Exception($"LobbyGameConfig for {GameType} has no SubTypes defined");
				}
			}
		}
		if (SubTypes.Count() == 1)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return SubTypes.First();
				}
			}
		}
		if (subtypeBit == 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					throw new Exception($"LobbyGameConfig instance created for a specific game of {GameType} but no subtype chosen (there are {SubTypes.Count()} subtypes)");
				}
			}
		}
		IEnumerable<GameSubType> subTypes = GetSubTypes(subtypeBit);
		if (subTypes.Count() > 1)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					throw new Exception($"LobbyGameConfig instance created for a specific game of {GameType} but multiple subtypes selected");
				}
			}
		}
		return subTypes.First();
	}

	public IEnumerable<GameSubType> GetSubTypes(ushort subTypeMask)
	{
		ushort bit = 1;
		bool bFoundSomething = false;
		foreach (GameSubType gst in SubTypes)
		{
			if ((bit & subTypeMask) != 0)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						bFoundSomething = true;
						yield return gst;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			bit = (ushort)(bit << 1);
		}
		if (!bFoundSomething)
		{
			throw new Exception($"There is no subtype in {GameType} that matches mask {subTypeMask:X}");
		}
	}

	public bool ApplyDisabledMaps(List<string> disabledMaps, LobbyGameConfig defaultGameConfig)
	{
		bool result = false;
		SubTypes = new List<GameSubType>();
		if (defaultGameConfig != null)
		{
			if (!defaultGameConfig.SubTypes.IsNullOrEmpty())
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						{
							foreach (GameSubType subType in defaultGameConfig.SubTypes)
							{
								GameSubType gameSubType = subType.Clone();
								SubTypes.Add(gameSubType);
								foreach (GameMapConfig gameMapConfig in gameSubType.GameMapConfigs)
								{
									if (gameMapConfig.IsActive)
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
										if (disabledMaps.Contains(gameMapConfig.Map))
										{
											result = true;
											gameMapConfig.IsActive = false;
											Log.Notice("Override disabling {0} in {1} {2}", gameMapConfig.Map, GameType, subType.GetNameAsPayload().Term);
										}
									}
								}
							}
							return result;
						}
					}
				}
			}
			IsActive = false;
			throw new Exception($"Why does the {GameType} json config have no sub-types defined?");
		}
		return result;
	}

	public override string ToString()
	{
		string text = (!HasSelectedSubType) ? string.Empty : $" {InstanceSubType.GetNameAsPayload().Term}";
		return $"{GameType}{text} {RoomName} {Map}";
	}
}
