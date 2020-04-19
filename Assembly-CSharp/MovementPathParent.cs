using System;
using UnityEngine;

public class MovementPathParent : MonoBehaviour
{
	private void OnDestroy()
	{
		MeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<MeshRenderer>(true);
		foreach (MeshRenderer meshRenderer in componentsInChildren)
		{
			UnityEngine.Object.Destroy(meshRenderer.material);
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(MovementPathParent.OnDestroy()).MethodHandle;
		}
		HighlightUtils.DestroyMeshesOnObject(base.gameObject);
	}
}
