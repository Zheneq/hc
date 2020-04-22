using System.Collections.Generic;
using UnityEngine;

public class ScampDashAndAoe : GenericAbility_Container
{
	[Separator("Shield Cost on Cast", true)]
	public int m_shieldCost = 20;

	[Separator("Target Select Component for when shield is down", true)]
	public GenericAbility_TargetSelectBase m_shieldDownTargetSelect;

	[Separator("On Hit Data for when shield is down", "yellow")]
	public OnHitAuthoredData m_shieldDownOnHitData;

	[Separator("Cooldown for Shield Down mode. If <= 0, use same cooldown for both modes", true)]
	public int m_shieldDownCooldown = -1;

	[Header("-- Cdr on suit dash when going into suit form")]
	public int m_cdrOnSuitApply;

	[Header("-- if > 0 and health below threshold, shield down form of dash has no cooldowns")]
	public int m_shieldDownNoCooldownHealthThresh;

	[Separator("Extra Energy for dashing through or onto orb", true)]
	public int m_extraEnergyForDashOnOrb;

	[Separator("Whether we can move after dashing when out of suit", true)]
	public bool m_shieldDownAllowMoveAfterEvade = true;

	private AbilityMod_ScampDashAndAoe m_abilityMod;

	private Scamp_SyncComponent m_syncComp;

	private OnHitAuthoredData m_cachedShieldDownOnHitData;

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "\n-- On Hit Data when shields are down --\n" + m_shieldDownOnHitData.GetInEditorDesc();
	}

	public override List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> relevantTargetSelectCompForEditor = base.GetRelevantTargetSelectCompForEditor();
		if (m_shieldDownTargetSelect != null)
		{
			relevantTargetSelectCompForEditor.Add(m_shieldDownTargetSelect);
		}
		return relevantTargetSelectCompForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
		base.SetupTargetersAndCachedVars();
		if (m_abilityMod != null)
		{
			m_cachedShieldDownOnHitData = m_abilityMod.m_shieldDownOnHitDataMod._001D(m_shieldDownOnHitData);
		}
		else
		{
			m_cachedShieldDownOnHitData = m_shieldDownOnHitData;
		}
	}

	public void ResetTargetersForShielding(bool hasShield)
	{
		ClearTargeters();
		List<AbilityUtil_Targeter> collection;
		if (!hasShield)
		{
			if (!(m_shieldDownTargetSelect == null))
			{
				collection = m_shieldDownTargetSelect.CreateTargeters(this);
				goto IL_004a;
			}
		}
		collection = m_targetSelectComp.CreateTargeters(this);
		goto IL_004a;
		IL_004a:
		base.Targeters.AddRange(collection);
	}

	public int GetShieldCost()
	{
		return (!(m_abilityMod != null)) ? m_shieldCost : m_abilityMod.m_shieldCostMod.GetModifiedValue(m_shieldCost);
	}

	public int GetShieldDownCooldown()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_shieldDownCooldownMod.GetModifiedValue(m_shieldDownCooldown);
		}
		else
		{
			result = m_shieldDownCooldown;
		}
		return result;
	}

	public int GetCdrOnSuitApply()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_cdrOnSuitApplyMod.GetModifiedValue(m_cdrOnSuitApply);
		}
		else
		{
			result = m_cdrOnSuitApply;
		}
		return result;
	}

	public int GetShieldDownNoCooldownHealthThresh()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_shieldDownNoCooldownHealthThreshMod.GetModifiedValue(m_shieldDownNoCooldownHealthThresh);
		}
		else
		{
			result = m_shieldDownNoCooldownHealthThresh;
		}
		return result;
	}

	public int GetExtraEnergyForDashOnOrb()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_extraEnergyForDashOnOrbMod.GetModifiedValue(m_extraEnergyForDashOnOrb);
		}
		else
		{
			result = m_extraEnergyForDashOnOrb;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_shieldDownOnHitData.AddTooltipTokens(tokens);
		AddTokenInt(tokens, "ShieldCost", string.Empty, m_shieldCost);
		AddTokenInt(tokens, "ShieldDownCooldown", string.Empty, m_shieldDownCooldown);
		AddTokenInt(tokens, "CdrOnSuitApply", string.Empty, m_cdrOnSuitApply);
		AddTokenInt(tokens, "ShidleDownNoCooldownHealthThresh", string.Empty, m_shieldDownNoCooldownHealthThresh);
		AddTokenInt(tokens, "ExtraEnergyForDashOnOrb", string.Empty, m_extraEnergyForDashOnOrb);
	}

	public bool IsInSuit()
	{
		int result;
		if (m_syncComp != null)
		{
			result = (m_syncComp.m_suitWasActiveOnTurnStart ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		if (IsInSuit())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return base.GetTargetSelectComp();
				}
			}
		}
		return m_shieldDownTargetSelect;
	}

	public override OnHitAuthoredData GetOnHitAuthoredData()
	{
		if (IsInSuit())
		{
			return base.GetOnHitAuthoredData();
		}
		OnHitAuthoredData result;
		if (m_cachedShieldDownOnHitData != null)
		{
			result = m_cachedShieldDownOnHitData;
		}
		else
		{
			result = m_shieldDownOnHitData;
		}
		return result;
	}

	public override int GetBaseCooldown()
	{
		if (!IsInSuit())
		{
			if (GetShieldDownCooldown() >= 0)
			{
				return GetShieldDownCooldown();
			}
		}
		return base.GetBaseCooldown();
	}

	public int CalcCurrentMaxCooldown(bool inSuit)
	{
		int num;
		if (inSuit)
		{
			num = m_cooldown;
		}
		else
		{
			num = GetShieldDownCooldown();
		}
		int result = num;
		if (GetShieldDownCooldown() < 0)
		{
			result = m_cooldown;
		}
		return result;
	}

	public override MovementAdjustment GetMovementAdjustment()
	{
		if (CanOverrideMoveStartSquare())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return MovementAdjustment.ReducedMovement;
				}
			}
		}
		return base.GetMovementAdjustment();
	}

	public override bool CanOverrideMoveStartSquare()
	{
		int result;
		if (!IsInSuit())
		{
			result = (m_shieldDownAllowMoveAfterEvade ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_ScampDashAndAoe);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}

	protected override void SetTargetSelectModReference()
	{
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_targetSelectComp.SetTargetSelectMod(m_abilityMod.m_inSuitTargetSelectMod);
					m_shieldDownTargetSelect.SetTargetSelectMod(m_abilityMod.m_shieldDownTargetSelectMod);
					return;
				}
			}
		}
		m_targetSelectComp.ClearTargetSelectMod();
		m_shieldDownTargetSelect.ClearTargetSelectMod();
	}
}
