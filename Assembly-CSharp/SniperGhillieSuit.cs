using System;
using System.Collections.Generic;
using UnityEngine;

public class SniperGhillieSuit : Ability
{
	public bool m_isToggleAbility = true;

	public int m_costPerTurn = 1;

	public bool m_proximityBasedInvisibility = true;

	public bool m_unsuppressInvisOnPhaseEnd = true;

	public StandardActorEffectData m_standardActorEffectData;

	[Header("-- Health threshold to trigger cooldown reset, value:(0-1)")]
	public float m_cooldownResetHealthThreshold = -1f;

	[Header("-- Sequences --------------------------------------")]
	public GameObject m_toggleOnSequencePrefab;

	public GameObject m_toggleOffSequencePrefab;

	[TextArea(1, 3)]
	public string m_sequenceNotes;

	private AbilityMod_SniperGhillieSuit m_abilityMod;

	private void Start()
	{
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, true, AbilityUtil_Targeter.AffectsActor.Always, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = false;
	}

	private int GetHealingAmountOnSelf()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperGhillieSuit.GetHealingAmountOnSelf()).MethodHandle;
			}
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_healingOnSelf;
		}
		return result;
	}

	private StandardActorEffectData GetStealthEffectData()
	{
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperGhillieSuit.GetStealthEffectData()).MethodHandle;
			}
			if (this.m_abilityMod.m_useStealthEffectDataOverride)
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
				return this.m_abilityMod.m_stealthEffectDataOverride;
			}
		}
		return this.m_standardActorEffectData;
	}

	public float GetCooldownResetHealthThreshold()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperGhillieSuit.GetCooldownResetHealthThreshold()).MethodHandle;
			}
			result = this.m_abilityMod.m_cooldownResetHealthThresholdMod.GetModifiedValue(this.m_cooldownResetHealthThreshold);
		}
		else
		{
			result = this.m_cooldownResetHealthThreshold;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.m_standardActorEffectData.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.GetHealingAmountOnSelf() > 0)
		{
			AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetHealingAmountOnSelf());
		}
		this.m_standardActorEffectData.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SniperGhillieSuit abilityMod_SniperGhillieSuit = modAsBase as AbilityMod_SniperGhillieSuit;
		base.AddTokenInt(tokens, "CostPerTurn", string.Empty, this.m_costPerTurn, false);
		this.m_standardActorEffectData.AddTooltipTokens(tokens, "StandardActorEffectData", false, null);
		string name = "CooldownResetHealthThreshold_Pct";
		string empty = string.Empty;
		float val;
		if (abilityMod_SniperGhillieSuit)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperGhillieSuit.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_SniperGhillieSuit.m_cooldownResetHealthThresholdMod.GetModifiedValue(this.m_cooldownResetHealthThreshold);
		}
		else
		{
			val = this.m_cooldownResetHealthThreshold;
		}
		base.AddTokenFloatAsPct(tokens, name, empty, val, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SniperGhillieSuit))
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SniperGhillieSuit.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_SniperGhillieSuit);
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
	}
}
