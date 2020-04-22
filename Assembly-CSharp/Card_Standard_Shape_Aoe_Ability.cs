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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Card Ability - Standard Shape Aoe";
		}
		m_sequencePrefab = m_castSequencePrefab;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, m_shape, m_penetrateLos, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, IncludeEnemies(), IncludeAllies());
		base.Targeter.ShowArcToShape = m_showTargeterArc;
	}

	public bool IncludeAllies()
	{
		int result;
		if (m_includeAllies)
		{
			while (true)
			{
				switch (6)
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
			if (m_healAmount <= 0)
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
				if (m_techPointGain <= 0)
				{
					result = (m_allyHitEffect.m_applyEffect ? 1 : 0);
					goto IL_0048;
				}
			}
			result = 1;
		}
		else
		{
			result = 0;
		}
		goto IL_0048;
		IL_0048:
		return (byte)result != 0;
	}

	public bool IncludeEnemies()
	{
		int result;
		if (m_includeEnemies)
		{
			if (m_damageAmount <= 0)
			{
				while (true)
				{
					switch (7)
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
				if (m_techPointLoss <= 0)
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
					result = (m_enemyHitEffect.m_applyEffect ? 1 : 0);
					goto IL_0048;
				}
			}
			result = 1;
		}
		else
		{
			result = 0;
		}
		goto IL_0048;
		IL_0048:
		return (byte)result != 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_requireTargetingOnActor)
		{
			return HasTargetableActorsInDecision(caster, IncludeEnemies(), IncludeAllies(), false, ValidateCheckPath.Ignore, true, false);
		}
		return base.CustomCanCastValidation(caster);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (m_requireTargetingOnActor)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					bool result = false;
					List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, IncludeAllies(), IncludeEnemies());
					List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(m_targeterValidationShape, target, m_penetrateLos, caster, relevantTeams, null);
					using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData current = enumerator.Current;
							if (CanTargetActorInDecision(caster, current, IncludeEnemies(), IncludeAllies(), false, ValidateCheckPath.Ignore, true, false))
							{
								return true;
							}
						}
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								return result;
							}
						}
					}
				}
				}
			}
		}
		return base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
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
