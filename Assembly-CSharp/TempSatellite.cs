using System;
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
		this.m_modelAnimator = base.GetComponentInChildren<Animator>();
		if (this.m_modelAnimator != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TempSatellite.Awake()).MethodHandle;
			}
			this.m_modelAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		}
		if (this.m_persistentVfxPrefab != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_persistentVfxJoint.Initialize(base.gameObject);
			if (this.m_persistentVfxJoint.m_jointObject != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_persistentVfxInstance = UnityEngine.Object.Instantiate<GameObject>(this.m_persistentVfxPrefab);
				this.m_persistentVfxInstance.transform.parent = this.m_persistentVfxJoint.m_jointObject.transform;
				this.m_persistentVfxInstance.transform.localPosition = Vector3.zero;
				this.m_persistentVfxInstance.transform.localRotation = Quaternion.identity;
				this.m_persistentVfxInstance.transform.localScale = Vector3.one;
			}
		}
	}

	private void Start()
	{
		this.Initialize();
	}

	private void OnDestroy()
	{
		if (this.m_persistentVfxInstance != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TempSatellite.OnDestroy()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_persistentVfxInstance);
		}
		this.OnTempSatelliteDestroy();
	}

	public virtual void OnTempSatelliteDestroy()
	{
	}

	protected virtual void Initialize()
	{
		GameObject gameObject = base.gameObject;
		GameObject gameObject2 = gameObject.transform.GetChild(0).gameObject;
		if (gameObject2.GetComponent<TempSatelliteAnimationEventReceiver>() == null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TempSatellite.Initialize()).MethodHandle;
			}
			TempSatelliteAnimationEventReceiver tempSatelliteAnimationEventReceiver = gameObject2.AddComponent<TempSatelliteAnimationEventReceiver>();
			tempSatelliteAnimationEventReceiver.Setup(this);
		}
	}

	public void Setup(Sequence owningSequence)
	{
		this.m_owningSequence = owningSequence;
	}

	public ActorData GetOwner()
	{
		if (this.m_owningSequence != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TempSatellite.GetOwner()).MethodHandle;
			}
			return this.m_owningSequence.Caster;
		}
		return null;
	}

	public void SetNotifyOwnerOnAnimEvent(bool notify)
	{
		this.m_notifyOwningSequenceOnAnimEvent = notify;
	}

	public virtual void TriggerAttack(GameObject attackTarget)
	{
		if (this.m_modelAnimator != null)
		{
			this.m_modelAnimator.SetTrigger("StartAttack");
		}
	}

	public virtual void TriggerSpawn()
	{
		if (this.m_modelAnimator != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TempSatellite.TriggerSpawn()).MethodHandle;
			}
			if (this.m_hasSpawnAnim)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_modelAnimator.SetTrigger("Spawn");
			}
		}
	}

	public virtual void TriggerDespawn()
	{
		if (this.m_modelAnimator != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TempSatellite.TriggerDespawn()).MethodHandle;
			}
			if (this.m_hasDespawnAnim)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_modelAnimator.SetTrigger("Despawn");
			}
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

	public void OnAnimationEvent(UnityEngine.Object eventObject)
	{
		bool flag;
		if (this.m_onlyPassAnimEventsIfActorDead)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TempSatellite.OnAnimationEvent(UnityEngine.Object)).MethodHandle;
			}
			if (!this.m_owningSequence.Caster.IsModelAnimatorDisabled())
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				flag = this.m_owningSequence.Caster.IsDead();
				goto IL_52;
			}
		}
		flag = true;
		IL_52:
		bool flag2 = flag;
		if (this.m_notifyOwningSequenceOnAnimEvent)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (flag2)
			{
				SequenceManager.Get().OnAnimationEvent(this.m_owningSequence.Caster, eventObject, base.gameObject, this.m_owningSequence.Source);
			}
		}
	}
}
