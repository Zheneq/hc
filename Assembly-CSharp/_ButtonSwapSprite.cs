using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class _ButtonSwapSprite : MonoBehaviour
{
	public delegate void ButtonClickCallback(BaseEventData data);

	public Image m_defaultImage;

	public Image m_hoverImage;

	public Image m_pressedImage;

	public Image m_hitBoxImage;

	public Text m_Label;

	public FrontEndButtonSounds m_soundToPlay;

	public ButtonClickCallback callback;

	public ButtonClickCallback pointerEnterCallback;

	public ButtonClickCallback pointerExitCallback;

	public bool m_disableWhenInReadyState;

	public bool m_ignoreDialogboxes;

	public _SelectableBtn selectableButton;

	private List<_ButtonSwapSprite> m_subButtons = new List<_ButtonSwapSprite>();

	private _ButtonSwapSprite m_parentButton;

	private bool m_pointerDown;

	private bool m_pointerEntered;

	private bool m_clickable = true;

	private bool m_isVisible = true;

	private bool m_alwaysDisplayHover;

	private bool m_forceHoverCallback;

	private bool m_forceExitCallback;

	private bool m_defaultActive = true;

	private bool m_hoverActive = true;

	private bool m_pressActive = true;

	private bool m_recordClick;

	private string m_clickMetricRecordContext = string.Empty;

	private ControlpadInputValue m_controlPadInput = ControlpadInputValue.INVALID;

	private void Start()
	{
		if (m_hitBoxImage == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Image component = GetComponent<Image>();
			if (component != null)
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
				if (component.sprite != null)
				{
					try
					{
						component.sprite.texture.GetPixel(0, 0);
						m_hitBoxImage = component;
					}
					catch
					{
						Log.Warning("Sprite used for hit box is not flagged for read/write.");
					}
				}
			}
		}
		if (m_hitBoxImage != null)
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
			m_hitBoxImage.alphaHitTestMinimumThreshold = 0.4f;
		}
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerClick, OnButtonClicked);
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerEnter, OnButtonEnter);
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerExit, OnButtonExit);
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerUp, OnButtonUp);
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerDown, OnButtonDown);
	}

	public void AddSubButton(_ButtonSwapSprite btn)
	{
		if (m_subButtons.Contains(btn))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_subButtons.Add(btn);
			btn.m_parentButton = this;
			return;
		}
	}

	public void RegisterScrollListener(UIEventTriggerUtils.EventDelegate eventDelegate)
	{
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.Scroll, eventDelegate);
	}

	public void RegisterControlPadInput(ControlpadInputValue controlPadInput)
	{
		m_controlPadInput = controlPadInput;
	}

	public void ForceSetPointerEntered(bool entered)
	{
		m_pointerEntered = entered;
	}

	public void SetForceExitCallback(bool alwaysOn)
	{
		m_forceExitCallback = alwaysOn;
	}

	public void SetForceHovercallback(bool alwaysOn)
	{
		m_forceHoverCallback = alwaysOn;
	}

	public void SetAlwaysHoverState(bool alwaysOn)
	{
		m_alwaysDisplayHover = alwaysOn;
	}

	public void SetClickable(bool canBeClicked)
	{
		m_clickable = canBeClicked;
	}

	public bool IsMouseHover()
	{
		return m_pointerEntered;
	}

	public bool IsClickable()
	{
		bool flag = false;
		if (m_disableWhenInReadyState)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (AppState.GetCurrent() == AppState_CharacterSelect.Get())
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GameManager.Get() != null)
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
					if (GameManager.Get().PlayerInfo != null)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						flag = (GameManager.Get().PlayerInfo.ReadyState == ReadyState.Ready);
					}
				}
			}
		}
		int result;
		if (m_isVisible)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_clickable)
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
				if (!flag)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!m_ignoreDialogboxes)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						result = ((UIDialogPopupManager.Get() == null || (UIDialogPopupManager.Get() != null && !UIDialogPopupManager.Get().IsDialogBoxOpen())) ? 1 : 0);
					}
					else
					{
						result = 1;
					}
					goto IL_010a;
				}
			}
		}
		result = 0;
		goto IL_010a;
		IL_010a:
		return (byte)result != 0;
	}

	public void SetText(string newText)
	{
		if (!(m_Label != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_Label.text = newText;
			return;
		}
	}

	public void SetRecordMetricClick(bool doRecording, string context = "")
	{
		m_recordClick = doRecording;
		m_clickMetricRecordContext = context;
	}

	private void OnButtonClicked(BaseEventData data)
	{
		if (IsClickable() && UIUtils.IsMouseInGameWindow())
		{
			ButtonClickedInternal(data);
		}
	}

	private void ButtonClickedInternal(BaseEventData data)
	{
		if (m_recordClick)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClientGameManager.Get().SendUIActionNotification(m_clickMetricRecordContext);
		}
		if (m_soundToPlay != 0)
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
			UIFrontEnd.PlaySound(m_soundToPlay);
		}
		if (callback != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			callback(data);
		}
		string text = base.gameObject.name;
		GameObject gameObject = base.gameObject;
		while (gameObject.transform.parent != null && gameObject.transform.parent != gameObject)
		{
			text = gameObject.transform.parent.name + "/" + text;
			gameObject = gameObject.transform.parent.gameObject;
		}
		if (!(HitchDetector.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			HitchDetector.Get().RecordFrameTimeForHitch(text + " was clicked");
			return;
		}
	}

	private void OnButtonEnter(BaseEventData data)
	{
		if (!UIUtils.IsMouseInGameWindow())
		{
			return;
		}
		m_pointerEntered = true;
		if (!m_forceHoverCallback)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!IsClickable())
			{
				return;
			}
		}
		if (pointerEnterCallback == null)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			pointerEnterCallback(data);
			return;
		}
	}

	private void NotifySubButtonExit(BaseEventData data, _ButtonSwapSprite childBtn)
	{
		if (!((data as PointerEventData).pointerCurrentRaycast.gameObject != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			_ButtonSwapSprite component = (data as PointerEventData).pointerCurrentRaycast.gameObject.GetComponent<_ButtonSwapSprite>();
			if (component != this)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					OnButtonExit(data);
					return;
				}
			}
			return;
		}
	}

	private void OnButtonExit(BaseEventData data)
	{
		if (m_subButtons != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_subButtons.Count > 0)
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
				if ((data as PointerEventData).pointerCurrentRaycast.gameObject != null)
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
					_ButtonSwapSprite component = (data as PointerEventData).pointerCurrentRaycast.gameObject.GetComponent<_ButtonSwapSprite>();
					if (component != null && m_subButtons.Contains(component))
					{
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
		if (m_parentButton != null)
		{
			m_parentButton.NotifySubButtonExit(data, this);
		}
		m_pointerDown = false;
		m_pointerEntered = false;
		if (!m_forceExitCallback)
		{
			if (!IsClickable())
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (pointerExitCallback != null)
		{
			pointerExitCallback(data);
		}
	}

	private void OnButtonUp(BaseEventData data)
	{
		m_pointerDown = false;
	}

	private void OnButtonDown(BaseEventData data)
	{
		if (!UIUtils.IsMouseInGameWindow())
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_pointerDown = true;
			return;
		}
	}

	public void ResetMouseState()
	{
		m_pointerDown = false;
		m_pointerEntered = false;
		if (selectableButton != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					selectableButton.NotifyHoverStatusChange(false);
					selectableButton.NotifyPressStatusChange(false);
					return;
				}
			}
		}
		if (m_hoverImage != null && m_hoverImage.gameObject != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(m_hoverImage, false);
		}
		if (!(m_pressedImage != null))
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
			if (m_pressedImage.gameObject != null)
			{
				UIManager.SetGameObjectActive(m_pressedImage, false);
			}
			return;
		}
	}

	public void SetVisible(bool visible)
	{
		m_isVisible = visible;
	}

	public void SetSelectableBtn(_SelectableBtn selectableBtn)
	{
		selectableButton = selectableBtn;
	}

	private void OnDisable()
	{
		ResetMouseState();
		m_hoverActive = false;
	}

	private void Update()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (m_isVisible)
		{
			if (m_pointerDown)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (selectableButton != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (selectableButton.m_ignorePressAnimationCall)
					{
						flag = !IsClickable();
						flag2 = (m_alwaysDisplayHover || IsClickable());
						flag3 = false;
						goto IL_00f3;
					}
				}
				flag = !IsClickable();
				int num;
				if (!m_alwaysDisplayHover)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					num = 0;
				}
				else
				{
					num = 1;
				}
				flag2 = ((byte)num != 0);
				flag3 = IsClickable();
			}
			else if (m_pointerEntered)
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
				flag = !IsClickable();
				int num2;
				if (!m_alwaysDisplayHover)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					num2 = (IsClickable() ? 1 : 0);
				}
				else
				{
					num2 = 1;
				}
				flag2 = ((byte)num2 != 0);
				flag3 = false;
			}
			else
			{
				flag = true;
				flag2 = (m_alwaysDisplayHover ? true : false);
				flag3 = false;
			}
			goto IL_00f3;
		}
		flag = false;
		int num3;
		if (!m_alwaysDisplayHover)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			num3 = 0;
		}
		else
		{
			num3 = 1;
		}
		flag2 = ((byte)num3 != 0);
		flag3 = false;
		goto IL_0148;
		IL_0148:
		if (m_defaultImage != null)
		{
			if (selectableButton != null)
			{
				if (m_defaultActive != flag)
				{
					selectableButton.NotifyDefaultStatusChange(flag);
				}
			}
			else
			{
				UIManager.SetGameObjectActive(m_defaultImage, flag);
			}
		}
		if (m_hoverImage != null)
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
			if (selectableButton != null)
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
				if (m_hoverActive != flag2)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					selectableButton.NotifyHoverStatusChange(flag2);
				}
			}
			else
			{
				UIManager.SetGameObjectActive(m_hoverImage, flag2);
			}
		}
		if (m_pressedImage != null)
		{
			if (selectableButton != null)
			{
				if (m_pressActive != flag3)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					selectableButton.NotifyPressStatusChange(flag3);
				}
			}
			else
			{
				UIManager.SetGameObjectActive(m_pressedImage, flag3);
			}
		}
		m_defaultActive = flag;
		m_hoverActive = flag2;
		m_pressActive = flag3;
		if (!(m_Label != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			UIManager.SetGameObjectActive(m_Label, flag3);
			return;
		}
		IL_00f3:
		if (IsClickable() && m_controlPadInput != ControlpadInputValue.INVALID && ControlpadGameplay.Get().GetButtonDown(m_controlPadInput))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			ButtonClickedInternal(null);
		}
		goto IL_0148;
	}
}
