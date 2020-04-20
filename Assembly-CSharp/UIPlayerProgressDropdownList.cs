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
		if (this.m_initialized)
		{
			return false;
		}
		this.m_initialized = true;
		this.m_hitboxes = new List<GameObject>();
		this.m_options = new List<UIPlayerProgressDropdownBtn>();
		if (this.m_scrollRect != null)
		{
			UIEventTriggerUtils.AddListener(this.m_scrollRect.verticalScrollbar.gameObject, EventTriggerType.Scroll, new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			this.m_hitboxes.Add(this.m_scrollRect.gameObject);
			this.m_hitboxes.Add(this.m_scrollRect.verticalScrollbar.gameObject);
		}
		this.m_animator = base.GetComponent<Animator>();
		this.m_maxHeight = (base.transform as RectTransform).sizeDelta.y;
		return true;
	}

	public void AddOption(int typeSpecificData, string text, CharacterType charType = CharacterType.None)
	{
		UIPlayerProgressDropdownBtn uiplayerProgressDropdownBtn = UnityEngine.Object.Instantiate<UIPlayerProgressDropdownBtn>(this.m_itemPrefab);
		uiplayerProgressDropdownBtn.transform.SetParent(this.m_layoutGroup.transform);
		uiplayerProgressDropdownBtn.transform.localPosition = Vector3.zero;
		uiplayerProgressDropdownBtn.transform.localScale = Vector3.one;
		uiplayerProgressDropdownBtn.SetOptionData(typeSpecificData);
		uiplayerProgressDropdownBtn.Setup(text, charType);
		UIManager.SetGameObjectActive(uiplayerProgressDropdownBtn, true, null);
		if (this.m_scrollRect != null)
		{
			uiplayerProgressDropdownBtn.m_button.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		uiplayerProgressDropdownBtn.AttachToDropdown(this);
		this.m_options.Add(uiplayerProgressDropdownBtn);
		this.m_hitboxes.Add(uiplayerProgressDropdownBtn.m_button.spriteController.gameObject);
	}

	public void AddOption(int typeSpecificData, string text, CharacterRole charRole)
	{
		UIPlayerProgressDropdownBtn uiplayerProgressDropdownBtn = UnityEngine.Object.Instantiate<UIPlayerProgressDropdownBtn>(this.m_itemPrefab);
		uiplayerProgressDropdownBtn.transform.SetParent(this.m_layoutGroup.transform);
		uiplayerProgressDropdownBtn.transform.localPosition = Vector3.zero;
		uiplayerProgressDropdownBtn.transform.localScale = Vector3.one;
		uiplayerProgressDropdownBtn.SetOptionData(typeSpecificData);
		uiplayerProgressDropdownBtn.Setup(text, charRole);
		UIManager.SetGameObjectActive(uiplayerProgressDropdownBtn, true, null);
		if (this.m_scrollRect != null)
		{
			uiplayerProgressDropdownBtn.m_button.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		uiplayerProgressDropdownBtn.AttachToDropdown(this);
		this.m_options.Add(uiplayerProgressDropdownBtn);
		this.m_hitboxes.Add(uiplayerProgressDropdownBtn.m_button.spriteController.gameObject);
	}

	private void OnScroll(BaseEventData data)
	{
		this.m_scrollRect.OnScroll((PointerEventData)data);
	}

	public void OnSelect(int typeSpecificData)
	{
		if (this.m_callback != null)
		{
			this.m_callback(typeSpecificData);
		}
		this.SetVisible(false);
	}

	public void SetSelectCallback(Action<int> callback)
	{
		this.m_callback = callback;
	}

	public void AddHitbox(GameObject hitbox)
	{
		this.m_hitboxes.Add(hitbox);
	}

	public void SetVisible(bool visible)
	{
		if (this.m_isVisible == visible)
		{
			return;
		}
		this.m_isVisible = visible;
		if (visible)
		{
			UIClickListener.Get().Enable(this.m_hitboxes, delegate
			{
				this.SetVisible(false);
			});
			UIManager.SetGameObjectActive(base.gameObject, true, null);
			for (int i = 0; i < this.m_layoutGroup.transform.childCount; i++)
			{
				RectTransform layoutRoot = this.m_layoutGroup.transform.GetChild(i) as RectTransform;
				LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_layoutGroup.transform as RectTransform);
			float preferredHeight = this.m_layoutGroup.preferredHeight;
			this.ChangeHeight(this.m_layoutGroup.transform, preferredHeight);
			this.ChangeHeight(base.transform, Mathf.Min(preferredHeight, this.m_maxHeight));
		}
		else
		{
			for (int j = 0; j < this.m_options.Count; j++)
			{
				this.m_options[j].m_button.SetSelected(false, false, string.Empty, string.Empty);
			}
			UIClickListener.Get().Disable();
		}
		if (this.m_animator != null)
		{
			if (this.m_animator.isInitialized)
			{
				UIAnimationEventManager.Get().PlayAnimation(this.m_animator, (!visible) ? "FadeDefaultOUT" : "FadeFastDefaultIN", new UIAnimationEventManager.AnimationDoneCallback(this.HandleFadeEnd), string.Empty, 0, 0f, true, false, null, null);
				return;
			}
		}
		if (!this.m_isVisible)
		{
			UIManager.SetGameObjectActive(base.gameObject, false, null);
		}
	}

	public void Toggle()
	{
		this.SetVisible(!this.m_isVisible);
	}

	private void HandleFadeEnd()
	{
		if (!this.m_isVisible)
		{
			UIManager.SetGameObjectActive(base.gameObject, false, null);
		}
	}

	public void CheckOptionDisplayState(UIPlayerProgressDropdownBtn.ShouldShow shouldShowFunction)
	{
		for (int i = 0; i < this.m_options.Count; i++)
		{
			this.m_options[i].CheckDisplayState(shouldShowFunction);
		}
	}

	public bool IsOptionVisible(int typeSpecificValue)
	{
		for (int i = 0; i < this.m_options.Count; i++)
		{
			if (this.m_options[i].IsOption(typeSpecificValue))
			{
				return this.m_options[i].gameObject.activeSelf;
			}
		}
		return false;
	}

	private void ChangeHeight(Transform transform, float newHeight)
	{
		RectTransform rectTransform = transform as RectTransform;
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);
	}

	public void HighlightCurrentOption(int currentValue)
	{
		for (int i = 0; i < this.m_options.Count; i++)
		{
			this.m_options[i].SetSelectedIfEqual(currentValue);
		}
	}
}
