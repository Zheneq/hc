using System;
using UnityEngine;

public class ChainAbility_EffectOnCaster : Ability
{
	public bool m_applyEffect = true;

	public StandardActorEffectData m_effect;

	[Header("-- Sequences ----------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "CHAIN_ABILITY_EFFECT_ON_CASTER";
		}
		this.m_sequencePrefab = this.m_castSequencePrefab;
	}
}
