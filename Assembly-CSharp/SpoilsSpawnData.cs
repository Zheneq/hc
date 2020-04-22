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
	private static InEditorDescHelper.GetListEntryStrDelegate<GameObject> _003C_003Ef__mg_0024cache0;

	public SpoilsSpawnData GetShallowCopy()
	{
		SpoilsSpawnData spoilsSpawnData = (SpoilsSpawnData)MemberwiseClone();
		spoilsSpawnData.m_powerupPrefabs = new List<GameObject>(m_powerupPrefabs);
		return spoilsSpawnData;
	}

	public void SpawnSpoilsAroundSquare(BoardSquare desiredSpawnSquare, Team forTeam, bool ignoreSpawnSplineForSequence = false, int maxBorderSearchLayers = 4)
	{
	}

	public bool HasResponse()
	{
		int result;
		if (m_numToSpawn > 0)
		{
			if (m_powerupPrefabs != null)
			{
				result = ((m_powerupPrefabs.Count > 0) ? 1 : 0);
				goto IL_003f;
			}
		}
		result = 0;
		goto IL_003f;
		IL_003f:
		return (byte)result != 0;
	}

	public string GetInEditorDescription(string header = "Spoils Spawn Data", string indent = "    ", bool diff = false, SpoilsSpawnData other = null)
	{
		int num;
		if (diff)
		{
			num = ((other != null) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		string str = "\n";
		string otherSep = "\t        \t | in base  =";
		string text = InEditorDescHelper.BoldedStirng(header) + str;
		if (HasResponse())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					string str2 = text;
					float myVal = m_numToSpawn;
					int num2;
					if (flag)
					{
						num2 = other.m_numToSpawn;
					}
					else
					{
						num2 = 0;
					}
					text = str2 + InEditorDescHelper.AssembleFieldWithDiff("[ Num Spoils To Spawn ] = ", indent, otherSep, myVal, flag, num2);
					string str3 = text;
					bool canSpawnOnEnemyOccupiedSquare = m_canSpawnOnEnemyOccupiedSquare;
					int otherVal;
					if (flag)
					{
						otherVal = (other.m_canSpawnOnEnemyOccupiedSquare ? 1 : 0);
					}
					else
					{
						otherVal = 0;
					}
					text = str3 + InEditorDescHelper.AssembleFieldWithDiff("[ Can Spawn On Enemy Square ] = ", indent, otherSep, canSpawnOnEnemyOccupiedSquare, flag, (byte)otherVal != 0);
					string str4 = text;
					bool canSpawnOnAllyOccupiedSquare = m_canSpawnOnAllyOccupiedSquare;
					int otherVal2;
					if (flag)
					{
						otherVal2 = (other.m_canSpawnOnAllyOccupiedSquare ? 1 : 0);
					}
					else
					{
						otherVal2 = 0;
					}
					text = str4 + InEditorDescHelper.AssembleFieldWithDiff("[ Can Spawn On Ally Square ] = ", indent, otherSep, canSpawnOnAllyOccupiedSquare, flag, (byte)otherVal2 != 0);
					string str5 = text;
					float myVal2 = m_duration;
					int num3;
					if (flag)
					{
						num3 = other.m_duration;
					}
					else
					{
						num3 = 0;
					}
					text = str5 + InEditorDescHelper.AssembleFieldWithDiff("[ Duration ] = ", indent, otherSep, myVal2, flag, num3);
					string str6 = text;
					GameObject[] myObjList = m_powerupPrefabs.ToArray();
					object otherObjList;
					if (flag)
					{
						otherObjList = other.m_powerupPrefabs.ToArray();
					}
					else
					{
						otherObjList = null;
					}
					if (_003C_003Ef__mg_0024cache0 == null)
					{
						_003C_003Ef__mg_0024cache0 = InEditorDescHelper.GetGameObjectEntryStr;
					}
					return str6 + InEditorDescHelper.GetListDiffString("PowerupPrefabs:\t", indent, myObjList, flag, (GameObject[])otherObjList, _003C_003Ef__mg_0024cache0);
				}
				}
			}
		}
		return text + "Not set to spawn spoils\n";
	}
}
