using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;

public struct Player
{
	private static byte s_nextId;

	public bool m_valid;

	public byte m_id;

	public int m_connectionId;

	[JsonIgnore]
	public long m_accountId;

	[CompilerGenerated]
	private static Action<GameState> _003C_003Ef__mg_0024cache0;

	//[CompilerGenerated]
	//private static Action<GameState> OnGameStateChanged;

	[JsonIgnore]
	public bool WasEverHuman => m_accountId > 0;

	internal Player(NetworkConnection connection, long accountId)
	{
		m_valid = true;
		m_id = checked(s_nextId++);
		int connectionId;
		if (connection == null)
		{
			connectionId = -1;
		}
		else
		{
			connectionId = connection.connectionId;
		}
		m_connectionId = connectionId;
		m_accountId = accountId;
		GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
		
		GameFlowData.s_onGameStateChanged += OnGameStateChanged;
	}

	public override bool Equals(object obj)
	{
		int result;
		if (obj is Player)
		{
			result = ((this == (Player)obj) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override int GetHashCode()
	{
		return m_id.GetHashCode();
	}

	public static bool operator ==(Player x, Player y)
	{
		return x.m_id == y.m_id && x.m_valid == y.m_valid;
	}

	public static bool operator !=(Player x, Player y)
	{
		return !(x == y);
	}

	internal void OnSerializeHelper(NetworkWriter stream)
	{
		OnSerializeHelper(new NetworkWriterAdapter(stream));
	}

	internal void OnSerializeHelper(IBitStream stream)
	{
		bool _valid = m_valid;
		byte _id = m_id;
		sbyte _connectionId = checked((sbyte)m_connectionId);
		stream.Serialize(ref _valid);
		stream.Serialize(ref _id);
		stream.Serialize(ref _connectionId);
		m_valid = _valid;
		m_id = _id;
		m_connectionId = _connectionId;
	}

	private static void OnGameStateChanged(GameState newState)
	{
		if (newState != GameState.EndingGame)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		s_nextId = 0;
	}

	public override string ToString()
	{
		return $"[Player: id={m_id}, connectionId={m_connectionId}, accountId={m_accountId}]";
	}
}
