using System;
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

		public override JSONNode symbol_0018
		{
			get
			{
				if (symbol_001D >= 0)
				{
					if (symbol_001D < this.nodes.Count)
					{
						return this.nodes[symbol_001D];
					}
				}
				return new JSONLazyCreator(this);
			}
			set
			{
				if (symbol_001D >= 0)
				{
					if (symbol_001D < this.nodes.Count)
					{
						this.nodes[symbol_001D] = value;
						return;
					}
				}
				this.nodes.Add(value);
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
				this.nodes.Add(value);
			}
		}

		public override int symbol_0019
		{
			get
			{
				return this.nodes.Count;
			}
		}

		public override void symbol_0013(string symbol_001D, JSONNode symbol_000E)
		{
			this.nodes.Add(symbol_000E);
		}

		public override JSONNode symbol_0011(int symbol_001D)
		{
			if (symbol_001D < 0 || symbol_001D >= this.nodes.Count)
			{
				return null;
			}
			JSONNode result = this.nodes[symbol_001D];
			this.nodes.RemoveAt(symbol_001D);
			return result;
		}

		public override JSONNode symbol_0011(JSONNode symbol_001D)
		{
			this.nodes.Remove(symbol_001D);
			return symbol_001D;
		}

		public override IEnumerable<JSONNode> symbol_001A
		{
			get
			{
				bool flag = false;
				uint num;
				List<JSONNode>.Enumerator enumerator;
				switch (num)
				{
				case 0U:
					enumerator = this.nodes.GetEnumerator();
					break;
				case 1U:
					break;
				default:
					yield break;
				}
				try
				{
					while (enumerator.MoveNext())
					{
						JSONNode jsonnode = enumerator.Current;
						yield return jsonnode;
						flag = true;
					}
				}
				finally
				{
					if (flag)
					{
					}
					else
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				yield break;
			}
		}

		public IEnumerator GetEnumerator()
		{
			bool flag = false;
			uint num;
			List<JSONNode>.Enumerator enumerator;
			switch (num)
			{
			case 0U:
				enumerator = this.nodes.GetEnumerator();
				break;
			case 1U:
				break;
			default:
				yield break;
			}
			try
			{
				while (enumerator.MoveNext())
				{
					JSONNode jsonnode = enumerator.Current;
					yield return jsonnode;
					flag = true;
				}
			}
			finally
			{
				if (flag)
				{
				}
				else
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			yield break;
		}

		public override string ToString()
		{
			string text = "[ ";
			using (List<JSONNode>.Enumerator enumerator = this.nodes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JSONNode jsonnode = enumerator.Current;
					if (text.Length > 2)
					{
						text += ", ";
					}
					text += jsonnode.ToString();
				}
			}
			text += " ]";
			return text;
		}

		public override string symbol_0004(string symbol_001D)
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.nodes)
			{
				if (text.Length > 3)
				{
					text += ", ";
				}
				text = text + "\n" + symbol_001D + "   ";
				text += jsonnode.symbol_0004(symbol_001D + "   ");
			}
			text = text + "\n" + symbol_001D + "]";
			return text;
		}

		public override void symbol_0002(BinaryWriter symbol_001D)
		{
			symbol_001D.Write(1);
			symbol_001D.Write(this.nodes.Count);
			for (int i = 0; i < this.nodes.Count; i++)
			{
				this.nodes[i].symbol_0002(symbol_001D);
			}
		}
	}
}
