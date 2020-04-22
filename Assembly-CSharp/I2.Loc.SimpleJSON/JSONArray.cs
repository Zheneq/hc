using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace I2.Loc.SimpleJSON
{
	[DefaultMember("Item")]
	public class JSONArray : JSONNode, IEnumerable
	{
		private List<JSONNode> nodes = new List<JSONNode>();

		public override JSONNode _0018
		{
			get
			{
				if (_001D >= 0)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (_001D < nodes.Count)
					{
						return nodes[_001D];
					}
				}
				return new JSONLazyCreator(this);
			}
			set
			{
				if (_001D >= 0)
				{
					while (true)
					{
						switch (3)
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
					if (_001D < nodes.Count)
					{
						nodes[_001D] = value;
						return;
					}
				}
				nodes.Add(value);
			}
		}

		public override JSONNode symbol_0018_2
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				nodes.Add(value);
			}
		}

		public override int _0019 => nodes.Count;

		public override IEnumerable<JSONNode> _001A
		{
			get
			{
				_003C_003Ec__Iterator0 _003C_003Ec__Iterator = new _003C_003Ec__Iterator0();
				_003C_003Ec__Iterator._0012 = this;
				_003C_003Ec__Iterator._0013 = -2;
				return _003C_003Ec__Iterator;
			}
		}

		public override void _0013(string _001D, JSONNode _000E)
		{
			nodes.Add(_000E);
		}

		public override JSONNode _0011(int _001D)
		{
			if (_001D < 0 || _001D >= nodes.Count)
			{
				return null;
			}
			JSONNode result = nodes[_001D];
			nodes.RemoveAt(_001D);
			return result;
		}

		public override JSONNode _0011(JSONNode _001D)
		{
			nodes.Remove(_001D);
			return _001D;
		}

		public IEnumerator GetEnumerator()
		{
			using (List<JSONNode>.Enumerator enumerator = nodes.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					yield return enumerator.Current;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
		}

		public override string ToString()
		{
			string text = "[ ";
			using (List<JSONNode>.Enumerator enumerator = nodes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JSONNode current = enumerator.Current;
					if (text.Length > 2)
					{
						text += ", ";
					}
					text += current.ToString();
				}
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
						goto end_IL_0014;
					}
				}
				end_IL_0014:;
			}
			return text + " ]";
		}

		public override string _0004(string _001D)
		{
			string text = "[ ";
			foreach (JSONNode node in nodes)
			{
				if (text.Length > 3)
				{
					while (true)
					{
						switch (3)
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
					text += ", ";
				}
				text = text + "\n" + _001D + "   ";
				text += node._0004(_001D + "   ");
			}
			return text + "\n" + _001D + "]";
		}

		public override void _0002(BinaryWriter _001D)
		{
			_001D.Write((byte)1);
			_001D.Write(nodes.Count);
			for (int i = 0; i < nodes.Count; i++)
			{
				nodes[i]._0002(_001D);
			}
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
				return;
			}
		}
	}
}
