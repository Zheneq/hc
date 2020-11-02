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
		LogJson(setBits);
		OnEndedEffectGuidsSync();
		OnEndedBarrierGuidsSync();
	}

	private void LogJson(uint setBits = uint.MaxValue)
	{
		var jsonLog = new List<string>();
		if (IsBitDirty(setBits, DirtyBit.EndedEffects))
		{
			jsonLog.Add($"\"endedEffectGuidsSync\":{DefaultJsonSerializer.Serialize(m_endedEffectGuidsSync)}");
		}
		if (IsBitDirty(setBits, DirtyBit.EndedBarriers))
		{
			jsonLog.Add($"\"endedBarrierGuidsSync\":{DefaultJsonSerializer.Serialize(m_endedBarrierGuidsSync)}");
		}

		Log.Info($"[JSON] {{\"sharedEffectBarrierManager\":{{{System.String.Join(",", jsonLog.ToArray())}}}}}");
	}

	private void UNetVersion()
	{
	}
}
