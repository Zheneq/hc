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
		targetSelect.SetTargetSelectMod(this.m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgConeOrLaser iceborgConeOrLaser = targetAbility as IceborgConeOrLaser;
		if (iceborgConeOrLaser != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_IceborgConeOrLaser.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, this.m_shieldPerEnemyHitMod, "ShieldPerEnemyHit", string.Empty, iceborgConeOrLaser.m_shieldPerEnemyHit, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldDurationMod, "ShieldDuration", string.Empty, iceborgConeOrLaser.m_shieldDuration, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrPerEnemyWithNovaCoreMod, "CdrPerEnemyWithNovaCore", string.Empty, iceborgConeOrLaser.m_cdrPerEnemyWithNovaCore, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgConeOrLaser iceborgConeOrLaser = base.GetTargetAbilityOnAbilityData(abilityData) as IceborgConeOrLaser;
		bool flag = iceborgConeOrLaser != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (iceborgConeOrLaser != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_IceborgConeOrLaser.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			text += base.GetTargetSelectModDesc(this.m_targetSelectMod, iceborgConeOrLaser.m_targetSelectComp, "-- Target Select --");
			text += base.PropDesc(this.m_shieldPerEnemyHitMod, "[ShieldPerEnemyHit]", flag, (!flag) ? 0 : iceborgConeOrLaser.m_shieldPerEnemyHit);
			string str = text;
			AbilityModPropertyInt shieldDurationMod = this.m_shieldDurationMod;
			string prefix = "[ShieldDuration]";
			bool showBaseVal = flag;
			int baseVal;
			if (flag)
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
				baseVal = iceborgConeOrLaser.m_shieldDuration;
			}
			else
			{
				baseVal = 0;
			}
			text = str + base.PropDesc(shieldDurationMod, prefix, showBaseVal, baseVal);
			string str2 = text;
			AbilityModPropertyBool applyDelayedAoeEffectMod = this.m_applyDelayedAoeEffectMod;
			string prefix2 = "[ApplyDelayedAoeEffect]";
			bool showBaseVal2 = flag;
			bool baseVal2;
			if (flag)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				baseVal2 = iceborgConeOrLaser.m_applyDelayedAoeEffect;
			}
			else
			{
				baseVal2 = false;
			}
			text = str2 + base.PropDesc(applyDelayedAoeEffectMod, prefix2, showBaseVal2, baseVal2);
			string str3 = text;
			AbilityModPropertyBool skipDelayedAoeEffectIfHasExistingMod = this.m_skipDelayedAoeEffectIfHasExistingMod;
			string prefix3 = "[SkipDelayedAoeEffectIfHasExisting]";
			bool showBaseVal3 = flag;
			bool baseVal3;
			if (flag)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				baseVal3 = iceborgConeOrLaser.m_skipDelayedAoeEffectIfHasExisting;
			}
			else
			{
				baseVal3 = false;
			}
			text = str3 + base.PropDesc(skipDelayedAoeEffectIfHasExistingMod, prefix3, showBaseVal3, baseVal3);
			text += base.PropDesc(this.m_cdrPerEnemyWithNovaCoreMod, "[CdrPerEnemyWithNovaCore]", flag, (!flag) ? 0 : iceborgConeOrLaser.m_cdrPerEnemyWithNovaCore);
		}
		return text;
	}
}
