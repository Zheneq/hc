using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace I2.Loc.SimpleJSON
{
	[DefaultMember("Item")]
	public class JSONClass : JSONNode, IEnumerable
	{
		private Dictionary<string, JSONNode> symbol_000E = new Dictionary<string, JSONNode>();

		public override JSONNode symbol_0018
		{
			get
			{
				if (this.symbol_000E.ContainsKey(symbol_001D))
				{
					return this.symbol_000E[symbol_001D];
				}
				return new JSONLazyCreator(this, symbol_001D);
			}
			set
			{
				if (this.symbol_000E.ContainsKey(symbol_001D))
				{
					this.symbol_000E[symbol_001D] = value;
				}
				else
				{
					this.symbol_000E.Add(symbol_001D, value);
				}
			}
		}

		public override JSONNode symbol_0018
		{
			get
			{
				if (symbol_001D >= 0)
				{
					if (symbol_001D < this.symbol_000E.Count)
					{
						return this.symbol_000E.ElementAt(symbol_001D).Value;
					}
				}
				return null;
			}
			set
			{
				if (symbol_001D >= 0)
				{
					if (symbol_001D < this.symbol_000E.Count)
					{
						string key = this.symbol_000E.ElementAt(symbol_001D).Key;
						this.symbol_000E[key] = value;
						return;
					}
				}
			}
		}

		public override int symbol_0019
		{
			get
			{
				return this.symbol_000E.Count;
			}
		}

		public override void symbol_0013(string symbol_001D, JSONNode symbol_000E)
		{
			if (!string.IsNullOrEmpty(symbol_001D))
			{
				if (this.symbol_000E.ContainsKey(symbol_001D))
				{
					this.symbol_000E[symbol_001D] = symbol_000E;
				}
				else
				{
					this.symbol_000E.Add(symbol_001D, symbol_000E);
				}
			}
			else
			{
				this.symbol_000E.Add(Guid.NewGuid().ToString(), symbol_000E);
			}
		}

		public override JSONNode symbol_0011(string symbol_001D)
		{
			if (!this.symbol_000E.ContainsKey(symbol_001D))
			{
				return null;
			}
			JSONNode result = this.symbol_000E[symbol_001D];
			this.symbol_000E.Remove(symbol_001D);
			return result;
		}

		public override JSONNode symbol_0011(int symbol_001D)
		{
			if (symbol_001D >= 0)
			{
				if (symbol_001D < this.symbol_000E.Count)
				{
					KeyValuePair<string, JSONNode> keyValuePair = this.symbol_000E.ElementAt(symbol_001D);
					this.symbol_000E.Remove(keyValuePair.Key);
					return keyValuePair.Value;
				}
			}
			return null;
		}

		public override JSONNode symbol_0011(JSONNode symbol_001D)
		{
			JSONClass._Removec__AnonStorey2 _Removec__AnonStorey = new JSONClass._Removec__AnonStorey2();
			_Removec__AnonStorey.symbol_001D = symbol_001D;
			JSONNode result;
			try
			{
				KeyValuePair<string, JSONNode> keyValuePair = this.symbol_000E.Where(new Func<KeyValuePair<string, JSONNode>, bool>(_Removec__AnonStorey.symbol_000E)).First<KeyValuePair<string, JSONNode>>();
				this.symbol_000E.Remove(keyValuePair.Key);
				result = _Removec__AnonStorey.symbol_001D;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		public override IEnumerable<JSONNode> symbol_001A
		{
			get
			{
				bool flag = false;
				uint num;
				Dictionary<string, JSONNode>.Enumerator enumerator;
				switch (num)
				{
				case 0U:
					enumerator = this.symbol_000E.GetEnumerator();
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
						KeyValuePair<string, JSONNode> keyValuePair = enumerator.Current;
						yield return keyValuePair.Value;
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
			Dictionary<string, JSONNode>.Enumerator enumerator;
			switch (num)
			{
			case 0U:
				enumerator = this.symbol_000E.GetEnumerator();
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
					KeyValuePair<string, JSONNode> keyValuePair = enumerator.Current;
					yield return keyValuePair;
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
			string text = "{";
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.symbol_000E)
			{
				if (text.Length > 2)
				{
					text += ", ";
				}
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\"",
					JSONNode.escapeString(keyValuePair.Key),
					"\":",
					keyValuePair.Value.ToString()
				});
			}
			text += "}";
			return text;
		}

		public override string symbol_0004(string symbol_001D)
		{
			string text = "{ ";
			using (Dictionary<string, JSONNode>.Enumerator enumerator = this.symbol_000E.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> keyValuePair = enumerator.Current;
					if (text.Length > 3)
					{
						text += ", ";
					}
					text = text + "\n" + symbol_001D + "   ";
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"\"",
						JSONNode.escapeString(keyValuePair.Key),
						"\" : ",
						keyValuePair.Value.symbol_0004(symbol_001D + "   ")
					});
				}
			}
			text = text + "\n" + symbol_001D + "}";
			return text;
		}

		public override void symbol_0002(BinaryWriter symbol_001D)
		{
			symbol_001D.Write(2);
			symbol_001D.Write(this.symbol_000E.Count);
			using (Dictionary<string, JSONNode>.KeyCollection.Enumerator enumerator = this.symbol_000E.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text = enumerator.Current;
					symbol_001D.Write(text);
					this.symbol_000E[text].symbol_0002(symbol_001D);
				}
			}
		}
	}
}
