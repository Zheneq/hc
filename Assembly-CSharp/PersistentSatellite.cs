using System;
using UnityEngine;

public class PersistentSatellite : MonoBehaviour
{
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
		this.m_modelAnimator = base.GetComponentInChildren<Animator>();
		this.m_renderer = base.GetComponentInChildren<Renderer>();
	}

	private void Start()
	{
		GameObject gameObject = base.gameObject;
		GameObject gameObject2 = gameObject.transform.GetChild(0).gameObject;
		if (gameObject2.GetComponent<PersistentSatelliteAnimationEventReceiver>() == null)
		{
			PersistentSatelliteAnimationEventReceiver persistentSatelliteAnimationEventReceiver = gameObject2.AddComponent<PersistentSatelliteAnimationEventReceiver>();
			persistentSatelliteAnimationEventReceiver.Setup(this);
		}
		this.m_ownerAnimationEventReceiver = this.m_ownerController.gameObject.GetComponentInChildren<AnimationEventReceiver>();
	}

	public bool IsVisible()
	{
		return this.m_renderer.enabled;
	}

	public void OverrideVisibility(bool shouldBeVisible)
	{
		if (this.m_renderer.enabled != shouldBeVisible)
		{
			this.m_renderer.enabled = shouldBeVisible;
		}
	}

	private void Update()
	{
		this.UpdateAnimatorParameterDistToGoal();
		Vector3 lhs = this.m_targetPosition - base.transform.position;
		if (!Mathf.Approximately(lhs.magnitude, 0f))
		{
			if (this.m_timeTillVisibleForSpawnAndRun > 0f)
			{
				this.m_timeTillVisibleForSpawnAndRun -= Time.deltaTime;
				if (this.m_timeTillVisibleForSpawnAndRun <= 0f)
				{
					this.m_renderer.enabled = true;
				}
			}
			if (this.m_timeTillVisibleForSpawnAndRun <= 0f)
			{
				Vector3 vector = base.transform.position + lhs.normalized * Time.deltaTime * this.m_movementSpeed;
				Vector3 rhs = this.m_targetPosition - vector;
				if (Vector3.Dot(lhs, rhs) < 0f)
				{
					base.transform.position = this.m_targetPosition;
				}
				else
				{
					base.transform.position = vector;
					base.transform.rotation = Quaternion.LookRotation(lhs.normalized);
				}
			}
		}
		else if (this.m_timeTillVisibleForSpawnAndRun > 0f)
		{
			this.m_timeTillVisibleForSpawnAndRun = 0f;
			this.m_renderer.enabled = true;
		}
		if (this.m_modelAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
		{
			if (this.m_attackTarget != null)
			{
				base.transform.rotation = Quaternion.LookRotation((this.m_attackTarget.transform.position - base.transform.position).normalized);
			}
		}
	}

	public void OnAssignedToInitialBoardSquare()
	{
		this.TeleportToLocation(this.m_ownerController.transform.position);
		if (!this.m_visibleAtStart)
		{
			if (this.m_renderer != null)
			{
				this.m_renderer.enabled = false;
			}
		}
	}

	public void OnRespawn()
	{
		this.TeleportToLocation(this.m_ownerController.transform.position);
		if (this.m_hideOnRespawn)
		{
			this.m_renderer.enabled = false;
		}
		else
		{
			this.TriggerSpawn();
		}
	}

	public void OnActorDeath()
	{
		if (this.m_hideOnDeath)
		{
			this.m_renderer.enabled = false;
		}
		else if (this.m_playDespawnAnimOnDeath)
		{
			this.TriggerDespawn();
		}
	}

	public void TeleportToLocation(Vector3 targetPosition)
	{
		this.m_targetPosition = targetPosition;
		base.transform.position = this.m_targetPosition;
	}

	public void OnAnimationEvent(UnityEngine.Object eventObject)
	{
		this.m_ownerAnimationEventReceiver.ProcessAnimationEvent(eventObject, base.gameObject);
	}

	public void Setup(SatelliteController ownerController)
	{
		this.m_ownerController = ownerController;
		this.m_targetPosition = ownerController.transform.position;
		base.transform.position = ownerController.transform.position;
		this.TriggerSpawn();
	}

	public void TriggerSpawn()
	{
		this.m_modelAnimator.SetTrigger("Spawn");
	}

	public void TriggerDespawn()
	{
		this.m_modelAnimator.SetTrigger("Despawn");
	}

	public bool IsMoving()
	{
		return (this.m_targetPosition - base.transform.position).magnitude > 0.01f;
	}

	private void UpdateAnimatorParameterDistToGoal()
	{
		Vector3 vector = this.m_targetPosition - base.transform.position;
		this.m_modelAnimator.SetFloat("DistToGoal", vector.magnitude);
	}

	public string GetMoveStartAnimTrigger(PersistentSatellite.SatelliteMoveStartType moveStartType, bool visible)
	{
		if (moveStartType == PersistentSatellite.SatelliteMoveStartType.Alt)
		{
			string result;
			if (visible)
			{
				result = "StartAltRun";
			}
			else
			{
				result = "StartSpawnAndAltRun";
			}
			return result;
		}
		string result2;
		if (visible)
		{
			result2 = "StartRun";
		}
		else
		{
			result2 = "StartSpawnAndRun";
		}
		return result2;
	}

	public void AltMoveToPosition(Vector3 targetPos)
	{
		this.MoveToPosition(targetPos, PersistentSatellite.SatelliteMoveStartType.Alt);
	}

	public void MoveToPosition(Vector3 targetPos, PersistentSatellite.SatelliteMoveStartType moveStartType = PersistentSatellite.SatelliteMoveStartType.Normal)
	{
		if (this.IsVisible())
		{
			this.m_modelAnimator.SetTrigger(this.GetMoveStartAnimTrigger(moveStartType, true));
		}
		else
		{
			this.m_modelAnimator.SetTrigger(this.GetMoveStartAnimTrigger(moveStartType, false));
			Vector3 vector = this.m_ownerController.transform.position;
			if (this.m_startOffsetDistanceWhenInvisible > 0f)
			{
				Vector3 a = targetPos - this.m_ownerController.transform.position;
				a.y = 0f;
				if (a.magnitude == 0f)
				{
					a = Vector3.forward;
				}
				else
				{
					a.Normalize();
				}
				vector -= a * this.m_startOffsetDistanceWhenInvisible;
			}
			this.TeleportToLocation(vector);
			if (this.m_spawnAndRunDelay > 0f)
			{
				this.m_timeTillVisibleForSpawnAndRun = this.m_spawnAndRunDelay;
			}
			else
			{
				this.m_timeTillVisibleForSpawnAndRun = 0f;
				this.m_renderer.enabled = true;
			}
		}
		this.m_targetPosition = targetPos;
		this.UpdateAnimatorParameterDistToGoal();
	}

	public void TriggerAttack(GameObject attackTarget)
	{
		this.m_modelAnimator.SetTrigger("StartAttack");
		this.m_attackTarget = attackTarget;
	}

	public enum SatelliteMoveStartType
	{
		Normal,
		Alt
	}
}
