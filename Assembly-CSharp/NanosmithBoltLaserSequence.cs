using System;
using System.Collections.Generic;
using UnityEngine;

public class NanosmithBoltLaserSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[Tooltip("FX at point(s) of impact")]
	public GameObject m_fxImpactPrefab;

	[JointPopup("Start position for projectile")]
	public JointPopupProperty m_fxJoint;

	public Sequence.ReferenceModelType m_jointReferenceType;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_startEvent;

	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_hitPosJoint;

	public float m_projectileSpeed;

	[AudioEvent(false)]
	public string m_audioEvent;

	[AudioEvent(false)]
	public string m_impactAudioEvent;

	public float m_startDelay;

	public float m_projectileAcceleration;

	public float m_splineFractionUntilImpact = 1f;

	public bool m_spawnImpactAtFXDespawn;

	[Header("-- For Arcing Projectile Sequence")]
	public float m_maxHeight;

	public bool m_useTargetHitPos;

	public float m_yOffset;

	public bool m_reverseDirection;

	[Header("-- Bolt Projectile Info")]
	public GenericSequenceProjectileAuthoredInfo m_boltProjectileInfo;

	private bool m_startEventHappened;

	private float m_impactDuration;

	protected GameObject m_fx;

	private GameObject m_fxImpact;

	private CRSpline m_spline;

	private float m_curSplineSpeed;

	private float m_splineSpeed;

	private float m_splineAcceleration;

	private float m_splineTraveled;

	private float m_impactDurationLeft;

	private int m_curIndex;

	private int m_maxIndex;

	private bool m_skipImpactFx;

	protected bool m_useOverrideStartPos;

	protected Vector3 m_overrideStartPos = Vector3.zero;

	private bool m_boltsSpawned;

	private Vector3[] m_boltEndPositions;

	private List<ActorData[]> m_boltHitActors;

	private List<GenericSequenceProjectileInfo> m_boltProjectiles = new List<GenericSequenceProjectileInfo>();

	protected bool m_markForRemovalAfterImpact = true;

	public Vector3[] GetSplinePath(int curIndex, int maxIndex)
	{
		Vector3 vector = this.m_fxJoint.m_jointObject.transform.position;
		if (this.m_useOverrideStartPos)
		{
			vector = this.m_overrideStartPos;
		}
		Vector3[] array = new Vector3[5];
		if (this.m_maxHeight == 0f)
		{
			Vector3 vector2;
			if (this.m_useTargetHitPos)
			{
				vector2 = base.GetTargetHitPosition(0, this.m_hitPosJoint);
			}
			else
			{
				vector2 = base.TargetPos;
				vector2.y += this.m_yOffset;
			}
			Vector3 b = vector2 - vector;
			array[0] = vector - b;
			array[1] = vector;
			array[2] = (vector + vector2) * 0.5f;
			array[3] = vector2;
			array[4] = vector2 + b;
		}
		else
		{
			Vector3 vector3;
			if (this.m_useTargetHitPos)
			{
				vector3 = base.GetTargetHitPosition(0, this.m_hitPosJoint);
			}
			else
			{
				vector3 = base.TargetPos;
			}
			array[0] = vector + Vector3.down * this.m_maxHeight;
			array[1] = vector;
			array[2] = (vector + vector3) * 0.5f + Vector3.up * this.m_maxHeight;
			array[3] = vector3;
			array[4] = vector3 + Vector3.down * this.m_maxHeight;
		}
		if (this.m_reverseDirection)
		{
			Vector3 vector4 = array[0];
			array[0] = array[4];
			array[4] = vector4;
			vector4 = array[1];
			array[1] = array[3];
			array[3] = vector4;
		}
		return array;
	}

	protected int GetCurrentSplineSegment()
	{
		return this.m_spline.Section(this.m_splineTraveled);
	}

	public override void FinishSetup()
	{
		this.m_impactDuration = Sequence.GetFXDuration(this.m_fxImpactPrefab);
		if (this.m_startEvent == null)
		{
			this.ScheduleFX();
		}
	}

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			NanosmithBoltLaserSequence.ExtraParams extraParams2 = extraSequenceParams as NanosmithBoltLaserSequence.ExtraParams;
			if (extraParams2 != null)
			{
				this.m_curIndex = extraParams2.curIndex;
				this.m_maxIndex = extraParams2.maxIndex;
				this.m_startDelay = extraParams2.startDelay;
				this.m_skipImpactFx = extraParams2.skipImpactFx;
				this.m_useOverrideStartPos = extraParams2.useOverrideStartPos;
				this.m_overrideStartPos = extraParams2.overrideStartPos;
				this.m_boltEndPositions = extraParams2.boltEndPositions;
				this.m_boltHitActors = extraParams2.boltHitActors;
			}
		}
	}

	private void OnDisable()
	{
		using (List<GenericSequenceProjectileInfo>.Enumerator enumerator = this.m_boltProjectiles.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GenericSequenceProjectileInfo genericSequenceProjectileInfo = enumerator.Current;
				if (genericSequenceProjectileInfo != null)
				{
					genericSequenceProjectileInfo.OnSequenceDisable();
				}
			}
		}
		this.OnSequenceDisable();
	}

	protected virtual void OnSequenceDisable()
	{
		if (this.m_fx != null)
		{
			UnityEngine.Object.Destroy(this.m_fx);
			this.m_fx = null;
		}
		if (this.m_fxImpact != null)
		{
			UnityEngine.Object.Destroy(this.m_fxImpact);
			this.m_fxImpact = null;
		}
		this.m_initialized = false;
	}

	protected virtual void SpawnImpactFX(Vector3 impactPos, Quaternion impactRot)
	{
		if (!this.m_skipImpactFx)
		{
			if (this.m_fxImpactPrefab)
			{
				this.m_fxImpact = base.InstantiateFX(this.m_fxImpactPrefab, impactPos, impactRot, true, true);
				this.m_impactDurationLeft = this.m_impactDuration;
			}
			if (!string.IsNullOrEmpty(this.m_impactAudioEvent))
			{
				AudioManager.PostEvent(this.m_impactAudioEvent, this.m_fx.gameObject);
			}
		}
		base.CallHitSequenceOnTargets(impactPos, 1f, null, true);
		this.SpawnBolts();
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			this.ScheduleFX();
		}
	}

	private void ScheduleFX()
	{
		this.m_startEventHappened = true;
	}

	private void SpawnFX()
	{
		Vector3[] splinePath = this.GetSplinePath(this.m_curIndex, this.m_maxIndex);
		this.m_spline = new CRSpline(splinePath);
		Vector3 a = this.m_spline.Interp(0.05f);
		(a - splinePath[1]).Normalize();
		Quaternion rotation = default(Quaternion);
		float num = (splinePath[1] - splinePath[2]).magnitude + (splinePath[2] - splinePath[3]).magnitude;
		float num2 = num / this.m_projectileSpeed;
		this.m_splineSpeed = 1f / num2;
		this.m_splineAcceleration = this.m_projectileAcceleration * this.m_splineSpeed / this.m_projectileSpeed;
		if (this.m_projectileAcceleration == 0f)
		{
			this.m_curSplineSpeed = this.m_splineSpeed;
		}
		else
		{
			this.m_curSplineSpeed = 0f;
		}
		this.m_fx = base.InstantiateFX(this.m_fxPrefab, splinePath[1], rotation, true, true);
		if (!string.IsNullOrEmpty(this.m_audioEvent))
		{
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
	}

	private void Update()
	{
		this.OnUpdate();
	}

	protected virtual void OnUpdate()
	{
		base.ProcessSequenceVisibility();
		if (this.m_initialized && this.m_startEventHappened)
		{
			if (this.m_fx == null)
			{
				this.m_startDelay -= GameTime.deltaTime;
				if (this.m_startDelay <= 0f)
				{
					GameObject referenceModel = base.GetReferenceModel(base.Caster, this.m_jointReferenceType);
					if (referenceModel != null)
					{
						this.m_fxJoint.Initialize(referenceModel);
					}
					this.SpawnFX();
				}
			}
			else
			{
				if (this.m_fx.activeSelf)
				{
					this.m_curSplineSpeed += this.m_splineAcceleration;
					this.m_curSplineSpeed = Mathf.Min(this.m_splineSpeed, this.m_curSplineSpeed);
					this.m_splineTraveled += this.m_curSplineSpeed * GameTime.deltaTime;
					if (this.m_splineTraveled < this.m_splineFractionUntilImpact)
					{
						Vector3 vector = this.m_spline.Interp(this.m_splineTraveled);
						Quaternion rotation = default(Quaternion);
						rotation.SetLookRotation((vector - this.m_fx.transform.position).normalized);
						this.m_fx.transform.position = vector;
						this.m_fx.transform.rotation = rotation;
					}
					else
					{
						if (this.m_spawnImpactAtFXDespawn)
						{
							this.SpawnImpactFX(this.m_fx.transform.position, this.m_fx.transform.rotation);
						}
						else
						{
							this.SpawnImpactFX(base.TargetPos, Quaternion.identity);
						}
						this.m_fx.SetActive(false);
						if (this.m_fxImpactPrefab == null && this.m_markForRemovalAfterImpact)
						{
						}
					}
				}
				if (this.m_fxImpact != null)
				{
					if (this.m_fxImpact.activeSelf)
					{
						if (this.m_impactDurationLeft > 0f)
						{
							this.m_impactDurationLeft -= GameTime.deltaTime;
						}
						else if (this.m_markForRemovalAfterImpact)
						{
						}
					}
				}
			}
		}
		using (List<GenericSequenceProjectileInfo>.Enumerator enumerator = this.m_boltProjectiles.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GenericSequenceProjectileInfo genericSequenceProjectileInfo = enumerator.Current;
				genericSequenceProjectileInfo.OnUpdate();
			}
		}
	}

	private void SpawnBolts()
	{
		if (!this.m_boltsSpawned)
		{
			if (this.m_boltEndPositions != null)
			{
				for (int i = 0; i < this.m_boltEndPositions.Length; i++)
				{
					ActorData[] targetActors = null;
					if (this.m_boltHitActors.Count > i)
					{
						targetActors = this.m_boltHitActors[i];
					}
					GenericSequenceProjectileInfo item = new GenericSequenceProjectileInfo(this, this.m_boltProjectileInfo, base.TargetPos, this.m_boltEndPositions[i], targetActors);
					this.m_boltProjectiles.Add(item);
				}
				this.m_boltsSpawned = true;
			}
		}
	}

	public class ExtraParams : Sequence.IExtraSequenceParams
	{
		public float startDelay;

		public int curIndex;

		public int maxIndex;

		public bool skipImpactFx;

		public bool useOverrideStartPos;

		public Vector3 overrideStartPos = Vector3.zero;

		public Vector3[] boltEndPositions;

		public List<ActorData[]> boltHitActors;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.startDelay);
			stream.Serialize(ref this.curIndex);
			stream.Serialize(ref this.maxIndex);
			stream.Serialize(ref this.skipImpactFx);
			stream.Serialize(ref this.useOverrideStartPos);
			stream.Serialize(ref this.overrideStartPos);
			int num;
			if (this.boltEndPositions != null)
			{
				num = this.boltEndPositions.Length;
			}
			else
			{
				num = 0;
			}
			int num2 = num;
			stream.Serialize(ref num2);
			for (int i = 0; i < num2; i++)
			{
				Vector3 vector = this.boltEndPositions[i];
				stream.Serialize(ref vector);
			}
			int num3;
			if (this.boltHitActors != null)
			{
				num3 = this.boltHitActors.Count;
			}
			else
			{
				num3 = 0;
			}
			int num4 = num3;
			stream.Serialize(ref num4);
			for (int j = 0; j < num4; j++)
			{
				ActorData[] array = this.boltHitActors[j];
				int num5 = (array == null) ? 0 : array.Length;
				stream.Serialize(ref num5);
				for (int k = 0; k < num5; k++)
				{
					ActorData actorData = array[k];
					int num6;
					if (actorData != null)
					{
						num6 = actorData.ActorIndex;
					}
					else
					{
						num6 = ActorData.s_invalidActorIndex;
					}
					int num7 = num6;
					stream.Serialize(ref num7);
				}
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.startDelay);
			stream.Serialize(ref this.curIndex);
			stream.Serialize(ref this.maxIndex);
			stream.Serialize(ref this.skipImpactFx);
			stream.Serialize(ref this.useOverrideStartPos);
			stream.Serialize(ref this.overrideStartPos);
			int num = 0;
			stream.Serialize(ref num);
			this.boltEndPositions = new Vector3[num];
			for (int i = 0; i < num; i++)
			{
				Vector3 zero = Vector3.zero;
				stream.Serialize(ref zero);
				this.boltEndPositions[i] = zero;
			}
			int num2 = 0;
			stream.Serialize(ref num2);
			this.boltHitActors = new List<ActorData[]>(num2);
			for (int j = 0; j < num2; j++)
			{
				int num3 = 0;
				stream.Serialize(ref num3);
				ActorData[] array = new ActorData[num3];
				for (int k = 0; k < num3; k++)
				{
					int s_invalidActorIndex = ActorData.s_invalidActorIndex;
					stream.Serialize(ref s_invalidActorIndex);
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex(s_invalidActorIndex);
					array[k] = actorData;
				}
				this.boltHitActors.Add(array);
			}
		}
	}
}
