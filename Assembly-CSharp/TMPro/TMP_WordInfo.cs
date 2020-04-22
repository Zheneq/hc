namespace TMPro
{
	public struct TMP_WordInfo
	{
		public TMP_Text textComponent;

		public int firstCharacterIndex;

		public int lastCharacterIndex;

		public int characterCount;

		public string GetWord()
		{
			string text = string.Empty;
			TMP_CharacterInfo[] characterInfo = textComponent.textInfo.characterInfo;
			for (int i = firstCharacterIndex; i < lastCharacterIndex + 1; i++)
			{
				text += characterInfo[i].character;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return text;
			}
		}
	}
}
