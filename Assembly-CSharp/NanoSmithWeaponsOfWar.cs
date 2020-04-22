using System.Collections.Generic;
using UnityEngine;

public class NanoSmithWeaponsOfWar : Ability
{
	[Header("-- Targeting in Ability")]
	public bool m_canTargetEnemies = true;

	public bool m_canTargetAllies = true;

	[Header("-- Effect on Ability Target")]
	public StandardEffectInfo m_targetAllyOnHitEffect;

	public StandardEffectInfo m_targetEnemyOnHitEffect;

	[Header("-- Sweep Info")]
	public int m_sweepDamageAmount = 10;

	public int m_sweepDuration = 3;

	public int m_sweepDamageDelay;

	[Header("-- Sweep On Hit Effects")]
	public StandardEffectInfo m_enemySweepOnHitEffect;

	public StandardEffectInfo m_allySweepOnHitEffect;

	[Header("-- Sweep Targeting")]
	public AbilityAreaShape m_sweepShape = AbilityAreaShape.Three_x_Three;

	public bool m_sweepIncludeEnemies = true;

	public bool m_sweepIncludeAllies;

	public bool m_sweepPenetrateLineOfSight;

	public bool m_sweepIncludeTarget;

	[Header("-- Sequences -----------------------------")]
	public GameObject m_castSequencePrefab;

	public GameObject m_persistentSequencePrefab;

	public GameObject m_rangeIndicatorSequencePrefab;

	public GameObject m_bladeSequencePrefab;

	public GameObject m_shieldPerTurnSequencePrefab;

	private AbilityMod_NanoSmithWeaponsOfWar m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Weapons of War";
		}
		AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, m_sweepShape, m_sweepPenetrateLineOfSight, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Never, AbilityUtil_Targeter.AffectsActor.Always);
		abilityUtil_Targeter_Shape.SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Secondary);
		base.Targeter = abilityUtil_Targeter_Shape;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetAllyTargetEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetSweepDamage());
		return numbers;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(caster, currentBestActorTarget, m_canTargetEnemies, m_canTargetAllies, m_canTargetAllies, ValidateCheckPath.Ignore, true, true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_NanoSmithWeaponsOfWar abilityMod_NanoSmithWeaponsOfWar = modAsBase as AbilityMod_NanoSmithWeaponsOfWar;
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_NanoSmithWeaponsOfWar)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			effectInfo = abilityMod_NanoSmithWeaponsOfWar.m_allyTargetEffectOverride.GetModifiedValue(m_targetAllyOnHitEffect);
		}
		else
		{
			effectInfo = m_targetAllyOnHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "TargetAllyOnHitEffect", m_targetAllyOnHitEffect);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_NanoSmithWeaponsOfWar)
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
			val = abilityMod_NanoSmithWeaponsOfWar.m_sweepDamageMod.GetModifiedValue(m_sweepDamageAmount);
		}
		else
		{
			val = m_sweepDamageAmount;
		}
		AddTokenInt(tokens, "SweepDamageAmount", empty, val);
		AddTokenInt(tokens, "SweepDuration", string.Empty, (!abilityMod_NanoSmithWeaponsOfWar) ? m_sweepDuration : abilityMod_NanoSmithWeaponsOfWar.m_sweepDurationMod.GetModifiedValue(m_sweepDuration));
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_NanoSmithWeaponsOfWar)
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
			effectInfo2 = abilityMod_NanoSmithWeaponsOfWar.m_enemySweepOnHitEffectOverride.GetModifiedValue(m_enemySweepOnHitEffect);
		}
		else
		{
			effectInfo2 = m_enemySweepOnHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EnemySweepOnHitEffect", m_enemySweepOnHitEffect);
		StandardEffectInfo effectInfo3;
		if ((bool)abilityMod_NanoSmithWeaponsOfWar)
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
			effectInfo3 = abilityMod_NanoSmithWeaponsOfWar.m_allySweepOnHitEffectOverride.GetModifiedValue(m_allySweepOnHitEffect);
		}
		else
		{
			effectInfo3 = m_allySweepOnHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "AllySweepOnHitEffect", m_allySweepOnHitEffect);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NanoSmithWeaponsOfWar))
		{
			m_abilityMod = (abilityMod as AbilityMod_NanoSmithWeaponsOfWar);
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
	}

	private int GetSweepDuration()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_sweepDurationMod.GetModifiedValue(m_sweepDuration) : m_sweepDuration;
	}

	private int GetSweepDamage()
	{
		int result;
		if (m_abilityMod == null)
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
			result = m_sweepDamageAmount;
		}
		else
		{
			result = m_abilityMod.m_sweepDamageMod.GetModifiedValue(m_sweepDamageAmount);
		}
		return result;
	}

	private int GetShieldGainPerTurn()
	{
		int result;
		if (m_abilityMod == null)
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
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_shieldGainPerTurnMod.GetModifiedValue(0);
		}
		return result;
	}

	private StandardEffectInfo GetAllyTargetEffect()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
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
			result = m_targetAllyOnHitEffect;
		}
		else
		{
			result = m_abilityMod.m_allyTargetEffectOverride.GetModifiedValue(m_targetAllyOnHitEffect);
		}
		return result;
	}

	private StandardEffectInfo GetAllySweepEffect()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
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
			result = m_allySweepOnHitEffect;
		}
		else
		{
			result = m_abilityMod.m_allySweepOnHitEffectOverride.GetModifiedValue(m_allySweepOnHitEffect);
		}
		return result;
	}

	private StandardEffectInfo GetEnemySweepEffect()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_enemySweepOnHitEffectOverride.GetModifiedValue(m_enemySweepOnHitEffect) : m_enemySweepOnHitEffect;
	}
}
