using System;
using UnityEngine;
using UnityEngine.UI;

public class _SelectableBtn : MonoBehaviour
{
	public _ButtonSwapSprite spriteController;

	public GameObject m_selectedContainer;

	public bool m_ignoreHoverAnimationCall;

	public bool m_ignoreDefaultAnimationCall;

	public bool m_ignorePressAnimationCall;

	public bool m_ignoreHoverOutAnimCall;

	public int m_hoverAnimLayer;

	public int m_defaultAnimLayer;

	public int m_pressAnimLayer;

	public int m_selectAnimLayer = -1;

	public Animator m_animationController;

	public string m_animPrefix;

	private bool m_isSelected;

	private bool m_isHover;

	private bool m_isPressed;

	private bool m_isDisabled;

	private GameObject CreateNewContainer(string name)
	{
		GameObject gameObject = new GameObject(name);
		RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
		rectTransform.sizeDelta = Vector2.zero;
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		HUD_UIResources.SetParentAndAlign(gameObject, base.gameObject);
		return gameObject;
	}

	public void CreateChildren()
	{
		GameObject gameObject = this.CreateNewContainer("Default");
		gameObject.AddComponent<Image>().color = Color.white;
		this.m_selectedContainer = this.CreateNewContainer("Selected");
		GameObject gameObject2 = this.CreateNewContainer("Hover");
		gameObject2.AddComponent<Image>().color = Color.white;
		GameObject gameObject3 = this.CreateNewContainer("Pressed");
		gameObject3.AddComponent<Image>().color = Color.white;
		GameObject gameObject4 = this.CreateNewContainer("HitBox");
		Image image = gameObject4.AddComponent<Image>();
		image.color = Color.clear;
		Button button = gameObject4.AddComponent<Button>();
		button.transition = Selectable.Transition.None;
		this.spriteController = gameObject4.AddComponent<_ButtonSwapSprite>();
		this.spriteController.m_defaultImage = gameObject.GetComponent<Image>();
		this.spriteController.m_hoverImage = gameObject2.GetComponent<Image>();
		this.spriteController.m_pressedImage = gameObject3.GetComponent<Image>();
	}

	public bool IsHover
	{
		get
		{
			return this.m_isHover;
		}
	}

	public bool IsPressed
	{
		get
		{
			return this.m_isPressed;
		}
	}

	public bool IsDisabled
	{
		get
		{
			return this.m_isDisabled;
		}
	}

	private void OnEnable()
	{
		if (this.m_isSelected)
		{
			this.DoSelect(this.m_isSelected, string.Empty, string.Empty);
		}
	}

	public void Awake()
	{
		if (this.m_animationController != null)
		{
			if (!this.m_animPrefix.IsNullOrEmpty())
			{
				this.spriteController.selectableButton = this;
			}
		}
	}

	public void SetDisabled(bool disabled)
	{
		this.m_isDisabled = disabled;
		if (this.spriteController != null)
		{
			if (this.spriteController.m_defaultImage != null)
			{
				UIManager.SetGameObjectActive(this.spriteController.m_defaultImage.gameObject, !disabled, null);
			}
			if (this.spriteController.m_hoverImage != null)
			{
				UIManager.SetGameObjectActive(this.spriteController.m_hoverImage.gameObject, !disabled, null);
			}
			if (this.spriteController.m_pressedImage != null)
			{
				UIManager.SetGameObjectActive(this.spriteController.m_pressedImage.gameObject, !disabled, null);
			}
			this.spriteController.SetClickable(!disabled);
		}
	}

	public void NotifyDefaultStatusChange(bool active)
	{
		if (!this.m_ignoreDefaultAnimationCall)
		{
			if (!this.m_isSelected)
			{
				if (this.m_animationController != null)
				{
					if (this.spriteController != null)
					{
						if (this.spriteController.m_defaultImage != null)
						{
							UIManager.SetGameObjectActive(this.spriteController.m_defaultImage.gameObject, !this.m_isDisabled, null);
						}
					}
					this.PlayAnimation("Default", this.m_defaultAnimLayer, 0f);
				}
			}
		}
	}

