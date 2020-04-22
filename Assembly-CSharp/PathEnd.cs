using UnityEngine;

public class PathEnd : MonoBehaviour
{
	public virtual void AbilityCasted(GridPos startPosition, GridPos endPosition)
	{
	}

	public virtual void Setup()
	{
	}

	public void SetColor(Color newColor)
	{
		MeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<MeshRenderer>();
		MeshRenderer[] array = componentsInChildren;
		foreach (MeshRenderer meshRenderer in array)
		{
			if (meshRenderer.materials.Length > 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (meshRenderer.materials[0] != null)
				{
					meshRenderer.materials[0].SetColor("_TintColor", newColor);
				}
			}
		}
	}
}
