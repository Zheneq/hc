using System;
using System.Collections.Generic;

public class AbilityMod_IceborgDamageArea : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_Shape m_targetSelectMod;

	[Separator("Targeting, Max Ranges", true)]
	public AbilityModPropertyFloat m_initialCastMaxRangeMod;

	public AbilityModPropertyFloat m_moveAreaCastMaxRangeMod;

	public AbilityModPropertyBool m_targetingAreaCheckLosMod;

	[Separator("Whether to add damage field", true)]
	public AbilityModPropertyBool m_addGroundFieldMod;

	public AbilityModPropertyBool m_stopMoversWithSlowStatusMod;

	public AbilityModPropertyBool m_stopMoverIfHitPreviousTurnMod;

	public AbilityModPropertyGroundEffectField m_groundFieldDataMod;

	[Separator("Extra Damage on Initial Cast", true)]
	public AbilityModPropertyInt m_extraDamageOnInitialCastMod;

	[Separator("Damage change on ground field per turn", true)]
	public AbilityModPropertyInt m_groundFieldDamageChangePerTurnMod;

	[Separator("Min Damage", true)]
	public AbilityModPropertyInt m_minDamageMod;

	[Separator("Shielding per enemy hit on cast", true)]
	public AbilityModPropertyInt m_shieldPerEnemyHitMod;

	public AbilityModPropertyInt m_shieldDurationMod;

	[Separator("Effect to apply if target has been hit by this ability on previous turn", true)]
	public AbilityModPropertyEffectInfo m_effectOnEnemyIfHitPreviousTurnMod;

	[Separator("Apply Nova effect?", true)]
	public AbilityModPropertyBool m_applyDelayedAoeEffectMod;

	public AbilityModPropertyBool m_applyNovaCoreIfHitPreviousTurnMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(IceborgDamageArea);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgDamageArea iceborgDamageArea = targetAbility as IceborgDamageArea;
		if (!(iceborgDamageArea != null))
		{
			return;
		}
		while (true)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, m_initialCastMaxRangeMod, "InitialCastMaxRange", string.Empty, iceborgDamageArea.m_initialCastMaxRange);
			AbilityMod.AddToken(tokens, m_moveAreaCastMaxRangeMod, "MoveAreaCastMaxRange", string.Empty, iceborgDamageArea.m_moveAreaCastMaxRange);
			AbilityMod.AddToken_GroundFieldMod(tokens, m_groundFieldDataMod, "GroundFieldData", iceborgDamageArea.m_groundFieldData);
			AbilityMod.AddToken(tokens, m_extraDamageOnInitialCastMod, "ExtraDamageOnInitialCast", string.Empty, iceborgDamageArea.m_extraDamageOnInitialCast);
			AbilityMod.AddToken(tokens, m_groundFieldDamageChangePerTurnMod, "GroundFieldDamageChangePerTurn", string.Empty, iceborgDamageArea.m_groundFieldDamageChangePerTurn);
			AbilityMod.AddToken(tokens, m_minDamageMod, "MinDamage", string.Empty, iceborgDamageArea.m_minDamage);
			AbilityMod.AddToken(tokens, m_shieldPerEnemyHitMod, "ShieldPerEnemyHit", string.Empty, iceborgDamageArea.m_shieldPerEnemyHit);
			AbilityMod.AddToken(tokens, m_shieldDurationMod, "ShieldDuration", string.Empty, iceborgDamageArea.m_shieldDuration);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnEnemyIfHitPreviousTurnMod, "EffectOnEnemyIfHitPreviousTurn", iceborgDamageArea.m_effectOnEnemyIfHitPreviousTurn);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgDamageArea iceborgDamageArea = GetTargetAbilityOnAbilityData(abilityData) as IceborgDamageArea;
		bool flag = iceborgDamageArea != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (iceborgDamageArea != null)
		{
			text += GetTargetSelectModDesc(m_targetSelectMod, iceborgDamageArea.m_targetSelectComp, "-- Target Select --");
			string str = text;
			AbilityModPropertyFloat initialCastMaxRangeMod = m_initialCastMaxRangeMod;
			float baseVal;
			if (flag)
			{
				baseVal = iceborgDamageArea.m_initialCastMaxRange;
			}
			else
			{
				baseVal = 0f;
			}
			text = str + PropDesc(initialCastMaxRangeMod, "[InitialCastMaxRange]", flag, baseVal);
			string str2 = text;
			AbilityModPropertyFloat moveAreaCastMaxRangeMod = m_moveAreaCastMaxRangeMod;
			float baseVal2;
			if (flag)
			{
				baseVal2 = iceborgDamageArea.m_moveAreaCastMaxRange;
			}
			else
			{
				baseVal2 = 0f;
			}
			text = str2 + PropDesc(moveAreaCastMaxRangeMod, "[MoveAreaCastMaxRange]", flag, baseVal2);
			string str3 = text;
			AbilityModPropertyBool targetingAreaCheckLosMod = m_targetingAreaCheckLosMod;
			int baseVal3;
			if (flag)
			{
				baseVal3 = (iceborgDamageArea.m_targetingAreaCheckLos ? 1 : 0);
			}
			else
			{
				baseVal3 = 0;
			}
			text = str3 + PropDesc(targetingAreaCheckLosMod, "[TargetingAreaCheckLos]", flag, (byte)baseVal3 != 0);
			text += PropDesc(m_addGroundFieldMod, "[AddGroundField]", flag, flag && iceborgDamageArea.m_addGroundField);
			string str4 = text;
			AbilityModPropertyBool stopMoversWithSlowStatusMod = m_stopMoversWithSlowStatusMod;
			int baseVal4;
			if (flag)
			{
				baseVal4 = (iceborgDamageArea.m_stopMoversWithSlowStatus ? 1 : 0);
			}
			else
			{
				baseVal4 = 0;
			}
			text = str4 + PropDesc(stopMoversWithSlowStatusMod, "[StopMoversWithSlowStatus]", flag, (byte)baseVal4 != 0);
			string str5 = text;
			AbilityModPropertyBool stopMoverIfHitPreviousTurnMod = m_stopMoverIfHitPreviousTurnMod;
			int baseVal5;
			if (flag)
			{
				baseVal5 = (iceborgDamageArea.m_stopMoverIfHitPreviousTurn ? 1 : 0);
			}
			else
			{
				baseVal5 = 0;
			}
			text = str5 + PropDesc(stopMoverIfHitPreviousTurnMod, "[StopMoverIfHitPreviousTurn]", flag, (byte)baseVal5 != 0);
			text += PropDescGroundFieldMod(m_groundFieldDataMod, "{ GroundFieldData }", iceborgDamageArea.m_groundFieldData);
			text += PropDesc(m_extraDamageOnInitialCastMod, "[ExtraDamageOnInitialCast]", flag, flag ? iceborgDamageArea.m_extraDamageOnInitialCast : 0);
			string str6 = text;
			AbilityModPropertyInt groundFieldDamageChangePerTurnMod = m_groundFieldDamageChangePerTurnMod;
			int baseVal6;
			if (flag)
			{
				baseVal6 = iceborgDamageArea.m_groundFieldDamageChangePerTurn;
			}
			else
			{
				baseVal6 = 0;
			}
			text = str6 + PropDesc(groundFieldDamageChangePerTurnMod, "[GroundFieldDamageChangePerTurn]", flag, baseVal6);
			string str7 = text;
			AbilityModPropertyInt minDamageMod = m_minDamageMod;
			int baseVal7;
			if (flag)
			{
				baseVal7 = iceborgDamageArea.m_minDamage;
			}
			else
			{
				baseVal7 = 0;
			}
			text = str7 + PropDesc(minDamageMod, "[MinDamage]", flag, baseVal7);
			text += PropDesc(m_shieldPerEnemyHitMod, "[ShieldPerEnemyHit]", flag, flag ? iceborgDamageArea.m_shieldPerEnemyHit : 0);
			string str8 = text;
			AbilityModPropertyInt shieldDurationMod = m_shieldDurationMod;
			int baseVal8;
			if (flag)
			{
				baseVal8 = iceborgDamageArea.m_shieldDuration;
			}
			else
			{
				baseVal8 = 0;
			}
			text = str8 + PropDesc(shieldDurationMod, "[ShieldDuration]", flag, baseVal8);
			text += PropDesc(m_effectOnEnemyIfHitPreviousTurnMod, "[EffectOnEnemyIfHitPreviousTurn]", flag, (!flag) ? null : iceborgDamageArea.m_effectOnEnemyIfHitPreviousTurn);
			string str9 = text;
			AbilityModPropertyBool applyDelayedAoeEffectMod = m_applyDelayedAoeEffectMod;
			int baseVal9;
			if (flag)
			{
				baseVal9 = (iceborgDamageArea.m_applyDelayedAoeEffect ? 1 : 0);
			}
			else
			{
				baseVal9 = 0;
			}
			text = str9 + PropDesc(applyDelayedAoeEffectMod, "[ApplyDelayedAoeEffect]", flag, (byte)baseVal9 != 0);
			string str10 = text;
			AbilityModPropertyBool applyNovaCoreIfHitPreviousTurnMod = m_applyNovaCoreIfHitPreviousTurnMod;
			int baseVal10;
			if (flag)
			{
				baseVal10 = (iceborgDamageArea.m_applyNovaCoreIfHitPreviousTurn ? 1 : 0);
			}
			else
			{
				baseVal10 = 0;
			}
			text = str10 + PropDesc(applyNovaCoreIfHitPreviousTurnMod, "[ApplyNovaCoreIfHitPreviousTurn]", flag, (byte)baseVal10 != 0);
		}
		return text;
	}
}
