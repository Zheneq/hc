using System;
using System.Collections.Generic;

public class AbilityMod_NekoEnlargeDisc : AbilityMod
{
	[Separator("Targeting")]
	public AbilityModPropertyFloat m_laserWidthOverrideMod;
	public AbilityModPropertyFloat m_aoeRadiusOverrideMod;
	public AbilityModPropertyFloat m_returnEndRadiusOverrideMod;
	[Separator("On Hit Damage/Effect")]
	public AbilityModPropertyInt m_additionalDamageAmountMod;
	public AbilityModPropertyEffectInfo m_effectOnEnemiesMod;
	[Separator("Ally Hits")]
	public AbilityModPropertyInt m_allyHealMod;
	public AbilityModPropertyEffectInfo m_allyHitEffectMod;
	[Separator("Shielding for target hit on return (applied on start of next turn)")]
	public AbilityModPropertyInt m_shieldPerTargetHitOnReturnMod;
	public AbilityModPropertyEffectData m_shieldEffectDataMod;
	[Separator("Cooldown Reduction")]
	public AbilityModPropertyInt m_cdrIfHitNoOneMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NekoEnlargeDisc);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NekoEnlargeDisc nekoEnlargeDisc = targetAbility as NekoEnlargeDisc;
		if (nekoEnlargeDisc != null)
		{
			AddToken(tokens, m_laserWidthOverrideMod, "LaserWidthOverride", string.Empty, nekoEnlargeDisc.m_laserWidthOverride);
			AddToken(tokens, m_aoeRadiusOverrideMod, "AoeRadiusOverride", string.Empty, nekoEnlargeDisc.m_aoeRadiusOverride);
			AddToken(tokens, m_returnEndRadiusOverrideMod, "ReturnEndRadiusOverride", string.Empty, nekoEnlargeDisc.m_returnEndRadiusOverride);
			AddToken(tokens, m_additionalDamageAmountMod, "AdditionalDamageAmount", string.Empty, nekoEnlargeDisc.m_additionalDamageAmount);
			AddToken_EffectMod(tokens, m_effectOnEnemiesMod, "EffectOnEnemies", nekoEnlargeDisc.m_effectOnEnemies);
			AddToken(tokens, m_allyHealMod, "AllyHeal", string.Empty, nekoEnlargeDisc.m_allyHeal);
			AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", nekoEnlargeDisc.m_allyHitEffect);
			AddToken(tokens, m_shieldPerTargetHitOnReturnMod, "ShieldPerTargetHitOnThrow", string.Empty, nekoEnlargeDisc.m_shieldPerTargetHitOnReturn);
			AddToken_EffectMod(tokens, m_shieldEffectDataMod, "ShieldEffectData", nekoEnlargeDisc.m_shieldEffectData);
			AddToken(tokens, m_cdrIfHitNoOneMod, "CdrIfHitNoOne", string.Empty, nekoEnlargeDisc.m_cdrIfHitNoOne);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NekoEnlargeDisc nekoEnlargeDisc = GetTargetAbilityOnAbilityData(abilityData) as NekoEnlargeDisc;
		bool isValid = nekoEnlargeDisc != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserWidthOverrideMod, "[LaserWidthOverride]", isValid, isValid ? nekoEnlargeDisc.m_laserWidthOverride : 0f);
		desc += PropDesc(m_aoeRadiusOverrideMod, "[AoeRadiusOverride]", isValid, isValid ? nekoEnlargeDisc.m_aoeRadiusOverride : 0f);
		desc += PropDesc(m_returnEndRadiusOverrideMod, "[ReturnEndRadiusOverride]", isValid, isValid ? nekoEnlargeDisc.m_returnEndRadiusOverride : 0f);
		desc += PropDesc(m_additionalDamageAmountMod, "[AdditionalDamageAmount]", isValid, isValid ? nekoEnlargeDisc.m_additionalDamageAmount : 0);
		desc += PropDesc(m_effectOnEnemiesMod, "[EffectOnEnemies]", isValid, isValid ? nekoEnlargeDisc.m_effectOnEnemies : null);
		desc += PropDesc(m_allyHealMod, "[AllyHeal]", isValid, isValid ? nekoEnlargeDisc.m_allyHeal : 0);
		desc += PropDesc(m_allyHitEffectMod, "[AllyHitEffect]", isValid, isValid ? nekoEnlargeDisc.m_allyHitEffect : null);
		desc += PropDesc(m_shieldPerTargetHitOnReturnMod, "[ShieldPerTargetHitOnThrow]", isValid, isValid ? nekoEnlargeDisc.m_shieldPerTargetHitOnReturn : 0);
		desc += PropDesc(m_shieldEffectDataMod, "[ShieldEffectData]", isValid, isValid ? nekoEnlargeDisc.m_shieldEffectData : null);
		return desc + PropDesc(m_cdrIfHitNoOneMod, "[CdrIfHitNoOne]", isValid, isValid ? nekoEnlargeDisc.m_cdrIfHitNoOne : 0);
	}
}
