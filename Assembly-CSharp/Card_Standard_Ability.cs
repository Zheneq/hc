using System;
using System.Collections.Generic;
using UnityEngine;

public class Card_Standard_Ability : Ability
{
	[Header("-- Targeting --")]
	public bool m_targetCenterAtCaster;

	[Header("-- On Hit --")]
	public int m_healAmount = 0x1E;

	public int m_techPointsAmount;

	[Tooltip("Credits to give to the actor who used the card.")]
	public int m_personalCredits;

	[Tooltip("Credits to give to each actor on the team of the actor who used the card (including the card user).")]
	public int m_teamCredits;

	public bool m_applyEffect;

	public StandardActorEffectData m_effect;

	public bool m_overrideEffectRunPhase;

	public AbilityPriority m_phaseOverrideValue = AbilityPriority.Combat_Damage;

	private void Start()
	{
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.m_healAmount);
		AbilityTooltipHelper.ReportEnergy(ref result, AbilityTooltipSubject.Self, this.m_techPointsAmount);
		if (this.m_applyEffect)
		{
			this.m_effect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		int val = Mathf.Max(0, this.m_healAmount + this.m_effect.m_healingPerTurn * Mathf.Max(0, this.m_effect.m_duration - 1));
		base.AddTokenInt(tokens, "TotalHeal", string.Empty, val, false);
		base.AddTokenInt(tokens, "HealAmount", string.Empty, this.m_healAmount, false);
		base.AddTokenInt(tokens, "TechPointsAmount", string.Empty, this.m_techPointsAmount, false);
		base.AddTokenInt(tokens, "PersonalCredits", string.Empty, this.m_personalCredits, false);
		base.AddTokenInt(tokens, "TeamCredits", string.Empty, this.m_teamCredits, false);
		this.m_effect.AddTooltipTokens(tokens, "Effect", false, null);
	}
}
