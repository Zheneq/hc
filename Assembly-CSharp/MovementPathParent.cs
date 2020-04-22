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
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			HighlightUtils.DestroyMeshesOnObject(base.gameObject);
			return;
		}
	}
}
