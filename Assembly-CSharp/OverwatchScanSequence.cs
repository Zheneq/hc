using UnityEngine;

public class OverwatchScanSequence : Sequence
{
	public enum JointAttribute
	{
		StartPoint,
		EndPoint,
		TopPoint
	}

	private Vector3 m_startPos = Vector3.zero;

	private Vector3 m_endPos = Vector3.zero;

	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[JointPopup("FX attach joint on the caster")]
	public JointPopupProperty m_fxCasterJoint;

	public ReferenceModelType m_fxCasterJointReferenceType = ReferenceModelType.TempSatellite;

	public JointAttribute m_fxAttributeForJoint = JointAttribute.TopPoint;

	public bool m_syncHeightOfEndToStart;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public Object m_startEvent;

	[Tooltip("Height above the ground to place the FX.")]
	public float m_heightOffset = 0.1f;

	[Tooltip("Distance in front of the startpoint to start")]
	public float m_startOffset = 0.5f;

	[AudioEvent(false)]
	public string m_audioEvent;

	private GameObject m_fx;

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			GroundLineSequence.ExtraParams extraParams2 = extraSequenceParams as GroundLineSequence.ExtraParams;
			if (extraParams2 != null)
			{
				Vector3 b = new Vector3(0f, m_heightOffset, 0f);
				Vector3 a = extraParams2.endPos - extraParams2.startPos;
				a.Normalize();
				m_startPos = extraParams2.startPos + b + a * m_startOffset;
				m_endPos = extraParams2.endPos + b;
			}
		}
		while (true)
		{
			switch (2)
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

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			ProcessSequenceVisibility();
			if (m_fx != null)
			{
				if (m_fx.GetComponent<FriendlyEnemyVFXSelector>() != null)
				{
					m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.GetTeam());
				}
			}
			if (m_fxCasterJoint.IsInitialized())
			{
				if (m_fxAttributeForJoint == JointAttribute.StartPoint)
				{
					m_startPos = m_fxCasterJoint.m_jointObject.transform.position;
				}
				else if (m_fxAttributeForJoint == JointAttribute.EndPoint)
				{
					m_endPos = m_fxCasterJoint.m_jointObject.transform.position;
				}
				else if (m_fxAttributeForJoint == JointAttribute.TopPoint)
				{
					Sequence.SetAttribute(m_fx, "topPoint", m_fxCasterJoint.m_jointObject.transform.position);
				}
			}
			if (m_syncHeightOfEndToStart)
			{
				m_endPos.y = m_startPos.y;
			}
			Sequence.SetAttribute(m_fx, "endPoint", m_endPos);
			Sequence.SetAttribute(m_fx, "startPoint", m_startPos);
			return;
		}
	}

	private void SpawnFX()
	{
		if (m_fxPrefab != null)
		{
			Vector3 startPos = m_startPos;
			m_fx = InstantiateFX(m_fxPrefab, startPos, default(Quaternion));
		}
		if (!m_fxCasterJoint.IsInitialized())
		{
			GameObject referenceModel = GetReferenceModel(base.Caster, m_fxCasterJointReferenceType);
			if (referenceModel != null)
			{
				m_fxCasterJoint.Initialize(referenceModel);
			}
		}
		for (int i = 0; i < base.Targets.Length; i++)
		{
			if (base.Targets[i] != null)
			{
				Vector3 targetHitPosition = GetTargetHitPosition(i);
				Vector3 hitDirection = targetHitPosition - base.Caster.transform.position;
				hitDirection.y = 0f;
				hitDirection.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
				base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo);
			}
		}
		while (true)
		{
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

	private void OnDisable()
	{
		if (!(m_fx != null))
		{
			return;
		}
		while (true)
		{
			Object.Destroy(m_fx.gameObject);
			m_fx = null;
			return;
		}
	}
}
