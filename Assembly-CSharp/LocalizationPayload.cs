using NetSerializer;
using System;
using System.Collections.Generic;
using System.IO;

[Serializable]
public class LocalizationPayload
{
	public string Term = "unset";

	public string Context;

	public byte[][] ArgumentsAsBinaryData;

	[NonSerialized]
	private static Serializer s_serializer;

	public static Serializer Serializer
	{
		get
		{
			if (s_serializer == null)
			{
				s_serializer = new Serializer();
				Type[] rootTypes = new Type[12]
				{
					typeof(LocalizationArg_AccessLevel),
					typeof(LocalizationArg_BroadcastMessage),
					typeof(LocalizationArg_ChatChannel),
					typeof(LocalizationArg_Faction),
					typeof(LocalizationArg_FirstRank),
					typeof(LocalizationArg_FirstType),
					typeof(LocalizationArg_Freelancer),
					typeof(LocalizationArg_GameType),
					typeof(LocalizationArg_Handle),
					typeof(LocalizationArg_Int32),
					typeof(LocalizationArg_LocalizationPayload),
					typeof(LocalizationArg_TimeSpan)
				};
				s_serializer.AddTypes(rootTypes);
			}
			return s_serializer;
		}
	}

	private List<LocalizationArg> ExtractArguments()
	{
		List<LocalizationArg> list = null;
		try
		{
			if (!ArgumentsAsBinaryData.IsNullOrEmpty())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						list = new List<LocalizationArg>();
						byte[][] argumentsAsBinaryData = ArgumentsAsBinaryData;
						foreach (byte[] buffer in argumentsAsBinaryData)
						{
							MemoryStream stream = new MemoryStream(buffer);
							Serializer.Deserialize(stream, out object ob);
							if (ob != null)
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
								if (ob is LocalizationArg)
								{
									list.Add(ob as LocalizationArg);
								}
							}
						}
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								return list;
							}
						}
					}
					}
				}
			}
			return list;
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
			return list;
		}
	}

	public override string ToString()
	{
		string text = StringUtil.TR(Term, Context);
		List<LocalizationArg> list = ExtractArguments();
		if (list.IsNullOrEmpty())
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
					return text;
				}
			}
		}
		if (list.Count == 1)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return string.Format(text, list[0].TR());
				}
			}
		}
		if (list.Count == 2)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return string.Format(text, list[0].TR(), list[1].TR());
				}
			}
		}
		if (list.Count == 3)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return string.Format(text, list[0].TR(), list[1].TR(), list[2].TR());
				}
			}
		}
		if (list.Count > 4)
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
			Log.Error("We do not support more than four arguments to localized payloads: {0}@{1}", Term, Context);
		}
		return string.Format(text, list[0].TR(), list[1].TR(), list[2].TR(), list[3].TR());
	}

	public string ToDebugString()
	{
		return ToString();
	}

	public static LocalizationPayload Create(string attedLocIdentifier)
	{
		string[] array = attedLocIdentifier.Split("@".ToCharArray(), 2);
		if (array.Length == 2)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					LocalizationPayload localizationPayload = new LocalizationPayload();
					localizationPayload.Term = array[0];
					localizationPayload.Context = array[1];
					return localizationPayload;
				}
				}
			}
		}
		throw new Exception($"Bad argument ({attedLocIdentifier}) to LocalizationPayload, expected string with an @ in it.");
	}

	public static LocalizationPayload Create(string term, string context)
	{
		LocalizationPayload localizationPayload = new LocalizationPayload();
		localizationPayload.Term = term;
		localizationPayload.Context = context;
		return localizationPayload;
	}

	public static LocalizationPayload Create(string term, string context, params LocalizationArg[] arguments)
	{
		List<byte[]> list = null;
		if (!arguments.IsNullOrEmpty())
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
			list = new List<byte[]>();
			foreach (LocalizationArg ob in arguments)
			{
				MemoryStream memoryStream = new MemoryStream();
				Serializer.Serialize(memoryStream, ob);
				list.Add(memoryStream.ToArray());
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
		LocalizationPayload localizationPayload = new LocalizationPayload();
		localizationPayload.Term = term;
		localizationPayload.Context = context;
		localizationPayload.ArgumentsAsBinaryData = list.ToArray();
		return localizationPayload;
	}
}
