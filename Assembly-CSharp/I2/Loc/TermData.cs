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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TermData.GetTranslation(int)).MethodHandle;
				}
				string result;
				if (!string.IsNullOrEmpty(this.Languages_Touch[idx]))
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
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
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TermData.IsAutoTranslated(int, bool)).MethodHandle;
				}
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
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(TermData.HasTouchTranslations()).MethodHandle;
					}
					if (!string.IsNullOrEmpty(this.Languages[i]))
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.Languages_Touch[i] != this.Languages[i])
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							return true;
						}
					}
				}
				i++;
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
			return false;
		}

		public void Validate()
		{
			int num = Mathf.Max(this.Languages.Length, Mathf.Max(this.Languages_Touch.Length, this.Flags.Length));
			if (this.Languages.Length != num)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TermData.Validate()).MethodHandle;
				}
				Array.Resize<string>(ref this.Languages, num);
			}
			if (this.Languages_Touch.Length != num)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				Array.Resize<string>(ref this.Languages_Touch, num);
			}
			if (this.Flags.Length != num)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TermData.IsTerm(string, bool)).MethodHandle;
				}
				return name == this.Term;
			}
			return name == LanguageSource.GetKeyFromFullTerm(this.Term, false);
		}
	}
}
