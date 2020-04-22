using UnityEngine;

public class MantaCreateBarriersChainFinal : Ability
{
	[Header("-- Ground effect")]
	public StandardGroundEffectInfo m_groundEffectInfo;

	public int m_damageOnCast = 30;

	[Header("-- Sequences -------------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Lair chained ability";
		}
	}

	public int GetDamageOnCast()
	{
		return m_damageOnCast;
	}
}
