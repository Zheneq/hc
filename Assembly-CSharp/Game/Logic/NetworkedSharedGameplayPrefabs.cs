// ROGUES
// SERVER
using UnityEngine;

// no methods in reactor
public class NetworkedSharedGameplayPrefabs : MonoBehaviour
{
	public GameObject m_sharedActionBuffer;
	public GameObject m_sharedEffectBarrierManager;
	public GameObject m_actorTeamSensitiveData_Friendly;
	public GameObject m_actorTeamSensitiveData_Hostile; // removed in rogues

#if SERVER
	// server-only
	public static GameObject GetSharedActionBufferPrefab()
	{
		NetworkedSharedGameplayPrefabs networkedSharedGameplayPrefabs = NetworkedSharedGameplayPrefabs.Get();
		if (networkedSharedGameplayPrefabs != null)
		{
			return networkedSharedGameplayPrefabs.m_sharedActionBuffer;
		}
		else
		{
			Debug.LogError("Trying to get SharedActionBufferPrefab by way of NetworkedSharedGameplayPrefabs, but it's null.");
			return null;
		}
	}

	// server-only
	public static GameObject GetSharedEffectBarrierManagerPrefab()
	{
		NetworkedSharedGameplayPrefabs networkedSharedGameplayPrefabs = NetworkedSharedGameplayPrefabs.Get();
		if (networkedSharedGameplayPrefabs != null)
		{
			return networkedSharedGameplayPrefabs.m_sharedEffectBarrierManager;
		}
		else
		{
			Debug.LogError("Trying to get SharedEffectBarrierManagerPrefab by way of NetworkedSharedGameplayPrefabs, but it's null.");
			return null;
		}
	}

	// server-only
	public static GameObject GetActorTeamSensitiveData_FriendlyPrefab()
	{
		NetworkedSharedGameplayPrefabs networkedSharedGameplayPrefabs = NetworkedSharedGameplayPrefabs.Get();
		if (networkedSharedGameplayPrefabs != null)
		{
			return networkedSharedGameplayPrefabs.m_actorTeamSensitiveData_Friendly;
		}
		else
		{
			Debug.LogError("Trying to get ActorTeamSensitiveData_Friendly by way of NetworkedSharedGameplayPrefabs, but it's null.");
			return null;
		}
	}

	// server-only, missing in rogues, recreated
	public static GameObject GetActorTeamSensitiveData_HostilePrefab()
	{
		NetworkedSharedGameplayPrefabs networkedSharedGameplayPrefabs = NetworkedSharedGameplayPrefabs.Get();
		if (networkedSharedGameplayPrefabs != null)
		{
			return networkedSharedGameplayPrefabs.m_actorTeamSensitiveData_Hostile;
		}
		else
		{
			Debug.LogError("Trying to get ActorTeamSensitiveData_Hostile by way of NetworkedSharedGameplayPrefabs, but it's null.");
			return null;
		}
	}

	// server-only
	public static NetworkedSharedGameplayPrefabs Get()
	{
		NetworkedSharedGameplayPrefabs result;
		if (ServerGameManager.Get() != null)
		{
			return ServerGameManager.Get().GetComponent<NetworkedSharedGameplayPrefabs>();
		}
		else
		{
			Debug.LogError("Trying to get NetworkedSharedGameplayPrefabs by way of ServerGameManager singleton, but it's null.");
			return null;
		}
	}
#endif
}
