using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class _ButtonSwapSprite : MonoBehaviour
{
	public Image m_defaultImage;

	public Image m_hoverImage;

	public Image m_pressedImage;

	public Image m_hitBoxImage;

	public Text m_Label;

	public FrontEndButtonSounds m_soundToPlay;

	public _ButtonSwapSprite.ButtonClickCallback callback;

	public _ButtonSwapSprite.ButtonClickCallback pointerEnterCallback;

	public _ButtonSwapSprite.ButtonClickCallback pointerExitCallback;

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
		if (this.m_hitBoxImage == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_ButtonSwapSprite.Start()).MethodHandle;
			}
			Image component = base.GetComponent<Image>();
			if (component != null)
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
				if (component.sprite != null)
				{
					try
					{
						component.sprite.texture.GetPixel(0, 0);
						this.m_hitBoxImage = component;
					}
					catch
					{
						Log.Warning("Sprite used for hit box is not flagged for read/write.", new object[0]);
					}
				}
			}
		}
		if (this.m_hitBoxImage != null)
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
			this.m_hitBoxImage.alphaHitTestMinimumThreshold = 0.4f;
		}
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnButtonClicked));
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnButtonEnter));
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.OnButtonExit));
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerUp, new UIEventTriggerUtils.EventDelegate(this.OnButtonUp));
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerDown, new UIEventTriggerUtils.EventDelegate(this.OnButtonDown));
	}

	public void AddSubButton(_ButtonSwapSprite btn)
	{
		if (!this.m_subButtons.Contains(btn))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_ButtonSwapSprite.AddSubButton(_ButtonSwapSprite)).MethodHandle;
			}
			this.m_subButtons.Add(btn);
			btn.m_parentButton = this;
		}
	}

	public void RegisterScrollListener(UIEventTriggerUtils.EventDelegate eventDelegate)
	{
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.Scroll, eventDelegate);
	}

	public void RegisterControlPadInput(ControlpadInputValue controlPadInput)
	{
		this.m_controlPadInput = controlPadInput;
	}

	public void ForceSetPointerEntered(bool entered)
	{
		this.m_pointerEntered = entered;
	}

	public void SetForceExitCallback(bool alwaysOn)
	{
		this.m_forceExitCallback = alwaysOn;
	}

	public void SetForceHovercallback(bool alwaysOn)
	{
		this.m_forceHoverCallback = alwaysOn;
	}

	public void SetAlwaysHoverState(bool alwaysOn)
	{
		this.m_alwaysDisplayHover = alwaysOn;
	}

	public void SetClickable(bool canBeClicked)
	{
		this.m_clickable = canBeClicked;
	}

	public bool IsMouseHover()
	{
		return this.m_pointerEntered;
	}

	public bool IsClickable()
	{
		bool flag = false;
		if (this.m_disableWhenInReadyState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_ButtonSwapSprite.IsClickable()).MethodHandle;
			}
			if (AppState.GetCurrent() == AppState_CharacterSelect.Get())
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
				if (GameManager.Get() != null)
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
					if (GameManager.Get().PlayerInfo != null)
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
						flag = (GameManager.Get().PlayerInfo.ReadyState == ReadyState.Ready);
					}
				}
			}
		}
		int result;
		if (this.m_isVisible)
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
			if (this.m_clickable)
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
				if (!flag)
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
					if (!this.m_ignoreDialogboxes)
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
						result = ((UIDialogPopupManager.Get() == null || (UIDialogPopupManager.Get() != null && !UIDialogPopupManager.Get().IsDialogBoxOpen())) ? 1 : 0);
					}
					else
					{
						result = 1;
					}
					return result != 0;
				}
			}
		}
		result = 0;
		return result != 0;
	}

	public void SetText(string newText)
	{
		if (this.m_Label != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_ButtonSwapSprite.SetText(string)).MethodHandle;
			}
			this.m_Label.text = newText;
		}
	}

	public void SetRecordMetricClick(bool doRecording, string context = "")
	{
		this.m_recordClick = doRecording;
		this.m_clickMetricRecordContext = context;
	}

	private void OnButtonClicked(BaseEventData data)
	{
		if (this.IsClickable() && UIUtils.IsMouseInGameWindow())
		{
			this.ButtonClickedInternal(data);
		}
	}

	private void ButtonClickedInternal(BaseEventData data)
	{
		if (this.m_recordClick)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_ButtonSwapSprite.ButtonClickedInternal(BaseEventData)).MethodHandle;
			}
			ClientGameManager.Get().SendUIActionNotification(this.m_clickMetricRecordContext);
		}
		if (this.m_soundToPlay != FrontEndButtonSounds.None)
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
			UIFrontEnd.PlaySound(this.m_soundToPlay);
		}
		if (this.callback != null)
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
			this.callback(data);
		}
		string text = base.gameObject.name;
		GameObject gameObject = base.gameObject;
		while (gameObject.transform.parent != null && gameObject.transform.parent != gameObject)
		{
			text = gameObject.transform.parent.name + "/" + text;
			gameObject = gameObject.transform.parent.gameObject;
		}
		if (HitchDetector.Get() != null)
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
			HitchDetector.Get().RecordFrameTimeForHitch(text + " was clicked");
		}
	}

	private void OnButtonEnter(BaseEventData data)
	{
		if (UIUtils.IsMouseInGameWindow())
		{
			this.m_pointerEntered = true;
			if (!this.m_forceHoverCallback)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(_ButtonSwapSprite.OnButtonEnter(BaseEventData)).MethodHandle;
				}
				if (!this.IsClickable())
				{
					return;
				}
			}
			if (this.pointerEnterCallback != null)
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
				this.pointerEnterCallback(data);
			}
		}
	}

	private void NotifySubButtonExit(BaseEventData data, _ButtonSwapSprite childBtn)
	{
		if ((data as PointerEventData).pointerCurrentRaycast.gameObject != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_ButtonSwapSprite.NotifySubButtonExit(BaseEventData, _ButtonSwapSprite)).MethodHandle;
			}
			_ButtonSwapSprite component = (data as PointerEventData).pointerCurrentRaycast.gameObject.GetComponent<_ButtonSwapSprite>();
			if (component != this)
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
				this.OnButtonExit(data);
			}
		}
	}

	private void OnButtonExit(BaseEventData data)
	{
		if (this.m_subButtons != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_ButtonSwapSprite.OnButtonExit(BaseEventData)).MethodHandle;
			}
			if (this.m_subButtons.Count > 0)
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
				if ((data as PointerEventData).pointerCurrentRaycast.gameObject != null)
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
					_ButtonSwapSprite component = (data as PointerEventData).pointerCurrentRaycast.gameObject.GetComponent<_ButtonSwapSprite>();
					if (component != null && this.m_subButtons.Contains(component))
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
						return;
					}
				}
			}
		}
		if (this.m_parentButton != null)
		{
			this.m_parentButton.NotifySubButtonExit(data, this);
		}
		this.m_pointerDown = false;
		this.m_pointerEntered = false;
		if (!this.m_forceExitCallback)
		{
			if (!this.IsClickable())
			{
				return;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (this.pointerExitCallback != null)
		{
			this.pointerExitCallback(data);
		}
	}

	private void OnButtonUp(BaseEventData data)
	{
		this.m_pointerDown = false;
	}

	private void OnButtonDown(BaseEventData data)
	{
		if (UIUtils.IsMouseInGameWindow())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_ButtonSwapSprite.OnButtonDown(BaseEventData)).MethodHandle;
			}
			this.m_pointerDown = true;
		}
	}

	public void ResetMouseState()
	{
		this.m_pointerDown = false;
		this.m_pointerEntered = false;
		if (this.selectableButton != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_ButtonSwapSprite.ResetMouseState()).MethodHandle;
			}
			this.selectableButton.NotifyHoverStatusChange(false, false, 0f);
			this.selectableButton.NotifyPressStatusChange(false, false);
		}
		else
		{
			if (this.m_hoverImage != null && this.m_hoverImage.gameObject != null)
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
				UIManager.SetGameObjectActive(this.m_hoverImage, false, null);
			}
			if (this.m_pressedImage != null)
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
				if (this.m_pressedImage.gameObject != null)
				{
					UIManager.SetGameObjectActive(this.m_pressedImage, false, null);
				}
			}
		}
	}

	public void SetVisible(bool visible)
	{
		this.m_isVisible = visible;
	}

	public void SetSelectableBtn(_SelectableBtn selectableBtn)
	{
		this.selectableButton = selectableBtn;
	}

	private void OnDisable()
	{
		this.ResetMouseState();
		this.m_hoverActive = false;
	}

	private void Update()
	{
		bool flag;
		bool flag2;
		bool flag3;
		if (this.m_isVisible)
		{
			if (this.m_pointerDown)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(_ButtonSwapSprite.Update()).MethodHandle;
				}
				if (this.selectableButton != null)
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
					if (this.selectableButton.m_ignorePressAnimationCall)
					{
						flag = !this.IsClickable();
						flag2 = (this.m_alwaysDisplayHover || this.IsClickable());
						flag3 = false;
						goto IL_A0;
					}
				}
				flag = !this.IsClickable();
				bool flag4;
				if (!this.m_alwaysDisplayHover)
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
					flag4 = false;
				}
				else
				{
					flag4 = true;
				}
				flag2 = flag4;
				flag3 = this.IsClickable();
				IL_A0:;
			}
			else if (this.m_pointerEntered)
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
				flag = !this.IsClickable();
				bool flag5;
				if (!this.m_alwaysDisplayHover)
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
					flag5 = this.IsClickable();
				}
				else
				{
					flag5 = true;
				}
				flag2 = flag5;
				flag3 = false;
			}
			else
			{
				flag = true;
				flag2 = this.m_alwaysDisplayHover;
				flag3 = false;
			}
			if (this.IsClickable() && this.m_controlPadInput != ControlpadInputValue.INVALID && ControlpadGameplay.Get().GetButtonDown(this.m_controlPadInput))
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
				this.ButtonClickedInternal(null);
			}
		}
		else
		{
			flag = false;
			bool flag6;
			if (!this.m_alwaysDisplayHover)
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
				flag6 = false;
			}
			else
			{
				flag6 = true;
			}
			flag2 = flag6;
			flag3 = false;
		}
		if (this.m_defaultImage != null)
		{
			if (this.selectableButton != null)
			{
				if (this.m_defaultActive != flag)
				{
					this.selectableButton.NotifyDefaultStatusChange(flag);
				}
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_defaultImage, flag, null);
			}
		}
		if (this.m_hoverImage != null)
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
			if (this.selectableButton != null)
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
				if (this.m_hoverActive != flag2)
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
					this.selectableButton.NotifyHoverStatusChange(flag2, false, 0f);
				}
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_hoverImage, flag2, null);
			}
		}
		if (this.m_pressedImage != null)
		{
			if (this.selectableButton != null)
			{
				if (this.m_pressActive != flag3)
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
					this.selectableButton.NotifyPressStatusChange(flag3, false);
				}
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_pressedImage, flag3, null);
			}
		}
		this.m_defaultActive = flag;
		this.m_hoverActive = flag2;
		this.m_pressActive = flag3;
		if (this.m_Label != null)
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
			UIManager.SetGameObjectActive(this.m_Label, flag3, null);
		}
	}

	public delegate void ButtonClickCallback(BaseEventData data);
}
