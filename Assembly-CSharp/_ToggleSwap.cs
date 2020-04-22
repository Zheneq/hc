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
			while (true)
			{
				switch (4)
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
			switch (2)
			{
			case 0:
				continue;
			}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		m_isOn = isOn;
		UIManager.SetGameObjectActive(m_onContainer.gameObject, isOn);
		if (m_offContainer != null)
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
			UIManager.SetGameObjectActive(m_offContainer.gameObject, !isOn);
		}
		if (!doNotify)
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
			if (changedNotify != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
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
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_offButton.SetClickable(canBeClicked);
			return;
		}
	}
}
