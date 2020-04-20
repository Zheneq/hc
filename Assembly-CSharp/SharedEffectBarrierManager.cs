﻿using System;
using System.Collections.Generic;
using UnityEngine.Networking;

public class SharedEffectBarrierManager : NetworkBehaviour
{
	public int m_numTurnsInMemory = 3;

	private List<int> m_endedEffectGuidsSync;

	private List<int> m_endedBarrierGuidsSync;

	private void Awake()
	{
		this.m_endedEffectGuidsSync = new List<int>();
		this.m_endedBarrierGuidsSync = new List<int>();
	}

	private void SetDirtyBit(SharedEffectBarrierManager.DirtyBit bit)
	{
		base.SetDirtyBit((uint)bit);
	}

	private bool IsBitDirty(uint setBits, SharedEffectBarrierManager.DirtyBit bitToTest)
	{
		return (setBits & (uint)bitToTest) != 0U;
	}

	private void OnEndedEffectGuidsSync()
	{
		if (this.m_endedEffectGuidsSync.Count > 0x64)
		{
			Log.Error("Remembering more than 100 effects?", new object[0]);
		}
		if (ClientEffectBarrierManager.Get() != null)
		{
			for (int i = 0; i < this.m_endedEffectGuidsSync.Count; i++)
			{
				ClientEffectBarrierManager.Get().EndEffect(this.m_endedEffectGuidsSync[i]);
			}
		}
	}

	private void OnEndedBarrierGuidsSync()
	{
		if (this.m_endedBarrierGuidsSync.Count > 0x32)
		{
			Log.Error("Remembering more than 50 barriers?", new object[0]);
		}
		if (ClientEffectBarrierManager.Get() != null)
		{
			for (int i = 0; i < this.m_endedBarrierGuidsSync.Count; i++)
			{
				ClientEffectBarrierManager.Get().EndBarrier(this.m_endedBarrierGuidsSync[i]);
			}
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		if (!initialState)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		uint num;
		if (initialState)
		{
			num = uint.MaxValue;
		}
		else
		{
			num = base.syncVarDirtyBits;
		}
		uint num2 = num;
		if (this.IsBitDirty(num2, SharedEffectBarrierManager.DirtyBit.EndedEffects))
		{
			short value = (short)this.m_endedEffectGuidsSync.Count;
			writer.Write(value);
			using (List<int>.Enumerator enumerator = this.m_endedEffectGuidsSync.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int value2 = enumerator.Current;
					writer.Write(value2);
				}
			}
		}
		if (this.IsBitDirty(num2, SharedEffectBarrierManager.DirtyBit.EndedBarriers))
		{
			short value3 = (short)this.m_endedBarrierGuidsSync.Count;
			writer.Write(value3);
			foreach (int value4 in this.m_endedBarrierGuidsSync)
			{
				writer.Write(value4);
			}
		}
		return num2 != 0U;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint setBits = uint.MaxValue;
		if (!initialState)
		{
			setBits = reader.ReadPackedUInt32();
		}
		if (this.IsBitDirty(setBits, SharedEffectBarrierManager.DirtyBit.EndedEffects))
		{
			this.m_endedEffectGuidsSync.Clear();
			short num = reader.ReadInt16();
			for (short num2 = 0; num2 < num; num2 += 1)
			{
				int item = reader.ReadInt32();
				this.m_endedEffectGuidsSync.Add(item);
			}
		}
		if (this.IsBitDirty(setBits, SharedEffectBarrierManager.DirtyBit.EndedBarriers))
		{
			this.m_endedBarrierGuidsSync.Clear();
			short num3 = reader.ReadInt16();
			for (short num4 = 0; num4 < num3; num4 += 1)
			{
				int item2 = reader.ReadInt32();
				this.m_endedBarrierGuidsSync.Add(item2);
			}
		}
		this.OnEndedEffectGuidsSync();
		this.OnEndedBarrierGuidsSync();
	}

	private void UNetVersion()
	{
	}

	private enum DirtyBit : uint
	{
		EndedEffects = 1U,
		EndedBarriers,
		All = 0xFFFFFFFFU
	}
}
