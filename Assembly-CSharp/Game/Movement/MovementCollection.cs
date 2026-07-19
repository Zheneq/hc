// ROGUES
// SERVER
using System.Collections.Generic;

// server-only, missing in reactor
#if SERVER
public class MovementCollection
{
	public MovementStage m_movementStage;

	public List<MovementInstance> m_movementInstances;

	public MovementCollection(List<ServerEvadeUtils.EvadeInfo> evades)
	{
		this.m_movementStage = MovementStage.Evasion;
		this.m_movementInstances = new List<MovementInstance>(evades.Count);
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in evades)
		{
			bool flag = evadeInfo.GetMovementType() == ActorData.MovementType.Charge;
			bool flag2 = evadeInfo.GetMovementType() == ActorData.MovementType.WaypointFlight;
			bool flag3 = flag || flag2;
			MovementInstance movementInstance = new MovementInstance(evadeInfo.GetMover(), evadeInfo.m_evadePath, flag3, false, evadeInfo.IsStealthEvade());
			movementInstance.m_canCrossBarriers = (flag3 && !flag2);
			this.m_movementInstances.Add(movementInstance);
		}
	}

	public MovementCollection(Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> knockbacks)
	{
		this.m_movementStage = MovementStage.Knockback;
		this.m_movementInstances = new List<MovementInstance>(knockbacks.Count);
		foreach (KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits> keyValuePair in knockbacks)
		{
			MovementInstance item = new MovementInstance(keyValuePair.Key, keyValuePair.Value.GetKnockbackPath(), true, false, false);
			this.m_movementInstances.Add(item);
		}
	}

	public MovementCollection(List<MovementRequest> moveRequests)
	{
		this.m_movementStage = MovementStage.Normal;
		this.m_movementInstances = new List<MovementInstance>(moveRequests.Count);
		foreach (MovementRequest movementRequest in moveRequests)
		{
			MovementInstance item = new MovementInstance(movementRequest.m_actor, movementRequest.m_path, true, movementRequest.WasEverChasing(), false);
			this.m_movementInstances.Add(item);
		}
	}

	public MovementCollection(MovementStage movementStage, List<MovementInstance> movementInstances)
	{
		this.m_movementStage = movementStage;
		this.m_movementInstances = movementInstances;
	}
}
#endif
