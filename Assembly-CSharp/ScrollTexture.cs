using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
	public Vector2 m_scrollSpeed = new Vector2(0.03f, 0f);

	public string[] m_textureNames = new string[2]
	{
		"_MainTex",
		"_EmissionMap"
	};

	private Dictionary<Material, Material> m_sharedMatToMatCopy = new Dictionary<Material, Material>();

	private void Start()
	{
		Renderer[] components = GetComponents<Renderer>();
		if (components == null)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			foreach (Renderer renderer in components)
			{
				for (int j = 0; j < renderer.sharedMaterials.Length; j++)
				{
					if (!m_sharedMatToMatCopy.ContainsKey(renderer.sharedMaterials[j]))
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						m_sharedMatToMatCopy[renderer.sharedMaterials[j]] = renderer.materials[j];
					}
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						goto end_IL_007d;
					}
					continue;
					end_IL_007d:
					break;
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				Renderer[] array = (Renderer[])Object.FindObjectsOfType(typeof(Renderer));
				int num = 0;
				while (num < array.Length)
				{
					Renderer renderer2 = array[num];
					List<int> list = new List<int>();
					for (int k = 0; k < renderer2.sharedMaterials.Length; k++)
					{
						if (renderer2.sharedMaterials[k] != null)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (m_sharedMatToMatCopy.ContainsKey(renderer2.sharedMaterials[k]))
							{
								list.Add(k);
							}
						}
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						if (list.Count > 0)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							Material[] array2 = new Material[renderer2.sharedMaterials.Length];
							for (int l = 0; l < array2.Length; l++)
							{
								if (list.Contains(l))
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
									array2[l] = m_sharedMatToMatCopy[renderer2.sharedMaterials[l]];
								}
								else
								{
									array2[l] = renderer2.sharedMaterials[l];
								}
							}
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							renderer2.sharedMaterials = array2;
							if (m_sharedMatToMatCopy.ContainsKey(renderer2.sharedMaterial))
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								renderer2.sharedMaterial = m_sharedMatToMatCopy[renderer2.sharedMaterial];
							}
						}
						num++;
						goto IL_020b;
					}
					IL_020b:;
				}
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	private void Update()
	{
		Vector2 textureOffset = new Vector2(Mathf.Repeat(Time.time * m_scrollSpeed.x, 1f), Mathf.Repeat(Time.time * m_scrollSpeed.y, 1f));
		SetTextureOffset(textureOffset);
	}

	private void OnDisable()
	{
		SetTextureOffset(Vector2.zero);
	}

	private void SetTextureOffset(Vector2 offset)
	{
		int num = 0;
		while (num < m_sharedMatToMatCopy.Count)
		{
			Material material = m_sharedMatToMatCopy.Values.ElementAt(num);
			for (int i = 0; i < m_textureNames.Length; i++)
			{
				material.SetTextureOffset(m_textureNames[i], offset);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num++;
				goto IL_0053;
			}
			IL_0053:;
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}
}
