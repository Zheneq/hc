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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Aim Shield";
		}
		Setup();
		ResetTooltipAndTargetingNumbers();
	}

	private void Setup()
	{
		if (m_passive == null)
		{
			m_passive = (GetComponent<PassiveData>().GetPassiveOfType(typeof(Passive_Rampart)) as Passive_Rampart);
		}
		float width = (!(m_passive != null)) ? 3f : m_passive.GetShieldBarrierData().m_width;
		base.Targeter = new AbilityUtil_Targeter_Barrier(this, width, m_snapToGrid, m_allowAimAtDiagonals, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_passive != null)
		{
			m_passive.GetShieldBarrierData().ReportAbilityTooltipNumbers(ref numbers);
		}
		return numbers;
	}
}
