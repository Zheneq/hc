// ROGUES
// SERVER
//using System;
using System.Collections.Generic;
using UnityEngine;

public class QueryArea : MonoBehaviour
{
	public int m_boardSquareSizeX = 1;

	public int m_boardSquareSizeY = 1;

	public Color m_gizmoColor = Color.cyan;

	[Tooltip("Use this to provide your own identifier on this query area (perhaps because its being passed to a state as an on enter event)")]
	public string m_name;

#if SERVER
	// added in rogues
	private BoardRegion m_boardRegion;

	// added in rogues
	private List<ActorData> m_charactersWalkingThrough = new List<ActorData>();

	// added in rogues
	private Vector3 m_lastGizmoPos = Vector3.zero;
#endif

	// added in rogues
#if SERVER
	private void Start()
	{
		QueryAreaManager.Get().AddQueryArea(this);
		this.CreateRegion();
	}
#endif

	// added in rogues
#if SERVER
	private void CreateRegion()
	{
		Board board = Board.Get();
		if (board != null && this.m_boardSquareSizeX > 0 && this.m_boardSquareSizeY > 0)
		{
			this.m_boardRegion = new BoardRegion();
			float num = (float)(this.m_boardSquareSizeX - 1) * board.squareSize * 0.5f;
			float num2 = (float)(this.m_boardSquareSizeY - 1) * board.squareSize * 0.5f;
			Vector3 worldCorner = new Vector3(base.transform.position.x - num, 0f, base.transform.position.z - num2);
			Vector3 worldCorner2 = new Vector3(base.transform.position.x + num, 0f, base.transform.position.z + num2);
			this.m_boardRegion.InitializeAsRect(worldCorner, worldCorner2);
		}
	}
#endif

	// added in rogues
#if SERVER
	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		if (this.m_boardRegion == null || (this.m_lastGizmoPos - base.transform.position).sqrMagnitude > 0f)
		{
			this.CreateRegion();
			this.m_lastGizmoPos = base.transform.position;
		}
		if (this.m_boardRegion != null)
		{
			this.m_boardRegion.GizmosDrawRegion(this.m_gizmoColor);
		}
	}
#endif

	// added in rogues
#if SERVER
	private void OnValidate()
	{
		this.m_boardRegion = null;
	}
#endif

	// added in rogues
#if SERVER
	public void OnTurnStart()
	{
		this.m_charactersWalkingThrough.Clear();
	}
#endif

	// added in rogues
#if SERVER
	public void OnActorMoved(ActorData actor, BoardSquarePathInfo path)
	{
		BoardSquare square = path.square;
		if (this.m_boardRegion.Contains(square.x, square.y))
		{
			if (path.prev == null || !this.m_boardRegion.Contains(path.prev.square.x, path.prev.square.y))
			{
				this.OnActorEnter(new ActorHitParameters(actor, square.ToVector3()));
				return;
			}
		}
		else if (path.prev != null && this.m_boardRegion.Contains(path.prev.square.x, path.prev.square.y))
		{
			this.OnActorExit(new ActorHitParameters(actor, square.ToVector3()));
		}
	}
#endif

	// added in rogues
#if SERVER
	public void OnActorEnter(ActorHitParameters hitParams)
	{
		if (hitParams.Target != null && !this.m_charactersWalkingThrough.Contains(hitParams.Target))
		{
			this.m_charactersWalkingThrough.Add(hitParams.Target);
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.CharacterEnteredQueryArea, new GameEventManager.QueryAreaArgs
		{
			area = this,
			characterActor = hitParams.Target
		});
	}
#endif

	// added in rogues
#if SERVER
	public void OnActorExit(ActorHitParameters hitParams)
	{
		this.m_charactersWalkingThrough.Remove(hitParams.Target);
		GameEventManager.Get().FireEvent(GameEventManager.EventType.CharacterExitedQueryArea, new GameEventManager.QueryAreaArgs
		{
			area = this,
			characterActor = hitParams.Target
		});
	}
#endif

	public List<ActorData> GetActorsInArea()
	{
		List<ActorData> list = new List<ActorData>();
#if SERVER
		if (this.m_boardRegion != null)
		{
			list = this.m_boardRegion.GetActorsInRegion();
		}
		foreach (ActorData item in this.m_charactersWalkingThrough)
		{
			if (!list.Contains(item))
			{
				list.Add(item);
			}
		}
#endif
		return list;
	}
}
