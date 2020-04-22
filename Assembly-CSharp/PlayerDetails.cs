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

	public bool ReplacedWithBots
	{
		get;
		private set;
	}

	public bool IsAIControlled
	{
		get
		{
			int result;
			if (!IsNPCBot && !IsLoadTestBot)
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
				result = (ReplacedWithBots ? 1 : 0);
			}
			else
			{
				result = 1;
			}
			return (byte)result != 0;
		}
	}

	public bool IsHumanControlled => !IsAIControlled;

	public bool IsNPCBot
	{
		get
		{
			return m_gameAccountType == PlayerGameAccountType.None;
		}
		set
		{
			if (!value)
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_gameAccountType = PlayerGameAccountType.None;
				return;
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

	public PlayerDetails(PlayerGameAccountType gameAccountType)
	{
		m_disconnected = false;
		m_gameObjects = new List<GameObject>();
		m_idleTurns = 0;
		m_gameAccountType = gameAccountType;
	}

	internal bool IsLocal()
	{
		if ((bool)ClientGameManager.Get())
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
			if (ClientGameManager.Get().Observer)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return m_replayGenerator;
					}
				}
			}
		}
		if ((bool)ReplayPlayManager.Get())
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
			if (ReplayPlayManager.Get().IsPlayback())
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return m_replayGenerator;
					}
				}
			}
		}
		int result;
		if (m_accountId != 0)
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
			result = ((m_accountId == HydrogenConfig.Get().Ticket.AccountId) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	internal void OnSerializeHelper(NetworkWriter stream)
	{
		OnSerializeHelper(new NetworkWriterAdapter(stream));
	}

	internal void OnSerializeHelper(IBitStream stream)
	{
		sbyte value = checked((sbyte)m_team);
		bool value2 = m_disconnected;
		string value3 = m_handle;
		long value4 = m_accountId;
		float value5 = m_accPrivateElo;
		float value6 = m_usedMatchmakingElo;
		int value7 = m_lobbyPlayerInfoId;
		float value8 = m_charPrivateElo;
		int value9 = (int)m_gameAccountType;
		bool value10 = m_replayGenerator;
		bool value11 = m_botsMasqueradeAsHumans;
		stream.Serialize(ref value);
		stream.Serialize(ref value2);
		stream.Serialize(ref value3);
		stream.Serialize(ref value4);
		stream.Serialize(ref value5);
		stream.Serialize(ref value6);
		stream.Serialize(ref value7);
		stream.Serialize(ref value8);
		stream.Serialize(ref value9);
		stream.Serialize(ref value10);
		stream.Serialize(ref value11);
		m_team = (Team)value;
		m_disconnected = value2;
		m_handle = value3;
		m_accountId = value4;
		m_accPrivateElo = value5;
		m_usedMatchmakingElo = value6;
		m_lobbyPlayerInfoId = value7;
		m_charPrivateElo = value8;
		m_gameAccountType = (PlayerGameAccountType)value9;
		m_replayGenerator = value10;
		m_botsMasqueradeAsHumans = value11;
	}

	public override string ToString()
	{
		return $"{m_handle}[{m_accountId}] team={m_team} disconnected={m_disconnected} replacedWithBots={ReplacedWithBots}";
	}
}
