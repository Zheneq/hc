// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SamuraiAfterimageStrike : Ability
{
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	[Header("-- for removing afterimage --")]
	public GameObject m_selfHitSequencePrefab;
	public ActorModelData.ActionAnimationType m_afterImageAnim = ActorModelData.ActionAnimationType.Ability1;

	private SamuraiSwordDash m_parentAbility;
	private Samurai_SyncComponent m_syncComp;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "AfterimageStrike (intended as a chain ability)";
		}
		m_syncComp = ActorData.GetComponent<Samurai_SyncComponent>();
		m_parentAbility = GetAbilityOfType<SamuraiSwordDash>();
	}

	public int GetDamageAmount()
	{
		return m_parentAbility != null
			? m_parentAbility.GetKnockbackDamage()
			: 0;
	}

	public int GetLessDamagePerTarget()
	{
		return m_parentAbility != null
			? m_parentAbility.GetKnockbackLessDamagePerTarget()
			: 0;
	}

	public float GetKnockbackDist()
	{
		return m_parentAbility != null
			? m_parentAbility.GetKnockbackDist()
			: 0f;
	}

	public KnockbackType GetKnockbackType()
	{
		return m_parentAbility != null
			? m_parentAbility.GetKnockbackType()
			: KnockbackType.AwayFromSource;
	}

	public float GetExtraDamageFromDamageTakenMult()
	{
		return m_parentAbility != null
			? m_parentAbility.GetKnockbackExtraDamageFromDamageTakenMult()
			: 0f;
	}
	
#if SERVER
	// added in rogues
	public override List<int> GetAdditionalBrushRegionsToDisrupt(ActorData caster, List<AbilityTarget> targets)
	{
		List<int> list = new List<int>();
		if (!FindDamageHitActors(targets, caster, null).IsNullOrEmpty() && caster.GetBrushRegion() >= 0)
		{
			list.Add(caster.GetBrushRegion());
		}
		return list;
	}

	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		base.Run(targets, caster, additionalData);
		if (m_syncComp != null && m_syncComp.m_afterimageVfxEffect != null)
		{
			m_syncComp.m_afterimageVfxEffect = null;
		}
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> result = new List<ServerClientUtils.SequenceStartData>();
		if (m_syncComp != null && m_syncComp.m_afterimageVfxEffect != null)
		{
			result.Add(new ServerClientUtils.SequenceStartData(
				m_selfHitSequencePrefab,
				caster.GetFreePos(),
				new[] { caster },
				caster,
				additionalData.m_sequenceSource,
				new Sequence.IExtraSequenceParams[0]));
		}
		List<ActorData> hitActors = additionalData.m_abilityResults.HitActorList();
		hitActors.Remove(caster);
		Vector3 afterimagePos = m_syncComp.m_afterimagePosition.ToVector3();
		Vector3 aimDir = targets[0].AimDirection;
		if (caster.GetCurrentBoardSquare() != null && caster.GetCurrentBoardSquare() != m_syncComp.m_afterimagePosition)
		{
			aimDir = caster.GetCurrentBoardSquare().ToVector3() - afterimagePos;
		}
		result.Add(new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			caster.GetFreePos(),
			Quaternion.LookRotation(aimDir),
			hitActors.ToArray(),
			caster,
			additionalData.m_sequenceSource,
			new Sequence.IExtraSequenceParams[]
			{
				new SplineProjectileSequence.DelayedProjectileExtraParams
				{
					overrideStartPos = afterimagePos,
					useOverrideStartPos = true
				},
				new SimpleVFXAtTargetPosSequence.PositionOverrideParam
				{
					m_positionOverride = afterimagePos
				}
			}));
		return result;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> actorsHitInEvade = FindAllActorsHitInEvade(targets, caster, nonActorTargetInfo);
		List<ActorData> damageHitActors = FindDamageHitActors(targets, caster, nonActorTargetInfo);
		int damage = GetDamageAmount() - (damageHitActors.Count - 1) * GetLessDamagePerTarget();
		damage = Mathf.Max(0, damage);
		if (m_parentAbility != null)
		{
			BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
			damage += m_parentAbility.CalcExtraDamageForDashDist(ActorData.SquareAtResolveStart, square);
			damage = Mathf.Max(0, damage);
		}
		if (m_syncComp != null)
		{
			damage += m_syncComp.GetExtraDamageFromQueuedSelfBuff();
		}
		Vector3 vector = m_syncComp.m_afterimagePosition.ToVector3();
		Vector3 aimDir = caster.GetFreePos() - vector;
		foreach (ActorData actorData in actorsHitInEvade)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, vector));
			if (damageHitActors.Contains(actorData))
			{
				int totalDamage = damage;
				if (GameFlowData.Get().IsInResolveState() && GetExtraDamageFromDamageTakenMult() > 0f)
				{
					int unresolvedDamage = actorData.UnresolvedDamage;
					int healing = 0;
					ServerActionBuffer.Get().CountDamageAndHealFromGatheredResults(AbilityPriority.Combat_Damage, actorData, ref unresolvedDamage, ref healing);
					totalDamage += Mathf.RoundToInt(unresolvedDamage * GetExtraDamageFromDamageTakenMult());
				}
				actorHitResults.SetBaseDamage(totalDamage);
				actorHitResults.AddEffectForRemoval(ServerEffectManager.Get().GetEffect(actorData, typeof(SamuraiMarkEffect)));
			}
			if (GetKnockbackDist() != 0f)
			{
				KnockbackHitData knockbackData = new KnockbackHitData(actorData, caster, GetKnockbackType(), aimDir, vector, GetKnockbackDist());
				actorHitResults.AddKnockbackData(knockbackData);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		if (m_syncComp != null && m_syncComp.m_afterimageVfxEffect != null)
		{
			ActorHitResults casterHitResults = MakeActorHitRes(caster, caster.GetFreePos());
			casterHitResults.AddEffectForRemoval(m_syncComp.m_afterimageVfxEffect);
			abilityResults.StoreActorHit(casterHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> FindDamageHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> list = new List<ActorData>();
		foreach (Effect effect in ServerEffectManager.Get().GetAllActorEffectsByCaster(caster, typeof(SamuraiMarkEffect)))
		{
			if (effect != null && effect.Target != null)
			{
				list.Add(effect.Target);
			}
		}
		return list;
	}

	// added in rogues
	private List<ActorData> FindAllActorsHitInEvade(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		if (m_parentAbility != null)
		{
			List<ActorData> list = new List<ActorData>(m_parentAbility.GetLastGatheredHitActors());
			foreach (ActorData item in FindDamageHitActors(targets, caster, nonActorTargetInfo))
			{
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
			return list;
		}
		return FindDamageHitActors(targets, caster, nonActorTargetInfo);
	}

	// added in rogues
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = base.CalcPointsOfInterestForCamera(targets, caster);
		if (m_syncComp != null && m_syncComp.m_afterimageX > 0 && m_syncComp.m_afterimageY > 0)
		{
			list.Add(Board.Get().GetSquareFromIndex(m_syncComp.m_afterimageX, m_syncComp.m_afterimageY).ToVector3());
		}
		return list;
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SamuraiStats.DamageDealtAndDodged_Ult, results.FinalDamage);
		}
	}
#endif
}
