using UnityEngine.Networking;

public class ServerMovementManager : NetworkBehaviour
{
	public enum MovementType
	{
		None,
		Evade,
		Knockback,
		NormalMovement_NonChase,
		NormalMovement_Chase
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
