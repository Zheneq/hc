using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class StandardBarrierData
{
	public float m_width;

	public bool m_bidirectional;

	public BlockingRules m_blocksVision;

	public BlockingRules m_blocksAbilities;

	public BlockingRules m_blocksMovement;

	public BlockingRules m_blocksMovementOnCrossover;

	public BlockingRules m_blocksPositionTargeting;

	public bool m_considerAsCover;

	[Header("-- Duration --")]
	public int m_maxDuration;

	[Header("-- Sequences for Barrier Visuals --")]
	public List<GameObject> m_barrierSequencePrefabs;

	[Header("-- Gameplay Responses on Moved-Through --")]
	public GameplayResponseForActor m_onEnemyMovedThrough;

	public GameplayResponseForActor m_onAllyMovedThrough;

	[Space(5f)]
	public bool m_removeAtTurnEndIfEnemyMovedThrough;

	public bool m_removeAtTurnEndIfAllyMovedThrough;

	[Space(5f)]
	public bool m_removeAtPhaseEndIfEnemyMovedThrough;

	public bool m_removeAtPhaseEndIfAllyMovedThrough;

	[Space(5f)]
	public int m_maxHits = -1;

	public bool m_endOnCasterDeath;

	[Header("-- Gameplay Response on Shot Block --")]
	public BarrierResponseOnShot m_responseOnShotBlock;

	[CompilerGenerated]
	private static InEditorDescHelper.GetListEntryStrDelegate<GameObject> f__mg_cache0;

	public void SetupForPattern(AbilityGridPattern pattern)
	{
		this.m_bidirectional = true;
		if (pattern != AbilityGridPattern.Plus_Two_x_Two)
		{
			if (pattern == AbilityGridPattern.Plus_Four_x_Four)
			{
				this.m_width = 4f;
			}
		}
		else
		{
			this.m_width = 2f;
		}
	}

	public virtual void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers)
	{
		this.m_onEnemyMovedThrough.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		this.m_onAllyMovedThrough.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
	}

	public StandardBarrierData GetShallowCopy()
	{
		StandardBarrierData standardBarrierData = (StandardBarrierData)base.MemberwiseClone();
		standardBarrierData.m_onEnemyMovedThrough = this.m_onEnemyMovedThrough.GetShallowCopy();
		standardBarrierData.m_onAllyMovedThrough = this.m_onAllyMovedThrough.GetShallowCopy();
		if (this.m_barrierSequencePrefabs != null)
		{
			standardBarrierData.m_barrierSequencePrefabs = new List<GameObject>(this.m_barrierSequencePrefabs);
		}
		else
		{
			standardBarrierData.m_barrierSequencePrefabs = new List<GameObject>();
		}
		standardBarrierData.m_responseOnShotBlock = this.m_responseOnShotBlock.GetShallowCopy();
		return standardBarrierData;
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens, string name, bool addCompare = false, StandardBarrierData other = null)
	{
		bool flag = addCompare && other != null;
		string name2 = name + "_Duration";
		string desc = "barrier duration";
		int maxDuration = this.m_maxDuration;
		bool addDiff = flag;
		int otherVal;
		if (flag)
		{
			otherVal = other.m_maxDuration;
		}
		else
		{
			otherVal = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name2, desc, maxDuration, addDiff, otherVal);
		string name3 = name + "_MaxHits";
		string desc2 = "max target hit count";
		int maxHits = this.m_maxHits;
		bool addDiff2 = flag;
		int otherVal2;
		if (flag)
		{
			otherVal2 = other.m_maxDuration;
		}
		else
		{
			otherVal2 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name3, desc2, maxHits, addDiff2, otherVal2);
		if (this.m_onEnemyMovedThrough != null && this.m_onEnemyMovedThrough.HasResponse())
		{
			GameplayResponseForActor onEnemyMovedThrough = this.m_onEnemyMovedThrough;
			string name4 = name + "_EnemyCross";
			bool addCompare2 = flag;
			GameplayResponseForActor other2;
			if (flag)
			{
				other2 = other.m_onEnemyMovedThrough;
			}
			else
			{
				other2 = null;
			}
			onEnemyMovedThrough.AddTooltipTokens(tokens, name4, addCompare2, other2);
		}
		if (this.m_onAllyMovedThrough != null)
		{
			if (this.m_onAllyMovedThrough.HasResponse())
			{
				GameplayResponseForActor onAllyMovedThrough = this.m_onAllyMovedThrough;
				string name5 = name + "_AllyCross";
				bool addCompare3 = flag;
				GameplayResponseForActor other3;
				if (flag)
				{
					other3 = other.m_onAllyMovedThrough;
				}
				else
				{
					other3 = null;
				}
				onAllyMovedThrough.AddTooltipTokens(tokens, name5, addCompare3, other3);
			}
		}
		if (this.m_responseOnShotBlock != null)
		{
			if (this.m_responseOnShotBlock.HasResponses())
			{
				this.m_responseOnShotBlock.AddTooltipTokens(tokens, name, flag, (!flag) ? null : other.m_responseOnShotBlock);
			}
		}
	}

	public string GetInEditorDescription(string header = "Barrier Data", string indent = "    ", bool diff = false, StandardBarrierData other = null)
	{
		bool flag = diff && other != null;
		string str = "\n";
		string text = "\t        \t | in base  =";
		string text2 = InEditorDescHelper.BoldedStirng(header) + str;
		string str2 = text2;
		string header2 = "[ Max Duration ] = ";
		string otherSep = text;
		float myVal = (float)this.m_maxDuration;
		bool showOther = flag;
		float num;
		if (flag)
		{
			num = (float)other.m_maxDuration;
		}
		else
		{
			num = (float)0;
		}
		text2 = str2 + InEditorDescHelper.AssembleFieldWithDiff(header2, indent, otherSep, myVal, showOther, num, null);
		if (this.m_maxDuration <= 0)
		{
			text2 = text2 + indent + "Barrier won't expire by time, [Max Duration] is not a positive number.\n";
		}
		string str3 = text2;
		string header3 = "[ Width ] = ";
		string otherSep2 = text;
		float width = this.m_width;
		bool showOther2 = flag;
		float otherVal;
		if (flag)
		{
			otherVal = other.m_width;
		}
		else
		{
			otherVal = 0f;
		}
		text2 = str3 + InEditorDescHelper.AssembleFieldWithDiff(header3, indent, otherSep2, width, showOther2, otherVal, null);
		text2 += InEditorDescHelper.AssembleFieldWithDiff("[ Bidirectional? ] = ", indent, text, this.m_bidirectional, flag, flag && other.m_bidirectional, null);
		text2 += InEditorDescHelper.AssembleFieldWithDiff("[ Block Vision ] = \t\t", indent, text, this.m_blocksVision, flag, (!flag) ? BlockingRules.ForNobody : other.m_blocksVision);
		text2 += InEditorDescHelper.AssembleFieldWithDiff("[ Block Ability ] = \t\t", indent, text, this.m_blocksAbilities, flag, (!flag) ? BlockingRules.ForNobody : other.m_blocksAbilities);
		text2 += InEditorDescHelper.AssembleFieldWithDiff("[ Block Movement ] = \t", indent, text, this.m_blocksMovement, flag, (!flag) ? BlockingRules.ForNobody : other.m_blocksMovement);
		text2 += InEditorDescHelper.AssembleFieldWithDiff("[ Block Move On Crossover ] = \t", indent, text, this.m_blocksMovementOnCrossover, flag, (!flag) ? BlockingRules.ForNobody : other.m_blocksMovementOnCrossover);
		text2 += InEditorDescHelper.AssembleFieldWithDiff("[ Block Position Targeting ] = \t", indent, text, this.m_blocksPositionTargeting, flag, (!flag) ? BlockingRules.ForNobody : other.m_blocksPositionTargeting);
		string str4 = text2;
		string header4 = "[ Consider as Cover ] = \t";
		string otherSep3 = text;
		bool considerAsCover = this.m_considerAsCover;
		bool showOther3 = flag;
		bool otherVal2;
		if (flag)
		{
			otherVal2 = other.m_considerAsCover;
		}
		else
		{
			otherVal2 = false;
		}
		text2 = str4 + InEditorDescHelper.AssembleFieldWithDiff(header4, indent, otherSep3, considerAsCover, showOther3, otherVal2, null);
		text2 += InEditorDescHelper.AssembleFieldWithDiff("[ Max Hits ] = ", indent, text, (float)this.m_maxHits, flag, (float)((!flag) ? 0 : other.m_maxHits), null);
		string str5 = text2;
		string header5 = "* Remove on Turn End: if Enemy Moved Through = ";
		string otherSep4 = text;
		bool removeAtTurnEndIfEnemyMovedThrough = this.m_removeAtTurnEndIfEnemyMovedThrough;
		bool showOther4 = flag;
		bool otherVal3;
		if (flag)
		{
			otherVal3 = other.m_removeAtTurnEndIfEnemyMovedThrough;
		}
		else
		{
			otherVal3 = false;
		}
		
		text2 = str5 + InEditorDescHelper.AssembleFieldWithDiff(header5, indent, otherSep4, removeAtTurnEndIfEnemyMovedThrough, showOther4, otherVal3, ((bool b) => b));
		string str6 = text2;
		string header6 = "* Remove on Turn End: if Ally Moved Through = ";
		string otherSep5 = text;
		bool removeAtTurnEndIfAllyMovedThrough = this.m_removeAtTurnEndIfAllyMovedThrough;
		bool showOther5 = flag;
		bool otherVal4;
		if (flag)
		{
			otherVal4 = other.m_removeAtTurnEndIfAllyMovedThrough;
		}
		else
		{
			otherVal4 = false;
		}
		
		text2 = str6 + InEditorDescHelper.AssembleFieldWithDiff(header6, indent, otherSep5, removeAtTurnEndIfAllyMovedThrough, showOther5, otherVal4, ((bool b) => b));
		string str7 = text2;
		string header7 = "* Remove on Phase End: if Enemy Moved Through =";
		string otherSep6 = text;
		bool removeAtPhaseEndIfEnemyMovedThrough = this.m_removeAtPhaseEndIfEnemyMovedThrough;
		bool showOther6 = flag;
		bool otherVal5;
		if (flag)
		{
			otherVal5 = other.m_removeAtPhaseEndIfEnemyMovedThrough;
		}
		else
		{
			otherVal5 = false;
		}
		
		text2 = str7 + InEditorDescHelper.AssembleFieldWithDiff(header7, indent, otherSep6, removeAtPhaseEndIfEnemyMovedThrough, showOther6, otherVal5, ((bool b) => b));
		string str8 = text2;
		string header8 = "* Remove on Phase End: if Ally Moved Through =";
		string otherSep7 = text;
		bool removeAtPhaseEndIfAllyMovedThrough = this.m_removeAtPhaseEndIfAllyMovedThrough;
		bool showOther7 = flag;
		bool otherVal6;
		if (flag)
		{
			otherVal6 = other.m_removeAtPhaseEndIfAllyMovedThrough;
		}
		else
		{
			otherVal6 = false;
		}
		
		text2 = str8 + InEditorDescHelper.AssembleFieldWithDiff(header8, indent, otherSep7, removeAtPhaseEndIfAllyMovedThrough, showOther7, otherVal6, ((bool b) => b));
		if (!this.m_onEnemyMovedThrough.HasResponse())
		{
			if (!flag)
			{
				goto IL_428;
			}
			if (!other.m_onEnemyMovedThrough.HasResponse())
			{
				goto IL_428;
			}
		}
		string str9 = text2;
		GameplayResponseForActor onEnemyMovedThrough = this.m_onEnemyMovedThrough;
		string header9 = "    >> On <color=cyan>Enemy</color> Moved Through <<";
		string indent2 = "            ";
		bool showDiff = flag;
		GameplayResponseForActor other2;
		if (flag)
		{
			other2 = other.m_onEnemyMovedThrough;
		}
		else
		{
			other2 = null;
		}
		text2 = str9 + onEnemyMovedThrough.GetInEditorDescription(header9, indent2, showDiff, other2);
		IL_428:
		if (!this.m_onAllyMovedThrough.HasResponse())
		{
			if (!flag)
			{
				goto IL_496;
			}
			if (!other.m_onAllyMovedThrough.HasResponse())
			{
				goto IL_496;
			}
		}
		text2 += this.m_onAllyMovedThrough.GetInEditorDescription("    >> On <color=cyan>Ally</color> Moved Through <<", "            ", flag, (!flag) ? null : other.m_onAllyMovedThrough);
		IL_496:
		string str10 = text2;
		string header10 = "Barrier Sequence Prefabs (for barrier visuals):";
		GameObject[] myObjList;
		if (this.m_barrierSequencePrefabs == null)
		{
			myObjList = null;
		}
		else
		{
			myObjList = this.m_barrierSequencePrefabs.ToArray();
		}
		bool showDiff2 = flag;
		GameObject[] otherObjList;
		if (flag)
		{
			if (other.m_barrierSequencePrefabs == null)
			{
				otherObjList = null;
			}
			else
			{
				otherObjList = other.m_barrierSequencePrefabs.ToArray();
			}
		}
		else
		{
			otherObjList = null;
		}
		
		text2 = str10 + InEditorDescHelper.GetListDiffString<GameObject>(header10, indent, myObjList, showDiff2, otherObjList, new InEditorDescHelper.GetListEntryStrDelegate<GameObject>(InEditorDescHelper.GetGameObjectEntryStr));
		if (this.m_responseOnShotBlock.HasResponses())
		{
			string str11 = text2;
			BarrierResponseOnShot responseOnShotBlock = this.m_responseOnShotBlock;
			string header11 = "    >> Response on Shot Block <<";
			string indent3 = "            ";
			bool showDiff3 = flag;
			BarrierResponseOnShot other3;
			if (flag)
			{
				other3 = other.m_responseOnShotBlock;
			}
			else
			{
				other3 = null;
			}
			text2 = str11 + responseOnShotBlock.GetInEditorDescription(header11, indent3, showDiff3, other3);
		}
		return text2 + "<b>END oF BarrierData</b> -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -\n";
	}

	private string DiffColorStr(string input)
	{
		return InEditorDescHelper.ColoredString(input, "orange", false);
	}
}
