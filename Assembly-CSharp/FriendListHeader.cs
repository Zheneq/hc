using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FriendListHeader : MonoBehaviour
{
	public _SelectableBtn m_statusDropdownBtn;

	public RectTransform m_statusDropdownMenuContainer;

	public _SelectableBtn[] m_statusSelectBtns;

	public _SelectableBtn m_closeBtn;

	public Image[] m_disabled;

	public Image m_statusColor;

	public TextMeshProUGUI[] m_statusLabels;

	public Image[] m_btnStatusColorDisplays;

	public Color[] m_friendListColors;

	private FriendListHeader.PlayerOnlineStatus m_currentStatus;

	private void Start()
	{
		UIManager.SetGameObjectActive(this.m_statusDropdownMenuContainer, false, null);
		this.m_statusDropdownBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.StatusDropDownClicked);
		this.m_statusDropdownBtn.SetSelected(false, true, string.Empty, string.Empty);
		for (int i = 0; i < this.m_disabled.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_disabled[i], false, null);
		}
		for (int j = 0; j < this.m_statusSelectBtns.Length; j++)
		{
			this.m_statusSelectBtns[j].spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.StatusButtonClicked);
			this.m_statusSelectBtns[j].SetSelected(false, false, string.Empty, string.Empty);
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListHeader.Start()).MethodHandle;
		}
		for (int k = 0; k < this.m_btnStatusColorDisplays.Length; k++)
		{
			this.m_btnStatusColorDisplays[k].color = this.m_friendListColors[k];
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		this.m_closeBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseBtnClicked);
		this.SetPlayerStatus(FriendListHeader.PlayerOnlineStatus.Online);
	}

	public void CloseBtnClicked(BaseEventData data)
	{
		if (FriendListPanel.Get().IsVisible())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListHeader.CloseBtnClicked(BaseEventData)).MethodHandle;
			}
			UIFrontEnd.Get().TogglePlayerFriendListVisibility();
		}
	}

	public void SetPlayerStatus(FriendListHeader.PlayerOnlineStatus status)
	{
		int num = (int)status;
		this.m_currentStatus = status;
		this.m_statusColor.color = this.m_friendListColors[num];
		for (int i = 0; i < this.m_statusLabels.Length; i++)
		{
			if (status != FriendListHeader.PlayerOnlineStatus.Online)
			{
				if (status != FriendListHeader.PlayerOnlineStatus.Busy)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListHeader.SetPlayerStatus(FriendListHeader.PlayerOnlineStatus)).MethodHandle;
					}
					if (status == FriendListHeader.PlayerOnlineStatus.Away)
					{
						this.m_statusLabels[i].text = StringUtil.TR("Away", "NewFrontEndScene");
					}
				}
				else
				{
					this.m_statusLabels[i].text = StringUtil.TR("Busy", "NewFrontEndScene");
				}
			}
			else
			{
				this.m_statusLabels[i].text = StringUtil.TR("Online", "NewFrontEndScene");
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager != null)
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
			if (clientGameManager.IsConnectedToLobbyServer)
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
				clientGameManager.UpdatePlayerStatus(status.ToString());
			}
		}
	}

	public void StatusButtonClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_statusDropdownMenuContainer, false, null);
		this.m_statusDropdownBtn.SetSelected(false, false, string.Empty, string.Empty);
		for (int i = 0; i < this.m_statusSelectBtns.Length; i++)
		{
			if (this.m_statusSelectBtns[i].spriteController.gameObject == (data as PointerEventData).selectedObject)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListHeader.StatusButtonClicked(BaseEventData)).MethodHandle;
				}
				if (i == 0)
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
					this.SetPlayerStatus(FriendListHeader.PlayerOnlineStatus.Online);
				}
				else if (i == 1)
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
					this.SetPlayerStatus(FriendListHeader.PlayerOnlineStatus.Busy);
				}
				else if (i == 2)
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
					this.SetPlayerStatus(FriendListHeader.PlayerOnlineStatus.Away);
				}
				return;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	public void StatusDropDownClicked(BaseEventData data)
	{
		bool flag = !this.m_statusDropdownMenuContainer.gameObject.activeSelf;
		UIManager.SetGameObjectActive(this.m_statusDropdownMenuContainer, flag, null);
		this.m_statusDropdownBtn.SetSelected(flag, false, string.Empty, string.Empty);
		if (flag)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListHeader.StatusDropDownClicked(BaseEventData)).MethodHandle;
			}
			for (int i = 0; i < this.m_statusSelectBtns.Length; i++)
			{
				this.m_statusSelectBtns[i].spriteController.ForceSetPointerEntered(false);
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (this.m_currentStatus == FriendListHeader.PlayerOnlineStatus.Online)
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
			this.m_statusSelectBtns[0].SetSelected(true, true, string.Empty, string.Empty);
			this.m_statusSelectBtns[1].SetSelected(false, true, string.Empty, string.Empty);
			this.m_statusSelectBtns[2].SetSelected(false, true, string.Empty, string.Empty);
		}
		else if (this.m_currentStatus == FriendListHeader.PlayerOnlineStatus.Busy)
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
			this.m_statusSelectBtns[0].SetSelected(false, true, string.Empty, string.Empty);
			this.m_statusSelectBtns[1].SetSelected(true, true, string.Empty, string.Empty);
			this.m_statusSelectBtns[2].SetSelected(false, true, string.Empty, string.Empty);
		}
		else if (this.m_currentStatus == FriendListHeader.PlayerOnlineStatus.Away)
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
			this.m_statusSelectBtns[0].SetSelected(false, true, string.Empty, string.Empty);
			this.m_statusSelectBtns[1].SetSelected(false, true, string.Empty, string.Empty);
			this.m_statusSelectBtns[2].SetSelected(true, true, string.Empty, string.Empty);
		}
		else
		{
			this.m_statusSelectBtns[0].SetSelected(false, true, string.Empty, string.Empty);
			this.m_statusSelectBtns[1].SetSelected(false, true, string.Empty, string.Empty);
			this.m_statusSelectBtns[2].SetSelected(false, true, string.Empty, string.Empty);
		}
	}

	public enum PlayerOnlineStatus
	{
		Online,
		Away,
		Busy
	}
}
