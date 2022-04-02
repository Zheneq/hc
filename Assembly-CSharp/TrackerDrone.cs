// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class TrackerDrone : Ability
{
	protected TrackerDroneInfoComponent m_droneInfoComp;
	protected TrackerDroneTrackerComponent m_droneTracker;
	protected ActorAdditionalVisionProviders m_visionProvider;
	protected bool m_droneEffectHandled;

	private AbilityMod_TrackerDrone m_abilityMod;

	protected virtual bool UseAltMovement()
	{
		return false;
	}

	public AbilityMod_TrackerDrone GetDroneMod()
	{
		return m_abilityMod;
	}

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Hawk Drone";
		}
		m_droneTracker = GetComponent<TrackerDroneTrackerComponent>();
		if (m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
		}
		m_visionProvider = GetComponent<ActorAdditionalVisionProviders>();
		if (m_visionProvider == null)
		{
			Debug.LogError("No additional vision provider component");
		}
		Setup();
		ResetTooltipAndTargetingNumbers();
	}

	private void Setup()
	{
		if (m_droneInfoComp == null)
		{
			m_droneInfoComp = GetComponent<TrackerDroneInfoComponent>();
		}
		if (m_droneInfoComp == null)
		{
			Debug.LogError("No Drone Info component");
		}
		if (m_droneTracker == null)
		{
			m_droneTracker = GetComponent<TrackerDroneTrackerComponent>();
		}
		if (m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
		}
		bool hitUntrackedTargets = m_droneInfoComp.GetUntrackedHitEffect().m_applyEffect || m_droneInfoComp.GetDamageOnUntracked(true) > 0;
		Targeter = new AbilityUtil_Targeter_TrackerDrone(
			this, m_droneTracker, m_droneInfoComp.m_travelTargeterEndRadius, m_droneInfoComp.m_travelTargeterEndRadius,
			m_droneInfoComp.m_travelTargeterLineRadius, -1, false, m_droneInfoComp.m_targetingIgnoreLos,
			m_droneInfoComp.m_droneTravelHitTargets, hitUntrackedTargets);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TrackerDrone abilityMod_TrackerDrone = modAsBase as AbilityMod_TrackerDrone;
		TrackerDroneInfoComponent component = GetComponent<TrackerDroneInfoComponent>();
		if (component != null)
		{
			tokens.Add(new TooltipTokenInt("DamageOnTracked", "damage on Tracked targets", abilityMod_TrackerDrone != null
				? abilityMod_TrackerDrone.m_trackedHitDamageMod.GetModifiedValue(component.m_droneHitDamageAmount)
				: component.m_droneHitDamageAmount));
			tokens.Add(new TooltipTokenInt("DamageOnUntracked", "damage on Untracked targets", abilityMod_TrackerDrone != null
				? abilityMod_TrackerDrone.m_untrackedHitDamageMod.GetModifiedValue(component.m_untrackedDroneHitDamageAmount)
				: component.m_untrackedDroneHitDamageAmount));
			AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TrackerDrone != null
				? abilityMod_TrackerDrone.m_trackedHitEffectOverride.GetModifiedValue(component.m_droneHitEffect)
				: component.m_droneHitEffect, "EffectOnTracked");
			AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TrackerDrone != null
				? abilityMod_TrackerDrone.m_untrackedHitEffectOverride.GetModifiedValue(component.m_untrackedDroneHitEffect)
				: component.m_untrackedDroneHitEffect, "EffectOnUntracked");
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_droneInfoComp != null)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_droneInfoComp.m_droneHitDamageAmount);
			m_droneInfoComp.m_droneHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_droneInfoComp.m_untrackedDroneHitDamageAmount);
			m_droneInfoComp.m_untrackedDroneHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		}
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_droneInfoComp != null)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_droneInfoComp.GetDamageOnTracked(true));
			m_droneInfoComp.m_droneHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_droneInfoComp.GetDamageOnUntracked(true));
			m_droneInfoComp.m_untrackedDroneHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		}
		return numbers;
	}

	public override List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = new List<int>();
		TrackerDroneInfoComponent component = GetComponent<TrackerDroneInfoComponent>();
		if (component != null)
		{
			list.Add(component.m_droneHitDamageAmount);
			list.Add(component.m_untrackedDroneHitDamageAmount);
		}
		return list;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null
			|| !targetSquare.IsValidForGameplay()
			|| caster.GetCurrentBoardSquare() == null)
		{
			return false;
		}
		float maxMoveDist = m_droneInfoComp.m_targeterMaxRangeFromDrone * Board.Get().squareSize;
		float maxDistFromCaster = m_droneInfoComp.GetTargeterMaxRangeFromCaster(false) * Board.Get().squareSize;
		Vector3 startPos = caster.GetFreePos();
		if (m_droneTracker.DroneIsActive())
		{
			BoardSquare dronePos = Board.Get().GetSquareFromIndex(m_droneTracker.BoardX(), m_droneTracker.BoardY());
			if (dronePos != null)
			{
				if (targetSquare == dronePos)
				{
					return false;
				}
				startPos = dronePos.ToVector3();
			}
		}
		Vector3 casterPos = caster.GetCurrentBoardSquare().ToVector3();
		return (maxMoveDist <= 0f || Vector3.Distance(targetSquare.ToVector3(), startPos) <= maxMoveDist)
			&& (maxDistFromCaster <= 0f || Vector3.Distance(targetSquare.ToVector3(), casterPos) <= maxDistFromCaster);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TrackerDrone))
		{
			m_abilityMod = (abilityMod as AbilityMod_TrackerDrone);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

#if SERVER
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		Vector3 startPoint = caster.GetFreePos();
		TrackerDroneEffect trackerDroneEffect = ServerEffectManager.Get().GetEffect(caster, typeof(TrackerDroneEffect)) as TrackerDroneEffect;
		if (trackerDroneEffect != null)
		{
			startPoint = trackerDroneEffect.TargetSquare.ToVector3();
		}
		bool useMonitorRadiusAtStart = trackerDroneEffect != null;
		List<ActorData> hitActorsInTravel = GetHitActorsInTravel(targets, caster, startPoint, useMonitorRadiusAtStart, out List<ActorData> _, null);
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		if (!UseAltMovement())
		{
			return new ServerClientUtils.SequenceStartData(m_droneInfoComp.m_droneMoveSequence, square, hitActorsInTravel.ToArray(), caster, additionalData.m_sequenceSource, null);
		}
		return new ServerClientUtils.SequenceStartData(m_droneInfoComp.m_droneUltimateMoveSequence, square, hitActorsInTravel.ToArray(), caster, additionalData.m_sequenceSource, null);
	}
