using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SparkEnergized : AbilityMod
{
	public AbilityModPropertyEffectData m_allyBuffEffectMod;

	public AbilityModPropertyEffectData m_enemyDebuffEffectMod;

	public AbilityModPropertyInt m_healAmtPerBeamMod;

	[Header("-- Heal/Damage boost mod")]
	public AbilityModPropertyInt m_additionalHealMod;

	public AbilityModPropertyInt m_additionalDamageMod;

	[Header("-- Heal on Self (from tether), will apply regardless of whether Radiate is used")]
	public AbilityModPropertyInt m_healOnSelfFromTetherMod;

	public AbilityModPropertyInt m_energyOnSelfFromTetherMod;

	[Header("-- Need to Choose Target (if false, targeting all attached)")]
	public AbilityModPropertyBool m_needToChooseTargetMod;

	[Header("-- Effect on Enemy on start of Next Turn")]
	public StandardEffectInfo m_effectOnEnemyOnNextTurn;

	[Separator("Effect and Boosted Heal/Damage when both tethers are attached", true)]
	public AbilityModPropertyEffectInfo m_bothTetherExtraEffectOnSelfMod;

	public AbilityModPropertyEffectInfo m_bothTetherAllyEffectMod;

	public AbilityModPropertyEffectInfo m_bothTetherEnemyEffectMod;

	[Space(10f)]
	public AbilityModPropertyInt m_bothTetherExtraHealMod;

	public AbilityModPropertyInt m_bothTetherExtraDamageMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SparkEnergized);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SparkEnergized sparkEnergized = targetAbility as SparkEnergized;
		if (sparkEnergized != null)
		{
			SparkHealingBeam component = targetAbility.GetComponent<SparkHealingBeam>();
			SparkBasicAttack component2 = targetAbility.GetComponent<SparkBasicAttack>();
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyBuffEffectMod, "AllyBuffEffect", sparkEnergized.m_allyBuffEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyDebuffEffectMod, "EnemyDebuffEffect", sparkEnergized.m_enemyDebuffEffect, true);
			AbilityMod.AddToken(tokens, this.m_healAmtPerBeamMod, "HealAmtPerBeam", string.Empty, sparkEnergized.m_healAmtPerBeam, true, false);
			AbilityMod.AddToken(tokens, this.m_additionalHealMod, "Heal_Additional", "additional heal", (!component) ? 0 : component.m_additionalEnergizedHealing, component != null, false);
			AbilityModPropertyInt additionalDamageMod = this.m_additionalDamageMod;
			string tokenName = "Damage_Additional";
			string desc = "additional damage";
			int baseVal;
			if (component2)
			{
				baseVal = component2.m_additionalEnergizedDamage;
			}
			else
			{
				baseVal = 0;
			}
			AbilityMod.AddToken(tokens, additionalDamageMod, tokenName, desc, baseVal, component2 != null, false);
			if (component != null)
			{
				AbilityModPropertyInt healOnSelfFromTetherMod = this.m_healOnSelfFromTetherMod;
				string tokenName2 = "Heal_TetherOnSelf";
				string desc2 = "heal on self from tether";
				int baseVal2;
				if (component)
				{
					baseVal2 = component.m_healOnSelfOnTick;
				}
				else
				{
					baseVal2 = 0;
				}
				AbilityMod.AddToken(tokens, healOnSelfFromTetherMod, tokenName2, desc2, baseVal2, component != null, false);
				AbilityModPropertyInt energyOnSelfFromTetherMod = this.m_energyOnSelfFromTetherMod;
				string tokenName3 = "Energy_TetherOnSelf";
				string desc3 = "energy on self from tether";
				int baseVal3;
				if (component)
				{
					baseVal3 = component.m_energyOnCasterPerTurn;
				}
				else
				{
					baseVal3 = 0;
				}
				AbilityMod.AddToken(tokens, energyOnSelfFromTetherMod, tokenName3, desc3, baseVal3, component != null, false);
			}
			AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnEnemyOnNextTurn, "EffectOnEnemyNextTurn", null, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_bothTetherExtraEffectOnSelfMod, "BothTetherExtraEffectOnSelf", sparkEnergized.m_bothTetherExtraEffectOnSelf, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_bothTetherAllyEffectMod, "BothTetherAllyEffect", sparkEnergized.m_bothTetherAllyEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_bothTetherEnemyEffectMod, "BothTetherEnemyEffect", sparkEnergized.m_bothTetherEnemyEffect, true);
			AbilityMod.AddToken(tokens, this.m_bothTetherExtraHealMod, "BothTetherExtraHeal", string.Empty, sparkEnergized.m_bothTetherExtraHeal, true, false);
			AbilityMod.AddToken(tokens, this.m_bothTetherExtraDamageMod, "BothTetherExtraDamage", string.Empty, sparkEnergized.m_bothTetherExtraDamage, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SparkEnergized sparkEnergized = base.GetTargetAbilityOnAbilityData(abilityData) as SparkEnergized;
		SparkHealingBeam sparkHealingBeam;
		if (sparkEnergized)
		{
			sparkHealingBeam = sparkEnergized.GetComponent<SparkHealingBeam>();
		}
		else
		{
			sparkHealingBeam = null;
		}
		SparkHealingBeam sparkHealingBeam2 = sparkHealingBeam;
		SparkBasicAttack sparkBasicAttack;
		if (sparkEnergized)
		{
			sparkBasicAttack = sparkEnergized.GetComponent<SparkBasicAttack>();
		}
		else
		{
			sparkBasicAttack = null;
		}
		SparkBasicAttack sparkBasicAttack2 = sparkBasicAttack;
		bool flag = sparkEnergized != null;
		bool flag2 = flag && sparkHealingBeam2 != null;
		bool flag3;
		if (flag)
		{
			flag3 = (sparkBasicAttack2 != null);
		}
		else
		{
			flag3 = false;
		}
		bool flag4 = flag3;
		string text = string.Empty;
		text += base.PropDesc(this.m_allyBuffEffectMod, "[AllyBuffEffect]", flag, (!flag) ? null : sparkEnergized.m_allyBuffEffect);
		string str = text;
		AbilityModPropertyEffectData enemyDebuffEffectMod = this.m_enemyDebuffEffectMod;
		string prefix = "[EnemyDebuffEffect]";
		bool showBaseVal = flag;
		StandardActorEffectData baseVal;
		if (flag)
		{
			baseVal = sparkEnergized.m_enemyDebuffEffect;
		}
		else
		{
			baseVal = null;
		}
		text = str + base.PropDesc(enemyDebuffEffectMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt healAmtPerBeamMod = this.m_healAmtPerBeamMod;
		string prefix2 = "[HealAmtPerBeam]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = sparkEnergized.m_healAmtPerBeam;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(healAmtPerBeamMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt additionalHealMod = this.m_additionalHealMod;
		string prefix3 = "[Additional Heal]";
		bool showBaseVal3 = flag2;
		int baseVal3;
		if (flag2)
		{
			baseVal3 = sparkHealingBeam2.m_additionalEnergizedHealing;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(additionalHealMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt additionalDamageMod = this.m_additionalDamageMod;
		string prefix4 = "[Additional Damage]";
		bool showBaseVal4 = flag4;
		int baseVal4;
		if (flag4)
		{
			baseVal4 = sparkBasicAttack2.m_additionalEnergizedDamage;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(additionalDamageMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt healOnSelfFromTetherMod = this.m_healOnSelfFromTetherMod;
		string prefix5 = "[Heal on Self from Tether]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			if (sparkHealingBeam2 != null)
			{
				baseVal5 = sparkHealingBeam2.m_healOnSelfOnTick;
				goto IL_1C9;
			}
		}
		baseVal5 = 0;
		IL_1C9:
		text = str5 + AbilityModHelper.GetModPropertyDesc(healOnSelfFromTetherMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt energyOnSelfFromTetherMod = this.m_energyOnSelfFromTetherMod;
		string prefix6 = "[EnergyOnSelfFromTether]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			if (sparkHealingBeam2 != null)
			{
				baseVal6 = sparkHealingBeam2.m_energyOnCasterPerTurn;
				goto IL_213;
			}
		}
		baseVal6 = 0;
		IL_213:
		text = str6 + base.PropDesc(energyOnSelfFromTetherMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyBool needToChooseTargetMod = this.m_needToChooseTargetMod;
		string prefix7 = "[Need to Choose Target?]";
		bool showBaseVal7 = flag;
		bool baseVal7;
		if (flag)
		{
			baseVal7 = sparkEnergized.m_needToSelectTarget;
		}
		else
		{
			baseVal7 = true;
		}
		text = str7 + AbilityModHelper.GetModPropertyDesc(needToChooseTargetMod, prefix7, showBaseVal7, baseVal7);
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_effectOnEnemyOnNextTurn, "[Effect on Enemy on Next Turn]", string.Empty, flag, null);
		string str8 = text;
		AbilityModPropertyEffectInfo bothTetherExtraEffectOnSelfMod = this.m_bothTetherExtraEffectOnSelfMod;
		string prefix8 = "[BothTetherExtraEffectOnSelf]";
		bool showBaseVal8 = flag;
		StandardEffectInfo baseVal8;
		if (flag)
		{
			baseVal8 = sparkEnergized.m_bothTetherExtraEffectOnSelf;
		}
		else
		{
			baseVal8 = null;
		}
		text = str8 + base.PropDesc(bothTetherExtraEffectOnSelfMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyEffectInfo bothTetherAllyEffectMod = this.m_bothTetherAllyEffectMod;
		string prefix9 = "[BothTetherAllyEffect]";
		bool showBaseVal9 = flag;
		StandardEffectInfo baseVal9;
		if (flag)
		{
			baseVal9 = sparkEnergized.m_bothTetherAllyEffect;
		}
		else
		{
			baseVal9 = null;
		}
		text = str9 + base.PropDesc(bothTetherAllyEffectMod, prefix9, showBaseVal9, baseVal9);
		text += base.PropDesc(this.m_bothTetherEnemyEffectMod, "[BothTetherEnemyEffect]", flag, (!flag) ? null : sparkEnergized.m_bothTetherEnemyEffect);
		string str10 = text;
		AbilityModPropertyInt bothTetherExtraHealMod = this.m_bothTetherExtraHealMod;
		string prefix10 = "[BothTetherExtraHeal]";
		bool showBaseVal10 = flag;
		int baseVal10;
		if (flag)
		{
			baseVal10 = sparkEnergized.m_bothTetherExtraHeal;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(bothTetherExtraHealMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyInt bothTetherExtraDamageMod = this.m_bothTetherExtraDamageMod;
		string prefix11 = "[BothTetherExtraDamage]";
		bool showBaseVal11 = flag;
		int baseVal11;
		if (flag)
		{
			baseVal11 = sparkEnergized.m_bothTetherExtraDamage;
		}
		else
		{
			baseVal11 = 0;
		}
		return str11 + base.PropDesc(bothTetherExtraDamageMod, prefix11, showBaseVal11, baseVal11);
	}
}
