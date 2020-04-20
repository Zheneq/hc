using System;
using UnityEngine;

public class ExoLaserHittingWallSequence : Sequence
{
	[Header("Vfx Prefab")]
	public GameObject m_fxPrefab;

	private GameObject m_fx;

	[JointPopup("FX attach joint on the caster - only used for its Y")]
	public JointPopupProperty m_fxCasterJoint;

	public Sequence.ReferenceModelType m_fxCasterJointReferenceType;

	[Header("Timing Anim Events")]
	[AnimEventPicker]
	public UnityEngine.Object m_startEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_stopEvent;

	private bool m_hittingWall;

	private Vector3 m_hitPosition;

	private Quaternion m_hitRotation;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		this.m_hittingWall = false;
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			GroundLineSequence.ExtraParams extraParams2 = extraSequenceParams as GroundLineSequence.ExtraParams;
			if (extraParams2 != null)
			{
				float num = Vector3.Distance(extraParams2.startPos, extraParams2.endPos);
				ExoAnchorLaser exoAnchorLaser = base.Caster.GetAbilityData().GetAbilityOfType(typeof(ExoAnchorLaser)) as ExoAnchorLaser;
				if (exoAnchorLaser != null)
				{
					float num2 = exoAnchorLaser.GetLaserInfo().range * Board.Get().squareSize;
					if ((double)num + 0.5 < (double)num2)
					{
						this.m_hittingWall = true;
						this.m_hitPosition = extraParams2.endPos;
					}
				}
				return;
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	private void Update()
	{
		if (this.m_initialized)
		{
			base.ProcessSequenceVisibility();
			if (this.m_fx != null && this.m_fx.GetComponent<FriendlyEnemyVFXSelector>() != null)
			{
				this.m_fx.GetComponent<FriendlyEnemyVFXSelector>().Setup(base.Caster.GetTeam());
			}
			if (this.m_fx != null)
			{
				if (this.m_hittingWall)
				{
					if (this.m_fxCasterJoint.IsInitialized())
					{
						Vector3 position = this.m_fx.transform.position;
						position.y = this.m_fxCasterJoint.m_jointObject.transform.position.y;
						this.m_fx.transform.position = position;
					}
				}
			}
		}
	}

	public override void FinishSetup()
	{
		if (!this.m_fxCasterJoint.IsInitialized())
		{
			GameObject referenceModel = base.GetReferenceModel(base.Caster, this.m_fxCasterJointReferenceType);
			if (referenceModel != null)
			{
				this.m_fxCasterJoint.Initialize(referenceModel);
			}
		}
		if (this.m_hittingWall)
		{
			if (this.m_fxCasterJoint.IsInitialized())
			{
				BoardSquare boardSquare = Board.Get().GetBoardSquare(this.m_hitPosition);
				this.m_hitPosition.y = this.m_fxCasterJoint.m_jointObject.transform.position.y;
				if (boardSquare == null)
				{
					return;
				}
				Vector3 vector = this.m_hitPosition - boardSquare.ToVector3();
				float num = Mathf.Abs(vector.x);
				float num2 = Mathf.Abs(vector.z);
				if (num > num2)
				{
					if (vector.x > 0f)
					{
						this.m_hitRotation = Quaternion.LookRotation(new Vector3(1f, 0f, 0f));
					}
					else
					{
						this.m_hitRotation = Quaternion.LookRotation(new Vector3(-1f, 0f, 0f));
					}
				}
				else if (vector.z > 0f)
				{
					this.m_hitRotation = Quaternion.LookRotation(new Vector3(0f, 0f, 1f));
				}
				else
				{
					this.m_hitRotation = Quaternion.LookRotation(new Vector3(0f, 0f, -1f));
				}
			}
		}
		if (this.m_startEvent == null)
		{
			this.SpawnFX();
		}
	}

	private void SpawnFX()
	{
		if (this.m_fxPrefab)
		{
			if (this.m_hittingWall)
			{
				this.m_fx = base.InstantiateFX(this.m_fxPrefab, this.m_hitPosition, this.m_hitRotation, true, true);
			}
		}
	}

	private void StopFX()
	{
		if (this.m_fx != null)
		{
			UnityEngine.Object.Destroy(this.m_fx);
			this.m_fx = null;
		}
	}

	private void OnDisable()
	{
		this.StopFX();
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			this.SpawnFX();
		}
		if (this.m_stopEvent == parameter)
		{
			this.StopFX();
		}
	}
}
