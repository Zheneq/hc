// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
public class BlasterDelayedLaserEffect : Effect
{
	public AbilityTarget m_target;
	public float m_laserLengthSquares;
	public float m_laserWidthSquares;
	public bool m_penetrateLoS;
	public bool m_aimAtCaster;
	
	private int m_turnsBeforeTriggering;
	private bool m_remoteTriggerMode;

	public int m_damage;
	public int m_overchargeExtraDamage;
	public int m_extraDamageForNearEnemy;
	public float m_nearDistance;
	public StandardEffectInfo m_effectOnHit;

	private GameObject m_groundSequencePrefab;
	private GameObject m_triggerSequencePrefab;
	private GameObject m_triggerOnCasterSequencePrefab;
	private int m_triggerAnimationIndex;
	private Vector3 m_laserStartPos;
	private Blaster_SyncComponent m_syncComp;

	public BlasterDelayedLaserEffect(EffectSource parent, ActorData caster, AbilityTarget target, float laserLengthSquares, float laserWidthSquares, bool penetrateLoS, bool aimAtCaster, int turnsBeforeTriggering, bool remoteTriggerMode, int damage, int overchargeExtraDamage, int extraDamageForNearEnemy, float nearDistance, StandardEffectInfo effectOnHit, GameObject groundSequencePrefab, GameObject triggerSequencePrefab, GameObject triggerOnCasterSequencePrefab, int triggerAnimationIndex, SequenceSource parentSequenceSource)
		: base(parent, Board.Get().GetSquare(target.GridPos), null, caster)
	{
		m_effectName = parent.Ability.m_abilityName;
		m_target = target;
		m_laserLengthSquares = laserLengthSquares;
		m_laserWidthSquares = laserWidthSquares;
		m_penetrateLoS = penetrateLoS;
		m_aimAtCaster = aimAtCaster;
		m_turnsBeforeTriggering = turnsBeforeTriggering;
		m_remoteTriggerMode = remoteTriggerMode;
		m_damage = damage;
		m_overchargeExtraDamage = overchargeExtraDamage;
		m_extraDamageForNearEnemy = extraDamageForNearEnemy;
		m_nearDistance = nearDistance;
		m_effectOnHit = effectOnHit;
		m_groundSequencePrefab = groundSequencePrefab;
		m_triggerSequencePrefab = triggerSequencePrefab;
		m_triggerOnCasterSequencePrefab = triggerOnCasterSequencePrefab;
		m_triggerAnimationIndex = triggerAnimationIndex;
		m_laserStartPos = caster.GetLoSCheckPos();
		HitPhase = AbilityPriority.Combat_Damage;
		m_time.duration = turnsBeforeTriggering + 1;
		SequenceSource = new SequenceSource(OnHit_Base, OnHit_Base, false);
	}

	public override void OnStart()
	{
		if (m_remoteTriggerMode)
		{
			m_syncComp = Caster.GetComponent<Blaster_SyncComponent>();
			if (m_syncComp != null)
			{
				m_syncComp.Networkm_canActivateDelayedLaser = true;
				m_syncComp.Networkm_delayedLaserStartPos = m_laserStartPos;
				m_syncComp.Networkm_delayedLaserAimDir = m_target.AimDirection;
			}
		}
	}

	public override void OnEnd()
	{
		if (m_remoteTriggerMode && m_syncComp != null)
		{
			m_syncComp.Networkm_canActivateDelayedLaser = false;
		}
	}

	private Vector3 GetLaserAimDir()
	{
		if (m_aimAtCaster && !Caster.IsDead() && Caster.GetCurrentBoardSquare() != null)
		{
			Vector3 result = Caster.GetCurrentBoardSquare().ToVector3() - m_laserStartPos;
			result.y = 0f;
			result.Normalize();
			if (result.magnitude > 0f)
			{
				return result;
			}
		}
		return m_target.AimDirection;
	}

	private Vector3 GetLaserEndPos()
	{
		float num = m_laserLengthSquares * Board.Get().squareSize;
		Vector3 laserAimDir = GetLaserAimDir();
		return m_laserStartPos + laserAimDir * num;
	}

	public List<ActorData> FindHitActors(List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return AreaEffectUtils.GetActorsInLaser(m_laserStartPos, GetLaserAimDir(), m_laserLengthSquares, m_laserWidthSquares, Caster, Caster.GetOtherTeams(), m_penetrateLoS, -1, false, true, out Vector3 _, nonActorTargetInfo);
	}

