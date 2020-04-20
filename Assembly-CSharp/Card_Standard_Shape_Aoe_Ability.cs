using System;
using System.Collections.Generic;
using UnityEngine;

public class Card_Standard_Shape_Aoe_Ability : Ability
{
	public AbilityAreaShape m_shape = AbilityAreaShape.Five_x_Five;

	public bool m_penetrateLos = true;

	public bool m_includeAllies;

	public bool m_includeEnemies;

	[Header("-- Whether require targeting on or near actor")]
	public bool m_requireTargetingOnActor;

	public AbilityAreaShape m_targeterValidationShape;

	[Header("-- Whether to center shape on caster (for self targeted abilities after Evasion phase)")]
	public bool m_centerShapeOnCaster;

	[Header("-- On Ally")]
	public int m_healAmount;

	public int m_techPointGain;

	public StandardEffectInfo m_allyHitEffect;

	[Header("-- On Enemy")]
	public int m_damageAmount;

	public int m_techPointLoss;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Vision on Target Square")]
	public bool m_addVisionOnTargetSquare;

	public float m_visionRadius = 1.5f;

	public int m_visionDuration = 1;

	public VisionProviderInfo.BrushRevealType m_brushRevealType = VisionProviderInfo.BrushRevealType.Always;

	public bool m_visionAreaIgnoreLos = true;

	public bool m_visionAreaCanFunctionInGlobalBlind = true;

	[Header("-- Whether to show targeter arc")]
	public bool m_showTargeterArc;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_persistentSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Card Ability - Standard Shape Aoe";
		}
		this.m_sequencePrefab = this.m_castSequencePrefab;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, this.m_shape, this.m_penetrateLos, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, this.IncludeEnemies(), this.IncludeAllies(), AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = this.m_showTargeterArc;
	}

	public bool IncludeAllies()
	{
		bool result;
		if (this.m_includeAllies)
		{
			if (this.m_healAmount <= 0)
			{
				if (this.m_techPointGain <= 0)
				{
					result = this.m_allyHitEffect.m_applyEffect;
					goto IL_45;
				}
			}
			result = true;
			IL_45:;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool IncludeEnemies()
	{
		bool result;
		if (this.m_includeEnemies)
		{
			if (this.m_damageAmount <= 0)
			{
				if (this.m_techPointLoss <= 0)
				{
					result = this.m_enemyHitEffect.m_applyEffect;
					goto IL_45;
				}
			}
			result = true;
			IL_45:;
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_requireTargetingOnActor)
		{
			return base.HasTargetableActorsInDecision(caster, this.IncludeEnemies(), this.IncludeAllies(), false, Ability.ValidateCheckPath.Ignore, true, false, false);
		}
		return base.CustomCanCastValidation(caster);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (this.m_requireTargetingOnActor)
		{
			bool result = false;
			List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, this.IncludeAllies(), this.IncludeEnemies());
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_targeterValidationShape, target, this.m_penetrateLos, caster, relevantTeams, null);
			using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData targetActor = enumerator.Current;
					bool flag = base.CanTargetActorInDecision(caster, targetActor, this.IncludeEnemies(), this.IncludeAllies(), false, Ability.ValidateCheckPath.Ignore, true, false, false);
					if (flag)
					{
						return true;
					}
				}
			}
			return result;
		}
		return base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "HealAmount", string.Empty, this.m_healAmount, false);
		base.AddTokenInt(tokens, "TechPointGain", string.Empty, this.m_techPointGain, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyHitEffect, "AllyHitEffect", this.m_allyHitEffect, true);
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, this.m_damageAmount, false);
		base.AddTokenInt(tokens, "TechPointLoss", string.Empty, this.m_techPointLoss, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffect, "EnemyHitEffect", this.m_enemyHitEffect, true);
		base.AddTokenInt(tokens, "VisionDuration", string.Empty, this.m_visionDuration, false);
	}
}
