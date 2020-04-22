using UnityEngine;
using UnityEngine.Networking;

public class ActorSkin : NetworkBehaviour
{
	public GameObject m_actorModelDataPrefab;

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
