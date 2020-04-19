using System;

namespace TMPro
{
	[Serializable]
	public class TMP_DigitValidator : TMP_InputValidator
	{
		public unsafe override char Validate(ref string text, ref int pos, char ch)
		{
			if (ch >= '0')
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_DigitValidator.Validate(string*, int*, char)).MethodHandle;
				}
				if (ch <= '9')
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
					pos++;
					return ch;
				}
			}
			return '\0';
		}
	}
}
