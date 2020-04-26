using System;
using System.Collections.Generic;

public class AbilityMod_IceborgConeOrLaser : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_ConeOrLaser m_targetSelectMod;

	[Separator("Shielding per enemy hit on cast", true)]
	public AbilityModPropertyInt m_shieldPerEnemyHitMod;

	public AbilityModPropertyInt m_shieldDurationMod;

	[Separator("Apply Nova effect?", true)]
	public AbilityModPropertyBool m_applyDelayedAoeEffectMod;

	public AbilityModPropertyBool m_skipDelayedAoeEffectIfHasExistingMod;

	[Separator("Cdr Per Hit Enemy with Nova Core", true)]
	public AbilityModPropertyInt m_cdrPerEnemyWithNovaCoreMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(IceborgConeOrLaser);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgConeOrLaser iceborgConeOrLaser = targetAbility as IceborgConeOrLaser;
		if (!(iceborgConeOrLaser != null))
		{
			return;
		}
		while (true)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, m_shieldPerEnemyHitMod, "ShieldPerEnemyHit", string.Empty, iceborgConeOrLaser.m_shieldPerEnemyHit);
			AbilityMod.AddToken(tokens, m_shieldDurationMod, "ShieldDuration", string.Empty, iceborgConeOrLaser.m_shieldDuration);
			AbilityMod.AddToken(tokens, m_cdrPerEnemyWithNovaCoreMod, "CdrPerEnemyWithNovaCore", string.Empty, iceborgConeOrLaser.m_cdrPerEnemyWithNovaCore);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgConeOrLaser iceborgConeOrLaser = GetTargetAbilityOnAbilityData(abilityData) as IceborgConeOrLaser;
		bool flag = iceborgConeOrLaser != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (iceborgConeOrLaser != null)
		{
			text += GetTargetSelectModDesc(m_targetSelectMod, iceborgConeOrLaser.m_targetSelectComp, "-- Target Select --");
			text += PropDesc(m_shieldPerEnemyHitMod, "[ShieldPerEnemyHit]", flag, flag ? iceborgConeOrLaser.m_shieldPerEnemyHit : 0);
			string str = text;
			AbilityModPropertyInt shieldDurationMod = m_shieldDurationMod;
			int baseVal;
			if (flag)
			{
				baseVal = iceborgConeOrLaser.m_shieldDuration;
			}
			else
			{
				baseVal = 0;
			}
			text = str + PropDesc(shieldDurationMod, "[ShieldDuration]", flag, baseVal);
			string str2 = text;
			AbilityModPropertyBool applyDelayedAoeEffectMod = m_applyDelayedAoeEffectMod;
			int baseVal2;
			if (flag)
			{
				baseVal2 = (iceborgConeOrLaser.m_applyDelayedAoeEffect ? 1 : 0);
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + PropDesc(applyDelayedAoeEffectMod, "[ApplyDelayedAoeEffect]", flag, (byte)baseVal2 != 0);
			string str3 = text;
			AbilityModPropertyBool skipDelayedAoeEffectIfHasExistingMod = m_skipDelayedAoeEffectIfHasExistingMod;
			int baseVal3;
			if (flag)
			{
				baseVal3 = (iceborgConeOrLaser.m_skipDelayedAoeEffectIfHasExisting ? 1 : 0);
			}
			else
			{
				baseVal3 = 0;
			}
			text = str3 + PropDesc(skipDelayedAoeEffectIfHasExistingMod, "[SkipDelayedAoeEffectIfHasExisting]", flag, (byte)baseVal3 != 0);
			text += PropDesc(m_cdrPerEnemyWithNovaCoreMod, "[CdrPerEnemyWithNovaCore]", flag, flag ? iceborgConeOrLaser.m_cdrPerEnemyWithNovaCore : 0);
		}
		return text;
	}
}
