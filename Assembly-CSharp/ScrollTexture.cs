using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
	public Vector2 m_scrollSpeed = new Vector2(0.03f, 0f);

	public string[] m_textureNames = new string[]
	{
		"_MainTex",
		"_EmissionMap"
	};

	private Dictionary<Material, Material> m_sharedMatToMatCopy = new Dictionary<Material, Material>();

	private void Start()
	{
		Renderer[] components = base.GetComponents<Renderer>();
		if (components != null)
		{
			foreach (Renderer renderer in components)
			{
				for (int j = 0; j < renderer.sharedMaterials.Length; j++)
				{
					if (!this.m_sharedMatToMatCopy.ContainsKey(renderer.sharedMaterials[j]))
					{
						this.m_sharedMatToMatCopy[renderer.sharedMaterials[j]] = renderer.materials[j];
					}
				}
			}
			foreach (Renderer renderer2 in (Renderer[])UnityEngine.Object.FindObjectsOfType(typeof(Renderer)))
			{
				List<int> list = new List<int>();
				for (int l = 0; l < renderer2.sharedMaterials.Length; l++)
				{
					if (renderer2.sharedMaterials[l] != null)
					{
						if (this.m_sharedMatToMatCopy.ContainsKey(renderer2.sharedMaterials[l]))
						{
							list.Add(l);
						}
					}
				}
				if (list.Count > 0)
				{
					Material[] array2 = new Material[renderer2.sharedMaterials.Length];
					for (int m = 0; m < array2.Length; m++)
					{
						if (list.Contains(m))
						{
							array2[m] = this.m_sharedMatToMatCopy[renderer2.sharedMaterials[m]];
						}
						else
						{
							array2[m] = renderer2.sharedMaterials[m];
						}
					}
					renderer2.sharedMaterials = array2;
					if (this.m_sharedMatToMatCopy.ContainsKey(renderer2.sharedMaterial))
					{
						renderer2.sharedMaterial = this.m_sharedMatToMatCopy[renderer2.sharedMaterial];
					}
				}
			}
		}
	}

	private void Update()
	{
		Vector2 textureOffset = new Vector2(Mathf.Repeat(Time.time * this.m_scrollSpeed.x, 1f), Mathf.Repeat(Time.time * this.m_scrollSpeed.y, 1f));
		this.SetTextureOffset(textureOffset);
	}

	private void OnDisable()
	{
		this.SetTextureOffset(Vector2.zero);
	}

	private void SetTextureOffset(Vector2 offset)
	{
		for (int i = 0; i < this.m_sharedMatToMatCopy.Count; i++)
		{
			Material material = this.m_sharedMatToMatCopy.Values.ElementAt(i);
			for (int j = 0; j < this.m_textureNames.Length; j++)
			{
				material.SetTextureOffset(this.m_textureNames[j], offset);
			}
		}
	}
}
