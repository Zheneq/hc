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

	public void OnTurnStart()
	{
		m_actorsMovedThroughThisTurn.Clear();
	}

	public int GetNumHits()
	{
		return m_hitsOnAllies + m_hitsOnEnemies;
	}
}
