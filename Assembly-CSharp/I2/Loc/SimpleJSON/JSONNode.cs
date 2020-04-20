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
		public virtual void \u0013(string \u001D, JSONNode \u000E)
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

		public virtual string \u0009
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		public virtual int \u0019
		{
			get
			{
				return 0;
			}
		}

		public virtual void \u0013(JSONNode \u001D)
		{
			this.\u0013(string.Empty, \u001D);
		}

		public virtual JSONNode \u0011(string \u001D)
		{
			return null;
		}

		public virtual JSONNode \u0011(int \u001D)
		{
			return null;
		}

		public virtual JSONNode \u0011(JSONNode \u001D)
		{
			return \u001D;
		}

		public virtual IEnumerable<JSONNode> \u001A
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
				JSONNode.<>c__Iterator1 <>c__Iterator = new JSONNode.<>c__Iterator1();
				<>c__Iterator.\u0016 = this;
				JSONNode.<>c__Iterator1 <>c__Iterator2 = <>c__Iterator;
				<>c__Iterator2.\u0009 = -2;
				return <>c__Iterator2;
			}
		}

		public override string ToString()
		{
			return "JSONNode";
		}

		public virtual string \u0004(string \u001D)
		{
			return "JSONNode";
		}

		public virtual int \u000B
		{
			get
			{
				int result = 0;
				if (int.TryParse(this.\u0009, out result))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(JSONNode.get_\u000B()).MethodHandle;
					}
					return result;
				}
				return 0;
			}
			set
			{
				this.\u0009 = value.ToString();
			}
		}

		public virtual float \u0003
		{
			get
			{
				float result = 0f;
				if (float.TryParse(this.\u0009, out result))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(JSONNode.get_\u0003()).MethodHandle;
					}
					return result;
				}
				return 0f;
			}
			set
			{
				this.\u0009 = value.ToString();
			}
		}

		public virtual double \u000F
		{
			get
			{
				double result = 0.0;
				if (double.TryParse(this.\u0009, out result))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(JSONNode.get_\u000F()).MethodHandle;
					}
					return result;
				}
				return 0.0;
			}
			set
			{
				this.\u0009 = value.ToString();
			}
		}

		public virtual bool \u0017
		{
			get
			{
				bool result = false;
				if (bool.TryParse(this.\u0009, out result))
				{
					return result;
				}
				return !string.IsNullOrEmpty(this.\u0009);
			}
			set
			{
				this.\u0009 = ((!value) ? "false" : "true");
			}
		}

		public virtual JSONArray \u000D
		{
			get
			{
				return this as JSONArray;
			}
		}

		public virtual JSONClass \u0008
		{
			get
			{
				return this as JSONClass;
			}
		}

		public static JSONNode JSONData(string \u001D)
		{
			return new JSONData(\u001D);
		}

		public static string toString_zq(JSONNode \u001D)
		{
			string result;
			if (JSONNode.\u000A(\u001D, null))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(JSONNode.toString_zq(JSONNode)).MethodHandle;
				}
				result = null;
			}
			else
			{
				result = \u001D.\u0009;
			}
			return result;
		}

		public static bool \u000A(JSONNode \u001D, object \u000E)
		{
			if (\u000E == null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(JSONNode.\u000A(JSONNode, object)).MethodHandle;
				}
				if (\u001D is JSONLazyCreator)
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
					return true;
				}
			}
			return object.ReferenceEquals(\u001D, \u000E);
		}

		public static bool \u0006(JSONNode \u001D, object \u000E)
		{
			return !JSONNode.\u000A(\u001D, \u000E);
		}

		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		internal static string escapeString(string \u001D)
		{
			string text = string.Empty;
			foreach (char c in \u001D)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(JSONNode.escapeString(string)).MethodHandle;
						}
						if (c != '\\')
						{
							for (;;)
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			return text;
		}

		public static JSONNode \u0020(string \u001D)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jsonnode = null;
			int i = 0;
			string text = string.Empty;
			string text2 = string.Empty;
			bool flag = false;
			while (i < \u001D.Length)
			{
				char c = \u001D[i];
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
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							text += \u001D[i];
							goto IL_4FD;
						}
						stack.Push(new JSONArray());
						if (JSONNode.\u0006(jsonnode, null))
						{
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
							{
								jsonnode.\u0013(stack.Peek());
							}
							else if (text2 != string.Empty)
							{
								jsonnode.\u0013(text2, stack.Peek());
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
							char c2 = \u001D[i];
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
										for (;;)
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
											for (;;)
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
								string s = \u001D.Substring(i + 1, 4);
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
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									text += \u001D[i];
									goto IL_4FD;
								}
								stack.Push(new JSONClass());
								if (JSONNode.\u0006(jsonnode, null))
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
									text2 = text2.Trim();
									if (jsonnode is JSONArray)
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
										jsonnode.\u0013(stack.Peek());
									}
									else if (text2 != string.Empty)
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
										jsonnode.\u0013(text2, stack.Peek());
									}
								}
								text2 = string.Empty;
								text = string.Empty;
								jsonnode = stack.Peek();
								goto IL_4FD;
							default:
								if (c != ',')
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
										RuntimeMethodHandle runtimeMethodHandle = methodof(JSONNode.\u0020(string)).MethodHandle;
									}
									if (c != ':')
									{
										text += \u001D[i];
										goto IL_4FD;
									}
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
										text += \u001D[i];
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
										for (;;)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
										text += \u001D[i];
										goto IL_4FD;
									}
									if (text != string.Empty)
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
										if (jsonnode is JSONArray)
										{
											jsonnode.\u0013(JSONNode.JSONData(text));
										}
										else if (text2 != string.Empty)
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
											jsonnode.\u0013(text2, JSONNode.JSONData(text));
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
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						text += \u001D[i];
					}
					else
					{
						if (stack.Count == 0)
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
							throw new Exception("JSON Parse: Too many closing brackets");
						}
						stack.Pop();
						if (text != string.Empty)
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
							text2 = text2.Trim();
							if (jsonnode is JSONArray)
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
								jsonnode.\u0013(JSONNode.JSONData(text));
							}
							else if (text2 != string.Empty)
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
								jsonnode.\u0013(text2, JSONNode.JSONData(text));
							}
						}
						text2 = string.Empty;
						text = string.Empty;
						if (stack.Count > 0)
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
					text += \u001D[i];
				}
				goto IL_4FD;
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
			if (flag)
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
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jsonnode;
		}

		public virtual void \u0002(BinaryWriter \u001D)
		{
		}

		public void write(Stream \u001D)
		{
			BinaryWriter u001D = new BinaryWriter(\u001D);
			this.\u0002(u001D);
		}

		public void compressed0006(Stream \u001D)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public void compressed000A(string \u001D)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public string \u000A()
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(JSONNode.writeToString()).MethodHandle;
					}
					((IDisposable)memoryStream).Dispose();
				}
			}
			return result;
		}

		public static JSONNode \u000A(BinaryReader \u001D)
		{
			JSONBinaryTag jsonbinaryTag = (JSONBinaryTag)\u001D.ReadByte();
			switch (jsonbinaryTag)
			{
			case JSONBinaryTag.\u001D:
			{
				int num = \u001D.ReadInt32();
				JSONArray jsonarray = new JSONArray();
				for (int i = 0; i < num; i++)
				{
					jsonarray.\u0013(JSONNode.\u000A(\u001D));
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(JSONNode.\u000A(BinaryReader)).MethodHandle;
				}
				return jsonarray;
			}
			case JSONBinaryTag.\u000E:
			{
				int num2 = \u001D.ReadInt32();
				JSONClass jsonclass = new JSONClass();
				for (int j = 0; j < num2; j++)
				{
					string u001D = \u001D.ReadString();
					JSONNode u000E = JSONNode.\u000A(\u001D);
					jsonclass.\u0013(u001D, u000E);
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
				return jsonclass;
			}
			case JSONBinaryTag.\u0012:
				return new JSONData(\u001D.ReadString());
			case JSONBinaryTag.\u0015:
				return new JSONData(\u001D.ReadInt32());
			case JSONBinaryTag.\u0016:
				return new JSONData(\u001D.ReadDouble());
			case JSONBinaryTag.\u0013:
				return new JSONData(\u001D.ReadBoolean());
			case JSONBinaryTag.\u0018:
				return new JSONData(\u001D.ReadSingle());
			default:
				throw new Exception("Error deserializing JSON. Unknown tag: " + jsonbinaryTag);
			}
		}

		public static JSONNode \u000C(string \u001D)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode \u000A(Stream \u001D)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode \u0014(string \u001D)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode \u0006(Stream \u001D)
		{
			BinaryReader binaryReader = new BinaryReader(\u001D);
			JSONNode result;
			try
			{
				result = JSONNode.\u000A(binaryReader);
			}
			finally
			{
				if (binaryReader != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(JSONNode.\u0006(Stream)).MethodHandle;
					}
					((IDisposable)binaryReader).Dispose();
				}
			}
			return result;
		}

		public static JSONNode \u0005(string \u001D)
		{
			JSONNode result;
			using (FileStream fileStream = File.OpenRead(\u001D))
			{
				result = JSONNode.\u0006(fileStream);
			}
			return result;
		}

		public static JSONNode \u001B(string \u001D)
		{
			byte[] buffer = Convert.FromBase64String(\u001D);
			return JSONNode.\u0006(new MemoryStream(buffer)
			{
				Position = 0L
			});
		}
	}
}
