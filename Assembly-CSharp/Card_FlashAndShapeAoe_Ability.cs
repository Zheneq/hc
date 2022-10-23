using System.Collections.Generic;
using UnityEngine;

// Fetter
public class Card_FlashAndShapeAoe_Ability : Ability
{
	public AbilityAreaShape m_shape;
	public bool m_penetrateLos;
	public bool m_affectEnemies;
	public bool m_affectAllies;
	[Header("-- Hit Effects")]
	public StandardEffectInfo m_casterHitEffect;
	public StandardEffectInfo m_allyHitEffect;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Hit Damage/Heal")]
	public int m_damageAmount;
	public int m_healAmount;
	[Header("-- Cast Sequence")]
	public GameObject m_startSquareSequence;
	public GameObject m_endSquareSequence;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Flash";
		}

		AbilityUtil_Targeter.AffectsActor affectsCaster = m_casterHitEffect.m_applyEffect
			? AbilityUtil_Targeter.AffectsActor.Always
			: AbilityUtil_Targeter.AffectsActor.Never;
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			m_shape,
			m_penetrateLos,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			m_affectEnemies,
			m_affectAllies,
			affectsCaster,
			AbilityUtil_Targeter.AffectsActor.Never);
		Targeter.ShowArcToShape = false;
		if (m_tags != null && !m_tags.Contains(AbilityTags.UseTeleportUIEffect))
		{
			m_tags.Add(AbilityTags.UseTeleportUIEffect);
		}
	}

	public override bool IsFlashAbility()
	{
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		return targetSquare != null
		       && targetSquare.IsValidForGameplay()
		       && targetSquare != caster.GetCurrentBoardSquare();
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Teleport;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "HealAmount", string.Empty, m_healAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_casterHitEffect, "CasterHitEffect", m_casterHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damageAmount),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, m_healAmount)
		};
	}
}
