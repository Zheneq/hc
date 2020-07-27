using System.Collections.Generic;
using UnityEngine;

public class BouncingShotSequence : Sequence
{
	public class ExtraParams : IExtraSequenceParams
	{
		public bool doPositionHitOnBounce;

		public bool useOriginalSegmentStartPos;

		public List<Vector3> segmentPts;

		public Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> laserTargets;

		public ActorData[] destinationHitTargets;

		public override string Json()
		{
			string segments = "";
			if (!segmentPts.IsNullOrEmpty())
			{
				foreach (var e in segmentPts)
				{
					segments += (segments.Length == 0 ? "" : ", ") + e;
				}
			}
			string targets = "";
			if (!laserTargets.IsNullOrEmpty())
			{
				foreach (var e in laserTargets)
				{
					targets += (targets.Length == 0 ? "" : ", ") + $"{{\"{e.Key.DisplayName}\": " +
						$"{{\"segmentOrigin\": {e.Value.m_segmentOrigin}, \"endpointIndex\": {e.Value.m_endpointIndex}}}}}";
				}
			}
			string destHit = "";
			if (!destinationHitTargets.IsNullOrEmpty())
			{
				foreach (var e in destinationHitTargets)
				{
					destHit += (destHit.Length == 0 ? "" : ", ") + $"\"{e?.DisplayName ?? "none"}\"";
				}
			}

			return $"{{" +
				$"\"doPositionHitOnBounce\": {doPositionHitOnBounce}, " +
				$"\"useOriginalSegmentStartPos\": {useOriginalSegmentStartPos}, " +
				$"\"segmentPts\": [{segments}], " +
				$"\"laserTargets\": [{targets}], " +
				$"\"destinationHitTargets\": [{destHit}]" +
				$"}}";
		}

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref doPositionHitOnBounce);
			stream.Serialize(ref useOriginalSegmentStartPos);
			sbyte value = (sbyte)segmentPts.Count;
			stream.Serialize(ref value);
			if (value > 0)
			{
				Vector3 vector = segmentPts[0];
				float value2 = vector.y;
				stream.Serialize(ref value2);
			}
			for (int i = 0; i < value; i++)
			{
				Vector3 vector2 = segmentPts[i];
				stream.Serialize(ref vector2.x);
				stream.Serialize(ref vector2.z);
			}
			while (true)
			{
				sbyte value3 = (sbyte)laserTargets.Count;
				stream.Serialize(ref value3);
				using (Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>.Enumerator enumerator = laserTargets.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> current = enumerator.Current;
						ActorData key = current.Key;
						short value4 = (short)key.ActorIndex;
						stream.Serialize(ref value4);
						AreaEffectUtils.BouncingLaserInfo value5 = current.Value;
						sbyte value6 = (sbyte)value5.m_endpointIndex;
						stream.Serialize(ref value6);
					}
				}
				sbyte value7;
				if (destinationHitTargets == null)
				{
					value7 = 0;
				}
				else
				{
					value7 = (sbyte)destinationHitTargets.Length;
				}
				stream.Serialize(ref value7);
				for (int j = 0; j < value7; j++)
				{
					ActorData actorData = destinationHitTargets[j];
					short value8 = (short)actorData.ActorIndex;
					stream.Serialize(ref value8);
				}
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref doPositionHitOnBounce);
			stream.Serialize(ref useOriginalSegmentStartPos);
			sbyte value = 0;
			stream.Serialize(ref value);
			float value2 = 0f;
			if (value > 0)
			{
				stream.Serialize(ref value2);
			}
			segmentPts = new List<Vector3>(value);
			for (int i = 0; i < value; i++)
			{
				Vector3 zero = Vector3.zero;
				float value3 = 0f;
				float value4 = 0f;
				stream.Serialize(ref value3);
				stream.Serialize(ref value4);
				zero.x = value3;
				zero.y = value2;
				zero.z = value4;
				segmentPts.Add(zero);
			}
			while (true)
			{
				sbyte value5 = 0;
				stream.Serialize(ref value5);
				laserTargets = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>(value5);
				for (int j = 0; j < value5; j++)
				{
					short value6 = (short)ActorData.s_invalidActorIndex;
					stream.Serialize(ref value6);
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex(value6);
					sbyte value7 = -1;
					Vector3 zero2 = Vector3.zero;
					stream.Serialize(ref value7);
					if (actorData != null)
					{
						AreaEffectUtils.BouncingLaserInfo value8 = new AreaEffectUtils.BouncingLaserInfo(zero2, value7);
						laserTargets.Add(actorData, value8);
					}
				}
				sbyte value9 = 0;
				stream.Serialize(ref value9);
				if (value9 == 0)
				{
					destinationHitTargets = null;
					return;
				}
				destinationHitTargets = new ActorData[value9];
				for (int k = 0; k < value9; k++)
				{
					short value10 = (short)ActorData.s_invalidActorIndex;
					stream.Serialize(ref value10);
					ActorData actorData2 = GameFlowData.Get().FindActorByActorIndex(value10);
					destinationHitTargets[k] = actorData2;
				}
				return;
			}
		}
	}

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
	public Object m_startEvent;

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

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		if (base.Source.RemoveAtEndOfTurn && GameWideData.Get() != null)
		{
			if (GameWideData.Get().ShouldMakeCasterVisibleOnCast())
			{
				m_forceAlwaysVisible = true;
			}
		}
		if (base.Caster != null)
		{
			m_fxJoint.Initialize(base.Caster.gameObject);
		}
		object obj;
		if (m_fxJoint != null)
		{
			if (m_fxJoint.m_jointObject != null)
			{
				obj = m_fxJoint.m_jointObject.gameObject;
				goto IL_00be;
			}
		}
		obj = null;
		goto IL_00be;
		IL_00be:
		GameObject gameObject = (GameObject)obj;
		if (gameObject == null)
		{
			gameObject = base.Caster.gameObject;
		}
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ExtraParams extraParams2 = extraSequenceParams as ExtraParams;
			if (extraParams2 != null)
			{
				m_ricochetFx = new List<GameObject>();
				m_hitFx = new List<GameObject>();
				m_laserTargets = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
				m_segmentPts = new List<Vector3>();
				m_destinationHitTargets = extraParams2.destinationHitTargets;
				m_doPositionHitOnBounce = extraParams2.doPositionHitOnBounce;
				m_useOriginalSegmentStartPos = extraParams2.useOriginalSegmentStartPos;
				m_hitDuration = Sequence.GetFXDuration(m_hitFxPrefab);
				if (extraParams2.segmentPts.Count > 0)
				{
					if (m_useOriginalSegmentStartPos)
					{
						Vector3 value = extraParams2.segmentPts[0];
						Vector3 position = gameObject.transform.position;
						value.y = position.y;
						extraParams2.segmentPts[0] = value;
						goto IL_01e4;
					}
				}
				m_segmentPts.Add(gameObject.transform.position);
				goto IL_01e4;
			}
			goto IL_0305;
			IL_01e4:
			using (List<Vector3>.Enumerator enumerator = extraParams2.segmentPts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Vector3 current = enumerator.Current;
					m_segmentPts.Add(current);
				}
			}
			m_unadjustedSegmentPts = new List<Vector3>(m_segmentPts);
			foreach (KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> laserTarget in extraParams2.laserTargets)
			{
				AreaEffectUtils.BouncingLaserInfo value2 = laserTarget.Value;
				value2.m_endpointIndex++;
				m_laserTargets[laserTarget.Key] = value2;
			}
			for (int i = 0; i < m_segmentPts.Count - 1; i++)
			{
				m_totalTravelDistance += (m_segmentPts[i + 1] - m_segmentPts[i]).magnitude;
			}
			goto IL_0305;
			IL_0305:
			if (extraSequenceParams is FxAttributeParam)
			{
				FxAttributeParam fxAttributeParam = extraSequenceParams as FxAttributeParam;
				if (fxAttributeParam != null)
				{
					if (fxAttributeParam.m_paramNameCode != 0)
					{
						string attributeName = fxAttributeParam.GetAttributeName();
						float paramValue = fxAttributeParam.m_paramValue;
						if (fxAttributeParam.m_paramTarget == FxAttributeParam.ParamTarget.MainVfx)
						{
							if (m_projectileFxAttributes == null)
							{
								m_projectileFxAttributes = new Dictionary<string, float>();
							}
							if (!m_projectileFxAttributes.ContainsKey(attributeName))
							{
								m_projectileFxAttributes.Add(attributeName, paramValue);
							}
						}
					}
				}
			}
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public override void FinishSetup()
	{
		if (!(m_startEvent == null))
		{
			return;
		}
		while (true)
		{
			SpawnFX();
			return;
		}
	}

	private void OnDisable()
	{
		if (m_fx != null)
		{
			Object.Destroy(m_fx);
			m_fx = null;
		}
		if (m_hitFx != null)
		{
			for (int i = 0; i < m_hitFx.Count; i++)
			{
				Object.Destroy(m_hitFx[i]);
			}
			m_hitFx.Clear();
		}
		if (m_ricochetFx != null)
		{
			for (int j = 0; j < m_ricochetFx.Count; j++)
			{
				Object.Destroy(m_ricochetFx[j]);
			}
			m_ricochetFx.Clear();
		}
		if (m_fxEndImpact != null)
		{
			Object.Destroy(m_fxEndImpact);
			m_fxEndImpact = null;
		}
		m_initialized = false;
	}

	internal override Vector3 GetSequencePos()
	{
		if (m_fx != null)
		{
			return m_fx.transform.position;
		}
		return Vector3.zero;
	}

	private void Update()
	{
		ProcessSequenceVisibility();
		if (!(m_fx != null))
		{
			return;
		}
		while (true)
		{
			if (!m_initialized)
			{
				return;
			}
			if (m_hitDurationLeft > 0f)
			{
				m_hitDurationLeft -= GameTime.deltaTime;
			}
			if (m_reachedDestination)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_fx.SetActive(false);
						PlayRemainingHitFX();
						if (m_hitDurationLeft <= 0f)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									MarkForRemoval();
									return;
								}
							}
						}
						return;
					}
				}
			}
			UpdateProjectileFX();
			return;
		}
	}

	private void SpawnRicochetFX(Vector3 pos, Vector3 ricochetDir)
	{
		if (m_ricochetFxPrefab != null)
		{
			if (!(m_fx == null))
			{
				if (!m_fx.activeInHierarchy)
				{
					goto IL_007a;
				}
			}
			Quaternion rotation = Quaternion.LookRotation(ricochetDir);
			m_ricochetFx.Add(InstantiateFX(m_ricochetFxPrefab, pos, rotation));
		}
		goto IL_007a;
		IL_007a:
		if (string.IsNullOrEmpty(m_ricochetAudioEvent))
		{
			return;
		}
		while (true)
		{
			AudioManager.PostEvent(m_ricochetAudioEvent, base.Caster.gameObject);
			return;
		}
	}

	protected virtual void UpdateProjectileFX()
	{
		m_distanceTraveled += m_projectileSpeed * GameTime.deltaTime;
		Vector3 curDelta = Vector3.zero;
		Vector3 a = Vector3.zero;
		float num = 0f;
		bool flag = false;
		int curEndPtIndex = 0;
		int num2 = 0;
		while (true)
		{
			if (num2 < m_segmentPts.Count)
			{
				if (num2 == m_segmentPts.Count - 1)
				{
					flag = true;
					break;
				}
				a = m_segmentPts[num2];
				curDelta = m_segmentPts[num2 + 1] - m_segmentPts[num2];
				curEndPtIndex = num2 + 1;
				if (m_distanceTraveled < num + curDelta.magnitude)
				{
					if (num2 > m_curSegment)
					{
						if (num2 > 0)
						{
							Vector3 vector = m_segmentPts[num2] - m_segmentPts[num2 - 1];
							Vector3 ricochetDir = (curDelta.normalized + vector.normalized) * 0.5f;
							SpawnRicochetFX(m_segmentPts[num2], ricochetDir);
							if (num2 > 0)
							{
								if (m_doPositionHitOnBounce)
								{
									base.Source.OnSequenceHit(this, m_unadjustedSegmentPts[num2]);
								}
							}
						}
					}
					m_curSegment = num2;
					break;
				}
				num += curDelta.magnitude;
				if (num2 > m_curSegment)
				{
					if (num2 > 0)
					{
						Vector3 vector2 = m_segmentPts[num2] - m_segmentPts[num2 - 1];
						Vector3 ricochetDir2 = (curDelta.normalized + vector2.normalized) * 0.5f;
						SpawnRicochetFX(m_segmentPts[num2], ricochetDir2);
						if (m_doPositionHitOnBounce)
						{
							base.Source.OnSequenceHit(this, m_unadjustedSegmentPts[num2]);
						}
					}
					m_curSegment = num2;
				}
				num2++;
				continue;
			}
			break;
		}
		if (flag)
		{
			m_reachedDestination = true;
			if (m_useSplineCurve)
			{
				m_fx.transform.position = m_spline.Interp(1f);
			}
			else
			{
				m_fx.transform.position = m_segmentPts[m_segmentPts.Count - 1];
			}
			OnDestinationSequenceHits();
			SpawnEndExplosionFX(m_fx.transform.position, m_fx.transform.rotation);
		}
		else
		{
			Quaternion rotation = default(Quaternion);
			Vector3 vector3;
			if (m_useSplineCurve && m_totalTravelDistance > 0f)
			{
				int index = m_spline.Section(m_splineDistanceTraveled / m_totalTravelDistance);
				m_splineDistanceTraveled += m_projectileSpeed * m_splineSpeedModifierPerSegment[index] * GameTime.deltaTime;
				float t = m_splineDistanceTraveled / m_totalTravelDistance;
				vector3 = m_spline.Interp(t);
				rotation.SetLookRotation((vector3 - m_fx.transform.position).normalized);
			}
			else
			{
				float d = m_distanceTraveled - num;
				vector3 = a + curDelta.normalized * d;
				rotation.SetLookRotation(curDelta.normalized);
			}
			m_fx.transform.position = vector3;
			m_fx.transform.rotation = rotation;
		}
		if (m_useSplineCurve)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					DoHitForSplineProjectile(m_fx.transform.forward);
					return;
				}
			}
		}
		ProcessHitFX(curEndPtIndex, curDelta);
	}

	private void PlayRemainingHitFX()
	{
		Vector3 curDelta = m_segmentPts[m_segmentPts.Count - 1] - m_segmentPts[m_segmentPts.Count - 2];
		if (m_useSplineCurve)
		{
			curDelta = m_segmentPts[m_segmentPts.Count - 2] - m_segmentPts[m_segmentPts.Count - 3];
		}
		curDelta.Normalize();
		using (Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>.Enumerator enumerator = m_laserTargets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SpawnHitFX(enumerator.Current.Key, curDelta);
			}
		}
		m_laserTargets.Clear();
	}

	private void ProcessHitFX(int curEndPtIndex, Vector3 curDelta)
	{
		List<ActorData> list = new List<ActorData>();
		using (Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>.Enumerator enumerator = m_laserTargets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> current = enumerator.Current;
				AreaEffectUtils.BouncingLaserInfo value = current.Value;
				if (value.m_endpointIndex == curEndPtIndex)
				{
					List<Vector3> segmentPts = m_segmentPts;
					AreaEffectUtils.BouncingLaserInfo value2 = current.Value;
					Vector3 a = segmentPts[value2.m_endpointIndex];
					Vector3 lhs = a - m_fx.transform.position;
					lhs[1] = 0f;
					Vector3 rhs = current.Key.transform.position - m_fx.transform.position;
					rhs[1] = 0f;
					if (Vector3.Dot(lhs, rhs) < 0f)
					{
						SpawnHitFX(current.Key, curDelta);
						list.Add(current.Key);
					}
				}
				else
				{
					AreaEffectUtils.BouncingLaserInfo value3 = current.Value;
					if (value3.m_endpointIndex < curEndPtIndex)
					{
						SpawnHitFX(current.Key, curDelta);
						list.Add(current.Key);
					}
				}
			}
		}
		using (List<ActorData>.Enumerator enumerator2 = list.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				ActorData current2 = enumerator2.Current;
				m_laserTargets.Remove(current2);
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void DoHitForSplineProjectile(Vector3 dir)
	{
		if (base.Targets == null)
		{
			return;
		}
		while (true)
		{
			ActorData[] targets = base.Targets;
			foreach (ActorData actorData in targets)
			{
				if (!(actorData != null))
				{
					continue;
				}
				if (!m_actorsHitAlready.Contains(actorData))
				{
					Vector3 rhs = actorData.transform.position - m_fx.transform.position;
					if (Vector3.Dot(dir, rhs) < 0f)
					{
						SpawnHitFX(actorData, dir);
					}
				}
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private Vector3 GetHitPosition(ActorData actorData)
	{
		Vector3 result = actorData.transform.position + Vector3.up;
		GameObject gameObject = actorData.gameObject.FindInChildren(Sequence.s_defaultHitAttachJoint);
		if (gameObject != null)
		{
			result = gameObject.transform.position;
		}
		else
		{
			gameObject = actorData.gameObject.FindInChildren(Sequence.s_defaultFallbackHitAttachJoint);
			if (gameObject != null)
			{
				result = gameObject.transform.position;
			}
		}
		return result;
	}

	private void SpawnHitFX(ActorData actorData, Vector3 curDelta)
	{
		if (m_actorsHitAlready.Contains(actorData))
		{
			return;
		}
		while (true)
		{
			Vector3 targetHitPosition = GetTargetHitPosition(actorData, m_hitPosJoint);
			Vector3 normalized = curDelta.normalized;
			Vector3 position = m_fx.transform.position;
			if ((bool)m_hitFxPrefab)
			{
				Quaternion rotation = Quaternion.LookRotation(normalized);
				m_hitFx.Add(InstantiateFX(m_hitFxPrefab, targetHitPosition, rotation));
				m_hitDurationLeft = m_hitDuration;
			}
			if (!string.IsNullOrEmpty(m_impactAudioEvent))
			{
				AudioManager.PostEvent(m_impactAudioEvent, m_fx.gameObject);
			}
			ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, normalized);
			base.Source.OnSequenceHit(this, actorData, impulseInfo);
			m_actorsHitAlready.Add(actorData);
			return;
		}
	}

	protected virtual void SpawnFX()
	{
		if (m_fxJoint == null)
		{
			Debug.LogError(base.name + " fxJoint is null spawnFxAttempted = " + m_spawnFxAttempted);
		}
		else if (m_fxJoint.m_jointObject == null)
		{
			Debug.LogError(base.name + " fxJoint.m_jointObject is null spawnFxAttempted = " + m_spawnFxAttempted);
		}
		else if (m_fxJoint.m_jointObject.gameObject == null)
		{
			Debug.LogError(base.name + " m_fxJoint.m_jointObject.gameObject is null spawnFxAttempted = " + m_spawnFxAttempted);
		}
		if (!(base.Caster == null))
		{
			if (!(base.Caster.gameObject == null))
			{
				goto IL_011e;
			}
		}
		Debug.LogError(base.name + " caster or caster gameObject is null spawnFxAttempted = " + m_spawnFxAttempted);
		goto IL_011e;
		IL_0158:
		object obj;
		GameObject gameObject = (GameObject)obj;
		if (gameObject == null)
		{
			gameObject = base.Caster.gameObject;
		}
		if (m_segmentPts == null)
		{
			Debug.LogError(base.name + " segment points is null, spawnFxAttempted = " + m_spawnFxAttempted);
		}
		m_spawnFxAttempted = true;
		if (m_useOriginalSegmentStartPos)
		{
			Vector3 value = m_segmentPts[0];
			Vector3 position = gameObject.transform.position;
			value.y = position.y;
			m_segmentPts[0] = value;
		}
		else
		{
			m_segmentPts[0] = gameObject.transform.position;
		}
		for (int i = 1; i < m_segmentPts.Count; i++)
		{
			Vector3 value2 = m_segmentPts[i];
			Vector3 vector = m_segmentPts[i - 1];
			value2.y = vector.y;
			m_segmentPts[i] = value2;
		}
		while (true)
		{
			Vector3 position2 = m_segmentPts[0];
			Vector3 lookRotation = m_segmentPts[1] - m_segmentPts[0];
			lookRotation.Normalize();
			Quaternion rotation = default(Quaternion);
			rotation.SetLookRotation(lookRotation);
			if (m_useSplineCurve)
			{
				m_splineSpeedModifierPerSegment = new List<float>();
				for (int j = 1; j < m_segmentPts.Count; j++)
				{
					float magnitude = (m_segmentPts[j] - m_segmentPts[j - 1]).magnitude;
					if (magnitude > 0f)
					{
						float num = m_totalTravelDistance / magnitude;
						m_splineSpeedModifierPerSegment.Add(num / (float)(m_segmentPts.Count - 1));
					}
					else
					{
						m_splineSpeedModifierPerSegment.Add(1f);
					}
				}
				m_segmentPts.Insert(1, m_segmentPts[0]);
				m_segmentPts.Add(m_segmentPts[m_segmentPts.Count - 1]);
				m_spline = new CRSpline(m_segmentPts.ToArray());
				position2 = m_spline.Interp(0f);
			}
			m_fx = InstantiateFX(m_fxPrefab, position2, rotation);
			if (m_fx != null)
			{
				if (m_projectileFxAttributes != null)
				{
					using (Dictionary<string, float>.Enumerator enumerator = m_projectileFxAttributes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, float> current = enumerator.Current;
							Sequence.SetAttribute(m_fx, current.Key, current.Value);
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(m_audioEvent))
			{
				while (true)
				{
					AudioManager.PostEvent(m_audioEvent, base.Caster.gameObject);
					return;
				}
			}
			return;
		}
		IL_011e:
		if (m_fxJoint != null)
		{
			if (m_fxJoint.m_jointObject != null)
			{
				obj = m_fxJoint.m_jointObject.gameObject;
				goto IL_0158;
			}
		}
		obj = null;
		goto IL_0158;
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (!(m_startEvent == parameter))
		{
			return;
		}
		while (true)
		{
			SpawnFX();
			return;
		}
	}

	private void OnDestinationSequenceHits()
	{
		if (m_impactSequenceHitsDone)
		{
			return;
		}
		while (true)
		{
			base.Source.OnSequenceHit(this, base.TargetPos);
			if (m_doPositionHitOnBounce)
			{
				base.Source.OnSequenceHit(this, m_unadjustedSegmentPts[m_unadjustedSegmentPts.Count - 1]);
			}
			if (m_destinationHitTargets != null)
			{
				ActorData[] destinationHitTargets = m_destinationHitTargets;
				foreach (ActorData actorData in destinationHitTargets)
				{
					if (!(actorData != null))
					{
						continue;
					}
					if (!m_actorsHitAlready.Contains(actorData))
					{
						ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(1f, m_segmentPts[m_segmentPts.Count - 1]);
						base.Source.OnSequenceHit(this, actorData, impulseInfo);
						m_actorsHitAlready.Add(actorData);
					}
				}
			}
			if (m_hitDuration > 0f)
			{
				if (m_hitDurationLeft == 0f)
				{
					m_hitDurationLeft = m_hitDuration;
				}
			}
			m_impactSequenceHitsDone = true;
			return;
		}
	}

	private void SpawnEndExplosionFX(Vector3 impactPos, Quaternion impactRot)
	{
		if (!(m_fxEndImpact == null))
		{
			return;
		}
		while (true)
		{
			if (!(m_fxEndExplosionPrefab != null))
			{
				return;
			}
			while (true)
			{
				if (!(m_fx != null))
				{
					return;
				}
				while (true)
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
						if (!LastDesiredVisible())
						{
							goto IL_0152;
						}
					}
					m_fxEndImpact = InstantiateFX(m_fxEndExplosionPrefab, impactPos, impactRot);
					if (flag)
					{
						m_fxEndImpact.transform.parent = base.gameObject.transform;
					}
					m_hitDurationLeft = m_hitDuration;
					goto IL_0152;
					IL_0152:
					if (!string.IsNullOrEmpty(m_endExplosionAudioEvent))
					{
						while (true)
						{
							AudioManager.PostEvent(m_endExplosionAudioEvent, m_fx.gameObject);
							return;
						}
					}
					return;
				}
			}
		}
	}
}
