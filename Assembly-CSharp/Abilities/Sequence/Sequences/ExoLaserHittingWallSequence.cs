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
			if (extraSequenceParams is GroundLineSequence.ExtraParams extraParams2)
			{
				float dist = Vector3.Distance(extraParams2.startPos, extraParams2.endPos);
				ExoAnchorLaser exoAnchorLaser = Caster.GetAbilityData().GetAbilityOfType(typeof(ExoAnchorLaser)) as ExoAnchorLaser;
				if (exoAnchorLaser != null)
				{
					float range = exoAnchorLaser.GetLaserInfo().range * Board.Get().squareSize;
					if (dist + 0.5 < range)
					{
						m_hittingWall = true;
						m_hitPosition = extraParams2.endPos;
					}
				}
				return;
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
			m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(Caster.GetTeam());
		}
		if (m_fx != null && m_hittingWall && m_fxCasterJoint.IsInitialized())
		{
			Vector3 position = m_fx.transform.position;
			position.y = m_fxCasterJoint.m_jointObject.transform.position.y;
			m_fx.transform.position = position;
		}
	}

	public override void FinishSetup()
	{
		if (!m_fxCasterJoint.IsInitialized())
		{
			GameObject referenceModel = GetReferenceModel(Caster, m_fxCasterJointReferenceType);
			if (referenceModel != null)
			{
				m_fxCasterJoint.Initialize(referenceModel);
			}
		}
		if (m_hittingWall && m_fxCasterJoint.IsInitialized())
		{
			BoardSquare hitSquare = Board.Get().GetSquareFromVec3(m_hitPosition);
			m_hitPosition.y = m_fxCasterJoint.m_jointObject.transform.position.y;
			if (hitSquare == null)
			{
				return;
			}
			Vector3 vector = m_hitPosition - hitSquare.ToVector3();
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
		if (m_startEvent == null)
		{
			SpawnFX();
		}
	}

	private void SpawnFX()
	{
		if (m_fxPrefab != null && m_hittingWall)
		{
			m_fx = InstantiateFX(m_fxPrefab, m_hitPosition, m_hitRotation);
		}
	}

	private void StopFX()
	{
		if (m_fx != null)
		{
			Destroy(m_fx);
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
		if (m_stopEvent == parameter)
		{
			StopFX();
		}
	}
}
