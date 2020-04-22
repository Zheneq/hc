using System;
using UnityEngine;

namespace I2.Loc
{
	[Serializable]
	public class TermData
	{
		public string Term = string.Empty;

		public eTermType TermType;

		public string Description = string.Empty;

		public string LocIdStr = string.Empty;

		public string[] Languages = new string[0];

		public string[] Languages_Touch = new string[0];

		public byte[] Flags = new byte[0];

		public string GetTranslation(int idx)
		{
			if (IsTouchType())
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						object result;
						if (!string.IsNullOrEmpty(Languages_Touch[idx]))
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							result = Languages_Touch[idx];
						}
						else
						{
							result = Languages[idx];
						}
						return (string)result;
					}
					}
				}
			}
			return string.IsNullOrEmpty(Languages[idx]) ? Languages_Touch[idx] : Languages[idx];
		}

		public bool IsAutoTranslated(int idx, bool IsTouch)
		{
			if (IsTouch)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return (Flags[idx] & 2) > 0;
					}
				}
			}
			return (Flags[idx] & 1) > 0;
		}

		public bool HasTouchTranslations()
		{
			int i = 0;
			for (int num = Languages_Touch.Length; i < num; i++)
			{
				if (string.IsNullOrEmpty(Languages_Touch[i]))
				{
					continue;
				}
				while (true)
				{
					switch (7)
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
				if (string.IsNullOrEmpty(Languages[i]))
				{
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(Languages_Touch[i] != Languages[i]))
				{
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					return true;
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				return false;
			}
		}

		public void Validate()
		{
			int num = Mathf.Max(Languages.Length, Mathf.Max(Languages_Touch.Length, Flags.Length));
			if (Languages.Length != num)
			{
				while (true)
				{
					switch (2)
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
				Array.Resize(ref Languages, num);
			}
			if (Languages_Touch.Length != num)
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
				Array.Resize(ref Languages_Touch, num);
			}
			if (Flags.Length == num)
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				Array.Resize(ref Flags, num);
				return;
			}
		}

		public static bool IsTouchType()
		{
			return false;
		}

		public bool IsTerm(string name, bool allowCategoryMistmatch)
		{
			if (!allowCategoryMistmatch)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return name == Term;
					}
				}
			}
			return name == LanguageSource.GetKeyFromFullTerm(Term);
		}
	}
}
