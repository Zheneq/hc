// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;
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

#if SERVER
	// added in rogues
	private List<List<int>> m_endedEffectGuids;
	// added in rogues
	private List<List<int>> m_endedBarrierGuids;
#endif

	private void Awake()
	{
		m_endedEffectGuidsSync = new List<int>();
		m_endedBarrierGuidsSync = new List<int>();

		// added in rogues
#if SERVER
		m_endedEffectGuids = new List<List<int>>(m_numTurnsInMemory);
		m_endedBarrierGuids = new List<List<int>>(m_numTurnsInMemory);
		for (int i = 0; i < m_numTurnsInMemory; i++)
		{
			m_endedEffectGuids.Add(new List<int>());
			m_endedBarrierGuids.Add(new List<int>());
		}
#endif
	}

	private void SetDirtyBit(DirtyBit bit)
	{
		SetDirtyBit((uint)bit); // ulong in rogues
	}

	private bool IsBitDirty(uint setBits, DirtyBit bitToTest) // ulong in rogues
	{
		return ((int)setBits & (int)bitToTest) != 0; // ulong in rogues
	}

	private void OnEndedEffectGuidsSync()
	{
		if (m_endedEffectGuidsSync.Count > 100)
		{
			Log.Error("Remembering more than 100 effects?");
		}
		if (ClientEffectBarrierManager.Get() != null)
		{
			foreach (int effectGuid in m_endedEffectGuidsSync)
			{
				ClientEffectBarrierManager.Get().EndEffect(effectGuid);
			}
		}
	}

	private void OnEndedBarrierGuidsSync()
	{
		if (m_endedBarrierGuidsSync.Count > 50)
		{
			Log.Error("Remembering more than 50 barriers?");
		}
		if (ClientEffectBarrierManager.Get() != null)
		{
			foreach (int effectGuid in m_endedBarrierGuidsSync)
			{
				ClientEffectBarrierManager.Get().EndBarrier(effectGuid);
			}
		}
	}

	// added in rogues
#if SERVER
	public void NotifyEffectEnded(int effectGUID)
	{
		m_endedEffectGuids[0].Add(effectGUID);
		m_endedEffectGuidsSync.Add(effectGUID);
		SetDirtyBit(DirtyBit.EndedEffects);
		OnEndedEffectGuidsSync();
	}
#endif

	// added in rogues
#if SERVER
	public void NotifyBarrierEnded(int barrierGUID)
	{
		m_endedBarrierGuids[0].Add(barrierGUID);
		m_endedBarrierGuidsSync.Add(barrierGUID);
		SetDirtyBit(DirtyBit.EndedBarriers);
		OnEndedBarrierGuidsSync();
	}
#endif

	// added in rogues
#if SERVER
	public void OnTurnStart()
	{
		int num = m_numTurnsInMemory - 1;
		List<int> list = m_endedEffectGuids[num];
		List<int> list2 = m_endedBarrierGuids[num];
		foreach (int item in list)
		{
			m_endedEffectGuidsSync.Remove(item);
		}
		foreach (int item2 in list2)
		{
			m_endedBarrierGuidsSync.Remove(item2);
		}
		for (int i = num; i >= 1; i--)
		{
			m_endedEffectGuids[i] = m_endedEffectGuids[i - 1];
			m_endedBarrierGuids[i] = m_endedBarrierGuids[i - 1];
		}
		m_endedEffectGuids[0] = new List<int>();
		m_endedBarrierGuids[0] = new List<int>();
		OnEndedEffectGuidsSync();
		OnEndedBarrierGuidsSync();
	}
#endif

	// added in rogues
#if SERVER
	private void LogEndedEffectMemmory(string prefix)
	{
		string text = "";
		for (int i = 0; i < m_endedEffectGuids.Count; i++)
		{
			text = string.Concat(new object[]
			{
				text,
				"Group at index ",
				i,
				", count: ",
				m_endedEffectGuids[i].Count,
				"\n"
			});
			for (int j = 0; j < m_endedEffectGuids[i].Count; j++)
			{
				text = string.Concat(new object[]
				{
					text,
					"\tID + ",
					m_endedEffectGuids[i][j],
					"\n"
				});
			}
		}
		text = string.Concat(new object[]
		{
			text,
			"Num of effects to sync: ",
			m_endedEffectGuidsSync.Count,
			"\n"
		});
		Debug.LogWarning(prefix + "\t" + text);
	}
