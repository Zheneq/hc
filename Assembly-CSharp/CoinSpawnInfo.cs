using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CoinSpawnInfo
{
	public string m_name = string.Empty;

	public List<Transform> m_spawnLocations = new List<Transform>();

	public CoinSpawnRule m_spawnRulePerSquare;
}
