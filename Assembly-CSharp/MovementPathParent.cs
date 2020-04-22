using UnityEngine;

public class MovementPathParent : MonoBehaviour
{
	private void OnDestroy()
	{
		MeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<MeshRenderer>(true);
		MeshRenderer[] array = componentsInChildren;
		foreach (MeshRenderer meshRenderer in array)
		{
			Object.Destroy(meshRenderer.material);
		}
		while (true)
		{
			HighlightUtils.DestroyMeshesOnObject(base.gameObject);
			return;
		}
	}
}
