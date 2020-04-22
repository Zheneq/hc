using UnityEngine.Networking;

public class ConnectingPlayer : NetworkBehaviour
{
	public override void OnStartLocalPlayer()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			NetworkIdentity component = GetComponent<NetworkIdentity>();
			component.RebuildObservers(true);
			return;
		}
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
