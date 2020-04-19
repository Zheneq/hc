using System;
using UnityEngine;

public class MatchObjective : MonoBehaviour
{
	public virtual void Client_OnActorDeath(ActorData actor)
	{
	}

	public virtual void Server_OnActorDeath(ActorData actor)
	{
	}
}
