using System;
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
	public int m_baseDamage = 0x14;

	public int m_subseqHitDamage = 0xA;

	public StandardEffectInfo m_enemyHitEffect;

	[Separator("Sequences", true)]
	public GameObject m_projectileSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalBomb.Start()).MethodHandle;
			}
			this.m_abilityName = "GrydCardinalBomb";
		}
		this.Setup();
	}

	private void Setup()
	{
		base.Targeter = new AbilityUtil_Targeter_GrydCardinalBomb(this, this.m_maxTrunkDist, this.m_maxBranchDist, this.m_splitOnWall, this.m_splitOnActor, this.m_trunkContinueAfterActorHit, this.m_maxNumSplits);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxNumSplits", string.Empty, this.m_maxNumSplits, false);
		base.AddTokenInt(tokens, "BaseDamage", string.Empty, this.m_baseDamage, false);
		base.AddTokenInt(tokens, "SubseqHitDamage", string.Empty, this.m_subseqHitDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffect, "EnemyHitEffect", this.m_enemyHitEffect, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_baseDamage);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		AbilityUtil_Targeter_GrydCardinalBomb abilityUtil_Targeter_GrydCardinalBomb = base.Targeter as AbilityUtil_Targeter_GrydCardinalBomb;
		if (abilityUtil_Targeter_GrydCardinalBomb != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalBomb.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (abilityUtil_Targeter_GrydCardinalBomb.m_actorToHitContext.ContainsKey(targetActor))
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				int numHits = abilityUtil_Targeter_GrydCardinalBomb.m_actorToHitContext[targetActor].m_numHits;
				int numHitsFromCover = abilityUtil_Targeter_GrydCardinalBomb.m_actorToHitContext[targetActor].m_numHitsFromCover;
				int damage = ActorMultiHitContext.CalcDamageFromNumHits(numHits, numHitsFromCover, this.m_baseDamage, this.m_subseqHitDamage);
				results.m_damage = damage;
				return true;
			}
		}
		return false;
	}
}
