// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;

// server-only -- missing in reactor
#if SERVER
public class ServerKnockbackStabilizer
{
	public void StabilizeKnockbacks(
		Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> incomingKnockbacks,
		List<BoardSquare> additionalInvalidSquares)
	{
		while (StabilizeKnockbacksForValidDestination(incomingKnockbacks, additionalInvalidSquares)
		       || StabilizeKnockbacksVsObstacles(incomingKnockbacks) 
		       || StabilizeKnockbacksVsStationaries(incomingKnockbacks)
		       || StabilizeKnockbacksVsKnockbackees(incomingKnockbacks))
		{
		}
	}

	private bool StabilizeKnockbacksForValidDestination(
		Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> incomingKnockbacks,
		List<BoardSquare> additionalInvalidSquares)
	{
		bool flag = false;
		foreach (KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits> knockbackEntry in incomingKnockbacks)
		{
			flag |= StabilizeKnockbackEntryForValidDestination(knockbackEntry, additionalInvalidSquares);
		}
		return flag;
	}

	private bool StabilizeKnockbackEntryForValidDestination(
		KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits> knockbackEntry,
		List<BoardSquare> additionalInvalidSquares)
	{
		BoardSquarePathInfo endpoint = knockbackEntry.Value.GetKnockbackPath().GetPathEndpoint();
		BoardSquarePathInfo initialEndpoint = endpoint;
		bool isValidEndpoint = endpoint.square != null
		            && endpoint.square.IsValidForGameplay()
		            && !additionalInvalidSquares.Contains(endpoint.square);
		while (!isValidEndpoint)
		{
			if (endpoint.prev == null)
			{
				isValidEndpoint = true;
			}
			else
			{
				endpoint.square = null;
				endpoint.prev.next = null;
				endpoint = endpoint.prev;
				isValidEndpoint = endpoint.square != null && endpoint.square.IsValidForGameplay();
			}
		}
		if (initialEndpoint != endpoint)
		{
			knockbackEntry.Value.OnKnockbackPathStabilized(endpoint.square);
			return true;
		}
		else
		{
			return false;
		}
	}

	private bool StabilizeKnockbacksVsObstacles(
		Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> incomingKnockbacks)
	{
		bool flag = false;
		foreach (KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits> knockbackEntry in incomingKnockbacks)
		{
			flag |= StabilizeKnockbackEntryVsObstacles(knockbackEntry);
		}
		return flag;
	}

	private bool StabilizeKnockbackEntryVsObstacles(
		KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits> knockbackEntry)
	{
		bool result = false;
		ActorData key = knockbackEntry.Key;
		BoardSquarePathInfo step = knockbackEntry.Value.GetKnockbackPath();
		BoardSquarePathInfo next = step.next;
		BoardSquarePathInfo prev = step.prev;
		while (next != null)
		{
			if ((prev != null
			     && BarrierManager.Get() != null
			     && BarrierManager.Get().IsMovementBlockedOnCrossover(key, prev.square, step.square))
			    || (BarrierManager.Get() != null
			        && BarrierManager.Get().IsMovementBlocked(key, step.square, next.square)))
			{
				step.next = null;
				next.prev = null;
				next = null;
				knockbackEntry.Value.OnKnockbackPathStabilized(step.square);
				result = true;
			}
			else
			{
				step = next;
				prev = next.prev;
				next = next.next;
			}
		}
		return result;
	}

	private bool StabilizeKnockbacksVsStationaries(
		Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> incomingKnockbacks)
	{
		bool result = false;
		foreach (ActorData actorData in GetStationaryActorsWrtKnockback(incomingKnockbacks))
		{
			if (actorData.IsDead())
			{
				continue;
			}
			BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
			foreach (KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits> keyValuePair in incomingKnockbacks)
			{
				ServerKnockbackManager.KnockbackHits value = keyValuePair.Value;
				if (value.GetKnockbackEndSquare() == currentBoardSquare)
				{
					BoardSquarePathInfo boardSquarePathInfo = value.GetKnockbackPath().BackUpOnceFromEnd();
					if (value.GetKnockbackEndSquare() != boardSquarePathInfo.square)
					{
						value.OnKnockbackPathStabilized(boardSquarePathInfo.square);
						result = true;
					}
					else
					{
						Log.Error($"Failed to back up {keyValuePair.Key} in StabilizeKnockbacksVsStationaries.");
					}
				}
			}
		}
		return result;
	}

	private bool StabilizeKnockbacksVsKnockbackees(
		Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> incomingKnockbacks)
	{
		List<KeyValuePair<ActorData, ServerKnockbackManager.KnockbackHits>> incomingKnockbacksSorted = incomingKnockbacks.ToList();
		incomingKnockbacksSorted.Sort((x, y) =>
		{
			float xDist = x.Value.GetKnockbackEndSquare().HorizontalDistanceOnBoardTo(x.Key.GetCurrentBoardSquare());
			float yDist = y.Value.GetKnockbackEndSquare().HorizontalDistanceOnBoardTo(y.Key.GetCurrentBoardSquare());
			return xDist.CompareTo(yDist);
		});
		
		bool stabilized = false;
		for (int i = 0; i < incomingKnockbacksSorted.Count; i++)
		{
			ServerKnockbackManager.KnockbackHits knockback1 = incomingKnockbacksSorted[i].Value;
			for (int j = i + 1; j < incomingKnockbacksSorted.Count; j++)
			{
				ServerKnockbackManager.KnockbackHits knockback2 = incomingKnockbacksSorted[j].Value;
				if (knockback1.GetKnockbackEndSquare() == knockback2.GetKnockbackEndSquare())
				{
					BoardSquarePathInfo stepBack = knockback2.GetKnockbackPath().BackUpOnceFromEnd();
					if (stepBack.square != knockback2.GetKnockbackEndSquare())
					{
						knockback2.OnKnockbackPathStabilized(stepBack.square);
						stabilized = true;
					}
					else
					{
						Log.Error($"Failed to back up {incomingKnockbacksSorted[j].Key} in StabilizeKnockbacksVsKnockbackees.");
					}
				}
			}
			if (stabilized)
			{
				return true;
			}
		}
		return false;
	}

	private List<ActorData> GetStationaryActorsWrtKnockback(
		Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> incomingKnockbacks)
	{
		List<ActorData> actors = new List<ActorData>(GameFlowData.Get().GetActors());
		foreach (ActorData item in incomingKnockbacks.Keys)
		{
			if (actors.Contains(item))
			{
				actors.Remove(item);
			}
		}
		return actors;
	}
}
#endif
