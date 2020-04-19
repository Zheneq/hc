using System;
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpSpawnerPlacement.OnDrawGizmos()).MethodHandle;
			}
			return;
		}
		Gizmos.DrawIcon(base.transform.position, "icon_PowerUp.png");
	}

	void IGameEventListener.OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
	}
}
