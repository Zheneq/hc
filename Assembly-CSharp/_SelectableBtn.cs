using System.Text;
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

	public bool IsHover
	{
		get { return m_isHover; }
	}

	public bool IsPressed
	{
		get { return m_isPressed; }
	}

	public bool IsDisabled
	{
		get { return m_isDisabled; }
	}

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
		GameObject gameObject = CreateNewContainer("Default");
		gameObject.AddComponent<Image>().color = Color.white;
		m_selectedContainer = CreateNewContainer("Selected");
		GameObject gameObject2 = CreateNewContainer("Hover");
		gameObject2.AddComponent<Image>().color = Color.white;
		GameObject gameObject3 = CreateNewContainer("Pressed");
		gameObject3.AddComponent<Image>().color = Color.white;
		GameObject gameObject4 = CreateNewContainer("HitBox");
		Image image = gameObject4.AddComponent<Image>();
		image.color = Color.clear;
		Button button = gameObject4.AddComponent<Button>();
		button.transition = Selectable.Transition.None;
		spriteController = gameObject4.AddComponent<_ButtonSwapSprite>();
		spriteController.m_defaultImage = gameObject.GetComponent<Image>();
		spriteController.m_hoverImage = gameObject2.GetComponent<Image>();
		spriteController.m_pressedImage = gameObject3.GetComponent<Image>();
	}

	private void OnEnable()
	{
		if (!m_isSelected)
		{
			return;
		}
		while (true)
		{
			DoSelect(m_isSelected, string.Empty, string.Empty);
			return;
		}
	}

	public void Awake()
	{
		if (!(m_animationController != null))
		{
			return;
		}
		while (true)
		{
			if (!m_animPrefix.IsNullOrEmpty())
			{
				spriteController.selectableButton = this;
			}
			return;
		}
	}

	public void SetDisabled(bool disabled)
	{
		m_isDisabled = disabled;
		if (!(spriteController != null))
		{
			return;
		}
		while (true)
		{
			if (spriteController.m_defaultImage != null)
			{
				UIManager.SetGameObjectActive(spriteController.m_defaultImage.gameObject, !disabled);
			}
			if (spriteController.m_hoverImage != null)
			{
				UIManager.SetGameObjectActive(spriteController.m_hoverImage.gameObject, !disabled);
			}
			if (spriteController.m_pressedImage != null)
			{
				UIManager.SetGameObjectActive(spriteController.m_pressedImage.gameObject, !disabled);
			}
			spriteController.SetClickable(!disabled);
			return;
		}
	}

	public void NotifyDefaultStatusChange(bool active)
	{
		if (m_ignoreDefaultAnimationCall)
		{
			return;
		}
		while (true)
		{
			if (m_isSelected)
			{
				return;
			}
			while (true)
			{
				if (!(m_animationController != null))
				{
					return;
				}
				while (true)
				{
					if (spriteController != null)
					{
						if (spriteController.m_defaultImage != null)
						{
							UIManager.SetGameObjectActive(spriteController.m_defaultImage.gameObject, !m_isDisabled);
						}
					}
					PlayAnimation("Default", m_defaultAnimLayer, 0f);
					return;
				}
			}
		}
	}

	public void NotifyHoverStatusChange(bool active, bool forceAnim = false, float overrideNormalizedTime = 0f)
	{
		if (m_ignoreHoverAnimationCall)
		{
			return;
		}
		while (true)
		{
			if (m_isSelected)
			{
				return;
			}
			while (true)
			{
				if (!(m_animationController != null))
				{
					return;
				}
				if (spriteController != null)
				{
					if (spriteController.m_hoverImage != null)
					{
						UIManager.SetGameObjectActive(spriteController.m_hoverImage.gameObject, !m_isDisabled);
					}
				}
				if (active)
				{
					PlayAnimation("HoverIN", m_hoverAnimLayer, overrideNormalizedTime);
				}
				else
				{
					if (!forceAnim)
					{
						if (m_isHover == active)
						{
							goto IL_00fc;
						}
					}
					if (m_ignoreHoverOutAnimCall)
					{
						if (!forceAnim)
						{
							goto IL_00fc;
						}
					}
					PlayAnimation("HoverOUT", m_hoverAnimLayer, overrideNormalizedTime);
				}
				goto IL_00fc;
				IL_00fc:
				m_isHover = active;
				return;
			}
		}
	}

	public void NotifyPressStatusChange(bool active, bool forceAnim = false)
	{
		if (m_ignorePressAnimationCall)
		{
			return;
		}
		while (true)
		{
			if (m_isSelected)
			{
				return;
			}
			while (true)
			{
				if (!(m_animationController != null))
				{
					return;
				}
				while (true)
				{
					if (spriteController != null)
					{
						if (spriteController.m_pressedImage != null)
						{
							UIManager.SetGameObjectActive(spriteController.m_pressedImage.gameObject, !m_isDisabled);
						}
					}
					if (active)
					{
						PlayAnimation("PressIN", m_pressAnimLayer, 0f);
					}
					else
					{
						if (!forceAnim)
						{
							if (m_isPressed == active)
							{
								goto IL_013c;
							}
						}
						if (spriteController != null)
						{
							if (spriteController.IsMouseHover())
							{
								NotifyHoverStatusChange(true, false, 1f);
								goto IL_013c;
							}
						}
						PlayAnimation("PressOUT", m_pressAnimLayer, 0f);
					}
					goto IL_013c;
					IL_013c:
					m_isPressed = active;
					return;
				}
			}
		}
	}

	public void SelectOutAnimDone()
	{
		if (!(m_selectedContainer != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_selectedContainer, false);
			return;
		}
	}

	private void DoSelect(bool selected, string selectINSuffixOverride = "", string selectOutSuffixOverride = "")
	{
		m_isSelected = selected;
		if (selected && m_selectedContainer != null)
		{
			UIManager.SetGameObjectActive(m_selectedContainer.gameObject, true);
		}
		if (m_animationController != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (selected)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
							{
								object stateName;
								if (selectINSuffixOverride.IsNullOrEmpty())
								{
									stateName = "SelectIN";
								}
								else
								{
									stateName = selectINSuffixOverride;
								}
								PlayAnimation((string)stateName, m_selectAnimLayer, 0f);
								return;
							}
							}
						}
					}
					if (spriteController != null)
					{
						if (spriteController.IsMouseHover())
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									NotifyHoverStatusChange(true);
									return;
								}
							}
						}
					}
					PlayAnimation((!selectOutSuffixOverride.IsNullOrEmpty()) ? selectOutSuffixOverride : "SelectOUT", m_selectAnimLayer, 0f);
					return;
				}
			}
		}
		if (m_selectedContainer != null)
		{
			UIManager.SetGameObjectActive(m_selectedContainer.gameObject, selected);
		}
	}

	public bool IsSelected()
	{
		return m_isSelected;
	}

	public void SetSelected(bool selected, bool forceReplayAnim = false, string selectINSuffixOverride = "", string selectOutSuffixOverride = "")
	{
		if (m_isSelected == selected)
		{
			if (!forceReplayAnim)
			{
				return;
			}
		}
		DoSelect(selected, selectINSuffixOverride, selectOutSuffixOverride);
	}

	public void ToggleSelected(bool forceReplayAnim = false)
	{
		SetSelected(!IsSelected(), forceReplayAnim, string.Empty, string.Empty);
	}

	public void PlayAnimation(string stateName, int layer, float normalizedTime)
	{
		if (!m_animationController.gameObject.activeInHierarchy)
		{
			return;
		}
		while (true)
		{
			if (!m_animationController.gameObject.activeSelf)
			{
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
			string name = new StringBuilder().Append(m_animPrefix).Append(stateName).ToString();
			int num = Animator.StringToHash(name);
			bool flag = false;
			if (layer < 0)
			{
				for (int i = 0; i < m_animationController.layerCount; i++)
				{
					if (m_animationController.HasState(i, num))
					{
						flag = true;
					}
				}
			}
			else
			{
				flag = m_animationController.HasState(layer, num);
			}
			if (flag)
			{
				while (true)
				{
					m_animationController.Play(num, layer, normalizedTime);
					return;
				}
			}
			return;
		}
	}

	public void SetRecordMetricClick(bool doRecording, string context = "")
	{
		spriteController.SetRecordMetricClick(doRecording, context);
	}
}
