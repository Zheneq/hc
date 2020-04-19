using System;
using System.Collections.Generic;
using UnityEngine;

public class QueryArea : MonoBehaviour
{
	public int m_boardSquareSizeX = 1;

	public int m_boardSquareSizeY = 1;

	public Color m_gizmoColor = Color.cyan;

	[Tooltip("Use this to provide your own identifier on this query area (perhaps because its being passed to a state as an on enter event)")]
	public string m_name;

	public List<ActorData> GetActorsInArea()
	{
		return new List<ActorData>();
	}
}
