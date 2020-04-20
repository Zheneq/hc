using System;
using UnityEngine.Networking;

public class ConnectingPlayer : NetworkBehaviour
{
	public override void OnStartLocalPlayer()
	{
		if (NetworkServer.active)
		{
			NetworkIdentity component = base.GetComponent<NetworkIdentity>();
			component.RebuildObservers(true);
		}
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
