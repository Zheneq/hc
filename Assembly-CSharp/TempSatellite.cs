using UnityEngine;

public class TempSatellite : MonoBehaviour, TempSatelliteAnimationEventReceiver.IOwner
{
	public GameObject m_persistentVfxPrefab;

	[JointPopup("FX attach joint for persistent Vfx")]
	public JointPopupProperty m_persistentVfxJoint;

	public bool m_hasSpawnAnim = true;

	public bool m_hasDespawnAnim;

	protected GameObject m_persistentVfxInstance;

	internal Animator m_modelAnimator;

	private Sequence m_owningSequence;

	private bool m_notifyOwningSequenceOnAnimEvent = true;

	public bool m_onlyPassAnimEventsIfActorDead;

	private void Awake()
	{
		m_modelAnimator = GetComponentInChildren<Animator>();
		if (m_modelAnimator != null)
		{
			m_modelAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		}
		if (!(m_persistentVfxPrefab != null))
		{
			return;
		}
		while (true)
		{
			m_persistentVfxJoint.Initialize(base.gameObject);
			if (m_persistentVfxJoint.m_jointObject != null)
			{
				while (true)
				{
					m_persistentVfxInstance = Object.Instantiate(m_persistentVfxPrefab);
					m_persistentVfxInstance.transform.parent = m_persistentVfxJoint.m_jointObject.transform;
					m_persistentVfxInstance.transform.localPosition = Vector3.zero;
					m_persistentVfxInstance.transform.localRotation = Quaternion.identity;
					m_persistentVfxInstance.transform.localScale = Vector3.one;
					return;
				}
			}
			return;
		}
	}

	private void Start()
	{
		Initialize();
	}

	private void OnDestroy()
	{
		if (m_persistentVfxInstance != null)
		{
			Object.Destroy(m_persistentVfxInstance);
		}
		OnTempSatelliteDestroy();
	}

	public virtual void OnTempSatelliteDestroy()
	{
	}

	protected virtual void Initialize()
	{
		GameObject gameObject = base.gameObject;
		GameObject gameObject2 = gameObject.transform.GetChild(0).gameObject;
		if (!(gameObject2.GetComponent<TempSatelliteAnimationEventReceiver>() == null))
		{
			return;
		}
		while (true)
		{
			TempSatelliteAnimationEventReceiver tempSatelliteAnimationEventReceiver = gameObject2.AddComponent<TempSatelliteAnimationEventReceiver>();
			tempSatelliteAnimationEventReceiver.Setup(this);
			return;
		}
	}

	public void Setup(Sequence owningSequence)
	{
		m_owningSequence = owningSequence;
	}

	public ActorData GetOwner()
	{
		if (m_owningSequence != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_owningSequence.Caster;
				}
			}
		}
		return null;
	}

	public void SetNotifyOwnerOnAnimEvent(bool notify)
	{
		m_notifyOwningSequenceOnAnimEvent = notify;
	}

	public virtual void TriggerAttack(GameObject attackTarget)
	{
		if (m_modelAnimator != null)
		{
			m_modelAnimator.SetTrigger("StartAttack");
		}
	}

	public virtual void TriggerSpawn()
	{
		if (!(m_modelAnimator != null))
		{
			return;
		}
		while (true)
		{
			if (m_hasSpawnAnim)
			{
				while (true)
				{
					m_modelAnimator.SetTrigger("Spawn");
					return;
				}
			}
			return;
		}
	}

	public virtual void TriggerDespawn()
	{
		if (!(m_modelAnimator != null))
		{
			return;
		}
		while (true)
		{
			if (m_hasDespawnAnim)
			{
				while (true)
				{
					m_modelAnimator.SetTrigger("Despawn");
					return;
				}
			}
			return;
		}
	}

	public void SetLookRotation(Vector3 lookDir)
	{
		base.transform.rotation = Quaternion.LookRotation(lookDir);
	}

	public void SetRotation(Quaternion rotation)
	{
		base.transform.rotation = rotation;
	}

	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	public void OnAnimationEvent(Object eventObject)
	{
		bool flag = true;
		int num;
		if (m_onlyPassAnimEventsIfActorDead)
		{
			if (!m_owningSequence.Caster.IsModelAnimatorDisabled())
			{
				num = (m_owningSequence.Caster.IsDead() ? 1 : 0);
				goto IL_0052;
			}
		}
		num = 1;
		goto IL_0052;
		IL_0052:
		flag = ((byte)num != 0);
		if (!m_notifyOwningSequenceOnAnimEvent)
		{
			return;
		}
		while (true)
		{
			if (flag)
			{
				SequenceManager.Get().OnAnimationEvent(m_owningSequence.Caster, eventObject, base.gameObject, m_owningSequence.Source);
			}
			return;
		}
	}
}