#endif

#if SERVER
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		m_droneEffectHandled = false;
		if (ServerEffectManager.Get().GetEffect(caster, typeof(TrackerDroneEffect)) is TrackerDroneEffect trackerDroneEffect)
		{
			ServerEffectManager.Get().RemoveEffect(trackerDroneEffect, ServerEffectManager.Get().GetActorEffects(caster));
		}
		if (caster.GetAdditionalActorVisionProviders().GetVisionProviderInfoOnSatellite(caster.ActorIndex, 0, out VisionProviderInfo visionProviderInfo)
			&& visionProviderInfo.m_radius != m_droneInfoComp.GetVisionRadius())
		{
			caster.GetAdditionalActorVisionProviders().RemoveVisionProviderOnSatellite(caster.ActorIndex, 0);
		}
		if (!caster.GetAdditionalActorVisionProviders().HasVisionProviderOnSatellite(caster.ActorIndex, 0))
		{
			caster.GetAdditionalActorVisionProviders().AddVisionProviderOnSatellite(caster.ActorIndex, 0, m_droneInfoComp.GetVisionRadius(), m_droneInfoComp.m_brushRevealType, BoardSquare.VisibilityFlags.Team);
		}
	}
#endif

#if SERVER
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Vector3 vector = caster.GetFreePos();
		TrackerDroneEffect trackerDroneEffect = ServerEffectManager.Get().GetEffect(caster, typeof(TrackerDroneEffect)) as TrackerDroneEffect;
		if (trackerDroneEffect != null)
		{
			vector = trackerDroneEffect.TargetSquare.ToVector3();
		}
		bool useMonitorRadiusAtStart = trackerDroneEffect != null;
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> list;
		foreach (ActorData actorData in GetHitActorsInTravel(targets, caster, vector, useMonitorRadiusAtStart, out list, nonActorTargetInfo))
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, vector));
			actorHitResults.CanBeReactedTo = m_droneInfoComp.UseDirectDamageForDrone();
			if (list.Contains(actorData))
			{
				actorHitResults.SetBaseDamage(m_droneInfoComp.GetDamageOnTracked(true));
				actorHitResults.AddStandardEffectInfo(m_droneInfoComp.GetTrackedHitEffect());
			}
			else
			{
				actorHitResults.SetBaseDamage(m_droneInfoComp.GetDamageOnUntracked(true));
				actorHitResults.AddStandardEffectInfo(m_droneInfoComp.GetUntrackedHitEffect());
			}
			if (m_droneInfoComp.ShouldAddHuntedEffectFromDrone())
			{
				TrackerHuntedEffect effect = new TrackerHuntedEffect(AsEffectSource(), actorData.GetCurrentBoardSquare(), actorData, caster, m_droneInfoComp.GetHuntedEffectData(), m_droneTracker);
				actorHitResults.AddEffect(effect);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters((square != null) ? square.ToVector3() : targets[0].FreePos));
		TrackerDrone trackerDrone = caster.GetAbilityData().GetAbilityOfType(typeof(TrackerDrone)) as TrackerDrone;
		if (trackerDrone != null)
		{
			TrackerDroneEffect effect2 = new TrackerDroneEffect(trackerDrone.AsEffectSource(), square, caster, caster, m_droneTracker, m_visionProvider, m_droneInfoComp);
			positionHitResults.AddEffect(effect2);
		}
		abilityResults.StorePositionHit(positionHitResults);
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif

