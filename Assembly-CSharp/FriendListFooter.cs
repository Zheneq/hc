using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FriendListFooter : MonoBehaviour
{
	public TMP_InputField m_inputField;

	public TextMeshProUGUI m_InviteText;

	public _ButtonSwapSprite m_inviteButton;

	public RectTransform m_invitePanel;

	public Color m_textColor;

	public _SelectableBtn m_InputFieldBtnHitBox;

	public Graphic[] m_placeHolders;

	private bool displayingPlaceholders;

	public void Start()
	{
		this.m_inviteButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.InviteClicked);
		this.displayingPlaceholders = true;
		for (int i = 0; i < this.m_placeHolders.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_placeHolders[i], this.displayingPlaceholders, null);
		}
	}

	public void InviteClicked(BaseEventData data)
	{
		FriendListPanel.Get().RequestToAddFriend(this.m_inputField.text);
		this.m_inputField.text = string.Empty;
	}

	public void Update()
	{
		if (EventSystem.current == null)
		{
			return;
		}
		bool flag = EventSystem.current.currentSelectedGameObject == this.m_inputField.gameObject;
		this.m_InputFieldBtnHitBox.SetSelected(flag, false, string.Empty, string.Empty);
		if (this.m_inputField.text == string.Empty)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FriendListFooter.Update()).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_invitePanel, false, null);
			for (int i = 0; i < this.m_placeHolders.Length; i++)
			{
				Component component = this.m_placeHolders[i];
				bool doActive;
				if (true)
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
					doActive = !flag;
				}
				else
				{
					doActive = false;
				}
				UIManager.SetGameObjectActive(component, doActive, null);
			}
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_invitePanel, true, null);
			for (int j = 0; j < this.m_placeHolders.Length; j++)
			{
				UIManager.SetGameObjectActive(this.m_placeHolders[j], false, null);
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_InviteText != null)
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
				this.m_InviteText.text = string.Format(StringUtil.TR("InviteFriendPrompt", "FriendList"), HUD_UIResources.ColorToHex(this.m_textColor), this.m_inputField.text);
			}
			if (flag)
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
				if (Input.GetKeyDown(KeyCode.Return))
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
					this.InviteClicked(null);
				}
			}
		}
	}
}
