// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server-only, added in rogues
#if SERVER
public class FishManGeyserEffect : Effect
{
	private AbilityTarget m_abilityTarget;
	
	public int m_turnsTillFirstExplosion = 1;
	public int m_numExplosionsBeforeEnding = 1;
	public AbilityAreaShape m_explodeShape = AbilityAreaShape.Seven_x_Seven_NoCorners;
	public bool m_explodePenetratesLoS;
	public int m_damageToEnemiesOnExplode = 30;
	public int m_healingToAlliesOnExplode;
	public bool m_applyKnockbackOnExplode = true;
	public float m_knockbackDistOnExplode = 4f;
	public KnockbackType m_knockbackTypeOnExplode = KnockbackType.AwayFromSource;
	public StandardEffectInfo m_effectToEnemiesOnExplode;
	public StandardEffectInfo m_effectToAlliesOnExplode;
	public GameObject m_hittingEnemySequence;
	public GameObject m_hittingAllySequence;
	public GameObject m_groundEffectPersistentSequence;
	public GameObject m_groundEffectExplodeSequence;

	public FishManGeyserEffect(
		EffectSource parent,
		ActorData caster,
		AbilityTarget abilityTarget,
		AbilityAreaShape explodeShape,
		bool explodePenetratesLoS,
		int damageToEnemiesOnExplode,
		int healingToAlliesOnExplode,
		bool applyKnockbackOnExplode,
		float knockbackDistOnExplode,
		KnockbackType knockbackTypeOnExplode,
		StandardEffectInfo effectToEnemiesOnExplode,
		StandardEffectInfo effectToAlliesOnExplode,
		int turnsTillFirstExplosion,
		int numExplosionsBeforeEnding,
		GameObject hittingEnemySequence,
		GameObject hittingAllySequence,
		GameObject groundEffectPersistentSequence,
		GameObject groundEffectExplodeSequence,
		SequenceSource parentSequenceSource)
		: base(parent, null, null, caster)
	{
		m_abilityTarget = abilityTarget;
		m_explodeShape = explodeShape;
		m_explodePenetratesLoS = explodePenetratesLoS;
		m_damageToEnemiesOnExplode = damageToEnemiesOnExplode;
		m_healingToAlliesOnExplode = healingToAlliesOnExplode;
		m_applyKnockbackOnExplode = applyKnockbackOnExplode;
		m_knockbackDistOnExplode = knockbackDistOnExplode;
		m_knockbackTypeOnExplode = knockbackTypeOnExplode;
		m_effectToEnemiesOnExplode = effectToEnemiesOnExplode;
		m_effectToAlliesOnExplode = effectToAlliesOnExplode;
		m_turnsTillFirstExplosion = turnsTillFirstExplosion;
		m_numExplosionsBeforeEnding = numExplosionsBeforeEnding;
		m_hittingEnemySequence = hittingEnemySequence;
		m_hittingAllySequence = hittingAllySequence;
		m_groundEffectPersistentSequence = groundEffectPersistentSequence;
		m_groundEffectExplodeSequence = groundEffectExplodeSequence;
		m_time.duration = turnsTillFirstExplosion + numExplosionsBeforeEnding;
		HitPhase = applyKnockbackOnExplode ? AbilityPriority.Combat_Knockback : AbilityPriority.Combat_Damage;
		m_effectName = "Geyser";
		SequenceSource = new SequenceSource(OnHit_Base, OnHit_Base, false, parentSequenceSource);
	}

	private bool CanAffectEnemies()
	{
		return m_damageToEnemiesOnExplode > 0
		       || m_effectToEnemiesOnExplode.m_applyEffect
		       || m_applyKnockbackOnExplode;
	}

	private bool CanAffectAllies()
	{
		return m_healingToAlliesOnExplode > 0
		       || m_effectToAlliesOnExplode.m_applyEffect;
	}

	private bool ShouldExplodeThisTurn()
	{
		return m_numExplosionsBeforeEnding > 0
		       && m_time.age >= m_turnsTillFirstExplosion;
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(
			m_groundEffectPersistentSequence,
			AreaEffectUtils.GetCenterOfShape(m_explodeShape, m_abilityTarget),
			null,
			Caster,
			SequenceSource);
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		List<ActorData> enemiesHit = new List<ActorData>();
		List<ActorData> alliesHit = new List<ActorData>();
		foreach (ActorData actorData in m_effectResults.HitActorsArray())
		{
			if (actorData.GetTeam() == Caster.GetTeam())
			{
				alliesHit.Add(actorData);
			}
			else
			{
				enemiesHit.Add(actorData);
			}
		}
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_explodeShape, m_abilityTarget);
		if (m_hittingAllySequence != null && alliesHit.Count > 0)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_hittingAllySequence, centerOfShape, alliesHit.ToArray(), Caster, SequenceSource));
		}
		if (m_hittingEnemySequence != null && enemiesHit.Count > 0)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_hittingEnemySequence, centerOfShape, enemiesHit.ToArray(), Caster, SequenceSource));
		}
		if (m_groundEffectExplodeSequence != null)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_groundEffectExplodeSequence, centerOfShape, null, Caster, SequenceSource));
		}
		return list;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (!ShouldExplodeThisTurn())
		{
			return;
		}
		
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<Team> affectedTeams = new List<Team>();
		if (CanAffectAllies())
		{
			affectedTeams.Add(Caster.GetTeam());
		}
		if (CanAffectEnemies())
		{
			affectedTeams.AddRange(Caster.GetOtherTeams());
		}
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			m_explodeShape,
			m_abilityTarget,
			m_explodePenetratesLoS,
			Caster,
			affectedTeams,
			nonActorTargetInfo);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_explodeShape, m_abilityTarget);
		foreach (ActorData actorData in actorsInShape)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, centerOfShape));
			if (actorData.GetTeam() == Caster.GetTeam())
			{
				actorHitResults.AddBaseHealing(m_healingToAlliesOnExplode);
				actorHitResults.AddStandardEffectInfo(m_effectToAlliesOnExplode);
			}
			else
			{
				actorHitResults.AddBaseDamage(m_damageToEnemiesOnExplode);
				actorHitResults.AddStandardEffectInfo(m_effectToEnemiesOnExplode);
				if (m_applyKnockbackOnExplode)
				{
					KnockbackHitData knockbackData = new KnockbackHitData(
						actorData,
						Caster,
						m_knockbackTypeOnExplode,
						m_abilityTarget.AimDirection,
						centerOfShape,
						m_knockbackDistOnExplode);
					actorHitResults.AddKnockbackData(knockbackData);
				}
			}
			effectResults.StoreActorHit(actorHitResults);
		}
		effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != Caster.GetTeam())
		{
			List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(m_explodeShape, m_abilityTarget, true, Caster);
			squaresToAvoid.UnionWith(squaresInShape);
		}
	}
}
#endif
