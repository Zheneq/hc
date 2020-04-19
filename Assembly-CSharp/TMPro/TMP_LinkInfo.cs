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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_LinkInfo.SetLinkID(char[], int, int)).MethodHandle;
				}
				if (this.linkID.Length >= length)
				{
					goto IL_3C;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.linkID = new char[length];
			IL_3C:
			for (int i = 0; i < length; i++)
			{
				this.linkID[i] = text[startIndex + i];
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
		}

		public string GetLinkText()
		{
			string text = string.Empty;
			TMP_TextInfo textInfo = this.textComponent.textInfo;
			for (int i = this.linkTextfirstCharacterIndex; i < this.linkTextfirstCharacterIndex + this.linkTextLength; i++)
			{
				text += textInfo.characterInfo[i].character;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_LinkInfo.GetLinkText()).MethodHandle;
			}
			return text;
		}

		public string GetLinkID()
		{
			if (this.textComponent == null)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_LinkInfo.GetLinkID()).MethodHandle;
				}
				return string.Empty;
			}
			return new string(this.linkID, 0, this.linkIdLength);
		}
	}
}
