using UnityEngine;

public class PointToActorLineSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[JointPopup("FX attach joint on the caster")]
	public JointPopupProperty m_fxJoint;

	public ReferenceModelType m_fxJointReferenceType;

	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public Object m_startEvent;

	[AudioEvent(false)]
	public string m_audioEvent;

	private GameObject m_fx;

	public override void FinishSetup()
	{
		if (!(m_startEvent == null))
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

	private void Update()
	{
		if (m_initialized)
		{
			Sequence.SetAttribute(m_fx, "startPoint", base.TargetPos);
			if (m_fxJoint.m_jointObject != null)
			{
				Sequence.SetAttribute(m_fx, "endPoint", m_fxJoint.m_jointObject.transform.position);
			}
		}
	}

	private void SpawnFX()
	{
		if (!m_fxJoint.IsInitialized())
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
			GameObject referenceModel = GetReferenceModel(base.Target, m_fxJointReferenceType);
			if (referenceModel != null)
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
				m_fxJoint.Initialize(referenceModel);
			}
		}
		if (m_fxPrefab != null)
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
			Vector3 targetPos = base.TargetPos;
			m_fx = InstantiateFX(m_fxPrefab, targetPos, default(Quaternion));
		}
		for (int i = 0; i < base.Targets.Length; i++)
		{
			if (base.Targets[i] != null)
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
				Vector3 targetHitPosition = GetTargetHitPosition(i);
				Vector3 hitDirection = targetHitPosition - base.Caster.transform.position;
				hitDirection.y = 0f;
				hitDirection.Normalize();
				ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, hitDirection);
				base.Source.OnSequenceHit(this, base.Targets[i], impulseInfo);
			}
		}
		if (string.IsNullOrEmpty(m_audioEvent))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			AudioManager.PostEvent(m_audioEvent, base.Caster.gameObject);
			return;
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (m_startEvent == parameter)
		{
			SpawnFX();
		}
	}

	private void OnDisable()
	{
		if (m_fx != null)
		{
			Object.Destroy(m_fx.gameObject);
			m_fx = null;
		}
	}
}
