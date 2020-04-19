using System;
using UnityEngine;

public class SpawnLookAtPoint : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		Gizmos.DrawIcon(base.transform.position, "icon_SpawnLocation.png");
	}
}
