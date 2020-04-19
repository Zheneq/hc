using System;
using System.Collections.Generic;
using System.Linq;

namespace TMPro
{
	[Serializable]
	public class KerningTable
	{
		public List<KerningPair> kerningPairs;

		public KerningTable()
		{
			this.kerningPairs = new List<KerningPair>();
		}

		public void AddKerningPair()
		{
			if (this.kerningPairs.Count == 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(KerningTable.AddKerningPair()).MethodHandle;
				}
				this.kerningPairs.Add(new KerningPair(0, 0, 0f));
			}
			else
			{
				int ascII_Left = this.kerningPairs.Last<KerningPair>().AscII_Left;
				int ascII_Right = this.kerningPairs.Last<KerningPair>().AscII_Right;
				float xadvanceOffset = this.kerningPairs.Last<KerningPair>().XadvanceOffset;
				this.kerningPairs.Add(new KerningPair(ascII_Left, ascII_Right, xadvanceOffset));
			}
		}

		public int AddKerningPair(int left, int right, float offset)
		{
			int num = this.kerningPairs.FindIndex(delegate(KerningPair item)
			{
				bool result;
				if (item.AscII_Left == left)
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
						RuntimeMethodHandle runtimeMethodHandle2 = methodof(KerningTable.<AddKerningPair>c__AnonStorey0.<>m__0(KerningPair)).MethodHandle;
					}
					result = (item.AscII_Right == right);
				}
				else
				{
					result = false;
				}
				return result;
			});
			if (num == -1)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(KerningTable.AddKerningPair(int, int, float)).MethodHandle;
				}
				this.kerningPairs.Add(new KerningPair(left, right, offset));
				return 0;
			}
			return -1;
		}

		public void RemoveKerningPair(int left, int right)
		{
			int num = this.kerningPairs.FindIndex(delegate(KerningPair item)
			{
				bool result;
				if (item.AscII_Left == left)
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
						RuntimeMethodHandle runtimeMethodHandle2 = methodof(KerningTable.<RemoveKerningPair>c__AnonStorey1.<>m__0(KerningPair)).MethodHandle;
					}
					result = (item.AscII_Right == right);
				}
				else
				{
					result = false;
				}
				return result;
			});
			if (num != -1)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(KerningTable.RemoveKerningPair(int, int)).MethodHandle;
				}
				this.kerningPairs.RemoveAt(num);
			}
		}

		public void RemoveKerningPair(int index)
		{
			this.kerningPairs.RemoveAt(index);
		}

		public void SortKerningPairs()
		{
			if (this.kerningPairs.Count > 0)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(KerningTable.SortKerningPairs()).MethodHandle;
				}
				IEnumerable<KerningPair> source = this.kerningPairs;
				if (KerningTable.<>f__am$cache0 == null)
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
					KerningTable.<>f__am$cache0 = ((KerningPair s) => s.AscII_Left);
				}
				IOrderedEnumerable<KerningPair> source2 = source.OrderBy(KerningTable.<>f__am$cache0);
				if (KerningTable.<>f__am$cache1 == null)
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
					KerningTable.<>f__am$cache1 = ((KerningPair s) => s.AscII_Right);
				}
				this.kerningPairs = source2.ThenBy(KerningTable.<>f__am$cache1).ToList<KerningPair>();
			}
		}
	}
}
