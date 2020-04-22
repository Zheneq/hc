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
		private Dictionary<string, JSONNode> _000E = new Dictionary<string, JSONNode>();

		public override JSONNode _0018
		{
			get
			{
				if (_000E.ContainsKey(_001D))
				{
					return _000E[_001D];
				}
				return new JSONLazyCreator(this, _001D);
			}
			set
			{
				if (_000E.ContainsKey(_001D))
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							_000E[_001D] = value;
							return;
						}
					}
				}
				_000E.Add(_001D, value);
			}
		}

		public override JSONNode _0018
		{
			get
			{
				if (_001D >= 0)
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
					if (_001D < _000E.Count)
					{
						return _000E.ElementAt(_001D).Value;
					}
					while (true)
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
				if (_001D < 0)
				{
					return;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (_001D < _000E.Count)
					{
						string key = _000E.ElementAt(_001D).Key;
						_000E[key] = value;
					}
					return;
				}
			}
		}

		public override int _0019 => _000E.Count;

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
			if (!string.IsNullOrEmpty(_001D))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (this._000E.ContainsKey(_001D))
						{
							this._000E[_001D] = _000E;
						}
						else
						{
							this._000E.Add(_001D, _000E);
						}
						return;
					}
				}
			}
			this._000E.Add(Guid.NewGuid().ToString(), _000E);
		}

		public override JSONNode _0011(string _001D)
		{
			if (!_000E.ContainsKey(_001D))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return null;
					}
				}
			}
			JSONNode result = _000E[_001D];
			_000E.Remove(_001D);
			return result;
		}

		public override JSONNode _0011(int _001D)
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
				if (_001D < _000E.Count)
				{
					KeyValuePair<string, JSONNode> keyValuePair = _000E.ElementAt(_001D);
					_000E.Remove(keyValuePair.Key);
					return keyValuePair.Value;
				}
			}
			return null;
		}

		public override JSONNode _0011(JSONNode _001D)
		{
			try
			{
				KeyValuePair<string, JSONNode> keyValuePair = _000E.Where((KeyValuePair<string, JSONNode> _001D) => JSONNode._000A(_001D.Value, _001D)).First();
				_000E.Remove(keyValuePair.Key);
				return _001D;
			}
			catch
			{
				return null;
			}
		}

		public IEnumerator GetEnumerator()
		{
			using (Dictionary<string, JSONNode>.Enumerator enumerator = _000E.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> current = enumerator.Current;
					yield return current;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
		}

		public override string ToString()
		{
			string text = "{";
			foreach (KeyValuePair<string, JSONNode> item in _000E)
			{
				if (text.Length > 2)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					text += ", ";
				}
				string text2 = text;
				text = text2 + "\"" + JSONNode.escapeString(item.Key) + "\":" + item.Value.ToString();
			}
			return text + "}";
		}

		public override string _0004(string _001D)
		{
			string text = "{ ";
			using (Dictionary<string, JSONNode>.Enumerator enumerator = _000E.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> current = enumerator.Current;
					if (text.Length > 3)
					{
						text += ", ";
					}
					text = text + "\n" + _001D + "   ";
					string text2 = text;
					text = text2 + "\"" + JSONNode.escapeString(current.Key) + "\" : " + current.Value._0004(_001D + "   ");
				}
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
						goto end_IL_0014;
					}
				}
				end_IL_0014:;
			}
			return text + "\n" + _001D + "}";
		}

		public override void _0002(BinaryWriter _001D)
		{
			_001D.Write((byte)2);
			_001D.Write(_000E.Count);
			using (Dictionary<string, JSONNode>.KeyCollection.Enumerator enumerator = _000E.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					_001D.Write(current);
					_000E[current]._0002(_001D);
				}
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
						return;
					}
				}
			}
		}
	}
}
