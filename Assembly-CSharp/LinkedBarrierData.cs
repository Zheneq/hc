using System;
using System.Collections.Generic;

public class LinkedBarrierData
{
	public List<ActorData> m_actorsMovedThrough;

	public List<ActorData> m_actorsMovedThroughThisTurn;

	public int m_hitsOnAllies;

	public int m_hitsOnEnemies;

	public LinkedBarrierData()
	{
		this.m_actorsMovedThrough = new List<ActorData>();
		this.m_actorsMovedThroughThisTurn = new List<ActorData>();
		this.m_hitsOnAllies = 0;
		this.m_hitsOnEnemies = 0;
	}

	public void OnTurnStart()
	{
		this.m_actorsMovedThroughThisTurn.Clear();
	}

	public int GetNumHits()
	{
		return this.m_hitsOnAllies + this.m_hitsOnEnemies;
	}
}
