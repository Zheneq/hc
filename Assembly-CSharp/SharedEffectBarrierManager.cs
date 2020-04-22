using System.Collections.Generic;
using UnityEngine.Networking;

public class SharedEffectBarrierManager : NetworkBehaviour
{
	private enum DirtyBit : uint
	{
		EndedEffects = 1u,
		EndedBarriers = 2u,
		All = uint.MaxValue
	}

	public int m_numTurnsInMemory = 3;

	private List<int> m_endedEffectGuidsSync;

	private List<int> m_endedBarrierGuidsSync;

	private void Awake()
	{
		m_endedEffectGuidsSync = new List<int>();
		m_endedBarrierGuidsSync = new List<int>();
	}

	private void SetDirtyBit(DirtyBit bit)
	{
		SetDirtyBit((uint)bit);
	}

	private bool IsBitDirty(uint setBits, DirtyBit bitToTest)
	{
		return ((int)setBits & (int)bitToTest) != 0;
	}

	private void OnEndedEffectGuidsSync()
	{
		if (m_endedEffectGuidsSync.Count > 100)
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
			Log.Error("Remembering more than 100 effects?");
		}
		if (!(ClientEffectBarrierManager.Get() != null))
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
			for (int i = 0; i < m_endedEffectGuidsSync.Count; i++)
			{
				ClientEffectBarrierManager.Get().EndEffect(m_endedEffectGuidsSync[i]);
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void OnEndedBarrierGuidsSync()
	{
		if (m_endedBarrierGuidsSync.Count > 50)
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
			Log.Error("Remembering more than 50 barriers?");
		}
		if (!(ClientEffectBarrierManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			for (int i = 0; i < m_endedBarrierGuidsSync.Count; i++)
			{
				ClientEffectBarrierManager.Get().EndBarrier(m_endedBarrierGuidsSync[i]);
			}
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

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		if (!initialState)
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
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		int num;
		if (initialState)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			num = -1;
		}
		else
		{
			num = (int)base.syncVarDirtyBits;
		}
		uint num2 = (uint)num;
		if (IsBitDirty(num2, DirtyBit.EndedEffects))
		{
			short value = (short)m_endedEffectGuidsSync.Count;
			writer.Write(value);
			using (List<int>.Enumerator enumerator = m_endedEffectGuidsSync.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.Current;
					writer.Write(current);
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		if (IsBitDirty(num2, DirtyBit.EndedBarriers))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			short value2 = (short)m_endedBarrierGuidsSync.Count;
			writer.Write(value2);
			foreach (int item in m_endedBarrierGuidsSync)
			{
				writer.Write(item);
			}
		}
		return num2 != 0;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint setBits = uint.MaxValue;
		if (!initialState)
		{
			while (true)
			{
				switch (1)
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
			setBits = reader.ReadPackedUInt32();
		}
		if (IsBitDirty(setBits, DirtyBit.EndedEffects))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			m_endedEffectGuidsSync.Clear();
			short num = reader.ReadInt16();
			for (short num2 = 0; num2 < num; num2 = (short)(num2 + 1))
			{
				int item = reader.ReadInt32();
				m_endedEffectGuidsSync.Add(item);
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (IsBitDirty(setBits, DirtyBit.EndedBarriers))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			m_endedBarrierGuidsSync.Clear();
			short num3 = reader.ReadInt16();
			for (short num4 = 0; num4 < num3; num4 = (short)(num4 + 1))
			{
				int item2 = reader.ReadInt32();
				m_endedBarrierGuidsSync.Add(item2);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		OnEndedEffectGuidsSync();
		OnEndedBarrierGuidsSync();
	}

	private void UNetVersion()
	{
	}
}
