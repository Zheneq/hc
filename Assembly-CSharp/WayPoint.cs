using System;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
	[Tooltip("How many turns should the NPC delay at this point. Use only values from 0..N")]
	public int TurnsToDelay;

	[Tooltip("If false, we will get as close as we can to this waypoint, then continue. If true, we will wait till is open (if occupied by someone else)")]
	public bool MustArriveAtWayPointToContinue;

	private void Start()
	{
		this.TurnsToDelay = Mathf.Max(0, this.TurnsToDelay);
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		Gizmos.DrawIcon(base.transform.position, "locationIcon.png");
	}
}
