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
		public virtual void symbol_0013(string symbol_001D, JSONNode symbol_000E)
		{
		}

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

		public virtual string symbol_0009
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		public virtual int symbol_0019
		{
			get
			{
				return 0;
			}
		}

		public virtual void symbol_0013(JSONNode symbol_001D)
		{
			this.symbol_0013(string.Empty, symbol_001D);
		}

		public virtual JSONNode symbol_0011(string symbol_001D)
		{
			return null;
		}

		public virtual JSONNode symbol_0011(int symbol_001D)
		{
			return null;
		}

		public virtual JSONNode symbol_0011(JSONNode symbol_001D)
		{
			return symbol_001D;
		}

		public virtual IEnumerable<JSONNode> symbol_001A
		{
			get
			{
				yield break;
			}
		}

		public IEnumerable<JSONNode> getter000A
		{
			get
			{
				JSONNode._c__Iterator1 _c__Iterator = new JSONNode._c__Iterator1();
				_c__Iterator.symbol_0016 = this;
				JSONNode._c__Iterator1 _c__Iterator2 = _c__Iterator;
				_c__Iterator2.symbol_0009 = -2;
				return _c__Iterator2;
			}
		}

		public override string ToString()
		{
			return "JSONNode";
		}

		public virtual string symbol_0004(string symbol_001D)
		{
			return "JSONNode";
		}

		public virtual int symbol_000B
		{
			get
			{
				int result = 0;
				if (int.TryParse(this.symbol_0009, out result))
				{
					return result;
				}
				return 0;
			}
			set
			{
				this.symbol_0009 = value.ToString();
			}
		}

		public virtual float symbol_0003
		{
			get
			{
				float result = 0f;
				if (float.TryParse(this.symbol_0009, out result))
				{
					return result;
				}
				return 0f;
			}
			set
			{
				this.symbol_0009 = value.ToString();
			}
		}

		public virtual double symbol_000F
		{
			get
			{
				double result = 0.0;
				if (double.TryParse(this.symbol_0009, out result))
				{
					return result;
				}
				return 0.0;
			}
			set
			{
				this.symbol_0009 = value.ToString();
			}
		}

		public virtual bool symbol_0017
		{
			get
			{
				bool result = false;
				if (bool.TryParse(this.symbol_0009, out result))
				{
					return result;
				}
				return !string.IsNullOrEmpty(this.symbol_0009);
			}
			set
			{
				this.symbol_0009 = ((!value) ? "false" : "true");
			}
		}

		public virtual JSONArray symbol_000D
		{
			get
			{
				return this as JSONArray;
			}
		}

		public virtual JSONClass symbol_0008
		{
			get
			{
				return this as JSONClass;
			}
		}

		public static JSONNode JSONData(string symbol_001D)
		{
			return new JSONData(symbol_001D);
		}

		public static string toString_zq(JSONNode symbol_001D)
		{
			string result;
			if (JSONNode.symbol_000A(symbol_001D, null))
			{
				result = null;
			}
			else
			{
				result = symbol_001D.symbol_0009;
			}
			return result;
		}

		public static bool symbol_000A(JSONNode symbol_001D, object symbol_000E)
		{
			if (symbol_000E == null)
			{
				if (symbol_001D is JSONLazyCreator)
				{
					return true;
				}
			}
			return object.ReferenceEquals(symbol_001D, symbol_000E);
		}

		public static bool symbol_0006(JSONNode symbol_001D, object symbol_000E)
		{
			return !JSONNode.symbol_000A(symbol_001D, symbol_000E);
		}

		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		internal static string escapeString(string symbol_001D)
		{
			string text = string.Empty;
			foreach (char c in symbol_001D)
			{
				switch (c)
				{
				case '\b':
					text += "\\b";
					break;
				case '\t':
					text += "\\t";
					break;
				case '\n':
					text += "\\n";
					break;
				default:
					if (c != '"')
					{
						if (c != '\\')
						{
							text += c;
						}
						else
						{
							text += "\\\\";
						}
					}
					else
					{
						text += "\\\"";
					}
					break;
				case '\f':
					text += "\\f";
					break;
				case '\r':
					text += "\\r";
					break;
				}
			}
			return text;
		}

		public static JSONNode symbol_0020(string symbol_001D)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jsonnode = null;
			int i = 0;
			string text = string.Empty;
			string text2 = string.Empty;
			bool flag = false;
			while (i < symbol_001D.Length)
			{
				char c = symbol_001D[i];
				switch (c)
				{
				case '\t':
					goto IL_3CB;
				case '\n':
				case '\r':
					break;
				default:
					switch (c)
					{
					case '[':
						if (flag)
						{
							text += symbol_001D[i];
							goto IL_4FD;
						}
						stack.Push(new JSONArray());
						if (JSONNode.symbol_0006(jsonnode, null))
						{
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
							{
								jsonnode.symbol_0013(stack.Peek());
							}
							else if (text2 != string.Empty)
							{
								jsonnode.symbol_0013(text2, stack.Peek());
							}
						}
						text2 = string.Empty;
						text = string.Empty;
						jsonnode = stack.Peek();
						goto IL_4FD;
					case '\\':
						i++;
						if (flag)
						{
							char c2 = symbol_001D[i];
							switch (c2)
							{
							case 'r':
								text += '\r';
								break;
							default:
								if (c2 != 'b')
								{
									if (c2 != 'f')
									{
										if (c2 != 'n')
										{
											text += c2;
										}
										else
										{
											text += '\n';
										}
									}
									else
									{
										text += '\f';
									}
								}
								else
								{
									text += '\b';
								}
								break;
							case 't':
								text += '\t';
								break;
							case 'u':
							{
								string s = symbol_001D.Substring(i + 1, 4);
								text += (char)int.Parse(s, NumberStyles.AllowHexSpecifier);
								i += 4;
								break;
							}
							}
						}
						goto IL_4FD;
					case ']':
						break;
					default:
						switch (c)
						{
						case ' ':
							goto IL_3CB;
						default:
							switch (c)
							{
							case '{':
								if (flag)
								{
									text += symbol_001D[i];
									goto IL_4FD;
								}
								stack.Push(new JSONClass());
								if (JSONNode.symbol_0006(jsonnode, null))
								{
									text2 = text2.Trim();
									if (jsonnode is JSONArray)
									{
										jsonnode.symbol_0013(stack.Peek());
									}
									else if (text2 != string.Empty)
									{
										jsonnode.symbol_0013(text2, stack.Peek());
									}
								}
								text2 = string.Empty;
								text = string.Empty;
								jsonnode = stack.Peek();
								goto IL_4FD;
							default:
								if (c != ',')
								{
									if (c != ':')
									{
										text += symbol_001D[i];
										goto IL_4FD;
									}
									if (flag)
									{
										text += symbol_001D[i];
										goto IL_4FD;
									}
									text2 = text;
									text = string.Empty;
									goto IL_4FD;
								}
								else
								{
									if (flag)
									{
										text += symbol_001D[i];
										goto IL_4FD;
									}
									if (text != string.Empty)
									{
										if (jsonnode is JSONArray)
										{
											jsonnode.symbol_0013(JSONNode.JSONData(text));
										}
										else if (text2 != string.Empty)
										{
											jsonnode.symbol_0013(text2, JSONNode.JSONData(text));
										}
									}
									text2 = string.Empty;
									text = string.Empty;
									goto IL_4FD;
								}
								break;
							case '}':
								break;
							}
							break;
						case '"':
							flag ^= true;
							goto IL_4FD;
						}
						break;
					}
					if (flag)
					{
						text += symbol_001D[i];
					}
					else
					{
						if (stack.Count == 0)
						{
							throw new Exception("JSON Parse: Too many closing brackets");
						}
						stack.Pop();
						if (text != string.Empty)
						{
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
							{
								jsonnode.symbol_0013(JSONNode.JSONData(text));
							}
							else if (text2 != string.Empty)
							{
								jsonnode.symbol_0013(text2, JSONNode.JSONData(text));
							}
						}
						text2 = string.Empty;
						text = string.Empty;
						if (stack.Count > 0)
						{
							jsonnode = stack.Peek();
						}
					}
					break;
				}
				IL_4FD:
				i++;
				continue;
				IL_3CB:
				if (flag)
				{
					text += symbol_001D[i];
				}
				goto IL_4FD;
			}
			if (flag)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jsonnode;
		}

		public virtual void symbol_0002(BinaryWriter symbol_001D)
		{
		}

		public void write(Stream symbol_001D)
		{
			BinaryWriter u001D = new BinaryWriter(symbol_001D);
			this.symbol_0002(u001D);
		}

		public void compressed0006(Stream symbol_001D)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public void compressed000A(string symbol_001D)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public string symbol_000A()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public void writeToFile(string path)
		{
			Directory.CreateDirectory(new FileInfo(path).Directory.FullName);
			using (FileStream fileStream = File.OpenWrite(path))
			{
				this.write(fileStream);
			}
		}

		public string writeToString()
		{
			MemoryStream memoryStream = new MemoryStream();
			string result;
			try
			{
				this.write(memoryStream);
				memoryStream.Position = 0L;
				result = Convert.ToBase64String(memoryStream.ToArray());
			}
			finally
			{
				if (memoryStream != null)
				{
					((IDisposable)memoryStream).Dispose();
				}
			}
			return result;
		}

		public static JSONNode symbol_000A(BinaryReader symbol_001D)
		{
			JSONBinaryTag jsonbinaryTag = (JSONBinaryTag)symbol_001D.ReadByte();
			switch (jsonbinaryTag)
			{
			case JSONBinaryTag.symbol_001D:
			{
				int num = symbol_001D.ReadInt32();
				JSONArray jsonarray = new JSONArray();
				for (int i = 0; i < num; i++)
				{
					jsonarray.symbol_0013(JSONNode.symbol_000A(symbol_001D));
				}
				return jsonarray;
			}
			case JSONBinaryTag.symbol_000E:
			{
				int num2 = symbol_001D.ReadInt32();
				JSONClass jsonclass = new JSONClass();
				for (int j = 0; j < num2; j++)
				{
					string u001D = symbol_001D.ReadString();
					JSONNode u000E = JSONNode.symbol_000A(symbol_001D);
					jsonclass.symbol_0013(u001D, u000E);
				}
				return jsonclass;
			}
			case JSONBinaryTag.symbol_0012:
				return new JSONData(symbol_001D.ReadString());
			case JSONBinaryTag.symbol_0015:
				return new JSONData(symbol_001D.ReadInt32());
			case JSONBinaryTag.symbol_0016:
				return new JSONData(symbol_001D.ReadDouble());
			case JSONBinaryTag.symbol_0013:
				return new JSONData(symbol_001D.ReadBoolean());
			case JSONBinaryTag.symbol_0018:
				return new JSONData(symbol_001D.ReadSingle());
			default:
				throw new Exception("Error deserializing JSON. Unknown tag: " + jsonbinaryTag);
			}
		}

		public static JSONNode symbol_000C(string symbol_001D)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode symbol_000A(Stream symbol_001D)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode symbol_0014(string symbol_001D)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode symbol_0006(Stream symbol_001D)
		{
			BinaryReader binaryReader = new BinaryReader(symbol_001D);
			JSONNode result;
			try
			{
				result = JSONNode.symbol_000A(binaryReader);
			}
			finally
			{
				if (binaryReader != null)
				{
					((IDisposable)binaryReader).Dispose();
				}
			}
			return result;
		}

		public static JSONNode symbol_0005(string symbol_001D)
		{
			JSONNode result;
			using (FileStream fileStream = File.OpenRead(symbol_001D))
			{
				result = JSONNode.symbol_0006(fileStream);
			}
			return result;
		}

		public static JSONNode symbol_001B(string symbol_001D)
		{
			byte[] buffer = Convert.FromBase64String(symbol_001D);
			return JSONNode.symbol_0006(new MemoryStream(buffer)
			{
				Position = 0L
			});
		}
	}
}
