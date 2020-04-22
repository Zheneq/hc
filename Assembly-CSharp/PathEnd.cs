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
				if (meshRenderer.materials[0] != null)
				{
					meshRenderer.materials[0].SetColor("_TintColor", newColor);
				}
			}
		}
	}
}
