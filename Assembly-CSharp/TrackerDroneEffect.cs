// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
public class TrackerDroneEffect : Effect
{
	private ActorAdditionalVisionProviders m_visionProviders;
	private TrackerDroneTrackerComponent m_droneTrackerComponent;
	private TrackerDroneInfoComponent m_droneInfoComp;
	private float m_radiusUsedForVisionProvider;

	public TrackerDroneEffect(EffectSource parent, BoardSquare targetSquare, ActorData target, ActorData caster, TrackerDroneTrackerComponent droneTrackerComponent, ActorAdditionalVisionProviders visionProviders, TrackerDroneInfoComponent droneInfoComp) : base(parent, targetSquare, target, caster)
	{
		m_effectName = "Tracker Drone Effect";
		m_time.duration = 0;
		m_droneTrackerComponent = droneTrackerComponent;
		m_visionProviders = visionProviders;
		m_droneInfoComp = droneInfoComp;
		HitPhase = m_droneInfoComp.m_droneMonitorPhase;
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(m_droneInfoComp.m_droneRadiusSequencePrefab, TargetSquare, null, Caster, SequenceSource, null);
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (!ServerActionBuffer.Get().HasUnresolvedAbilityRequestOfType(Caster, typeof(TrackerDrone))
			&& !ServerActionBuffer.Get().HasUnresolvedAbilityRequestOfType(Caster, typeof(TrackerTeslaPrison))
			&& m_time.age >= m_droneInfoComp.m_droneMonitorStartDelay)
		{
			SequenceSource sequenceSource = SequenceSource;
			if (AddActorAnimEntryIfHasHits(HitPhase))
			{
				sequenceSource = SequenceSource.GetShallowCopy();
				sequenceSource.SetWaitForClientEnable(true);
			}
			foreach (ActorData actorData in GetHitActorsInMonitorArea(out List<ActorData> _, null))
			{
				list.Add(new ServerClientUtils.SequenceStartData(m_droneInfoComp.m_missileSequencePrefab, actorData.GetFreePos(), actorData.AsArray(), Caster, sequenceSource, null));
			}
		}
		return list;
	}

	public override void OnStart()
	{
		if (m_droneTrackerComponent != null)
		{
			m_droneTrackerComponent.UpdateDroneBoardPos(TargetSquare.x, TargetSquare.y);
			m_droneTrackerComponent.UpdateDroneActiveFlag(true);
			if (m_visionProviders != null)
			{
				m_radiusUsedForVisionProvider = m_droneInfoComp.GetVisionRadius();
				m_visionProviders.AddVisionProviderOnGridPos(TargetSquare.GetGridPos(), m_radiusUsedForVisionProvider, m_droneInfoComp.m_brushRevealType, false, BoardSquare.VisibilityFlags.Team);
			}
		}
	}

	public override void OnEnd()
	{
		if (m_droneTrackerComponent != null)
		{
			m_droneTrackerComponent.UpdateDroneActiveFlag(false);
			if (m_visionProviders != null)
			{
				m_visionProviders.RemoveVisionProviderOnGridPos(TargetSquare.GetGridPos(), m_radiusUsedForVisionProvider, m_droneInfoComp.m_brushRevealType, false, BoardSquare.VisibilityFlags.Team);
			}
		}
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (!ServerActionBuffer.Get().HasUnresolvedAbilityRequestOfType(Caster, typeof(TrackerDrone)) && !ServerActionBuffer.Get().HasUnresolvedAbilityRequestOfType(Caster, typeof(TrackerTeslaPrison)) && m_time.age >= m_droneInfoComp.m_droneMonitorStartDelay)
		{
			List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
			foreach (ActorData actorData in GetHitActorsInMonitorArea(out List<ActorData> _, nonActorTargetInfo))
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, TargetSquare.ToVector3()))
				{
					CanBeReactedTo = m_droneInfoComp.UseDirectDamageForDrone()
				};
				if (m_droneTrackerComponent.IsTrackingActor(actorData.ActorIndex))
				{
					actorHitResults.AddBaseDamage(m_droneInfoComp.GetDamageOnTracked(false));
					actorHitResults.AddStandardEffectInfo(m_droneInfoComp.GetTrackedHitEffect());
				}
				else
				{
					actorHitResults.AddBaseDamage(m_droneInfoComp.GetDamageOnUntracked(false));
					actorHitResults.AddStandardEffectInfo(m_droneInfoComp.GetUntrackedHitEffect());
				}
				if (m_droneInfoComp.ShouldAddHuntedEffectFromDrone())
				{
					TrackerHuntedEffect effect = new TrackerHuntedEffect(Parent, actorData.GetCurrentBoardSquare(), actorData, Caster, m_droneInfoComp.GetHuntedEffectData(), m_droneTrackerComponent);
					actorHitResults.AddEffect(effect);
				}
				effectResults.StoreActorHit(actorHitResults);
			}
			effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		}
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return true;
	}

	public override bool ShouldEndEarly()
	{
		return Caster.IsDead();
	}

	private List<ActorData> GetHitActorsInMonitorArea(out List<ActorData> trackedActors, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> list = new List<ActorData>();
		trackedActors = new List<ActorData>();
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(m_droneInfoComp.m_droneMonitorShape, TargetSquare.ToVector3(), TargetSquare, m_droneInfoComp.m_targetingIgnoreLos, Caster, Caster.GetOtherTeams(), nonActorTargetInfo);
		bool isHittingUntracked = m_droneInfoComp.m_hitUntrackedWhenStationary
			&& (m_droneInfoComp.GetDamageOnUntracked(false) > 0 || m_droneInfoComp.GetUntrackedHitEffect().m_applyEffect);
		foreach (ActorData actorData in actorsInShape)
		{
			if (m_droneInfoComp.CanHitInvisibleActors() || CanHitActorByVisibility(actorData, Caster))
			{
				bool isTrackingActor = m_droneTrackerComponent.IsTrackingActor(actorData.ActorIndex);
				if (isHittingUntracked || isTrackingActor)
				{
					list.Add(actorData);
					if (isTrackingActor)
					{
						trackedActors.Add(actorData);
					}
				}
			}
		}
		return list;
	}

	private bool CanHitActorByVisibility(ActorData target, ActorData caster)
	{
		return target.IsActorVisibleIgnoringFogOfWar(caster);
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != Caster.GetTeam())
		{
			List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(m_droneInfoComp.m_droneMonitorShape, TargetSquare.ToVector3(), TargetSquare, true, Caster);
			squaresToAvoid.UnionWith(squaresInShape);
		}
	}

	public override void DrawGizmos()
	{
		Gizmos.color = Color.green;
		if (TargetSquare != null)
		{
			foreach (BoardSquare boardSquare in AreaEffectUtils.GetSquaresInShape(m_droneInfoComp.m_droneMonitorShape, TargetSquare.ToVector3(), TargetSquare, true, Caster))
			{
				Gizmos.DrawCube(boardSquare.ToVector3(), 0.5f * Vector3.one);
			}
		}
	}
}
#endif
