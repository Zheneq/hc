using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GroundEffectField
{
	public int duration = 2;

	public int hitDelayTurns;

	public AbilityAreaShape shape = AbilityAreaShape.Three_x_Three;

	public bool canIncludeCaster = true;

	[Header("-- Whether to ignore movement hits")]
	public bool ignoreMovementHits;

	public bool endIfHasDoneHits;

	public bool ignoreNonCasterAllies;

	[Header("-- For Hits --")]
	public int damageAmount;

	public int subsequentDamageAmount;

	public int healAmount;

	public int subsequentHealAmount;

	public int energyGain;

	public int subsequentEnergyGain;

	public bool stopMovementInField;

	public bool stopMovementOutOfField;

	public StandardEffectInfo effectOnEnemies;

	public StandardEffectInfo effectOnAllies;

	[Header("-- Sequences --")]
	public GameObject persistentSequencePrefab;

	public GameObject hitPulseSequencePrefab;

	public GameObject allyHitSequencePrefab;

	public GameObject enemyHitSequencePrefab;

	public bool perSquareSequences;

	internal bool penetrateLos => true;

	public void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject enemySubject, AbilityTooltipSubject allySubject)
	{
		effectOnEnemies.ReportAbilityTooltipNumbers(ref numbers, enemySubject);
		effectOnAllies.ReportAbilityTooltipNumbers(ref numbers, allySubject);
		AbilityTooltipHelper.ReportDamage(ref numbers, enemySubject, damageAmount);
		AbilityTooltipHelper.ReportHealing(ref numbers, allySubject, healAmount);
		AbilityTooltipHelper.ReportEnergy(ref numbers, allySubject, energyGain);
	}

	public bool IncludeEnemies()
	{
		return damageAmount > 0 || effectOnEnemies.m_applyEffect;
	}

	public bool IncludeAllies()
	{
		int result;
		if (healAmount <= 0)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!effectOnAllies.m_applyEffect)
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
				result = ((energyGain > 0) ? 1 : 0);
				goto IL_003f;
			}
		}
		result = 1;
		goto IL_003f;
		IL_003f:
		return (byte)result != 0;
	}

	public List<Team> GetAffectedTeams(ActorData allyActor)
	{
		return TargeterUtils.GetRelevantTeams(allyActor, IncludeAllies(), IncludeEnemies());
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens, string name, bool addCompare = false, GroundEffectField other = null)
	{
		int num;
		if (addCompare)
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
			num = ((other != null) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		string name2 = name + "_Duration";
		string empty = string.Empty;
		int val = duration;
		int otherVal;
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
			otherVal = other.duration;
		}
		else
		{
			otherVal = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name2, empty, val, flag, otherVal);
		string name3 = name + "_HitDelayTurns";
		string empty2 = string.Empty;
		int val2 = hitDelayTurns;
		int otherVal2;
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
			otherVal2 = other.hitDelayTurns;
		}
		else
		{
			otherVal2 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name3, empty2, val2, flag, otherVal2);
		string name4 = name + "_Damage";
		string empty3 = string.Empty;
		int val3 = damageAmount;
		int otherVal3;
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
			otherVal3 = other.damageAmount;
		}
		else
		{
			otherVal3 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name4, empty3, val3, flag, otherVal3);
		string name5 = name + "_SubsequentDamage";
		string empty4 = string.Empty;
		int val4 = subsequentDamageAmount;
		int otherVal4;
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
			otherVal4 = other.subsequentDamageAmount;
		}
		else
		{
			otherVal4 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name5, empty4, val4, flag, otherVal4);
		string name6 = name + "_Healing";
		string empty5 = string.Empty;
		int val5 = healAmount;
		int otherVal5;
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
			otherVal5 = other.healAmount;
		}
		else
		{
			otherVal5 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name6, empty5, val5, flag, otherVal5);
		string name7 = name + "_SubsequentHealing";
		string empty6 = string.Empty;
		int val6 = subsequentHealAmount;
		int otherVal6;
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
			otherVal6 = other.subsequentHealAmount;
		}
		else
		{
			otherVal6 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name7, empty6, val6, flag, otherVal6);
		string name8 = name + "_AllyEnergyGain";
		string empty7 = string.Empty;
		int val7 = energyGain;
		int otherVal7;
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
			otherVal7 = other.energyGain;
		}
		else
		{
			otherVal7 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name8, empty7, val7, flag, otherVal7);
		string name9 = name + "_SubsequentEnergyGain";
		string empty8 = string.Empty;
		int val8 = subsequentEnergyGain;
		int otherVal8;
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
			otherVal8 = other.subsequentEnergyGain;
		}
		else
		{
			otherVal8 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name9, empty8, val8, flag, otherVal8);
		StandardEffectInfo effectInfo = effectOnEnemies;
		object baseVal;
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
			baseVal = other.effectOnEnemies;
		}
		else
		{
			baseVal = null;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", (StandardEffectInfo)baseVal, flag);
		StandardEffectInfo effectInfo2 = effectOnAllies;
		object baseVal2;
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
			baseVal2 = other.effectOnAllies;
		}
		else
		{
			baseVal2 = null;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "AllyHitEffect", (StandardEffectInfo)baseVal2, flag);
	}

	public GroundEffectField GetShallowCopy()
	{
		return (GroundEffectField)MemberwiseClone();
	}

	public string GetInEditorDescription(string header = "GroundEffectField", string indent = "    ", bool diff = false, GroundEffectField other = null)
	{
		int num;
		if (diff)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = ((other != null) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		string str = "\n";
		string otherSep = "\t        \t | in base  =";
		string str2 = InEditorDescHelper.BoldedStirng(header) + str;
		if (duration <= 0)
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
			str2 = str2 + indent + "WARNING: IS PERMANENT (duration <= 0). Woof Woof Woof Woof\n";
		}
		else
		{
			str2 += InEditorDescHelper.AssembleFieldWithDiff("[ Max Duration ] = ", indent, otherSep, duration, flag, flag ? other.duration : 0);
		}
		if (hitDelayTurns > 0)
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
			str2 += InEditorDescHelper.AssembleFieldWithDiff("[ Hit Delay Turns ] = ", indent, otherSep, hitDelayTurns, flag, flag ? other.hitDelayTurns : 0);
		}
		string str3 = str2;
		object myVal = shape;
		int num2;
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
			num2 = (int)other.shape;
		}
		else
		{
			num2 = 0;
		}
		str2 = str3 + InEditorDescHelper.AssembleFieldWithDiff("[ Shape ] = ", indent, otherSep, (Enum)myVal, flag, (AbilityAreaShape)num2);
		string str4 = str2;
		bool myVal2 = ignoreMovementHits;
		int otherVal;
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
			otherVal = (other.ignoreMovementHits ? 1 : 0);
		}
		else
		{
			otherVal = 0;
		}
		str2 = str4 + InEditorDescHelper.AssembleFieldWithDiff("[ Ignore Movement Hits? ] = ", indent, otherSep, myVal2, flag, (byte)otherVal != 0);
		string str5 = str2;
		bool myVal3 = endIfHasDoneHits;
		int otherVal2;
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
			otherVal2 = (other.endIfHasDoneHits ? 1 : 0);
		}
		else
		{
			otherVal2 = 0;
		}
		str2 = str5 + InEditorDescHelper.AssembleFieldWithDiff("[ End If Has Done Hits? ] = ", indent, otherSep, myVal3, flag, (byte)otherVal2 != 0);
		str2 += InEditorDescHelper.AssembleFieldWithDiff("[ Ignore Non Caster Allies? ] = ", indent, otherSep, ignoreNonCasterAllies, flag, flag && other.ignoreNonCasterAllies);
		string str6 = str2;
		float myVal4 = damageAmount;
		int num3;
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
			num3 = other.damageAmount;
		}
		else
		{
			num3 = 0;
		}
		str2 = str6 + InEditorDescHelper.AssembleFieldWithDiff("[ Damage ] = ", indent, otherSep, myVal4, flag, num3);
		str2 += InEditorDescHelper.AssembleFieldWithDiff("[ Subsequent Damage ] = ", indent, otherSep, subsequentDamageAmount, flag, flag ? other.subsequentDamageAmount : 0);
		string str7 = str2;
		float myVal5 = healAmount;
		int num4;
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
			num4 = other.healAmount;
		}
		else
		{
			num4 = 0;
		}
		str2 = str7 + InEditorDescHelper.AssembleFieldWithDiff("[ Healing ] = ", indent, otherSep, myVal5, flag, num4);
		string str8 = str2;
		float myVal6 = subsequentHealAmount;
		int num5;
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
			num5 = other.subsequentHealAmount;
		}
		else
		{
			num5 = 0;
		}
		str2 = str8 + InEditorDescHelper.AssembleFieldWithDiff("[ Subsequent Healing ] = ", indent, otherSep, myVal6, flag, num5);
		string str9 = str2;
		float myVal7 = energyGain;
		int num6;
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
			num6 = other.energyGain;
		}
		else
		{
			num6 = 0;
		}
		str2 = str9 + InEditorDescHelper.AssembleFieldWithDiff("[ EnergyGain ] = ", indent, otherSep, myVal7, flag, num6);
		str2 += InEditorDescHelper.AssembleFieldWithDiff("[ Subsequent EnergyGain ] = ", indent, otherSep, subsequentEnergyGain, flag, flag ? other.subsequentEnergyGain : 0);
		if (effectOnEnemies.m_applyEffect)
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
			str2 = str2 + indent + "Effect on Enemies:\n";
			string str10 = str2;
			StandardActorEffectData effectData = effectOnEnemies.m_effectData;
			object other2;
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
				other2 = other.effectOnEnemies.m_effectData;
			}
			else
			{
				other2 = null;
			}
			str2 = str10 + effectData.GetInEditorDescription(indent, false, flag, (StandardActorEffectData)other2);
		}
		if (effectOnAllies.m_applyEffect)
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
			str2 = str2 + indent + "Effect on Allies:\n";
			string str11 = str2;
			StandardActorEffectData effectData2 = effectOnAllies.m_effectData;
			object other3;
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
				other3 = other.effectOnAllies.m_effectData;
			}
			else
			{
				other3 = null;
			}
			str2 = str11 + effectData2.GetInEditorDescription(indent, false, flag, (StandardActorEffectData)other3);
		}
		string str12 = str2;
		GameObject myVal8 = persistentSequencePrefab;
		object otherVal3;
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
			otherVal3 = other.persistentSequencePrefab;
		}
		else
		{
			otherVal3 = null;
		}
		str2 = str12 + InEditorDescHelper.AssembleFieldWithDiff("Persistent Sequence Prefab", indent, otherSep, myVal8, flag, (GameObject)otherVal3);
		str2 += InEditorDescHelper.AssembleFieldWithDiff("Hit Pulse Sequence", indent, otherSep, hitPulseSequencePrefab, flag, (!flag) ? null : other.hitPulseSequencePrefab);
		string str13 = str2;
		GameObject myVal9 = allyHitSequencePrefab;
		object otherVal4;
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
			otherVal4 = other.allyHitSequencePrefab;
		}
		else
		{
			otherVal4 = null;
		}
		str2 = str13 + InEditorDescHelper.AssembleFieldWithDiff("Ally Hit Sequence", indent, otherSep, myVal9, flag, (GameObject)otherVal4);
		str2 += InEditorDescHelper.AssembleFieldWithDiff("Enemy Hit Sequence", indent, otherSep, enemyHitSequencePrefab, flag, (!flag) ? null : other.enemyHitSequencePrefab);
		return str2 + "-  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -\n";
	}
}
