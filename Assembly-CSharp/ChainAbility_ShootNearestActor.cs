using System;
using System.Collections.Generic;
using UnityEngine;

public class ChainAbility_ShootNearestActor : Ability
{
	[Header("-- On Hit - Enemy ----------------------------------------")]
	public int m_enemyDamageAmount = 3;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- On Hit - Ally")]
	public int m_allyHealAmount = 3;

	public StandardEffectInfo m_allyHitEffect;

	[Header("-- Target selection")]
	public float m_maxRange = 5f;

	public bool m_includeAllies;

	public bool m_includeEnemies = true;

	public bool m_penetrateLos;

	[Header("-- Sequences ----------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "CHAIN_ABILITY_SHOOT_NEAREST_ACTOR";
		}
		this.m_sequencePrefab = this.m_castSequencePrefab;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}
}
