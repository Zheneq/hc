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
			this.m_canvasGroup.blocksRaycasts = false;
			this.m_canvasGroup.interactable = false;
		}
	}

	public void Initialize(ActorData actor, UIOverconData.NameToOverconEntry entry)
	{
		this.m_initialized = true;
		if (entry != null)
		{
			if (this.m_foregroundImg != null && !string.IsNullOrEmpty(entry.m_staticSpritePath))
			{
				Sprite sprite = Resources.Load(entry.m_staticSpritePath, typeof(Sprite)) as Sprite;
				if (sprite != null)
				{
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
				if (this.m_customPrefabParent != null)
				{
					GameObject gameObject = Resources.Load(entry.m_customPrefabPath, typeof(GameObject)) as GameObject;
					if (gameObject != null)
					{
						GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
						if (gameObject2 != null)
						{
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
			if (visible)
			{
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
			UnityEngine.Object.Destroy(this.m_customPrefabParent);
		}
	}

	private void Update()
	{
		if (this.m_initialized)
		{
			if (Time.time >= this.m_timeToDestroy)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
