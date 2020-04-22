using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace I2.Loc.SimpleJSON
{
	[DefaultMember("Item")]
	public class JSONNode
	{
		public virtual JSONNode no_op_return_null_A
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public virtual JSONNode no_op_return_null_B
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public virtual string _0009
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		public virtual int _0019 => 0;

		public virtual IEnumerable<JSONNode> _001A
		{
			get
			{
				_003C_003Ec__Iterator0 _003C_003Ec__Iterator = new _003C_003Ec__Iterator0();
				_003C_003Ec__Iterator._0012 = -2;
				return _003C_003Ec__Iterator;
			}
		}

		public IEnumerable<JSONNode> getter000A
		{
			get
			{
				_003C_003Ec__Iterator1 _003C_003Ec__Iterator = new _003C_003Ec__Iterator1();
				_003C_003Ec__Iterator._0016 = this;
				_003C_003Ec__Iterator._0009 = -2;
				return _003C_003Ec__Iterator;
			}
		}

		public virtual int _000B
		{
			get
			{
				int result = 0;
				if (int.TryParse(_0009, out result))
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
							return result;
						}
					}
				}
				return 0;
			}
			set
			{
				_0009 = value.ToString();
			}
		}

		public virtual float _0003
		{
			get
			{
				float result = 0f;
				if (float.TryParse(_0009, out result))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							return result;
						}
					}
				}
				return 0f;
			}
			set
			{
				_0009 = value.ToString();
			}
		}

		public virtual double _000F
		{
			get
			{
				double result = 0.0;
				if (double.TryParse(_0009, out result))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							return result;
						}
					}
				}
				return 0.0;
			}
			set
			{
				_0009 = value.ToString();
			}
		}

		public virtual bool _0017
		{
			get
			{
				bool result = false;
				if (bool.TryParse(_0009, out result))
				{
					return result;
				}
				return !string.IsNullOrEmpty(_0009);
			}
			set
			{
				_0009 = ((!value) ? "false" : "true");
			}
		}

		public virtual JSONArray _000D => this as JSONArray;

		public virtual JSONClass _0008 => this as JSONClass;

		public virtual void _0013(string _001D, JSONNode _000E)
		{
		}

		public virtual void _0013(JSONNode _001D)
		{
			_0013(string.Empty, _001D);
		}

		public virtual JSONNode _0011(string _001D)
		{
			return null;
		}

		public virtual JSONNode _0011(int _001D)
		{
			return null;
		}

		public virtual JSONNode _0011(JSONNode _001D)
		{
			return _001D;
		}

		public override string ToString()
		{
			return "JSONNode";
		}

		public virtual string _0004(string _001D)
		{
			return "JSONNode";
		}

		public static JSONNode JSONData(string _001D)
		{
			return new JSONData(_001D);
		}

		public static string toString_zq(JSONNode _001D)
		{
			object result;
			if (_000A(_001D, null))
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
				result = null;
			}
			else
			{
				result = _001D._0009;
			}
			return (string)result;
		}

		public static bool _000A(JSONNode _001D, object _000E)
		{
			if (_000E == null)
			{
				while (true)
				{
					switch (5)
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
				if (_001D is JSONLazyCreator)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
			return object.ReferenceEquals(_001D, _000E);
		}

		public static bool _0006(JSONNode _001D, object _000E)
		{
			return !_000A(_001D, _000E);
		}

		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		internal static string escapeString(string _001D)
		{
			string text = string.Empty;
			foreach (char c in _001D)
			{
				switch (c)
				{
				case '"':
					text += "\\\"";
					continue;
				case '\n':
					text += "\\n";
					continue;
				case '\r':
					text += "\\r";
					continue;
				case '\t':
					text += "\\t";
					continue;
				case '\b':
					text += "\\b";
					continue;
				case '\f':
					text += "\\f";
					continue;
				}
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
				if (c != '\\')
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					text += c;
				}
				else
				{
					text += "\\\\";
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				return text;
			}
		}

		public static JSONNode _0020(string _001D)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jSONNode = null;
			int i = 0;
			string text = string.Empty;
			string text2 = string.Empty;
			bool flag = false;
			for (; i < _001D.Length; i++)
			{
				char c = _001D[i];
				switch (c)
				{
				case '{':
					if (flag)
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
						text += _001D[i];
						continue;
					}
					stack.Push(new JSONClass());
					if (_0006(jSONNode, null))
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
						text2 = text2.Trim();
						if (jSONNode is JSONArray)
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
							jSONNode._0013(stack.Peek());
						}
						else if (text2 != string.Empty)
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
							jSONNode._0013(text2, stack.Peek());
						}
					}
					text2 = string.Empty;
					text = string.Empty;
					jSONNode = stack.Peek();
					continue;
				case '[':
					if (flag)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						text += _001D[i];
						continue;
					}
					stack.Push(new JSONArray());
					if (_0006(jSONNode, null))
					{
						text2 = text2.Trim();
						if (jSONNode is JSONArray)
						{
							jSONNode._0013(stack.Peek());
						}
						else if (text2 != string.Empty)
						{
							jSONNode._0013(text2, stack.Peek());
						}
					}
					text2 = string.Empty;
					text = string.Empty;
					jSONNode = stack.Peek();
					continue;
				case ']':
				case '}':
					if (flag)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						text += _001D[i];
						continue;
					}
					if (stack.Count == 0)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								throw new Exception("JSON Parse: Too many closing brackets");
							}
						}
					}
					stack.Pop();
					if (text != string.Empty)
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
						text2 = text2.Trim();
						if (jSONNode is JSONArray)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							jSONNode._0013(JSONData(text));
						}
						else if (text2 != string.Empty)
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
							jSONNode._0013(text2, JSONData(text));
						}
					}
					text2 = string.Empty;
					text = string.Empty;
					if (stack.Count > 0)
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
						jSONNode = stack.Peek();
					}
					continue;
				case '"':
					flag = ((byte)((flag ? 1 : 0) ^ 1) != 0);
					continue;
				case ',':
					if (flag)
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
						text += _001D[i];
						continue;
					}
					if (text != string.Empty)
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
						if (jSONNode is JSONArray)
						{
							jSONNode._0013(JSONData(text));
						}
						else if (text2 != string.Empty)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							jSONNode._0013(text2, JSONData(text));
						}
					}
					text2 = string.Empty;
					text = string.Empty;
					continue;
				case '\t':
				case ' ':
					if (flag)
					{
						text += _001D[i];
					}
					continue;
				case '\\':
				{
					i++;
					if (!flag)
					{
						continue;
					}
					char c2 = _001D[i];
					switch (c2)
					{
					default:
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (c2 != 'n')
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
							text += c2;
						}
						else
						{
							text += '\n';
						}
						break;
					case 't':
						text += '\t';
						break;
					case 'r':
						text += '\r';
						break;
					case 'b':
						text += '\b';
						break;
					case 'f':
						text += '\f';
						break;
					case 'u':
					{
						string s = _001D.Substring(i + 1, 4);
						text += (char)int.Parse(s, NumberStyles.AllowHexSpecifier);
						i += 4;
						break;
					}
					}
					continue;
				}
				case '\n':
				case '\r':
					continue;
				}
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
				if (c == ':')
				{
					if (flag)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						text += _001D[i];
					}
					else
					{
						text2 = text;
						text = string.Empty;
					}
				}
				else
				{
					text += _001D[i];
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (flag)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
						}
					}
				}
				return jSONNode;
			}
		}

		public virtual void _0002(BinaryWriter _001D)
		{
		}

		public void write(Stream _001D)
		{
			BinaryWriter binaryWriter = new BinaryWriter(_001D);
			_0002(binaryWriter);
		}

		public void compressed0006(Stream _001D)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public void compressed000A(string _001D)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public string _000A()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public void writeToFile(string path)
		{
			Directory.CreateDirectory(new FileInfo(path).Directory.FullName);
			using (FileStream fileStream = File.OpenWrite(path))
			{
				write(fileStream);
			}
		}

		public string writeToString()
		{
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				write(memoryStream);
				memoryStream.Position = 0L;
				return Convert.ToBase64String(memoryStream.ToArray());
			}
			finally
			{
				if (memoryStream != null)
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
							((IDisposable)memoryStream).Dispose();
							goto end_IL_0025;
						}
					}
				}
				end_IL_0025:;
			}
		}

		public static JSONNode _000A(BinaryReader _001D)
		{
			JSONBinaryTag jSONBinaryTag = (JSONBinaryTag)_001D.ReadByte();
			switch (jSONBinaryTag)
			{
			case JSONBinaryTag._001D:
			{
				int num2 = _001D.ReadInt32();
				JSONArray jSONArray = new JSONArray();
				for (int j = 0; j < num2; j++)
				{
					jSONArray._0013(_000A(_001D));
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return jSONArray;
				}
			}
			case JSONBinaryTag._000E:
			{
				int num = _001D.ReadInt32();
				JSONClass jSONClass = new JSONClass();
				for (int i = 0; i < num; i++)
				{
					string text = _001D.ReadString();
					JSONNode jSONNode = _000A(_001D);
					jSONClass._0013(text, jSONNode);
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					return jSONClass;
				}
			}
			case JSONBinaryTag._0012:
				return new JSONData(_001D.ReadString());
			case JSONBinaryTag._0015:
				return new JSONData(_001D.ReadInt32());
			case JSONBinaryTag._0016:
				return new JSONData(_001D.ReadDouble());
			case JSONBinaryTag._0013:
				return new JSONData(_001D.ReadBoolean());
			case JSONBinaryTag._0018:
				return new JSONData(_001D.ReadSingle());
			default:
				throw new Exception("Error deserializing JSON. Unknown tag: " + jSONBinaryTag);
			}
		}

		public static JSONNode _000C(string _001D)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode _000A(Stream _001D)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode _0014(string _001D)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode _0006(Stream _001D)
		{
			BinaryReader binaryReader = new BinaryReader(_001D);
			try
			{
				return _000A(binaryReader);
			}
			finally
			{
				if (binaryReader != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							((IDisposable)binaryReader).Dispose();
							goto end_IL_0012;
						}
					}
				}
				end_IL_0012:;
			}
		}

		public static JSONNode _0005(string _001D)
		{
			using (FileStream fileStream = File.OpenRead(_001D))
			{
				return _0006(fileStream);
			}
		}

		public static JSONNode _001B(string _001D)
		{
			byte[] buffer = Convert.FromBase64String(_001D);
			MemoryStream memoryStream = new MemoryStream(buffer);
			memoryStream.Position = 0L;
			return _0006(memoryStream);
		}
	}
}
