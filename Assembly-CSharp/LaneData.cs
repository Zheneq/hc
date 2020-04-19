using System;
using UnityEngine;

[Serializable]
public class LaneData
{
	public string m_designerComment;

	public Transform m_spawnPoint;

	public Transform[] m_waypoints;

	public float m_pathWidth = 3f;
}
