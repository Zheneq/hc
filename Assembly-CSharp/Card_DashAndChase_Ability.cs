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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Card_DashAndChase_Ability";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(this, m_targetShape, m_targetShapePenetratesLoS, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, CanIncludeEnemy(), CanIncludeAlly());
		abilityUtil_Targeter_Charge.m_forceChase = m_chaseTarget;
		base.Targeter = abilityUtil_Targeter_Charge;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (RequireTargetActor())
		{
			return HasTargetableActorsInDecision(caster, CanIncludeEnemy(), CanIncludeAlly(), false, ValidateCheckPath.CanBuildPath, true, false);
		}
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool result = true;
		if (RequireTargetActor())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					result = false;
					List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, CanIncludeAlly(), CanIncludeEnemy());
					List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(m_targetShape, target, m_targetShapePenetratesLoS, caster, relevantTeams, null);
					{
						foreach (ActorData item in actorsInShape)
						{
							if (CanTargetActorInDecision(caster, item, CanIncludeEnemy(), CanIncludeAlly(), false, ValidateCheckPath.CanBuildPath, true, false))
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										return true;
									}
								}
							}
						}
						return result;
					}
				}
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
		AddTokenInt(tokens, "MaxTargetsHit", string.Empty, m_maxTargetsHit);
		AbilityMod.AddToken_EffectInfo(tokens, m_chaserEffect, "ChaserEffect", m_chaserEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyTargetEffect, "EnemyTargetEffect", m_enemyTargetEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyTargetEffect, "AllyTargetEffect", m_allyTargetEffect);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public bool RequireTargetActor()
	{
		return m_requireTargetActor;
	}

	public bool CanIncludeEnemy()
	{
		return m_canIncludeEnemy;
	}

	public bool CanIncludeAlly()
	{
		return m_canIncludeAlly;
	}
}