	public void NotifyHoverStatusChange(bool active, bool forceAnim = false, float overrideNormalizedTime = 0f)
	{
		if (!this.m_ignoreHoverAnimationCall)
		{
			if (!this.m_isSelected)
			{
				if (this.m_animationController != null)
				{
					if (this.spriteController != null)
					{
						if (this.spriteController.m_hoverImage != null)
						{
							UIManager.SetGameObjectActive(this.spriteController.m_hoverImage.gameObject, !this.m_isDisabled, null);
						}
					}
					if (active)
					{
						this.PlayAnimation("HoverIN", this.m_hoverAnimLayer, overrideNormalizedTime);
					}
					else
					{
						if (!forceAnim)
						{
							if (this.m_isHover == active)
							{
								goto IL_FC;
							}
						}
						if (this.m_ignoreHoverOutAnimCall)
						{
							if (!forceAnim)
							{
								goto IL_FC;
							}
						}
						this.PlayAnimation("HoverOUT", this.m_hoverAnimLayer, overrideNormalizedTime);
					}
					IL_FC:
					this.m_isHover = active;
				}
			}
		}
	}

	public void NotifyPressStatusChange(bool active, bool forceAnim = false)
	{
		if (!this.m_ignorePressAnimationCall)
		{
			if (!this.m_isSelected)
			{
				if (this.m_animationController != null)
				{
					if (this.spriteController != null)
					{
						if (this.spriteController.m_pressedImage != null)
						{
							UIManager.SetGameObjectActive(this.spriteController.m_pressedImage.gameObject, !this.m_isDisabled, null);
						}
					}
					if (active)
					{
						this.PlayAnimation("PressIN", this.m_pressAnimLayer, 0f);
					}
					else
					{
						if (!forceAnim)
						{
							if (this.m_isPressed == active)
							{
								goto IL_13C;
							}
						}
						if (this.spriteController != null)
						{
							if (this.spriteController.IsMouseHover())
							{
								this.NotifyHoverStatusChange(true, false, 1f);
								goto IL_13C;
							}
						}
						this.PlayAnimation("PressOUT", this.m_pressAnimLayer, 0f);
					}
					IL_13C:
					this.m_isPressed = active;
				}
			}
		}
	}

	public void SelectOutAnimDone()
	{
		if (this.m_selectedContainer != null)
		{
			UIManager.SetGameObjectActive(this.m_selectedContainer, false, null);
		}
	}

	private void DoSelect(bool selected, string selectINSuffixOverride = "", string selectOutSuffixOverride = "")
	{
		this.m_isSelected = selected;
		if (selected && this.m_selectedContainer != null)
		{
			UIManager.SetGameObjectActive(this.m_selectedContainer.gameObject, true, null);
		}
		if (this.m_animationController != null)
		{
			if (selected)
			{
				string stateName;
				if (selectINSuffixOverride.IsNullOrEmpty())
				{
					stateName = "SelectIN";
				}
				else
				{
					stateName = selectINSuffixOverride;
				}
				this.PlayAnimation(stateName, this.m_selectAnimLayer, 0f);
			}
			else
			{
				if (this.spriteController != null)
				{
					if (this.spriteController.IsMouseHover())
					{
						this.NotifyHoverStatusChange(true, false, 0f);
						goto IL_F1;
					}
				}
				this.PlayAnimation((!selectOutSuffixOverride.IsNullOrEmpty()) ? selectOutSuffixOverride : "SelectOUT", this.m_selectAnimLayer, 0f);
			}
			IL_F1:;
		}
		else if (this.m_selectedContainer != null)
		{
			UIManager.SetGameObjectActive(this.m_selectedContainer.gameObject, selected, null);
		}
	}

	public bool IsSelected()
	{
		return this.m_isSelected;
	}

	public void SetSelected(bool selected, bool forceReplayAnim = false, string selectINSuffixOverride = "", string selectOutSuffixOverride = "")
	{
		if (this.m_isSelected == selected)
		{
			if (!forceReplayAnim)
			{
				return;
			}
		}
		this.DoSelect(selected, selectINSuffixOverride, selectOutSuffixOverride);
	}

	public void ToggleSelected(bool forceReplayAnim = false)
	{
		this.SetSelected(!this.IsSelected(), forceReplayAnim, string.Empty, string.Empty);
	}

	public void PlayAnimation(string stateName, int layer, float normalizedTime)
	{
		if (this.m_animationController.gameObject.activeInHierarchy)
		{
			if (this.m_animationController.gameObject.activeSelf)
			{
				string name = this.m_animPrefix + stateName;
				int num = Animator.StringToHash(name);
				bool flag = false;
				if (layer < 0)
				{
					for (int i = 0; i < this.m_animationController.layerCount; i++)
					{
						if (this.m_animationController.HasState(i, num))
						{
							flag = true;
						}
					}
				}
				else
				{
					flag = this.m_animationController.HasState(layer, num);
				}
				if (flag)
				{
					this.m_animationController.Play(num, layer, normalizedTime);
				}
				return;
			}
		}
	}

	public void SetRecordMetricClick(bool doRecording, string context = "")
	{
		this.spriteController.SetRecordMetricClick(doRecording, context);
	}
}
