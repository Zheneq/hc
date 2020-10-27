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
        if (num != 0)
        {
            OnSerializeHelper(new NetworkReaderAdapter(reader));
		}
		LogJson(num);
	}

	private void LogJson(uint mask = System.UInt32.MaxValue)
	{
		var jsonLog = new System.Collections.Generic.List<string>();
		if (mask != 0)
		{
			jsonLog.Add($"\"credits\": {m_credits}, \"creditsSpent\": {m_creditsSpent}");
		}

		Log.Info($"[JSON] {{\"itemData\":{{{System.String.Join(",", jsonLog.ToArray())}}}}}");
	}

	private void OnSerializeHelper(IBitStream stream)
	{
        if (NetworkServer.active && stream.isReading)
        {
            return;
        }
        int _credits = 0;
		int _creditsSpent = 0;
		if (stream.isWriting)
		{
			_credits = m_credits;
			_creditsSpent = m_creditsSpent;
			stream.Serialize(ref _credits);
			stream.Serialize(ref _creditsSpent);
		}
		else
        {
			stream.Serialize(ref _credits);
			stream.Serialize(ref _creditsSpent);
			m_credits = _credits;
			m_creditsSpent = _creditsSpent;
		}
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
