using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OverconSelectBtn : MonoBehaviour
{
	public Image m_sprite;

	public _SelectableBtn m_selectableBtn;

	public Image m_hoverImage;

	public Sprite m_activeHoverSprite;

	public Sprite m_inactiveHoverSprite;

	private const float kLockedAlpha = 25f / 128f;

	private UIOverconData.NameToOverconEntry m_overconRef;

	private bool m_isUnlocked;

	public bool IsUnlocked => m_isUnlocked;

	public UIOverconData.NameToOverconEntry GetOvercon()
	{
		return m_overconRef;
	}

	public void Setup(UIOverconData.NameToOverconEntry overcon, bool unlocked)
	{
		m_isUnlocked = unlocked;
		m_overconRef = overcon;
		m_sprite.sprite = Resources.Load<Sprite>(overcon.m_iconSpritePath);
		if (unlocked)
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
			m_selectableBtn.spriteController.callback = OverconClicked;
		}
		Color color = m_sprite.color;
		float a;
		if (unlocked)
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
			a = 1f;
		}
		else
		{
			a = 25f / 128f;
		}
		color.a = a;
		m_sprite.color = color;
		Image hoverImage = m_hoverImage;
		Sprite sprite;
		if (unlocked)
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
			sprite = m_activeHoverSprite;
		}
		else
		{
			sprite = m_inactiveHoverSprite;
		}
		hoverImage.sprite = sprite;
		m_selectableBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, SetupTooltip);
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		string text = StringUtil.TR("/overcon", "SlashCommand") + " " + m_overconRef.GetCommandName();
		string text2 = m_overconRef.GetObtainedDescription().Trim();
		if (!text2.IsNullOrEmpty())
		{
			text = text + Environment.NewLine + text2;
		}
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		uITitledTooltip.Setup(m_overconRef.GetDisplayName(), text, string.Empty);
		return true;
	}

	public void OverconClicked(BaseEventData data)
	{
		ActorData actorData = (!(GameFlowData.Get() != null)) ? null : GameFlowData.Get().activeOwnedActorData;
		if (!(actorData != null) || !(actorData.GetActorController() != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(HUD_UI.Get() != null))
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
				if (!(UIOverconData.Get() != null))
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
					if (ClientGameManager.Get() != null)
					{
						ClientGameManager.Get().SendUseOverconRequest(m_overconRef.m_overconId, m_overconRef.m_commandName, actorData.ActorIndex, GameFlowData.Get().CurrentTurn);
						HUD_UI.Get().m_textConsole.Hide();
						UIChatBox.Get().m_overconsPanel.SetPanelOpen(false);
					}
					return;
				}
			}
		}
	}
}
