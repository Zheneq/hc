using UnityEngine;
using UnityEngine.EventSystems;

public class _ToggleSwap : MonoBehaviour
{
	public delegate void NotifyChanged(_ToggleSwap btn);

	public _ButtonSwapSprite m_onButton;

	public _ButtonSwapSprite m_offButton;

	public RectTransform m_onContainer;

	public RectTransform m_offContainer;

	public NotifyChanged changedNotify;

	private bool m_isOn;

	public bool IsChecked()
	{
		return m_isOn;
	}

	private void Awake()
	{
		if (m_onButton != null)
		{
			m_onButton.callback = ToggleButton;
		}
		if (m_offButton != null)
		{
			m_offButton.callback = ToggleButton;
		}
		if (m_onContainer != null)
		{
			UIManager.SetGameObjectActive(m_onContainer.gameObject, m_isOn);
		}
		if (!(m_offContainer != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_offContainer.gameObject, !m_isOn);
			return;
		}
	}

	public void ToggleButton(BaseEventData data)
	{
		SetOn(!m_isOn, true);
	}

	public void SetOn(bool isOn, bool doNotify = false)
	{
		if (m_isOn == isOn)
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
		m_isOn = isOn;
		UIManager.SetGameObjectActive(m_onContainer.gameObject, isOn);
		if (m_offContainer != null)
		{
			UIManager.SetGameObjectActive(m_offContainer.gameObject, !isOn);
		}
		if (!doNotify)
		{
			return;
		}
		while (true)
		{
			if (changedNotify != null)
			{
				while (true)
				{
					changedNotify(this);
					return;
				}
			}
			return;
		}
	}

	public void SetClickable(bool canBeClicked)
	{
		m_onButton.SetClickable(canBeClicked);
		if (!(m_offButton != null))
		{
			return;
		}
		while (true)
		{
			m_offButton.SetClickable(canBeClicked);
			return;
		}
	}
}
