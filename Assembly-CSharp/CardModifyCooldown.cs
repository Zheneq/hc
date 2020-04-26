using System.Collections.Generic;
using UnityEngine;

public class CardModifyCooldown : Ability
{
	[Header("-- Whether to delay cooldown modification till end of combat --")]
	public bool m_modifyCooldownOnEndOfCombat;

	[Header("-- Cooldown Modify Amount")]
	public int m_cooldownModificationAmount = -1;

	public int m_minCooldownAmount;

	public int m_maxCooldownAmount = 100;

	[Header("-- Affected Abilities")]
	public bool m_reduceAbility0 = true;

	public bool m_reduceAbility1 = true;

	public bool m_reduceAbility2 = true;

	public bool m_reduceAbility3 = true;

	public bool m_reduceAbility4 = true;

	[Header("-- Targeting")]
	public bool m_shapeCenterAroundCaster = true;

	public AbilityAreaShape m_shape;

	public bool m_penetrateLineOfSight = true;

	public bool m_friendly = true;

	private void Start()
	{
		base.Targeter = new AbilityUtil_Targeter_Shape(this, m_shape, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, !m_friendly, m_friendly);
		base.Targeter.ShowArcToShape = false;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "CooldownModificationAmount", string.Empty, Mathf.Abs(m_cooldownModificationAmount));
		AddTokenInt(tokens, "MinCooldownAmount", string.Empty, m_minCooldownAmount);
		AddTokenInt(tokens, "MaxCooldownAmount", string.Empty, m_maxCooldownAmount);
	}
}
