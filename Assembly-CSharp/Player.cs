using System;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;

public struct Player
{
	private static byte s_nextId;

	public bool m_valid;

	public byte m_id;

	public int m_connectionId;

	public long m_accountId;

	[CompilerGenerated]
	private static Action<GameState> f__mg_cache0;

	[CompilerGenerated]
	private static Action<GameState> f__mg_cache1;

	internal Player(NetworkConnection connection, long accountId)
	{
		this.m_valid = true;
		byte b = Player.s_nextId;
		byte nextId = b;
		Player.s_nextId = checked(nextId++);
		this.m_id = b;
		int connectionId;
		if (connection == null)
		{
			connectionId = -1;
		}
		else
		{
			connectionId = connection.connectionId;
		}
		this.m_connectionId = connectionId;
		this.m_accountId = accountId;
		
		GameFlowData.s_onGameStateChanged -= new Action<GameState>(Player.OnGameStateChanged);
		
		GameFlowData.s_onGameStateChanged += new Action<GameState>(Player.OnGameStateChanged);
	}

	public bool WasEverHuman
	{
		get
		{
			return this.m_accountId > 0L;
		}
	}

	public override bool Equals(object obj)
	{
		bool result;
		if (obj is Player)
		{
			result = (this == (Player)obj);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override int GetHashCode()
	{
		return this.m_id.GetHashCode();
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
		this.OnSerializeHelper(new NetworkWriterAdapter(stream));
	}

	internal void OnSerializeHelper(IBitStream stream)
	{
		bool valid = this.m_valid;
		byte id = this.m_id;
		sbyte b = checked((sbyte)this.m_connectionId);
		stream.Serialize(ref valid);
		stream.Serialize(ref id);
		stream.Serialize(ref b);
		this.m_valid = valid;
		this.m_id = id;
		this.m_connectionId = (int)b;
	}

	private static void OnGameStateChanged(GameState newState)
	{
		if (newState != GameState.EndingGame)
		{
		}
		else
		{
			Player.s_nextId = 0;
		}
	}

	public override string ToString()
	{
		return string.Format("[Player: id={0}, connectionId={1}, accountId={2}]", this.m_id, this.m_connectionId, this.m_accountId);
	}
}
