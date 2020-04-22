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
			bold = 0;
			italic = 0;
			underline = 0;
			strikethrough = 0;
			highlight = 0;
			superscript = 0;
			subscript = 0;
			uppercase = 0;
			lowercase = 0;
			smallcaps = 0;
		}

		public byte Add(FontStyles style)
		{
			switch (style)
			{
			default:
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (style != FontStyles.Superscript)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								if (style != FontStyles.Subscript)
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											break;
										default:
											if (style == FontStyles.Highlight)
											{
												highlight++;
												return highlight;
											}
											return 0;
										}
									}
								}
								subscript++;
								return subscript;
							}
						}
					}
					superscript++;
					return superscript;
				}
			case FontStyles.Bold:
				bold++;
				return bold;
			case FontStyles.Italic:
				italic++;
				return italic;
			case FontStyles.Underline:
				underline++;
				return underline;
			case FontStyles.Strikethrough:
				strikethrough++;
				return strikethrough;
			}
		}

		public byte Remove(FontStyles style)
		{
			switch (style)
			{
			default:
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (style != FontStyles.Superscript)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								switch (style)
								{
								default:
									while (true)
									{
										switch (1)
										{
										case 0:
											break;
										default:
											return 0;
										}
									}
								case FontStyles.Highlight:
									if (highlight > 1)
									{
										while (true)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											break;
										}
										highlight--;
									}
									else
									{
										highlight = 0;
									}
									return highlight;
								case FontStyles.Subscript:
									if (subscript > 1)
									{
										while (true)
										{
											switch (6)
											{
											case 0:
												continue;
											}
											break;
										}
										subscript--;
									}
									else
									{
										subscript = 0;
									}
									return subscript;
								}
							}
						}
					}
					if (superscript > 1)
					{
						superscript--;
					}
					else
					{
						superscript = 0;
					}
					return superscript;
				}
			case FontStyles.Bold:
				if (bold > 1)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					bold--;
				}
				else
				{
					bold = 0;
				}
				return bold;
			case FontStyles.Italic:
				if (italic > 1)
				{
					italic--;
				}
				else
				{
					italic = 0;
				}
				return italic;
			case FontStyles.Underline:
				if (underline > 1)
				{
					underline--;
				}
				else
				{
					underline = 0;
				}
				return underline;
			case FontStyles.Strikethrough:
				if (strikethrough > 1)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					strikethrough--;
				}
				else
				{
					strikethrough = 0;
				}
				return strikethrough;
			}
		}
	}
}
