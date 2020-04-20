using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITextConsoleMenu : MonoBehaviour
{
	public TextMeshProUGUI m_playerName;

	public Color m_unhighlightedMenuItemColor;

	public UITextConsoleMenu.UITextConsoleTooltipBannerButton[] m_menuButtons;

	private string m_handle;

	private static UITextConsoleMenu s_instance;

	public static UITextConsoleMenu Get()
	{
		return UITextConsoleMenu.s_instance;
	}

	public void Awake()
	{
		UITextConsoleMenu.s_instance = this;
		this.SetVisible(false);
	}

	public void Start()
	{
		for (int i = 0; i < this.m_menuButtons.Length; i++)
		{
			UITextConsoleMenu.UITextConsoleButtonAction action = (UITextConsoleMenu.UITextConsoleButtonAction)i;
			if (this.IsValidButtonAction(action, true))
			{
				UIEventTriggerUtils.AddListener(this.m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnMenuMouseOver));
				UIEventTriggerUtils.AddListener(this.m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.OnMenuMouseExit));
				UIEventTriggerUtils.AddListener(this.m_menuButtons[i].m_button.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnMenuMouseClicked));
			}
		}
	}

	public bool IsValidButtonAction(UITextConsoleMenu.UITextConsoleButtonAction action, bool IsForSetup = false)
	{
		return action != UITextConsoleMenu.UITextConsoleButtonAction.ViewProfile;
	}

	public bool IsValidInGame(UITextConsoleMenu.UITextConsoleButtonAction action)
	{
		return action == UITextConsoleMenu.UITextConsoleButtonAction.BlockPlayer || action == UITextConsoleMenu.UITextConsoleButtonAction.ReportPlayer;
	}

	public void OnMenuMouseClicked(BaseEventData data)
	{
		if (this.m_handle.IsNullOrEmpty())
		{
			return;
		}
		for (int i = 0; i < this.m_menuButtons.Length; i++)
		{
			if (this.IsValidButtonAction((UITextConsoleMenu.UITextConsoleButtonAction)i, false))
			{
				if ((data as PointerEventData).pointerCurrentRaycast.gameObject == this.m_menuButtons[i].m_button.gameObject)
				{
					switch (i)
					{
					case 0:
						UIFrontEnd.Get().m_frontEndChatConsole.SelectInput("/whisper " + this.m_handle + " ");
						this.SetVisible(false);
						break;
					case 2:
						FriendListPanel.Get().RequestToAddFriend(this.m_handle);
						this.SetVisible(false);
						break;
					case 3:
						SlashCommands.Get().RunSlashCommand("/invite", this.m_handle);
						this.SetVisible(false);
						break;
					case 4:
					{
						string title = StringUtil.TR("BlockPlayer", "FriendList");
						string description = string.Format(StringUtil.TR("DoYouWantToBlock", "FriendList"), this.m_handle);
						string blockReportHandle = this.m_handle;
						UIDialogPopupManager.OpenTwoButtonDialog(title, description, StringUtil.TR("Yes", "Global"), StringUtil.TR("No", "Global"), delegate(UIDialogBox dialogReference)
						{
							SlashCommands.Get().RunSlashCommand("/block", blockReportHandle);
						}, null, false, false);
						this.SetVisible(false);
						break;
					}
					case 5:
						UILandingPageFullScreenMenus.Get().SetReportContainerVisible(true, this.m_handle, 0L, false);
						this.SetVisible(false);
						break;
					}
					return;
				}
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(base.gameObject, visible, null);
		if (!visible)
		{
			this.m_handle = string.Empty;
		}
	}

	public void OnMenuMouseOver(BaseEventData data)
	{
		for (int i = 0; i < this.m_menuButtons.Length; i++)
		{
			UITextConsoleMenu.UITextConsoleButtonAction action = (UITextConsoleMenu.UITextConsoleButtonAction)i;
			if (this.IsValidButtonAction(action, false))
			{
				if ((data as PointerEventData).pointerCurrentRaycast.gameObject == this.m_menuButtons[i].m_button.gameObject)
				{
					this.m_menuButtons[i].m_icon.color = Color.white;
					this.m_menuButtons[i].m_label.color = Color.white;
				}
				else
				{
					this.m_menuButtons[i].m_icon.color = this.m_unhighlightedMenuItemColor;
					this.m_menuButtons[i].m_label.color = this.m_unhighlightedMenuItemColor;
				}
			}
			else
			{
				this.m_menuButtons[i].m_icon.color = Color.gray;
				this.m_menuButtons[i].m_label.color = Color.gray;
			}
		}
	}

	public void OnMenuMouseExit(BaseEventData data)
	{
	}

	public void Setup(string clickedHandle, bool inGame)
	{
		this.m_handle = clickedHandle;
		this.m_playerName.text = this.m_handle;
		for (int i = 0; i < this.m_menuButtons.Length; i++)
		{
			UITextConsoleMenu.UITextConsoleButtonAction action = (UITextConsoleMenu.UITextConsoleButtonAction)i;
			if (this.IsValidButtonAction(action, false))
			{
				this.m_menuButtons[i].m_icon.color = this.m_unhighlightedMenuItemColor;
				this.m_menuButtons[i].m_label.color = this.m_unhighlightedMenuItemColor;
			}
			else
			{
				this.m_menuButtons[i].m_icon.color = Color.gray;
				this.m_menuButtons[i].m_label.color = Color.gray;
			}
			Component container = this.m_menuButtons[i].m_container;
			bool doActive;
			if (inGame)
			{
				doActive = this.IsValidInGame(action);
			}
			else
			{
				doActive = true;
			}
			UIManager.SetGameObjectActive(container, doActive, null);
		}
	}

	public void SetToMousePosition()
	{
		base.transform.position = base.GetComponentInParent<Canvas>().worldCamera.ScreenToWorldPoint(Input.mousePosition);
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = 0f;
		base.transform.localPosition = localPosition;
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (!(EventSystem.current.currentSelectedGameObject == null))
			{
				if (!(EventSystem.current.currentSelectedGameObject.GetComponentInParent<UITextConsoleMenu>() == null))
				{
					return;
				}
			}
			UITextConsoleMenu.Get().SetVisible(false);
		}
	}

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
}
