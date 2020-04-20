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
		private Dictionary<string, JSONNode> \u000E = new Dictionary<string, JSONNode>();

		public override JSONNode \u0018
		{
			get
			{
				if (this.\u000E.ContainsKey(\u001D))
				{
					return this.\u000E[\u001D];
				}
				return new JSONLazyCreator(this, \u001D);
			}
			set
			{
				if (this.\u000E.ContainsKey(\u001D))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(JSONClass.set_\u0018(string, JSONNode)).MethodHandle;
					}
					this.\u000E[\u001D] = value;
				}
				else
				{
					this.\u000E.Add(\u001D, value);
				}
			}
		}

		public override JSONNode \u0018
		{
			get
			{
				if (\u001D >= 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(JSONClass.get_\u0018(int)).MethodHandle;
					}
					if (\u001D < this.\u000E.Count)
					{
						return this.\u000E.ElementAt(\u001D).Value;
					}
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				return null;
			}
			set
			{
				if (\u001D >= 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(JSONClass.set_\u0018(int, JSONNode)).MethodHandle;
					}
					if (\u001D < this.\u000E.Count)
					{
						string key = this.\u000E.ElementAt(\u001D).Key;
						this.\u000E[key] = value;
						return;
					}
				}
			}
		}

		public override int \u0019
		{
			get
			{
				return this.\u000E.Count;
			}
		}

		public override void \u0013(string \u001D, JSONNode \u000E)
		{
			if (!string.IsNullOrEmpty(\u001D))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(JSONClass.\u0013(string, JSONNode)).MethodHandle;
				}
				if (this.\u000E.ContainsKey(\u001D))
				{
					this.\u000E[\u001D] = \u000E;
				}
				else
				{
					this.\u000E.Add(\u001D, \u000E);
				}
			}
			else
			{
				this.\u000E.Add(Guid.NewGuid().ToString(), \u000E);
			}
		}

		public override JSONNode \u0011(string \u001D)
		{
			if (!this.\u000E.ContainsKey(\u001D))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(JSONClass.\u0011(string)).MethodHandle;
				}
				return null;
			}
			JSONNode result = this.\u000E[\u001D];
			this.\u000E.Remove(\u001D);
			return result;
		}

		public override JSONNode \u0011(int \u001D)
		{
			if (\u001D >= 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(JSONClass.\u0011(int)).MethodHandle;
				}
				if (\u001D < this.\u000E.Count)
				{
					KeyValuePair<string, JSONNode> keyValuePair = this.\u000E.ElementAt(\u001D);
					this.\u000E.Remove(keyValuePair.Key);
					return keyValuePair.Value;
				}
			}
			return null;
		}

		public override JSONNode \u0011(JSONNode \u001D)
		{
			JSONClass.<Remove>c__AnonStorey2 <Remove>c__AnonStorey = new JSONClass.<Remove>c__AnonStorey2();
			<Remove>c__AnonStorey.\u001D = \u001D;
			JSONNode result;
			try
			{
				KeyValuePair<string, JSONNode> keyValuePair = this.\u000E.Where(new Func<KeyValuePair<string, JSONNode>, bool>(<Remove>c__AnonStorey.\u000E)).First<KeyValuePair<string, JSONNode>>();
				this.\u000E.Remove(keyValuePair.Key);
				result = <Remove>c__AnonStorey.\u001D;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		public override IEnumerable<JSONNode> \u001A
		{
			get
			{
				bool flag = false;
				uint num;
				Dictionary<string, JSONNode>.Enumerator enumerator;
				switch (num)
				{
				case 0U:
					enumerator = this.\u000E.GetEnumerator();
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(JSONClass.<>c__Iterator0.MoveNext()).MethodHandle;
					}
				}
				finally
				{
					if (flag)
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
				enumerator = this.\u000E.GetEnumerator();
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(JSONClass.<GetEnumerator>c__Iterator1.MoveNext()).MethodHandle;
					}
					flag = true;
				}
			}
			finally
			{
				if (flag)
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
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.\u000E)
			{
				if (text.Length > 2)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(JSONClass.ToString()).MethodHandle;
					}
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

		public override string \u0004(string \u001D)
		{
			string text = "{ ";
			using (Dictionary<string, JSONNode>.Enumerator enumerator = this.\u000E.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> keyValuePair = enumerator.Current;
					if (text.Length > 3)
					{
						text += ", ";
					}
					text = text + "\n" + \u001D + "   ";
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"\"",
						JSONNode.escapeString(keyValuePair.Key),
						"\" : ",
						keyValuePair.Value.\u0004(\u001D + "   ")
					});
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(JSONClass.\u0004(string)).MethodHandle;
				}
			}
			text = text + "\n" + \u001D + "}";
			return text;
		}

		public override void \u0002(BinaryWriter \u001D)
		{
			\u001D.Write(2);
			\u001D.Write(this.\u000E.Count);
			using (Dictionary<string, JSONNode>.KeyCollection.Enumerator enumerator = this.\u000E.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text = enumerator.Current;
					\u001D.Write(text);
					this.\u000E[text].\u0002(\u001D);
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(JSONClass.\u0002(BinaryWriter)).MethodHandle;
				}
			}
		}
	}
}
