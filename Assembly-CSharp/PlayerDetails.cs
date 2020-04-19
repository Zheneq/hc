using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDetails
{
	public Team m_team;

	public bool m_disconnected;

	public string m_handle;

	public long m_accountId;

	public float m_accPrivateElo;

	public float m_charPrivateElo;

	public float m_usedMatchmakingElo;

	public int m_lobbyPlayerInfoId;

	public PlayerGameAccountType m_gameAccountType;

	public bool m_replayGenerator;

	public bool m_botsMasqueradeAsHumans;

	public string m_buildVersion;

	public List<GameObject> m_gameObjects;

	public int m_idleTurns;

	public PlayerDetails(PlayerGameAccountType gameAccountType)
	{
		this.m_disconnected = false;
		this.m_gameObjects = new List<GameObject>();
		this.m_idleTurns = 0;
		this.m_gameAccountType = gameAccountType;
	}

	public bool ReplacedWithBots { get; private set; }

	public bool IsAIControlled
	{
		get
		{
			bool result;
			if (!this.IsNPCBot && !this.IsLoadTestBot)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerDetails.get_IsAIControlled()).MethodHandle;
				}
				result = this.ReplacedWithBots;
			}
			else
			{
				result = true;
			}
			return result;
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
			return this.m_gameAccountType == PlayerGameAccountType.None;
		}
		set
		{
			if (value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerDetails.set_IsNPCBot(bool)).MethodHandle;
				}
				this.m_gameAccountType = PlayerGameAccountType.None;
			}
		}
	}

	public bool IsLoadTestBot
	{
		get
		{
			return this.m_gameAccountType == PlayerGameAccountType.LoadTest;
		}
		set
		{
			if (value)
			{
				this.m_gameAccountType = PlayerGameAccountType.LoadTest;
			}
		}
	}

	public bool IsSpectator
	{
		get
		{
			return this.m_team == Team.Spectator;
		}
	}

	public bool IsConnected
	{
		get
		{
			return !this.m_disconnected;
		}
		private set
		{
		}
	}

	internal bool IsLocal()
	{
		if (ClientGameManager.Get())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerDetails.IsLocal()).MethodHandle;
			}
			if (ClientGameManager.Get().Observer)
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
				return this.m_replayGenerator;
			}
		}
		if (ReplayPlayManager.Get())
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
			if (ReplayPlayManager.Get().IsPlayback())
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
				return this.m_replayGenerator;
			}
		}
		bool result;
		if (this.m_accountId != 0L)
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
			result = (this.m_accountId == HydrogenConfig.Get().Ticket.AccountId);
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal void OnSerializeHelper(NetworkWriter stream)
	{
		this.OnSerializeHelper(new NetworkWriterAdapter(stream));
	}

	internal void OnSerializeHelper(IBitStream stream)
	{
		sbyte b = checked((sbyte)this.m_team);
		bool disconnected = this.m_disconnected;
		string handle = this.m_handle;
		long accountId = this.m_accountId;
		float accPrivateElo = this.m_accPrivateElo;
		float usedMatchmakingElo = this.m_usedMatchmakingElo;
		int lobbyPlayerInfoId = this.m_lobbyPlayerInfoId;
		float charPrivateElo = this.m_charPrivateElo;
		int gameAccountType = (int)this.m_gameAccountType;
		bool replayGenerator = this.m_replayGenerator;
		bool botsMasqueradeAsHumans = this.m_botsMasqueradeAsHumans;
		stream.Serialize(ref b);
		stream.Serialize(ref disconnected);
		stream.Serialize(ref handle);
		stream.Serialize(ref accountId);
		stream.Serialize(ref accPrivateElo);
		stream.Serialize(ref usedMatchmakingElo);
		stream.Serialize(ref lobbyPlayerInfoId);
		stream.Serialize(ref charPrivateElo);
		stream.Serialize(ref gameAccountType);
		stream.Serialize(ref replayGenerator);
		stream.Serialize(ref botsMasqueradeAsHumans);
		this.m_team = (Team)b;
		this.m_disconnected = disconnected;
		this.m_handle = handle;
		this.m_accountId = accountId;
		this.m_accPrivateElo = accPrivateElo;
		this.m_usedMatchmakingElo = usedMatchmakingElo;
		this.m_lobbyPlayerInfoId = lobbyPlayerInfoId;
		this.m_charPrivateElo = charPrivateElo;
		this.m_gameAccountType = (PlayerGameAccountType)gameAccountType;
		this.m_replayGenerator = replayGenerator;
		this.m_botsMasqueradeAsHumans = botsMasqueradeAsHumans;
	}

	public override string ToString()
	{
		return string.Format("{0}[{1}] team={2} disconnected={3} replacedWithBots={4}", new object[]
		{
			this.m_handle,
			this.m_accountId,
			this.m_team,
			this.m_disconnected,
			this.ReplacedWithBots
		});
	}
}
