using System;
using UnityEngine;
using UnityEngine.UI;

public class UINameplateOvercon : MonoBehaviour
{
	public Image m_foregroundImg;

	public GameObject m_customPrefabParent;

	private CanvasGroup m_canvasGroup;

	private bool m_initialized;

	private float m_timeToDestroy = -1f;

	private void Awake()
	{
		this.m_canvasGroup = base.GetComponent<CanvasGroup>();
		if (this.m_canvasGroup != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplateOvercon.Awake()).MethodHandle;
			}
			this.m_canvasGroup.blocksRaycasts = false;
			this.m_canvasGroup.interactable = false;
		}
	}

	public void Initialize(ActorData actor, UIOverconData.NameToOverconEntry entry)
	{
		this.m_initialized = true;
		if (entry != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplateOvercon.Initialize(ActorData, UIOverconData.NameToOverconEntry)).MethodHandle;
			}
			if (this.m_foregroundImg != null && !string.IsNullOrEmpty(entry.m_staticSpritePath))
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
				Sprite sprite = Resources.Load(entry.m_staticSpritePath, typeof(Sprite)) as Sprite;
				if (sprite != null)
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
					this.m_foregroundImg.sprite = sprite;
					Color color = this.m_foregroundImg.color;
					color.a = entry.m_initialAlpha;
					this.m_foregroundImg.color = color;
				}
				else if (Application.isEditor)
				{
					Debug.LogWarning("Did not find overcon sprite at: " + entry.m_staticSpritePath);
				}
			}
			if (!string.IsNullOrEmpty(entry.m_customPrefabPath))
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
				if (this.m_customPrefabParent != null)
				{
					GameObject gameObject = Resources.Load(entry.m_customPrefabPath, typeof(GameObject)) as GameObject;
					if (gameObject != null)
					{
						GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
						if (gameObject2 != null)
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
							gameObject2.transform.SetParent(this.m_customPrefabParent.transform);
							gameObject2.transform.localPosition = new Vector3(0f, entry.m_customPrefabHeightOffset, 0f);
						}
					}
					else if (Application.isEditor)
					{
						Debug.LogWarning("Did not find overcon prefab at: " + entry.m_customPrefabPath);
					}
				}
			}
			this.m_timeToDestroy = Time.time + ((entry.m_ageInSeconds > 0f) ? entry.m_ageInSeconds : 8f);
		}
		else
		{
			this.m_timeToDestroy = Time.time;
		}
	}

	public void SetCanvasGroupVisibility(bool visible)
	{
		if (this.m_canvasGroup != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplateOvercon.SetCanvasGroupVisibility(bool)).MethodHandle;
			}
			if (visible)
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
				this.m_canvasGroup.alpha = 1f;
				this.m_canvasGroup.blocksRaycasts = true;
				this.m_canvasGroup.interactable = true;
			}
			else
			{
				this.m_canvasGroup.alpha = 0f;
				this.m_canvasGroup.blocksRaycasts = false;
				this.m_canvasGroup.interactable = false;
			}
		}
	}

	private void OnDestroy()
	{
		if (this.m_customPrefabParent != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplateOvercon.OnDestroy()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_customPrefabParent);
		}
	}

	private void Update()
	{
		if (this.m_initialized)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UINameplateOvercon.Update()).MethodHandle;
			}
			if (Time.time >= this.m_timeToDestroy)
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
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
