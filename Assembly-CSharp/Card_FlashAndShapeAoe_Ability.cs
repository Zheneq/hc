using System;
using System.Collections.Generic;
using UnityEngine;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Flash";
		}
		AbilityUtil_Targeter.AffectsActor affectsActor;
		if (this.m_casterHitEffect.m_applyEffect)
		{
			affectsActor = AbilityUtil_Targeter.AffectsActor.Always;
		}
		else
		{
			affectsActor = AbilityUtil_Targeter.AffectsActor.Never;
		}
		AbilityUtil_Targeter.AffectsActor affectsCaster = affectsActor;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, this.m_shape, this.m_penetrateLos, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, this.m_affectEnemies, this.m_affectAllies, affectsCaster, AbilityUtil_Targeter.AffectsActor.Never);
		base.Targeter.ShowArcToShape = false;
		if (this.m_tags != null)
		{
			if (!this.m_tags.Contains(AbilityTags.UseTeleportUIEffect))
			{
				this.m_tags.Add(AbilityTags.UseTeleportUIEffect);
			}
		}
	}

	public override bool IsFlashAbility()
	{
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe.IsBaselineHeight() && boardSquareSafe != caster.GetCurrentBoardSquare())
			{
				return true;
			}
		}
		return false;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Teleport;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, this.m_damageAmount, false);
		base.AddTokenInt(tokens, "HealAmount", string.Empty, this.m_healAmount, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_casterHitEffect, "CasterHitEffect", this.m_casterHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyHitEffect, "AllyHitEffect", this.m_allyHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffect, "EnemyHitEffect", this.m_enemyHitEffect, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.m_damageAmount),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, this.m_healAmount)
		};
	}
}
