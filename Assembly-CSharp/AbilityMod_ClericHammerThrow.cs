using System;
using System.Collections.Generic;
using System.Text;

public class AbilityMod_ClericHammerThrow : AbilityMod
{
	[Separator("Targeting", true)]
	public AbilityModPropertyFloat m_maxDistToRingCenterMod;
	public AbilityModPropertyFloat m_outerRadiusMod;
	public AbilityModPropertyFloat m_innerRadiusMod;
	public AbilityModPropertyBool m_ignoreLosMod;
	public AbilityModPropertyBool m_clampRingToCursorPosMod;
	[Separator("On Hit", true)]
	public AbilityModPropertyInt m_outerHitDamageMod;
	public AbilityModPropertyEffectInfo m_outerEnemyHitEffectMod;
	public AbilityModPropertyInt m_innerHitDamageMod;
	public AbilityModPropertyEffectInfo m_innerEnemyHitEffectMod;
	public AbilityModPropertyEffectInfo m_outerEnemyHitEffectWithNoInnerHits;
	public AbilityModPropertyInt m_extraInnerDamagePerOuterHit;
	public AbilityModPropertyInt m_extraTechPointGainInAreaBuff;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClericHammerThrow);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClericHammerThrow clericHammerThrow = targetAbility as ClericHammerThrow;
		if (clericHammerThrow != null)
		{
			AddToken(tokens, m_maxDistToRingCenterMod, "MaxDistToRingCenter", string.Empty, clericHammerThrow.m_maxDistToRingCenter);
			AddToken(tokens, m_outerRadiusMod, "OuterRadius", string.Empty, clericHammerThrow.m_outerRadius);
			AddToken(tokens, m_innerRadiusMod, "InnerRadius", string.Empty, clericHammerThrow.m_innerRadius);
			AddToken(tokens, m_outerHitDamageMod, "OuterHitDamage", string.Empty, clericHammerThrow.m_outerHitDamage);
			AddToken_EffectMod(tokens, m_outerEnemyHitEffectMod, "OuterEnemyHitEffect", clericHammerThrow.m_outerEnemyHitEffect);
			AddToken(tokens, m_innerHitDamageMod, "InnerHitDamage", string.Empty, clericHammerThrow.m_innerHitDamage);
			AddToken_EffectMod(tokens, m_innerEnemyHitEffectMod, "InnerEnemyHitEffect", clericHammerThrow.m_innerEnemyHitEffect);
			AddToken_EffectMod(tokens, m_outerEnemyHitEffectWithNoInnerHits, "OuterEnemyHitEffectWithNoInnerHits");
			AddToken(tokens, m_extraInnerDamagePerOuterHit, "ExtraInnerDamagePerOuterHit", string.Empty, 0);
			AddToken(tokens, m_extraTechPointGainInAreaBuff, "ExtraEnergyGainInAreaBuff", string.Empty, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClericHammerThrow clericHammerThrow = GetTargetAbilityOnAbilityData(abilityData) as ClericHammerThrow;
		bool isValid = clericHammerThrow != null;
		string desc = string.Empty;
		desc += PropDesc(m_maxDistToRingCenterMod, "[MaxDistToRingCenter]", isValid, isValid ? clericHammerThrow.m_maxDistToRingCenter : 0f);
		desc += PropDesc(m_outerRadiusMod, "[OuterRadius]", isValid, isValid ? clericHammerThrow.m_outerRadius : 0f);
		desc += PropDesc(m_innerRadiusMod, "[InnerRadius]", isValid, isValid ? clericHammerThrow.m_innerRadius : 0f);
		desc += PropDesc(m_ignoreLosMod, "[IgnoreLos]", isValid, isValid && clericHammerThrow.m_ignoreLos);
		desc += PropDesc(m_clampRingToCursorPosMod, "[ClampRingToCursorPos]", isValid, isValid && clericHammerThrow.m_clampRingToCursorPos);
		desc += PropDesc(m_outerHitDamageMod, "[OuterHitDamage]", isValid, isValid ? clericHammerThrow.m_outerHitDamage : 0);
		desc += PropDesc(m_outerEnemyHitEffectMod, "[OuterEnemyHitEffect]", isValid, isValid ? clericHammerThrow.m_outerEnemyHitEffect : null);
		desc += PropDesc(m_innerHitDamageMod, "[InnerHitDamage]", isValid, isValid ? clericHammerThrow.m_innerHitDamage : 0);
		desc += PropDesc(m_innerEnemyHitEffectMod, "[InnerEnemyHitEffect]", isValid, isValid ? clericHammerThrow.m_innerEnemyHitEffect : null);
		desc += PropDesc(m_outerEnemyHitEffectWithNoInnerHits, "[OuterEnemyHitEffectWithNoInnerHits]", isValid);
		desc += PropDesc(m_extraInnerDamagePerOuterHit, "[ExtraInnerDamagePerOuterHit]", isValid);
		return new StringBuilder().Append(desc).Append(PropDesc(m_extraTechPointGainInAreaBuff, "[ExtraEnergyGainInAreaBuff]", isValid)).ToString();
	}
}
