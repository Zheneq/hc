using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerProgressDropdownList : MonoBehaviour
{
	public UIPlayerProgressDropdownBtn m_itemPrefab;

	public VerticalLayoutGroup m_layoutGroup;

	public ScrollRect m_scrollRect;

	private Action<int> m_callback;

	private List<GameObject> m_hitboxes;

	private bool m_initialized;

	private List<UIPlayerProgressDropdownBtn> m_options;

	private Animator m_animator;

	private bool m_isVisible;

	private float m_maxHeight;

	public bool Initialize()
	{
		if (m_initialized)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		m_initialized = true;
		m_hitboxes = new List<GameObject>();
		m_options = new List<UIPlayerProgressDropdownBtn>();
		if (m_scrollRect != null)
		{
			UIEventTriggerUtils.AddListener(m_scrollRect.verticalScrollbar.gameObject, EventTriggerType.Scroll, OnScroll);
			m_hitboxes.Add(m_scrollRect.gameObject);
			m_hitboxes.Add(m_scrollRect.verticalScrollbar.gameObject);
		}
		m_animator = GetComponent<Animator>();
		Vector2 sizeDelta = (base.transform as RectTransform).sizeDelta;
		m_maxHeight = sizeDelta.y;
		return true;
	}

	public void AddOption(int typeSpecificData, string text, CharacterType charType = CharacterType.None)
	{
		UIPlayerProgressDropdownBtn uIPlayerProgressDropdownBtn = UnityEngine.Object.Instantiate(m_itemPrefab);
		uIPlayerProgressDropdownBtn.transform.SetParent(m_layoutGroup.transform);
		uIPlayerProgressDropdownBtn.transform.localPosition = Vector3.zero;
		uIPlayerProgressDropdownBtn.transform.localScale = Vector3.one;
		uIPlayerProgressDropdownBtn.SetOptionData(typeSpecificData);
		uIPlayerProgressDropdownBtn.Setup(text, charType);
		UIManager.SetGameObjectActive(uIPlayerProgressDropdownBtn, true);
		if (m_scrollRect != null)
		{
			uIPlayerProgressDropdownBtn.m_button.spriteController.RegisterScrollListener(OnScroll);
		}
		uIPlayerProgressDropdownBtn.AttachToDropdown(this);
		m_options.Add(uIPlayerProgressDropdownBtn);
		m_hitboxes.Add(uIPlayerProgressDropdownBtn.m_button.spriteController.gameObject);
	}

	public void AddOption(int typeSpecificData, string text, CharacterRole charRole)
	{
		UIPlayerProgressDropdownBtn uIPlayerProgressDropdownBtn = UnityEngine.Object.Instantiate(m_itemPrefab);
		uIPlayerProgressDropdownBtn.transform.SetParent(m_layoutGroup.transform);
		uIPlayerProgressDropdownBtn.transform.localPosition = Vector3.zero;
		uIPlayerProgressDropdownBtn.transform.localScale = Vector3.one;
		uIPlayerProgressDropdownBtn.SetOptionData(typeSpecificData);
		uIPlayerProgressDropdownBtn.Setup(text, charRole);
		UIManager.SetGameObjectActive(uIPlayerProgressDropdownBtn, true);
		if (m_scrollRect != null)
		{
			uIPlayerProgressDropdownBtn.m_button.spriteController.RegisterScrollListener(OnScroll);
		}
		uIPlayerProgressDropdownBtn.AttachToDropdown(this);
		m_options.Add(uIPlayerProgressDropdownBtn);
		m_hitboxes.Add(uIPlayerProgressDropdownBtn.m_button.spriteController.gameObject);
	}

	private void OnScroll(BaseEventData data)
	{
		m_scrollRect.OnScroll((PointerEventData)data);
	}

	public void OnSelect(int typeSpecificData)
	{
		if (m_callback != null)
		{
			m_callback(typeSpecificData);
		}
		SetVisible(false);
	}

	public void SetSelectCallback(Action<int> callback)
	{
		m_callback = callback;
	}

	public void AddHitbox(GameObject hitbox)
	{
		m_hitboxes.Add(hitbox);
	}

	public void SetVisible(bool visible)
	{
		if (m_isVisible == visible)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_isVisible = visible;
		if (visible)
		{
			UIClickListener.Get().Enable(m_hitboxes, delegate
			{
				SetVisible(false);
			});
			UIManager.SetGameObjectActive(base.gameObject, true);
			for (int i = 0; i < m_layoutGroup.transform.childCount; i++)
			{
				RectTransform layoutRoot = m_layoutGroup.transform.GetChild(i) as RectTransform;
				LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(m_layoutGroup.transform as RectTransform);
			float preferredHeight = m_layoutGroup.preferredHeight;
			ChangeHeight(m_layoutGroup.transform, preferredHeight);
			ChangeHeight(base.transform, Mathf.Min(preferredHeight, m_maxHeight));
		}
		else
		{
			for (int j = 0; j < m_options.Count; j++)
			{
				m_options[j].m_button.SetSelected(false, false, string.Empty, string.Empty);
			}
			UIClickListener.Get().Disable();
		}
		if (m_animator != null)
		{
			if (m_animator.isInitialized)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						UIAnimationEventManager.Get().PlayAnimation(m_animator, (!visible) ? "FadeDefaultOUT" : "FadeFastDefaultIN", HandleFadeEnd, string.Empty);
						return;
					}
				}
			}
		}
		if (m_isVisible)
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(base.gameObject, false);
			return;
		}
	}

	public void Toggle()
	{
		SetVisible(!m_isVisible);
	}

	private void HandleFadeEnd()
	{
		if (m_isVisible)
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(base.gameObject, false);
			return;
		}
	}

	public void CheckOptionDisplayState(UIPlayerProgressDropdownBtn.ShouldShow shouldShowFunction)
	{
		for (int i = 0; i < m_options.Count; i++)
		{
			m_options[i].CheckDisplayState(shouldShowFunction);
		}
		while (true)
		{
			return;
		}
	}

	public bool IsOptionVisible(int typeSpecificValue)
	{
		for (int i = 0; i < m_options.Count; i++)
		{
			if (!m_options[i].IsOption(typeSpecificValue))
			{
				continue;
			}
			while (true)
			{
				return m_options[i].gameObject.activeSelf;
			}
		}
		while (true)
		{
			return false;
		}
	}

	private void ChangeHeight(Transform transform, float newHeight)
	{
		RectTransform rectTransform = transform as RectTransform;
		Vector2 sizeDelta = rectTransform.sizeDelta;
		rectTransform.sizeDelta = new Vector2(sizeDelta.x, newHeight);
	}

	public void HighlightCurrentOption(int currentValue)
	{
		for (int i = 0; i < m_options.Count; i++)
		{
			m_options[i].SetSelectedIfEqual(currentValue);
		}
		while (true)
		{
			return;
		}
	}
}
