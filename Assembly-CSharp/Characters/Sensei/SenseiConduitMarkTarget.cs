// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SenseiConduitMarkTarget : Ability
{
	public bool m_penetratesLoS;
	public StandardEffectInfo m_conduitEffectOnEnemy;
	public StandardEffectInfo m_reactionEffectOnAlliesHittingTarget;
	public int m_healAmountOnAlliesHittingTarget = 10;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;
	public GameObject m_reactionProjectilePrefab;

	private StandardEffectInfo m_cachedConduitEffectOnEnemy;
	private StandardEffectInfo m_cachedReactionEffectOnAlliesHittingTarget;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Sensei Conduit Mark Target";
		}
		SetCachedFields();
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			GetPenetratesLoS(),
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			true,
			false,
			AbilityUtil_Targeter.AffectsActor.Never,
			AbilityUtil_Targeter.AffectsActor.Always);
	}

	private void SetCachedFields()
	{
		m_cachedConduitEffectOnEnemy = m_conduitEffectOnEnemy;
		m_cachedReactionEffectOnAlliesHittingTarget = m_reactionEffectOnAlliesHittingTarget;
	}

	public StandardEffectInfo GetConduitEffectOnEnemy()
	{
		return m_cachedConduitEffectOnEnemy ?? m_conduitEffectOnEnemy;
	}

	public StandardEffectInfo GetReactionEffectOnAlliesHittingTarget()
	{
		return m_cachedReactionEffectOnAlliesHittingTarget ?? m_reactionEffectOnAlliesHittingTarget;
	}

	public bool GetPenetratesLoS()
	{
		return m_penetratesLoS;
	}

	public int GetReactionHealAmount()
	{
		return m_healAmountOnAlliesHittingTarget;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(
			caster,
			true,
			false,
			false,
			ValidateCheckPath.Ignore,
			!GetPenetratesLoS(),
			false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		return CanTargetActorInDecision(
			caster,
			target.GetCurrentBestActorTarget(),
			true,
			false,
			false,
			ValidateCheckPath.Ignore,
			!GetPenetratesLoS(),
			false);
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ActorData> hitActors = GetHitActors(targets, caster);
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				targets[0].FreePos,
				hitActors.ToArray(),
				caster,
				additionalData.m_sequenceSource,
				new Sequence.IExtraSequenceParams[0])
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<ActorData> hitActors = GetHitActors(targets, caster);
		foreach (ActorData target in hitActors)
		{
			ActorHitParameters hitParams = new ActorHitParameters(target, caster.GetFreePos());
			ActorHitResults hitResults = new ActorHitResults(new SenseiConduitReactionEffect(
				AsEffectSource(),
				target,
				caster,
				GetConduitEffectOnEnemy().m_effectData,
				GetReactionHealAmount(),
				GetReactionEffectOnAlliesHittingTarget(),
				m_reactionProjectilePrefab),
				hitParams);
			abilityResults.StoreActorHit(hitResults);
		}
	}

	// added in rogues
	private List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster)
	{
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		if (square != null
		    && square.OccupantActor != null
		    && !square.OccupantActor.IgnoreForAbilityHits
		    && square.OccupantActor.GetTeam() != caster.GetTeam())
		{
			return new List<ActorData>
			{
				square.OccupantActor
			};
		}
		return new List<ActorData>();
	}
#endif
}
