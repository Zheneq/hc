using System;
using System.Collections.Generic;
using UnityEngine;

public class BouncingShotSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	public GameObject m_ricochetFxPrefab;

	[Tooltip("FX at point(s) of hit")]
	public GameObject m_hitFxPrefab;

	[Tooltip("FX at end of projectile")]
	public GameObject m_fxEndExplosionPrefab;

	[JointPopup("Start position for projectile")]
	public JointPopupProperty m_fxJoint;

	[JointPopup("End joint for projectile")]
	public JointPopupProperty m_targetPosJoint;

	[JointPopup("Hit pos for projectile")]
	public JointPopupProperty m_hitPosJoint;

	protected List<Vector3> m_segmentPts;

	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public UnityEngine.Object m_startEvent;

	public float m_projectileSpeed;

	[AudioEvent(false)]
	[Tooltip("Audio event to play when the projectile initially fires")]
	public string m_audioEvent;

	[AudioEvent(false)]
	[Tooltip("Audio event to play when the projectile hits a target")]
	public string m_impactAudioEvent;

	[Tooltip("Audio event to play when the projectile bounces off a wall")]
	[AudioEvent(false)]
	public string m_ricochetAudioEvent;

	[AudioEvent(false)]
	public string m_endExplosionAudioEvent;

	protected GameObject m_fx;

	private List<GameObject> m_hitFx;

	private List<GameObject> m_ricochetFx;

	private GameObject m_fxEndImpact;

	protected float m_distanceTraveled;

	protected float m_totalTravelDistance;

	private float m_hitDuration;

	private float m_hitDurationLeft;

	protected bool m_reachedDestination;

	protected bool m_impactSequenceHitsDone;

	private bool m_doPositionHitOnBounce;

	private bool m_useOriginalSegmentStartPos;

	private List<Vector3> m_unadjustedSegmentPts;

	private Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> m_laserTargets;

	private ActorData[] m_destinationHitTargets;

	public bool m_useSplineCurve;

	private CRSpline m_spline;

	private List<float> m_splineSpeedModifierPerSegment;

	private float m_splineDistanceTraveled;

	private bool m_spawnFxAttempted;

	protected int m_curSegment = -1;

	protected Dictionary<string, float> m_projectileFxAttributes;

	private List<ActorData> m_actorsHitAlready = new List<ActorData>();

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		if (base.Source.RemoveAtEndOfTurn && GameWideData.Get() != null)
		{
			if (GameWideData.Get().ShouldMakeCasterVisibleOnCast())
			{
				this.m_forceAlwaysVisible = true;
			}
		}
		if (base.Caster != null)
		{
			this.m_fxJoint.Initialize(base.Caster.gameObject);
		}
		GameObject gameObject;
		if (this.m_fxJoint != null)
		{
			if (this.m_fxJoint.m_jointObject != null)
			{
				gameObject = this.m_fxJoint.m_jointObject.gameObject;
				goto IL_BE;
			}
		}
		gameObject = null;
		IL_BE:
		GameObject gameObject2 = gameObject;
		if (gameObject2 == null)
		{
			gameObject2 = base.Caster.gameObject;
		}
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			BouncingShotSequence.ExtraParams extraParams2 = extraSequenceParams as BouncingShotSequence.ExtraParams;
			if (extraParams2 != null)
			{
				this.m_ricochetFx = new List<GameObject>();
				this.m_hitFx = new List<GameObject>();
				this.m_laserTargets = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
				this.m_segmentPts = new List<Vector3>();
				this.m_destinationHitTargets = extraParams2.destinationHitTargets;
				this.m_doPositionHitOnBounce = extraParams2.doPositionHitOnBounce;
				this.m_useOriginalSegmentStartPos = extraParams2.useOriginalSegmentStartPos;
				this.m_hitDuration = Sequence.GetFXDuration(this.m_hitFxPrefab);
				if (extraParams2.segmentPts.Count <= 0)
				{
					goto IL_1CC;
				}
				if (!this.m_useOriginalSegmentStartPos)
				{
					goto IL_1CC;
				}
				Vector3 value = extraParams2.segmentPts[0];
				value.y = gameObject2.transform.position.y;
				extraParams2.segmentPts[0] = value;
				IL_1E4:
				using (List<Vector3>.Enumerator enumerator = extraParams2.segmentPts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Vector3 item = enumerator.Current;
						this.m_segmentPts.Add(item);
					}
				}
				this.m_unadjustedSegmentPts = new List<Vector3>(this.m_segmentPts);
				foreach (KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> keyValuePair in extraParams2.laserTargets)
				{
					AreaEffectUtils.BouncingLaserInfo value2 = keyValuePair.Value;
					value2.m_endpointIndex++;
					this.m_laserTargets[keyValuePair.Key] = value2;
				}
				for (int j = 0; j < this.m_segmentPts.Count - 1; j++)
				{
					this.m_totalTravelDistance += (this.m_segmentPts[j + 1] - this.m_segmentPts[j]).magnitude;
				}
				goto IL_305;
				IL_1CC:
				this.m_segmentPts.Add(gameObject2.transform.position);
				goto IL_1E4;
			}
			IL_305:
			if (extraSequenceParams is Sequence.FxAttributeParam)
			{
				Sequence.FxAttributeParam fxAttributeParam = extraSequenceParams as Sequence.FxAttributeParam;
				if (fxAttributeParam != null)
				{
					if (fxAttributeParam.m_paramNameCode != Sequence.FxAttributeParam.ParamNameCode.None)
					{
						string attributeName = fxAttributeParam.GetAttributeName();
						float paramValue = fxAttributeParam.m_paramValue;
						if (fxAttributeParam.m_paramTarget == Sequence.FxAttributeParam.ParamTarget.MainVfx)
						{
							if (this.m_projectileFxAttributes == null)
							{
								this.m_projectileFxAttributes = new Dictionary<string, float>();
							}
							if (!this.m_projectileFxAttributes.ContainsKey(attributeName))
							{
								this.m_projectileFxAttributes.Add(attributeName, paramValue);
							}
						}
					}
				}
			}
		}
	}

	public override void FinishSetup()
	{
		if (this.m_startEvent == null)
		{
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
			for (int i = 0; i < this.m_hitFx.Count; i++)
			{
				UnityEngine.Object.Destroy(this.m_hitFx[i]);
			}
			this.m_hitFx.Clear();
		}
		if (this.m_ricochetFx != null)
		{
			for (int j = 0; j < this.m_ricochetFx.Count; j++)
			{
				UnityEngine.Object.Destroy(this.m_ricochetFx[j]);
			}
			this.m_ricochetFx.Clear();
		}
		if (this.m_fxEndImpact != null)
		{
			UnityEngine.Object.Destroy(this.m_fxEndImpact);
			this.m_fxEndImpact = null;
		}
		this.m_initialized = false;
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
		if (this.m_fx != null)
		{
			if (this.m_initialized)
			{
				if (this.m_hitDurationLeft > 0f)
				{
					this.m_hitDurationLeft -= GameTime.deltaTime;
				}
				if (this.m_reachedDestination)
				{
					this.m_fx.SetActive(false);
					this.PlayRemainingHitFX();
					if (this.m_hitDurationLeft <= 0f)
					{
						base.MarkForRemoval();
					}
				}
				else
				{
					this.UpdateProjectileFX();
				}
			}
		}
	}

	private void SpawnRicochetFX(Vector3 pos, Vector3 ricochetDir)
	{
		if (this.m_ricochetFxPrefab != null)
		{
			if (!(this.m_fx == null))
			{
				if (!this.m_fx.activeInHierarchy)
				{
					goto IL_7A;
				}
			}
			Quaternion rotation = Quaternion.LookRotation(ricochetDir);
			this.m_ricochetFx.Add(base.InstantiateFX(this.m_ricochetFxPrefab, pos, rotation, true, true));
		}
		IL_7A:
		if (!string.IsNullOrEmpty(this.m_ricochetAudioEvent))
		{
			AudioManager.PostEvent(this.m_ricochetAudioEvent, base.Caster.gameObject);
		}
	}

	protected virtual void UpdateProjectileFX()
	{
		this.m_distanceTraveled += this.m_projectileSpeed * GameTime.deltaTime;
		Vector3 curDelta = Vector3.zero;
		Vector3 a = Vector3.zero;
		float num = 0f;
		bool flag = false;
		int curEndPtIndex = 0;
		int i = 0;
		while (i < this.m_segmentPts.Count)
		{
			if (i == this.m_segmentPts.Count - 1)
			{
				flag = true;
			}
			else
			{
				a = this.m_segmentPts[i];
				curDelta = this.m_segmentPts[i + 1] - this.m_segmentPts[i];
				curEndPtIndex = i + 1;
				if (this.m_distanceTraveled >= num + curDelta.magnitude)
				{
					num += curDelta.magnitude;
					if (i > this.m_curSegment)
					{
						if (i > 0)
						{
							Vector3 vector = this.m_segmentPts[i] - this.m_segmentPts[i - 1];
							Vector3 ricochetDir = (curDelta.normalized + vector.normalized) * 0.5f;
							this.SpawnRicochetFX(this.m_segmentPts[i], ricochetDir);
							if (this.m_doPositionHitOnBounce)
							{
								base.Source.OnSequenceHit(this, this.m_unadjustedSegmentPts[i], null);
							}
						}
						this.m_curSegment = i;
					}
					i++;
					continue;
				}
				if (i > this.m_curSegment)
				{
					if (i > 0)
					{
						Vector3 vector2 = this.m_segmentPts[i] - this.m_segmentPts[i - 1];
						Vector3 ricochetDir2 = (curDelta.normalized + vector2.normalized) * 0.5f;
						this.SpawnRicochetFX(this.m_segmentPts[i], ricochetDir2);
						if (i > 0)
						{
							if (this.m_doPositionHitOnBounce)
							{
								base.Source.OnSequenceHit(this, this.m_unadjustedSegmentPts[i], null);
							}
						}
					}
				}
				this.m_curSegment = i;
			}
			break;
		}
		if (flag)
		{
			this.m_reachedDestination = true;
			if (this.m_useSplineCurve)
			{
				this.m_fx.transform.position = this.m_spline.Interp(1f);
			}
			else
			{
				this.m_fx.transform.position = this.m_segmentPts[this.m_segmentPts.Count - 1];
			}
			this.OnDestinationSequenceHits();
			this.SpawnEndExplosionFX(this.m_fx.transform.position, this.m_fx.transform.rotation);
		}
		else
		{
			Quaternion rotation = default(Quaternion);
			Vector3 vector3;
			if (this.m_useSplineCurve && this.m_totalTravelDistance > 0f)
			{
				int index = this.m_spline.Section(this.m_splineDistanceTraveled / this.m_totalTravelDistance);
				this.m_splineDistanceTraveled += this.m_projectileSpeed * this.m_splineSpeedModifierPerSegment[index] * GameTime.deltaTime;
				float t = this.m_splineDistanceTraveled / this.m_totalTravelDistance;
				vector3 = this.m_spline.Interp(t);
				rotation.SetLookRotation((vector3 - this.m_fx.transform.position).normalized);
			}
			else
			{
				float d = this.m_distanceTraveled - num;
				vector3 = a + curDelta.normalized * d;
				rotation.SetLookRotation(curDelta.normalized);
			}
			this.m_fx.transform.position = vector3;
			this.m_fx.transform.rotation = rotation;
		}
		if (this.m_useSplineCurve)
		{
			this.DoHitForSplineProjectile(this.m_fx.transform.forward);
		}
		else
		{
			this.ProcessHitFX(curEndPtIndex, curDelta);
		}
	}

	private void PlayRemainingHitFX()
	{
		Vector3 curDelta = this.m_segmentPts[this.m_segmentPts.Count - 1] - this.m_segmentPts[this.m_segmentPts.Count - 2];
		if (this.m_useSplineCurve)
		{
			curDelta = this.m_segmentPts[this.m_segmentPts.Count - 2] - this.m_segmentPts[this.m_segmentPts.Count - 3];
		}
		curDelta.Normalize();
		using (Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>.Enumerator enumerator = this.m_laserTargets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> keyValuePair = enumerator.Current;
				this.SpawnHitFX(keyValuePair.Key, curDelta);
			}
		}
		this.m_laserTargets.Clear();
	}

	private void ProcessHitFX(int curEndPtIndex, Vector3 curDelta)
	{
		List<ActorData> list = new List<ActorData>();
		using (Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>.Enumerator enumerator = this.m_laserTargets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> keyValuePair = enumerator.Current;
				if (keyValuePair.Value.m_endpointIndex == curEndPtIndex)
				{
					Vector3 a = this.m_segmentPts[keyValuePair.Value.m_endpointIndex];
					Vector3 lhs = a - this.m_fx.transform.position;
					lhs[1] = 0f;
					Vector3 rhs = keyValuePair.Key.transform.position - this.m_fx.transform.position;
					rhs[1] = 0f;
					if (Vector3.Dot(lhs, rhs) < 0f)
					{
						this.SpawnHitFX(keyValuePair.Key, curDelta);
						list.Add(keyValuePair.Key);
					}
				}
				else if (keyValuePair.Value.m_endpointIndex < curEndPtIndex)
				{
					this.SpawnHitFX(keyValuePair.Key, curDelta);
					list.Add(keyValuePair.Key);
				}
			}
		}
		using (List<ActorData>.Enumerator enumerator2 = list.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				ActorData key = enumerator2.Current;
				this.m_laserTargets.Remove(key);
			}
		}
	}

	private void DoHitForSplineProjectile(Vector3 dir)
	{
		if (base.Targets != null)
		{
			foreach (ActorData actorData in base.Targets)
			{
				if (actorData != null)
				{
					if (!this.m_actorsHitAlready.Contains(actorData))
					{
						Vector3 rhs = actorData.transform.position - this.m_fx.transform.position;
						if (Vector3.Dot(dir, rhs) < 0f)
						{
							this.SpawnHitFX(actorData, dir);
						}
					}
				}
			}
		}
	}

	private Vector3 GetHitPosition(ActorData actorData)
	{
		Vector3 result = actorData.transform.position + Vector3.up;
		GameObject gameObject = actorData.gameObject.FindInChildren(Sequence.s_defaultHitAttachJoint, 0);
		if (gameObject != null)
		{
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
		if (!this.m_actorsHitAlready.Contains(actorData))
		{
			Vector3 targetHitPosition = base.GetTargetHitPosition(actorData, this.m_hitPosJoint);
			Vector3 normalized = curDelta.normalized;
			Vector3 position = this.m_fx.transform.position;
			if (this.m_hitFxPrefab)
			{
				Quaternion rotation = Quaternion.LookRotation(normalized);
				this.m_hitFx.Add(base.InstantiateFX(this.m_hitFxPrefab, targetHitPosition, rotation, true, true));
				this.m_hitDurationLeft = this.m_hitDuration;
			}
			if (!string.IsNullOrEmpty(this.m_impactAudioEvent))
			{
				AudioManager.PostEvent(this.m_impactAudioEvent, this.m_fx.gameObject);
			}
			ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, normalized);
			base.Source.OnSequenceHit(this, actorData, impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
			this.m_actorsHitAlready.Add(actorData);
		}
	}

	protected virtual void SpawnFX()
	{
		if (this.m_fxJoint == null)
		{
			Debug.LogError(base.name + " fxJoint is null spawnFxAttempted = " + this.m_spawnFxAttempted);
		}
		else if (this.m_fxJoint.m_jointObject == null)
		{
			Debug.LogError(base.name + " fxJoint.m_jointObject is null spawnFxAttempted = " + this.m_spawnFxAttempted);
		}
		else if (this.m_fxJoint.m_jointObject.gameObject == null)
		{
			Debug.LogError(base.name + " m_fxJoint.m_jointObject.gameObject is null spawnFxAttempted = " + this.m_spawnFxAttempted);
		}
		if (!(base.Caster == null))
		{
			if (!(base.Caster.gameObject == null))
			{
				goto IL_11E;
			}
		}
		Debug.LogError(base.name + " caster or caster gameObject is null spawnFxAttempted = " + this.m_spawnFxAttempted);
		IL_11E:
		GameObject gameObject;
		if (this.m_fxJoint != null)
		{
			if (this.m_fxJoint.m_jointObject != null)
			{
				gameObject = this.m_fxJoint.m_jointObject.gameObject;
				goto IL_158;
			}
		}
		gameObject = null;
		IL_158:
		GameObject gameObject2 = gameObject;
		if (gameObject2 == null)
		{
			gameObject2 = base.Caster.gameObject;
		}
		if (this.m_segmentPts == null)
		{
			Debug.LogError(base.name + " segment points is null, spawnFxAttempted = " + this.m_spawnFxAttempted);
		}
		this.m_spawnFxAttempted = true;
		if (this.m_useOriginalSegmentStartPos)
		{
			Vector3 value = this.m_segmentPts[0];
			value.y = gameObject2.transform.position.y;
			this.m_segmentPts[0] = value;
		}
		else
		{
			this.m_segmentPts[0] = gameObject2.transform.position;
		}
		for (int i = 1; i < this.m_segmentPts.Count; i++)
		{
			Vector3 value2 = this.m_segmentPts[i];
			value2.y = this.m_segmentPts[i - 1].y;
			this.m_segmentPts[i] = value2;
		}
		Vector3 position = this.m_segmentPts[0];
		Vector3 lookRotation = this.m_segmentPts[1] - this.m_segmentPts[0];
		lookRotation.Normalize();
		Quaternion rotation = default(Quaternion);
		rotation.SetLookRotation(lookRotation);
		if (this.m_useSplineCurve)
		{
			this.m_splineSpeedModifierPerSegment = new List<float>();
			for (int j = 1; j < this.m_segmentPts.Count; j++)
			{
				float magnitude = (this.m_segmentPts[j] - this.m_segmentPts[j - 1]).magnitude;
				if (magnitude > 0f)
				{
					float num = this.m_totalTravelDistance / magnitude;
					this.m_splineSpeedModifierPerSegment.Add(num / (float)(this.m_segmentPts.Count - 1));
				}
				else
				{
					this.m_splineSpeedModifierPerSegment.Add(1f);
				}
			}
			this.m_segmentPts.Insert(1, this.m_segmentPts[0]);
			this.m_segmentPts.Add(this.m_segmentPts[this.m_segmentPts.Count - 1]);
			this.m_spline = new CRSpline(this.m_segmentPts.ToArray());
			position = this.m_spline.Interp(0f);
		}
		this.m_fx = base.InstantiateFX(this.m_fxPrefab, position, rotation, true, true);
		if (this.m_fx != null)
		{
			if (this.m_projectileFxAttributes != null)
			{
				using (Dictionary<string, float>.Enumerator enumerator = this.m_projectileFxAttributes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, float> keyValuePair = enumerator.Current;
						Sequence.SetAttribute(this.m_fx, keyValuePair.Key, keyValuePair.Value);
					}
				}
			}
		}
		if (!string.IsNullOrEmpty(this.m_audioEvent))
		{
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			this.SpawnFX();
		}
	}

	private void OnDestinationSequenceHits()
	{
		if (!this.m_impactSequenceHitsDone)
		{
			base.Source.OnSequenceHit(this, base.TargetPos, null);
			if (this.m_doPositionHitOnBounce)
			{
				base.Source.OnSequenceHit(this, this.m_unadjustedSegmentPts[this.m_unadjustedSegmentPts.Count - 1], null);
			}
			if (this.m_destinationHitTargets != null)
			{
				foreach (ActorData actorData in this.m_destinationHitTargets)
				{
					if (actorData != null)
					{
						if (!this.m_actorsHitAlready.Contains(actorData))
						{
							ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(1f, this.m_segmentPts[this.m_segmentPts.Count - 1]);
							base.Source.OnSequenceHit(this, actorData, impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
							this.m_actorsHitAlready.Add(actorData);
						}
					}
				}
			}
			if (this.m_hitDuration > 0f)
			{
				if (this.m_hitDurationLeft == 0f)
				{
					this.m_hitDurationLeft = this.m_hitDuration;
				}
			}
			this.m_impactSequenceHitsDone = true;
		}
	}

	private void SpawnEndExplosionFX(Vector3 impactPos, Quaternion impactRot)
	{
		if (this.m_fxEndImpact == null)
		{
			if (this.m_fxEndExplosionPrefab != null)
			{
				if (this.m_fx != null)
				{
					bool flag = false;
					if (base.Targets != null)
					{
						if (base.Targets.Length > 0)
						{
							if (base.Caster != null)
							{
								for (int i = 0; i < base.Targets.Length; i++)
								{
									if (base.Caster.GetTeam() != base.Targets[i].GetTeam())
									{
										flag = true;
										break;
									}
								}
							}
						}
					}
					if (!flag)
					{
						if (!base.LastDesiredVisible())
						{
							goto IL_152;
						}
					}
					this.m_fxEndImpact = base.InstantiateFX(this.m_fxEndExplosionPrefab, impactPos, impactRot, true, true);
					if (flag)
					{
						this.m_fxEndImpact.transform.parent = base.gameObject.transform;
					}
					this.m_hitDurationLeft = this.m_hitDuration;
					IL_152:
					if (!string.IsNullOrEmpty(this.m_endExplosionAudioEvent))
					{
						AudioManager.PostEvent(this.m_endExplosionAudioEvent, this.m_fx.gameObject);
					}
				}
			}
		}
	}

	public class ExtraParams : Sequence.IExtraSequenceParams
	{
		public bool doPositionHitOnBounce;

		public bool useOriginalSegmentStartPos;

		public List<Vector3> segmentPts;

		public Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> laserTargets;

		public ActorData[] destinationHitTargets;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.doPositionHitOnBounce);
			stream.Serialize(ref this.useOriginalSegmentStartPos);
			sbyte b = (sbyte)this.segmentPts.Count;
			stream.Serialize(ref b);
			if ((int)b > 0)
			{
				float y = this.segmentPts[0].y;
				stream.Serialize(ref y);
			}
			for (int i = 0; i < (int)b; i++)
			{
				Vector3 vector = this.segmentPts[i];
				stream.Serialize(ref vector.x);
				stream.Serialize(ref vector.z);
			}
			sbyte b2 = (sbyte)this.laserTargets.Count;
			stream.Serialize(ref b2);
			using (Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>.Enumerator enumerator = this.laserTargets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> keyValuePair = enumerator.Current;
					ActorData key = keyValuePair.Key;
					short num = (short)key.ActorIndex;
					stream.Serialize(ref num);
					sbyte b3 = (sbyte)keyValuePair.Value.m_endpointIndex;
					stream.Serialize(ref b3);
				}
			}
			sbyte b4;
			if (this.destinationHitTargets == null)
			{
				b4 = 0;
			}
			else
			{
				b4 = (sbyte)this.destinationHitTargets.Length;
			}
			stream.Serialize(ref b4);
			for (int j = 0; j < (int)b4; j++)
			{
				ActorData actorData = this.destinationHitTargets[j];
				short num2 = (short)actorData.ActorIndex;
				stream.Serialize(ref num2);
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.doPositionHitOnBounce);
			stream.Serialize(ref this.useOriginalSegmentStartPos);
			sbyte b = 0;
			stream.Serialize(ref b);
			float y = 0f;
			if ((int)b > 0)
			{
				stream.Serialize(ref y);
			}
			this.segmentPts = new List<Vector3>((int)b);
			for (int i = 0; i < (int)b; i++)
			{
				Vector3 zero = Vector3.zero;
				float x = 0f;
				float z = 0f;
				stream.Serialize(ref x);
				stream.Serialize(ref z);
				zero.x = x;
				zero.y = y;
				zero.z = z;
				this.segmentPts.Add(zero);
			}
			sbyte b2 = 0;
			stream.Serialize(ref b2);
			this.laserTargets = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>((int)b2);
			for (int j = 0; j < (int)b2; j++)
			{
				short actorIndex = (short)ActorData.s_invalidActorIndex;
				stream.Serialize(ref actorIndex);
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex((int)actorIndex);
				sbyte b3 = -1;
				Vector3 zero2 = Vector3.zero;
				stream.Serialize(ref b3);
				if (actorData != null)
				{
					AreaEffectUtils.BouncingLaserInfo value = new AreaEffectUtils.BouncingLaserInfo(zero2, (int)b3);
					this.laserTargets.Add(actorData, value);
				}
			}
			sbyte b4 = 0;
			stream.Serialize(ref b4);
			if ((int)b4 == 0)
			{
				this.destinationHitTargets = null;
			}
			else
			{
				this.destinationHitTargets = new ActorData[(int)b4];
				for (int k = 0; k < (int)b4; k++)
				{
					short actorIndex2 = (short)ActorData.s_invalidActorIndex;
					stream.Serialize(ref actorIndex2);
					ActorData actorData2 = GameFlowData.Get().FindActorByActorIndex((int)actorIndex2);
					this.destinationHitTargets[k] = actorData2;
				}
			}
		}
	}
}
