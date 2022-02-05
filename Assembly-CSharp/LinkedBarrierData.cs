// ROGUES
// SERVER
using System.Collections.Generic;

public class LinkedBarrierData
{
	public List<ActorData> m_actorsMovedThrough;
	public List<ActorData> m_actorsMovedThroughThisTurn;
	public int m_hitsOnAllies;
	public int m_hitsOnEnemies;

	public LinkedBarrierData()
	{
		m_actorsMovedThrough = new List<ActorData>();
		m_actorsMovedThroughThisTurn = new List<ActorData>();
		m_hitsOnAllies = 0;
		m_hitsOnEnemies = 0;
	}

	// removed in rogues
	public void OnTurnStart()
	{
		m_actorsMovedThroughThisTurn.Clear();
	}

	// NOTE ROGUES they moved it from OnTurnStart to OnTurnEnd -- might need some adjustments on the calling side too
	// added in rogues
#if SERVER
	public void OnTurnEnd()
	{
		//m_actorsMovedThroughThisTurn.Clear();
	}
#endif

	public int GetNumHits()
	{
		return m_hitsOnAllies + m_hitsOnEnemies;
	}
}
