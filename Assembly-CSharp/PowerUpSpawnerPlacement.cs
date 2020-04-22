using UnityEngine;

public class PowerUpSpawnerPlacement : MonoBehaviour, IGameEventListener
{
	public PowerUpSpawner m_powerUpSpawnerPrefab;

	private PowerUpSpawner m_powerUpSpawnerInstance;

	public int m_spawnInterval = 2;

	public int m_initialSpawnDelay = 1;

	[Tooltip("Objects=everyone can get it")]
	public Team m_teamRestriction = Team.Objects;

	public string[] m_tagsToApplyToPowerup;

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		Gizmos.DrawIcon(base.transform.position, "icon_PowerUp.png");
	}

	void IGameEventListener.OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
	}
}
