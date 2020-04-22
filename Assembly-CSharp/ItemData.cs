using UnityEngine;
using UnityEngine.Networking;

public class ItemData : NetworkBehaviour
{
	private int m_credits;

	private int m_creditsSpent;

	private ActorData m_actorData;

	public int credits => m_credits;

	private void Awake()
	{
		m_actorData = GetComponent<ActorData>();
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		if (!initialState)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		if (!initialState)
		{
			if (base.syncVarDirtyBits == 0)
			{
				return false;
			}
		}
		OnSerializeHelper(new NetworkWriterAdapter(writer));
		return true;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint num = uint.MaxValue;
		if (!initialState)
		{
			num = reader.ReadPackedUInt32();
		}
		if (num == 0)
		{
			return;
		}
		while (true)
		{
			OnSerializeHelper(new NetworkReaderAdapter(reader));
			return;
		}
	}

	private void OnSerializeHelper(IBitStream stream)
	{
		if (NetworkServer.active)
		{
			if (stream.isReading)
			{
				while (true)
				{
					switch (6)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		int value = 0;
		int value2 = 0;
		if (stream.isWriting)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					value = m_credits;
					value2 = m_creditsSpent;
					stream.Serialize(ref value);
					stream.Serialize(ref value2);
					return;
				}
			}
		}
		stream.Serialize(ref value);
		stream.Serialize(ref value2);
		m_credits = value;
		m_creditsSpent = value2;
	}

	[Server]
	internal void OnTurnStart()
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Server] function 'System.Void ItemData::OnTurnStart()' called on client");
					return;
				}
			}
		}
		if (GameFlowData.Get().CurrentTurn == 1)
		{
			m_credits = GameplayData.Get().m_startingCredits;
		}
		else
		{
			ActorStats component = GetComponent<ActorStats>();
			int modifiedStatInt = component.GetModifiedStatInt(StatType.CreditsPerTurn);
			GiveCredits(modifiedStatInt);
		}
		SetDirtyBit(1u);
	}

	[Server]
	internal void GiveCredits(int numCredits)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Server] function 'System.Void ItemData::GiveCredits(System.Int32)' called on client");
					return;
				}
			}
		}
		m_credits += numCredits;
		SetDirtyBit(1u);
	}

	[Server]
	internal void SpendCredits(int numCredits)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Server] function 'System.Void ItemData::SpendCredits(System.Int32)' called on client");
					return;
				}
			}
		}
		if (Debug.isDebugBuild)
		{
			if (numCredits > m_credits)
			{
				ActorData actorData = m_actorData;
				Log.Error("Spending " + numCredits + " credits from actor " + actorData.DisplayName + "  but they only have " + m_credits + " credits.");
			}
		}
		m_credits -= numCredits;
		m_creditsSpent += numCredits;
		SetDirtyBit(1u);
	}

	public int GetNetWorth()
	{
		return m_credits + m_creditsSpent;
	}

	private void UNetVersion()
	{
	}
}
