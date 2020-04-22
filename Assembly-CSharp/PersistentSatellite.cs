using UnityEngine;

public class PersistentSatellite : MonoBehaviour
{
	public enum SatelliteMoveStartType
	{
		Normal,
		Alt
	}

	internal Animator m_modelAnimator;

	private SatelliteController m_ownerController;

	public float m_movementSpeed = 5f;

	public bool m_visibleAtStart = true;

	public bool m_hideOnRespawn;

	public bool m_hideOnDeath;

	public bool m_playDespawnAnimOnDeath;

	[Tooltip("If not already visible on start of movement, delay turning on visibility")]
	public float m_spawnAndRunDelay;

	[Tooltip("Distance offset when starting to move while invisible, to reduce cases where satellite looks like it's dropping only vertically")]
	public float m_startOffsetDistanceWhenInvisible;

	private Renderer m_renderer;

	private AnimationEventReceiver m_ownerAnimationEventReceiver;

	private Vector3 m_targetPosition;

	private GameObject m_attackTarget;

	private float m_timeTillVisibleForSpawnAndRun;

	private void Awake()
	{
		m_modelAnimator = GetComponentInChildren<Animator>();
		m_renderer = GetComponentInChildren<Renderer>();
	}

	private void Start()
	{
		GameObject gameObject = base.gameObject;
		GameObject gameObject2 = gameObject.transform.GetChild(0).gameObject;
		if (gameObject2.GetComponent<PersistentSatelliteAnimationEventReceiver>() == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			PersistentSatelliteAnimationEventReceiver persistentSatelliteAnimationEventReceiver = gameObject2.AddComponent<PersistentSatelliteAnimationEventReceiver>();
			persistentSatelliteAnimationEventReceiver.Setup(this);
		}
		m_ownerAnimationEventReceiver = m_ownerController.gameObject.GetComponentInChildren<AnimationEventReceiver>();
	}

	public bool IsVisible()
	{
		return m_renderer.enabled;
	}

	public void OverrideVisibility(bool shouldBeVisible)
	{
		if (m_renderer.enabled == shouldBeVisible)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_renderer.enabled = shouldBeVisible;
			return;
		}
	}

	private void Update()
	{
		UpdateAnimatorParameterDistToGoal();
		Vector3 lhs = m_targetPosition - base.transform.position;
		if (!Mathf.Approximately(lhs.magnitude, 0f))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_timeTillVisibleForSpawnAndRun > 0f)
			{
				m_timeTillVisibleForSpawnAndRun -= Time.deltaTime;
				if (m_timeTillVisibleForSpawnAndRun <= 0f)
				{
					m_renderer.enabled = true;
				}
			}
			if (m_timeTillVisibleForSpawnAndRun <= 0f)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 vector = base.transform.position + lhs.normalized * Time.deltaTime * m_movementSpeed;
				Vector3 rhs = m_targetPosition - vector;
				if (Vector3.Dot(lhs, rhs) < 0f)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					base.transform.position = m_targetPosition;
				}
				else
				{
					base.transform.position = vector;
					base.transform.rotation = Quaternion.LookRotation(lhs.normalized);
				}
			}
		}
		else if (m_timeTillVisibleForSpawnAndRun > 0f)
		{
			m_timeTillVisibleForSpawnAndRun = 0f;
			m_renderer.enabled = true;
		}
		if (!m_modelAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (m_attackTarget != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					base.transform.rotation = Quaternion.LookRotation((m_attackTarget.transform.position - base.transform.position).normalized);
					return;
				}
			}
			return;
		}
	}

	public void OnAssignedToInitialBoardSquare()
	{
		TeleportToLocation(m_ownerController.transform.position);
		if (m_visibleAtStart)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_renderer != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					m_renderer.enabled = false;
					return;
				}
			}
			return;
		}
	}

	public void OnRespawn()
	{
		TeleportToLocation(m_ownerController.transform.position);
		if (m_hideOnRespawn)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_renderer.enabled = false;
					return;
				}
			}
		}
		TriggerSpawn();
	}

	public void OnActorDeath()
	{
		if (m_hideOnDeath)
		{
			m_renderer.enabled = false;
		}
		else
		{
			if (!m_playDespawnAnimOnDeath)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				TriggerDespawn();
				return;
			}
		}
	}

	public void TeleportToLocation(Vector3 targetPosition)
	{
		m_targetPosition = targetPosition;
		base.transform.position = m_targetPosition;
	}

	public void OnAnimationEvent(Object eventObject)
	{
		m_ownerAnimationEventReceiver.ProcessAnimationEvent(eventObject, base.gameObject);
	}

	public void Setup(SatelliteController ownerController)
	{
		m_ownerController = ownerController;
		m_targetPosition = ownerController.transform.position;
		base.transform.position = ownerController.transform.position;
		TriggerSpawn();
	}

	public void TriggerSpawn()
	{
		m_modelAnimator.SetTrigger("Spawn");
	}

	public void TriggerDespawn()
	{
		m_modelAnimator.SetTrigger("Despawn");
	}

	public bool IsMoving()
	{
		return (m_targetPosition - base.transform.position).magnitude > 0.01f;
	}

	private void UpdateAnimatorParameterDistToGoal()
	{
		Vector3 vector = m_targetPosition - base.transform.position;
		m_modelAnimator.SetFloat("DistToGoal", vector.magnitude);
	}

	public string GetMoveStartAnimTrigger(SatelliteMoveStartType moveStartType, bool visible)
	{
		if (moveStartType == SatelliteMoveStartType.Alt)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					object result;
					if (visible)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						result = "StartAltRun";
					}
					else
					{
						result = "StartSpawnAndAltRun";
					}
					return (string)result;
				}
				}
			}
		}
		object result2;
		if (visible)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			result2 = "StartRun";
		}
		else
		{
			result2 = "StartSpawnAndRun";
		}
		return (string)result2;
	}

	public void AltMoveToPosition(Vector3 targetPos)
	{
		MoveToPosition(targetPos, SatelliteMoveStartType.Alt);
	}

	public void MoveToPosition(Vector3 targetPos, SatelliteMoveStartType moveStartType = SatelliteMoveStartType.Normal)
	{
		if (IsVisible())
		{
			m_modelAnimator.SetTrigger(GetMoveStartAnimTrigger(moveStartType, true));
		}
		else
		{
			m_modelAnimator.SetTrigger(GetMoveStartAnimTrigger(moveStartType, false));
			Vector3 position = m_ownerController.transform.position;
			if (m_startOffsetDistanceWhenInvisible > 0f)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				Vector3 a = targetPos - m_ownerController.transform.position;
				a.y = 0f;
				if (a.magnitude == 0f)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					a = Vector3.forward;
				}
				else
				{
					a.Normalize();
				}
				position -= a * m_startOffsetDistanceWhenInvisible;
			}
			TeleportToLocation(position);
			if (m_spawnAndRunDelay > 0f)
			{
				m_timeTillVisibleForSpawnAndRun = m_spawnAndRunDelay;
			}
			else
			{
				m_timeTillVisibleForSpawnAndRun = 0f;
				m_renderer.enabled = true;
			}
		}
		m_targetPosition = targetPos;
		UpdateAnimatorParameterDistToGoal();
	}

	public void TriggerAttack(GameObject attackTarget)
	{
		m_modelAnimator.SetTrigger("StartAttack");
		m_attackTarget = attackTarget;
	}
}
