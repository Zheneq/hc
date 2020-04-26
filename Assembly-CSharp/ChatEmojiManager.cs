using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatEmojiManager : MonoBehaviour
{
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
			m_framesPerSecond = 12;
		}

		public string GetEmojiName()
		{
			int chatEmojiIDByName = GameBalanceVars.Get().GetChatEmojiIDByName(m_emojiName);
			return StringUtil.TR_EmojiName(chatEmojiIDByName);
		}

		public string GetEmojiTag()
		{
			int chatEmojiIDByName = GameBalanceVars.Get().GetChatEmojiIDByName(m_emojiName);
			return StringUtil.TR_EmojiTag(chatEmojiIDByName);
		}

		public string GetHowToUnlock()
		{
			int chatEmojiIDByName = GameBalanceVars.Get().GetChatEmojiIDByName(m_emojiName);
			return StringUtil.TR_EmojiUnlock(chatEmojiIDByName);
		}
	}

	public class EmojiDisplayInfo
	{
		public class PerEmojiDisplayInfo
		{
			public ChatEmoji m_refInfo;

			public int m_currentIndex;

			public float m_lastTimeUpdate;
		}

		public List<PerEmojiDisplayInfo> m_emojiDisplayList = new List<PerEmojiDisplayInfo>();

		public TextMeshProUGUI m_textField;

		public EmojiDisplayInfo(TextMeshProUGUI textfield, List<int> allowedEmojis)
		{
			m_textField = textfield;
			using (List<ChatEmoji>.Enumerator enumerator = Get().m_emojiList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ChatEmoji current = enumerator.Current;
					if (m_textField.text.Contains(current.m_emojiTag))
					{
						if (allowedEmojis != null)
						{
							for (int i = 0; i < allowedEmojis.Count; i++)
							{
								string unlocalizedChatEmojiName = GameBalanceVars.Get().GetUnlocalizedChatEmojiName(allowedEmojis[i], string.Empty);
								if (unlocalizedChatEmojiName == current.m_emojiName)
								{
									PerEmojiDisplayInfo perEmojiDisplayInfo = new PerEmojiDisplayInfo();
									m_textField.text = m_textField.text.Replace(current.m_emojiTag, "<size=" + Get().m_displayTextSize + "><link=emoji-" + current.m_emojiTag + "><sprite=\"EmoticonsAssets\" index=" + current.m_startIndex + "></link>\u200b</size>");
									perEmojiDisplayInfo.m_refInfo = current;
									perEmojiDisplayInfo.m_currentIndex = current.m_startIndex;
									perEmojiDisplayInfo.m_lastTimeUpdate = Time.time;
									m_emojiDisplayList.Add(perEmojiDisplayInfo);
								}
							}
						}
					}
				}
				while (true)
				{
					switch (2)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}

		public void Update()
		{
			using (List<PerEmojiDisplayInfo>.Enumerator enumerator = m_emojiDisplayList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PerEmojiDisplayInfo current = enumerator.Current;
					if (Time.time - current.m_lastTimeUpdate >= 1f / (float)current.m_refInfo.m_framesPerSecond)
					{
						if (current.m_currentIndex < current.m_refInfo.m_endIndex)
						{
							m_textField.text = m_textField.text.Replace("<sprite=\"EmoticonsAssets\" index=" + current.m_currentIndex + ">", "<sprite=\"EmoticonsAssets\" index=" + (current.m_currentIndex + 1) + ">");
							current.m_currentIndex++;
						}
						else
						{
							m_textField.text = m_textField.text.Replace("<sprite=\"EmoticonsAssets\" index=" + current.m_currentIndex + ">", "<sprite=\"EmoticonsAssets\" index=" + current.m_refInfo.m_startIndex + ">");
							current.m_currentIndex = current.m_refInfo.m_startIndex;
						}
						current.m_lastTimeUpdate = Time.time;
					}
				}
				while (true)
				{
					switch (7)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	public List<ChatEmoji> m_emojiList = new List<ChatEmoji>();

	public int m_displayTextSize = 36;

	public TMP_SpriteAsset m_chatEmojiAsset;

	private static ChatEmojiManager s_instance;

	private List<EmojiDisplayInfo> m_emojiTextFields = new List<EmojiDisplayInfo>();

	public static ChatEmojiManager Get()
	{
		return s_instance;
	}

	public void AddNewEmoji(TextMeshProUGUI textfield, List<int> allowedEmojis = null)
	{
		if (!(textfield != null))
		{
			return;
		}
		while (true)
		{
			if (ContainsEmoji(textfield, allowedEmojis))
			{
				EmojiDisplayInfo item = new EmojiDisplayInfo(textfield, allowedEmojis);
				m_emojiTextFields.Add(item);
			}
			return;
		}
	}

	private bool ContainsEmoji(TextMeshProUGUI textField, List<int> allowedEmojis = null)
	{
		using (List<ChatEmoji>.Enumerator enumerator = m_emojiList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ChatEmoji current = enumerator.Current;
				if (textField.text.Contains(current.m_emojiTag))
				{
					if (allowedEmojis != null)
					{
						for (int i = 0; i < allowedEmojis.Count; i++)
						{
							string unlocalizedChatEmojiName = GameBalanceVars.Get().GetUnlocalizedChatEmojiName(allowedEmojis[i], string.Empty);
							if (unlocalizedChatEmojiName == current.m_emojiName)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										return true;
									}
								}
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
		using (List<ChatEmoji>.Enumerator enumerator = m_emojiList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ChatEmoji current = enumerator.Current;
				if (text.Contains(current.m_emojiTag))
				{
					int chatEmojiIndexByName = GameBalanceVars.Get().GetChatEmojiIndexByName(current.m_emojiName);
					list.Add(chatEmojiIndexByName);
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return list;
				}
			}
		}
	}

	public List<int> GetAllEmojisInString(string text)
	{
		List<int> list = new List<int>();
		using (List<ChatEmoji>.Enumerator enumerator = m_emojiList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ChatEmoji current = enumerator.Current;
				if (text.Contains(current.m_emojiTag))
				{
					int chatEmojiIDByName = GameBalanceVars.Get().GetChatEmojiIDByName(current.m_emojiName);
					list.Add(chatEmojiIDByName);
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (true)
					{
						return list;
					}
					/*OpCode not supported: LdMemberToken*/;
					return list;
				}
			}
		}
	}

	public string UnlocalizeEmojis(string theString)
	{
		string text = theString;
		using (List<ChatEmoji>.Enumerator enumerator = m_emojiList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ChatEmoji current = enumerator.Current;
				string emojiTag = current.GetEmojiTag();
				if (theString.Contains(emojiTag))
				{
					if (!emojiTag.IsNullOrEmpty())
					{
						text = text.Replace(emojiTag, current.m_emojiTag);
					}
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return text;
				}
			}
		}
	}

	private void Update()
	{
		for (int i = 0; i < m_emojiTextFields.Count; i++)
		{
			EmojiDisplayInfo emojiDisplayInfo = m_emojiTextFields[i];
			if (emojiDisplayInfo.m_textField != null)
			{
				emojiDisplayInfo.Update();
			}
			else
			{
				m_emojiTextFields.Remove(emojiDisplayInfo);
				i--;
			}
		}
	}

	private void Start()
	{
		s_instance = this;
	}
}
