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

	private const float kLockedAlpha = 0.1953125f;

	private UIOverconData.NameToOverconEntry m_overconRef;

	private bool m_isUnlocked;

	public bool IsUnlocked
	{
		get
		{
			return this.m_isUnlocked;
		}
	}

	public UIOverconData.NameToOverconEntry GetOvercon()
	{
		return this.m_overconRef;
	}

	public void Setup(UIOverconData.NameToOverconEntry overcon, bool unlocked)
	{
		this.m_isUnlocked = unlocked;
		this.m_overconRef = overcon;
		this.m_sprite.sprite = Resources.Load<Sprite>(overcon.m_iconSpritePath);
		if (unlocked)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(OverconSelectBtn.Setup(UIOverconData.NameToOverconEntry, bool)).MethodHandle;
			}
			this.m_selectableBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OverconClicked);
		}
		Color color = this.m_sprite.color;
		float a;
		if (unlocked)
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
			a = 1f;
		}
		else
		{
			a = 0.1953125f;
		}
		color.a = a;
		this.m_sprite.color = color;
		Image hoverImage = this.m_hoverImage;
		Sprite sprite;
		if (unlocked)
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
			sprite = this.m_activeHoverSprite;
		}
		else
		{
			sprite = this.m_inactiveHoverSprite;
		}
		hoverImage.sprite = sprite;
		this.m_selectableBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.SetupTooltip), null);
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		string text = StringUtil.TR("/overcon", "SlashCommand") + " " + this.m_overconRef.GetCommandName();
		string text2 = this.m_overconRef.GetObtainedDescription().Trim();
		if (!text2.IsNullOrEmpty())
		{
			text = text + Environment.NewLine + text2;
		}
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		uititledTooltip.Setup(this.m_overconRef.GetDisplayName(), text, string.Empty);
		return true;
	}

	public void OverconClicked(BaseEventData data)
	{
		ActorData actorData = (!(GameFlowData.Get() != null)) ? null : GameFlowData.Get().activeOwnedActorData;
		if (actorData != null && actorData.GetActorController() != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(OverconSelectBtn.OverconClicked(BaseEventData)).MethodHandle;
			}
			if (HUD_UI.Get() != null)
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
				if (UIOverconData.Get() != null)
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
					if (ClientGameManager.Get() != null)
					{
						ClientGameManager.Get().SendUseOverconRequest(this.m_overconRef.m_overconId, this.m_overconRef.m_commandName, actorData.ActorIndex, GameFlowData.Get().CurrentTurn);
						HUD_UI.Get().m_textConsole.Hide();
						UIChatBox.Get().m_overconsPanel.SetPanelOpen(false);
					}
				}
			}
		}
	}
}
