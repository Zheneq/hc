using System;
using UnityEngine.Networking;

public class ConnectingPlayer : NetworkBehaviour
{
	public override void OnStartLocalPlayer()
	{
		if (NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ConnectingPlayer.OnStartLocalPlayer()).MethodHandle;
			}
			NetworkIdentity component = base.GetComponent<NetworkIdentity>();
			component.RebuildObservers(true);
		}
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result;
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
