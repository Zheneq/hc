using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class SpoilsSpawnData
{
	public List<GameObject> m_powerupPrefabs = new List<GameObject>();

	public int m_numToSpawn;

	public bool m_canSpawnOnEnemyOccupiedSquare;

	public bool m_canSpawnOnAllyOccupiedSquare;

	public int m_duration;

	[CompilerGenerated]
	private static InEditorDescHelper.GetListEntryStrDelegate<GameObject> f__mg_cache0;

	public SpoilsSpawnData GetShallowCopy()
	{
		SpoilsSpawnData spoilsSpawnData = (SpoilsSpawnData)base.MemberwiseClone();
		spoilsSpawnData.m_powerupPrefabs = new List<GameObject>(this.m_powerupPrefabs);
		return spoilsSpawnData;
	}

	public void SpawnSpoilsAroundSquare(BoardSquare desiredSpawnSquare, Team forTeam, bool ignoreSpawnSplineForSequence = false, int maxBorderSearchLayers = 4)
	{
	}

	public bool HasResponse()
	{
		if (this.m_numToSpawn > 0)
		{
			if (this.m_powerupPrefabs != null)
			{
				return this.m_powerupPrefabs.Count > 0;
			}
		}
		return false;
	}

	public string GetInEditorDescription(string header = "Spoils Spawn Data", string indent = "    ", bool diff = false, SpoilsSpawnData other = null)
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
		if (this.HasResponse())
		{
			string str2 = text2;
			string header2 = "[ Num Spoils To Spawn ] = ";
			string otherSep = text;
			float myVal = (float)this.m_numToSpawn;
			bool showOther = flag2;
			float num;
			if (flag2)
			{
				num = (float)other.m_numToSpawn;
			}
			else
			{
				num = (float)0;
			}
			text2 = str2 + InEditorDescHelper.AssembleFieldWithDiff(header2, indent, otherSep, myVal, showOther, num, null);
			string str3 = text2;
			string header3 = "[ Can Spawn On Enemy Square ] = ";
			string otherSep2 = text;
			bool canSpawnOnEnemyOccupiedSquare = this.m_canSpawnOnEnemyOccupiedSquare;
			bool showOther2 = flag2;
			bool otherVal;
			if (flag2)
			{
				otherVal = other.m_canSpawnOnEnemyOccupiedSquare;
			}
			else
			{
				otherVal = false;
			}
			text2 = str3 + InEditorDescHelper.AssembleFieldWithDiff(header3, indent, otherSep2, canSpawnOnEnemyOccupiedSquare, showOther2, otherVal, null);
			string str4 = text2;
			string header4 = "[ Can Spawn On Ally Square ] = ";
			string otherSep3 = text;
			bool canSpawnOnAllyOccupiedSquare = this.m_canSpawnOnAllyOccupiedSquare;
			bool showOther3 = flag2;
			bool otherVal2;
			if (flag2)
			{
				otherVal2 = other.m_canSpawnOnAllyOccupiedSquare;
			}
			else
			{
				otherVal2 = false;
			}
			text2 = str4 + InEditorDescHelper.AssembleFieldWithDiff(header4, indent, otherSep3, canSpawnOnAllyOccupiedSquare, showOther3, otherVal2, null);
			string str5 = text2;
			string header5 = "[ Duration ] = ";
			string otherSep4 = text;
			float myVal2 = (float)this.m_duration;
			bool showOther4 = flag2;
			float num2;
			if (flag2)
			{
				num2 = (float)other.m_duration;
			}
			else
			{
				num2 = (float)0;
			}
			text2 = str5 + InEditorDescHelper.AssembleFieldWithDiff(header5, indent, otherSep4, myVal2, showOther4, num2, null);
			string str6 = text2;
			string header6 = "PowerupPrefabs:\t";
			GameObject[] myObjList = this.m_powerupPrefabs.ToArray();
			bool showDiff = flag2;
			GameObject[] otherObjList;
			if (flag2)
			{
				otherObjList = other.m_powerupPrefabs.ToArray();
			}
			else
			{
				otherObjList = null;
			}
			
			text2 = str6 + InEditorDescHelper.GetListDiffString<GameObject>(header6, indent, myObjList, showDiff, otherObjList, new InEditorDescHelper.GetListEntryStrDelegate<GameObject>(InEditorDescHelper.GetGameObjectEntryStr));
		}
		else
		{
			text2 += "Not set to spawn spoils\n";
		}
		return text2;
	}
}
