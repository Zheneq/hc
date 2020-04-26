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
	private static InEditorDescHelper.GetListEntryStrDelegate<GameObject> _003C_003Ef__mg_0024cache0;

	public void SetupForPattern(AbilityGridPattern pattern)
	{
		m_bidirectional = true;
		if (pattern != AbilityGridPattern.Plus_Two_x_Two)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (pattern == AbilityGridPattern.Plus_Four_x_Four)
					{
						m_width = 4f;
					}
					return;
				}
			}
		}
		m_width = 2f;
	}

	public virtual void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers)
	{
		m_onEnemyMovedThrough.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		m_onAllyMovedThrough.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
	}

	public StandardBarrierData GetShallowCopy()
	{
		StandardBarrierData standardBarrierData = (StandardBarrierData)MemberwiseClone();
		standardBarrierData.m_onEnemyMovedThrough = m_onEnemyMovedThrough.GetShallowCopy();
		standardBarrierData.m_onAllyMovedThrough = m_onAllyMovedThrough.GetShallowCopy();
		if (m_barrierSequencePrefabs != null)
		{
			standardBarrierData.m_barrierSequencePrefabs = new List<GameObject>(m_barrierSequencePrefabs);
		}
		else
		{
			standardBarrierData.m_barrierSequencePrefabs = new List<GameObject>();
		}
		standardBarrierData.m_responseOnShotBlock = m_responseOnShotBlock.GetShallowCopy();
		return standardBarrierData;
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens, string name, bool addCompare = false, StandardBarrierData other = null)
	{
		bool flag = addCompare && other != null;
		string name2 = name + "_Duration";
		int maxDuration = m_maxDuration;
		int otherVal;
		if (flag)
		{
			otherVal = other.m_maxDuration;
		}
		else
		{
			otherVal = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name2, "barrier duration", maxDuration, flag, otherVal);
		string name3 = name + "_MaxHits";
		int maxHits = m_maxHits;
		int otherVal2;
		if (flag)
		{
			otherVal2 = other.m_maxDuration;
		}
		else
		{
			otherVal2 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name3, "max target hit count", maxHits, flag, otherVal2);
		if (m_onEnemyMovedThrough != null && m_onEnemyMovedThrough.HasResponse())
		{
			GameplayResponseForActor onEnemyMovedThrough = m_onEnemyMovedThrough;
			string name4 = name + "_EnemyCross";
			object other2;
			if (flag)
			{
				other2 = other.m_onEnemyMovedThrough;
			}
			else
			{
				other2 = null;
			}
			onEnemyMovedThrough.AddTooltipTokens(tokens, name4, flag, (GameplayResponseForActor)other2);
		}
		if (m_onAllyMovedThrough != null)
		{
			if (m_onAllyMovedThrough.HasResponse())
			{
				GameplayResponseForActor onAllyMovedThrough = m_onAllyMovedThrough;
				string name5 = name + "_AllyCross";
				object other3;
				if (flag)
				{
					other3 = other.m_onAllyMovedThrough;
				}
				else
				{
					other3 = null;
				}
				onAllyMovedThrough.AddTooltipTokens(tokens, name5, flag, (GameplayResponseForActor)other3);
			}
		}
		if (m_responseOnShotBlock == null)
		{
			return;
		}
		while (true)
		{
			if (m_responseOnShotBlock.HasResponses())
			{
				while (true)
				{
					m_responseOnShotBlock.AddTooltipTokens(tokens, name, flag, (!flag) ? null : other.m_responseOnShotBlock);
					return;
				}
			}
			return;
		}
	}

	public string GetInEditorDescription(string header = "Barrier Data", string indent = "    ", bool diff = false, StandardBarrierData other = null)
	{
		bool flag = diff && other != null;
		string str = "\n";
		string otherSep = "\t        \t | in base  =";
		string text = InEditorDescHelper.BoldedStirng(header) + str;
		string str2 = text;
		float myVal = m_maxDuration;
		int num;
		if (flag)
		{
			num = other.m_maxDuration;
		}
		else
		{
			num = 0;
		}
		text = str2 + InEditorDescHelper.AssembleFieldWithDiff("[ Max Duration ] = ", indent, otherSep, myVal, flag, num);
		if (m_maxDuration <= 0)
		{
			text = text + indent + "Barrier won't expire by time, [Max Duration] is not a positive number.\n";
		}
		string str3 = text;
		float width = m_width;
		float otherVal;
		if (flag)
		{
			otherVal = other.m_width;
		}
		else
		{
			otherVal = 0f;
		}
		text = str3 + InEditorDescHelper.AssembleFieldWithDiff("[ Width ] = ", indent, otherSep, width, flag, otherVal);
		text += InEditorDescHelper.AssembleFieldWithDiff("[ Bidirectional? ] = ", indent, otherSep, m_bidirectional, flag, flag && other.m_bidirectional);
		text += InEditorDescHelper.AssembleFieldWithDiff("[ Block Vision ] = \t\t", indent, otherSep, m_blocksVision, flag, flag ? other.m_blocksVision : BlockingRules.ForNobody);
		text += InEditorDescHelper.AssembleFieldWithDiff("[ Block Ability ] = \t\t", indent, otherSep, m_blocksAbilities, flag, flag ? other.m_blocksAbilities : BlockingRules.ForNobody);
		text += InEditorDescHelper.AssembleFieldWithDiff("[ Block Movement ] = \t", indent, otherSep, m_blocksMovement, flag, flag ? other.m_blocksMovement : BlockingRules.ForNobody);
		text += InEditorDescHelper.AssembleFieldWithDiff("[ Block Move On Crossover ] = \t", indent, otherSep, m_blocksMovementOnCrossover, flag, flag ? other.m_blocksMovementOnCrossover : BlockingRules.ForNobody);
		text += InEditorDescHelper.AssembleFieldWithDiff("[ Block Position Targeting ] = \t", indent, otherSep, m_blocksPositionTargeting, flag, flag ? other.m_blocksPositionTargeting : BlockingRules.ForNobody);
		string str4 = text;
		bool considerAsCover = m_considerAsCover;
		int otherVal2;
		if (flag)
		{
			otherVal2 = (other.m_considerAsCover ? 1 : 0);
		}
		else
		{
			otherVal2 = 0;
		}
		text = str4 + InEditorDescHelper.AssembleFieldWithDiff("[ Consider as Cover ] = \t", indent, otherSep, considerAsCover, flag, (byte)otherVal2 != 0);
		text += InEditorDescHelper.AssembleFieldWithDiff("[ Max Hits ] = ", indent, otherSep, m_maxHits, flag, flag ? other.m_maxHits : 0);
		string str5 = text;
		bool removeAtTurnEndIfEnemyMovedThrough = m_removeAtTurnEndIfEnemyMovedThrough;
		int otherVal3;
		if (flag)
		{
			otherVal3 = (other.m_removeAtTurnEndIfEnemyMovedThrough ? 1 : 0);
		}
		else
		{
			otherVal3 = 0;
		}
		
		text = str5 + InEditorDescHelper.AssembleFieldWithDiff("* Remove on Turn End: if Enemy Moved Through = ", indent, otherSep, removeAtTurnEndIfEnemyMovedThrough, flag, (byte)otherVal3 != 0, ((bool b) => b));
		string str6 = text;
		bool removeAtTurnEndIfAllyMovedThrough = m_removeAtTurnEndIfAllyMovedThrough;
		int otherVal4;
		if (flag)
		{
			otherVal4 = (other.m_removeAtTurnEndIfAllyMovedThrough ? 1 : 0);
		}
		else
		{
			otherVal4 = 0;
		}
		
		text = str6 + InEditorDescHelper.AssembleFieldWithDiff("* Remove on Turn End: if Ally Moved Through = ", indent, otherSep, removeAtTurnEndIfAllyMovedThrough, flag, (byte)otherVal4 != 0, ((bool b) => b));
		string str7 = text;
		bool removeAtPhaseEndIfEnemyMovedThrough = m_removeAtPhaseEndIfEnemyMovedThrough;
		int otherVal5;
		if (flag)
		{
			otherVal5 = (other.m_removeAtPhaseEndIfEnemyMovedThrough ? 1 : 0);
		}
		else
		{
			otherVal5 = 0;
		}
		
		text = str7 + InEditorDescHelper.AssembleFieldWithDiff("* Remove on Phase End: if Enemy Moved Through =", indent, otherSep, removeAtPhaseEndIfEnemyMovedThrough, flag, (byte)otherVal5 != 0, ((bool b) => b));
		string str8 = text;
		bool removeAtPhaseEndIfAllyMovedThrough = m_removeAtPhaseEndIfAllyMovedThrough;
		int otherVal6;
		if (flag)
		{
			otherVal6 = (other.m_removeAtPhaseEndIfAllyMovedThrough ? 1 : 0);
		}
		else
		{
			otherVal6 = 0;
		}
		
		text = str8 + InEditorDescHelper.AssembleFieldWithDiff("* Remove on Phase End: if Ally Moved Through =", indent, otherSep, removeAtPhaseEndIfAllyMovedThrough, flag, (byte)otherVal6 != 0, ((bool b) => b));
		if (m_onEnemyMovedThrough.HasResponse())
		{
			goto IL_03f2;
		}
		if (flag)
		{
			if (other.m_onEnemyMovedThrough.HasResponse())
			{
				goto IL_03f2;
			}
		}
		goto IL_0428;
		IL_03f2:
		string str9 = text;
		GameplayResponseForActor onEnemyMovedThrough = m_onEnemyMovedThrough;
		object other2;
		if (flag)
		{
			other2 = other.m_onEnemyMovedThrough;
		}
		else
		{
			other2 = null;
		}
		text = str9 + onEnemyMovedThrough.GetInEditorDescription("    >> On <color=cyan>Enemy</color> Moved Through <<", "            ", flag, (GameplayResponseForActor)other2);
		goto IL_0428;
		IL_0468:
		text += m_onAllyMovedThrough.GetInEditorDescription("    >> On <color=cyan>Ally</color> Moved Through <<", "            ", flag, (!flag) ? null : other.m_onAllyMovedThrough);
		goto IL_0496;
		IL_0428:
		if (m_onAllyMovedThrough.HasResponse())
		{
			goto IL_0468;
		}
		if (flag)
		{
			if (other.m_onAllyMovedThrough.HasResponse())
			{
				goto IL_0468;
			}
		}
		goto IL_0496;
		IL_0496:
		string str10 = text;
		object myObjList;
		if (m_barrierSequencePrefabs == null)
		{
			myObjList = null;
		}
		else
		{
			myObjList = m_barrierSequencePrefabs.ToArray();
		}
		object otherObjList;
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
		text = str10 + InEditorDescHelper.GetListDiffString("Barrier Sequence Prefabs (for barrier visuals):", indent, (GameObject[])myObjList, flag, (GameObject[])otherObjList, InEditorDescHelper.GetGameObjectEntryStr);
		if (m_responseOnShotBlock.HasResponses())
		{
			string str11 = text;
			BarrierResponseOnShot responseOnShotBlock = m_responseOnShotBlock;
			object other3;
			if (flag)
			{
				other3 = other.m_responseOnShotBlock;
			}
			else
			{
				other3 = null;
			}
			text = str11 + responseOnShotBlock.GetInEditorDescription("    >> Response on Shot Block <<", "            ", flag, (BarrierResponseOnShot)other3);
		}
		return text + "<b>END oF BarrierData</b> -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -\n";
	}

	private string DiffColorStr(string input)
	{
		return InEditorDescHelper.ColoredString(input, "orange");
	}
}
