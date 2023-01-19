// ROGUES
// SERVER

using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class SenseiBideEffect : StandardActorEffect
{
	private float m_explosionRadius;
	private bool m_ignoreLos;
	private int m_maxDamage;
	private int m_baseDamage;
	private float m_damageMultiplier;
	private StandardEffectInfo m_enemyHitEffect;
	private float m_absorbMultForHeal;
	private float m_multOnInitialDamageForSubseqHits;
	private GameObject m_explosionSequencePrefab;
	private int m_damageTakenLastTurn;
	private int m_damageTakenThisTurn;
	private int m_initialDamageAmount;
	private int m_lastGatheredInitialDamage;
	private Passive_Sensei m_passive;

	public SenseiBideEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData data,
		Passive_Sensei passive,
		float explosionRadius,
		bool ignoreLos,
		int maxDamage,
		int baseDamage,
		float damageMult,
		StandardEffectInfo enemyHitEffect,
		float absorbMultForHeal,
		float multOnInitialDamageForSubseqHits,
		GameObject explosionSequencePrefab)
		: base(parent, targetSquare, target, caster, data)
	{
		m_passive = passive;
		m_explosionRadius = explosionRadius;
		m_ignoreLos = ignoreLos;
		m_maxDamage = maxDamage;
		m_baseDamage = baseDamage;
		m_damageMultiplier = damageMult;
		m_enemyHitEffect = enemyHitEffect;
		m_absorbMultForHeal = absorbMultForHeal;
		m_multOnInitialDamageForSubseqHits = multOnInitialDamageForSubseqHits;
		m_explosionSequencePrefab = explosionSequencePrefab;
	}

	public override bool HitsCanBeReactedTo()
	{
		return false;
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		m_damageTakenLastTurn = m_damageTakenThisTurn;
		m_damageTakenThisTurn = 0;
		if (m_time.age == 2)
		{
			m_initialDamageAmount = m_lastGatheredInitialDamage;
		}
		Sensei_SyncComponent component = Caster.GetComponent<Sensei_SyncComponent>();
		if (component != null && m_time.age == 1)
		{
			float num = m_damageMultiplier > 0f ? m_damageMultiplier : 1f;
			int num2 = m_maxDamage - m_baseDamage;
			if (num2 > 0)
			{
				float networkm_syncBideExtraDamagePct = Mathf.Min(m_damageTakenLastTurn * num / num2, 1f);
				component.Networkm_syncBideExtraDamagePct = networkm_syncBideExtraDamagePct;
				return;
			}
			component.Networkm_syncBideExtraDamagePct = 0f;
		}
	}

	protected override void OnDamaged(ActorData damageTarget, ActorData damageCaster, DamageSource damageSource, int damageAmount, ActorHitResults actorHitResults)
	{
		if (Target == damageTarget)
		{
			m_damageTakenThisTurn += damageAmount;
		}
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		foreach (GameObject gameObject in m_data.m_sequencePrefabs)
		{
			if (gameObject != null)
			{
				Sequence.ActorIndexExtraParam actorIndexExtraParam = new Sequence.ActorIndexExtraParam();
				actorIndexExtraParam.m_actorIndex = (short)Caster.ActorIndex;
				list.Add(new ServerClientUtils.SequenceStartData(gameObject, Target.GetFreePos(), Target.AsArray(), Target, SequenceSource, actorIndexExtraParam.ToArray()));
			}
		}
		return list;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		SequenceSource shallowCopy = SequenceSource.GetShallowCopy();
		shallowCopy.SetWaitForClientEnable(true);
		Sequence.ActorIndexExtraParam actorIndexExtraParam = new Sequence.ActorIndexExtraParam
		{
			m_actorIndex = (short)Caster.ActorIndex
		};
		list.Add(new ServerClientUtils.SequenceStartData(
			m_explosionSequencePrefab,
			Target.GetCurrentBoardSquare(),
			m_effectResults.HitActorsArray(),
			Target,
			shallowCopy,
			actorIndexExtraParam.ToArray()));
		if (m_time.age + 1 >= m_time.duration)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				SequenceLookup.Get().GetSimpleHitSequencePrefab(),
				Vector3.one,
				null,
				Target,
				shallowCopy));
		}
		return list;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_time.age > 0)
		{
			List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
			List<ActorData> hitActors = GetHitActors(nonActorTargetInfo);
			int num2;
			if (m_time.age == 1)
			{
				int damageTakenLastTurn = m_damageTakenLastTurn;
				float num = (m_damageMultiplier > 0f) ? m_damageMultiplier : 1f;
				num2 = m_baseDamage + Mathf.RoundToInt(num * damageTakenLastTurn);
				if (m_maxDamage > 0 && num2 > m_maxDamage)
				{
					num2 = m_maxDamage;
				}
				if (isReal)
				{
					m_lastGatheredInitialDamage = num2;
				}
			}
			else
			{
				num2 = Mathf.RoundToInt(m_initialDamageAmount * m_multOnInitialDamageForSubseqHits);
				num2 = Mathf.Max(0, num2);
			}
			foreach (ActorData target in hitActors)
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, Target.GetFreePos()));
				actorHitResults.SetBaseDamage(num2);
				actorHitResults.AddStandardEffectInfo(m_enemyHitEffect);
				effectResults.StoreActorHit(actorHitResults);
			}
			effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
			ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(Target, Target.GetFreePos()));
			int num3 = Mathf.RoundToInt(m_absorbMultForHeal * m_passive.m_bideAbsorbRemainingOnEnd);
			if (num3 > 0)
			{
				actorHitResults2.SetBaseHealing(num3);
			}
			effectResults.StoreActorHit(actorHitResults2);
			if (m_time.age + 1 >= m_time.duration)
			{
				PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(Vector3.one));
				if (m_data.m_sequencePrefabs != null && m_data.m_sequencePrefabs.Length != 0)
				{
					for (int i = 0; i < m_data.m_sequencePrefabs.Length; i++)
					{
						if (m_data.m_sequencePrefabs[i] != null)
						{
							positionHitResults.AddEffectSequenceToEnd(m_data.m_sequencePrefabs[i], m_guid);
						}
					}
				}
				effectResults.StorePositionHit(positionHitResults);
			}
		}
	}

	private List<ActorData> GetHitActors(List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return AreaEffectUtils.GetActorsInRadius(Target.GetFreePos(), m_explosionRadius, m_ignoreLos, Caster, Caster.GetOtherTeams(), nonActorTargetInfo);
	}

	public override ActorData GetActorAnimationActor()
	{
		return Target;
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return true;
	}
}
#endif