#endif

	// reactor
	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		if (!initialState)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		uint bitMask = (uint)(initialState ? -1 : (int)base.syncVarDirtyBits);
		if (IsBitDirty(bitMask, DirtyBit.EndedEffects))
		{
			short effectNum = (short)m_endedEffectGuidsSync.Count;
			writer.Write(effectNum);
			foreach (int effectGuid in m_endedEffectGuidsSync)
			{
				writer.Write(effectGuid);
			}
		}
		if (IsBitDirty(bitMask, DirtyBit.EndedBarriers))
		{
			short effectNum = (short)m_endedBarrierGuidsSync.Count;
			writer.Write(effectNum);
			foreach (int effectGuid in m_endedBarrierGuidsSync)
			{
				writer.Write(effectGuid);
			}
		}
		return bitMask != 0;
	}
	// rogues
	//public override bool OnSerialize(NetworkWriter writer, bool initialState)
	//{
	//	if (!initialState)
	//	{
	//		writer.WritePackedUInt64(base.syncVarDirtyBits);
	//	}
	//	ulong num = initialState ? ulong.MaxValue : base.syncVarDirtyBits;
	//	if (IsBitDirty(num, DirtyBit.EndedEffects))
	//	{
	//		short num2 = (short)m_endedEffectGuidsSync.Count;
	//		writer.Write(num2);
	//		foreach (int num3 in m_endedEffectGuidsSync)
	//		{
	//			writer.Write(num3);
	//		}
	//	}
	//	if (IsBitDirty(num, DirtyBit.EndedBarriers))
	//	{
	//		short num4 = (short)m_endedBarrierGuidsSync.Count;
	//		writer.Write(num4);
	//		foreach (int num5 in m_endedBarrierGuidsSync)
	//		{
	//			writer.Write(num5);
	//		}
	//	}
	//	return num > 0UL;
	//}

	// reactor
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint setBits = uint.MaxValue;
		if (!initialState)
		{
			setBits = reader.ReadPackedUInt32();
		}
		if (IsBitDirty(setBits, DirtyBit.EndedEffects))
		{
			m_endedEffectGuidsSync.Clear();
			short effectNum = reader.ReadInt16();
			for (short i = 0; i < effectNum; i++)
			{
				int effectGuid = reader.ReadInt32();
				m_endedEffectGuidsSync.Add(effectGuid);
			}
		}
		if (IsBitDirty(setBits, DirtyBit.EndedBarriers))
		{
			m_endedBarrierGuidsSync.Clear();
			short effectNum = reader.ReadInt16();
			for (short i = 0; i < effectNum; i++)
			{
				int effectGuid = reader.ReadInt32();
				m_endedBarrierGuidsSync.Add(effectGuid);
			}
		}
		OnEndedEffectGuidsSync();
		OnEndedBarrierGuidsSync();
	}
	// rogues
	//public override void OnDeserialize(NetworkReader reader, bool initialState)
	//{
	//	ulong setBits = ulong.MaxValue;
	//	if (!initialState)
	//	{
	//		setBits = reader.ReadPackedUInt64();
	//	}
	//	if (IsBitDirty(setBits, DirtyBit.EndedEffects))
	//	{
	//		m_endedEffectGuidsSync.Clear();
	//		short num = reader.ReadInt16();
	//		for (short num2 = 0; num2 < num; num2 += 1)
	//		{
	//			int item = reader.ReadInt32();
	//			m_endedEffectGuidsSync.Add(item);
	//		}
	//	}
	//	if (IsBitDirty(setBits, DirtyBit.EndedBarriers))
	//	{
	//		m_endedBarrierGuidsSync.Clear();
	//		short num3 = reader.ReadInt16();
	//		for (short num4 = 0; num4 < num3; num4 += 1)
	//		{
	//			int item2 = reader.ReadInt32();
	//			m_endedBarrierGuidsSync.Add(item2);
	//		}
	//	}
	//	OnEndedEffectGuidsSync();
	//	OnEndedBarrierGuidsSync();
	//}

	// reactor
	private void UNetVersion()
	{
	}
	// added in rogues
	//private void MirrorProcessed()
	//{
	//}
}
