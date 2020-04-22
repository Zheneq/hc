using System.Collections.Generic;
using UnityEngine;

public abstract class SplineProjectileSequence : Sequence
{
	public class DelayedProjectileExtraParams : IExtraSequenceParams
	{
		public float startDelay = -1f;

		public int curIndex;

		public int maxIndex;

		public bool skipImpactFx;

		public bool useOverrideStartPos;

		public Vector3 overrideStartPos = Vector3.zero;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			sbyte value = (sbyte)curIndex;
			sbyte value2 = (sbyte)maxIndex;
			stream.Serialize(ref startDelay);
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			stream.Serialize(ref skipImpactFx);
			stream.Serialize(ref useOverrideStartPos);
			if (!useOverrideStartPos)
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
				stream.Serialize(ref overrideStartPos);
				return;
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte value = 0;
			sbyte value2 = 0;
			stream.Serialize(ref startDelay);
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			stream.Serialize(ref skipImpactFx);
			stream.Serialize(ref useOverrideStartPos);
			if (useOverrideStartPos)
			{
				stream.Serialize(ref overrideStartPos);
			}
			curIndex = value;
			maxIndex = value2;
		}
	}

	public class MultiEventExtraParams : IExtraSequenceParams
	{
		public int eventNumberToKeyOffOf;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			short value = (short)eventNumberToKeyOffOf;
			stream.Serialize(ref value);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			short value = 0;
			stream.Serialize(ref value);
			eventNumberToKeyOffOf = value;
		}
	}

	public class ProjectilePropertyParams : IExtraSequenceParams
	{
		public float projectileWidthInWorld = -1f;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref projectileWidthInWorld);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref projectileWidthInWorld);
		}
	}

	[Tooltip("Main FX prefab.")]
	[Separator("Projectile FX", true)]
	public GameObject m_fxPrefab;

	[Tooltip("FX at point(s) of impact")]
	[Separator("Impact FX (at end of projectile)", true)]
	public GameObject m_fxImpactPrefab;

	[Tooltip("Use different FX/sound at impact if no Target")]
	public bool m_useDifferentImpactIfNoTarget;

	public GameObject m_fxNoTargetImpactPrefab;

	[Tooltip("If spawning impact vfx, whether it's always visible")]
	public bool m_impactAlwaysVisibleIfSpawned;

	public bool m_spawnImpactAtFXDespawn;

	public bool m_skipImpactFxIfNoTargetActor;

	[Separator("Target Hit FX (as hits happen on targets)", true)]
	[Tooltip("Fx at targets hit")]
	public GameObject m_targetHitFxPrefab;

	public HitVFXSpawnTeam m_hitFxTeamFilter = HitVFXSpawnTeam.AllExcludeCaster;

	[JointPopup("Start position for projectile", order = 1)]
	[Space(10f, order = 0)]
	[Separator("Joints", true, order = 2)]
	public JointPopupProperty m_fxJoint;

	public ReferenceModelType m_jointReferenceType;

	[JointPopup("FX attach joint for Target Hit Fx")]
	[Header("-- joint for hit location --")]
	public JointPopupProperty m_hitPosJoint;

	public bool m_targetHitFxAttachToJoint;

	[Separator("Anim Event: to spawn projectile", "orange", order = 2)]
	[AnimEventPicker(order = 3)]
	[Space(10f, order = 0)]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.", order = 1)]
	public Object m_startEvent;

	[AudioEvent(false, order = 1)]
	[Separator("Audio Events", "orange", order = 2)]
	[Space(10f, order = 0)]
	public string m_audioEvent;

	[Header("    (( Audio Event on END of projectile ))")]
	[AudioEvent(false)]
	public string m_impactAudioEvent;

	[AudioEvent(false)]
	[Header("    (( only applicable if using different impact vfx if no target ))")]
	public string m_impactAudioEventIfNoTarget;

	[Header("    (( Audio Events on hitting target (useful for piercing projectiles) ))")]
	[AudioEvent(false)]
	public string m_targetHitAudioEvent;

	[Header("    (( Audio Events on hitting [ally] target (useful for piercing projectiles) ))")]
	[AudioEvent(false)]
	public string m_allyTargetHitAudioEvent;

	[Separator("Delay Timing", true)]
	public float m_hitReactDelay;

	public float m_startDelay;

	[Separator("Speed / Rotation", true)]
	public float m_projectileSpeed;

	public float m_projectileAcceleration;

	public float m_splineFractionUntilImpact = 1f;

	public bool m_alignToTravelDir = true;

	public bool m_keepAlignmentOnHorizontalPlane;

	public float m_projectileTravelHitWidth = -1f;

	private bool m_startEventHappened;

	private float m_impactDuration;

	private int m_eventNumberToKeyOffOf;

	private int m_numStartEventsReceived;

	protected bool m_doHitsAsProjectileTravels = true;

	private float m_hitReactTime = -1f;

	private bool m_hitReactPlayed;

	protected GameObject m_fx;

	private GameObject m_fxImpact;

	private List<GameObject> m_targetHitFx = new List<GameObject>();

	protected CRSpline m_spline;

	private float m_curSplineSpeed;

	private float m_splineSpeed;

	private float m_splineAcceleration;

	protected float m_splineTraveled;

	private float m_impactDurationLeft;

	private int m_curIndex;

	private int m_maxIndex;

	private bool m_skipImpactFx;

	private bool m_ignoreStartEvent;

	protected bool m_useOverrideStartPos;

	protected Vector3 m_overrideStartPos = Vector3.zero;

	protected bool m_markForRemovalAfterImpact = true;

	protected List<ActorData> m_actorsAlreadyHit = new List<ActorData>();

	protected Dictionary<string, float> m_projectileFxAttributes;

	protected Dictionary<string, float> m_impactFxAttributes;

	protected Vector3 m_fxSpawnPos;

	protected float m_totalTravelDist2D = 10f;

	internal abstract Vector3[] GetSplinePath(int curIndex, int maxIndex);

	protected int GetCurrentSplineSegment()
	{
		return m_spline.Section(m_splineTraveled);
	}

	public override void FinishSetup()
	{
		m_impactDuration = Sequence.GetFXDuration(GetImpactFxPrefab());
		if (m_startEvent == null || m_ignoreStartEvent)
		{
			ScheduleFX();
		}
	}

	protected virtual GameObject GetProjectileFxPrefab()
	{
		return m_fxPrefab;
	}

	protected virtual GameObject GetImpactFxPrefab()
	{
		if (m_useDifferentImpactIfNoTarget)
		{
			if (base.Targets != null)
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
				if (base.Targets.Length != 0)
				{
					goto IL_0042;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			return m_fxNoTargetImpactPrefab;
		}
		goto IL_0042;
		IL_0042:
		return m_fxImpactPrefab;
	}

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		if (base.Source.RemoveAtEndOfTurn)
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
			if (GameWideData.Get() != null)
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
				if (GameWideData.Get().ShouldMakeCasterVisibleOnCast())
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					m_forceAlwaysVisible = true;
				}
			}
		}
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (extraSequenceParams is DelayedProjectileExtraParams)
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
				DelayedProjectileExtraParams delayedProjectileExtraParams = extraSequenceParams as DelayedProjectileExtraParams;
				if (delayedProjectileExtraParams == null)
				{
					continue;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				m_curIndex = delayedProjectileExtraParams.curIndex;
				m_maxIndex = delayedProjectileExtraParams.maxIndex;
				if (delayedProjectileExtraParams.startDelay >= 0f)
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
					m_startDelay = delayedProjectileExtraParams.startDelay;
				}
				m_skipImpactFx = delayedProjectileExtraParams.skipImpactFx;
				m_useOverrideStartPos = delayedProjectileExtraParams.useOverrideStartPos;
				m_overrideStartPos = delayedProjectileExtraParams.overrideStartPos;
			}
			else if (extraSequenceParams is MultiEventExtraParams)
			{
				MultiEventExtraParams multiEventExtraParams = extraSequenceParams as MultiEventExtraParams;
				if (multiEventExtraParams != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					m_eventNumberToKeyOffOf = multiEventExtraParams.eventNumberToKeyOffOf;
				}
			}
			else if (extraSequenceParams is ProjectilePropertyParams)
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
				ProjectilePropertyParams projectilePropertyParams = extraSequenceParams as ProjectilePropertyParams;
				if (projectilePropertyParams != null)
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
					m_projectileTravelHitWidth = projectilePropertyParams.projectileWidthInWorld;
				}
			}
			else if (extraSequenceParams is FxAttributeParam)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				FxAttributeParam fxAttributeParam = extraSequenceParams as FxAttributeParam;
				if (fxAttributeParam == null || fxAttributeParam.m_paramNameCode == FxAttributeParam.ParamNameCode.None)
				{
					continue;
				}
				string attributeName = fxAttributeParam.GetAttributeName();
				float paramValue = fxAttributeParam.m_paramValue;
				if (fxAttributeParam.m_paramTarget == FxAttributeParam.ParamTarget.MainVfx)
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
					if (m_projectileFxAttributes == null)
					{
						m_projectileFxAttributes = new Dictionary<string, float>();
					}
					if (!m_projectileFxAttributes.ContainsKey(attributeName))
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
						m_projectileFxAttributes.Add(attributeName, paramValue);
					}
				}
				else if (fxAttributeParam.m_paramTarget == FxAttributeParam.ParamTarget.ImpactVfx)
				{
					if (m_impactFxAttributes == null)
					{
						m_impactFxAttributes = new Dictionary<string, float>();
					}
					if (!m_impactFxAttributes.ContainsKey(attributeName))
					{
						m_impactFxAttributes.Add(attributeName, paramValue);
					}
				}
			}
			else if (extraSequenceParams is SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam)
			{
				SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam ignoreStartEventExtraParam = extraSequenceParams as SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam;
				m_ignoreStartEvent = ignoreStartEventExtraParam.ignoreStartEvent;
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void OnDisable()
	{
		OnSequenceDisable();
	}

	protected virtual void OnSequenceDisable()
	{
		if (m_fx != null)
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
			Object.Destroy(m_fx);
			m_fx = null;
		}
		if (m_fxImpact != null)
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
			Object.Destroy(m_fxImpact);
			m_fxImpact = null;
		}
		if (m_targetHitFx != null)
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
			if (m_targetHitFx.Count > 0)
			{
				for (int i = 0; i < m_targetHitFx.Count; i++)
				{
					Object.Destroy(m_targetHitFx[i]);
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				m_targetHitFx.Clear();
			}
		}
		m_initialized = false;
	}

	protected virtual void SpawnImpactFX(Vector3 impactPos, Quaternion impactRot)
	{
		if (!m_skipImpactFx)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_skipImpactFxIfNoTargetActor)
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
				if (base.Targets != null)
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
					if (base.Targets.Length <= 0)
					{
						goto IL_0305;
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			GameObject impactFxPrefab = GetImpactFxPrefab();
			if ((bool)impactFxPrefab)
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
				if (m_impactAlwaysVisibleIfSpawned)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					m_fxImpact = Object.Instantiate(impactFxPrefab, impactPos, impactRot);
					m_fxImpact.transform.parent = base.gameObject.transform;
				}
				else
				{
					bool flag = false;
					if (base.Targets != null && base.Targets.Length > 0)
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
						if (base.Caster != null)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							int num = 0;
							while (true)
							{
								if (num < base.Targets.Length)
								{
									if (base.Caster.GetTeam() != base.Targets[num].GetTeam())
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
										flag = true;
										break;
									}
									num++;
									continue;
								}
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								break;
							}
						}
					}
					if (!flag)
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
						if (!LastDesiredVisible())
						{
							goto IL_019d;
						}
					}
					m_fxImpact = InstantiateFX(impactFxPrefab, impactPos, impactRot);
					if (flag)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						m_fxImpact.transform.parent = base.gameObject.transform;
					}
				}
				goto IL_019d;
			}
			goto IL_027b;
		}
		goto IL_0305;
		IL_02df:
		if (!string.IsNullOrEmpty(m_impactAudioEvent))
		{
			AudioManager.PostEvent(m_impactAudioEvent, m_fx.gameObject);
		}
		goto IL_0305;
		IL_027b:
		if (!m_useDifferentImpactIfNoTarget)
		{
			goto IL_02df;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		if (base.Targets != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (base.Targets.Length != 0)
			{
				goto IL_02df;
			}
		}
		if (!string.IsNullOrEmpty(m_impactAudioEventIfNoTarget))
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
			AudioManager.PostEvent(m_impactAudioEventIfNoTarget, m_fx.gameObject);
		}
		goto IL_0305;
		IL_0305:
		m_hitReactTime = GameTime.time + m_hitReactDelay;
		return;
		IL_019d:
		m_impactDurationLeft = m_impactDuration;
		if (m_fxImpact != null)
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
			if (m_impactFxAttributes != null)
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
				foreach (KeyValuePair<string, float> impactFxAttribute in m_impactFxAttributes)
				{
					Sequence.SetAttribute(m_fxImpact, impactFxAttribute.Key, impactFxAttribute.Value);
				}
			}
		}
		if (m_fxImpact != null)
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
			if (m_fxImpact.GetComponent<FriendlyEnemyVFXSelector>() != null)
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
				m_fxImpact.GetComponent<FriendlyEnemyVFXSelector>().Setup(GetFoFObservingTeam());
			}
		}
		goto IL_027b;
	}

	protected virtual Team GetFoFObservingTeam()
	{
		return base.Caster.GetTeam();
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (!(m_startEvent == parameter))
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
			if (m_eventNumberToKeyOffOf == m_numStartEventsReceived)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						ScheduleFX();
						return;
					}
				}
			}
			m_numStartEventsReceived++;
			return;
		}
	}

	private void ScheduleFX()
	{
		m_startEventHappened = true;
	}

	protected virtual void SpawnFX()
	{
		Vector3[] splinePath = GetSplinePath(m_curIndex, m_maxIndex);
		m_spline = new CRSpline(splinePath);
		Vector3 a = m_spline.Interp(0.05f);
		Vector3 lookRotation = a - splinePath[1];
		if (lookRotation.magnitude > 1E-05f)
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
			lookRotation.Normalize();
		}
		else
		{
			lookRotation = Vector3.forward;
		}
		Quaternion rotation = default(Quaternion);
		if (m_alignToTravelDir)
		{
			if (m_keepAlignmentOnHorizontalPlane)
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
				lookRotation.y = 0f;
			}
			if (lookRotation.sqrMagnitude > 0f)
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
				rotation.SetLookRotation(lookRotation);
			}
		}
		if (m_projectileSpeed <= 0f)
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
			m_projectileSpeed = 10f;
		}
		float num = (splinePath[1] - splinePath[2]).magnitude + (splinePath[2] - splinePath[3]).magnitude;
		float num2 = num / m_projectileSpeed;
		if (num2 == 0f)
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
			m_splineSpeed = m_projectileSpeed;
		}
		else
		{
			m_splineSpeed = 1f / num2;
		}
		m_splineAcceleration = m_projectileAcceleration * m_splineSpeed / m_projectileSpeed;
		if (m_projectileAcceleration == 0f)
		{
			m_curSplineSpeed = m_splineSpeed;
		}
		else
		{
			m_curSplineSpeed = 0f;
		}
		m_fxSpawnPos = splinePath[1];
		m_totalTravelDist2D = VectorUtils.HorizontalPlaneDistInWorld(m_fxSpawnPos, splinePath[3]);
		m_fx = InstantiateFX(GetProjectileFxPrefab(), m_fxSpawnPos, rotation, true, false);
		if (m_fx != null && m_projectileFxAttributes != null)
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
			using (Dictionary<string, float>.Enumerator enumerator = m_projectileFxAttributes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, float> current = enumerator.Current;
					Sequence.SetAttribute(m_fx, current.Key, current.Value);
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		if (string.IsNullOrEmpty(m_audioEvent))
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
			AudioManager.PostEvent(m_audioEvent, base.Caster.gameObject);
			return;
		}
	}

	protected virtual void SpawnTargetHitFx(ActorData target)
	{
		if (!(target != null) || !(m_fx != null))
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
			if (!IsHitFXVisibleWrtTeamFilter(target, m_hitFxTeamFilter))
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
				if (m_targetHitFxPrefab != null)
				{
					GameObject gameObject = m_hitPosJoint.FindJointObject(target.gameObject);
					Vector3 vector;
					if (gameObject != null)
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
						vector = gameObject.transform.position;
					}
					else
					{
						vector = target.GetTravelBoardSquareWorldPosition();
					}
					Vector3 position = vector;
					GameObject gameObject2 = InstantiateFX(m_targetHitFxPrefab, position, m_fx.transform.rotation);
					if (gameObject2 != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_targetHitFxAttachToJoint)
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
							AttachToBone(gameObject2, gameObject);
							gameObject2.transform.localPosition = Vector3.zero;
							gameObject2.transform.localScale = Vector3.one;
							gameObject2.transform.localRotation = Quaternion.identity;
						}
						else
						{
							gameObject2.transform.parent = base.transform;
						}
						m_targetHitFx.Add(gameObject2);
					}
				}
				if (target.GetTeam() == base.Caster.GetTeam())
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
					if (!string.IsNullOrEmpty(m_allyTargetHitAudioEvent))
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								AudioManager.PostEvent(m_allyTargetHitAudioEvent, target.gameObject);
								return;
							}
						}
					}
				}
				if (!string.IsNullOrEmpty(m_targetHitAudioEvent))
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						AudioManager.PostEvent(m_targetHitAudioEvent, target.gameObject);
						return;
					}
				}
				return;
			}
		}
	}

	private void Update()
	{
		OnUpdate();
	}

	protected virtual void OnUpdate()
	{
		ProcessSequenceVisibility();
		if (!m_initialized || !m_startEventHappened)
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
			if (m_fx == null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						m_startDelay -= GameTime.deltaTime;
						if (m_startDelay <= 0f)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
								{
									GameObject referenceModel = GetReferenceModel(base.Caster, m_jointReferenceType);
									if (referenceModel != null)
									{
										m_fxJoint.Initialize(referenceModel);
									}
									SpawnFX();
									return;
								}
								}
							}
						}
						return;
					}
				}
			}
			if (m_fx.activeSelf)
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
				if (m_fx != null)
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
					if (m_fx.GetComponent<FriendlyEnemyVFXSelector>() != null)
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
						m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(GetFoFObservingTeam());
					}
				}
				m_curSplineSpeed += m_splineAcceleration;
				m_curSplineSpeed = Mathf.Min(m_splineSpeed, m_curSplineSpeed);
				m_splineTraveled += m_curSplineSpeed * GameTime.deltaTime;
				if (m_splineTraveled < m_splineFractionUntilImpact)
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
					Vector3 vector = m_spline.Interp(m_splineTraveled);
					if (m_alignToTravelDir)
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
						if (GameTime.scale != 0f)
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
							Quaternion rotation = default(Quaternion);
							Vector3 vector2 = vector - m_fx.transform.position;
							if (m_keepAlignmentOnHorizontalPlane)
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
								vector2.y = 0f;
							}
							if (vector2.sqrMagnitude > 0f)
							{
								rotation.SetLookRotation(vector2.normalized);
							}
							m_fx.transform.rotation = rotation;
						}
					}
					if (m_doHitsAsProjectileTravels && base.Targets != null)
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
						Vector3 vector3 = vector - m_fx.transform.position;
						vector3.Normalize();
						float actorTargetingRadius = AreaEffectUtils.GetActorTargetingRadius();
						ActorData[] targets = base.Targets;
						foreach (ActorData actorData in targets)
						{
							if (!(actorData != null))
							{
								continue;
							}
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (m_actorsAlreadyHit.Contains(actorData))
							{
								continue;
							}
							Vector3 rhs = actorData.transform.position - m_fx.transform.position;
							if (!(Vector3.Dot(vector3, rhs) < 0f))
							{
								continue;
							}
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!(m_projectileTravelHitWidth <= 0f))
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!(AreaEffectUtils.PointToLineDistance2D(actorData.transform.position, vector - vector3, vector + vector3) <= m_projectileTravelHitWidth + actorTargetingRadius))
								{
									continue;
								}
							}
							Vector3 position = m_fx.transform.position;
							ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, vector3);
							base.Source.OnSequenceHit(this, actorData, impulseInfo);
							m_actorsAlreadyHit.Add(actorData);
							SpawnTargetHitFx(actorData);
						}
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					m_fx.transform.position = vector;
				}
				else
				{
					if (m_splineFractionUntilImpact > 0f)
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
						Vector3 position2 = m_spline.Interp(m_splineFractionUntilImpact);
						m_fx.transform.position = position2;
					}
					if (m_spawnImpactAtFXDespawn)
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
						SpawnImpactFX(m_fx.transform.position, m_fx.transform.rotation);
					}
					else
					{
						SpawnImpactFX(base.TargetPos, Quaternion.identity);
					}
					m_fx.SetActive(false);
					if (GetImpactFxPrefab() == null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_markForRemovalAfterImpact)
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
							if (m_hitReactPlayed)
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
								MarkForRemoval();
							}
						}
					}
				}
			}
			if (GameTime.time > m_hitReactTime)
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
				if (m_hitReactTime > 0f && !m_hitReactPlayed)
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
					m_hitReactPlayed = true;
					Vector3 forward = m_fx.transform.forward;
					ActorData[] targets2 = base.Targets;
					foreach (ActorData actorData2 in targets2)
					{
						if (actorData2 != null && !m_actorsAlreadyHit.Contains(actorData2))
						{
							Vector3 position3 = m_fx.transform.position;
							ActorModelData.ImpulseInfo impulseInfo2 = new ActorModelData.ImpulseInfo(position3, forward);
							base.Source.OnSequenceHit(this, actorData2, impulseInfo2);
							m_actorsAlreadyHit.Add(actorData2);
							SpawnTargetHitFx(actorData2);
						}
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					CallHitSequenceOnTargets(base.TargetPos, 1f, m_actorsAlreadyHit, !m_doHitsAsProjectileTravels);
				}
			}
			if (!(m_fxImpact != null))
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
				if (!m_fxImpact.activeSelf)
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
					if (m_impactDurationLeft > 0f)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								m_impactDurationLeft -= GameTime.deltaTime;
								return;
							}
						}
					}
					if (!m_markForRemovalAfterImpact)
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
						if (m_hitReactPlayed)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								MarkForRemoval();
								return;
							}
						}
						return;
					}
				}
			}
		}
	}
}
