using Newtonsoft.Json;
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

	[JsonIgnore]
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

	[JsonIgnore]
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

	public PlayerDetails(PlayerGameAccountType gameAccountType)
	{
		m_disconnected = false;
		m_gameObjects = new List<GameObject>();
		m_idleTurns = 0;
		m_gameAccountType = gameAccountType;
	}

	internal bool IsLocal()
	{
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

	internal void OnSerializeHelper(NetworkWriter stream)
	{
		OnSerializeHelper(new NetworkWriterAdapter(stream));
	}

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

	public override string ToString()
	{
		return $"{m_handle}[{m_accountId}] team={m_team} disconnected={m_disconnected} replacedWithBots={ReplacedWithBots}";
	}
}
