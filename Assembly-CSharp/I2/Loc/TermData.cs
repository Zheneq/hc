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
			if (TermData.IsTouchType())
			{
				string result;
				if (!string.IsNullOrEmpty(this.Languages_Touch[idx]))
				{
					result = this.Languages_Touch[idx];
				}
				else
				{
					result = this.Languages[idx];
				}
				return result;
			}
			return string.IsNullOrEmpty(this.Languages[idx]) ? this.Languages_Touch[idx] : this.Languages[idx];
		}

		public bool IsAutoTranslated(int idx, bool IsTouch)
		{
			if (IsTouch)
			{
				return (this.Flags[idx] & 2) > 0;
			}
			return (this.Flags[idx] & 1) > 0;
		}

		public bool HasTouchTranslations()
		{
			int i = 0;
			int num = this.Languages_Touch.Length;
			while (i < num)
			{
				if (!string.IsNullOrEmpty(this.Languages_Touch[i]))
				{
					if (!string.IsNullOrEmpty(this.Languages[i]))
					{
						if (this.Languages_Touch[i] != this.Languages[i])
						{
							return true;
						}
					}
				}
				i++;
			}
			return false;
		}

		public void Validate()
		{
			int num = Mathf.Max(this.Languages.Length, Mathf.Max(this.Languages_Touch.Length, this.Flags.Length));
			if (this.Languages.Length != num)
			{
				Array.Resize<string>(ref this.Languages, num);
			}
			if (this.Languages_Touch.Length != num)
			{
				Array.Resize<string>(ref this.Languages_Touch, num);
			}
			if (this.Flags.Length != num)
			{
				Array.Resize<byte>(ref this.Flags, num);
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
				return name == this.Term;
			}
			return name == LanguageSource.GetKeyFromFullTerm(this.Term, false);
		}
	}
}
