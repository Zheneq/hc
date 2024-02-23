using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

[Serializable]
public class SpoilsSpawnData
{
	public List<GameObject> m_powerupPrefabs = new List<GameObject>();
	public int m_numToSpawn;
	public bool m_canSpawnOnEnemyOccupiedSquare;
	public bool m_canSpawnOnAllyOccupiedSquare;
	public int m_duration;

	public SpoilsSpawnData GetShallowCopy()
	{
		SpoilsSpawnData spoilsSpawnData = (SpoilsSpawnData)base.MemberwiseClone();
		spoilsSpawnData.m_powerupPrefabs = new List<GameObject>(m_powerupPrefabs);
		return spoilsSpawnData;
	}

	public void SpawnSpoilsAroundSquare(BoardSquare desiredSpawnSquare, Team forTeam, bool ignoreSpawnSplineForSequence = false, int maxBorderSearchLayers = 4)
	{
	}

	public bool HasResponse()
	{
		if (m_numToSpawn > 0 && m_powerupPrefabs != null)
		{
			return m_powerupPrefabs.Count > 0;
		}
		return false;
	}

	public string GetInEditorDescription(string header = "Spoils Spawn Data", string indent = "    ", bool diff = false, SpoilsSpawnData other = null)
	{
		bool showOther = diff && other != null;
		string otherSep = "\t        \t | in base  =";
		string desc = new StringBuilder().Append(InEditorDescHelper.BoldedStirng(header)).Append("\n").ToString();
		if (HasResponse())
		{
			float otherNum = showOther ? other.m_numToSpawn : 0;
			desc += InEditorDescHelper.AssembleFieldWithDiff("[ Num Spoils To Spawn ] = ", indent, otherSep, m_numToSpawn, showOther, otherNum, null);
			bool otherCanSpawnOnEnemy = showOther && other.m_canSpawnOnEnemyOccupiedSquare;
			desc += InEditorDescHelper.AssembleFieldWithDiff("[ Can Spawn On Enemy Square ] = ", indent, otherSep, m_canSpawnOnEnemyOccupiedSquare, showOther, otherCanSpawnOnEnemy, null);
			bool otherCanSpawnOnAlly = showOther && other.m_canSpawnOnAllyOccupiedSquare;
			desc += InEditorDescHelper.AssembleFieldWithDiff("[ Can Spawn On Ally Square ] = ", indent, otherSep, m_canSpawnOnAllyOccupiedSquare, showOther, otherCanSpawnOnAlly, null);
			float otherDuration = showOther ? other.m_duration : 0;
			desc += InEditorDescHelper.AssembleFieldWithDiff("[ Duration ] = ", indent, otherSep, m_duration, showOther, otherDuration, null);
			GameObject[] otherPups = showOther ? other.m_powerupPrefabs.ToArray() : null;
			desc += InEditorDescHelper.GetListDiffString("PowerupPrefabs:\t", indent, m_powerupPrefabs.ToArray(), showOther, otherPups, new InEditorDescHelper.GetListEntryStrDelegate<GameObject>(InEditorDescHelper.GetGameObjectEntryStr));
		}
		else
		{
			desc += "Not set to spawn spoils\n";
		}
		return desc;
	}
}
