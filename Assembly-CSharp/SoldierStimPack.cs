using System.Collections.Generic;
using UnityEngine;

public class SoldierStimPack : Ability
{
	[Separator("On Hit", true)]
	public int m_selfHealAmount;

	public StandardEffectInfo m_selfHitEffect;

	[Separator("For other abilities when active", true)]
	public bool m_basicAttackIgnoreCover;

	public bool m_basicAttackReduceCoverEffectiveness;

	public float m_grenadeExtraRange;

	public StandardEffectInfo m_dashShootExtraEffect;

	[Separator("CDR - Health threshold to trigger cooldown reset, value:(0-1)", true)]
	public float m_cooldownResetHealthThreshold = -1f;

	[Header("-- CDR - if dash and shoot used on same turn")]
	public int m_cdrIfDashAndShootUsed;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	private AbilityMod_SoldierStimPack m_abilityMod;

	private AbilityData m_abilityData;

	private SoldierGrenade m_grenadeAbility;

	private StandardEffectInfo m_cachedSelfHitEffect;

	private StandardEffectInfo m_cachedDashShootExtraEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			m_abilityName = "Stim Pack";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_abilityData == null)
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
			m_abilityData = GetComponent<AbilityData>();
		}
		if (m_abilityData != null && m_grenadeAbility == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			m_grenadeAbility = (m_abilityData.GetAbilityOfType(typeof(SoldierGrenade)) as SoldierGrenade);
		}
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always);
		base.Targeter.ShowArcToShape = false;
	}

	private void SetCachedFields()
	{
		m_cachedSelfHitEffect = ((!m_abilityMod) ? m_selfHitEffect : m_abilityMod.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect));
		StandardEffectInfo cachedDashShootExtraEffect;
		if ((bool)m_abilityMod)
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
			cachedDashShootExtraEffect = m_abilityMod.m_dashShootExtraEffectMod.GetModifiedValue(m_dashShootExtraEffect);
		}
		else
		{
			cachedDashShootExtraEffect = m_dashShootExtraEffect;
		}
		m_cachedDashShootExtraEffect = cachedDashShootExtraEffect;
	}

	public int GetSelfHealAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
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
			result = m_abilityMod.m_selfHealAmountMod.GetModifiedValue(m_selfHealAmount);
		}
		else
		{
			result = m_selfHealAmount;
		}
		return result;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedSelfHitEffect != null)
		{
			while (true)
			{
				switch (2)
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
			result = m_cachedSelfHitEffect;
		}
		else
		{
			result = m_selfHitEffect;
		}
		return result;
	}

	public bool BasicAttackIgnoreCover()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (4)
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
			result = m_abilityMod.m_basicAttackIgnoreCoverMod.GetModifiedValue(m_basicAttackIgnoreCover);
		}
		else
		{
			result = m_basicAttackIgnoreCover;
		}
		return result;
	}

	public bool BasicAttackReduceCoverEffectiveness()
	{
		return (!m_abilityMod) ? m_basicAttackReduceCoverEffectiveness : m_abilityMod.m_basicAttackReduceCoverEffectivenessMod.GetModifiedValue(m_basicAttackReduceCoverEffectiveness);
	}

	public float GetGrenadeExtraRange()
	{
		return (!m_abilityMod) ? m_grenadeExtraRange : m_abilityMod.m_grenadeExtraRangeMod.GetModifiedValue(m_grenadeExtraRange);
	}

	public StandardEffectInfo GetDashShootExtraEffect()
	{
		return (m_cachedDashShootExtraEffect == null) ? m_dashShootExtraEffect : m_cachedDashShootExtraEffect;
	}

	public float GetCooldownResetHealthThreshold()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
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
			result = m_abilityMod.m_cooldownResetHealthThresholdMod.GetModifiedValue(m_cooldownResetHealthThreshold);
		}
		else
		{
			result = m_cooldownResetHealthThreshold;
		}
		return result;
	}

	public int GetCdrIfDashAndShootUsed()
	{
		return (!m_abilityMod) ? m_cdrIfDashAndShootUsed : m_abilityMod.m_cdrIfDashAndShootUsedMod.GetModifiedValue(m_cdrIfDashAndShootUsed);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Self, GetSelfHealAmount());
		GetSelfHitEffect().ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Self);
		return number;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SoldierStimPack abilityMod_SoldierStimPack = modAsBase as AbilityMod_SoldierStimPack;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_SoldierStimPack)
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
			val = abilityMod_SoldierStimPack.m_selfHealAmountMod.GetModifiedValue(m_selfHealAmount);
		}
		else
		{
			val = m_selfHealAmount;
		}
		AddTokenInt(tokens, "SelfHealAmount", empty, val);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_SoldierStimPack)
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
			effectInfo = abilityMod_SoldierStimPack.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect);
		}
		else
		{
			effectInfo = m_selfHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "SelfHitEffect", m_selfHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_SoldierStimPack) ? m_dashShootExtraEffect : abilityMod_SoldierStimPack.m_dashShootExtraEffectMod.GetModifiedValue(m_dashShootExtraEffect), "DashShootExtraEffect", m_dashShootExtraEffect);
		AddTokenInt(tokens, "CdrIfDashAndShootUsed", string.Empty, m_cdrIfDashAndShootUsed);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SoldierStimPack))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_SoldierStimPack);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
