using System;

namespace TMPro
{
	public struct TMP_LinkInfo
	{
		public TMP_Text textComponent;

		public int hashCode;

		public int linkIdFirstCharacterIndex;

		public int linkIdLength;

		public int linkTextfirstCharacterIndex;

		public int linkTextLength;

		internal char[] linkID;

		internal void SetLinkID(char[] text, int startIndex, int length)
		{
			if (this.linkID != null)
			{
				if (this.linkID.Length >= length)
				{
					goto IL_3C;
				}
			}
			this.linkID = new char[length];
			IL_3C:
			for (int i = 0; i < length; i++)
			{
				this.linkID[i] = text[startIndex + i];
			}
		}

		public string GetLinkText()
		{
			string text = string.Empty;
			TMP_TextInfo textInfo = this.textComponent.textInfo;
			for (int i = this.linkTextfirstCharacterIndex; i < this.linkTextfirstCharacterIndex + this.linkTextLength; i++)
			{
				text += textInfo.characterInfo[i].character;
			}
			return text;
		}

		public string GetLinkID()
		{
			if (this.textComponent == null)
			{
				return string.Empty;
			}
			return new string(this.linkID, 0, this.linkIdLength);
		}
	}
}
