using System;
using System.Text;

[Serializable]
public class BannedWords
{
	public BannedWordsData[] m_bannedWords;

	public static BannedWords Get()
	{
		return GameWideData.Get().m_bannedWords;
	}

	public string FilterPhrase(string phrase, string languageCode)
	{
		if (phrase.IsNullOrEmpty())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return phrase;
				}
			}
		}
		StringBuilder stringBuilder = new StringBuilder(phrase);
		char[] anyOf = " ,./?'<>:;\"[]{}-_+=!@#^&*()\\|".ToCharArray();
		int num = 0;
		while (true)
		{
			if (num < phrase.Length)
			{
				int num2 = phrase.IndexOfAny(anyOf, num);
				int num3;
				if (num2 < 0)
				{
					num3 = phrase.Length - num;
				}
				else
				{
					num3 = num2 - num;
				}
				int num4 = num3;
				if (num4 > 0)
				{
					string text = phrase.Substring(num, num4).ToLower();
					BannedWordsData[] bannedWords = m_bannedWords;
					foreach (BannedWordsData bannedWordsData in bannedWords)
					{
						if (!(bannedWordsData.Name == languageCode))
						{
							continue;
						}
						string[] fullStrings = bannedWordsData.m_fullStrings;
						foreach (string b in fullStrings)
						{
							if (text == b)
							{
								Mask(stringBuilder, num, num4);
							}
						}
						string[] subStrings = bannedWordsData.m_subStrings;
						foreach (string text2 in subStrings)
						{
							int length = text2.Length;
							int num5 = 0;
							while (true)
							{
								if (num5 < num4)
								{
									num5 = text.IndexOf(text2, num5);
									if (num5 == -1)
									{
										break;
									}
									Mask(stringBuilder, num + num5, length);
									num5++;
									continue;
								}
								break;
							}
						}
						string[] prefixStrings = bannedWordsData.m_prefixStrings;
						foreach (string text3 in prefixStrings)
						{
							int length2 = text3.Length;
							if (text.StartsWith(text3))
							{
								Mask(stringBuilder, num, length2);
							}
						}
						string[] suffixStrings = bannedWordsData.m_suffixStrings;
						foreach (string text4 in suffixStrings)
						{
							int length3 = text4.Length;
							if (text.EndsWith(text4))
							{
								Mask(stringBuilder, num + num4 - length3, length3);
							}
						}
					}
				}
				if (num2 < 0)
				{
					break;
				}
				num = num2 + 1;
				continue;
			}
			break;
		}
		return stringBuilder.ToString();
	}

	private void Mask(StringBuilder stringBuilder, int maskStartIndex, int maskLength)
	{
		stringBuilder.Remove(maskStartIndex, maskLength);
		stringBuilder.Insert(maskStartIndex, new string('*', maskLength));
	}
}
