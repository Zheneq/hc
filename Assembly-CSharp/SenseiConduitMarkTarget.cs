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
}
