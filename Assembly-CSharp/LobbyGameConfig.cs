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
		this.Map = string.Empty;
		this.InstanceSubTypeBit = 0;
		this.TeamAPlayers = 4;
		this.TeamBPlayers = 4;
		this.TeamABots = 0;
		this.TeamBBots = 0;
		this.Spectators = 0;
		this.ResolveTimeoutLimit = 0xA0;
		this.GameOptionFlags = GameOptionFlag.None;
	}

	public LobbyGameConfig Clone()
	{
		LobbyGameConfig lobbyGameConfig = (LobbyGameConfig)base.MemberwiseClone();
		lobbyGameConfig.SubTypes = new List<GameSubType>();
		foreach (GameSubType gameSubType in this.SubTypes)
		{
			lobbyGameConfig.SubTypes.Add(gameSubType.Clone());
		}
		return lobbyGameConfig;
	}

	public bool HasGameOption(GameOptionFlag gameOptionFlag)
	{
		return this.GameOptionFlags.HasGameOption(gameOptionFlag);
	}

	public void SetGameOption(GameOptionFlag flag, bool on)
	{
		if (on)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyGameConfig.SetGameOption(GameOptionFlag, bool)).MethodHandle;
			}
			this.GameOptionFlags = this.GameOptionFlags.WithGameOption(flag);
		}
		else
		{
			this.GameOptionFlags = this.GameOptionFlags.WithoutGameOption(flag);
		}
	}

	[JsonIgnore]
	public bool NeedsPreSelectedFreelancer
	{
		get
		{
			List<GameSubType> subTypes = this.SubTypes;
			if (LobbyGameConfig.<>f__am$cache0 == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyGameConfig.get_NeedsPreSelectedFreelancer()).MethodHandle;
				}
				LobbyGameConfig.<>f__am$cache0 = ((GameSubType p) => p.NeedsPreSelectedFreelancer);
			}
			return subTypes.Exists(LobbyGameConfig.<>f__am$cache0);
		}
	}

	[JsonIgnore]
	public bool HasSelectedSubType
	{
		get
		{
			if (this.SubTypes.IsNullOrEmpty<GameSubType>())
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyGameConfig.get_HasSelectedSubType()).MethodHandle;
				}
				throw new Exception(string.Format("LobbyGameConfig for {0} has no SubTypes defined", this.GameType));
			}
			if (this.SubTypes.Count<GameSubType>() == 1)
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
				return true;
			}
			if (this.InstanceSubTypeBit == 0)
			{
				return false;
			}
			IEnumerable<GameSubType> subTypes = this.GetSubTypes(this.InstanceSubTypeBit);
			return subTypes.Count<GameSubType>() == 1;
		}
	}

	[JsonIgnore]
	public GameSubType InstanceSubType
	{
		get
		{
			return this.GetSubType(this.InstanceSubTypeBit);
		}
	}

	public GameSubType GetSubType(ushort subtypeBit)
	{
		if (this.SubTypes.IsNullOrEmpty<GameSubType>())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyGameConfig.GetSubType(ushort)).MethodHandle;
			}
			throw new Exception(string.Format("LobbyGameConfig for {0} has no SubTypes defined", this.GameType));
		}
		if (this.SubTypes.Count<GameSubType>() == 1)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			return this.SubTypes.First<GameSubType>();
		}
		if (subtypeBit == 0)
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
			throw new Exception(string.Format("LobbyGameConfig instance created for a specific game of {0} but no subtype chosen (there are {1} subtypes)", this.GameType, this.SubTypes.Count<GameSubType>()));
		}
		IEnumerable<GameSubType> subTypes = this.GetSubTypes(subtypeBit);
		if (subTypes.Count<GameSubType>() > 1)
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
			throw new Exception(string.Format("LobbyGameConfig instance created for a specific game of {0} but multiple subtypes selected", this.GameType));
		}
		return subTypes.First<GameSubType>();
	}

	[JsonIgnore]
	public bool DoAFKPlayersAbortPreLoadGames
	{
		get
		{
			return this.InstanceSubType.HasMod(GameSubType.SubTypeMods.AFKPlayersAbortPreLoadGame);
		}
	}

	[JsonIgnore]
	public bool DoesSubGameTypeBlockQueueMMRUpdate
	{
		get
		{
			return this.InstanceSubType.HasMod(GameSubType.SubTypeMods.BlockQueueMMRUpdate);
		}
	}

	[JsonIgnore]
	public bool DoesSubGameTypeOverrideFreelancerSelection
	{
		get
		{
			return this.InstanceSubType.HasMod(GameSubType.SubTypeMods.OverrideFreelancerSelection);
		}
	}

	[JsonIgnore]
	public bool DoesSubGameTypeAllowPlayingLockedCharacters
	{
		get
		{
			return this.InstanceSubType.HasMod(GameSubType.SubTypeMods.AllowPlayingLockedCharacters);
		}
	}

	[JsonIgnore]
	public double TurnTime
	{
		get
		{
			if (!this.SubTypes.IsNullOrEmpty<GameSubType>())
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyGameConfig.get_TurnTime()).MethodHandle;
				}
				if (this.InstanceSubTypeBit != 0)
				{
					if (this.InstanceSubType.GameOverrides != null)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						TimeSpan? turnTimeSpan = this.InstanceSubType.GameOverrides.TurnTimeSpan;
						if (turnTimeSpan != null)
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
							TimeSpan? turnTimeSpan2 = this.InstanceSubType.GameOverrides.TurnTimeSpan;
							return turnTimeSpan2.Value.TotalSeconds;
						}
					}
					return TimeSpan.FromSeconds(20.0).TotalSeconds;
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
			}
			return 20.0;
		}
	}

	public IEnumerable<GameSubType> GetSubTypes(ushort subTypeMask)
	{
		bool flag = false;
		uint num;
		ushort bit;
		bool bFoundSomething;
		List<GameSubType>.Enumerator enumerator;
		switch (num)
		{
		case 0U:
			bit = 1;
			bFoundSomething = false;
			enumerator = this.SubTypes.GetEnumerator();
			break;
		case 1U:
			break;
		default:
			yield break;
		}
		try
		{
			while (enumerator.MoveNext())
			{
				GameSubType gst = enumerator.Current;
				if ((bit & subTypeMask) != 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyGameConfig.<GetSubTypes>c__Iterator0.MoveNext()).MethodHandle;
					}
					bFoundSomething = true;
					yield return gst;
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = true;
				}
				bit = (ushort)(bit << 1);
			}
		}
		finally
		{
			if (flag)
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
			}
			else
			{
				((IDisposable)enumerator).Dispose();
			}
		}
		if (!bFoundSomething)
		{
			throw new Exception(string.Format("There is no subtype in {0} that matches mask {1:X}", this.GameType, subTypeMask));
		}
		yield break;
	}

	[JsonIgnore]
	public int TotalPlayers
	{
		get
		{
			return this.TeamAPlayers + this.TeamBPlayers;
		}
	}

	[JsonIgnore]
	public int TotalBots
	{
		get
		{
			return this.TeamABots + this.TeamBBots;
		}
	}

	[JsonIgnore]
	public int TotalHumanPlayers
	{
		get
		{
			return this.TotalPlayers - this.TotalBots;
		}
	}

	[JsonIgnore]
	public int MaxGroupSize
	{
		get
		{
			return Math.Max(this.TeamAPlayers, this.TeamBPlayers);
		}
	}

	[JsonIgnore]
	public int TeamAHumanPlayers
	{
		get
		{
			return this.TeamAPlayers - this.TeamABots;
		}
	}

	[JsonIgnore]
	public int TeamBHumanPlayers
	{
		get
		{
			return this.TeamBPlayers - this.TeamBBots;
		}
	}

	public bool ApplyDisabledMaps(List<string> disabledMaps, LobbyGameConfig defaultGameConfig)
	{
		bool result = false;
		this.SubTypes = new List<GameSubType>();
		if (defaultGameConfig != null)
		{
			if (defaultGameConfig.SubTypes.IsNullOrEmpty<GameSubType>())
			{
				this.IsActive = false;
				throw new Exception(string.Format("Why does the {0} json config have no sub-types defined?", this.GameType));
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyGameConfig.ApplyDisabledMaps(List<string>, LobbyGameConfig)).MethodHandle;
			}
			foreach (GameSubType gameSubType in defaultGameConfig.SubTypes)
			{
				GameSubType gameSubType2 = gameSubType.Clone();
				this.SubTypes.Add(gameSubType2);
				foreach (GameMapConfig gameMapConfig in gameSubType2.GameMapConfigs)
				{
					if (gameMapConfig.IsActive)
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
						if (disabledMaps.Contains(gameMapConfig.Map))
						{
							result = true;
							gameMapConfig.IsActive = false;
							Log.Notice("Override disabling {0} in {1} {2}", new object[]
							{
								gameMapConfig.Map,
								this.GameType,
								gameSubType.GetNameAsPayload().Term
							});
						}
					}
				}
			}
		}
		return result;
	}

	public override string ToString()
	{
		string text = (!this.HasSelectedSubType) ? string.Empty : string.Format(" {0}", this.InstanceSubType.GetNameAsPayload().Term);
		return string.Format("{0}{1} {2} {3}", new object[]
		{
			this.GameType,
			text,
			this.RoomName,
			this.Map
		});
	}
}
