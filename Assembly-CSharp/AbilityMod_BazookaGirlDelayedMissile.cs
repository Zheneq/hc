using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BazookaGirlDelayedMissile : AbilityMod
{
	[Header("-- Shape and Layered Shapes")]
	public AbilityModPropertyShape m_shapeMod;

	public bool m_useAdditionalShapeToHitInfoOverride;

	public List<AbilityMod_BazookaGirlDelayedMissile.ShapeToHitInfoMod> m_additionalShapeToHitInfoMod = new List<AbilityMod_BazookaGirlDelayedMissile.ShapeToHitInfoMod>();

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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_BazookaGirlDelayedMissile.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnEnemyOnCastOverride, "OnCastEnemyHitEffect", bazookaGirlDelayedMissile.m_onCastEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_damageMod, "Damage", string.Empty, bazookaGirlDelayedMissile.m_damage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_onExplosionEffectMod, "EffectOnHit", bazookaGirlDelayedMissile.m_effectOnHit, true);
			if (this.m_cooldownReductionsWhenNoHits != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_cooldownReductionsWhenNoHits.AddTooltipTokens(tokens, "OnMiss");
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BazookaGirlDelayedMissile bazookaGirlDelayedMissile = base.GetTargetAbilityOnAbilityData(abilityData) as BazookaGirlDelayedMissile;
		bool flag = bazookaGirlDelayedMissile != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix = "[Damage]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_BazookaGirlDelayedMissile.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = bazookaGirlDelayedMissile.m_damage;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(damageMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_onExplosionEffectMod, "[EffectOnHit]", flag, (!flag) ? null : bazookaGirlDelayedMissile.m_effectOnHit);
		string str2 = text;
		AbilityModPropertyEffectInfo effectOnEnemyOnCastOverride = this.m_effectOnEnemyOnCastOverride;
		string prefix2 = "{ Effect on Enemy in Shape on Ability Cast }";
		bool showBaseVal2 = flag;
		StandardEffectInfo baseVal2;
		if (flag)
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
			baseVal2 = bazookaGirlDelayedMissile.m_onCastEnemyHitEffect;
		}
		else
		{
			baseVal2 = null;
		}
		text = str2 + base.PropDesc(effectOnEnemyOnCastOverride, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_shapeMod, "[Shape]", flag, (!flag) ? AbilityAreaShape.SingleSquare : bazookaGirlDelayedMissile.m_shape);
		if (this.m_useAdditionalShapeToHitInfoOverride && this.m_additionalShapeToHitInfoMod != null)
		{
			text += "Using Additional Shape to Hit Info override\n";
			for (int i = 0; i < this.m_additionalShapeToHitInfoMod.Count; i++)
			{
				AbilityMod_BazookaGirlDelayedMissile.ShapeToHitInfoMod shapeToHitInfoMod = this.m_additionalShapeToHitInfoMod[i];
				int num;
				if (flag)
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
					num = bazookaGirlDelayedMissile.m_damage;
				}
				else
				{
					num = 0;
				}
				int baseVal3 = num;
				if (bazookaGirlDelayedMissile != null)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (bazookaGirlDelayedMissile.m_additionalShapeToHitInfo.Count > i)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						baseVal3 = bazookaGirlDelayedMissile.m_additionalShapeToHitInfo[i].m_damage;
					}
				}
				text = text + "* [Shape] = " + shapeToHitInfoMod.m_shape.ToString() + "\n";
				text += base.PropDesc(shapeToHitInfoMod.m_damageMod, "[Damage]", flag, baseVal3);
				text += base.PropDesc(shapeToHitInfoMod.m_onExplosionEffectInfo, "[Effect]", false, null);
			}
		}
		if (this.m_cooldownReductionsWhenNoHits.HasCooldownReduction())
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
			text = text + "Cooldown Reduction on Miss\n" + this.m_cooldownReductionsWhenNoHits.GetDescription(abilityData);
		}
		return text + base.PropDesc(this.m_useFakeMarkerIndexStartMod, "[UseFakeMarkerIndexStart]", flag, (!flag) ? 0 : bazookaGirlDelayedMissile.m_useFakeMarkerIndexStart);
	}

	[Serializable]
	public class ShapeToHitInfoMod
	{
		public AbilityAreaShape m_shape;

		public AbilityModPropertyInt m_damageMod;

		public AbilityModPropertyEffectInfo m_onExplosionEffectInfo;
	}
}
