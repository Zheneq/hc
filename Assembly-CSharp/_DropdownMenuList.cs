using UnityEngine;
using UnityEngine.EventSystems;

public class _DropdownMenuList : MonoBehaviour
{
	public delegate void CallBack();

	public RectTransform m_listContainer;

	public _ButtonSwapSprite m_listToggleBtn;

	[HideInInspector]
	public CallBack m_openCallback;

	[HideInInspector]
	public CallBack m_closeCallback;

	private bool m_listVisible;

	private void Awake()
	{
		m_listVisible = false;
		UIManager.SetGameObjectActive(m_listContainer, m_listVisible);
		m_listToggleBtn.callback = ToggleListContainer;
	}

	public void SetListContainerVisible(bool visible)
	{
		m_listVisible = visible;
		UIManager.SetGameObjectActive(m_listContainer, m_listVisible);
		if (m_listVisible && m_openCallback != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_openCallback();
					return;
				}
			}
		}
		if (m_listVisible || m_closeCallback == null)
		{
			return;
		}
		while (true)
		{
			m_closeCallback();
			return;
		}
	}

	public void ToggleListContainer(BaseEventData data)
	{
		SetListContainerVisible(!m_listVisible);
	}
}
