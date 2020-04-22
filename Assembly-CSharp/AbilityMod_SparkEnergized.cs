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
		if (!(sparkEnergized != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			SparkHealingBeam component = targetAbility.GetComponent<SparkHealingBeam>();
			SparkBasicAttack component2 = targetAbility.GetComponent<SparkBasicAttack>();
			AbilityMod.AddToken_EffectMod(tokens, m_allyBuffEffectMod, "AllyBuffEffect", sparkEnergized.m_allyBuffEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyDebuffEffectMod, "EnemyDebuffEffect", sparkEnergized.m_enemyDebuffEffect);
			AbilityMod.AddToken(tokens, m_healAmtPerBeamMod, "HealAmtPerBeam", string.Empty, sparkEnergized.m_healAmtPerBeam);
			AbilityMod.AddToken(tokens, m_additionalHealMod, "Heal_Additional", "additional heal", component ? component.m_additionalEnergizedHealing : 0, component != null);
			AbilityModPropertyInt additionalDamageMod = m_additionalDamageMod;
			int baseVal;
			if ((bool)component2)
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
				baseVal = component2.m_additionalEnergizedDamage;
			}
			else
			{
				baseVal = 0;
			}
			AbilityMod.AddToken(tokens, additionalDamageMod, "Damage_Additional", "additional damage", baseVal, component2 != null);
			if (component != null)
			{
				AbilityModPropertyInt healOnSelfFromTetherMod = m_healOnSelfFromTetherMod;
				int baseVal2;
				if ((bool)component)
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
					baseVal2 = component.m_healOnSelfOnTick;
				}
				else
				{
					baseVal2 = 0;
				}
				AbilityMod.AddToken(tokens, healOnSelfFromTetherMod, "Heal_TetherOnSelf", "heal on self from tether", baseVal2, component != null);
				AbilityModPropertyInt energyOnSelfFromTetherMod = m_energyOnSelfFromTetherMod;
				int baseVal3;
				if ((bool)component)
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
					baseVal3 = component.m_energyOnCasterPerTurn;
				}
				else
				{
					baseVal3 = 0;
				}
				AbilityMod.AddToken(tokens, energyOnSelfFromTetherMod, "Energy_TetherOnSelf", "energy on self from tether", baseVal3, component != null);
			}
			AbilityMod.AddToken_EffectInfo(tokens, m_effectOnEnemyOnNextTurn, "EffectOnEnemyNextTurn");
			AbilityMod.AddToken_EffectMod(tokens, m_bothTetherExtraEffectOnSelfMod, "BothTetherExtraEffectOnSelf", sparkEnergized.m_bothTetherExtraEffectOnSelf);
			AbilityMod.AddToken_EffectMod(tokens, m_bothTetherAllyEffectMod, "BothTetherAllyEffect", sparkEnergized.m_bothTetherAllyEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_bothTetherEnemyEffectMod, "BothTetherEnemyEffect", sparkEnergized.m_bothTetherEnemyEffect);
			AbilityMod.AddToken(tokens, m_bothTetherExtraHealMod, "BothTetherExtraHeal", string.Empty, sparkEnergized.m_bothTetherExtraHeal);
			AbilityMod.AddToken(tokens, m_bothTetherExtraDamageMod, "BothTetherExtraDamage", string.Empty, sparkEnergized.m_bothTetherExtraDamage);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SparkEnergized sparkEnergized = GetTargetAbilityOnAbilityData(abilityData) as SparkEnergized;
		object obj;
		if ((bool)sparkEnergized)
		{
			while (true)
			{
				switch (5)
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
			obj = sparkEnergized.GetComponent<SparkHealingBeam>();
		}
		else
		{
			obj = null;
		}
		SparkHealingBeam sparkHealingBeam = (SparkHealingBeam)obj;
		object obj2;
		if ((bool)sparkEnergized)
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
			obj2 = sparkEnergized.GetComponent<SparkBasicAttack>();
		}
		else
		{
			obj2 = null;
		}
		SparkBasicAttack sparkBasicAttack = (SparkBasicAttack)obj2;
		bool flag = sparkEnergized != null;
		bool flag2 = flag && sparkHealingBeam != null;
		int num;
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
			num = ((sparkBasicAttack != null) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag3 = (byte)num != 0;
		string empty = string.Empty;
		empty += PropDesc(m_allyBuffEffectMod, "[AllyBuffEffect]", flag, (!flag) ? null : sparkEnergized.m_allyBuffEffect);
		string str = empty;
		AbilityModPropertyEffectData enemyDebuffEffectMod = m_enemyDebuffEffectMod;
		object baseVal;
		if (flag)
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
			baseVal = sparkEnergized.m_enemyDebuffEffect;
		}
		else
		{
			baseVal = null;
		}
		empty = str + PropDesc(enemyDebuffEffectMod, "[EnemyDebuffEffect]", flag, (StandardActorEffectData)baseVal);
		string str2 = empty;
		AbilityModPropertyInt healAmtPerBeamMod = m_healAmtPerBeamMod;
		int baseVal2;
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
			baseVal2 = sparkEnergized.m_healAmtPerBeam;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(healAmtPerBeamMod, "[HealAmtPerBeam]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt additionalHealMod = m_additionalHealMod;
		int baseVal3;
		if (flag2)
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
			baseVal3 = sparkHealingBeam.m_additionalEnergizedHealing;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(additionalHealMod, "[Additional Heal]", flag2, baseVal3);
		string str4 = empty;
		AbilityModPropertyInt additionalDamageMod = m_additionalDamageMod;
		int baseVal4;
		if (flag3)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = sparkBasicAttack.m_additionalEnergizedDamage;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(additionalDamageMod, "[Additional Damage]", flag3, baseVal4);
		string str5 = empty;
		AbilityModPropertyInt healOnSelfFromTetherMod = m_healOnSelfFromTetherMod;
		int baseVal5;
		if (flag)
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
			if (sparkHealingBeam != null)
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
				baseVal5 = sparkHealingBeam.m_healOnSelfOnTick;
				goto IL_01c9;
			}
		}
		baseVal5 = 0;
		goto IL_01c9;
		IL_0213:
		object str6;
		object energyOnSelfFromTetherMod;
		int baseVal6;
		empty = (string)str6 + PropDesc((AbilityModPropertyInt)energyOnSelfFromTetherMod, "[EnergyOnSelfFromTether]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyBool needToChooseTargetMod = m_needToChooseTargetMod;
		int baseVal7;
		if (flag)
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
			baseVal7 = (sparkEnergized.m_needToSelectTarget ? 1 : 0);
		}
		else
		{
			baseVal7 = 1;
		}
		empty = str7 + AbilityModHelper.GetModPropertyDesc(needToChooseTargetMod, "[Need to Choose Target?]", flag, (byte)baseVal7 != 0);
		empty += AbilityModHelper.GetModEffectInfoDesc(m_effectOnEnemyOnNextTurn, "[Effect on Enemy on Next Turn]", string.Empty, flag);
		string str8 = empty;
		AbilityModPropertyEffectInfo bothTetherExtraEffectOnSelfMod = m_bothTetherExtraEffectOnSelfMod;
		object baseVal8;
		if (flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal8 = sparkEnergized.m_bothTetherExtraEffectOnSelf;
		}
		else
		{
			baseVal8 = null;
		}
		empty = str8 + PropDesc(bothTetherExtraEffectOnSelfMod, "[BothTetherExtraEffectOnSelf]", flag, (StandardEffectInfo)baseVal8);
		string str9 = empty;
		AbilityModPropertyEffectInfo bothTetherAllyEffectMod = m_bothTetherAllyEffectMod;
		object baseVal9;
		if (flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal9 = sparkEnergized.m_bothTetherAllyEffect;
		}
		else
		{
			baseVal9 = null;
		}
		empty = str9 + PropDesc(bothTetherAllyEffectMod, "[BothTetherAllyEffect]", flag, (StandardEffectInfo)baseVal9);
		empty += PropDesc(m_bothTetherEnemyEffectMod, "[BothTetherEnemyEffect]", flag, (!flag) ? null : sparkEnergized.m_bothTetherEnemyEffect);
		string str10 = empty;
		AbilityModPropertyInt bothTetherExtraHealMod = m_bothTetherExtraHealMod;
		int baseVal10;
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
			baseVal10 = sparkEnergized.m_bothTetherExtraHeal;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(bothTetherExtraHealMod, "[BothTetherExtraHeal]", flag, baseVal10);
		string str11 = empty;
		AbilityModPropertyInt bothTetherExtraDamageMod = m_bothTetherExtraDamageMod;
		int baseVal11;
		if (flag)
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
			baseVal11 = sparkEnergized.m_bothTetherExtraDamage;
		}
		else
		{
			baseVal11 = 0;
		}
		return str11 + PropDesc(bothTetherExtraDamageMod, "[BothTetherExtraDamage]", flag, baseVal11);
		IL_01c9:
		empty = str5 + AbilityModHelper.GetModPropertyDesc(healOnSelfFromTetherMod, "[Heal on Self from Tether]", flag, baseVal5);
		str6 = empty;
		energyOnSelfFromTetherMod = m_energyOnSelfFromTetherMod;
		if (flag)
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
			if (sparkHealingBeam != null)
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
				baseVal6 = sparkHealingBeam.m_energyOnCasterPerTurn;
				goto IL_0213;
			}
		}
		baseVal6 = 0;
		goto IL_0213;
	}
}
