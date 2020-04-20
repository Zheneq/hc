using System;
using UnityEngine;

public class BrushLocation : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		Gizmos.DrawIcon(base.transform.position, "icon_Brush.png");
	}
}
