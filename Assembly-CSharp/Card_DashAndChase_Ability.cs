using System;
using System.Collections.Generic;
using UnityEngine;

public class Card_DashAndChase_Ability : Ability
{
	public int m_maxTargetsHit = 1;

	public AbilityAreaShape m_targetShape;

	public bool m_targetShapePenetratesLoS;

	public bool m_chaseTarget = true;

	public StandardEffectInfo m_chaserEffect;

	public StandardEffectInfo m_enemyTargetEffect;

	public StandardEffectInfo m_allyTargetEffect;

	public float m_recoveryTime = 1f;

	[Header("-- Targeting")]
	public bool m_requireTargetActor = true;

	public bool m_canIncludeEnemy = true;

	public bool m_canIncludeAlly = true;

	[Header("-- Sequence")]
	public GameObject m_onCastSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Card_DashAndChase_Ability";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_Charge(this, this.m_targetShape, this.m_targetShapePenetratesLoS, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, this.CanIncludeEnemy(), this.CanIncludeAlly())
		{
			m_forceChase = this.m_chaseTarget
		};
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return !this.RequireTargetActor() || base.HasTargetableActorsInDecision(caster, this.CanIncludeEnemy(), this.CanIncludeAlly(), false, Ability.ValidateCheckPath.CanBuildPath, true, false, false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool result = true;
		if (this.RequireTargetActor())
		{
			result = false;
			List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, this.CanIncludeAlly(), this.CanIncludeEnemy());
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_targetShape, target, this.m_targetShapePenetratesLoS, caster, relevantTeams, null);
			foreach (ActorData targetActor in actorsInShape)
			{
				bool flag = base.CanTargetActorInDecision(caster, targetActor, this.CanIncludeEnemy(), this.CanIncludeAlly(), false, Ability.ValidateCheckPath.CanBuildPath, true, false, false);
				if (flag)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxTargetsHit", string.Empty, this.m_maxTargetsHit, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_chaserEffect, "ChaserEffect", this.m_chaserEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyTargetEffect, "EnemyTargetEffect", this.m_enemyTargetEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyTargetEffect, "AllyTargetEffect", this.m_allyTargetEffect, true);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public bool RequireTargetActor()
	{
		return this.m_requireTargetActor;
	}

	public bool CanIncludeEnemy()
	{
		return this.m_canIncludeEnemy;
	}

	public bool CanIncludeAlly()
	{
		return this.m_canIncludeAlly;
	}
}
