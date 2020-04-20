﻿using System;
using UnityEngine;

public class PathStart : MonoBehaviour
{
	public virtual void AbilityCasted(GridPos startPosition, GridPos endPosition)
	{
	}

	public virtual void SetColor(Color newColor)
	{
		MeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in componentsInChildren)
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
