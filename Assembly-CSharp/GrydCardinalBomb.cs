using System.Collections.Generic;
using UnityEngine;

public class GrydCardinalBomb : Ability
{
	[Separator("Targeting", true)]
	public float m_maxTrunkDist = 8.5f;

	public float m_maxBranchDist = 5f;

	public bool m_splitOnWall = true;

	public bool m_splitOnActor;

	public bool m_trunkContinueAfterActorHit;

	public int m_maxNumSplits = 1;

	[Separator("On Hit", true)]
	public int m_baseDamage = 20;

	public int m_subseqHitDamage = 10;

	public StandardEffectInfo m_enemyHitEffect;

	[Separator("Sequences", true)]
	public GameObject m_projectileSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "GrydCardinalBomb";
		}
		Setup();
	}

	private void Setup()
	{
		base.Targeter = new AbilityUtil_Targeter_GrydCardinalBomb(this, m_maxTrunkDist, m_maxBranchDist, m_splitOnWall, m_splitOnActor, m_trunkContinueAfterActorHit, m_maxNumSplits);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxNumSplits", string.Empty, m_maxNumSplits);
		AddTokenInt(tokens, "BaseDamage", string.Empty, m_baseDamage);
		AddTokenInt(tokens, "SubseqHitDamage", string.Empty, m_subseqHitDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_baseDamage);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		AbilityUtil_Targeter_GrydCardinalBomb abilityUtil_Targeter_GrydCardinalBomb = base.Targeter as AbilityUtil_Targeter_GrydCardinalBomb;
		if (abilityUtil_Targeter_GrydCardinalBomb != null)
		{
			if (abilityUtil_Targeter_GrydCardinalBomb.m_actorToHitContext.ContainsKey(targetActor))
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						int numHits = abilityUtil_Targeter_GrydCardinalBomb.m_actorToHitContext[targetActor].m_numHits;
						int numHitsFromCover = abilityUtil_Targeter_GrydCardinalBomb.m_actorToHitContext[targetActor].m_numHitsFromCover;
						int num = results.m_damage = ActorMultiHitContext.CalcDamageFromNumHits(numHits, numHitsFromCover, m_baseDamage, m_subseqHitDamage);
						return true;
					}
					}
				}
			}
		}
		return false;
	}
}
