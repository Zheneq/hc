using System;
using UnityEngine.Networking;

public class CardDeck : NetworkBehaviour
{
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