#if SERVER
	private List<ActorData> GetHitActorsInTravel(List<AbilityTarget> targets, ActorData caster, Vector3 startPoint, bool useMonitorRadiusAtStart, out List<ActorData> trackedActors, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> list = new List<ActorData>();
		trackedActors = new List<ActorData>();
		if (m_droneInfoComp.m_droneTravelHitTargets)
		{
			Vector3 endPos = Board.Get().GetSquare(targets[0].GridPos).ToVector3();
			float startRadiusInSquares = useMonitorRadiusAtStart ? m_droneInfoComp.m_travelTargeterEndRadius : 0f;
			List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(startPoint, endPos, startRadiusInSquares, m_droneInfoComp.m_travelTargeterEndRadius, m_droneInfoComp.m_travelTargeterLineRadius, m_droneInfoComp.m_targetingIgnoreLos, caster, null, nonActorTargetInfo);
			bool flag = m_droneInfoComp.GetUntrackedHitEffect().m_applyEffect || m_droneInfoComp.GetDamageOnUntracked(true) > 0;
			foreach (ActorData actorData in actorsInRadiusOfLine)
			{
				if (m_droneInfoComp.CanHitInvisibleActors() || CanHitActorByVisibility(actorData, caster))
				{
					bool flag2 = m_droneTracker.IsTrackingActor(actorData.ActorIndex);
					if (actorData.GetTeam() != caster.GetTeam() && (flag || flag2))
					{
						list.Add(actorData);
						if (flag2)
						{
							trackedActors.Add(actorData);
						}
					}
				}
			}
		}
		return list;
	}
#endif

#if SERVER
	private bool CanHitActorByVisibility(ActorData target, ActorData caster)
	{
		return target.IsActorVisibleIgnoringFogOfWar(caster);
	}
#endif

#if SERVER
	public override bool ShouldRevealEffectHolderOnHostileEffectHit()
	{
		return false;
	}
#endif

#if SERVER
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.TrackerStats.DamageDoneByDrone, results.FinalDamage);
		}
	}
#endif

#if SERVER
	public override void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.TrackerStats.DamageDoneByDrone, results.FinalDamage);
		}
	}
#endif

#if SERVER
	public override void OnExecutedActorHit_Barrier(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.TrackerStats.DamageDoneByDrone, results.FinalDamage);
		}
	}
#endif
}
