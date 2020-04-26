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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Flash";
		}
		int num;
		if (m_casterHitEffect.m_applyEffect)
		{
			num = 2;
		}
		else
		{
			num = 0;
		}
		AbilityUtil_Targeter.AffectsActor affectsCaster = (AbilityUtil_Targeter.AffectsActor)num;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, m_shape, m_penetrateLos, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, m_affectEnemies, m_affectAllies, affectsCaster, AbilityUtil_Targeter.AffectsActor.Never);
		base.Targeter.ShowArcToShape = false;
		if (m_tags == null)
		{
			return;
		}
		while (true)
		{
			if (!m_tags.Contains(AbilityTags.UseTeleportUIEffect))
			{
				while (true)
				{
					m_tags.Add(AbilityTags.UseTeleportUIEffect);
					return;
				}
			}
			return;
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
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
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
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "HealAmount", string.Empty, m_healAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_casterHitEffect, "CasterHitEffect", m_casterHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damageAmount));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, m_healAmount));
		return list;
	}
}
