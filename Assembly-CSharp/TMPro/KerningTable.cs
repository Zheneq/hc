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
			kerningPairs = new List<KerningPair>();
		}

		public void AddKerningPair()
		{
			if (kerningPairs.Count == 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						kerningPairs.Add(new KerningPair(0, 0, 0f));
						return;
					}
				}
			}
			int ascII_Left = kerningPairs.Last().AscII_Left;
			int ascII_Right = kerningPairs.Last().AscII_Right;
			float xadvanceOffset = kerningPairs.Last().XadvanceOffset;
			kerningPairs.Add(new KerningPair(ascII_Left, ascII_Right, xadvanceOffset));
		}

		public int AddKerningPair(int left, int right, float offset)
		{
			int num = kerningPairs.FindIndex(delegate(KerningPair item)
			{
				int result;
				if (item.AscII_Left == left)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					result = ((item.AscII_Right == right) ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
			});
			if (num == -1)
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
						kerningPairs.Add(new KerningPair(left, right, offset));
						return 0;
					}
				}
			}
			return -1;
		}

		public void RemoveKerningPair(int left, int right)
		{
			int num = kerningPairs.FindIndex(delegate(KerningPair item)
			{
				int result;
				if (item.AscII_Left == left)
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
					result = ((item.AscII_Right == right) ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
			});
			if (num == -1)
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				kerningPairs.RemoveAt(num);
				return;
			}
		}

		public void RemoveKerningPair(int index)
		{
			kerningPairs.RemoveAt(index);
		}

		public void SortKerningPairs()
		{
			if (kerningPairs.Count <= 0)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				List<KerningPair> source = kerningPairs;
				if (_003C_003Ef__am_0024cache0 == null)
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
					_003C_003Ef__am_0024cache0 = ((KerningPair s) => s.AscII_Left);
				}
				IOrderedEnumerable<KerningPair> source2 = source.OrderBy(_003C_003Ef__am_0024cache0);
				if (_003C_003Ef__am_0024cache1 == null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					_003C_003Ef__am_0024cache1 = ((KerningPair s) => s.AscII_Right);
				}
				kerningPairs = source2.ThenBy(_003C_003Ef__am_0024cache1).ToList();
				return;
			}
		}
	}
}
