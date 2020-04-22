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
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
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
