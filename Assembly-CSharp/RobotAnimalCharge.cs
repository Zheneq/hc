using System;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalCharge : Ability
{
	public int m_damageAmount = 0x14;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Death Snuggle";
		}
		this.Setup();
	}

	private void Setup()
	{
		AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(this, this.m_targetShape, this.m_targetShapePenetratesLoS, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, this.CanIncludeEnemy(), this.CanIncludeAlly());
		abilityUtil_Targeter_Charge.m_forceChase = true;
		if (this.ModdedLifeOnFirstHit() <= 0f)
		{
			if (this.ModdedLifePerHit() <= 0f)
			{
				goto IL_73;
			}
		}
		abilityUtil_Targeter_Charge.m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
		abilityUtil_Targeter_Charge.m_affectCasterDelegate = new AbilityUtil_Targeter_Shape.IsAffectingCasterDelegate(this.TargeterIncludeCaster);
		IL_73:
		base.Targeter = abilityUtil_Targeter_Charge;
	}

	private bool TargeterIncludeCaster(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
	{
		int enemyCount = AbilityUtils.GetEnemyCount(actorsSoFar, caster);
		return enemyCount > 0;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	public int ModdedDamage()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_damageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damageAmount);
		}
		return result;
	}

	public float ModdedLifeOnFirstHit()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			result = this.m_lifeOnFirstHit;
		}
		else
		{
			result = this.m_abilityMod.m_lifeOnFirstHitMod.GetModifiedValue(this.m_lifeOnFirstHit);
		}
		return result;
	}

	public float ModdedLifePerHit()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			result = this.m_lifePerHit;
		}
		else
		{
			result = this.m_abilityMod.m_lifePerHitMod.GetModifiedValue(this.m_lifePerHit);
		}
		return result;
	}

	public int ModdedHealOnNextTurnStartIfKilledTarget()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_healOnNextTurnStartIfKilledTarget;
		}
		return result;
	}

	public StandardEffectInfo ModdedEffectForSelfPerAdjacentAlly()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
		{
			result = new StandardEffectInfo();
		}
		else
		{
			result = this.m_abilityMod.m_effectToSelfPerAdjacentAlly;
		}
		return result;
	}

	public int ModdedTechPointGainPerAdjacentAlly()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_techPointsPerAdjacentAlly;
		}
		return result;
	}

	public bool RequireTargetActor()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_requireTargetActorMod.GetModifiedValue(this.m_requireTargetActor);
		}
		else
		{
			result = this.m_requireTargetActor;
		}
		return result;
	}

	public bool CanIncludeEnemy()
	{
		return (!this.m_abilityMod) ? this.m_canIncludeEnemy : this.m_abilityMod.m_canIncludeEnemyMod.GetModifiedValue(this.m_canIncludeEnemy);
	}

	public bool CanIncludeAlly()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_canIncludeAllyMod.GetModifiedValue(this.m_canIncludeAlly);
		}
		else
		{
			result = this.m_canIncludeAlly;
		}
		return result;
	}

	public int GetCdrOnHittingAlly()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_cdrOnHittingAllyMod.GetModifiedValue(this.m_cdrOnHittingAlly);
		}
		else
		{
			result = this.m_cdrOnHittingAlly;
		}
		return result;
	}

	public int GetCdrOnHittingEnemy()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_cdrOnHittingEnemyMod.GetModifiedValue(this.m_cdrOnHittingEnemy);
		}
		else
		{
			result = this.m_cdrOnHittingEnemy;
		}
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.RequireTargetActor())
		{
			return base.HasTargetableActorsInDecision(caster, this.CanIncludeEnemy(), this.CanIncludeAlly(), false, Ability.ValidateCheckPath.CanBuildPath, true, false, false);
		}
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool result = true;
		if (this.RequireTargetActor())
		{
			result = false;
			List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, this.CanIncludeAlly(), this.CanIncludeEnemy());
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_targetShape, target, this.m_targetShapePenetratesLoS, caster, relevantTeams, null);
			using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData targetActor = enumerator.Current;
					bool flag = base.CanTargetActorInDecision(caster, targetActor, this.CanIncludeEnemy(), this.CanIncludeAlly(), false, Ability.ValidateCheckPath.CanBuildPath, true, false, false);
					if (flag)
					{
						return true;
					}
				}
			}
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.m_damageAmount);
		int amount = Mathf.RoundToInt(this.m_lifeOnFirstHit);
		int amount2 = Mathf.RoundToInt(this.m_lifePerHit);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Tertiary, amount);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Quaternary, amount2);
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.ModdedDamage());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, Mathf.RoundToInt(Mathf.Max(this.ModdedLifeOnFirstHit(), this.ModdedLifePerHit())));
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
				int lifeGainAmount = this.GetLifeGainAmount(visibleActorsInRangeByTooltipSubject.Count);
				dictionary[AbilityTooltipSymbol.Healing] = lifeGainAmount;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				dictionary[AbilityTooltipSymbol.Damage] = this.ModdedDamage();
			}
			return dictionary;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RobotAnimalCharge abilityMod_RobotAnimalCharge = modAsBase as AbilityMod_RobotAnimalCharge;
		string name = "DamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_RobotAnimalCharge)
		{
			val = abilityMod_RobotAnimalCharge.m_damageMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			val = this.m_damageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		base.AddTokenInt(tokens, "MaxTargetsHit", string.Empty, this.m_maxTargetsHit, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_chaserEffect, "ChaserEffect", this.m_chaserEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyTargetEffect, "EnemyTargetEffect", this.m_enemyTargetEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyTargetEffect, "AllyTargetEffect", this.m_allyTargetEffect, true);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RobotAnimalCharge))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_RobotAnimalCharge);
			this.Setup();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	public int GetLifeGainAmount(int hitCount)
	{
		float num = 0f;
		if (hitCount > 0)
		{
			if (this.ModdedLifeOnFirstHit() != 0f)
			{
				num += this.ModdedLifeOnFirstHit();
			}
		}
		if (this.ModdedLifePerHit() != 0f)
		{
			num += this.ModdedLifePerHit() * (float)hitCount;
		}
		return Mathf.RoundToInt(num);
	}
}
