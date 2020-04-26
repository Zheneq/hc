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

	public int Networkm_lastSelfBuffTurn
	{
		get
		{
			return m_lastSelfBuffTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_lastSelfBuffTurn, 1u);
		}
	}

	public int Networkm_selfBuffIncomingHitsThisTurn
	{
		get
		{
			return m_selfBuffIncomingHitsThisTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_selfBuffIncomingHitsThisTurn, 2u);
		}
	}

	public int Networkm_afterimageX
	{
		get
		{
			return m_afterimageX;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_afterimageX, 4u);
		}
	}

	public int Networkm_afterimageY
	{
		get
		{
			return m_afterimageY;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_afterimageY, 8u);
		}
	}

	private void Start()
	{
		m_abilityData = GetComponent<AbilityData>();
		m_selfBuffAbility = (m_abilityData.GetAbilityOfType(typeof(SamuraiSelfBuff)) as SamuraiSelfBuff);
	}

	public bool IsSelfBuffActive(ref int damageIncrease)
	{
		StandardActorEffectData effectData;
		if (m_selfBuffAbility != null)
		{
			int currentTurn = GameFlowData.Get().CurrentTurn;
			effectData = m_selfBuffAbility.GetSelfBuffEffect().m_effectData;
			bool flag = effectData.m_perTurnHitDelayTurns <= 0;
			int num;
			if (!m_selfBuffAbility.m_selfBuffLastsUntilYouDealDamage)
			{
				num = ((m_lastSelfBuffTurn > currentTurn - effectData.m_duration) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			bool flag2 = (byte)num != 0;
			if (m_abilityData.HasQueuedAbilityOfType(typeof(SamuraiSelfBuff)))
			{
				if (flag)
				{
					goto IL_00ea;
				}
			}
			if (m_lastSelfBuffTurn >= 0)
			{
				if (flag2)
				{
					if (currentTurn > m_lastSelfBuffTurn)
					{
						goto IL_00ea;
					}
				}
			}
		}
		return false;
		IL_00ea:
		float num2 = 0f;
		if (!effectData.m_statMods.IsNullOrEmpty())
		{
			num2 = effectData.m_statMods[0].modValue;
		}
		if (m_selfBuffIncomingHitsThisTurn > 0)
		{
			num2 += (float)m_selfBuffAbility.GetDamageIncreaseFirstHit();
			num2 += (float)(m_selfBuffAbility.GetDamageIncreaseSubseqHits() * (m_selfBuffIncomingHitsThisTurn - 1));
		}
		damageIncrease = Mathf.RoundToInt(num2);
		return true;
	}

	private int GetExtraDamageFromQueuedSelfBuff()
	{
		if (m_abilityData != null)
		{
			if (m_selfBuffAbility != null && m_selfBuffAbility.GetExtraDamageIfQueued() > 0)
			{
				if (m_abilityData.HasQueuedAbilityOfType(typeof(SamuraiSelfBuff)))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return m_selfBuffAbility.GetExtraDamageIfQueued();
						}
					}
				}
			}
		}
		return 0;
	}

	public int CalcExtraDamageFromSelfBuffAbility()
	{
		int damageIncrease = 0;
		IsSelfBuffActive(ref damageIncrease);
		return damageIncrease + GetExtraDamageFromQueuedSelfBuff();
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					writer.WritePackedUInt32((uint)m_lastSelfBuffTurn);
					writer.WritePackedUInt32((uint)m_selfBuffIncomingHitsThisTurn);
					writer.WritePackedUInt32((uint)m_afterimageX);
					writer.WritePackedUInt32((uint)m_afterimageY);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_lastSelfBuffTurn);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_selfBuffIncomingHitsThisTurn);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_afterimageX);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_afterimageY);
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
		if ((num & 8) == 0)
		{
			return;
		}
		while (true)
		{
			m_afterimageY = (int)reader.ReadPackedUInt32();
			return;
		}
	}
}
