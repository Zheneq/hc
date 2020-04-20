using System;

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
			TMP_CharacterInfo[] characterInfo = this.textComponent.textInfo.characterInfo;
			for (int i = this.firstCharacterIndex; i < this.lastCharacterIndex + 1; i++)
			{
				text += characterInfo[i].character;
			}
			return text;
		}
	}
}
