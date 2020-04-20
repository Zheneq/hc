﻿using System;
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

		public override JSONNode \u0018
		{
			get
			{
				if (\u001D >= 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(JSONArray.get_\u0018(int)).MethodHandle;
					}
					if (\u001D < this.nodes.Count)
					{
						return this.nodes[\u001D];
					}
				}
				return new JSONLazyCreator(this);
			}
			set
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(JSONArray.set_\u0018(int, JSONNode)).MethodHandle;
					}
					if (\u001D < this.nodes.Count)
					{
						this.nodes[\u001D] = value;
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

		public override int \u0019
		{
			get
			{
				return this.nodes.Count;
			}
		}

		public override void \u0013(string \u001D, JSONNode \u000E)
		{
			this.nodes.Add(\u000E);
		}

		public override JSONNode \u0011(int \u001D)
		{
			if (\u001D < 0 || \u001D >= this.nodes.Count)
			{
				return null;
			}
			JSONNode result = this.nodes[\u001D];
			this.nodes.RemoveAt(\u001D);
			return result;
		}

		public override JSONNode \u0011(JSONNode \u001D)
		{
			this.nodes.Remove(\u001D);
			return \u001D;
		}

		public override IEnumerable<JSONNode> \u001A
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(JSONArray.<>c__Iterator0.MoveNext()).MethodHandle;
						}
						flag = true;
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(JSONArray.<GetEnumerator>c__Iterator1.MoveNext()).MethodHandle;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(JSONArray.ToString()).MethodHandle;
				}
			}
			text += " ]";
			return text;
		}

		public override string \u0004(string \u001D)
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.nodes)
			{
				if (text.Length > 3)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(JSONArray.\u0004(string)).MethodHandle;
					}
					text += ", ";
				}
				text = text + "\n" + \u001D + "   ";
				text += jsonnode.\u0004(\u001D + "   ");
			}
			text = text + "\n" + \u001D + "]";
			return text;
		}

		public override void \u0002(BinaryWriter \u001D)
		{
			\u001D.Write(1);
			\u001D.Write(this.nodes.Count);
			for (int i = 0; i < this.nodes.Count; i++)
			{
				this.nodes[i].\u0002(\u001D);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(JSONArray.\u0002(BinaryWriter)).MethodHandle;
			}
		}
	}
}
