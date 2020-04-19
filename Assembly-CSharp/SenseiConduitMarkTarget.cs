using System;
using System.Collections.Generic;
using UnityEngine;

public class SenseiConduitMarkTarget : Ability
{
	public bool m_penetratesLoS;

	public StandardEffectInfo m_conduitEffectOnEnemy;

	public StandardEffectInfo m_reactionEffectOnAlliesHittingTarget;

	public int m_healAmountOnAlliesHittingTarget = 0xA;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	public GameObject m_reactionProjectilePrefab;

	private StandardEffectInfo m_cachedConduitEffectOnEnemy;

	private StandardEffectInfo m_cachedReactionEffectOnAlliesHittingTarget;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Sensei Conduit Mark Target";
		}
		this.SetCachedFields();
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, this.GetPenetratesLoS(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Never, AbilityUtil_Targeter.AffectsActor.Always);
	}

	private void SetCachedFields()
	{
		this.m_cachedConduitEffectOnEnemy = this.m_conduitEffectOnEnemy;
		this.m_cachedReactionEffectOnAlliesHittingTarget = this.m_reactionEffectOnAlliesHittingTarget;
	}

	public StandardEffectInfo GetConduitEffectOnEnemy()
	{
		StandardEffectInfo result;
		if (this.m_cachedConduitEffectOnEnemy != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiConduitMarkTarget.GetConduitEffectOnEnemy()).MethodHandle;
			}
			result = this.m_cachedConduitEffectOnEnemy;
		}
		else
		{
			result = this.m_conduitEffectOnEnemy;
		}
		return result;
	}

	public StandardEffectInfo GetReactionEffectOnAlliesHittingTarget()
	{
		StandardEffectInfo result;
		if (this.m_cachedReactionEffectOnAlliesHittingTarget != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiConduitMarkTarget.GetReactionEffectOnAlliesHittingTarget()).MethodHandle;
			}
			result = this.m_cachedReactionEffectOnAlliesHittingTarget;
		}
		else
		{
			result = this.m_reactionEffectOnAlliesHittingTarget;
		}
		return result;
	}

	public bool GetPenetratesLoS()
	{
		return this.m_penetratesLoS;
	}

	public int GetReactionHealAmount()
	{
		return this.m_healAmountOnAlliesHittingTarget;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return base.HasTargetableActorsInDecision(caster, true, false, false, Ability.ValidateCheckPath.Ignore, !this.GetPenetratesLoS(), false, false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return base.CanTargetActorInDecision(caster, currentBestActorTarget, true, false, false, Ability.ValidateCheckPath.Ignore, !this.GetPenetratesLoS(), false, false);
	}
}
