using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class _ToggleSwap : MonoBehaviour
{
	public _ButtonSwapSprite m_onButton;

	public _ButtonSwapSprite m_offButton;

	public RectTransform m_onContainer;

	public RectTransform m_offContainer;

	public _ToggleSwap.NotifyChanged changedNotify;

	private bool m_isOn;

	public bool IsChecked()
	{
		return this.m_isOn;
	}

	private void Awake()
	{
		if (this.m_onButton != null)
		{
			this.m_onButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ToggleButton);
		}
		if (this.m_offButton != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_ToggleSwap.Awake()).MethodHandle;
			}
			this.m_offButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ToggleButton);
		}
		if (this.m_onContainer != null)
		{
			UIManager.SetGameObjectActive(this.m_onContainer.gameObject, this.m_isOn, null);
		}
		if (this.m_offContainer != null)
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
			UIManager.SetGameObjectActive(this.m_offContainer.gameObject, !this.m_isOn, null);
		}
	}

	public void ToggleButton(BaseEventData data)
	{
		this.SetOn(!this.m_isOn, true);
	}

	public void SetOn(bool isOn, bool doNotify = false)
	{
		if (this.m_isOn == isOn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_ToggleSwap.SetOn(bool, bool)).MethodHandle;
			}
			return;
		}
		this.m_isOn = isOn;
		UIManager.SetGameObjectActive(this.m_onContainer.gameObject, isOn, null);
		if (this.m_offContainer != null)
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
			UIManager.SetGameObjectActive(this.m_offContainer.gameObject, !isOn, null);
		}
		if (doNotify)
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
			if (this.changedNotify != null)
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
				this.changedNotify(this);
			}
		}
	}

	public void SetClickable(bool canBeClicked)
	{
		this.m_onButton.SetClickable(canBeClicked);
		if (this.m_offButton != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(_ToggleSwap.SetClickable(bool)).MethodHandle;
			}
			this.m_offButton.SetClickable(canBeClicked);
		}
	}

	public delegate void NotifyChanged(_ToggleSwap btn);
}
