// ROGUES
// SERVER
using System;
using System.Collections.Generic;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

// server-only
#if SERVER
public class MovementRequest : IComparable
{
	private BoardSquare m_targetSquareInternal;

	public ActorData m_chaseTarget;
	public ActorData m_actor;
	public BoardSquarePathInfo m_path;
	public MovementRequest.MovementResolveState m_resolveState;
	public bool m_isForcedChase;
	public bool m_wasEverChase;
	public ActorData m_preStabilizeChaseTarget;
	public bool m_chaserInitiatedForceChase;
	public bool m_clashStabilized; // TODO never set

	public MovementRequest(int x, int y, ActorData actor, BoardSquarePathInfo pathToUse = null)
	{
		if (actor.IsDead())
		{
			Log.Warning($"Dead actor requesting to path {actor}");
		}
		m_targetSquare = Board.Get().GetSquareFromIndex(x, y);
		m_chaseTarget = null;
		m_actor = actor;
		m_resolveState = MovementRequest.MovementResolveState.QUEUED;
		if (pathToUse != null && pathToUse.square == actor.InitialMoveStartSquare)
		{
			BoardSquarePathInfo pathEndpoint = pathToUse.GetPathEndpoint();
			if (pathEndpoint.square.x != x || pathEndpoint.square.y != y)
			{
				Log.Error("Precalculated path has different end position from desired destination");
			}
			else
			{
				m_path = pathToUse;
			}
		}
		if (m_path == null)
		{
			m_path = actor.GetActorMovement().BuildPathTo(actor.InitialMoveStartSquare, m_targetSquare);
		}
		if (m_path == null)
		{
			float num = actor.GetActorMovement().CalculateMaxHorizontalMovement(false, false);
			Log.Error($"{actor.DebugNameString()} failed to build path for movement request. MaxMovement={num} | " +
				$"From {actor.InitialMoveStartSquare.GetGridPos().ToStringWithCross()} to ({x} x {y})");
		}
	}

	public MovementRequest(ActorData chaseTarget, ActorData mover, bool isForced = false)
	{
		if (mover.IsDead())
		{
			Log.Warning($"Dead actor requesting to path {mover}");
		}
		m_targetSquare = chaseTarget.GetCurrentBoardSquare();
		m_chaseTarget = chaseTarget;
		m_isForcedChase = isForced;
		m_wasEverChase = true;
		m_preStabilizeChaseTarget = chaseTarget;
		m_actor = mover;
		m_resolveState = MovementResolveState.QUEUED;
		m_path = null;
	}

	public string ToLogString()
	{
		string text = "";
		switch (m_resolveState)
		{
		case MovementResolveState.QUEUED:
			text += "Queued ";
			break;
		case MovementResolveState.RESOLVING:
			text += "Resolving ";
			break;
		case MovementResolveState.RESOLVED:
			text += "Resolved ";
			break;
		}
		if (m_wasEverChase)
		{
			if (null != m_chaseTarget)
			{
				text += "pending";
			}
			else
			{
				text += "unwound";
			}
			if (m_isForcedChase)
			{
				text += " forced";
			}
			text += " chase";
		}
		else
		{
			text += "move";
		}
		if (m_actor)
		{
			text = $"{text} of {m_actor.DisplayName}({m_actor.GetAllyTeamName()})";
		}
		return $"{text} to {m_targetSquare.x},{m_targetSquare.y}";
	}

	public void AppendMovement(int x, int y)
	{
		BoardSquare squareFromIndex = Board.Get().GetSquareFromIndex(x, y);
		if (IsChasing())
		{
			Log.Error($"Actor {m_actor.DisplayName} tried to append movement to square ({x},{y}), but cannot because he's chasing.");
			return;
		}
		if (squareFromIndex == null)
		{
			Log.Error($"Actor {m_actor.DisplayName} tried to append movement to square ({x},{y}), but cannot because that square is null.");
			return;
		}
		if (!m_actor.CanMoveToBoardSquare(squareFromIndex))
		{
			Log.Error($"Actor {m_actor.DisplayName} tried to append movement to square ({x},{y}), but cannot because he can't move there.");
			return;
		}
		bool success = m_path.GetPathEndpoint().square == squareFromIndex || m_actor.GetComponent<ActorMovement>().AppendToPath(m_path, squareFromIndex);
		if (success)
		{
			m_targetSquare = squareFromIndex;
		}
		else
        {
			Log.Error($"Actor {m_actor.DisplayName} tried to append movement to square ({x},{y}), but it was unsuccessful.");
		}
	}

	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}
		MovementRequest movementRequest = obj as MovementRequest;
		if (movementRequest == null)
		{
			throw new ArgumentException("Object is not a MovementRequest");
		}
		if (WasEverChasing() != movementRequest.WasEverChasing())
		{
			return WasEverChasing() ? 1 : -1;
		}
		else if (m_path == null != (movementRequest.m_path == null))
		{
			return m_path == null ? 1 : -1;
		}
		else
		{
			if (m_path == null && movementRequest.m_path == null)
			{
				return 0;
			}
			return m_path.FindMoveCostToEnd().CompareTo(movementRequest.m_path.FindMoveCostToEnd());
		}
	}

	public bool IsChasing()
	{
		return m_chaseTarget != null;
	}

	public bool IsForcedChase()
	{
		return IsChasing() && m_isForcedChase;
	}

	public bool IsBeingDragged()
	{
		return m_isForcedChase && !m_chaserInitiatedForceChase;
	}

	public bool IsClashStabilized()
	{
		return m_clashStabilized;
	}

	public List<GridPos> ToGridPosPath()
	{
		List<GridPos> list = new List<GridPos>();
		if (IsChasing())
		{
			list.Add(m_actor.MoveFromBoardSquare.GetGridPos());
			list.Add(m_chaseTarget.GetCurrentBoardSquare().GetGridPos());
		}
		else if (m_path != null)
		{
			list = m_path.ToGridPosPath();
		}
		return list;
	}

	public bool WasEverChasing()
	{
		return m_wasEverChase;
	}

	public BoardSquare m_targetSquare
	{
		get
		{
			return m_targetSquareInternal;
		}
		set
		{
			if (NetworkServer.active && value == null)
			{
				Debug.LogError("{act} MoveStabilizationErr: Trying to set movement request target square to null, stack\n" + Environment.StackTrace);
			}
			m_targetSquareInternal = value;
		}
	}

	public enum MovementResolveState
	{
		QUEUED,
		RESOLVING,
		RESOLVED
	}
}
#endif
