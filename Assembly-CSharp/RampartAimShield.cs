using System;
using System.Collections.Generic;
using UnityEngine;

public class RampartAimShield : Ability
{
	[Header("-- Shield Barrier (Barrier Data specified on Passive)")]
	public bool m_allowAimAtDiagonals;

	[Header("-- Sequences")]
	public GameObject m_removeShieldSequencePrefab;

	public GameObject m_applyShieldSequencePrefab;

	private bool m_snapToGrid = true;

	private Passive_Rampart m_passive;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAimShield.Start()).MethodHandle;
			}
			this.m_abilityName = "Aim Shield";
		}
		this.Setup();
		base.ResetTooltipAndTargetingNumbers();
	}

	private void Setup()
	{
		if (this.m_passive == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAimShield.Setup()).MethodHandle;
			}
			this.m_passive = (base.GetComponent<PassiveData>().GetPassiveOfType(typeof(Passive_Rampart)) as Passive_Rampart);
		}
		float width = (!(this.m_passive != null)) ? 3f : this.m_passive.GetShieldBarrierData().m_width;
		base.Targeter = new AbilityUtil_Targeter_Barrier(this, width, this.m_snapToGrid, this.m_allowAimAtDiagonals, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.m_passive != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartAimShield.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			this.m_passive.GetShieldBarrierData().ReportAbilityTooltipNumbers(ref result);
		}
		return result;
	}
}
