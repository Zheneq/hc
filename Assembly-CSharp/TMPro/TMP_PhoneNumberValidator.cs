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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_PhoneNumberValidator.Validate(string*, int*, char)).MethodHandle;
				}
				if (ch > '9')
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
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						text = "(" + ch;
					}
					pos = 2;
					break;
				case 1:
					if (i == length)
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
						text += ch;
					}
					pos = 2;
					break;
				case 2:
					if (i == length)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						text += ch;
					}
					pos = 3;
					break;
				case 3:
					if (i == length)
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
						text = text + ch + ") ";
					}
					pos = 6;
					break;
				case 4:
					if (i == length)
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
						text = text + ") " + ch;
					}
					pos = 7;
					break;
				case 5:
					if (i == length)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						text = text + " " + ch;
					}
					pos = 7;
					break;
				case 6:
					if (i == length)
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
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
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
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			return ch;
		}
	}
}
