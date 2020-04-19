using System;
using UnityEngine;
using UnityEngine.Networking;

public class ItemData : NetworkBehaviour
{
	private int m_credits;

	private int m_creditsSpent;

	private ActorData m_actorData;

	public int credits
	{
		get
		{
			return this.m_credits;
		}
	}

	private void Awake()
	{
		this.m_actorData = base.GetComponent<ActorData>();
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		if (!initialState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ItemData.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		if (!initialState)
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
			if (base.syncVarDirtyBits == 0U)
			{
				return false;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.OnSerializeHelper(new NetworkWriterAdapter(writer));
		return true;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint num = uint.MaxValue;
		if (!initialState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ItemData.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			num = reader.ReadPackedUInt32();
		}
		if (num != 0U)
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
			this.OnSerializeHelper(new NetworkReaderAdapter(reader));
		}
	}

	private void OnSerializeHelper(IBitStream stream)
	{
		if (NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ItemData.OnSerializeHelper(IBitStream)).MethodHandle;
			}
			if (stream.isReading)
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
				return;
			}
		}
		int credits = 0;
		int creditsSpent = 0;
		if (stream.isWriting)
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
			credits = this.m_credits;
			creditsSpent = this.m_creditsSpent;
			stream.Serialize(ref credits);
			stream.Serialize(ref creditsSpent);
		}
		else
		{
			stream.Serialize(ref credits);
			stream.Serialize(ref creditsSpent);
			this.m_credits = credits;
			this.m_creditsSpent = creditsSpent;
		}
	}

	[Server]
	internal void OnTurnStart()
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ItemData.OnTurnStart()).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void ItemData::OnTurnStart()' called on client");
			return;
		}
		if (GameFlowData.Get().CurrentTurn == 1)
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
			this.m_credits = GameplayData.Get().m_startingCredits;
		}
		else
		{
			ActorStats component = base.GetComponent<ActorStats>();
			int modifiedStatInt = component.GetModifiedStatInt(StatType.CreditsPerTurn);
			this.GiveCredits(modifiedStatInt);
		}
		base.SetDirtyBit(1U);
	}

	[Server]
	internal void GiveCredits(int numCredits)
	{
		if (!NetworkServer.active)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ItemData.GiveCredits(int)).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void ItemData::GiveCredits(System.Int32)' called on client");
			return;
		}
		this.m_credits += numCredits;
		base.SetDirtyBit(1U);
	}

	[Server]
	internal void SpendCredits(int numCredits)
	{
		if (!NetworkServer.active)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ItemData.SpendCredits(int)).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void ItemData::SpendCredits(System.Int32)' called on client");
			return;
		}
		if (Debug.isDebugBuild)
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
			if (numCredits > this.m_credits)
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
				ActorData actorData = this.m_actorData;
				Log.Error(string.Concat(new object[]
				{
					"Spending ",
					numCredits,
					" credits from actor ",
					actorData.DisplayName,
					"  but they only have ",
					this.m_credits,
					" credits."
				}), new object[0]);
			}
		}
		this.m_credits -= numCredits;
		this.m_creditsSpent += numCredits;
		base.SetDirtyBit(1U);
	}

	public int GetNetWorth()
	{
		return this.m_credits + this.m_creditsSpent;
	}

	private void UNetVersion()
	{
	}
}
