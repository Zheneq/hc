using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class _DropdownMenuList : MonoBehaviour
{
	public RectTransform m_listContainer;

	public _ButtonSwapSprite m_listToggleBtn;

	[HideInInspector]
	public _DropdownMenuList.CallBack m_openCallback;

	[HideInInspector]
	public _DropdownMenuList.CallBack m_closeCallback;

	private bool m_listVisible;

	private void Awake()
	{
		this.m_listVisible = false;
		UIManager.SetGameObjectActive(this.m_listContainer, this.m_listVisible, null);
		this.m_listToggleBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ToggleListContainer);
	}

	public void SetListContainerVisible(bool visible)
	{
		this.m_listVisible = visible;
		UIManager.SetGameObjectActive(this.m_listContainer, this.m_listVisible, null);
		if (this.m_listVisible && this.m_openCallback != null)
		{
			this.m_openCallback();
		}
		else if (!this.m_listVisible && this.m_closeCallback != null)
		{
			this.m_closeCallback();
		}
	}

	public void ToggleListContainer(BaseEventData data)
	{
		this.SetListContainerVisible(!this.m_listVisible);
	}

	public delegate void CallBack();
}
