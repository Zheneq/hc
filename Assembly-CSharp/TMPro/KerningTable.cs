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
				IEnumerable<KerningPair> source = this.kerningPairs;
				
				IOrderedEnumerable<KerningPair> source2 = source.OrderBy(((KerningPair s) => s.AscII_Left));
				
				this.kerningPairs = source2.ThenBy(((KerningPair s) => s.AscII_Right)).ToList<KerningPair>();
			}
		}
	}
}
