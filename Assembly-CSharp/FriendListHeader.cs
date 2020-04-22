using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FriendListHeader : MonoBehaviour
{
	public enum PlayerOnlineStatus
	{
		Online,
		Away,
		Busy
	}

	public _SelectableBtn m_statusDropdownBtn;

	public RectTransform m_statusDropdownMenuContainer;

	public _SelectableBtn[] m_statusSelectBtns;

	public _SelectableBtn m_closeBtn;

	public Image[] m_disabled;

	public Image m_statusColor;

	public TextMeshProUGUI[] m_statusLabels;

	public Image[] m_btnStatusColorDisplays;

	public Color[] m_friendListColors;

	private PlayerOnlineStatus m_currentStatus;

	private void Start()
	{
		UIManager.SetGameObjectActive(m_statusDropdownMenuContainer, false);
		m_statusDropdownBtn.spriteController.callback = StatusDropDownClicked;
		m_statusDropdownBtn.SetSelected(false, true, string.Empty, string.Empty);
		for (int i = 0; i < m_disabled.Length; i++)
		{
			UIManager.SetGameObjectActive(m_disabled[i], false);
		}
		for (int j = 0; j < m_statusSelectBtns.Length; j++)
		{
			m_statusSelectBtns[j].spriteController.callback = StatusButtonClicked;
			m_statusSelectBtns[j].SetSelected(false, false, string.Empty, string.Empty);
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int k = 0; k < m_btnStatusColorDisplays.Length; k++)
			{
				m_btnStatusColorDisplays[k].color = m_friendListColors[k];
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				m_closeBtn.spriteController.callback = CloseBtnClicked;
				SetPlayerStatus(PlayerOnlineStatus.Online);
				return;
			}
		}
	}

	public void CloseBtnClicked(BaseEventData data)
	{
		if (!FriendListPanel.Get().IsVisible())
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIFrontEnd.Get().TogglePlayerFriendListVisibility();
			return;
		}
	}

	public void SetPlayerStatus(PlayerOnlineStatus status)
	{
		int num = (int)status;
		m_currentStatus = status;
		m_statusColor.color = m_friendListColors[num];
		for (int i = 0; i < m_statusLabels.Length; i++)
		{
			switch (status)
			{
			case PlayerOnlineStatus.Online:
				m_statusLabels[i].text = StringUtil.TR("Online", "NewFrontEndScene");
				continue;
			case PlayerOnlineStatus.Busy:
				m_statusLabels[i].text = StringUtil.TR("Busy", "NewFrontEndScene");
				continue;
			}
			while (true)
			{
				switch (7)
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
			if (status == PlayerOnlineStatus.Away)
			{
				m_statusLabels[i].text = StringUtil.TR("Away", "NewFrontEndScene");
			}
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			ClientGameManager clientGameManager = ClientGameManager.Get();
			if (!(clientGameManager != null))
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
				if (clientGameManager.IsConnectedToLobbyServer)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						clientGameManager.UpdatePlayerStatus(status.ToString());
						return;
					}
				}
				return;
			}
		}
	}

	public void StatusButtonClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_statusDropdownMenuContainer, false);
		m_statusDropdownBtn.SetSelected(false, false, string.Empty, string.Empty);
		for (int i = 0; i < m_statusSelectBtns.Length; i++)
		{
			if (!(m_statusSelectBtns[i].spriteController.gameObject == (data as PointerEventData).selectedObject))
			{
				continue;
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
				if (i == 0)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							SetPlayerStatus(PlayerOnlineStatus.Online);
							return;
						}
					}
				}
				if (i == 1)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							SetPlayerStatus(PlayerOnlineStatus.Busy);
							return;
						}
					}
				}
				if (i == 2)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						SetPlayerStatus(PlayerOnlineStatus.Away);
						return;
					}
				}
				return;
			}
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void StatusDropDownClicked(BaseEventData data)
	{
		bool flag = !m_statusDropdownMenuContainer.gameObject.activeSelf;
		UIManager.SetGameObjectActive(m_statusDropdownMenuContainer, flag);
		m_statusDropdownBtn.SetSelected(flag, false, string.Empty, string.Empty);
		if (flag)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < m_statusSelectBtns.Length; i++)
			{
				m_statusSelectBtns[i].spriteController.ForceSetPointerEntered(false);
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (m_currentStatus == PlayerOnlineStatus.Online)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_statusSelectBtns[0].SetSelected(true, true, string.Empty, string.Empty);
					m_statusSelectBtns[1].SetSelected(false, true, string.Empty, string.Empty);
					m_statusSelectBtns[2].SetSelected(false, true, string.Empty, string.Empty);
					return;
				}
			}
		}
		if (m_currentStatus == PlayerOnlineStatus.Busy)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_statusSelectBtns[0].SetSelected(false, true, string.Empty, string.Empty);
					m_statusSelectBtns[1].SetSelected(true, true, string.Empty, string.Empty);
					m_statusSelectBtns[2].SetSelected(false, true, string.Empty, string.Empty);
					return;
				}
			}
		}
		if (m_currentStatus == PlayerOnlineStatus.Away)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_statusSelectBtns[0].SetSelected(false, true, string.Empty, string.Empty);
					m_statusSelectBtns[1].SetSelected(false, true, string.Empty, string.Empty);
					m_statusSelectBtns[2].SetSelected(true, true, string.Empty, string.Empty);
					return;
				}
			}
		}
		m_statusSelectBtns[0].SetSelected(false, true, string.Empty, string.Empty);
		m_statusSelectBtns[1].SetSelected(false, true, string.Empty, string.Empty);
		m_statusSelectBtns[2].SetSelected(false, true, string.Empty, string.Empty);
	}
}
