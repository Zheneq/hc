using System;

namespace TMPro
{
	public struct TMP_BasicXmlTagStack
	{
		public byte bold;

		public byte italic;

		public byte underline;

		public byte strikethrough;

		public byte highlight;

		public byte superscript;

		public byte subscript;

		public byte uppercase;

		public byte lowercase;

		public byte smallcaps;

		public void Clear()
		{
			this.bold = 0;
			this.italic = 0;
			this.underline = 0;
			this.strikethrough = 0;
			this.highlight = 0;
			this.superscript = 0;
			this.subscript = 0;
			this.uppercase = 0;
			this.lowercase = 0;
			this.smallcaps = 0;
		}

		public byte Add(FontStyles style)
		{
			switch (style)
			{
			case FontStyles.Bold:
				this.bold += 1;
				return this.bold;
			case FontStyles.Italic:
				this.italic += 1;
				return this.italic;
			default:
				if (style == FontStyles.Strikethrough)
				{
					this.strikethrough += 1;
					return this.strikethrough;
				}
				if (style == FontStyles.Superscript)
				{
					this.superscript += 1;
					return this.superscript;
				}
				if (style == FontStyles.Subscript)
				{
					this.subscript += 1;
					return this.subscript;
				}
				if (style != FontStyles.Highlight)
				{
					return 0;
				}
				this.highlight += 1;
				return this.highlight;
			case FontStyles.Underline:
				this.underline += 1;
				return this.underline;
			}
		}

		public byte Remove(FontStyles style)
		{
			switch (style)
			{
			case FontStyles.Bold:
				if (this.bold > 1)
				{
					this.bold -= 1;
				}
				else
				{
					this.bold = 0;
				}
				return this.bold;
			case FontStyles.Italic:
				if (this.italic > 1)
				{
					this.italic -= 1;
				}
				else
				{
					this.italic = 0;
				}
				return this.italic;
			default:
				if (style == FontStyles.Strikethrough)
				{
					if (this.strikethrough > 1)
					{
						this.strikethrough -= 1;
					}
					else
					{
						this.strikethrough = 0;
					}
					return this.strikethrough;
				}
				if (style == FontStyles.Superscript)
				{
					if (this.superscript > 1)
					{
						this.superscript -= 1;
					}
					else
					{
						this.superscript = 0;
					}
					return this.superscript;
				}
				if (style == FontStyles.Subscript)
				{
					if (this.subscript > 1)
					{
						this.subscript -= 1;
					}
					else
					{
						this.subscript = 0;
					}
					return this.subscript;
				}
				if (style != FontStyles.Highlight)
				{
					return 0;
				}
				if (this.highlight > 1)
				{
					this.highlight -= 1;
				}
				else
				{
					this.highlight = 0;
				}
				return this.highlight;
			case FontStyles.Underline:
				if (this.underline > 1)
				{
					this.underline -= 1;
				}
				else
				{
					this.underline = 0;
				}
				return this.underline;
			}
		}
	}
}
