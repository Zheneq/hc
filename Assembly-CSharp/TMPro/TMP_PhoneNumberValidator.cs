using System;
using System.Text;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public class TMP_PhoneNumberValidator : TMP_InputValidator
	{
		public override char Validate(ref string text, ref int pos, char ch)
		{
			Debug.Log("Trying to validate...");
			if (ch < '0')
			{
				if (ch > '9')
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return '\0';
						}
					}
				}
			}
			int length = text.Length;
			for (int i = 0; i < length + 1; i++)
			{
				switch (i)
				{
				case 0:
					if (i == length)
					{
						text = new StringBuilder().Append("(").Append(ch).ToString();
					}
					pos = 2;
					break;
				case 1:
					if (i == length)
					{
						text += ch;
					}
					pos = 2;
					break;
				case 2:
					if (i == length)
					{
						text += ch;
					}
					pos = 3;
					break;
				case 3:
					if (i == length)
					{
						text = new StringBuilder().Append(text).Append(ch).Append(") ").ToString();
					}
					pos = 6;
					break;
				case 4:
					if (i == length)
					{
						text = new StringBuilder().Append(text).Append(") ").Append(ch).ToString();
					}
					pos = 7;
					break;
				case 5:
					if (i == length)
					{
						text = new StringBuilder().Append(text).Append(" ").Append(ch).ToString();
					}
					pos = 7;
					break;
				case 6:
					if (i == length)
					{
						text += ch;
					}
					pos = 7;
					break;
				case 7:
					if (i == length)
					{
						text += ch;
					}
					pos = 8;
					break;
				case 8:
					if (i == length)
					{
						text = new StringBuilder().Append(text).Append(ch).Append("-").ToString();
					}
					pos = 10;
					break;
				case 9:
					if (i == length)
					{
						text = new StringBuilder().Append(text).Append("-").Append(ch).ToString();
					}
					pos = 11;
					break;
				case 10:
					if (i == length)
					{
						text += ch;
					}
					pos = 11;
					break;
				case 11:
					if (i == length)
					{
						text += ch;
					}
					pos = 12;
					break;
				case 12:
					if (i == length)
					{
						text += ch;
					}
					pos = 13;
					break;
				case 13:
					if (i == length)
					{
						text += ch;
					}
					pos = 14;
					break;
				}
			}
			while (true)
			{
				return ch;
			}
		}
	}
}
