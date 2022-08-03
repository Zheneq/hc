// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ScoundrelEvasionRoll : Ability
{
	[Header("-- Energy gain per step")]
	public int m_extraEnergyPerStep;

	// rogues
	// public StandardEffectInfo m_effectOnStart;

	private AbilityMod_ScoundrelEvasionRoll m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Evasion Roll";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_ScoundrelEvasionRoll(this, ShouldCreateTrapWire(), GetTrapwirePattern());
		if (HasAdditionalEffectFromMod())  // || m_effectOnStart.m_applyEffect in rogues
		{
			(Targeter as AbilityUtil_Targeter_ScoundrelEvasionRoll).m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Always;
		}
		else if (EffectForLandingInBrush() != null)
		{
			(Targeter as AbilityUtil_Targeter_ScoundrelEvasionRoll).m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
		}
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		return targetSquare != null
			&& targetSquare.IsValidForGameplay()
			&& targetSquare != caster.GetCurrentBoardSquare()
			&& KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare) != null;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		StandardEffectInfo brushEffect = EffectForLandingInBrush();
		if (brushEffect != null)
		{
			brushEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		}
		// rogues
		// if (m_effectOnStart.m_applyEffect)
		// {
		// 	m_effectOnStart.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		// }
		if (HasAdditionalEffectFromMod())
		{
			m_abilityMod.m_additionalEffectOnStart.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		}
		return numbers;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetExtraEnergyPerStep() > 0 && base.Targeter is AbilityUtil_Targeter_ScoundrelEvasionRoll)
		{
			AbilityUtil_Targeter_ScoundrelEvasionRoll targeter = Targeter as AbilityUtil_Targeter_ScoundrelEvasionRoll;
			if (targeter.m_numNodesInPath > 1)
			{
				return GetExtraEnergyPerStep() * (targeter.m_numNodesInPath - 1);
			}
		}
		return 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ScoundrelEvasionRoll))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}

		m_abilityMod = (abilityMod as AbilityMod_ScoundrelEvasionRoll);
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public int GetExtraEnergyPerStep()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraEnergyPerStepMod.GetModifiedValue(m_extraEnergyPerStep)
			: m_extraEnergyPerStep;
	}

	private bool ShouldCreateTrapWire()
	{
		return m_abilityMod != null
			&& m_abilityMod.m_dropTrapWireOnStart
			&& m_abilityMod.m_trapwirePattern != AbilityGridPattern.NoPattern;
	}

	private bool HasAdditionalEffectFromMod()
	{
		return m_abilityMod != null
			&& m_abilityMod.m_additionalEffectOnStart.m_applyEffect;
	}

	private AbilityGridPattern GetTrapwirePattern()
	{
		return m_abilityMod != null
			? m_abilityMod.m_trapwirePattern
			: AbilityGridPattern.NoPattern;
	}

	private int TechPointGainPerAdjacentAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointGainPerAdjacentAlly
			: 0;
	}

	private int TechPointGrantedToAdjacentAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointGrantedToAdjacentAllies
			: 0;
	}

	private StandardEffectInfo EffectForLandingInBrush()
	{
		if (m_abilityMod != null
			&& m_abilityMod.m_effectToSelfForLandingInBrush != null
			&& m_abilityMod.m_effectToSelfForLandingInBrush.m_applyEffect)
		{
			return m_abilityMod.m_effectToSelfForLandingInBrush;
		}
		return null;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ScoundrelEvasionRoll mod = modAsBase as AbilityMod_ScoundrelEvasionRoll;
		AddTokenInt(tokens, "ExtraEnergyPerStep", "", mod != null
			? mod.m_extraEnergyPerStepMod.GetModifiedValue(m_extraEnergyPerStep)
			: m_extraEnergyPerStep);
		Passive_StickAndMove stickAndMove = GetComponent<Passive_StickAndMove>();
		AddTokenInt(tokens, "CDR_OnDamageTaken", "", stickAndMove != null && stickAndMove.m_damageToAdvanceCooldown > 0 ? 1 : 0);
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		BoardSquare squareAtPhaseStart = caster.GetSquareAtPhaseStart();
		Vector3 loSCheckPos = caster.GetLoSCheckPos(squareAtPhaseStart);
		BoardSquare startSquare = Board.Get().GetSquareFromVec3(loSCheckPos);
		BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		list.Add(new ServerClientUtils.SequenceStartData(
			m_sequencePrefab,
			startSquare,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource));
		if (ShouldCreateTrapWire() && targetSquare != null)
		{
			Vector3 centerOfGridPattern = AreaEffectUtils.GetCenterOfGridPattern(
				m_abilityMod.m_trapwirePattern,
				targetSquare.ToVector3(),
				squareAtPhaseStart);
			list.Add(new ServerClientUtils.SequenceStartData(
				m_abilityMod.m_trapwireCastSequencePrefab,
				centerOfGridPattern,
				null,
				caster,
				additionalData.m_sequenceSource));
		}
		if (HasAdditionalEffectFromMod())
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_abilityMod.m_additionalEffectCastSequencePrefab,
				startSquare,
				caster.AsArray(),
				caster,
				additionalData.m_sequenceSource));
		}
		if (EffectForLandingInBrush() != null)  // || m_effectOnStart.m_applyEffect in rogues
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				SequenceLookup.Get().GetSimpleHitSequencePrefab(),
				startSquare,
				caster.AsArray(),
				caster,
				additionalData.m_sequenceSource));
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		if (ShouldCreateTrapWire() && targetSquare != null)
		{
			Vector3 centerOfGridPattern = AreaEffectUtils.GetCenterOfGridPattern(
				m_abilityMod.m_trapwirePattern,
				targetSquare.ToVector3(),
				caster.GetSquareAtPhaseStart());
			PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(centerOfGridPattern));
			m_abilityMod.m_trapWireBarrierData.SetupForPattern(m_abilityMod.m_trapwirePattern);
			Barrier barrier1 = new Barrier(
				m_abilityName,
				centerOfGridPattern,
				new Vector3(0f, 0f, 1f),
				caster,
				m_abilityMod.m_trapWireBarrierData);
			barrier1.SetSourceAbility(this);
			Barrier barrier2 = new Barrier(
				m_abilityName,
				centerOfGridPattern,
				new Vector3(1f, 0f, 0f),
				caster,
				m_abilityMod.m_trapWireBarrierData,
				false);
			barrier2.SetSourceAbility(this);
			LinkedBarrierData linkData = new LinkedBarrierData();
			List<Barrier> list = new List<Barrier>
			{
				barrier1,
				barrier2
			};
			positionHitResults.AddBarrier(barrier1);
			positionHitResults.AddBarrier(barrier2);
			BarrierManager.Get().LinkBarriers(list, linkData);
			abilityResults.StorePositionHit(positionHitResults);
		}
		int energyGainOnAlly = TechPointGrantedToAdjacentAllies();
		int energyGainPerAlly = TechPointGainPerAdjacentAlly();
		StandardEffectInfo standardEffectInfo = EffectForLandingInBrush();
		ActorHitResults actorHitResultsSelf = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		int numSquaresInProcessedEvade = ServerActionBuffer.Get().GetNumSquaresInProcessedEvade(caster);
		if (numSquaresInProcessedEvade > 1 && GetExtraEnergyPerStep() > 0)
		{
			actorHitResultsSelf.AddTechPointGain(GetExtraEnergyPerStep() * (numSquaresInProcessedEvade - 1));
		}
		if (HasAdditionalEffectFromMod()
		    // || m_effectOnStart.m_applyEffect // rogues
		    || standardEffectInfo != null
		    || energyGainPerAlly > 0
		    || energyGainOnAlly > 0)
		{
			// rogues
			// actorHitResults.AddStandardEffectInfo(m_effectOnStart);
			
			if (HasAdditionalEffectFromMod())
			{
				actorHitResultsSelf.AddStandardEffectInfo(m_abilityMod.m_additionalEffectOnStart);
			}
			if (standardEffectInfo != null && ActorData.IsInBrush())
			{
				actorHitResultsSelf.AddStandardEffectInfo(standardEffectInfo);
			}
			if (energyGainPerAlly > 0 || energyGainOnAlly > 0)
			{
				int alliesNearby = 0;
				List<BoardSquare> squares = new List<BoardSquare>();
				Board.Get().GetAllAdjacentSquares(caster.GetCurrentBoardSquare().x, caster.GetCurrentBoardSquare().y, ref squares);
				foreach (BoardSquare square in squares)
				{
					if (square.OccupantActor != null
					    && square.OccupantActor.GetTeam() == caster.GetTeam()
					    && square.OccupantActor != caster
					    && !square.OccupantActor.IgnoreForAbilityHits)
					{
						alliesNearby++;
						if (energyGainOnAlly > 0)
						{
							ActorHitParameters actorHitParameters = new ActorHitParameters(square.OccupantActor, caster.GetFreePos());
							ActorHitResults actorHitResults = new ActorHitResults(actorHitParameters);
							actorHitResults.AddTechPointGain(energyGainOnAlly);
							abilityResults.StoreActorHit(actorHitResults);
						}
					}
				}
				if (energyGainPerAlly > 0)
				{
					actorHitResultsSelf.AddTechPointGain(alliesNearby * energyGainPerAlly);
				}
			}
		}
		abilityResults.StoreActorHit(actorHitResultsSelf);
	}

	// added in rogues
	public override void OnDodgedDamage(ActorData caster, int damageDodged)
	{
		caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.ScoundrelStats.DamageDodgedByEvasionRoll, damageDodged);
	}
#endif
}
