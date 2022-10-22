// ROGUES
// SERVER
using UnityEngine;
using UnityEngine.Networking;

public class PowerUpSpawnerPlacement : MonoBehaviour, IGameEventListener  // , IMissionTagged in rogues
{
	public PowerUpSpawner m_powerUpSpawnerPrefab;
	private PowerUpSpawner m_powerUpSpawnerInstance;
	public int m_spawnInterval = 2;
	public int m_initialSpawnDelay = 1;
	[Tooltip("Objects=everyone can get it")]
	public Team m_teamRestriction = Team.Objects;
	public string[] m_tagsToApplyToPowerup;

	// rogues
	// public List<PowerUpSpawner.PowerUpByTag> m_powerUpPrefabByTag;

	private void OnDrawGizmos()
	{
		if (CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			Gizmos.DrawIcon(base.transform.position, "icon_PowerUp.png");
		}
	}

	// rogues
	// public List<string> GetRelevantTags()
	// {
	// 	return (from p in this.m_powerUpPrefabByTag
	// 	select p.tag).ToList<string>();
	// }

	// rogues
	// public bool MatchesTag(string tag)
	// {
	// 	return this.m_powerUpPrefabByTag.ContainsWhere((PowerUpSpawner.PowerUpByTag p) => p.tag == tag);
	// }

#if SERVER
	// added in rogues
	public void Awake()
	{
		if (VisualsLoader.Get() != null && VisualsLoader.Get().LevelLoaded())
		{
			Spawn();
		}
		else
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.VisualSceneLoaded);
		}
	}
#endif

	void IGameEventListener.OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
#if SERVER
		// added in rogues
		if (eventType == GameEventManager.EventType.VisualSceneLoaded)
		{
			Spawn();
		}
#endif
	}

#if SERVER
	// added in rogues
	private void Spawn()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (m_powerUpSpawnerInstance == null)
		{
			// PowerUp powerUpToSpawn = PowerUpSpawner.GetPowerUpToSpawn(this.m_powerUpPrefabByTag, GameManager.Get().GameMission);
			// if (this.m_powerUpPrefabByTag.IsNullOrEmpty<PowerUpSpawner.PowerUpByTag>() || powerUpToSpawn != null)
			// {
				m_powerUpSpawnerInstance = Instantiate(m_powerUpSpawnerPrefab, transform.position, transform.rotation);
				m_powerUpSpawnerInstance.m_initialSpawnDelay = m_initialSpawnDelay;
				m_powerUpSpawnerInstance.m_spawnInterval = m_spawnInterval;
				m_powerUpSpawnerInstance.m_teamRestriction = m_teamRestriction;
				m_powerUpSpawnerInstance.m_tagsToApplyToPowerup = m_tagsToApplyToPowerup;
				// m_powerUpSpawnerInstance.m_powerUpPrefabByTag = this.m_powerUpPrefabByTag;
				DontDestroyOnLoad(m_powerUpSpawnerInstance.gameObject);
				NetworkServer.Spawn(m_powerUpSpawnerInstance.gameObject);
			// }
		}
		else
		{
			Debug.LogError("Trying to spawn powerup spawner at " + gameObject.transform.position + " multiple times");
		}
	}
#endif

#if SERVER
	// added in rogues
	private void OnDestroy()
	{
		if (NetworkServer.active && m_powerUpSpawnerInstance != null)
		{
			NetworkServer.Destroy(m_powerUpSpawnerInstance.gameObject);
			m_powerUpSpawnerInstance = null;
		}
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.VisualSceneLoaded);
		}
	}
#endif
}
