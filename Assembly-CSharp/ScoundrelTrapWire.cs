using System.Collections.Generic;
using UnityEngine;

public class ScoundrelTrapWire : Ability
{
	public AbilityGridPattern m_pattern = AbilityGridPattern.Plus_Two_x_Two;

	public float m_barrierSizeScale = 1f;

	public StandardBarrierData m_barrierData;

	private AbilityMod_ScoundrelTrapWire m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Trap Wires";
		}
		if (m_pattern != 0)
		{
			ModdedBarrierData().SetupForPattern(m_pattern);
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (m_pattern != 0)
		{
			AbilityUtil_Targeter_Grid abilityUtil_Targeter_Grid = (AbilityUtil_Targeter_Grid)(base.Targeter = new AbilityUtil_Targeter_Grid(this, m_pattern, ModdedBarrierScale()));
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_Barrier(this, ModdedBarrierData().m_width * ModdedBarrierScale());
		}
		base.Targeter.ShowArcToShape = true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		ModdedBarrierData().AddTooltipTokens(tokens, "Wall");
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		ModdedBarrierData().ReportAbilityTooltipNumbers(ref numbers);
		return numbers;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ScoundrelTrapWire))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_ScoundrelTrapWire);
					SetupTargeter();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public StandardBarrierData ModdedBarrierData()
	{
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_abilityMod.m_barrierDataMod.GetModifiedValue(m_barrierData);
				}
			}
		}
		return m_barrierData;
	}

	private float ModdedBarrierScale()
	{
		float num = m_barrierSizeScale;
		if (m_abilityMod != null)
		{
			num = m_abilityMod.m_barrierScaleMod.GetModifiedValue(num);
		}
		return num;
	}

	public List<GameObject> ModdedBarrierSequencePrefab()
	{
		List<GameObject> result = ModdedBarrierData().m_barrierSequencePrefabs;
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_barrierSequence != null)
			{
				if (m_abilityMod.m_barrierSequence.Count > 0)
				{
					result = m_abilityMod.m_barrierSequence;
				}
			}
		}
		return result;
	}
}
