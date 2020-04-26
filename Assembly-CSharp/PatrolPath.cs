using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PatrolPath
{
	[Serializable]
	public enum PatrolStyle
	{
		Loop,
		OutAndBack,
		Random
	}

	[Serializable]
	public enum Direction
	{
		Backward = -1,
		Forward = 1
	}

	public enum IncrementWaypointResult
	{
		None,
		Incremented,
		CycleCompleted
	}

	internal BoardSquare m_AlternateDestination;

	internal WayPoint m_currentWayPoint;

	internal Direction m_StartingDirection;

	[Tooltip("This is the list of way points the NPC will travel to as they continue along this patrol")]
	public List<WayPoint> mWayPoints;

	[Tooltip("Set this to an entry in mWayPoints. It will be the first entry that we travel to (and then we honor m_PatrolStyle)")]
	public WayPoint InitalWaypoint;

	[Tooltip("How should the NPC travel along this path?")]
	public PatrolStyle m_PatrolStyle;

	[Tooltip("Should they travel forwards or backwards through mWayPoints. Typically this is forward.")]
	public Direction m_Direction;

	[Tooltip("A custom tag you can use for anything you want to identify this patrol state (but this tag will be the same for all NPCs that use this patrol path)")]
	public string m_Tag;

	public int WaypointsVisitedThisCycle
	{
		get;
		internal set;
	}

	public int PatrolCyclesCompleted
	{
		get;
		internal set;
	}

	public void Initialze()
	{
		m_StartingDirection = m_Direction;
		WaypointsVisitedThisCycle = 0;
		PatrolCyclesCompleted = 0;
	}

	internal WayPoint GetInitalWaypoint()
	{
		WayPoint wayPoint = InitalWaypoint;
		if (wayPoint == null)
		{
			if (mWayPoints != null)
			{
				if (mWayPoints.Count != 0)
				{
					int index = (m_PatrolStyle == PatrolStyle.Random) ? GameplayRandom.Range(0, mWayPoints.Count) : 0;
					wayPoint = mWayPoints[index];
					goto IL_0092;
				}
			}
			Log.Error("Patrol path '" + ToString() + "' has no way points. Add them in mWayPoints");
			return null;
		}
		goto IL_0092;
		IL_0092:
		return wayPoint;
	}

	internal WayPoint IncremementWayPoint(Action<IncrementWaypointResult> onIncremenet)
	{
		int num = mWayPoints.IndexOf(m_currentWayPoint);
		int num2 = (int)(num + m_Direction);
		if (m_PatrolStyle == PatrolStyle.OutAndBack)
		{
			if (num2 <= -1 || num2 >= mWayPoints.Count)
			{
				m_Direction = (Direction)(0 - m_Direction);
				num2 = (int)(num2 + m_Direction);
				Debug.Log("Swapping Direction to " + m_Direction.ToString() + " new index: " + num2);
				int obj;
				if (m_StartingDirection == m_Direction)
				{
					obj = 2;
				}
				else
				{
					obj = 1;
				}
				onIncremenet((IncrementWaypointResult)obj);
			}
			else
			{
				Debug.Log("New index: " + num2);
				onIncremenet(IncrementWaypointResult.Incremented);
			}
		}
		else if (m_PatrolStyle == PatrolStyle.Loop)
		{
			if (num2 < 0)
			{
				num2 = mWayPoints.Count - 1;
				onIncremenet(IncrementWaypointResult.CycleCompleted);
			}
			else if (num2 >= mWayPoints.Count)
			{
				num2 = 0;
				onIncremenet(IncrementWaypointResult.CycleCompleted);
			}
			else
			{
				onIncremenet(IncrementWaypointResult.Incremented);
			}
		}
		else if (m_PatrolStyle == PatrolStyle.Random)
		{
			if (mWayPoints.Count <= 1)
			{
				num2 = num;
			}
			else
			{
				do
				{
					num2 = GameplayRandom.Range(0, mWayPoints.Count);
				}
				while (num2 == num);
			}
			onIncremenet(IncrementWaypointResult.Incremented);
		}
		else
		{
			onIncremenet(IncrementWaypointResult.None);
		}
		return mWayPoints[num2];
	}
}
