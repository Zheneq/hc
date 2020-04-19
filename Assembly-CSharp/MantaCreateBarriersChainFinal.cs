using System;
using UnityEngine;

public class MantaCreateBarriersChainFinal : Ability
{
	[Header("-- Ground effect")]
	public StandardGroundEffectInfo m_groundEffectInfo;

	public int m_damageOnCast = 0x1E;

	[Header("-- Sequences -------------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Lair chained ability";
		}
	}

	public int GetDamageOnCast()
	{
		return this.m_damageOnCast;
	}
}
