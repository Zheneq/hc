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
		return emojiRef;
	}

	public void Setup(ChatEmojiManager.ChatEmoji emoji, bool unlocked)
	{
		emojiRef = emoji;
		m_DefaultSprite.text = $"<sprite=\"EmoticonsAssets\" index={emoji.m_frameToDisplayForSelect}>";
		m_HoverSprite.text = $"<sprite=\"EmoticonsAssets\" index={emoji.m_startIndex}>";
		currentEmojiFrame = emoji.m_startIndex;
		m_lastTimeUpdate = -1f;
		if (unlocked)
		{
			m_theBtn.spriteController.callback = ChatEmojiClicked;
		}
		m_theBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, ChatEmojiTooltip);
	}

	private bool ChatEmojiTooltip(UITooltipBase tooltip)
	{
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		string tooltipText = string.Format(StringUtil.TR("ChatEmojiTagTooltip", "ChatEmoji"), emojiRef.GetEmojiTag());
		uITitledTooltip.Setup(emojiRef.GetEmojiName(), tooltipText, string.Empty);
		return true;
	}

	public void ChatEmojiClicked(BaseEventData data)
	{
		if (UIManager.Get().CurrentState == UIManager.ClientState.InFrontEnd && UIFrontEnd.Get() != null)
		{
			if (UIFrontEnd.Get().m_frontEndChatConsole != null)
			{
				UIFrontEnd.Get().m_frontEndChatConsole.AppendInput(emojiRef.GetEmojiTag() + " ", true);
				UIFrontEnd.Get().m_frontEndChatConsole.SelectInput(string.Empty);
				UIFrontEnd.Get().m_frontEndChatConsole.DehighlightTextAndPositionCarat();
				EmoticonPanel.Get().SetPanelOpen(false);
				return;
			}
		}
		if (UIManager.Get().CurrentState != UIManager.ClientState.InGame)
		{
			return;
		}
		while (true)
		{
			if (!(HUD_UI.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (HUD_UI.Get().m_textConsole != null)
				{
					while (true)
					{
						HUD_UI.Get().m_textConsole.AppendInput(emojiRef.GetEmojiTag() + " ", true);
						EmoticonPanel.Get().SetPanelOpen(false);
						return;
					}
				}
				return;
			}
		}
	}

	private void Update()
	{
		if (emojiRef == null)
		{
			return;
		}
		if (m_theBtn.IsHover)
		{
			if (!(Time.time - m_lastTimeUpdate >= 1f / (float)emojiRef.m_framesPerSecond))
			{
				return;
			}
			while (true)
			{
				m_lastTimeUpdate = Time.time;
				currentEmojiFrame++;
				if (currentEmojiFrame > emojiRef.m_endIndex)
				{
					currentEmojiFrame = emojiRef.m_startIndex;
				}
				m_DefaultSprite.text = $"<sprite=\"EmoticonsAssets\" index={emojiRef.m_frameToDisplayForSelect}>";
				m_HoverSprite.text = $"<sprite=\"EmoticonsAssets\" index={currentEmojiFrame}>";
				return;
			}
		}
		currentEmojiFrame = emojiRef.m_startIndex;
	}
}
