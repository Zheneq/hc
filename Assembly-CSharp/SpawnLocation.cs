using System;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SpawnLocation.OnDrawGizmos()).MethodHandle;
			}
			return;
		}
		Gizmos.DrawIcon(base.transform.position, "icon_SpawnLocation.png");
	}
}
