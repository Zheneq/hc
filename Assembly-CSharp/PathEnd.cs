using System;
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
		foreach (MeshRenderer meshRenderer in componentsInChildren)
		{
			if (meshRenderer.materials.Length > 0)
			{
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(PathEnd.SetColor(Color)).MethodHandle;
				}
				if (meshRenderer.materials[0] != null)
				{
					meshRenderer.materials[0].SetColor("_TintColor", newColor);
				}
			}
		}
	}
}
