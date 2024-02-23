using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_BazookaGirlDelayedMissile : AbilityMod
{
	[Serializable]
	public class ShapeToHitInfoMod
	{
		public AbilityAreaShape m_shape;
		public AbilityModPropertyInt m_damageMod;
		public AbilityModPropertyEffectInfo m_onExplosionEffectInfo;
	}

	[Header("-- Shape and Layered Shapes")]
	public AbilityModPropertyShape m_shapeMod;
	public bool m_useAdditionalShapeToHitInfoOverride;
	public List<ShapeToHitInfoMod> m_additionalShapeToHitInfoMod = new List<ShapeToHitInfoMod>();
	[Header("-- Fake Markers")]
	public AbilityModPropertyInt m_useFakeMarkerIndexStartMod;
	[Header("-- On Delayed Impact Damage and Effect")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyEffectInfo m_onExplosionEffectMod;
	[Header("-- Effect to Enemy on Ability Cast")]
	public AbilityModPropertyEffectInfo m_effectOnEnemyOnCastOverride;
	[Header("-- Cooldown Reduction On Miss")]
	public AbilityModCooldownReduction m_cooldownReductionsWhenNoHits;

	public override Type GetTargetAbilityType()
	{
		return typeof(BazookaGirlDelayedMissile);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BazookaGirlDelayedMissile bazookaGirlDelayedMissile = targetAbility as BazookaGirlDelayedMissile;
		if (bazookaGirlDelayedMissile != null)
		{
			AddToken_EffectMod(tokens, m_effectOnEnemyOnCastOverride, "OnCastEnemyHitEffect", bazookaGirlDelayedMissile.m_onCastEnemyHitEffect);
			AddToken(tokens, m_damageMod, "Damage", string.Empty, bazookaGirlDelayedMissile.m_damage);
			AddToken_EffectMod(tokens, m_onExplosionEffectMod, "EffectOnHit", bazookaGirlDelayedMissile.m_effectOnHit);
			if (m_cooldownReductionsWhenNoHits != null)
			{
				m_cooldownReductionsWhenNoHits.AddTooltipTokens(tokens, "OnMiss");
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BazookaGirlDelayedMissile bazookaGirlDelayedMissile = GetTargetAbilityOnAbilityData(abilityData) as BazookaGirlDelayedMissile;
		bool isAbilityPresent = bazookaGirlDelayedMissile != null;
		string desc = string.Empty;
		desc += PropDesc(m_damageMod, "[Damage]", isAbilityPresent, isAbilityPresent ? bazookaGirlDelayedMissile.m_damage : 0);
		desc += PropDesc(m_onExplosionEffectMod, "[EffectOnHit]", isAbilityPresent, (!isAbilityPresent) ? null : bazookaGirlDelayedMissile.m_effectOnHit);
		desc += PropDesc(m_effectOnEnemyOnCastOverride, "{ Effect on Enemy in Shape on Ability Cast }", isAbilityPresent, isAbilityPresent ? bazookaGirlDelayedMissile.m_onCastEnemyHitEffect : null);
		desc += PropDesc(m_shapeMod, "[Shape]", isAbilityPresent, isAbilityPresent ? bazookaGirlDelayedMissile.m_shape : AbilityAreaShape.SingleSquare);
		if (m_useAdditionalShapeToHitInfoOverride && m_additionalShapeToHitInfoMod != null)
		{
			desc += "Using Additional Shape to Hit Info override\n";
			for (int i = 0; i < m_additionalShapeToHitInfoMod.Count; i++)
			{
				ShapeToHitInfoMod shapeToHitInfoMod = m_additionalShapeToHitInfoMod[i];
				int damage = isAbilityPresent ? bazookaGirlDelayedMissile.m_damage : 0;
				if (bazookaGirlDelayedMissile != null
				    && bazookaGirlDelayedMissile.m_additionalShapeToHitInfo.Count > i)
				{
					damage = bazookaGirlDelayedMissile.m_additionalShapeToHitInfo[i].m_damage;
				}

				desc += new StringBuilder().Append("* [Shape] = ").Append(shapeToHitInfoMod.m_shape).Append("\n").ToString();
				desc += PropDesc(shapeToHitInfoMod.m_damageMod, "[Damage]", isAbilityPresent, damage);
				desc += PropDesc(shapeToHitInfoMod.m_onExplosionEffectInfo, "[Effect]");
			}
		}
		if (m_cooldownReductionsWhenNoHits.HasCooldownReduction())
		{
			desc += new StringBuilder().Append("Cooldown Reduction on Miss\n").Append(m_cooldownReductionsWhenNoHits.GetDescription(abilityData)).ToString();
		}
		return new StringBuilder().Append(desc).Append(PropDesc(m_useFakeMarkerIndexStartMod, "[UseFakeMarkerIndexStart]", isAbilityPresent, isAbilityPresent ? bazookaGirlDelayedMissile.m_useFakeMarkerIndexStart : 0)).ToString();
	}
}
