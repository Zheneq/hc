using UnityEngine;

public class ExoLaserHittingWallSequence : Sequence
{
	[Header("Vfx Prefab")]
	public GameObject m_fxPrefab;

	private GameObject m_fx;

	[JointPopup("FX attach joint on the caster - only used for its Y")]
	public JointPopupProperty m_fxCasterJoint;

	public ReferenceModelType m_fxCasterJointReferenceType;

	[Header("Timing Anim Events")]
	[AnimEventPicker]
	public Object m_startEvent;

	[AnimEventPicker]
	public Object m_stopEvent;

	private bool m_hittingWall;

	private Vector3 m_hitPosition;

	private Quaternion m_hitRotation;

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		m_hittingWall = false;
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			GroundLineSequence.ExtraParams extraParams2 = extraSequenceParams as GroundLineSequence.ExtraParams;
			if (extraParams2 == null)
			{
				continue;
			}
			float num = Vector3.Distance(extraParams2.startPos, extraParams2.endPos);
			ExoAnchorLaser exoAnchorLaser = base.Caster.GetAbilityData().GetAbilityOfType(typeof(ExoAnchorLaser)) as ExoAnchorLaser;
			if (!(exoAnchorLaser != null))
			{
				return;
			}
			while (true)
			{
				float num2 = exoAnchorLaser.GetLaserInfo().range * Board.Get().squareSize;
				if ((double)num + 0.5 < (double)num2)
				{
					m_hittingWall = true;
					m_hitPosition = extraParams2.endPos;
				}
				return;
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

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		ProcessSequenceVisibility();
		if (m_fx != null && m_fx.GetComponent<FriendlyEnemyVFXSelector>() != null)
		{
			m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.GetTeam());
		}
		if (!(m_fx != null))
		{
			return;
		}
		while (true)
		{
			if (!m_hittingWall)
			{
				return;
			}
			while (true)
			{
				if (m_fxCasterJoint.IsInitialized())
				{
					while (true)
					{
						Vector3 position = m_fx.transform.position;
						Vector3 position2 = m_fxCasterJoint.m_jointObject.transform.position;
						position.y = position2.y;
						m_fx.transform.position = position;
						return;
					}
				}
				return;
			}
		}
	}

	public override void FinishSetup()
	{
		if (!m_fxCasterJoint.IsInitialized())
		{
			GameObject referenceModel = GetReferenceModel(base.Caster, m_fxCasterJointReferenceType);
			if (referenceModel != null)
			{
				m_fxCasterJoint.Initialize(referenceModel);
			}
		}
		if (m_hittingWall)
		{
			if (m_fxCasterJoint.IsInitialized())
			{
				BoardSquare boardSquare = Board.Get().GetSquareFromVec3(m_hitPosition);
				ref Vector3 hitPosition = ref m_hitPosition;
				Vector3 position = m_fxCasterJoint.m_jointObject.transform.position;
				hitPosition.y = position.y;
				if (boardSquare == null)
				{
					return;
				}
				Vector3 vector = m_hitPosition - boardSquare.ToVector3();
				float num = Mathf.Abs(vector.x);
				float num2 = Mathf.Abs(vector.z);
				if (num > num2)
				{
					if (vector.x > 0f)
					{
						m_hitRotation = Quaternion.LookRotation(new Vector3(1f, 0f, 0f));
					}
					else
					{
						m_hitRotation = Quaternion.LookRotation(new Vector3(-1f, 0f, 0f));
					}
				}
				else if (vector.z > 0f)
				{
					m_hitRotation = Quaternion.LookRotation(new Vector3(0f, 0f, 1f));
				}
				else
				{
					m_hitRotation = Quaternion.LookRotation(new Vector3(0f, 0f, -1f));
				}
			}
		}
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

	private void SpawnFX()
	{
		if (!m_fxPrefab)
		{
			return;
		}
		while (true)
		{
			if (m_hittingWall)
			{
				while (true)
				{
					m_fx = InstantiateFX(m_fxPrefab, m_hitPosition, m_hitRotation);
					return;
				}
			}
			return;
		}
	}

	private void StopFX()
	{
		if (m_fx != null)
		{
			Object.Destroy(m_fx);
			m_fx = null;
		}
	}

	private void OnDisable()
	{
		StopFX();
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (m_startEvent == parameter)
		{
			SpawnFX();
		}
		if (!(m_stopEvent == parameter))
		{
			return;
		}
		while (true)
		{
			StopFX();
			return;
		}
	}
}
