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

	internal bool penetrateLos
	{
		get
		{
			return true;
		}
	}

	public void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject enemySubject, AbilityTooltipSubject allySubject)
	{
		this.effectOnEnemies.ReportAbilityTooltipNumbers(ref numbers, enemySubject);
		this.effectOnAllies.ReportAbilityTooltipNumbers(ref numbers, allySubject);
		AbilityTooltipHelper.ReportDamage(ref numbers, enemySubject, this.damageAmount);
		AbilityTooltipHelper.ReportHealing(ref numbers, allySubject, this.healAmount);
		AbilityTooltipHelper.ReportEnergy(ref numbers, allySubject, this.energyGain);
	}

	public bool IncludeEnemies()
	{
		return this.damageAmount > 0 || this.effectOnEnemies.m_applyEffect;
	}

	public bool IncludeAllies()
	{
		if (this.healAmount <= 0)
		{
			if (!this.effectOnAllies.m_applyEffect)
			{
				return this.energyGain > 0;
			}
		}
		return true;
	}

	public List<Team> GetAffectedTeams(ActorData allyActor)
	{
		return TargeterUtils.GetRelevantTeams(allyActor, this.IncludeAllies(), this.IncludeEnemies());
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens, string name, bool addCompare = false, GroundEffectField other = null)
	{
		bool flag;
		if (addCompare)
		{
			flag = (other != null);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		string name2 = name + "_Duration";
		string empty = string.Empty;
		int val = this.duration;
		bool addDiff = flag2;
		int otherVal;
		if (flag2)
		{
			otherVal = other.duration;
		}
		else
		{
			otherVal = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name2, empty, val, addDiff, otherVal);
		string name3 = name + "_HitDelayTurns";
		string empty2 = string.Empty;
		int val2 = this.hitDelayTurns;
		bool addDiff2 = flag2;
		int otherVal2;
		if (flag2)
		{
			otherVal2 = other.hitDelayTurns;
		}
		else
		{
			otherVal2 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name3, empty2, val2, addDiff2, otherVal2);
		string name4 = name + "_Damage";
		string empty3 = string.Empty;
		int val3 = this.damageAmount;
		bool addDiff3 = flag2;
		int otherVal3;
		if (flag2)
		{
			otherVal3 = other.damageAmount;
		}
		else
		{
			otherVal3 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name4, empty3, val3, addDiff3, otherVal3);
		string name5 = name + "_SubsequentDamage";
		string empty4 = string.Empty;
		int val4 = this.subsequentDamageAmount;
		bool addDiff4 = flag2;
		int otherVal4;
		if (flag2)
		{
			otherVal4 = other.subsequentDamageAmount;
		}
		else
		{
			otherVal4 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name5, empty4, val4, addDiff4, otherVal4);
		string name6 = name + "_Healing";
		string empty5 = string.Empty;
		int val5 = this.healAmount;
		bool addDiff5 = flag2;
		int otherVal5;
		if (flag2)
		{
			otherVal5 = other.healAmount;
		}
		else
		{
			otherVal5 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name6, empty5, val5, addDiff5, otherVal5);
		string name7 = name + "_SubsequentHealing";
		string empty6 = string.Empty;
		int val6 = this.subsequentHealAmount;
		bool addDiff6 = flag2;
		int otherVal6;
		if (flag2)
		{
			otherVal6 = other.subsequentHealAmount;
		}
		else
		{
			otherVal6 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name7, empty6, val6, addDiff6, otherVal6);
		string name8 = name + "_AllyEnergyGain";
		string empty7 = string.Empty;
		int val7 = this.energyGain;
		bool addDiff7 = flag2;
		int otherVal7;
		if (flag2)
		{
			otherVal7 = other.energyGain;
		}
		else
		{
			otherVal7 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name8, empty7, val7, addDiff7, otherVal7);
		string name9 = name + "_SubsequentEnergyGain";
		string empty8 = string.Empty;
		int val8 = this.subsequentEnergyGain;
		bool addDiff8 = flag2;
		int otherVal8;
		if (flag2)
		{
			otherVal8 = other.subsequentEnergyGain;
		}
		else
		{
			otherVal8 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name9, empty8, val8, addDiff8, otherVal8);
		StandardEffectInfo effectInfo = this.effectOnEnemies;
		string tokenName = "EnemyHitEffect";
		StandardEffectInfo baseVal;
		if (flag2)
		{
			baseVal = other.effectOnEnemies;
		}
		else
		{
			baseVal = null;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, tokenName, baseVal, flag2);
		StandardEffectInfo effectInfo2 = this.effectOnAllies;
		string tokenName2 = "AllyHitEffect";
		StandardEffectInfo baseVal2;
		if (flag2)
		{
			baseVal2 = other.effectOnAllies;
		}
		else
		{
			baseVal2 = null;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, tokenName2, baseVal2, flag2);
	}

	public GroundEffectField GetShallowCopy()
	{
		return (GroundEffectField)base.MemberwiseClone();
	}

	public string GetInEditorDescription(string header = "GroundEffectField", string indent = "    ", bool diff = false, GroundEffectField other = null)
	{
		bool flag;
		if (diff)
		{
			flag = (other != null);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		string str = "\n";
		string text = "\t        \t | in base  =";
		string text2 = InEditorDescHelper.BoldedStirng(header) + str;
		if (this.duration <= 0)
		{
			text2 = text2 + indent + "WARNING: IS PERMANENT (duration <= 0). Woof Woof Woof Woof\n";
		}
		else
		{
			text2 += InEditorDescHelper.AssembleFieldWithDiff("[ Max Duration ] = ", indent, text, (float)this.duration, flag2, (float)((!flag2) ? 0 : other.duration), null);
		}
		if (this.hitDelayTurns > 0)
		{
			text2 += InEditorDescHelper.AssembleFieldWithDiff("[ Hit Delay Turns ] = ", indent, text, (float)this.hitDelayTurns, flag2, (float)((!flag2) ? 0 : other.hitDelayTurns), null);
		}
		string str2 = text2;
		string header2 = "[ Shape ] = ";
		string otherSep = text;
		Enum myVal = this.shape;
		bool showOther = flag2;
		AbilityAreaShape abilityAreaShape;
		if (flag2)
		{
			abilityAreaShape = other.shape;
		}
		else
		{
			abilityAreaShape = AbilityAreaShape.SingleSquare;
		}
		text2 = str2 + InEditorDescHelper.AssembleFieldWithDiff(header2, indent, otherSep, myVal, showOther, abilityAreaShape);
		string str3 = text2;
		string header3 = "[ Ignore Movement Hits? ] = ";
		string otherSep2 = text;
		bool myVal2 = this.ignoreMovementHits;
		bool showOther2 = flag2;
		bool otherVal;
		if (flag2)
		{
			otherVal = other.ignoreMovementHits;
		}
		else
		{
			otherVal = false;
		}
		text2 = str3 + InEditorDescHelper.AssembleFieldWithDiff(header3, indent, otherSep2, myVal2, showOther2, otherVal, null);
		string str4 = text2;
		string header4 = "[ End If Has Done Hits? ] = ";
		string otherSep3 = text;
		bool myVal3 = this.endIfHasDoneHits;
		bool showOther3 = flag2;
		bool otherVal2;
		if (flag2)
		{
			otherVal2 = other.endIfHasDoneHits;
		}
		else
		{
			otherVal2 = false;
		}
		text2 = str4 + InEditorDescHelper.AssembleFieldWithDiff(header4, indent, otherSep3, myVal3, showOther3, otherVal2, null);
		text2 += InEditorDescHelper.AssembleFieldWithDiff("[ Ignore Non Caster Allies? ] = ", indent, text, this.ignoreNonCasterAllies, flag2, flag2 && other.ignoreNonCasterAllies, null);
		string str5 = text2;
		string header5 = "[ Damage ] = ";
		string otherSep4 = text;
		float myVal4 = (float)this.damageAmount;
		bool showOther4 = flag2;
		float num;
		if (flag2)
		{
			num = (float)other.damageAmount;
		}
		else
		{
			num = (float)0;
		}
		text2 = str5 + InEditorDescHelper.AssembleFieldWithDiff(header5, indent, otherSep4, myVal4, showOther4, num, null);
		text2 += InEditorDescHelper.AssembleFieldWithDiff("[ Subsequent Damage ] = ", indent, text, (float)this.subsequentDamageAmount, flag2, (float)((!flag2) ? 0 : other.subsequentDamageAmount), null);
		string str6 = text2;
		string header6 = "[ Healing ] = ";
		string otherSep5 = text;
		float myVal5 = (float)this.healAmount;
		bool showOther5 = flag2;
		float num2;
		if (flag2)
		{
			num2 = (float)other.healAmount;
		}
		else
		{
			num2 = (float)0;
		}
		text2 = str6 + InEditorDescHelper.AssembleFieldWithDiff(header6, indent, otherSep5, myVal5, showOther5, num2, null);
		string str7 = text2;
		string header7 = "[ Subsequent Healing ] = ";
		string otherSep6 = text;
		float myVal6 = (float)this.subsequentHealAmount;
		bool showOther6 = flag2;
		float num3;
		if (flag2)
		{
			num3 = (float)other.subsequentHealAmount;
		}
		else
		{
			num3 = (float)0;
		}
		text2 = str7 + InEditorDescHelper.AssembleFieldWithDiff(header7, indent, otherSep6, myVal6, showOther6, num3, null);
		string str8 = text2;
		string header8 = "[ EnergyGain ] = ";
		string otherSep7 = text;
		float myVal7 = (float)this.energyGain;
		bool showOther7 = flag2;
		float num4;
		if (flag2)
		{
			num4 = (float)other.energyGain;
		}
		else
		{
			num4 = (float)0;
		}
		text2 = str8 + InEditorDescHelper.AssembleFieldWithDiff(header8, indent, otherSep7, myVal7, showOther7, num4, null);
		text2 += InEditorDescHelper.AssembleFieldWithDiff("[ Subsequent EnergyGain ] = ", indent, text, (float)this.subsequentEnergyGain, flag2, (float)((!flag2) ? 0 : other.subsequentEnergyGain), null);
		if (this.effectOnEnemies.m_applyEffect)
		{
			text2 = text2 + indent + "Effect on Enemies:\n";
			string str9 = text2;
			StandardActorEffectData effectData = this.effectOnEnemies.m_effectData;
			bool showDivider = false;
			bool diff2 = flag2;
			StandardActorEffectData other2;
			if (flag2)
			{
				other2 = other.effectOnEnemies.m_effectData;
			}
			else
			{
				other2 = null;
			}
			text2 = str9 + effectData.GetInEditorDescription(indent, showDivider, diff2, other2);
		}
		if (this.effectOnAllies.m_applyEffect)
		{
			text2 = text2 + indent + "Effect on Allies:\n";
			string str10 = text2;
			StandardActorEffectData effectData2 = this.effectOnAllies.m_effectData;
			bool showDivider2 = false;
			bool diff3 = flag2;
			StandardActorEffectData other3;
			if (flag2)
			{
				other3 = other.effectOnAllies.m_effectData;
			}
			else
			{
				other3 = null;
			}
			text2 = str10 + effectData2.GetInEditorDescription(indent, showDivider2, diff3, other3);
		}
		string str11 = text2;
		string header9 = "Persistent Sequence Prefab";
		string otherSep8 = text;
		GameObject myVal8 = this.persistentSequencePrefab;
		bool showOther8 = flag2;
		GameObject otherVal3;
		if (flag2)
		{
			otherVal3 = other.persistentSequencePrefab;
		}
		else
		{
			otherVal3 = null;
		}
		text2 = str11 + InEditorDescHelper.AssembleFieldWithDiff(header9, indent, otherSep8, myVal8, showOther8, otherVal3);
		text2 += InEditorDescHelper.AssembleFieldWithDiff("Hit Pulse Sequence", indent, text, this.hitPulseSequencePrefab, flag2, (!flag2) ? null : other.hitPulseSequencePrefab);
		string str12 = text2;
		string header10 = "Ally Hit Sequence";
		string otherSep9 = text;
		GameObject myVal9 = this.allyHitSequencePrefab;
		bool showOther9 = flag2;
		GameObject otherVal4;
		if (flag2)
		{
			otherVal4 = other.allyHitSequencePrefab;
		}
		else
		{
			otherVal4 = null;
		}
		text2 = str12 + InEditorDescHelper.AssembleFieldWithDiff(header10, indent, otherSep9, myVal9, showOther9, otherVal4);
		text2 += InEditorDescHelper.AssembleFieldWithDiff("Enemy Hit Sequence", indent, text, this.enemyHitSequencePrefab, flag2, (!flag2) ? null : other.enemyHitSequencePrefab);
		return text2 + "-  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -\n";
	}
}
