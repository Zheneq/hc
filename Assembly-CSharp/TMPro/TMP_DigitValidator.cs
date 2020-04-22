using System;

namespace TMPro
{
	[Serializable]
	public class TMP_DigitValidator : TMP_InputValidator
	{
		public override char Validate(ref string text, ref int pos, char ch)
		{
			if (ch >= '0')
			{
				if (ch <= '9')
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							pos++;
							return ch;
						}
					}
				}
			}
			return '\0';
		}
	}
}
