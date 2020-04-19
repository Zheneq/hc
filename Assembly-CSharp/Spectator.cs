using System;
using UnityEngine.Networking;

public class Spectator : NetworkBehaviour
{
	public override void OnStartClient()
	{
		base.OnStartClient();
	}

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
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
