using System;
using System.Collections.Generic;
using UnityEngine;

public class HealLaserSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[Tooltip("FX at point(s) of impact")]
	public GameObject m_hitFxPrefab;

	public GameObject m_healHitFxPrefab;

	[JointPopup("Start position for projectile")]
	public JointPopupProperty m_fxJoint;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_startEvent;

	public float m_projectileSpeed;

	[AudioEvent(false)]
	public string m_audioEvent;

	[AudioEvent(false)]
	public string m_impactAudioEvent;

	[AudioEvent(false)]
	public string m_impactHealAudioEvent;

	private GameObject m_fx;

	private Dictionary<ActorData, GameObject> m_hitFx;

	private float m_distanceTraveled;

	private float m_hitDuration;

	private float m_hitDurationLeft;

	private bool m_reachedDestination;

	private Vector3 m_startPos;

	private Vector3 m_endPos;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		if (base.Source.RemoveAtEndOfTurn)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(HealLaserSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
			}
			if (GameWideData.Get() != null && GameWideData.Get().ShouldMakeCasterVisibleOnCast())
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
				this.m_forceAlwaysVisible = true;
			}
		}
		if (base.Caster != null)
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
			this.m_fxJoint.Initialize(base.Caster.gameObject);
		}
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			HealLaserSequence.ExtraParams extraParams2 = extraSequenceParams as HealLaserSequence.ExtraParams;
			if (extraParams2 != null)
			{
				this.m_hitFx = new Dictionary<ActorData, GameObject>();
				this.CalculateHitDuration();
				this.m_endPos = extraParams2.endPos;
			}
		}
	}

	internal override Vector3 GetSequencePos()
	{
		if (this.m_fx != null)
		{
			return this.m_fx.transform.position;
		}
		return Vector3.zero;
	}

	private void Update()
	{
		base.ProcessSequenceVisibility();
		if (this.m_hitFx == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(HealLaserSequence.Update()).MethodHandle;
			}
			this.m_hitFx = new Dictionary<ActorData, GameObject>();
			this.CalculateHitDuration();
		}
		if (base.Caster != null)
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
			if (!this.m_fxJoint.IsInitialized())
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_fxJoint.Initialize(base.Caster.gameObject);
			}
		}
		if (this.m_fx != null)
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
			if (this.m_initialized)
			{
				if (this.m_hitDurationLeft > 0f)
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
					this.m_hitDurationLeft -= GameTime.deltaTime;
				}
				if (this.m_reachedDestination)
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
					if (this.m_fx.activeSelf)
					{
						this.m_fx.SetActive(false);
						this.PlayRemainingHitFX();
						if (this.m_hitDurationLeft <= 0f)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							base.MarkForRemoval();
						}
					}
				}
				else
				{
					this.UpdateProjectileFX();
				}
			}
		}
	}

	private void UpdateProjectileFX()
	{
		this.m_distanceTraveled += this.m_projectileSpeed * GameTime.deltaTime;
		this.m_fx.transform.position = this.m_startPos + this.m_fx.transform.forward * this.m_distanceTraveled;
		Vector3 lhs = this.m_endPos - this.m_fx.transform.position;
		if (Vector3.Dot(lhs, this.m_fx.transform.forward) < 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HealLaserSequence.UpdateProjectileFX()).MethodHandle;
			}
			this.m_fx.transform.position = this.m_endPos;
			this.m_reachedDestination = true;
		}
		this.ProcessHitFX();
	}

	private void PlayRemainingHitFX()
	{
		foreach (ActorData actorData in base.Targets)
		{
			if (!this.m_hitFx.ContainsKey(actorData))
			{
				this.SpawnHitFX(actorData, this.m_fx.transform.forward);
			}
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(HealLaserSequence.PlayRemainingHitFX()).MethodHandle;
		}
		base.Source.OnSequenceHit(this, base.TargetPos, null);
	}

	private void ProcessHitFX()
	{
		Vector3 lhs = this.m_endPos - this.m_fx.transform.position;
		foreach (ActorData actorData in base.Targets)
		{
			if (!this.m_hitFx.ContainsKey(actorData))
			{
				Vector3 rhs = actorData.transform.position - this.m_fx.transform.position;
				if (Vector3.Dot(lhs, rhs) < 0f)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(HealLaserSequence.ProcessHitFX()).MethodHandle;
					}
					this.SpawnHitFX(actorData, this.m_fx.transform.forward);
				}
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private Vector3 GetHitPosition(ActorData actorData)
	{
		Vector3 result = actorData.transform.position + Vector3.up;
		GameObject gameObject = actorData.gameObject.FindInChildren(Sequence.s_defaultHitAttachJoint, 0);
		if (gameObject != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HealLaserSequence.GetHitPosition(ActorData)).MethodHandle;
			}
			result = gameObject.transform.position;
		}
		else
		{
			gameObject = actorData.gameObject.FindInChildren(Sequence.s_defaultFallbackHitAttachJoint, 0);
			if (gameObject != null)
			{
				result = gameObject.transform.position;
			}
		}
		return result;
	}

	private void SpawnHitFX(ActorData actorData, Vector3 curDelta)
	{
		Vector3 hitPosition = this.GetHitPosition(actorData);
		bool flag = actorData.GetTeam() == base.Caster.GetTeam();
		this.m_hitFx[actorData] = null;
		if (this.m_hitFxPrefab)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HealLaserSequence.SpawnHitFX(ActorData, Vector3)).MethodHandle;
			}
			if (!flag && base.IsHitFXVisible(actorData))
			{
				this.m_hitFx[actorData] = base.InstantiateFX(this.m_hitFxPrefab, hitPosition, Quaternion.identity, true, true);
				this.m_hitFx[actorData].transform.parent = base.transform;
				this.m_hitDurationLeft = this.m_hitDuration;
				goto IL_133;
			}
		}
		if (this.m_healHitFxPrefab && flag)
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
			if (base.IsHitFXVisible(actorData))
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
				this.m_hitFx[actorData] = base.InstantiateFX(this.m_healHitFxPrefab, hitPosition, Quaternion.identity, true, true);
				this.m_hitFx[actorData].transform.parent = base.transform;
				this.m_hitDurationLeft = this.m_hitDuration;
			}
		}
		IL_133:
		if (!string.IsNullOrEmpty(this.m_impactAudioEvent))
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
			if (!flag)
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
				AudioManager.PostEvent(this.m_impactAudioEvent, this.m_fx.gameObject);
				goto IL_1B3;
			}
		}
		if (!string.IsNullOrEmpty(this.m_impactHealAudioEvent))
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
			if (flag)
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
				AudioManager.PostEvent(this.m_impactHealAudioEvent, this.m_fx.gameObject);
			}
		}
		IL_1B3:
		Vector3 normalized = curDelta.normalized;
		Vector3 position = this.m_fx.transform.position;
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, normalized);
		base.Source.OnSequenceHit(this, actorData, impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
	}

	private void SpawnFX()
	{
		this.m_startPos = this.m_fxJoint.m_jointObject.transform.position;
		Vector3 lookRotation = this.m_endPos - this.m_startPos;
		lookRotation.Normalize();
		Quaternion rotation = default(Quaternion);
		rotation.SetLookRotation(lookRotation);
		this.m_fx = base.InstantiateFX(this.m_fxPrefab, this.m_startPos, rotation, true, true);
		if (!string.IsNullOrEmpty(this.m_audioEvent))
		{
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(HealLaserSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.SpawnFX();
		}
	}

	private void OnDisable()
	{
		if (this.m_fx != null)
		{
			UnityEngine.Object.Destroy(this.m_fx);
			this.m_fx = null;
		}
		if (this.m_hitFx != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(HealLaserSequence.OnDisable()).MethodHandle;
			}
			using (Dictionary<ActorData, GameObject>.ValueCollection.Enumerator enumerator = this.m_hitFx.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject obj = enumerator.Current;
					UnityEngine.Object.Destroy(obj);
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.m_hitFx.Clear();
		}
		this.m_initialized = false;
	}

	private void CalculateHitDuration()
	{
		float fxduration = Sequence.GetFXDuration(this.m_hitFxPrefab);
		float fxduration2 = Sequence.GetFXDuration(this.m_healHitFxPrefab);
		this.m_hitDuration = Mathf.Max(fxduration, fxduration2);
	}

	public class ExtraParams : Sequence.IExtraSequenceParams
	{
		public Vector3 endPos;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.endPos);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.endPos);
		}
	}
}
