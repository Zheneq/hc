using System;
using System.Collections.Generic;
using UnityEngine;

public class Card_KnockbackAoe_Ability : Ability
{
	[Header("-- Knockback --")]
	public float m_knockbackDist = 1f;

	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	public AbilityAreaShape m_aoeShape;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Knockback_Aoe_Catalyst";
		}
		this.Setup();
	}

	private void Setup()
	{
		AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
		AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Possible;
		base.Targeter = new AbilityUtil_Targeter_HealingKnockback(this, this.m_aoeShape, false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, affectsCaster, affectsBestTarget, this.m_knockbackDist, this.m_knockbackType);
		base.Targeter.ShowArcToShape = false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffect, "EnemyHitEffect", this.m_enemyHitEffect, true);
	}
}
