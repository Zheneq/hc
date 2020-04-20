using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PatrolPath
{
	internal BoardSquare m_AlternateDestination;

	internal WayPoint m_currentWayPoint;

	internal PatrolPath.Direction m_StartingDirection;

	[Tooltip("This is the list of way points the NPC will travel to as they continue along this patrol")]
	public List<WayPoint> mWayPoints;

	[Tooltip("Set this to an entry in mWayPoints. It will be the first entry that we travel to (and then we honor m_PatrolStyle)")]
	public WayPoint InitalWaypoint;

	[Tooltip("How should the NPC travel along this path?")]
	public PatrolPath.PatrolStyle m_PatrolStyle;

	[Tooltip("Should they travel forwards or backwards through mWayPoints. Typically this is forward.")]
	public PatrolPath.Direction m_Direction;

	[Tooltip("A custom tag you can use for anything you want to identify this patrol state (but this tag will be the same for all NPCs that use this patrol path)")]
	public string m_Tag;

	public int WaypointsVisitedThisCycle { get; internal set; }

	public int PatrolCyclesCompleted { get; internal set; }

	public void Initialze()
	{
		this.m_StartingDirection = this.m_Direction;
		this.WaypointsVisitedThisCycle = 0;
		this.PatrolCyclesCompleted = 0;
	}

	internal WayPoint GetInitalWaypoint()
	{
		WayPoint initalWaypoint = this.InitalWaypoint;
		if (initalWaypoint == null)
		{
			if (this.mWayPoints != null)
			{
				if (this.mWayPoints.Count != 0)
				{
					int index = (this.m_PatrolStyle != PatrolPath.PatrolStyle.Random) ? 0 : GameplayRandom.Range(0, this.mWayPoints.Count);
					return this.mWayPoints[index];
				}
			}
			Log.Error("Patrol path '" + this.ToString() + "' has no way points. Add them in mWayPoints", new object[0]);
			return null;
		}
		return initalWaypoint;
	}

	internal WayPoint IncremementWayPoint(Action<PatrolPath.IncrementWaypointResult> onIncremenet)
	{
		int num = this.mWayPoints.IndexOf(this.m_currentWayPoint);
		int num2 = (int)(num + this.m_Direction);
		if (this.m_PatrolStyle == PatrolPath.PatrolStyle.OutAndBack)
		{
			if (num2 <= -1 || num2 >= this.mWayPoints.Count)
			{
				this.m_Direction = (PatrolPath.Direction)(-(int)this.m_Direction);
				num2 = (int)(num2 + this.m_Direction);
				Debug.Log(string.Concat(new object[]
				{
					"Swapping Direction to ",
					this.m_Direction.ToString(),
					" new index: ",
					num2
				}));
				PatrolPath.IncrementWaypointResult obj;
				if (this.m_StartingDirection == this.m_Direction)
				{
					obj = PatrolPath.IncrementWaypointResult.CycleCompleted;
				}
				else
				{
					obj = PatrolPath.IncrementWaypointResult.Incremented;
				}
				onIncremenet(obj);
			}
			else
			{
				Debug.Log("New index: " + num2);
				onIncremenet(PatrolPath.IncrementWaypointResult.Incremented);
			}
		}
		else if (this.m_PatrolStyle == PatrolPath.PatrolStyle.Loop)
		{
			if (num2 < 0)
			{
				num2 = this.mWayPoints.Count - 1;
				onIncremenet(PatrolPath.IncrementWaypointResult.CycleCompleted);
			}
			else if (num2 >= this.mWayPoints.Count)
			{
				num2 = 0;
				onIncremenet(PatrolPath.IncrementWaypointResult.CycleCompleted);
			}
			else
			{
				onIncremenet(PatrolPath.IncrementWaypointResult.Incremented);
			}
		}
		else if (this.m_PatrolStyle == PatrolPath.PatrolStyle.Random)
		{
			if (this.mWayPoints.Count <= 1)
			{
				num2 = num;
			}
			else
			{
				do
				{
					num2 = GameplayRandom.Range(0, this.mWayPoints.Count);
				}
				while (num2 == num);
			}
			onIncremenet(PatrolPath.IncrementWaypointResult.Incremented);
		}
		else
		{
			onIncremenet(PatrolPath.IncrementWaypointResult.None);
		}
		return this.mWayPoints[num2];
	}

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
}
