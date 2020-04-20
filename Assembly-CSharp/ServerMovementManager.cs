using System;
using UnityEngine.Networking;

public class ServerMovementManager : NetworkBehaviour
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

	public enum MovementType
	{
		None,
		Evade,
		Knockback,
		NormalMovement_NonChase,
		NormalMovement_Chase
	}
}
