using System.Collections.Generic;
using UnityEngine;

public class Card_Standard_Ability : Ability
{
	[Header("-- Targeting --")]
	public bool m_targetCenterAtCaster;

	[Header("-- On Hit --")]
	public int m_healAmount = 30;

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
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false);
		base.Targeter.ShowArcToShape = false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Self, m_healAmount);
		AbilityTooltipHelper.ReportEnergy(ref number, AbilityTooltipSubject.Self, m_techPointsAmount);
		if (m_applyEffect)
		{
			m_effect.ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Self);
		}
		return number;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		int val = Mathf.Max(0, m_healAmount + m_effect.m_healingPerTurn * Mathf.Max(0, m_effect.m_duration - 1));
		AddTokenInt(tokens, "TotalHeal", string.Empty, val);
		AddTokenInt(tokens, "HealAmount", string.Empty, m_healAmount);
		AddTokenInt(tokens, "TechPointsAmount", string.Empty, m_techPointsAmount);
		AddTokenInt(tokens, "PersonalCredits", string.Empty, m_personalCredits);
		AddTokenInt(tokens, "TeamCredits", string.Empty, m_teamCredits);
		m_effect.AddTooltipTokens(tokens, "Effect");
	}
}
