// ROGUES
// SERVER
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Samurai_SyncComponent : NetworkBehaviour
{
	[SyncVar]
	internal int m_lastSelfBuffTurn = -1;
	[SyncVar]
	internal int m_selfBuffIncomingHitsThisTurn;
	internal BoardSquare m_afterimagePosition;
	[SyncVar]
	internal int m_afterimageX = -1;
	[SyncVar]
	internal int m_afterimageY = -1;
	internal bool m_swordBuffVfxPending;
	internal bool m_swordBuffFinalTurnVfxPending;

	private AbilityData m_abilityData;
	private SamuraiSelfBuff m_selfBuffAbility;
	
#if SERVER
	// added in rogues
	internal Effect m_afterimageVfxEffect;
#endif

	public int Networkm_lastSelfBuffTurn
	{
		get => m_lastSelfBuffTurn;
		[param: In]
		set => SetSyncVar(value, ref m_lastSelfBuffTurn, 1u);
	}

	public int Networkm_selfBuffIncomingHitsThisTurn
	{
		get => m_selfBuffIncomingHitsThisTurn;
		[param: In]
		set => SetSyncVar(value, ref m_selfBuffIncomingHitsThisTurn, 2u);
	}

	public int Networkm_afterimageX
	{
		get => m_afterimageX;
		[param: In]
		set => SetSyncVar(value, ref m_afterimageX, 4u);
	}

	public int Networkm_afterimageY
	{
		get => m_afterimageY;
		[param: In]
		set => SetSyncVar(value, ref m_afterimageY, 8u);
	}

	private void Start()
	{
		m_abilityData = GetComponent<AbilityData>();
		m_selfBuffAbility = m_abilityData.GetAbilityOfType(typeof(SamuraiSelfBuff)) as SamuraiSelfBuff;
	}

	public bool IsSelfBuffActive(ref int damageIncrease)
	{
		if (m_selfBuffAbility == null)
		{
			return false;
		}
		int currentTurn = GameFlowData.Get().CurrentTurn;
		StandardActorEffectData effectData = m_selfBuffAbility.GetSelfBuffEffect().m_effectData;
		bool isActiveImmediately = effectData.m_perTurnHitDelayTurns <= 0;
		bool isStillActive = m_selfBuffAbility.m_selfBuffLastsUntilYouDealDamage
		             || m_lastSelfBuffTurn > currentTurn - effectData.m_duration;
		if (m_abilityData.HasQueuedAbilityOfType(typeof(SamuraiSelfBuff)) && isActiveImmediately  // HasQueuedAbilityOfType(typeof(SamuraiSelfBuff), true) in rogues
		    || m_lastSelfBuffTurn >= 0 && isStillActive && currentTurn > m_lastSelfBuffTurn)  //  && currentTurn > m_lastSelfBuffTurn removed in rogues
		{
			float damageBuff = 0f;
			if (!effectData.m_statMods.IsNullOrEmpty())
			{
				damageBuff = effectData.m_statMods[0].modValue;
			}
			if (m_selfBuffIncomingHitsThisTurn > 0)
			{
				damageBuff += m_selfBuffAbility.GetDamageIncreaseFirstHit();
				damageBuff += m_selfBuffAbility.GetDamageIncreaseSubseqHits() * (m_selfBuffIncomingHitsThisTurn - 1);
			}
			damageIncrease = Mathf.RoundToInt(damageBuff);
			return true;
		}
		return false;
	}

	public int GetExtraDamageFromQueuedSelfBuff()  // NOTE CHANGE private in reactor
	{
		return m_abilityData != null
		       && m_selfBuffAbility != null
		       && m_selfBuffAbility.GetExtraDamageIfQueued() > 0
		       && m_abilityData.HasQueuedAbilityOfType(typeof(SamuraiSelfBuff)) // , true in rogues
			? m_selfBuffAbility.GetExtraDamageIfQueued()
			: 0;
	}

	// missing in rogues
	public int CalcExtraDamageFromSelfBuffAbility()
	{
		int damageIncrease = 0;
		IsSelfBuffActive(ref damageIncrease);
		return damageIncrease + GetExtraDamageFromQueuedSelfBuff();
	}

	private void UNetVersion()  // MirrorProcessed in rogues
	{
	}

	// changed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)m_lastSelfBuffTurn);
			writer.WritePackedUInt32((uint)m_selfBuffIncomingHitsThisTurn);
			writer.WritePackedUInt32((uint)m_afterimageX);
			writer.WritePackedUInt32((uint)m_afterimageY);
			return true;
		}
		bool flag = false;
		if ((syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_lastSelfBuffTurn);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_selfBuffIncomingHitsThisTurn);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_afterimageX);
		}
		if ((syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_afterimageY);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	// changed in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			m_lastSelfBuffTurn = (int)reader.ReadPackedUInt32();
			m_selfBuffIncomingHitsThisTurn = (int)reader.ReadPackedUInt32();
			m_afterimageX = (int)reader.ReadPackedUInt32();
			m_afterimageY = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_lastSelfBuffTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			m_selfBuffIncomingHitsThisTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			m_afterimageX = (int)reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
		{
			m_afterimageY = (int)reader.ReadPackedUInt32();
		}
	}
}
