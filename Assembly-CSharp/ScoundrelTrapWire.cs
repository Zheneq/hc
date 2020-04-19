using System;
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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Trap Wires";
		}
		if (this.m_pattern != AbilityGridPattern.NoPattern)
		{
			this.ModdedBarrierData().SetupForPattern(this.m_pattern);
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (this.m_pattern == AbilityGridPattern.NoPattern)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelTrapWire.SetupTargeter()).MethodHandle;
			}
			base.Targeter = new AbilityUtil_Targeter_Barrier(this, this.ModdedBarrierData().m_width * this.ModdedBarrierScale(), false, false, true);
		}
		else
		{
			AbilityUtil_Targeter_Grid targeter = new AbilityUtil_Targeter_Grid(this, this.m_pattern, this.ModdedBarrierScale());
			base.Targeter = targeter;
		}
		base.Targeter.ShowArcToShape = true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		this.ModdedBarrierData().AddTooltipTokens(tokens, "Wall", false, null);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.ModdedBarrierData().ReportAbilityTooltipNumbers(ref result);
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ScoundrelTrapWire))
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelTrapWire.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_ScoundrelTrapWire);
			this.SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	public StandardBarrierData ModdedBarrierData()
	{
		if (this.m_abilityMod != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelTrapWire.ModdedBarrierData()).MethodHandle;
			}
			return this.m_abilityMod.m_barrierDataMod.GetModifiedValue(this.m_barrierData);
		}
		return this.m_barrierData;
	}

	private float ModdedBarrierScale()
	{
		float num = this.m_barrierSizeScale;
		if (this.m_abilityMod != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelTrapWire.ModdedBarrierScale()).MethodHandle;
			}
			num = this.m_abilityMod.m_barrierScaleMod.GetModifiedValue(num);
		}
		return num;
	}

	public List<GameObject> ModdedBarrierSequencePrefab()
	{
		List<GameObject> result = this.ModdedBarrierData().m_barrierSequencePrefabs;
		if (this.m_abilityMod != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelTrapWire.ModdedBarrierSequencePrefab()).MethodHandle;
			}
			if (this.m_abilityMod.m_barrierSequence != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_abilityMod.m_barrierSequence.Count > 0)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					result = this.m_abilityMod.m_barrierSequence;
				}
			}
		}
		return result;
	}
}
