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
		targetSelect.SetTargetSelectMod(this.m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgDamageArea iceborgDamageArea = targetAbility as IceborgDamageArea;
		if (iceborgDamageArea != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_IceborgDamageArea.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, this.m_initialCastMaxRangeMod, "InitialCastMaxRange", string.Empty, iceborgDamageArea.m_initialCastMaxRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_moveAreaCastMaxRangeMod, "MoveAreaCastMaxRange", string.Empty, iceborgDamageArea.m_moveAreaCastMaxRange, true, false, false);
			AbilityMod.AddToken_GroundFieldMod(tokens, this.m_groundFieldDataMod, "GroundFieldData", iceborgDamageArea.m_groundFieldData);
			AbilityMod.AddToken(tokens, this.m_extraDamageOnInitialCastMod, "ExtraDamageOnInitialCast", string.Empty, iceborgDamageArea.m_extraDamageOnInitialCast, true, false);
			AbilityMod.AddToken(tokens, this.m_groundFieldDamageChangePerTurnMod, "GroundFieldDamageChangePerTurn", string.Empty, iceborgDamageArea.m_groundFieldDamageChangePerTurn, true, false);
			AbilityMod.AddToken(tokens, this.m_minDamageMod, "MinDamage", string.Empty, iceborgDamageArea.m_minDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldPerEnemyHitMod, "ShieldPerEnemyHit", string.Empty, iceborgDamageArea.m_shieldPerEnemyHit, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldDurationMod, "ShieldDuration", string.Empty, iceborgDamageArea.m_shieldDuration, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnEnemyIfHitPreviousTurnMod, "EffectOnEnemyIfHitPreviousTurn", iceborgDamageArea.m_effectOnEnemyIfHitPreviousTurn, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgDamageArea iceborgDamageArea = base.GetTargetAbilityOnAbilityData(abilityData) as IceborgDamageArea;
		bool flag = iceborgDamageArea != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (iceborgDamageArea != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_IceborgDamageArea.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			text += base.GetTargetSelectModDesc(this.m_targetSelectMod, iceborgDamageArea.m_targetSelectComp, "-- Target Select --");
			string str = text;
			AbilityModPropertyFloat initialCastMaxRangeMod = this.m_initialCastMaxRangeMod;
			string prefix = "[InitialCastMaxRange]";
			bool showBaseVal = flag;
			float baseVal;
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
				baseVal = iceborgDamageArea.m_initialCastMaxRange;
			}
			else
			{
				baseVal = 0f;
			}
			text = str + base.PropDesc(initialCastMaxRangeMod, prefix, showBaseVal, baseVal);
			string str2 = text;
			AbilityModPropertyFloat moveAreaCastMaxRangeMod = this.m_moveAreaCastMaxRangeMod;
			string prefix2 = "[MoveAreaCastMaxRange]";
			bool showBaseVal2 = flag;
			float baseVal2;
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
				baseVal2 = iceborgDamageArea.m_moveAreaCastMaxRange;
			}
			else
			{
				baseVal2 = 0f;
			}
			text = str2 + base.PropDesc(moveAreaCastMaxRangeMod, prefix2, showBaseVal2, baseVal2);
			string str3 = text;
			AbilityModPropertyBool targetingAreaCheckLosMod = this.m_targetingAreaCheckLosMod;
			string prefix3 = "[TargetingAreaCheckLos]";
			bool showBaseVal3 = flag;
			bool baseVal3;
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
				baseVal3 = iceborgDamageArea.m_targetingAreaCheckLos;
			}
			else
			{
				baseVal3 = false;
			}
			text = str3 + base.PropDesc(targetingAreaCheckLosMod, prefix3, showBaseVal3, baseVal3);
			text += base.PropDesc(this.m_addGroundFieldMod, "[AddGroundField]", flag, flag && iceborgDamageArea.m_addGroundField);
			string str4 = text;
			AbilityModPropertyBool stopMoversWithSlowStatusMod = this.m_stopMoversWithSlowStatusMod;
			string prefix4 = "[StopMoversWithSlowStatus]";
			bool showBaseVal4 = flag;
			bool baseVal4;
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
				baseVal4 = iceborgDamageArea.m_stopMoversWithSlowStatus;
			}
			else
			{
				baseVal4 = false;
			}
			text = str4 + base.PropDesc(stopMoversWithSlowStatusMod, prefix4, showBaseVal4, baseVal4);
			string str5 = text;
			AbilityModPropertyBool stopMoverIfHitPreviousTurnMod = this.m_stopMoverIfHitPreviousTurnMod;
			string prefix5 = "[StopMoverIfHitPreviousTurn]";
			bool showBaseVal5 = flag;
			bool baseVal5;
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
				baseVal5 = iceborgDamageArea.m_stopMoverIfHitPreviousTurn;
			}
			else
			{
				baseVal5 = false;
			}
			text = str5 + base.PropDesc(stopMoverIfHitPreviousTurnMod, prefix5, showBaseVal5, baseVal5);
			text += base.PropDescGroundFieldMod(this.m_groundFieldDataMod, "{ GroundFieldData }", iceborgDamageArea.m_groundFieldData);
			text += base.PropDesc(this.m_extraDamageOnInitialCastMod, "[ExtraDamageOnInitialCast]", flag, (!flag) ? 0 : iceborgDamageArea.m_extraDamageOnInitialCast);
			string str6 = text;
			AbilityModPropertyInt groundFieldDamageChangePerTurnMod = this.m_groundFieldDamageChangePerTurnMod;
			string prefix6 = "[GroundFieldDamageChangePerTurn]";
			bool showBaseVal6 = flag;
			int baseVal6;
			if (flag)
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
				baseVal6 = iceborgDamageArea.m_groundFieldDamageChangePerTurn;
			}
			else
			{
				baseVal6 = 0;
			}
			text = str6 + base.PropDesc(groundFieldDamageChangePerTurnMod, prefix6, showBaseVal6, baseVal6);
			string str7 = text;
			AbilityModPropertyInt minDamageMod = this.m_minDamageMod;
			string prefix7 = "[MinDamage]";
			bool showBaseVal7 = flag;
			int baseVal7;
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
				baseVal7 = iceborgDamageArea.m_minDamage;
			}
			else
			{
				baseVal7 = 0;
			}
			text = str7 + base.PropDesc(minDamageMod, prefix7, showBaseVal7, baseVal7);
			text += base.PropDesc(this.m_shieldPerEnemyHitMod, "[ShieldPerEnemyHit]", flag, (!flag) ? 0 : iceborgDamageArea.m_shieldPerEnemyHit);
			string str8 = text;
			AbilityModPropertyInt shieldDurationMod = this.m_shieldDurationMod;
			string prefix8 = "[ShieldDuration]";
			bool showBaseVal8 = flag;
			int baseVal8;
			if (flag)
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
				baseVal8 = iceborgDamageArea.m_shieldDuration;
			}
			else
			{
				baseVal8 = 0;
			}
			text = str8 + base.PropDesc(shieldDurationMod, prefix8, showBaseVal8, baseVal8);
			text += base.PropDesc(this.m_effectOnEnemyIfHitPreviousTurnMod, "[EffectOnEnemyIfHitPreviousTurn]", flag, (!flag) ? null : iceborgDamageArea.m_effectOnEnemyIfHitPreviousTurn);
			string str9 = text;
			AbilityModPropertyBool applyDelayedAoeEffectMod = this.m_applyDelayedAoeEffectMod;
			string prefix9 = "[ApplyDelayedAoeEffect]";
			bool showBaseVal9 = flag;
			bool baseVal9;
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
				baseVal9 = iceborgDamageArea.m_applyDelayedAoeEffect;
			}
			else
			{
				baseVal9 = false;
			}
			text = str9 + base.PropDesc(applyDelayedAoeEffectMod, prefix9, showBaseVal9, baseVal9);
			string str10 = text;
			AbilityModPropertyBool applyNovaCoreIfHitPreviousTurnMod = this.m_applyNovaCoreIfHitPreviousTurnMod;
			string prefix10 = "[ApplyNovaCoreIfHitPreviousTurn]";
			bool showBaseVal10 = flag;
			bool baseVal10;
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
				baseVal10 = iceborgDamageArea.m_applyNovaCoreIfHitPreviousTurn;
			}
			else
			{
				baseVal10 = false;
			}
			text = str10 + base.PropDesc(applyNovaCoreIfHitPreviousTurnMod, prefix10, showBaseVal10, baseVal10);
		}
		return text;
	}
}
