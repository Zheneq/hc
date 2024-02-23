using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FadeObjectGroup : MonoBehaviour
{
	public GameObject[] m_gameObjectsToFade;

	public bool m_fadeChildObjects;

	public bool m_overrideFadedValueWithZero;

	[Header("-- Objects to enable/disable based on current alpha, for things like vfx objects --")]
	public GameObject[] m_vfxObjectsToDisable;

	public float m_alphaCutoffToDisableVfxObjects = 0.98f;

	private FadeObject[] m_fadeObjects;

	private Renderer[] m_renderers;

	private bool m_vfxObjectsEnableOnLastUpdate = true;

	private void Start()
	{
		List<GameObject> list = new List<GameObject>();
		if (m_gameObjectsToFade != null)
		{
			for (int i = 0; i < m_gameObjectsToFade.Length; i++)
			{
				if (m_gameObjectsToFade[i] != null)
				{
					list.Add(m_gameObjectsToFade[i]);
				}
			}
		}
		m_gameObjectsToFade = list.ToArray();
		if (m_fadeChildObjects)
		{
			Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
			List<GameObject> list2 = new List<GameObject>(componentsInChildren.Length);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				list2.Add(componentsInChildren[j].gameObject);
			}
			list2.AddRange(m_gameObjectsToFade);
			m_gameObjectsToFade = list2.ToArray();
		}
		m_fadeObjects = new FadeObject[m_gameObjectsToFade.Length];
		m_renderers = new Renderer[m_gameObjectsToFade.Length];
		for (int k = 0; k < m_gameObjectsToFade.Length; k++)
		{
			if (!(m_gameObjectsToFade[k] != null))
			{
				continue;
			}
			m_fadeObjects[k] = m_gameObjectsToFade[k].GetComponent<FadeObject>();
			if (m_fadeObjects[k] == null)
			{
				m_fadeObjects[k] = m_gameObjectsToFade[k].AddComponent<FadeObject>();
			}
			m_renderers[k] = m_gameObjectsToFade[k].GetComponent<Renderer>();
		}
		while (true)
		{
			if (m_fadeObjects.Length > 32)
			{
				Log.Warning("Level art warning: too many ({0}) renderers grouped with {1}, performance will suffer. Please combine static meshes that share the same materials.", m_fadeObjects.Length, base.gameObject.name);
			}
			if (m_vfxObjectsToDisable == null)
			{
				return;
			}
			while (true)
			{
				if (m_vfxObjectsToDisable.Length <= 0)
				{
					return;
				}
				while (true)
				{
					List<GameObject> list3 = new List<GameObject>(m_gameObjectsToFade);
					List<GameObject> list4 = new List<GameObject>();
					for (int l = 0; l < m_vfxObjectsToDisable.Length; l++)
					{
						GameObject gameObject = m_vfxObjectsToDisable[l];
						if (!(gameObject != null))
						{
							continue;
						}
						if (list4.Contains(gameObject))
						{
							continue;
						}
						if (list3.Contains(gameObject))
						{
							if (Application.isEditor)
							{
								Debug.LogWarning(new StringBuilder().Append("[").Append(gameObject.name).Append("] in group [").Append(base.gameObject.name).Append("] is already in fade object list, please remove from VFX Objects to Disable list").ToString());
							}
						}
						else
						{
							list4.Add(gameObject);
						}
					}
					m_vfxObjectsToDisable = list4.ToArray();
					return;
				}
			}
		}
	}

	public void SetTargetTransparency(float transparency, float fadeOutDuration, float fadeInDuration, Shader transparentShader)
	{
		for (int i = 0; i < m_fadeObjects.Length; i++)
		{
			if (!(m_fadeObjects[i] != null))
			{
				continue;
			}
			if (m_overrideFadedValueWithZero)
			{
				m_fadeObjects[i].SetTargetTransparency(0f, 0.1f, 0.1f, transparentShader);
			}
			else
			{
				m_fadeObjects[i].SetTargetTransparency(transparency, fadeOutDuration, fadeInDuration, transparentShader);
			}
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public bool AreRenderersEnabled()
	{
		bool result = false;
		int num = 0;
		while (true)
		{
			if (num < m_renderers.Length)
			{
				if (m_renderers[num].enabled)
				{
					result = true;
					break;
				}
				num++;
				continue;
			}
			break;
		}
		return result;
	}

	public bool ShouldProcessEvenIfRendererIsDisabled()
	{
		bool result = false;
		int num = 0;
		while (true)
		{
			if (num < m_gameObjectsToFade.Length)
			{
				if (m_gameObjectsToFade[num] != null)
				{
					FadeObject component = m_gameObjectsToFade[num].GetComponent<FadeObject>();
					if (component != null && component.ShouldProcessEvenIfRendererIsDisabled())
					{
						result = true;
						break;
					}
				}
				num++;
				continue;
			}
			break;
		}
		return result;
	}

	private void Update()
	{
		if (m_vfxObjectsToDisable == null)
		{
			return;
		}
		while (true)
		{
			if (m_vfxObjectsToDisable.Length <= 0)
			{
				return;
			}
			while (true)
			{
				if (m_fadeObjects.Length <= 0)
				{
					return;
				}
				while (true)
				{
					if (!(m_fadeObjects[0] != null))
					{
						return;
					}
					float currentAlpha = m_fadeObjects[0].GetCurrentAlpha();
					bool flag = currentAlpha >= m_alphaCutoffToDisableVfxObjects;
					if (flag == m_vfxObjectsEnableOnLastUpdate)
					{
						return;
					}
					while (true)
					{
						m_vfxObjectsEnableOnLastUpdate = flag;
						for (int i = 0; i < m_vfxObjectsToDisable.Length; i++)
						{
							GameObject gameObject = m_vfxObjectsToDisable[i];
							if (gameObject != null)
							{
								gameObject.SetActive(flag);
							}
						}
						while (true)
						{
							switch (6)
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
		}
	}

	public float GetCurrentAlpha()
	{
		if (m_fadeObjects != null)
		{
			if (m_fadeObjects.Length > 0 && m_fadeObjects[0] != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return m_fadeObjects[0].GetCurrentAlpha();
					}
				}
			}
		}
		return 1f;
	}
}
