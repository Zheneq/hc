﻿using System;
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

	private void Start()
	{
		this.m_abilityData = base.GetComponent<AbilityData>();
		this.m_selfBuffAbility = (this.m_abilityData.GetAbilityOfType(typeof(SamuraiSelfBuff)) as SamuraiSelfBuff);
	}

	public unsafe bool IsSelfBuffActive(ref int damageIncrease)
	{
		if (this.m_selfBuffAbility != null)
		{
			int currentTurn = GameFlowData.Get().CurrentTurn;
			StandardActorEffectData effectData = this.m_selfBuffAbility.GetSelfBuffEffect().m_effectData;
			bool flag = effectData.m_perTurnHitDelayTurns <= 0;
			bool flag2;
			if (!this.m_selfBuffAbility.m_selfBuffLastsUntilYouDealDamage)
			{
				flag2 = (this.m_lastSelfBuffTurn > currentTurn - effectData.m_duration);
			}
			else
			{
				flag2 = true;
			}
			bool flag3 = flag2;
			if (this.m_abilityData.HasQueuedAbilityOfType(typeof(SamuraiSelfBuff)))
			{
				if (flag)
				{
					goto IL_EA;
				}
			}
			if (this.m_lastSelfBuffTurn < 0)
			{
				return false;
			}
			if (!flag3)
			{
				return false;
			}
			if (currentTurn <= this.m_lastSelfBuffTurn)
			{
				return false;
			}
			IL_EA:
			float num = 0f;
			if (!effectData.m_statMods.IsNullOrEmpty<AbilityStatMod>())
			{
				num = effectData.m_statMods[0].modValue;
			}
			if (this.m_selfBuffIncomingHitsThisTurn > 0)
			{
				num += (float)this.m_selfBuffAbility.GetDamageIncreaseFirstHit();
				num += (float)(this.m_selfBuffAbility.GetDamageIncreaseSubseqHits() * (this.m_selfBuffIncomingHitsThisTurn - 1));
			}
			damageIncrease = Mathf.RoundToInt(num);
			return true;
		}
		return false;
	}

	private int GetExtraDamageFromQueuedSelfBuff()
	{
		if (this.m_abilityData != null)
		{
			if (this.m_selfBuffAbility != null && this.m_selfBuffAbility.GetExtraDamageIfQueued() > 0)
			{
				if (this.m_abilityData.HasQueuedAbilityOfType(typeof(SamuraiSelfBuff)))
				{
					return this.m_selfBuffAbility.GetExtraDamageIfQueued();
				}
			}
		}
		return 0;
	}

	public int CalcExtraDamageFromSelfBuffAbility()
	{
		int num = 0;
		this.IsSelfBuffActive(ref num);
		num += this.GetExtraDamageFromQueuedSelfBuff();
		return num;
	}

	private void UNetVersion()
	{
	}

	public int Networkm_lastSelfBuffTurn
	{
		get
		{
			return this.m_lastSelfBuffTurn;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_lastSelfBuffTurn, 1U);
		}
	}

	public int Networkm_selfBuffIncomingHitsThisTurn
	{
		get
		{
			return this.m_selfBuffIncomingHitsThisTurn;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_selfBuffIncomingHitsThisTurn, 2U);
		}
	}

	public int Networkm_afterimageX
	{
		get
		{
			return this.m_afterimageX;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_afterimageX, 4U);
		}
	}

	public int Networkm_afterimageY
	{
		get
		{
			return this.m_afterimageY;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_afterimageY, 8U);
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)this.m_lastSelfBuffTurn);
			writer.WritePackedUInt32((uint)this.m_selfBuffIncomingHitsThisTurn);
			writer.WritePackedUInt32((uint)this.m_afterimageX);
			writer.WritePackedUInt32((uint)this.m_afterimageY);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_lastSelfBuffTurn);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_selfBuffIncomingHitsThisTurn);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_afterimageX);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_afterimageY);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			this.m_lastSelfBuffTurn = (int)reader.ReadPackedUInt32();
			this.m_selfBuffIncomingHitsThisTurn = (int)reader.ReadPackedUInt32();
			this.m_afterimageX = (int)reader.ReadPackedUInt32();
			this.m_afterimageY = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.m_lastSelfBuffTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			this.m_selfBuffIncomingHitsThisTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			this.m_afterimageX = (int)reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
		{
			this.m_afterimageY = (int)reader.ReadPackedUInt32();
		}
	}
}
