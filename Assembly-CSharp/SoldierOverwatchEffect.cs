// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class SoldierOverwatchEffect : Effect
{
	private bool m_useCone;
	private ConeTargetingInfo m_coneInfo;
	private LaserTargetingInfo m_laserInfo;
	private Vector3 m_startPos;
	private float m_facingHorizontalAngle;
	private Vector3 m_facingDir;
	private int m_damageAmount;
	private StandardEffectInfo m_enemyHitEffect;
	private float m_nearTargetDist;
	private int m_extraDamageOnNearTargets;
	private int m_extraDamageForEvaders;
	private int m_extraEnergyPerConeHit;
	private int m_extraEnergyPerLaserHit;
	private int m_coneTriggerAnimIndex;
	private int m_laserTriggerAnimIndex;
	private int m_cinematicRequested;
	private StandardEffectInfo m_stimPackExtraEffect;
	private GameObject m_coneSequencePrefab;
	private GameObject m_laserSequencePrefab;
	private AbilityData m_abilityData;

	public SoldierOverwatchEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData caster,
		bool useCone,
		ConeTargetingInfo coneInfo,
		LaserTargetingInfo laserInfo,
		Vector3 startPos,
		Vector3 facingDir,
		int damageAmount,
		StandardEffectInfo enemyHitEffect,
		float nearTargetDist,
		int extraDamageOnNearTargets,
		int extraDamageForEvaders,
		int extraEnergyPerConeHit,
		int extraEnergyPerLaserHit,
		AbilityPriority hitPhase,
		int coneTriggerAnimIndex,
		int laserTriggerAnimIndex,
		int cinematicsRequested,
		StandardEffectInfo stimPackExtraEffect,
		GameObject coneSequencePrefab,
		GameObject laserSequencePrefab)
		: base(parent, targetSquare, caster, caster)
	{
		m_useCone = useCone;
		m_coneInfo = coneInfo;
		m_laserInfo = laserInfo;
		m_startPos = startPos;
		m_startPos.y = Board.Get().LosCheckHeight;
		m_facingHorizontalAngle = VectorUtils.HorizontalAngle_Deg(facingDir);
		m_facingDir = facingDir;
		m_damageAmount = damageAmount;
		m_enemyHitEffect = enemyHitEffect;
		m_nearTargetDist = nearTargetDist;
		m_extraDamageOnNearTargets = extraDamageOnNearTargets;
		m_extraDamageForEvaders = extraDamageForEvaders;
		m_extraEnergyPerConeHit = extraEnergyPerConeHit;
		m_extraEnergyPerLaserHit = extraEnergyPerLaserHit;
		m_coneTriggerAnimIndex = coneTriggerAnimIndex;
		m_laserTriggerAnimIndex = laserTriggerAnimIndex;
		m_cinematicRequested = cinematicsRequested;
		m_stimPackExtraEffect = stimPackExtraEffect;
		m_coneSequencePrefab = coneSequencePrefab;
		m_laserSequencePrefab = laserSequencePrefab;
		m_time.duration = 1;
		HitPhase = hitPhase;
		m_abilityData = caster.GetAbilityData();
	}

	public override bool HitsCanBeReactedTo()
	{
		return true;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		ActorData[] targetActorArray = m_effectResults.HitActorsArray();
		SequenceSource sequence = SequenceSource.GetShallowCopy();
		if (GetCasterAnimationIndex(HitPhase) > 0)
		{
			sequence.SetWaitForClientEnable(true);
		}
		GetHitActors(null, out Vector3 targetPos);
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_useCone
					? m_coneSequencePrefab
					: m_laserSequencePrefab,
				targetPos,
				targetActorArray,
				Caster,
				sequence,
				new BlasterStretchConeSequence.ExtraParams
				{
					forwardAngle = VectorUtils.HorizontalAngle_Deg(m_facingDir),
					angleInDegrees = m_coneInfo.m_widthAngleDeg,
					lengthInSquares = m_coneInfo.m_radiusInSquares
				}.ToArray())
		};
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (!CanTriggerHits())
		{
			return;
		}
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(nonActorTargetInfo, out _);
		bool applyEnemyHitEffect = m_abilityData != null
		            && m_abilityData.HasQueuedAbilityOfType(typeof(SoldierStimPack)) // , true in rogues
		            && m_stimPackExtraEffect != null
		            && m_stimPackExtraEffect.m_applyEffect;
		foreach (ActorData actorData in hitActors)
		{
			if (actorData.GetTeam() != Caster.GetTeam())
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, m_startPos));
				int damage = m_damageAmount;
				if (m_extraDamageOnNearTargets > 0 && m_nearTargetDist > 0f)
				{
					Vector3 vector2 = Caster.GetFreePos() - actorData.GetFreePos();
					vector2.y = 0f;
					if (vector2.magnitude <= m_nearTargetDist * Board.Get().squareSize)
					{
						damage += m_extraDamageOnNearTargets;
					}
				}
				if (m_extraDamageForEvaders > 0 && ServerActionBuffer.Get().ActorIsEvading(actorData))
				{
					damage += m_extraDamageForEvaders;
				}
				actorHitResults.SetBaseDamage(damage);
				actorHitResults.AddStandardEffectInfo(m_enemyHitEffect);
				if (applyEnemyHitEffect)
				{
					actorHitResults.AddStandardEffectInfo(m_stimPackExtraEffect);
				}
				if (m_useCone && m_extraEnergyPerConeHit > 0)
				{
					actorHitResults.AddTechPointGainOnCaster(m_extraEnergyPerConeHit);
				}
				else if (!m_useCone && m_extraEnergyPerLaserHit > 0)
				{
					actorHitResults.AddTechPointGainOnCaster(m_extraEnergyPerLaserHit);
				}
				effectResults.StoreActorHit(actorHitResults);
			}
		}
		effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	private List<ActorData> GetHitActors(List<NonActorTargetInfo> nonActorTargetInfo, out Vector3 endPosIfLaser)
	{
		if (m_useCone)
		{
			endPosIfLaser = Vector3.zero;
			return AreaEffectUtils.GetActorsInCone(
				m_startPos,
				m_facingHorizontalAngle,
				m_coneInfo.m_widthAngleDeg,
				m_coneInfo.m_radiusInSquares,
				m_coneInfo.m_backwardsOffset,
				m_coneInfo.m_penetrateLos,
				Caster,
				Caster.GetOtherTeams(),
				nonActorTargetInfo);
		}
		else
		{
			return AreaEffectUtils.GetActorsInLaser(
				m_startPos,
				m_facingDir,
				m_laserInfo.range,
				m_laserInfo.width,
				Caster,
				Caster.GetOtherTeams(),
				m_laserInfo.penetrateLos,
				m_laserInfo.maxTargets,
				false,
				true,
				out endPosIfLaser,
				nonActorTargetInfo);
		}
	}

	private bool CanTriggerHits()
	{
		return !Caster.IsDead()
		       && Caster.GetCurrentBoardSquare() != null
		       && (Caster.GetCurrentBoardSquare() == TargetSquare || GameFlowData.Get().gameState == GameState.BothTeams_Decision);
	}

	public override void OnActorAnimEntryPlay()
	{
		base.OnActorAnimEntryPlay();
		if (BrushCoordinator.Get() != null && Caster != null && Caster.IsInBrush())
		{
			int brushRegion = Caster.GetBrushRegion();
			BrushCoordinator.Get().DisruptBrush(brushRegion);
		
			// custom
			Log.Info($"Brush {brushRegion} disrupted because {Caster.DisplayName} {Caster.GetTravelBoardSquare().GetGridPos()} used OverwatchEffect");
		}
	}

	public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
	{
		if (phaseIndex != HitPhase)
		{
			return 0;
		}
		return m_useCone ? m_coneTriggerAnimIndex : m_laserTriggerAnimIndex;
	}

	public override int GetCinematicRequested(AbilityPriority phaseIndex)
	{
		return m_cinematicRequested;
	}

	public override Vector3 GetRotationTargetPos(AbilityPriority phaseIndex)
	{
		return Caster.GetFreePos() + m_facingDir;
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly() || !CanTriggerHits();
	}
}
#endif
