using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatEmojiManager : MonoBehaviour
{
	public List<ChatEmojiManager.ChatEmoji> m_emojiList = new List<ChatEmojiManager.ChatEmoji>();

	public int m_displayTextSize = 0x24;

	public TMP_SpriteAsset m_chatEmojiAsset;

	private static ChatEmojiManager s_instance;

	private List<ChatEmojiManager.EmojiDisplayInfo> m_emojiTextFields = new List<ChatEmojiManager.EmojiDisplayInfo>();

	public static ChatEmojiManager Get()
	{
		return ChatEmojiManager.s_instance;
	}

	public void AddNewEmoji(TextMeshProUGUI textfield, List<int> allowedEmojis = null)
	{
		if (textfield != null)
		{
			if (this.ContainsEmoji(textfield, allowedEmojis))
			{
				ChatEmojiManager.EmojiDisplayInfo item = new ChatEmojiManager.EmojiDisplayInfo(textfield, allowedEmojis);
				this.m_emojiTextFields.Add(item);
			}
		}
	}

	private bool ContainsEmoji(TextMeshProUGUI textField, List<int> allowedEmojis = null)
	{
		using (List<ChatEmojiManager.ChatEmoji>.Enumerator enumerator = this.m_emojiList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ChatEmojiManager.ChatEmoji chatEmoji = enumerator.Current;
				if (textField.text.Contains(chatEmoji.m_emojiTag))
				{
					if (allowedEmojis != null)
					{
						for (int i = 0; i < allowedEmojis.Count; i++)
						{
							string unlocalizedChatEmojiName = GameBalanceVars.Get().GetUnlocalizedChatEmojiName(allowedEmojis[i], string.Empty);
							if (unlocalizedChatEmojiName == chatEmoji.m_emojiName)
							{
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	public List<int> GetAllEmojisIndicesInString(string text)
	{
		List<int> list = new List<int>();
		using (List<ChatEmojiManager.ChatEmoji>.Enumerator enumerator = this.m_emojiList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ChatEmojiManager.ChatEmoji chatEmoji = enumerator.Current;
				if (text.Contains(chatEmoji.m_emojiTag))
				{
					int chatEmojiIndexByName = GameBalanceVars.Get().GetChatEmojiIndexByName(chatEmoji.m_emojiName);
					list.Add(chatEmojiIndexByName);
				}
			}
		}
		return list;
	}

	public List<int> GetAllEmojisInString(string text)
	{
		List<int> list = new List<int>();
		using (List<ChatEmojiManager.ChatEmoji>.Enumerator enumerator = this.m_emojiList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ChatEmojiManager.ChatEmoji chatEmoji = enumerator.Current;
				if (text.Contains(chatEmoji.m_emojiTag))
				{
					int chatEmojiIDByName = GameBalanceVars.Get().GetChatEmojiIDByName(chatEmoji.m_emojiName);
					list.Add(chatEmojiIDByName);
				}
			}
		}
		return list;
	}

	public string UnlocalizeEmojis(string theString)
	{
		string text = theString;
		using (List<ChatEmojiManager.ChatEmoji>.Enumerator enumerator = this.m_emojiList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ChatEmojiManager.ChatEmoji chatEmoji = enumerator.Current;
				string emojiTag = chatEmoji.GetEmojiTag();
				if (theString.Contains(emojiTag))
				{
					if (!emojiTag.IsNullOrEmpty())
					{
						text = text.Replace(emojiTag, chatEmoji.m_emojiTag);
					}
				}
			}
		}
		return text;
	}

	private void Update()
	{
		for (int i = 0; i < this.m_emojiTextFields.Count; i++)
		{
			ChatEmojiManager.EmojiDisplayInfo emojiDisplayInfo = this.m_emojiTextFields[i];
			if (emojiDisplayInfo.m_textField != null)
			{
				emojiDisplayInfo.Update();
			}
			else
			{
				this.m_emojiTextFields.Remove(emojiDisplayInfo);
				i--;
			}
		}
	}

	private void Start()
	{
		ChatEmojiManager.s_instance = this;
	}

	[Serializable]
	public class ChatEmoji
	{
		public string m_emojiName = "New Emoji";

		public string m_emojiTag = ":newtag:";

		public string m_howToUnlock = "...";

		public int m_frameToDisplayForSelect;

		public int m_startIndex;

		public int m_endIndex;

		public int m_framesPerSecond;

		public ChatEmoji()
		{
			this.m_framesPerSecond = 0xC;
		}

		public string GetEmojiName()
		{
			int chatEmojiIDByName = GameBalanceVars.Get().GetChatEmojiIDByName(this.m_emojiName);
			return StringUtil.TR_EmojiName(chatEmojiIDByName);
		}

		public string GetEmojiTag()
		{
			int chatEmojiIDByName = GameBalanceVars.Get().GetChatEmojiIDByName(this.m_emojiName);
			return StringUtil.TR_EmojiTag(chatEmojiIDByName);
		}

		public string GetHowToUnlock()
		{
			int chatEmojiIDByName = GameBalanceVars.Get().GetChatEmojiIDByName(this.m_emojiName);
			return StringUtil.TR_EmojiUnlock(chatEmojiIDByName);
		}
	}

	public class EmojiDisplayInfo
	{
		public List<ChatEmojiManager.EmojiDisplayInfo.PerEmojiDisplayInfo> m_emojiDisplayList = new List<ChatEmojiManager.EmojiDisplayInfo.PerEmojiDisplayInfo>();

		public TextMeshProUGUI m_textField;

		public EmojiDisplayInfo(TextMeshProUGUI textfield, List<int> allowedEmojis)
		{
			this.m_textField = textfield;
			using (List<ChatEmojiManager.ChatEmoji>.Enumerator enumerator = ChatEmojiManager.Get().m_emojiList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ChatEmojiManager.ChatEmoji chatEmoji = enumerator.Current;
					if (this.m_textField.text.Contains(chatEmoji.m_emojiTag))
					{
						if (allowedEmojis != null)
						{
							for (int i = 0; i < allowedEmojis.Count; i++)
							{
								string unlocalizedChatEmojiName = GameBalanceVars.Get().GetUnlocalizedChatEmojiName(allowedEmojis[i], string.Empty);
								if (unlocalizedChatEmojiName == chatEmoji.m_emojiName)
								{
									ChatEmojiManager.EmojiDisplayInfo.PerEmojiDisplayInfo perEmojiDisplayInfo = new ChatEmojiManager.EmojiDisplayInfo.PerEmojiDisplayInfo();
									this.m_textField.text = this.m_textField.text.Replace(chatEmoji.m_emojiTag, string.Concat(new object[]
									{
										"<size=",
										ChatEmojiManager.Get().m_displayTextSize,
										"><link=emoji-",
										chatEmoji.m_emojiTag,
										"><sprite=\"EmoticonsAssets\" index=",
										chatEmoji.m_startIndex,
										"></link>​</size>"
									}));
									perEmojiDisplayInfo.m_refInfo = chatEmoji;
									perEmojiDisplayInfo.m_currentIndex = chatEmoji.m_startIndex;
									perEmojiDisplayInfo.m_lastTimeUpdate = Time.time;
									this.m_emojiDisplayList.Add(perEmojiDisplayInfo);
								}
							}
						}
					}
				}
			}
		}

		public void Update()
		{
			using (List<ChatEmojiManager.EmojiDisplayInfo.PerEmojiDisplayInfo>.Enumerator enumerator = this.m_emojiDisplayList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ChatEmojiManager.EmojiDisplayInfo.PerEmojiDisplayInfo perEmojiDisplayInfo = enumerator.Current;
					if (Time.time - perEmojiDisplayInfo.m_lastTimeUpdate >= 1f / (float)perEmojiDisplayInfo.m_refInfo.m_framesPerSecond)
					{
						if (perEmojiDisplayInfo.m_currentIndex < perEmojiDisplayInfo.m_refInfo.m_endIndex)
						{
							this.m_textField.text = this.m_textField.text.Replace("<sprite=\"EmoticonsAssets\" index=" + perEmojiDisplayInfo.m_currentIndex + ">", "<sprite=\"EmoticonsAssets\" index=" + (perEmojiDisplayInfo.m_currentIndex + 1) + ">");
							perEmojiDisplayInfo.m_currentIndex++;
						}
						else
						{
							this.m_textField.text = this.m_textField.text.Replace("<sprite=\"EmoticonsAssets\" index=" + perEmojiDisplayInfo.m_currentIndex + ">", "<sprite=\"EmoticonsAssets\" index=" + perEmojiDisplayInfo.m_refInfo.m_startIndex + ">");
							perEmojiDisplayInfo.m_currentIndex = perEmojiDisplayInfo.m_refInfo.m_startIndex;
						}
						perEmojiDisplayInfo.m_lastTimeUpdate = Time.time;
					}
				}
			}
		}

		public class PerEmojiDisplayInfo
		{
			public ChatEmojiManager.ChatEmoji m_refInfo;

			public int m_currentIndex;

			public float m_lastTimeUpdate;
		}
	}
}
