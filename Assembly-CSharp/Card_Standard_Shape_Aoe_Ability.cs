using System.Collections.Generic;
using UnityEngine;

// Chronosurge, Probe
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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Card Ability - Standard Shape Aoe";
		}
		m_sequencePrefab = m_castSequencePrefab;
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			m_shape,
			m_penetrateLos,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			IncludeEnemies(),
			IncludeAllies());
		Targeter.ShowArcToShape = m_showTargeterArc;
	}

	public bool IncludeAllies()
	{
		return m_includeAllies && (m_healAmount > 0 || m_techPointGain > 0 || m_allyHitEffect.m_applyEffect);
	}

	public bool IncludeEnemies()
	{
		return m_includeEnemies && (m_damageAmount > 0 || m_techPointLoss > 0 || m_enemyHitEffect.m_applyEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_requireTargetingOnActor)
		{
			return HasTargetableActorsInDecision(
				caster,
				IncludeEnemies(),
				IncludeAllies(), 
				false,
				ValidateCheckPath.Ignore,
				true,
				false);
		}
		return base.CustomCanCastValidation(caster);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (!m_requireTargetingOnActor)
		{
			return base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
		}
		
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, IncludeAllies(), IncludeEnemies());
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			m_targeterValidationShape, target, m_penetrateLos, caster, relevantTeams, null);
					
		foreach (ActorData actor in actorsInShape)
		{
			if (CanTargetActorInDecision(
				    caster,
				    actor,
				    IncludeEnemies(),
				    IncludeAllies(),
				    false,
				    ValidateCheckPath.Ignore,
				    true,
				    false))
			{
				return true;
			}
		}
		return false;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "HealAmount", string.Empty, m_healAmount);
		AddTokenInt(tokens, "TechPointGain", string.Empty, m_techPointGain);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "TechPointLoss", string.Empty, m_techPointLoss);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "VisionDuration", string.Empty, m_visionDuration);
	}
}
