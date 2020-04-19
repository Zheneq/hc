using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmoticonSelectBtn : MonoBehaviour
{
	public TextMeshProUGUI m_DefaultSprite;

	public TextMeshProUGUI m_HoverSprite;

	public _SelectableBtn m_theBtn;

	private ChatEmojiManager.ChatEmoji emojiRef;

	private int currentEmojiFrame;

	private float m_lastTimeUpdate;

	public ChatEmojiManager.ChatEmoji GetEmoji()
	{
		return this.emojiRef;
	}

	public void Setup(ChatEmojiManager.ChatEmoji emoji, bool unlocked)
	{
		this.emojiRef = emoji;
		this.m_DefaultSprite.text = string.Format("<sprite=\"EmoticonsAssets\" index={0}>", emoji.m_frameToDisplayForSelect);
		this.m_HoverSprite.text = string.Format("<sprite=\"EmoticonsAssets\" index={0}>", emoji.m_startIndex);
		this.currentEmojiFrame = emoji.m_startIndex;
		this.m_lastTimeUpdate = -1f;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(EmoticonSelectBtn.Setup(ChatEmojiManager.ChatEmoji, bool)).MethodHandle;
			}
			this.m_theBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ChatEmojiClicked);
		}
		this.m_theBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.ChatEmojiTooltip), null);
	}

	private bool ChatEmojiTooltip(UITooltipBase tooltip)
	{
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		string tooltipText = string.Format(StringUtil.TR("ChatEmojiTagTooltip", "ChatEmoji"), this.emojiRef.GetEmojiTag());
		uititledTooltip.Setup(this.emojiRef.GetEmojiName(), tooltipText, string.Empty);
		return true;
	}

	public void ChatEmojiClicked(BaseEventData data)
	{
		if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd && UIFrontEnd.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(EmoticonSelectBtn.ChatEmojiClicked(BaseEventData)).MethodHandle;
			}
			if (UIFrontEnd.Get().m_frontEndChatConsole != null)
			{
				UIFrontEnd.Get().m_frontEndChatConsole.AppendInput(this.emojiRef.GetEmojiTag() + " ", true);
				UIFrontEnd.Get().m_frontEndChatConsole.SelectInput(string.Empty);
				UIFrontEnd.Get().m_frontEndChatConsole.DehighlightTextAndPositionCarat();
				EmoticonPanel.Get().SetPanelOpen(false);
				return;
			}
		}
		if (UIManager.Get().CurrentState == UIManager.ClientState.InGame)
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
			if (HUD_UI.Get() != null)
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
				if (HUD_UI.Get().m_textConsole != null)
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
					HUD_UI.Get().m_textConsole.AppendInput(this.emojiRef.GetEmojiTag() + " ", true);
					EmoticonPanel.Get().SetPanelOpen(false);
				}
			}
		}
	}

	private void Update()
	{
		if (this.emojiRef != null)
		{
			if (this.m_theBtn.IsHover)
			{
				if (Time.time - this.m_lastTimeUpdate >= 1f / (float)this.emojiRef.m_framesPerSecond)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(EmoticonSelectBtn.Update()).MethodHandle;
					}
					this.m_lastTimeUpdate = Time.time;
					this.currentEmojiFrame++;
					if (this.currentEmojiFrame > this.emojiRef.m_endIndex)
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
						this.currentEmojiFrame = this.emojiRef.m_startIndex;
					}
					this.m_DefaultSprite.text = string.Format("<sprite=\"EmoticonsAssets\" index={0}>", this.emojiRef.m_frameToDisplayForSelect);
					this.m_HoverSprite.text = string.Format("<sprite=\"EmoticonsAssets\" index={0}>", this.currentEmojiFrame);
				}
			}
			else
			{
				this.currentEmojiFrame = this.emojiRef.m_startIndex;
			}
		}
	}
}
