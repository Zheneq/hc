// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class TrackerFlewTheCoop : Ability
{
	public AbilityAreaShape m_hookshotShape = AbilityAreaShape.Three_x_Three_NoCorners;
	public bool m_includeDroneSquare = true;

	private TrackerDroneTrackerComponent m_droneTracker;
	private AbilityMod_TrackerFlewTheCoop m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Flew The Coop";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_droneTracker = GetComponent<TrackerDroneTrackerComponent>();
		if (m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
		}
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		bool doesAffectCaster = 
			(moddedEffectForSelf != null
				&& moddedEffectForSelf.m_applyEffect)
			|| (m_abilityMod != null
				&& m_abilityMod.m_additionalEffectOnSelf != null
				&& m_abilityMod.m_additionalEffectOnSelf.m_applyEffect);
		AbilityUtil_Targeter.AffectsActor affectsCaster = doesAffectCaster ? AbilityUtil_Targeter.AffectsActor.Always : AbilityUtil_Targeter.AffectsActor.Never;
		Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, affectsCaster)
		{
			ShowArcToShape = false
		};
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AppendTooltipNumbersFromBaseModEffects(ref numbers, AbilityTooltipSubject.Enemy);
		if (m_abilityMod != null && m_abilityMod.m_additionalEffectOnSelf != null)
		{
			m_abilityMod.m_additionalEffectOnSelf.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		}
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_droneTracker == null)
		{
			return false;
		}
		bool isActive = m_droneTracker.DroneIsActive();
		bool isInRange = false;
		if (!caster.IsDead())
		{
			BoardSquare droneSquare = Board.Get().GetSquareFromIndex(m_droneTracker.BoardX(), m_droneTracker.BoardY());
			if (droneSquare != null)
			{
				float rangeInSquares = GetRangeInSquares(0);
				float dist = VectorUtils.HorizontalPlaneDistInSquares(caster.GetFreePos(), droneSquare.ToVector3());
				if (rangeInSquares == 0f || dist <= rangeInSquares)
				{
					isInRange = true;
				}
			}
		}
		return isActive && isInRange;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Flight;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null
			|| !targetSquare.IsValidForGameplay()
			|| targetSquare == caster.GetCurrentBoardSquare())
		{
			return false;
		}
		if (m_droneTracker != null)
		{
			BoardSquare droneSquare = Board.Get().GetSquareFromIndex(m_droneTracker.BoardX(), m_droneTracker.BoardY());
			if (droneSquare != null)
			{
				if (!m_includeDroneSquare && droneSquare == targetSquare)
				{
					return false;
				}
				List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(GetLandingShape(), droneSquare.ToVector3(), droneSquare, true, caster);
				if (squaresInShape.Contains(targetSquare))
				{
					return true;
				}
			}
		}
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TrackerFlewTheCoop))
		{
			m_abilityMod = (abilityMod as AbilityMod_TrackerFlewTheCoop);
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private AbilityAreaShape GetLandingShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_landingShapeMod.GetModifiedValue(m_hookshotShape)
			: m_hookshotShape;
	}

	public int GetModdedExtraDroneDamageDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDroneDamageDuration.GetModifiedValue(0)
			: 0;
	}

	public int GetModdedExtraDroneDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDroneDamage.GetModifiedValue(0)
			: 0;
	}

	public int GetModdedExtraDroneUntrackedDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDroneUntrackedDamage.GetModifiedValue(0)
			: 0;
	}

#if SERVER
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			targets[0].FreePos,
			new ActorData[] { caster },
			caster,
			additionalData.m_sequenceSource,
			null);
	}
#endif

#if SERVER
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		if (m_abilityMod != null)
		{
			actorHitResults.AddStandardEffectInfo(m_abilityMod.m_additionalEffectOnSelf);
			if (m_abilityMod.m_addVisionAroundStartSquare)
			{
				float num = m_abilityMod.m_visionRadius;
				if (num <= 0f)
				{
					num = caster.GetSightRange();
				}
				int duration = Mathf.Max(1, m_abilityMod.m_visionDuration);
				PositionVisionProviderEffect effect = new PositionVisionProviderEffect(AsEffectSource(), caster.GetSquareAtPhaseStart(), caster, duration, num, m_abilityMod.m_brushRevealType, false, null);
				actorHitResults.AddEffect(effect);
			}
		}
		abilityResults.StoreActorHit(actorHitResults);
		TrackerDroneInfoComponent component = GetComponent<TrackerDroneInfoComponent>();
		if (component != null)
		{
			component.m_escapeLastTurnCast = GameFlowData.Get().CurrentTurn;
		}
	}
#endif

#if SERVER
	public override void OnDodgedDamage(ActorData caster, int damageDodged)
	{
		caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.TrackerStats.DamageDodgedWithEvade, damageDodged);
	}
#endif
}
