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
		HighlightUtils.DestroyMeshesOnObject(base.gameObject);
	}
}
