using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SparkDash : Ability
{
	[Header("-- Targeting")]
	public bool m_canTargetAny;

	private AbilityAreaShape m_targetShape;
	private bool m_targetShapePenetratesLoS;

	[Space(5f)]
	public bool m_chooseDestination = true;
	public AbilityAreaShape m_chooseDestinationShape = AbilityAreaShape.Three_x_Three;
	[Header("-- On Hit")]
	public bool m_applyTetherEffectToTarget;
	public StandardEffectInfo m_effectOnTargetEnemy;
	public StandardEffectInfo m_effectOnTargetAlly;
	[Space(5f)]
	public bool m_chaseTargetActor;
	[Header("-- whether to heal allies who dashed away")]
	public bool m_healAllyWhoDashedAway;
	[Header("-- If Hitting Targets In Between")]
	public bool m_hitActorsInBetween;
	public float m_chargeHitWidth = 1f;
	public bool m_chargeHitPenetrateLos;
	public StandardEffectInfo m_effectOnEnemyInBetween;
	public StandardEffectInfo m_effectOnAllyInBetween;
	[Header("-- Dash Sequences")]
	public GameObject m_dashToEnemySequence;
	public GameObject m_dashToFriendlySequence;

	private AbilityMod_SparkDash m_abilityMod;
	private SparkBeamTrackerComponent m_beamSyncComp;
	private SparkBasicAttack m_damageBeamAbility;
	private SparkHealingBeam m_healBeamAbility;

	private StandardEffectInfo m_cachedTargetEnemyEffect;
	private StandardEffectInfo m_cachedTargetAllyEffect;
	private StandardEffectInfo m_cachedInBetweenEnemyEffect;
	private StandardEffectInfo m_cachedInBetweenAllyEffect;

	private void Start()
	{
		SetupTargeter();
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return ChooseDestinaton() ? 2 : 1;
	}

	private void SetupTargeter()
	{
		AbilityData component = GetComponent<AbilityData>();
		m_damageBeamAbility = component.GetAbilityOfType(typeof(SparkBasicAttack)) as SparkBasicAttack;
		m_healBeamAbility = component.GetAbilityOfType(typeof(SparkHealingBeam)) as SparkHealingBeam;
		if (m_beamSyncComp == null)
		{
			m_beamSyncComp = GetComponent<SparkBeamTrackerComponent>();
		}
		SetCachedFields();
		ClearTargeters();
		AbilityUtil_Targeter abilityUtil_Targeter = null;
		if (ShouldHitActorsInBetween())
		{
			AbilityUtil_Targeter_ChargeAoE targeter = new AbilityUtil_Targeter_ChargeAoE(this, 0f, 0f, 0.5f * GetChargeWidth(), -1, false, m_chargeHitPenetrateLos);
			targeter.SetAffectedGroups(true, true, false);
			if (ShouldHitActorsInBetween())
			{
				targeter.m_shouldAddTargetDelegate = TargeterAddActorInbetweenDelegate;
			}
			abilityUtil_Targeter = targeter;
		}
		else
		{
			abilityUtil_Targeter = new AbilityUtil_Targeter_Charge(this, m_targetShape, m_targetShapePenetratesLoS, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, true, true)
			{
				m_affectCasterDelegate = TargeterAffectsCaster
			};
		}
		if (ChooseDestinaton())
		{
			Targeters.Add(abilityUtil_Targeter);
			AbilityUtil_Targeter_Charge targeter = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, false);
			targeter.SetUseMultiTargetUpdate(true);
			Targeters.Add(targeter);
		}
		else
		{
			Targeter = abilityUtil_Targeter;
		}
		ResetNameplateTargetingNumbers();
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	private bool TargeterAffectsCaster(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
	{
		bool hasHealForAlly = GetHealOnSelfForAllyHit() > 0;
		bool hasHealForEnemy = GetHealOnSelfForEnemyHit() > 0;
		if ((hasHealForAlly || hasHealForEnemy) && caster != null && actorsSoFar != null)
		{
			foreach (ActorData actor in actorsSoFar)
			{
				if (hasHealForAlly && actor.GetTeam() == caster.GetTeam())
				{
					return true;
				}
				if (hasHealForEnemy && actor.GetTeam() != caster.GetTeam())
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool TargeterAddActorInbetweenDelegate(ActorData actorToConsider, AbilityTarget abilityTarget, List<ActorData> hitActors, ActorData caster, Ability ability)
	{
		SparkDash sparkDash = ability as SparkDash;
		BoardSquare targetSquare = Board.Get().GetSquare(abilityTarget.GridPos);
		if (sparkDash == null || targetSquare == null)
		{
			return true;
		}
		if (actorToConsider.GetCurrentBoardSquare() == targetSquare)
		{
			return true;
		}
		if (sparkDash.GetInBetweenEnemyEffect().m_applyEffect && actorToConsider.GetTeam() != caster.GetTeam())
		{
			return true;
		}
		if (sparkDash.GetInBetweenAllyEffect().m_applyEffect && actorToConsider.GetTeam() == caster.GetTeam())
		{
			return true;
		}
		return false;
	}

	public bool ChooseDestinaton()
	{
		bool chooseDestination = m_abilityMod != null
			? m_abilityMod.m_chooseDestinationMod.GetModifiedValue(m_chooseDestination)
			: m_chooseDestination;
		return chooseDestination && m_targetData.Length > 1;
	}

	public AbilityAreaShape GetChooseDestShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chooseDestShapeMod.GetModifiedValue(m_chooseDestinationShape)
			: m_chooseDestinationShape;
	}

	public bool ShouldChaseTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chaseTargetActorMod.GetModifiedValue(m_chaseTargetActor)
			: m_chaseTargetActor;
	}

	public int GetDamage()
	{
		return m_damageBeamAbility != null
			? m_damageBeamAbility.GetInitialDamage()
			: 0;
	}

	public int GetHealOnAlly()
	{
		return m_healBeamAbility != null
			? m_healBeamAbility.GetHealingOnAttach()
			: 0;
	}

	public int GetHealOnSelfForAllyHit()
	{
		return m_healBeamAbility != null
			? m_healBeamAbility.GetHealOnSelfPerTurn()
			: 0;
	}

	public int GetHealOnSelfForEnemyHit()
	{
		return m_damageBeamAbility != null
			? m_damageBeamAbility.GetHealOnCasterPerTurn()
			: 0;
	}

	public bool ShouldHitActorsInBetween()
	{
		bool hitActorsInBetween = m_abilityMod != null
			? m_abilityMod.m_hitActorsInBetweenMod.GetModifiedValue(m_hitActorsInBetween)
			: m_hitActorsInBetween;
		bool applyEffect = GetInBetweenEnemyEffect().m_applyEffect || GetInBetweenAllyEffect().m_applyEffect;
		return hitActorsInBetween
			&& applyEffect
			&& GetChargeWidth() > 0f;
	}

	public float GetChargeWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chargeHitWidthMod.GetModifiedValue(m_chargeHitWidth)
			: m_chargeHitWidth;
	}

	public StandardEffectInfo GetTargetEnemyEffect()
	{
		return m_cachedTargetEnemyEffect;
	}

	public StandardEffectInfo GetTargetAllyEffect()
	{
		return m_cachedTargetAllyEffect;
	}

	public StandardEffectInfo GetInBetweenEnemyEffect()
	{
		return m_cachedInBetweenEnemyEffect;
	}

	public StandardEffectInfo GetInBetweenAllyEffect()
	{
		return m_cachedInBetweenAllyEffect;
	}

	private void SetCachedFields()
	{
		m_cachedTargetEnemyEffect = m_abilityMod != null
			? m_abilityMod.m_effectOnEnemyMod.GetModifiedValue(m_effectOnTargetEnemy)
			: m_effectOnTargetEnemy;
		m_cachedTargetAllyEffect = m_abilityMod != null
			? m_abilityMod.m_effectOnAllyMod.GetModifiedValue(m_effectOnTargetAlly)
			: m_effectOnTargetAlly;
		m_cachedInBetweenEnemyEffect = m_abilityMod != null
			? m_abilityMod.m_effectOnEnemyInBetweenMod.GetModifiedValue(m_effectOnEnemyInBetween)
			: m_effectOnEnemyInBetween;
		m_cachedInBetweenAllyEffect = m_abilityMod != null
			? m_abilityMod.m_effectOnAllyInBetweenMod.GetModifiedValue(m_effectOnAllyInBetween)
			: m_effectOnAllyInBetween;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamage());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetHealOnAlly());
		if (GetTargetAllyEffect() != null)
		{
			GetTargetAllyEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		}
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, 1);
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
		BoardSquare targetSquare = Board.Get().GetSquare(Targeter.LastUpdatingGridPos);
		bool isTarget = targetSquare != null && targetSquare == targetActor.GetCurrentBoardSquare();
		int age = m_beamSyncComp != null
			? m_beamSyncComp.GetTetherAgeOnActor(targetActor.ActorIndex)
			: 0;
		int allyNum = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
		int enemyNum = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
		{
			int damage = GetDamage();
			if (m_damageBeamAbility != null)
			{
				damage += m_damageBeamAbility.GetBonusDamageFromTetherAge(age);
			}
			dictionary[AbilityTooltipSymbol.Damage] = isTarget ? damage : 0;
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
		{
			int heal = GetHealOnAlly();
			bool isTracked = m_beamSyncComp.IsActorIndexTracked(targetActor.ActorIndex);
			dictionary[AbilityTooltipSymbol.Healing] = isTarget && !isTracked ? heal : 0;
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			int selfHeal = 0;
			if (allyNum > 0)
			{
				selfHeal = GetHealOnSelfForAllyHit();
			}
			else if (enemyNum > 0)
			{
				selfHeal = GetHealOnSelfForEnemyHit();
			}
			dictionary[AbilityTooltipSymbol.Healing] = selfHeal;
		}
		return dictionary;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		TargetingParadigm targetingParadigm = GetTargetingParadigm(0);
		if (targetingParadigm != TargetingParadigm.BoardSquare && targetingParadigm != TargetingParadigm.Position)
		{
			return true;
		}
		SparkBeamTrackerComponent trackerComponent = caster.GetComponent<SparkBeamTrackerComponent>();
		List<ActorData> targets = null;
		if (m_canTargetAny)
		{
			if (NetworkServer.active)
			{
				targets = GameFlowData.Get().GetActorsVisibleToActor(caster);
			}
			else
			{
				targets = GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData);
			}
			targets.Remove(caster);
		}
		else if (trackerComponent.BeamIsActive())
		{
			targets = trackerComponent.GetBeamActors();
		}
		if (targets != null)
		{
			foreach (ActorData current in targets)
			{
				if (CanTargetActorInDecision(caster, current, true, true, false, ValidateCheckPath.CanBuildPath, true, false))
				{
					return true;
				}
			}
		}
		return false;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		bool canCharge = false;
		if (targetIndex == 0)
		{
			List<Team> list = new List<Team>
			{
				caster.GetEnemyTeam(),
				caster.GetTeam()
			};
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(m_targetShape, target, m_targetShapePenetratesLoS, caster, list, null);
			SparkBeamTrackerComponent trackerComponent = caster.GetComponent<SparkBeamTrackerComponent>();
			foreach (ActorData current in actorsInShape)
			{
				if (CanTargetActorInDecision(caster, current, true, true, false, ValidateCheckPath.CanBuildPath, true, false)
					&& (m_canTargetAny || trackerComponent.IsTrackingActor(current.ActorIndex)))
				{
					flag = true;
					canCharge = true;
					break;
				}
			}
		}
		else
		{
			flag = true;
			BoardSquare prevTargetSquare = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
			BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
			if (targetSquare != null
				&& targetSquare.IsValidForGameplay()
				&& targetSquare != prevTargetSquare
				&& targetSquare != caster.GetCurrentBoardSquare()
				&& AreaEffectUtils.IsSquareInShape(targetSquare, GetChooseDestShape(), target.FreePos, prevTargetSquare, false, caster))
			{
				canCharge = KnockbackUtils.CanBuildStraightLineChargePath(caster, targetSquare, prevTargetSquare, false, out int _);
			}
		}
		return canCharge && flag;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SparkDash))
		{
			m_abilityMod = abilityMod as AbilityMod_SparkDash;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
