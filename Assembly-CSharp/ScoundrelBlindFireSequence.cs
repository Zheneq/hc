using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoundrelBlindFireSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[Tooltip("FX at point(s) of impact")]
	public GameObject m_fxImpactPrefab;

	[JointPopup("hit FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_hitFxJoint;

	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_fxJointLeft;

	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_fxJointRight;

	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public UnityEngine.Object m_startEventLeft;

	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public UnityEngine.Object m_startEventRight;

	[AnimEventPicker]
	public UnityEngine.Object m_hitReactEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_lastHitReactEvent;

	[Tooltip("Amount of time to trigger actual impact after hit react event has been received")]
	public float m_hitImpactDelayTime = -1f;

	public float m_maxDistInSquares = 7f;

	[Header("    (this is half angle, to left and right of center direction)")]
	public float m_angleRange = 30f;

	[Tooltip("will assume sequence will shoot at least 1 at the moment")]
	public int m_projectilesPerAnimEvent = 1;

	public bool m_raycastDistanceFromLosHeight = true;

	[Header("    ( if 1 projectile at a time, estimated number of projectiles total )")]
	public int m_expectedNumProjectilesForEdgeShots = -1;

	[Header("    ( chance to add extra shots at edges beyond guaranteed ones )")]
	public float m_extraEdgeShotChance;

	private List<GameObject> m_FXs;

	private List<GameObject> m_fxImpacts;

	[AudioEvent(false)]
	public string m_audioEvent;

	[AudioEvent(false)]
	public string m_impactAudioEvent;

	[Header("-- Alternative IMpact Audio Events, handled per ability, unused otherwise")]
	[AudioEvent(false)]
	public string[] m_alternativeImpactAudioEvents;

	private int m_alternativeAudioIndex = -1;

	private int m_numSpawnAttempts;

	private int m_spawnIndexEdgeMin = -1;

	private int m_spawnIndexEdgeMax = -1;

	private List<SimpleAttachedVFXSequence.DelayedImpact> m_delayedImpacts = new List<SimpleAttachedVFXSequence.DelayedImpact>();

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ScoundrelBlindFireSequence.ConeExtraParams coneExtraParams = extraSequenceParams as ScoundrelBlindFireSequence.ConeExtraParams;
			if (coneExtraParams != null)
			{
				if (coneExtraParams.maxDistInSquares > 0f)
				{
					this.m_maxDistInSquares = coneExtraParams.maxDistInSquares;
				}
				if (coneExtraParams.halfAngleDegrees > 0f)
				{
					this.m_angleRange = coneExtraParams.halfAngleDegrees;
				}
			}
			SimpleAttachedVFXSequence.ImpactDelayParams impactDelayParams = extraSequenceParams as SimpleAttachedVFXSequence.ImpactDelayParams;
			if (impactDelayParams != null)
			{
				if (impactDelayParams.impactDelayTime > 0f)
				{
					this.m_hitImpactDelayTime = impactDelayParams.impactDelayTime;
				}
				if ((int)impactDelayParams.alternativeImpactAudioIndex >= 0)
				{
					this.m_alternativeAudioIndex = (int)impactDelayParams.alternativeImpactAudioIndex;
				}
			}
		}
	}

	public override void FinishSetup()
	{
		this.m_FXs = new List<GameObject>();
		this.m_fxImpacts = new List<GameObject>();
		if (this.m_projectilesPerAnimEvent <= 0x14)
		{
			if (this.m_projectilesPerAnimEvent >= 1)
			{
				goto IL_4D;
			}
		}
		this.m_projectilesPerAnimEvent = 1;
		IL_4D:
		if (this.m_projectilesPerAnimEvent <= 1)
		{
			int num = this.m_expectedNumProjectilesForEdgeShots;
			if (num <= 0)
			{
				num = 4;
			}
			this.m_spawnIndexEdgeMin = UnityEngine.Random.Range(0, num);
			this.m_spawnIndexEdgeMax = UnityEngine.Random.Range(0, num);
			if (this.m_spawnIndexEdgeMax == this.m_spawnIndexEdgeMin)
			{
				this.m_spawnIndexEdgeMax = (this.m_spawnIndexEdgeMax + 1) % num;
			}
		}
	}

	private float GetProjectileDistance(Vector3 start, Vector3 forward, float maxDist)
	{
		Vector3 vector = start;
		if (this.m_raycastDistanceFromLosHeight)
		{
			vector.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		}
		else
		{
			vector.y += 0.5f;
		}
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, forward, maxDist, false, base.Caster, null, true);
		return (vector - laserEndPoint).magnitude;
	}

	private void SpawnFX(bool left)
	{
		JointPopupProperty jointPopupProperty = (!left) ? this.m_fxJointRight : this.m_fxJointLeft;
		if (!jointPopupProperty.IsInitialized())
		{
			jointPopupProperty.Initialize(base.Caster.gameObject);
		}
		Vector3 forward = jointPopupProperty.m_jointObject.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		int projectilesPerAnimEvent = this.m_projectilesPerAnimEvent;
		int num = -1;
		int num2 = -1;
		if (this.m_angleRange < 180f)
		{
			if (projectilesPerAnimEvent > 1)
			{
				num = UnityEngine.Random.Range(0, projectilesPerAnimEvent);
				num2 = UnityEngine.Random.Range(0, projectilesPerAnimEvent);
				if (num2 == num)
				{
					num2 = (num2 + 1) % projectilesPerAnimEvent;
				}
			}
			else if (projectilesPerAnimEvent == 1)
			{
				if (this.m_numSpawnAttempts == this.m_spawnIndexEdgeMin)
				{
					num = this.m_spawnIndexEdgeMin;
				}
				else if (this.m_numSpawnAttempts == this.m_spawnIndexEdgeMax)
				{
					num2 = this.m_spawnIndexEdgeMax;
				}
			}
		}
		for (int i = 0; i < projectilesPerAnimEvent; i++)
		{
			float angle = UnityEngine.Random.Range(-this.m_angleRange, this.m_angleRange);
			if (i == num)
			{
				angle = -this.m_angleRange;
			}
			else if (i == num2)
			{
				angle = this.m_angleRange;
			}
			GameObject item = this.CreateProjectileFx(forward, angle, jointPopupProperty);
			this.m_FXs.Add(item);
			if (UnityEngine.Random.Range(0f, 1f) < this.m_extraEdgeShotChance)
			{
				float angle2 = -this.m_angleRange;
				if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
				{
					angle2 = this.m_angleRange;
				}
				GameObject item2 = this.CreateProjectileFx(forward, angle2, jointPopupProperty);
				this.m_FXs.Add(item2);
			}
		}
		if (!string.IsNullOrEmpty(this.m_audioEvent))
		{
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
		if (this.m_hitReactEvent == null)
		{
			if (this.m_lastHitReactEvent == null)
			{
				if (this.m_hitImpactDelayTime > 0f)
				{
					this.m_delayedImpacts.Add(new SimpleAttachedVFXSequence.DelayedImpact(GameTime.time + this.m_hitImpactDelayTime, true));
				}
				else
				{
					this.SpawnImpactFX(true);
				}
			}
		}
		this.m_numSpawnAttempts++;
	}

	private GameObject CreateProjectileFx(Vector3 forward, float angle, JointPopupProperty fxJoint)
	{
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
		Vector3 vector = rotation * forward;
		GameObject gameObject = base.InstantiateFX(this.m_fxPrefab, fxJoint.m_jointObject.transform.position, Quaternion.LookRotation(vector), true, true);
		float projectileDistance = this.GetProjectileDistance(base.Caster.transform.position, vector, this.m_maxDistInSquares * 1.5f);
		Sequence.SetAttribute(gameObject, "projectileDistance", projectileDistance);
		Debug.DrawRay(fxJoint.m_jointObject.transform.position, projectileDistance * vector, Color.red, 5f);
		return gameObject;
	}

	private void SpawnImpactFX(bool lastHit)
	{
		if (base.Targets != null)
		{
			for (int i = 0; i < base.Targets.Length; i++)
			{
				Vector3 targetHitPosition = base.GetTargetHitPosition(i, this.m_hitFxJoint);
				Vector3 hitDirection = targetHitPosition - base.Caster.transform.position;
				hitDirection.y = 0f;
				hitDirection.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
				if (this.m_fxImpactPrefab)
				{
					this.m_fxImpacts.Add(base.InstantiateFX(this.m_fxImpactPrefab, targetHitPosition, Quaternion.identity, true, true));
				}
				string text = this.m_impactAudioEvent;
				if (this.m_alternativeAudioIndex >= 0)
				{
					if (this.m_alternativeAudioIndex < this.m_alternativeImpactAudioEvents.Length)
					{
						text = this.m_alternativeImpactAudioEvents[this.m_alternativeAudioIndex];
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					AudioManager.PostEvent(text, base.Targets[i].gameObject);
				}
				if (base.Targets[i] != null)
				{
					base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo, (!lastHit) ? ActorModelData.RagdollActivation.None : ActorModelData.RagdollActivation.HealthBased, true);
				}
			}
		}
		base.Source.OnSequenceHit(this, base.TargetPos, null);
	}

	private bool ImpactsFinished()
	{
		bool result = true;
		if (this.m_FXs.Count == 0)
		{
			result = false;
		}
		using (List<GameObject>.Enumerator enumerator = this.m_FXs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject gameObject = enumerator.Current;
				if (gameObject.activeSelf)
				{
					result = false;
					goto IL_69;
				}
			}
		}
		IL_69:
		using (List<GameObject>.Enumerator enumerator2 = this.m_fxImpacts.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				GameObject gameObject2 = enumerator2.Current;
				if (gameObject2.activeSelf)
				{
					result = false;
					goto IL_BD;
				}
			}
		}
		IL_BD:
		if (this.m_delayedImpacts.Count > 0)
		{
			result = false;
		}
		return result;
	}

	private void Update()
	{
		if (this.m_initialized)
		{
			for (int i = this.m_delayedImpacts.Count - 1; i >= 0; i--)
			{
				SimpleAttachedVFXSequence.DelayedImpact delayedImpact = this.m_delayedImpacts[i];
				if (GameTime.time >= delayedImpact.m_timeToSpawnImpact)
				{
					this.SpawnImpactFX(delayedImpact.m_lastHit);
					this.m_delayedImpacts.RemoveAt(i);
				}
			}
			if (this.ImpactsFinished())
			{
				base.MarkForRemoval();
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (parameter == this.m_startEventLeft)
		{
			this.SpawnFX(true);
		}
		else if (parameter == this.m_startEventRight)
		{
			this.SpawnFX(false);
		}
		else if (parameter == this.m_hitReactEvent)
		{
			if (this.m_hitImpactDelayTime > 0f)
			{
				this.m_delayedImpacts.Add(new SimpleAttachedVFXSequence.DelayedImpact(GameTime.time + this.m_hitImpactDelayTime, this.m_lastHitReactEvent == null));
			}
			else
			{
				this.SpawnImpactFX(this.m_lastHitReactEvent == null);
			}
		}
		else if (parameter == this.m_lastHitReactEvent)
		{
			if (this.m_hitImpactDelayTime > 0f)
			{
				this.m_delayedImpacts.Add(new SimpleAttachedVFXSequence.DelayedImpact(GameTime.time + this.m_hitImpactDelayTime, true));
			}
			else
			{
				this.SpawnImpactFX(true);
			}
		}
	}

	private void OnDisable()
	{
		if (this.m_FXs != null)
		{
			using (List<GameObject>.Enumerator enumerator = this.m_FXs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = enumerator.Current;
					UnityEngine.Object.Destroy(gameObject.gameObject);
				}
			}
			this.m_FXs = null;
		}
		if (this.m_fxImpacts != null)
		{
			using (List<GameObject>.Enumerator enumerator2 = this.m_fxImpacts.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					GameObject gameObject2 = enumerator2.Current;
					UnityEngine.Object.Destroy(gameObject2.gameObject);
				}
			}
			this.m_fxImpacts = null;
		}
		this.m_initialized = false;
	}

	public class ConeExtraParams : Sequence.IExtraSequenceParams
	{
		public float halfAngleDegrees = -1f;

		public float maxDistInSquares = -1f;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.halfAngleDegrees);
			stream.Serialize(ref this.maxDistInSquares);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.halfAngleDegrees);
			stream.Serialize(ref this.maxDistInSquares);
		}
	}
}
