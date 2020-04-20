using System;
using UnityEngine;

public class GrydDetonateBomb : Ability
{
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Detonate";
		}
	}
}
