using System;
using System.Collections.Generic;
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
		if (!(bazookaGirlDelayedMissile != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnEnemyOnCastOverride, "OnCastEnemyHitEffect", bazookaGirlDelayedMissile.m_onCastEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_damageMod, "Damage", string.Empty, bazookaGirlDelayedMissile.m_damage);
			AbilityMod.AddToken_EffectMod(tokens, m_onExplosionEffectMod, "EffectOnHit", bazookaGirlDelayedMissile.m_effectOnHit);
			if (m_cooldownReductionsWhenNoHits != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					m_cooldownReductionsWhenNoHits.AddTooltipTokens(tokens, "OnMiss");
					return;
				}
			}
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BazookaGirlDelayedMissile bazookaGirlDelayedMissile = GetTargetAbilityOnAbilityData(abilityData) as BazookaGirlDelayedMissile;
		bool flag = bazookaGirlDelayedMissile != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = bazookaGirlDelayedMissile.m_damage;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(damageMod, "[Damage]", flag, baseVal);
		empty += PropDesc(m_onExplosionEffectMod, "[EffectOnHit]", flag, (!flag) ? null : bazookaGirlDelayedMissile.m_effectOnHit);
		string str2 = empty;
		AbilityModPropertyEffectInfo effectOnEnemyOnCastOverride = m_effectOnEnemyOnCastOverride;
		object baseVal2;
		if (flag)
		{
			while (true)
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
		empty = str2 + PropDesc(effectOnEnemyOnCastOverride, "{ Effect on Enemy in Shape on Ability Cast }", flag, (StandardEffectInfo)baseVal2);
		empty += PropDesc(m_shapeMod, "[Shape]", flag, flag ? bazookaGirlDelayedMissile.m_shape : AbilityAreaShape.SingleSquare);
		if (m_useAdditionalShapeToHitInfoOverride && m_additionalShapeToHitInfoMod != null)
		{
			empty += "Using Additional Shape to Hit Info override\n";
			for (int i = 0; i < m_additionalShapeToHitInfoMod.Count; i++)
			{
				ShapeToHitInfoMod shapeToHitInfoMod = m_additionalShapeToHitInfoMod[i];
				int num;
				if (flag)
				{
					while (true)
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
					while (true)
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
						while (true)
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
				empty = empty + "* [Shape] = " + shapeToHitInfoMod.m_shape.ToString() + "\n";
				empty += PropDesc(shapeToHitInfoMod.m_damageMod, "[Damage]", flag, baseVal3);
				empty += PropDesc(shapeToHitInfoMod.m_onExplosionEffectInfo, "[Effect]");
			}
		}
		if (m_cooldownReductionsWhenNoHits.HasCooldownReduction())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			empty = empty + "Cooldown Reduction on Miss\n" + m_cooldownReductionsWhenNoHits.GetDescription(abilityData);
		}
		return empty + PropDesc(m_useFakeMarkerIndexStartMod, "[UseFakeMarkerIndexStart]", flag, flag ? bazookaGirlDelayedMissile.m_useFakeMarkerIndexStart : 0);
	}
}
