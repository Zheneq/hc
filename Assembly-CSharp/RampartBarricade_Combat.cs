using System.Collections.Generic;
using UnityEngine;

public class RampartBarricade_Combat : Ability
{
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private RampartBarricade_Prep m_prepAbility;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Barricade - Chain - Knockback";
		}
		m_prepAbility = (GetComponent<AbilityData>().GetAbilityOfType(typeof(RampartBarricade_Prep)) as RampartBarricade_Prep);
		if (!(m_prepAbility == null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Debug.LogError("Rampart Barricade Chain: did not find parent ability");
			return;
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}
}
