using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;

public static class ErrorCheck
{
	[Conditional("HYDROGEN_DEBUG")]
	public static void CheckNetworkedObject(Component c, string legiblePath, bool _unused = false)
	{
		if (c == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Log.Error("Component is null.");
					return;
				}
			}
		}
		if (c.gameObject == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Log.Error(c.ToString() + " '" + c.name + "' has a null gameObject");
					return;
				}
			}
		}
		NetworkView[] components = c.gameObject.GetComponents<NetworkView>();
		if (components != null)
		{
			if (components.Length > 0)
			{
				Log.Error(string.Concat(c.GetType(), " in ", legiblePath, " needs a NetworkIdentity but has an old NetworkView."));
			}
		}
		NetworkIdentity[] components2 = c.GetComponents<NetworkIdentity>();
		if (components2 != null)
		{
			if (components2.Length == 1)
			{
				return;
			}
		}
		object[] obj = new object[5]
		{
			c.gameObject,
			" in ",
			legiblePath,
			" needs exactly one NetworkIdentity, but has ",
			null
		};
		int num;
		if (components2 == null)
		{
			num = 0;
		}
		else
		{
			num = components2.Length;
		}
		obj[4] = num;
		Log.Error(string.Concat(obj));
	}

	[Conditional("HYDROGEN_DEBUG")]
	public static void CheckNetworkedObject(Component c, bool _unused = false)
	{
		if (c == null)
		{
			Log.Error("Component is null.");
		}
		else if (c.gameObject == null)
		{
			Log.Error(c.ToString() + " '" + c.name + "' has a null gameObject");
		}
	}

	internal static void CheckOnSerialize(string componentToString, string gameObjectToString, short writerPositionBeforeSerialize, short writerPositionAfterSerialize, int channelIndex, NetworkWriter writer)
	{
		if (componentToString == null)
		{
			Log.Error("null argument");
			return;
		}
		if (writerPositionBeforeSerialize < 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Log.Error("net serialization writer position is negative and may have overflowed: {0} for {1}", writerPositionBeforeSerialize, componentToString);
					return;
				}
			}
		}
		if (writerPositionAfterSerialize < 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Log.Error("net serialization writer position is negative and may have overflowed: {0} for {1}", writerPositionBeforeSerialize, componentToString);
					return;
				}
			}
		}
		if (writerPositionAfterSerialize <= 1024)
		{
			return;
		}
		while (true)
		{
			bool flag = true;
			if (flag)
			{
				QosType qosType = NetworkManager.singleton.channels[channelIndex];
				if (qosType != QosType.ReliableFragmented && qosType != QosType.UnreliableFragmented)
				{
					flag = false;
				}
			}
			if (!flag)
			{
				while (true)
				{
					Log.Error("Too much net serialization data on object {0} for channel {1}.\nObject sends at least {2} byte packet, max allowed is {3}.\nScript \"{4}\" wrote {5} bytes.\nSend less, or change sending script to use another channel.", gameObjectToString, channelIndex, writerPositionAfterSerialize, (short)1024, componentToString, writerPositionAfterSerialize - writerPositionBeforeSerialize);
					return;
				}
			}
			return;
		}
	}
}
