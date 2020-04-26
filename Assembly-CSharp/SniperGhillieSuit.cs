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
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, true, AbilityUtil_Targeter.AffectsActor.Always);
		base.Targeter.ShowArcToShape = false;
	}

	private int GetHealingAmountOnSelf()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_healingOnSelf;
		}
		return result;
	}

	private StandardActorEffectData GetStealthEffectData()
	{
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_useStealthEffectDataOverride)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return m_abilityMod.m_stealthEffectDataOverride;
					}
				}
			}
		}
		return m_standardActorEffectData;
	}

	public float GetCooldownResetHealthThreshold()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cooldownResetHealthThresholdMod.GetModifiedValue(m_cooldownResetHealthThreshold);
		}
		else
		{
			result = m_cooldownResetHealthThreshold;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_standardActorEffectData.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (GetHealingAmountOnSelf() > 0)
		{
			AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetHealingAmountOnSelf());
		}
		m_standardActorEffectData.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SniperGhillieSuit abilityMod_SniperGhillieSuit = modAsBase as AbilityMod_SniperGhillieSuit;
		AddTokenInt(tokens, "CostPerTurn", string.Empty, m_costPerTurn);
		m_standardActorEffectData.AddTooltipTokens(tokens, "StandardActorEffectData");
		string empty = string.Empty;
		float val;
		if ((bool)abilityMod_SniperGhillieSuit)
		{
			val = abilityMod_SniperGhillieSuit.m_cooldownResetHealthThresholdMod.GetModifiedValue(m_cooldownResetHealthThreshold);
		}
		else
		{
			val = m_cooldownResetHealthThreshold;
		}
		AddTokenFloatAsPct(tokens, "CooldownResetHealthThreshold_Pct", empty, val);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SniperGhillieSuit))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_SniperGhillieSuit);
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
	}
}
