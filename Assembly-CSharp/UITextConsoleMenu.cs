using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITextConsoleMenu : MonoBehaviour
{
	public enum UITextConsoleButtonAction
	{
		SendMessage,
		ViewProfile,
		AddToFriends,
		InviteToParty,
		BlockPlayer,
		ReportPlayer
	}

	[Serializable]
	public struct UITextConsoleTooltipBannerButton
	{
		public RectTransform m_container;

		public Image m_icon;

		public TextMeshProUGUI m_label;

		public Button m_button;
	}

	public TextMeshProUGUI m_playerName;

	public Color m_unhighlightedMenuItemColor;

	public UITextConsoleTooltipBannerButton[] m_menuButtons;

	private string m_handle;

	private static UITextConsoleMenu s_instance;

	public static UITextConsoleMenu Get()
	{
		return s_instance;
	}

	public void Awake()
	{
		s_instance = this;
		SetVisible(false);
	}

	public void Start()
	{
		for (int i = 0; i < m_menuButtons.Length; i++)
		{
			UITextConsoleButtonAction action = (UITextConsoleButtonAction)i;
			if (IsValidButtonAction(action, true))
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
				UIEventTriggerUtils.AddListener(m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerEnter, OnMenuMouseOver);
				UIEventTriggerUtils.AddListener(m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerExit, OnMenuMouseExit);
				UIEventTriggerUtils.AddListener(m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerClick, OnMenuMouseClicked);
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public bool IsValidButtonAction(UITextConsoleButtonAction action, bool IsForSetup = false)
	{
		if (action == UITextConsoleButtonAction.ViewProfile)
		{
			return false;
		}
		return true;
	}

	public bool IsValidInGame(UITextConsoleButtonAction action)
	{
		return action == UITextConsoleButtonAction.BlockPlayer || action == UITextConsoleButtonAction.ReportPlayer;
	}

	public void OnMenuMouseClicked(BaseEventData data)
	{
		if (m_handle.IsNullOrEmpty())
		{
			return;
		}
		string blockReportHandle;
		for (int i = 0; i < m_menuButtons.Length; i++)
		{
			if (!IsValidButtonAction((UITextConsoleButtonAction)i))
			{
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
			if ((data as PointerEventData).pointerCurrentRaycast.gameObject == m_menuButtons[i].m_button.gameObject)
			{
				switch (i)
				{
				case 1:
					break;
				case 0:
					UIFrontEnd.Get().m_frontEndChatConsole.SelectInput("/whisper " + m_handle + " ");
					SetVisible(false);
					break;
				case 3:
					SlashCommands.Get().RunSlashCommand("/invite", m_handle);
					SetVisible(false);
					break;
				case 2:
					FriendListPanel.Get().RequestToAddFriend(m_handle);
					SetVisible(false);
					break;
				case 4:
				{
					string title = StringUtil.TR("BlockPlayer", "FriendList");
					string description = string.Format(StringUtil.TR("DoYouWantToBlock", "FriendList"), m_handle);
					blockReportHandle = m_handle;
					UIDialogPopupManager.OpenTwoButtonDialog(title, description, StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate
					{
						SlashCommands.Get().RunSlashCommand("/block", blockReportHandle);
					});
					SetVisible(false);
					break;
				}
				case 5:
					UILandingPageFullScreenMenus.Get().SetReportContainerVisible(true, m_handle, 0L);
					SetVisible(false);
					break;
				}
				return;
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(base.gameObject, visible);
		if (visible)
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
			m_handle = string.Empty;
			return;
		}
	}

	public void OnMenuMouseOver(BaseEventData data)
	{
		for (int i = 0; i < m_menuButtons.Length; i++)
		{
			UITextConsoleButtonAction action = (UITextConsoleButtonAction)i;
			if (IsValidButtonAction(action))
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
				if ((data as PointerEventData).pointerCurrentRaycast.gameObject == m_menuButtons[i].m_button.gameObject)
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
					m_menuButtons[i].m_icon.color = Color.white;
					m_menuButtons[i].m_label.color = Color.white;
				}
				else
				{
					m_menuButtons[i].m_icon.color = m_unhighlightedMenuItemColor;
					m_menuButtons[i].m_label.color = m_unhighlightedMenuItemColor;
				}
			}
			else
			{
				m_menuButtons[i].m_icon.color = Color.gray;
				m_menuButtons[i].m_label.color = Color.gray;
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void OnMenuMouseExit(BaseEventData data)
	{
	}

	public void Setup(string clickedHandle, bool inGame)
	{
		m_handle = clickedHandle;
		m_playerName.text = m_handle;
		for (int i = 0; i < m_menuButtons.Length; i++)
		{
			UITextConsoleButtonAction action = (UITextConsoleButtonAction)i;
			if (IsValidButtonAction(action))
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
				m_menuButtons[i].m_icon.color = m_unhighlightedMenuItemColor;
				m_menuButtons[i].m_label.color = m_unhighlightedMenuItemColor;
			}
			else
			{
				m_menuButtons[i].m_icon.color = Color.gray;
				m_menuButtons[i].m_label.color = Color.gray;
			}
			RectTransform container = m_menuButtons[i].m_container;
			int doActive;
			if (inGame)
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
				doActive = (IsValidInGame(action) ? 1 : 0);
			}
			else
			{
				doActive = 1;
			}
			UIManager.SetGameObjectActive(container, (byte)doActive != 0);
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void SetToMousePosition()
	{
		base.transform.position = GetComponentInParent<Canvas>().worldCamera.ScreenToWorldPoint(Input.mousePosition);
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = 0f;
		base.transform.localPosition = localPosition;
	}

	public void Update()
	{
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}
		if (!(EventSystem.current.currentSelectedGameObject == null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(EventSystem.current.currentSelectedGameObject.GetComponentInParent<UITextConsoleMenu>() == null))
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
				break;
			}
		}
		Get().SetVisible(false);
	}
}