	private bool AmDetonatingThisTurn()
	{
		bool flag = m_time.age >= m_turnsBeforeTriggering;
		if (m_remoteTriggerMode && m_time.age > 0)
		{
			bool flag2 = ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(BlasterDelayedLaser));
			flag = flag || flag2;
		}
		return flag;
	}

	private bool AmQueuedToRemoteDetonate()
	{
		return m_remoteTriggerMode
		       && m_time.age > 0
		       && ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(BlasterDelayedLaser));
	}

	public override bool ShouldEndEarly()
	{
		AbilityPriority abilityPhase = ServerActionBuffer.Get().AbilityPhase;
		return (AmDetonatingThisTurn() && abilityPhase > HitPhase) || base.ShouldEndEarly();
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		bool amQueuedToRemoteDetonate = AmQueuedToRemoteDetonate();
		bool cooldownOverrideApplied = false;
		if (AmDetonatingThisTurn())
		{
			List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
			List<ActorData> hitActors = FindHitActors(nonActorTargetInfo);
			int baseDamage = m_damage;
			if (m_syncComp != null
			    // custom
			    && m_syncComp.m_overchargeBuffs > 0
			    // rogues
			    // && m_syncComp.m_overchargeCount > 0
			    && m_overchargeExtraDamage > 0)
			{
				baseDamage += m_overchargeExtraDamage;
			}
			float nearDistance = m_nearDistance * Board.Get().squareSize;
			foreach (ActorData actorData in hitActors)
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, m_laserStartPos));
				int damage = baseDamage;
				if (m_extraDamageForNearEnemy > 0 && nearDistance > 0f)
				{
					Vector3 vector = actorData.GetFreePos() - m_laserStartPos;
					vector.y = 0f;
					if (vector.magnitude <= nearDistance)
					{
						damage += m_extraDamageForNearEnemy;
					}
				}
				actorHitResults.AddBaseDamage(damage);
				actorHitResults.AddStandardEffectInfo(m_effectOnHit);
				if (actorData == Caster && !amQueuedToRemoteDetonate)
				{
					AddCooldownOverrideToActorHit(ref actorHitResults);
					cooldownOverrideApplied = true;
				}
				effectResults.StoreActorHit(actorHitResults);
			}
			ActorHitResults hitResults = new ActorHitResults(new ActorHitParameters(Caster, m_laserStartPos));
			if (!cooldownOverrideApplied && !amQueuedToRemoteDetonate)
			{
				AddCooldownOverrideToActorHit(ref hitResults);
				cooldownOverrideApplied = true;
			}
			effectResults.StoreActorHit(hitResults);
			PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(m_laserStartPos));
			positionHitResults.AddEffectSequenceToEnd(m_groundSequencePrefab, m_guid);
			effectResults.StorePositionHit(positionHitResults);
			effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		}
	}

	private void AddCooldownOverrideToActorHit(ref ActorHitResults casterHitResults)
	{
		Ability ability = Parent.Ability;
		if (ability != null)
		{
			int age = m_time.age;
			int overrideValue = ability.GetModdedCooldown() - age;
			MiscHitEventData_OverrideCooldown hitEvent = new MiscHitEventData_OverrideCooldown(Caster.GetAbilityData().GetActionTypeOfAbility(ability), overrideValue);
			casterHitResults.AddMiscHitEvent(hitEvent);
		}
	}

	public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
	{
		if (!AmDetonatingThisTurn() || phaseIndex != HitPhase || Caster.IsDead())
		{
			return 0;
		}
		return m_triggerAnimationIndex;
	}

	public override int GetCinematicRequested(AbilityPriority phaseIndex)
	{
		if (!AmDetonatingThisTurn() || phaseIndex != HitPhase || m_syncComp == null)
		{
			return -1;
		}
		return m_syncComp.m_lastCinematicRequested;
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(
			m_groundSequencePrefab,
			m_laserStartPos,
			Quaternion.LookRotation(GetLaserAimDir()),
			null,
			Caster,
			SequenceSource,
			new Sequence.IExtraSequenceParams[]
			{
				new HealLaserSequence.ExtraParams
				{
					endPos = GetLaserEndPos()
				}
			});
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (AmDetonatingThisTurn())
		{
			HealLaserSequence.ExtraParams extraParams = new HealLaserSequence.ExtraParams
			{
				endPos = GetLaserEndPos()
			};
			SequenceSource shallowCopy = SequenceSource.GetShallowCopy();
			if (GetCasterAnimationIndex(HitPhase) > 0 || AddActorAnimEntryIfHasHits(HitPhase))
			{
				shallowCopy.SetWaitForClientEnable(true);
			}
			ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(m_triggerSequencePrefab, m_laserStartPos, Quaternion.LookRotation(GetLaserAimDir()), m_effectResults.HitActorsArray(), Caster, shallowCopy, new Sequence.IExtraSequenceParams[]
			{
				extraParams
			});
			list.Add(item);
			if (GetCasterAnimationIndex(HitPhase) > 0)
			{
				ServerClientUtils.SequenceStartData item2 = new ServerClientUtils.SequenceStartData(m_triggerOnCasterSequencePrefab, Caster.GetFreePos(), Caster.AsArray(), Caster, shallowCopy);
				list.Add(item2);
			}
		}
		return list;
	}

	public override List<Vector3> CalcPointsOfInterestForCamera()
	{
		return new List<Vector3>
		{
			m_laserStartPos,
			GetLaserEndPos()
		};
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return AmDetonatingThisTurn() && phaseIndex == HitPhase;
	}

	public override ActorData GetActorAnimationActor()
	{
		if (Caster.IsDead())
		{
			foreach (ActorData actorData in m_effectResults.HitActorsArray())
			{
				if (actorData != null && !actorData.IsDead())
				{
					return actorData;
				}
			}
		}
		return Caster;
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != Caster.GetTeam())
		{
			List<BoardSquare> squaresInBoxByActorRadius = AreaEffectUtils.GetSquaresInBoxByActorRadius(m_laserStartPos, m_laserStartPos + m_laserLengthSquares * Board.Get().squareSize * GetLaserAimDir(), m_laserWidthSquares, true, Caster);
			squaresToAvoid.UnionWith(squaresInBoxByActorRadius);
		}
	}
}
#endif
