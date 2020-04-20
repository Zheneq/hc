using System;
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
			Log.Error("Component is null.", new object[0]);
		}
		else if (c.gameObject == null)
		{
			Log.Error(c.ToString() + " '" + c.name + "' has a null gameObject", new object[0]);
		}
		else
		{
			NetworkView[] components = c.gameObject.GetComponents<NetworkView>();
			if (components != null)
			{
				if (components.Length > 0)
				{
					Log.Error(string.Concat(new object[]
					{
						c.GetType(),
						" in ",
						legiblePath,
						" needs a NetworkIdentity but has an old NetworkView."
					}), new object[0]);
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
			object[] array = new object[5];
			array[0] = c.gameObject;
			array[1] = " in ";
			array[2] = legiblePath;
			array[3] = " needs exactly one NetworkIdentity, but has ";
			int num = 4;
			int num2;
			if (components2 == null)
			{
				num2 = 0;
			}
			else
			{
				num2 = components2.Length;
			}
			array[num] = num2;
			Log.Error(string.Concat(array), new object[0]);
		}
	}

	[Conditional("HYDROGEN_DEBUG")]
	public static void CheckNetworkedObject(Component c, bool _unused = false)
	{
		if (c == null)
		{
			Log.Error("Component is null.", new object[0]);
		}
		else if (c.gameObject == null)
		{
			Log.Error(c.ToString() + " '" + c.name + "' has a null gameObject", new object[0]);
		}
	}

	internal static void CheckOnSerialize(string componentToString, string gameObjectToString, short writerPositionBeforeSerialize, short writerPositionAfterSerialize, int channelIndex, NetworkWriter writer)
	{
		if (componentToString == null)
		{
			Log.Error("null argument", new object[0]);
		}
		else if (writerPositionBeforeSerialize < 0)
		{
			Log.Error("net serialization writer position is negative and may have overflowed: {0} for {1}", new object[]
			{
				writerPositionBeforeSerialize,
				componentToString
			});
		}
		else if (writerPositionAfterSerialize < 0)
		{
			Log.Error("net serialization writer position is negative and may have overflowed: {0} for {1}", new object[]
			{
				writerPositionBeforeSerialize,
				componentToString
			});
		}
		else if (writerPositionAfterSerialize > 0x400)
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
				Log.Error("Too much net serialization data on object {0} for channel {1}.\nObject sends at least {2} byte packet, max allowed is {3}.\nScript \"{4}\" wrote {5} bytes.\nSend less, or change sending script to use another channel.", new object[]
				{
					gameObjectToString,
					channelIndex,
					writerPositionAfterSerialize,
					0x400,
					componentToString,
					(int)(writerPositionAfterSerialize - writerPositionBeforeSerialize)
				});
			}
		}
	}
}
