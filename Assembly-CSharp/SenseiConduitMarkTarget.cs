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
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, GetPenetratesLoS(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Never, AbilityUtil_Targeter.AffectsActor.Always);
	}

	private void SetCachedFields()
	{
		m_cachedConduitEffectOnEnemy = m_conduitEffectOnEnemy;
		m_cachedReactionEffectOnAlliesHittingTarget = m_reactionEffectOnAlliesHittingTarget;
	}

	public StandardEffectInfo GetConduitEffectOnEnemy()
	{
		StandardEffectInfo result;
		if (m_cachedConduitEffectOnEnemy != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedConduitEffectOnEnemy;
		}
		else
		{
			result = m_conduitEffectOnEnemy;
		}
		return result;
	}

	public StandardEffectInfo GetReactionEffectOnAlliesHittingTarget()
	{
		StandardEffectInfo result;
		if (m_cachedReactionEffectOnAlliesHittingTarget != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedReactionEffectOnAlliesHittingTarget;
		}
		else
		{
			result = m_reactionEffectOnAlliesHittingTarget;
		}
		return result;
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
		return HasTargetableActorsInDecision(caster, true, false, false, ValidateCheckPath.Ignore, !GetPenetratesLoS(), false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(caster, currentBestActorTarget, true, false, false, ValidateCheckPath.Ignore, !GetPenetratesLoS(), false);
	}
}
