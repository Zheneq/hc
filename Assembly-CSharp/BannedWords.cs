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
			return phrase;
		}
		StringBuilder stringBuilder = new StringBuilder(phrase);
		char[] anyOf = " ,./?'<>:;\"[]{}-_+=!@#^&*()\\|".ToCharArray();
		int num;
		for (int i = 0; i < phrase.Length; i = num + 1)
		{
			num = phrase.IndexOfAny(anyOf, i);
			int num2;
			if (num < 0)
			{
				num2 = phrase.Length - i;
			}
			else
			{
				num2 = num - i;
			}
			int num3 = num2;
			if (num3 > 0)
			{
				string text = phrase.Substring(i, num3).ToLower();
				foreach (BannedWordsData bannedWordsData in this.m_bannedWords)
				{
					if (bannedWordsData.Name == languageCode)
					{
						foreach (string b in bannedWordsData.m_fullStrings)
						{
							if (text == b)
							{
								this.Mask(stringBuilder, i, num3);
							}
						}
						string[] subStrings = bannedWordsData.m_subStrings;
						int l = 0;
						IL_157:
						while (l < subStrings.Length)
						{
							string text2 = subStrings[l];
							int length = text2.Length;
							for (int m = 0; m < num3; m++)
							{
								m = text.IndexOf(text2, m);
								if (m == -1)
								{
									IL_151:
									l++;
									goto IL_157;
								}
								this.Mask(stringBuilder, i + m, length);
							}
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								goto IL_151;
							}
						}
						foreach (string text3 in bannedWordsData.m_prefixStrings)
						{
							int length2 = text3.Length;
							if (text.StartsWith(text3))
							{
								this.Mask(stringBuilder, i, length2);
							}
						}
						foreach (string text4 in bannedWordsData.m_suffixStrings)
						{
							int length3 = text4.Length;
							if (text.EndsWith(text4))
							{
								this.Mask(stringBuilder, i + num3 - length3, length3);
							}
						}
					}
				}
			}
			if (num < 0)
			{
				IL_25A:
				return stringBuilder.ToString();
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			goto IL_25A;
		}
	}

	private void Mask(StringBuilder stringBuilder, int maskStartIndex, int maskLength)
	{
		stringBuilder.Remove(maskStartIndex, maskLength);
		stringBuilder.Insert(maskStartIndex, new string('*', maskLength));
	}
}
