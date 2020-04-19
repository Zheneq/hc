using System;
using System.Collections.Generic;
using AbilityContextNamespace;

public class AbilityMod_DinoTargetedKnockback : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_LaserTargetedPull m_targetSelMod;

	[Separator("Extra Damage, Shielding", true)]
	public AbilityModPropertyInt m_extraDamageIfFullPowerLayerConeMod;

	public AbilityModPropertyInt m_shieldPerEnemyHitMod;

	public AbilityModPropertyInt m_shieldDurationMod;

	[Separator("For hits around knockback destinations", true)]
	public AbilityModPropertyBool m_doHitsAroundKnockbackDestMod;

	public AbilityModPropertyShape m_hitsAroundKnockbackDestShapeMod;

	[Separator("On Hit Data Mod for knockback destination hit", "yellow")]
	public OnHitDataMod m_knockbackDestOnHitDataMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(DinoTargetedKnockback);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(this.m_targetSelMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		DinoTargetedKnockback dinoTargetedKnockback = targetAbility as DinoTargetedKnockback;
		if (dinoTargetedKnockback != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_DinoTargetedKnockback.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, this.m_extraDamageIfFullPowerLayerConeMod, "ExtraDamageIfFullPowerLayerCone", string.Empty, dinoTargetedKnockback.m_extraDamageIfFullPowerLayerCone, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldPerEnemyHitMod, "ShieldPerEnemyHit", string.Empty, dinoTargetedKnockback.m_shieldPerEnemyHit, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldDurationMod, "ShieldDuration", string.Empty, dinoTargetedKnockback.m_shieldDuration, true, false);
			base.AddOnHitDataTokens(tokens, this.m_knockbackDestOnHitDataMod, dinoTargetedKnockback.m_knockbackDestOnHitData);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		DinoTargetedKnockback dinoTargetedKnockback = base.GetTargetAbilityOnAbilityData(abilityData) as DinoTargetedKnockback;
		bool flag = dinoTargetedKnockback != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (dinoTargetedKnockback != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_DinoTargetedKnockback.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			text += base.GetTargetSelectModDesc(this.m_targetSelMod, dinoTargetedKnockback.m_targetSelectComp, "-- Target Select Mod --");
			text += base.PropDesc(this.m_extraDamageIfFullPowerLayerConeMod, "[ExtraDamageIfFullPowerLayerCone]", flag, (!flag) ? 0 : dinoTargetedKnockback.m_extraDamageIfFullPowerLayerCone);
			string str = text;
			AbilityModPropertyInt shieldPerEnemyHitMod = this.m_shieldPerEnemyHitMod;
			string prefix = "[ShieldPerEnemyHit]";
			bool showBaseVal = flag;
			int baseVal;
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
				baseVal = dinoTargetedKnockback.m_shieldPerEnemyHit;
			}
			else
			{
				baseVal = 0;
			}
			text = str + base.PropDesc(shieldPerEnemyHitMod, prefix, showBaseVal, baseVal);
			text += base.PropDesc(this.m_shieldDurationMod, "[ShieldDuration]", flag, (!flag) ? 0 : dinoTargetedKnockback.m_shieldDuration);
			string str2 = text;
			AbilityModPropertyBool doHitsAroundKnockbackDestMod = this.m_doHitsAroundKnockbackDestMod;
			string prefix2 = "[DoHitsAroundKnockbackDest]";
			bool showBaseVal2 = flag;
			bool baseVal2;
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
				baseVal2 = dinoTargetedKnockback.m_doHitsAroundKnockbackDest;
			}
			else
			{
				baseVal2 = false;
			}
			text = str2 + base.PropDesc(doHitsAroundKnockbackDestMod, prefix2, showBaseVal2, baseVal2);
			string str3 = text;
			AbilityModPropertyShape hitsAroundKnockbackDestShapeMod = this.m_hitsAroundKnockbackDestShapeMod;
			string prefix3 = "[HitsAroundKnockbackDestShape]";
			bool showBaseVal3 = flag;
			AbilityAreaShape baseVal3;
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
				baseVal3 = dinoTargetedKnockback.m_hitsAroundKnockbackDestShape;
			}
			else
			{
				baseVal3 = AbilityAreaShape.SingleSquare;
			}
			text = str3 + base.PropDesc(hitsAroundKnockbackDestShapeMod, prefix3, showBaseVal3, baseVal3);
			text += base.GetOnHitDataDesc(this.m_knockbackDestOnHitDataMod, dinoTargetedKnockback.m_knockbackDestOnHitData, "-- On Hit Data Mod --");
		}
		return text;
	}
}
