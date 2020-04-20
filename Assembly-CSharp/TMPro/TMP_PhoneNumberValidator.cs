using System;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public class TMP_PhoneNumberValidator : TMP_InputValidator
	{
		public unsafe override char Validate(ref string text, ref int pos, char ch)
		{
			Debug.Log("Trying to validate...");
			if (ch < '0')
			{
				if (ch > '9')
				{
					return '\0';
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
						text = "(" + ch;
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
						text = text + ch + ") ";
					}
					pos = 6;
					break;
				case 4:
					if (i == length)
					{
						text = text + ") " + ch;
					}
					pos = 7;
					break;
				case 5:
					if (i == length)
					{
						text = text + " " + ch;
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
						text = text + ch + "-";
					}
					pos = 0xA;
					break;
				case 9:
					if (i == length)
					{
						text = text + "-" + ch;
					}
					pos = 0xB;
					break;
				case 0xA:
					if (i == length)
					{
						text += ch;
					}
					pos = 0xB;
					break;
				case 0xB:
					if (i == length)
					{
						text += ch;
					}
					pos = 0xC;
					break;
				case 0xC:
					if (i == length)
					{
						text += ch;
					}
					pos = 0xD;
					break;
				case 0xD:
					if (i == length)
					{
						text += ch;
					}
					pos = 0xE;
					break;
				}
			}
			return ch;
		}
	}
}
