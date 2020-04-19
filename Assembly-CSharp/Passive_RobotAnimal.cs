using System;
using System.Collections.Generic;

public class Passive_RobotAnimal : Passive
{
	public int m_chargeLastHitTurn = -1;

	public List<ActorData> m_chargeHitActors = new List<ActorData>();

	public int m_dragLastCastTurn = -1;

	public List<ActorData> m_dragHitActors = new List<ActorData>();

	public bool m_shouldApplyAdditionalEffectFromStealth;

	public int m_biteLastCastTurn = -1;

	public List<ActorData> m_biteAdjacentEnemies = new List<ActorData>();
}
