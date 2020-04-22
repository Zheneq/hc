using AbilityContextNamespace;
using System;
using System.Collections.Generic;

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
		targetSelect.SetTargetSelectMod(m_targetSelMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		DinoTargetedKnockback dinoTargetedKnockback = targetAbility as DinoTargetedKnockback;
		if (!(dinoTargetedKnockback != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, m_extraDamageIfFullPowerLayerConeMod, "ExtraDamageIfFullPowerLayerCone", string.Empty, dinoTargetedKnockback.m_extraDamageIfFullPowerLayerCone);
			AbilityMod.AddToken(tokens, m_shieldPerEnemyHitMod, "ShieldPerEnemyHit", string.Empty, dinoTargetedKnockback.m_shieldPerEnemyHit);
			AbilityMod.AddToken(tokens, m_shieldDurationMod, "ShieldDuration", string.Empty, dinoTargetedKnockback.m_shieldDuration);
			AddOnHitDataTokens(tokens, m_knockbackDestOnHitDataMod, dinoTargetedKnockback.m_knockbackDestOnHitData);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		DinoTargetedKnockback dinoTargetedKnockback = GetTargetAbilityOnAbilityData(abilityData) as DinoTargetedKnockback;
		bool flag = dinoTargetedKnockback != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (dinoTargetedKnockback != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			text += GetTargetSelectModDesc(m_targetSelMod, dinoTargetedKnockback.m_targetSelectComp);
			text += PropDesc(m_extraDamageIfFullPowerLayerConeMod, "[ExtraDamageIfFullPowerLayerCone]", flag, flag ? dinoTargetedKnockback.m_extraDamageIfFullPowerLayerCone : 0);
			string str = text;
			AbilityModPropertyInt shieldPerEnemyHitMod = m_shieldPerEnemyHitMod;
			int baseVal;
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
				baseVal = dinoTargetedKnockback.m_shieldPerEnemyHit;
			}
			else
			{
				baseVal = 0;
			}
			text = str + PropDesc(shieldPerEnemyHitMod, "[ShieldPerEnemyHit]", flag, baseVal);
			text += PropDesc(m_shieldDurationMod, "[ShieldDuration]", flag, flag ? dinoTargetedKnockback.m_shieldDuration : 0);
			string str2 = text;
			AbilityModPropertyBool doHitsAroundKnockbackDestMod = m_doHitsAroundKnockbackDestMod;
			int baseVal2;
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
				baseVal2 = (dinoTargetedKnockback.m_doHitsAroundKnockbackDest ? 1 : 0);
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + PropDesc(doHitsAroundKnockbackDestMod, "[DoHitsAroundKnockbackDest]", flag, (byte)baseVal2 != 0);
			string str3 = text;
			AbilityModPropertyShape hitsAroundKnockbackDestShapeMod = m_hitsAroundKnockbackDestShapeMod;
			int baseVal3;
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
				baseVal3 = (int)dinoTargetedKnockback.m_hitsAroundKnockbackDestShape;
			}
			else
			{
				baseVal3 = 0;
			}
			text = str3 + PropDesc(hitsAroundKnockbackDestShapeMod, "[HitsAroundKnockbackDestShape]", flag, (AbilityAreaShape)baseVal3);
			text += GetOnHitDataDesc(m_knockbackDestOnHitDataMod, dinoTargetedKnockback.m_knockbackDestOnHitData);
		}
		return text;
	}
}
