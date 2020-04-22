using System.Collections.Generic;
using UnityEngine;

public class HealLaserSequence : Sequence
{
	public class ExtraParams : IExtraSequenceParams
	{
		public Vector3 endPos;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref endPos);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref endPos);
		}
	}

	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[Tooltip("FX at point(s) of impact")]
	public GameObject m_hitFxPrefab;

	public GameObject m_healHitFxPrefab;

	[JointPopup("Start position for projectile")]
	public JointPopupProperty m_fxJoint;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public Object m_startEvent;

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

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		if (base.Source.RemoveAtEndOfTurn)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GameWideData.Get() != null && GameWideData.Get().ShouldMakeCasterVisibleOnCast())
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
				m_forceAlwaysVisible = true;
			}
		}
		if (base.Caster != null)
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
			m_fxJoint.Initialize(base.Caster.gameObject);
		}
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ExtraParams extraParams2 = extraSequenceParams as ExtraParams;
			if (extraParams2 != null)
			{
				m_hitFx = new Dictionary<ActorData, GameObject>();
				CalculateHitDuration();
				m_endPos = extraParams2.endPos;
			}
		}
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
		if (m_hitFx == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_hitFx = new Dictionary<ActorData, GameObject>();
			CalculateHitDuration();
		}
		if (base.Caster != null)
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
			if (!m_fxJoint.IsInitialized())
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
				m_fxJoint.Initialize(base.Caster.gameObject);
			}
		}
		if (!(m_fx != null))
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
			if (!m_initialized)
			{
				return;
			}
			if (m_hitDurationLeft > 0f)
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
				m_hitDurationLeft -= GameTime.deltaTime;
			}
			if (m_reachedDestination)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (m_fx.activeSelf)
						{
							m_fx.SetActive(false);
							PlayRemainingHitFX();
							if (m_hitDurationLeft <= 0f)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
										MarkForRemoval();
										return;
									}
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

	private void UpdateProjectileFX()
	{
		m_distanceTraveled += m_projectileSpeed * GameTime.deltaTime;
		m_fx.transform.position = m_startPos + m_fx.transform.forward * m_distanceTraveled;
		Vector3 lhs = m_endPos - m_fx.transform.position;
		if (Vector3.Dot(lhs, m_fx.transform.forward) < 0f)
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
			m_fx.transform.position = m_endPos;
			m_reachedDestination = true;
		}
		ProcessHitFX();
	}

	private void PlayRemainingHitFX()
	{
		ActorData[] targets = base.Targets;
		foreach (ActorData actorData in targets)
		{
			if (!m_hitFx.ContainsKey(actorData))
			{
				SpawnHitFX(actorData, m_fx.transform.forward);
			}
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			base.Source.OnSequenceHit(this, base.TargetPos);
			return;
		}
	}

	private void ProcessHitFX()
	{
		Vector3 lhs = m_endPos - m_fx.transform.position;
		ActorData[] targets = base.Targets;
		foreach (ActorData actorData in targets)
		{
			if (m_hitFx.ContainsKey(actorData))
			{
				continue;
			}
			Vector3 rhs = actorData.transform.position - m_fx.transform.position;
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				SpawnHitFX(actorData, m_fx.transform.forward);
			}
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private Vector3 GetHitPosition(ActorData actorData)
	{
		Vector3 result = actorData.transform.position + Vector3.up;
		GameObject gameObject = actorData.gameObject.FindInChildren(Sequence.s_defaultHitAttachJoint);
		if (gameObject != null)
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
		Vector3 hitPosition = GetHitPosition(actorData);
		bool flag = actorData.GetTeam() == base.Caster.GetTeam();
		m_hitFx[actorData] = null;
		if ((bool)m_hitFxPrefab)
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
			if (!flag && IsHitFXVisible(actorData))
			{
				m_hitFx[actorData] = InstantiateFX(m_hitFxPrefab, hitPosition, Quaternion.identity);
				m_hitFx[actorData].transform.parent = base.transform;
				m_hitDurationLeft = m_hitDuration;
				goto IL_0133;
			}
		}
		if ((bool)m_healHitFxPrefab && flag)
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
			if (IsHitFXVisible(actorData))
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
				m_hitFx[actorData] = InstantiateFX(m_healHitFxPrefab, hitPosition, Quaternion.identity);
				m_hitFx[actorData].transform.parent = base.transform;
				m_hitDurationLeft = m_hitDuration;
			}
		}
		goto IL_0133;
		IL_0133:
		if (!string.IsNullOrEmpty(m_impactAudioEvent))
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
			if (!flag)
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
				AudioManager.PostEvent(m_impactAudioEvent, m_fx.gameObject);
				goto IL_01b3;
			}
		}
		if (!string.IsNullOrEmpty(m_impactHealAudioEvent))
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
			if (flag)
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
				AudioManager.PostEvent(m_impactHealAudioEvent, m_fx.gameObject);
			}
		}
		goto IL_01b3;
		IL_01b3:
		Vector3 normalized = curDelta.normalized;
		Vector3 position = m_fx.transform.position;
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, normalized);
		base.Source.OnSequenceHit(this, actorData, impulseInfo);
	}

	private void SpawnFX()
	{
		m_startPos = m_fxJoint.m_jointObject.transform.position;
		Vector3 lookRotation = m_endPos - m_startPos;
		lookRotation.Normalize();
		Quaternion rotation = default(Quaternion);
		rotation.SetLookRotation(lookRotation);
		m_fx = InstantiateFX(m_fxPrefab, m_startPos, rotation);
		if (!string.IsNullOrEmpty(m_audioEvent))
		{
			AudioManager.PostEvent(m_audioEvent, base.Caster.gameObject);
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (!(m_startEvent == parameter))
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
			while (true)
			{
				switch (6)
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
			using (Dictionary<ActorData, GameObject>.ValueCollection.Enumerator enumerator = m_hitFx.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject current = enumerator.Current;
					Object.Destroy(current);
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			m_hitFx.Clear();
		}
		m_initialized = false;
	}

	private void CalculateHitDuration()
	{
		float fXDuration = Sequence.GetFXDuration(m_hitFxPrefab);
		float fXDuration2 = Sequence.GetFXDuration(m_healHitFxPrefab);
		m_hitDuration = Mathf.Max(fXDuration, fXDuration2);
	}
}
