using System;
using System.Text;
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
			if ((data as PointerEventData).pointerCurrentRaycast.gameObject == m_menuButtons[i].m_button.gameObject)
			{
				switch (i)
				{
				case 1:
					break;
				case 0:
					UIFrontEnd.Get().m_frontEndChatConsole.SelectInput(new StringBuilder().Append("/whisper ").Append(m_handle).Append(" ").ToString());
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
				if ((data as PointerEventData).pointerCurrentRaycast.gameObject == m_menuButtons[i].m_button.gameObject)
				{
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
			if (!(EventSystem.current.currentSelectedGameObject.GetComponentInParent<UITextConsoleMenu>() == null))
			{
				return;
			}
		}
		Get().SetVisible(false);
	}
}
