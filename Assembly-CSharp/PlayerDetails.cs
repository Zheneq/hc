// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDetails
{
	public Team m_team;
	public bool m_disconnected;
	public string m_handle;
	public long m_accountId;
	// removed in rogues
	public float m_accPrivateElo;
	// removed in rogues
	public float m_charPrivateElo;
	// removed in rogues
	public float m_usedMatchmakingElo;
	public int m_lobbyPlayerInfoId;
	public PlayerGameAccountType m_gameAccountType;
	// removed in rogues
	public bool m_replayGenerator;
	public bool m_botsMasqueradeAsHumans;
	public string m_buildVersion;
	public List<GameObject> m_gameObjects;

	// server-only
#if SERVER
	internal ServerPlayerInfo m_serverPlayerInfo;
#endif

	public int m_idleTurns;

	public bool ReplacedWithBots { get; private set; }
	public bool IsAIControlled => IsNPCBot || IsLoadTestBot || ReplacedWithBots;
	public bool IsHumanControlled => !IsAIControlled;
	public bool IsNPCBot
	{
		get
		{
			return m_gameAccountType == PlayerGameAccountType.None;
		}
		set
		{
			if (value)
			{
				m_gameAccountType = PlayerGameAccountType.None;
			}
		}
	}

	public bool IsLoadTestBot
	{
		get
		{
			return m_gameAccountType == PlayerGameAccountType.LoadTest;
		}
		set
		{
			if (value)
			{
				m_gameAccountType = PlayerGameAccountType.LoadTest;
			}
		}
	}

	public bool IsSpectator => m_team == Team.Spectator;
	public bool IsConnected
	{
		get
		{
			return !m_disconnected;
		}
		private set
		{
		}
	}

	// added in rogues
#if SERVER
	public PlayerGameOptionFlag GameOptionFlags
	{
		get
		{
			if (m_serverPlayerInfo != null)
			{
				return m_serverPlayerInfo.LobbyPlayerInfo.GameOptionFlags;
			}
			return PlayerGameOptionFlag.None;
		}
	}
#endif

	public PlayerDetails(PlayerGameAccountType gameAccountType)
	{
		m_disconnected = false;
		m_gameObjects = new List<GameObject>();
		m_idleTurns = 0;
		m_gameAccountType = gameAccountType;
	}

	// server-only
#if SERVER
	public IEnumerable<ServerPlayerInfo> AllServerPlayerInfos
	{
		get
		{
			if (m_serverPlayerInfo != null)
			{
				return new ServerPlayerInfo[] { m_serverPlayerInfo }
					.Concat(m_serverPlayerInfo.ProxyPlayerInfos);
			}
			return Enumerable.Empty<ServerPlayerInfo>();
		}
	}
#endif

	internal bool IsLocal()
	{
		// rogues
		//return m_accountId != 0L && m_accountId == HydrogenConfig.Get().Ticket.AccountId;
		// reactor
		if (ClientGameManager.Get() != null && ClientGameManager.Get().Observer)
		{
			return m_replayGenerator;
		}
		if (ReplayPlayManager.Get() != null && ReplayPlayManager.Get().IsPlayback())
		{
			return m_replayGenerator;
		}
		return m_accountId != 0 && m_accountId == HydrogenConfig.Get().Ticket.AccountId;
	}

	// removed in rogues
	internal void OnSerializeHelper(NetworkWriter stream)
	{
		OnSerializeHelper(new NetworkWriterAdapter(stream));
	}

	// removed in rogues
	internal void OnSerializeHelper(IBitStream stream)
	{
		sbyte team = checked((sbyte)m_team);
		bool disconnected = m_disconnected;
		string handle = m_handle;
		long accountId = m_accountId;
		float accPrivateElo = m_accPrivateElo;
		float usedMatchmakingElo = m_usedMatchmakingElo;
		int lobbyPlayerInfoId = m_lobbyPlayerInfoId;
		float charPrivateElo = m_charPrivateElo;
		int gameAccountType = (int)m_gameAccountType;
		bool replayGenerator = m_replayGenerator;
		bool botsMasqueradeAsHumans = m_botsMasqueradeAsHumans;
		stream.Serialize(ref team);
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
		m_team = (Team)team;
		m_disconnected = disconnected;
		m_handle = handle;
		m_accountId = accountId;
		m_accPrivateElo = accPrivateElo;
		m_usedMatchmakingElo = usedMatchmakingElo;
		m_lobbyPlayerInfoId = lobbyPlayerInfoId;
		m_charPrivateElo = charPrivateElo;
		m_gameAccountType = (PlayerGameAccountType)gameAccountType;
		m_replayGenerator = replayGenerator;
		m_botsMasqueradeAsHumans = botsMasqueradeAsHumans;
	}

	// added in rogues
	//internal void OnSerialize(NetworkWriter writer)
	//{
	//	sbyte b = checked((sbyte)this.m_team);
	//	bool disconnected = this.m_disconnected;
	//	string handle = this.m_handle;
	//	long accountId = this.m_accountId;
	//	int lobbyPlayerInfoId = this.m_lobbyPlayerInfoId;
	//	int gameAccountType = (int)this.m_gameAccountType;
	//	bool botsMasqueradeAsHumans = this.m_botsMasqueradeAsHumans;
	//	writer.Write(b);
	//	writer.Write(disconnected);
	//	writer.Write(handle);
	//	writer.Write(accountId);
	//	writer.Write(lobbyPlayerInfoId);
	//	writer.Write(gameAccountType);
	//	writer.Write(botsMasqueradeAsHumans);
	//}

	// added in rogues
	//internal void OnDeserialize(NetworkReader reader)
	//{
	//	this.m_team = (Team)reader.ReadSByte();
	//	this.m_disconnected = reader.ReadBoolean();
	//	this.m_handle = reader.ReadString();
	//	this.m_accountId = reader.ReadInt64();
	//	this.m_lobbyPlayerInfoId = reader.ReadInt32();
	//	this.m_gameAccountType = (PlayerGameAccountType)reader.ReadInt32();
	//	this.m_botsMasqueradeAsHumans = reader.ReadBoolean();
	//}

	// server-only
#if SERVER
	internal void ReplaceWithBots()
	{
		ReplacedWithBots = true;
	}
#endif

	// server-only
#if SERVER
	internal void ReplaceWithHumans()
	{
		m_disconnected = false;
		ReplacedWithBots = false;
	}
#endif

	public override string ToString()
	{
		return $"{m_handle}[{m_accountId}] team={m_team} disconnected={m_disconnected} replacedWithBots={ReplacedWithBots}";
	}
}
