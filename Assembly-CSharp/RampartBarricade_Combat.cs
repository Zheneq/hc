using System;
using System.Collections.Generic;
using UnityEngine;

public class RampartBarricade_Combat : Ability
{
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private RampartBarricade_Prep m_prepAbility;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Barricade - Chain - Knockback";
		}
		this.m_prepAbility = (base.GetComponent<AbilityData>().GetAbilityOfType(typeof(RampartBarricade_Prep)) as RampartBarricade_Prep);
		if (this.m_prepAbility == null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartBarricade_Combat.Start()).MethodHandle;
			}
			Debug.LogError("Rampart Barricade Chain: did not find parent ability");
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}
}
