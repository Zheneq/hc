// ROGUES
// SERVER
//using System;
using System.Collections.Generic;
using UnityEngine;

// was empty in reactor
public class QueryAreaManager : MonoBehaviour
{
#if SERVER
	private List<QueryArea> m_queryAreas = new List<QueryArea>();
	private static QueryAreaManager s_instance;

	public static QueryAreaManager Get()
	{
		return s_instance;
	}

	public void Awake()
	{
		if (s_instance == null)
		{
			s_instance = this;
		}
	}

	public void OnDestroy()
	{
		m_queryAreas.Clear();
		s_instance = null;
	}

	public void AddQueryArea(QueryArea area)
	{
		m_queryAreas.Add(area);
	}

	public void OnActorMoved(ActorData actor, BoardSquarePathInfo path)
	{
		foreach (QueryArea queryArea in m_queryAreas)
		{
			if (queryArea != null)
			{
				queryArea.OnActorMoved(actor, path);
			}
		}
	}

	public void OnTurnStart()
	{
		foreach (QueryArea queryArea in m_queryAreas)
		{
			if (queryArea != null)
			{
				queryArea.OnTurnStart();
			}
		}
	}
#endif
}
