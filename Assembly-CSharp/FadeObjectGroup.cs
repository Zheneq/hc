using System;
using System.Collections.Generic;
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
		if (this.m_gameObjectsToFade != null)
		{
			for (int i = 0; i < this.m_gameObjectsToFade.Length; i++)
			{
				if (this.m_gameObjectsToFade[i] != null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(FadeObjectGroup.Start()).MethodHandle;
					}
					list.Add(this.m_gameObjectsToFade[i]);
				}
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_gameObjectsToFade = list.ToArray();
		if (this.m_fadeChildObjects)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
			List<GameObject> list2 = new List<GameObject>(componentsInChildren.Length);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				list2.Add(componentsInChildren[j].gameObject);
			}
			list2.AddRange(this.m_gameObjectsToFade);
			this.m_gameObjectsToFade = list2.ToArray();
		}
		this.m_fadeObjects = new FadeObject[this.m_gameObjectsToFade.Length];
		this.m_renderers = new Renderer[this.m_gameObjectsToFade.Length];
		for (int k = 0; k < this.m_gameObjectsToFade.Length; k++)
		{
			if (this.m_gameObjectsToFade[k] != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_fadeObjects[k] = this.m_gameObjectsToFade[k].GetComponent<FadeObject>();
				if (this.m_fadeObjects[k] == null)
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
					this.m_fadeObjects[k] = this.m_gameObjectsToFade[k].AddComponent<FadeObject>();
				}
				this.m_renderers[k] = this.m_gameObjectsToFade[k].GetComponent<Renderer>();
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		if (this.m_fadeObjects.Length > 0x20)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			Log.Warning("Level art warning: too many ({0}) renderers grouped with {1}, performance will suffer. Please combine static meshes that share the same materials.", new object[]
			{
				this.m_fadeObjects.Length,
				base.gameObject.name
			});
		}
		if (this.m_vfxObjectsToDisable != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_vfxObjectsToDisable.Length > 0)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				List<GameObject> list3 = new List<GameObject>(this.m_gameObjectsToFade);
				List<GameObject> list4 = new List<GameObject>();
				for (int l = 0; l < this.m_vfxObjectsToDisable.Length; l++)
				{
					GameObject gameObject = this.m_vfxObjectsToDisable[l];
					if (gameObject != null)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!list4.Contains(gameObject))
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (list3.Contains(gameObject))
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								if (Application.isEditor)
								{
									Debug.LogWarning(string.Concat(new string[]
									{
										"[",
										gameObject.name,
										"] in group [",
										base.gameObject.name,
										"] is already in fade object list, please remove from VFX Objects to Disable list"
									}));
								}
							}
							else
							{
								list4.Add(gameObject);
							}
						}
					}
				}
				this.m_vfxObjectsToDisable = list4.ToArray();
			}
		}
	}

	public void SetTargetTransparency(float transparency, float fadeOutDuration, float fadeInDuration, Shader transparentShader)
	{
		for (int i = 0; i < this.m_fadeObjects.Length; i++)
		{
			if (this.m_fadeObjects[i] != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FadeObjectGroup.SetTargetTransparency(float, float, float, Shader)).MethodHandle;
				}
				if (this.m_overrideFadedValueWithZero)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_fadeObjects[i].SetTargetTransparency(0f, 0.1f, 0.1f, transparentShader);
				}
				else
				{
					this.m_fadeObjects[i].SetTargetTransparency(transparency, fadeOutDuration, fadeInDuration, transparentShader);
				}
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	public bool AreRenderersEnabled()
	{
		bool result = false;
		for (int i = 0; i < this.m_renderers.Length; i++)
		{
			if (this.m_renderers[i].enabled)
			{
				result = true;
				return result;
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(FadeObjectGroup.AreRenderersEnabled()).MethodHandle;
			return result;
		}
		return result;
	}

	public bool ShouldProcessEvenIfRendererIsDisabled()
	{
		bool result = false;
		for (int i = 0; i < this.m_gameObjectsToFade.Length; i++)
		{
			if (this.m_gameObjectsToFade[i] != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FadeObjectGroup.ShouldProcessEvenIfRendererIsDisabled()).MethodHandle;
				}
				FadeObject component = this.m_gameObjectsToFade[i].GetComponent<FadeObject>();
				if (component != null && component.ShouldProcessEvenIfRendererIsDisabled())
				{
					result = true;
					return result;
				}
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			return result;
		}
	}

	private void Update()
	{
		if (this.m_vfxObjectsToDisable != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FadeObjectGroup.Update()).MethodHandle;
			}
			if (this.m_vfxObjectsToDisable.Length > 0)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_fadeObjects.Length > 0)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_fadeObjects[0] != null)
					{
						float currentAlpha = this.m_fadeObjects[0].GetCurrentAlpha();
						bool flag = currentAlpha >= this.m_alphaCutoffToDisableVfxObjects;
						if (flag != this.m_vfxObjectsEnableOnLastUpdate)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_vfxObjectsEnableOnLastUpdate = flag;
							for (int i = 0; i < this.m_vfxObjectsToDisable.Length; i++)
							{
								GameObject gameObject = this.m_vfxObjectsToDisable[i];
								if (gameObject != null)
								{
									for (;;)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									gameObject.SetActive(flag);
								}
							}
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
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
		if (this.m_fadeObjects != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FadeObjectGroup.GetCurrentAlpha()).MethodHandle;
			}
			if (this.m_fadeObjects.Length > 0 && this.m_fadeObjects[0] != null)
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
				return this.m_fadeObjects[0].GetCurrentAlpha();
			}
		}
		return 1f;
	}
}
