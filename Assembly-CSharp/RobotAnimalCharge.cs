using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalCharge : Ability
{
	public int m_damageAmount = 20;
	public float m_lifeOnFirstHit;
	public float m_lifePerHit;
	public int m_maxTargetsHit = 1;
	public AbilityAreaShape m_targetShape;
	public bool m_targetShapePenetratesLoS;
	public bool m_chaseTarget = true;
	public StandardEffectInfo m_chaserEffect;
	public StandardEffectInfo m_enemyTargetEffect;
	public StandardEffectInfo m_allyTargetEffect;
	public float m_recoveryTime = 1f;
	[Header("-- Targeting: Whether require dashing at target actor")]
	public bool m_requireTargetActor = true;
	public bool m_canIncludeEnemy = true;
	public bool m_canIncludeAlly = true;
	[Header("-- Cooldown reduction on hitting target")]
	public int m_cdrOnHittingAlly;
	public int m_cdrOnHittingEnemy;
	private AbilityMod_RobotAnimalCharge m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Death Snuggle";
		}
		Setup();
	}

	private void Setup()
	{
		AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(
			this,
			m_targetShape,
			m_targetShapePenetratesLoS,
			AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos,
			CanIncludeEnemy(),
			CanIncludeAlly())
		{
			m_forceChase = true
		};
		if (ModdedLifeOnFirstHit() > 0f || ModdedLifePerHit() > 0f)
		{
			abilityUtil_Targeter_Charge.m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
			abilityUtil_Targeter_Charge.m_affectCasterDelegate = TargeterIncludeCaster;
		}
		Targeter = abilityUtil_Targeter_Charge;
	}

	private bool TargeterIncludeCaster(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
	{
		return AbilityUtils.GetEnemyCount(actorsSoFar, caster) > 0;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	public int ModdedDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public float ModdedLifeOnFirstHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lifeOnFirstHitMod.GetModifiedValue(m_lifeOnFirstHit)
			: m_lifeOnFirstHit;
	}

	public float ModdedLifePerHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lifePerHitMod.GetModifiedValue(m_lifePerHit)
			: m_lifePerHit;
	}

	public int ModdedHealOnNextTurnStartIfKilledTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healOnNextTurnStartIfKilledTarget
			: 0;
	}

	public StandardEffectInfo ModdedEffectForSelfPerAdjacentAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_effectToSelfPerAdjacentAlly
			: new StandardEffectInfo();
	}

	public int ModdedTechPointGainPerAdjacentAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointsPerAdjacentAlly
			: 0;
	}

	public bool RequireTargetActor()
	{
		return m_abilityMod != null
			? m_abilityMod.m_requireTargetActorMod.GetModifiedValue(m_requireTargetActor)
			: m_requireTargetActor;
	}

	public bool CanIncludeEnemy()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canIncludeEnemyMod.GetModifiedValue(m_canIncludeEnemy)
			: m_canIncludeEnemy;
	}

	public bool CanIncludeAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canIncludeAllyMod.GetModifiedValue(m_canIncludeAlly)
			: m_canIncludeAlly;
	}

	public int GetCdrOnHittingAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrOnHittingAllyMod.GetModifiedValue(m_cdrOnHittingAlly)
			: m_cdrOnHittingAlly;
	}

	public int GetCdrOnHittingEnemy()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrOnHittingEnemyMod.GetModifiedValue(m_cdrOnHittingEnemy)
			: m_cdrOnHittingEnemy;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (!RequireTargetActor())
		{
			return true;
		}
		return HasTargetableActorsInDecision(
			caster,
			CanIncludeEnemy(),
			CanIncludeAlly(),
			false,
			ValidateCheckPath.CanBuildPath,
			true,
			false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (!RequireTargetActor())
		{
			return true;
		}
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, CanIncludeAlly(), CanIncludeEnemy());
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			m_targetShape,
			target,
			m_targetShapePenetratesLoS,
			caster,
			relevantTeams,
			null);
		foreach (ActorData current in actorsInShape)
		{
			if (CanTargetActorInDecision(
				    caster,
				    current,
				    CanIncludeEnemy(),
				    CanIncludeAlly(),
				    false,
				    ValidateCheckPath.CanBuildPath,
				    true,
				    false))
			{
				return true;
			}
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_damageAmount);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Tertiary, Mathf.RoundToInt(m_lifeOnFirstHit));
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Quaternary, Mathf.RoundToInt(m_lifePerHit));
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, ModdedDamage());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, Mathf.RoundToInt(Mathf.Max(ModdedLifeOnFirstHit(), ModdedLifePerHit())));
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			List<ActorData> visibleActorsInRangeByTooltipSubject = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
			dictionary[AbilityTooltipSymbol.Healing] = GetLifeGainAmount(visibleActorsInRangeByTooltipSubject.Count);
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
		{
			dictionary[AbilityTooltipSymbol.Damage] = ModdedDamage();
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RobotAnimalCharge abilityMod_RobotAnimalCharge = modAsBase as AbilityMod_RobotAnimalCharge;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_RobotAnimalCharge != null
			? abilityMod_RobotAnimalCharge.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AddTokenInt(tokens, "MaxTargetsHit", string.Empty, m_maxTargetsHit);
		AbilityMod.AddToken_EffectInfo(tokens, m_chaserEffect, "ChaserEffect", m_chaserEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyTargetEffect, "EnemyTargetEffect", m_enemyTargetEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyTargetEffect, "AllyTargetEffect", m_allyTargetEffect);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_RobotAnimalCharge))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_RobotAnimalCharge;
		Setup();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	public int GetLifeGainAmount(int hitCount)
	{
		float num = 0f;
		if (hitCount > 0 && ModdedLifeOnFirstHit() != 0f)
		{
			num += ModdedLifeOnFirstHit();
		}
		if (ModdedLifePerHit() != 0f)
		{
			num += ModdedLifePerHit() * hitCount;
		}
		return Mathf.RoundToInt(num);
	}
}
