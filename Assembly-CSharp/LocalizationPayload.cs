using System;
using System.Collections.Generic;
using System.IO;
using NetSerializer;

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
			if (LocalizationPayload.s_serializer == null)
			{
				LocalizationPayload.s_serializer = new Serializer();
				Type[] rootTypes = new Type[]
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
				LocalizationPayload.s_serializer.AddTypes(rootTypes);
			}
			return LocalizationPayload.s_serializer;
		}
	}

	private List<LocalizationArg> ExtractArguments()
	{
		List<LocalizationArg> list = null;
		try
		{
			if (!this.ArgumentsAsBinaryData.IsNullOrEmpty<byte[]>())
			{
				list = new List<LocalizationArg>();
				foreach (byte[] buffer in this.ArgumentsAsBinaryData)
				{
					MemoryStream stream = new MemoryStream(buffer);
					object obj;
					LocalizationPayload.Serializer.Deserialize(stream, out obj);
					if (obj != null)
					{
						if (obj is LocalizationArg)
						{
							list.Add(obj as LocalizationArg);
						}
					}
				}
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		return list;
	}

	public override string ToString()
	{
		string text = StringUtil.TR(this.Term, this.Context);
		List<LocalizationArg> list = this.ExtractArguments();
		if (list.IsNullOrEmpty<LocalizationArg>())
		{
			return text;
		}
		if (list.Count == 1)
		{
			return string.Format(text, list[0].TR());
		}
		if (list.Count == 2)
		{
			return string.Format(text, list[0].TR(), list[1].TR());
		}
		if (list.Count == 3)
		{
			return string.Format(text, list[0].TR(), list[1].TR(), list[2].TR());
		}
		if (list.Count > 4)
		{
			Log.Error("We do not support more than four arguments to localized payloads: {0}@{1}", new object[]
			{
				this.Term,
				this.Context
			});
		}
		return string.Format(text, new object[]
		{
			list[0].TR(),
			list[1].TR(),
			list[2].TR(),
			list[3].TR()
		});
	}

	public string ToDebugString()
	{
		return this.ToString();
	}

	public static LocalizationPayload Create(string attedLocIdentifier)
	{
		string[] array = attedLocIdentifier.Split("@".ToCharArray(), 2);
		if (array.Length == 2)
		{
			return new LocalizationPayload
			{
				Term = array[0],
				Context = array[1]
			};
		}
		throw new Exception(string.Format("Bad argument ({0}) to LocalizationPayload, expected string with an @ in it.", attedLocIdentifier));
	}

	public static LocalizationPayload Create(string term, string context)
	{
		return new LocalizationPayload
		{
			Term = term,
			Context = context
		};
	}

	public static LocalizationPayload Create(string term, string context, params LocalizationArg[] arguments)
	{
		List<byte[]> list = null;
		if (!arguments.IsNullOrEmpty<LocalizationArg>())
		{
			list = new List<byte[]>();
			foreach (LocalizationArg ob in arguments)
			{
				MemoryStream memoryStream = new MemoryStream();
				LocalizationPayload.Serializer.Serialize(memoryStream, ob);
				list.Add(memoryStream.ToArray());
			}
		}
		return new LocalizationPayload
		{
			Term = term,
			Context = context,
			ArgumentsAsBinaryData = list.ToArray()
		};
	}
}
