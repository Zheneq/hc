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
		m_inviteButton.callback = InviteClicked;
		displayingPlaceholders = true;
		for (int i = 0; i < m_placeHolders.Length; i++)
		{
			UIManager.SetGameObjectActive(m_placeHolders[i], displayingPlaceholders);
		}
	}

	public void InviteClicked(BaseEventData data)
	{
		FriendListPanel.Get().RequestToAddFriend(m_inputField.text);
		m_inputField.text = string.Empty;
	}

	public void Update()
	{
		if (EventSystem.current == null)
		{
			return;
		}
		bool flag = EventSystem.current.currentSelectedGameObject == m_inputField.gameObject;
		m_InputFieldBtnHitBox.SetSelected(flag, false, string.Empty, string.Empty);
		if (m_inputField.text == string.Empty)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					UIManager.SetGameObjectActive(m_invitePanel, false);
					for (int i = 0; i < m_placeHolders.Length; i++)
					{
						Graphic component = m_placeHolders[i];
						int doActive;
						if (true)
						{
							doActive = ((!flag) ? 1 : 0);
						}
						else
						{
							doActive = 0;
						}
						UIManager.SetGameObjectActive(component, (byte)doActive != 0);
					}
					return;
				}
				}
			}
		}
		UIManager.SetGameObjectActive(m_invitePanel, true);
		for (int j = 0; j < m_placeHolders.Length; j++)
		{
			UIManager.SetGameObjectActive(m_placeHolders[j], false);
		}
		while (true)
		{
			if (m_InviteText != null)
			{
				m_InviteText.text = string.Format(StringUtil.TR("InviteFriendPrompt", "FriendList"), HUD_UIResources.ColorToHex(m_textColor), m_inputField.text);
			}
			if (!flag)
			{
				return;
			}
			while (true)
			{
				if (Input.GetKeyDown(KeyCode.Return))
				{
					while (true)
					{
						InviteClicked(null);
						return;
					}
				}
				return;
			}
		}
	}
}
