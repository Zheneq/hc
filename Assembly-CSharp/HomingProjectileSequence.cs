using System;
using UnityEngine;

public class HomingProjectileSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	public GameObject m_hitFxPrefab;

	[JointPopup("FX attach joint on the caster")]
	public JointPopupProperty m_fxCasterJoint;

	public Sequence.ReferenceModelType m_fxCasterJointReferenceType;

	[JointPopup("FX attach joint on the target")]
	public JointPopupProperty m_fxTargetJoint;

	[AudioEvent(false)]
	[Tooltip("Audio event to play when the projectile initially fires")]
	public string m_audioEvent;

	[AudioEvent(false)]
	[Tooltip("Audio event to play when the projectile hits a target")]
	public string m_impactAudioEvent;

	public float m_shotRange = 4f;

	public float m_projectileSpeed = 10f;

	public float m_initialProjectileTurnSpeed = 180f;

	public float m_projectileTurnAcceleration = 600f;

	private GameObject m_fx;

	private GameObject m_hitFx;

	private JointPopupProperty m_joint;

	private float m_curTurnSpeed;

	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public UnityEngine.Object m_startEvent;

	private void OnDisable()
	{
		UnityEngine.Object.Destroy(this.m_fx);
		UnityEngine.Object.Destroy(this.m_hitFx);
	}

	public override void FinishSetup()
	{
		GameObject referenceModel = base.GetReferenceModel(base.Caster, this.m_fxCasterJointReferenceType);
		if (referenceModel != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(HomingProjectileSequence.FinishSetup()).MethodHandle;
			}
			this.m_fxCasterJoint.Initialize(referenceModel);
		}
		if (this.m_startEvent == null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			this.FireAtTarget();
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			this.FireAtTarget();
		}
	}

	private void FireAtTarget()
	{
		this.m_fx = base.InstantiateFX(this.m_fxPrefab, this.m_fxCasterJoint.m_jointObject.transform.position, Quaternion.LookRotation(base.Caster.transform.forward), true, true);
		this.m_curTurnSpeed = this.m_initialProjectileTurnSpeed;
		if (!string.IsNullOrEmpty(this.m_audioEvent))
		{
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
		JointPopupProperty jointPopupProperty = new JointPopupProperty();
		jointPopupProperty.m_joint = this.m_fxTargetJoint.m_joint;
		jointPopupProperty.m_jointCharacter = this.m_fxTargetJoint.m_jointCharacter;
		jointPopupProperty.Initialize(base.Target.gameObject);
		this.m_joint = jointPopupProperty;
	}

	private void SpawnHitFx()
	{
		if (this.m_hitFxPrefab != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(HomingProjectileSequence.SpawnHitFx()).MethodHandle;
			}
			GameObject hitFx = base.InstantiateFX(this.m_hitFxPrefab, this.m_joint.m_jointObject.transform.position, Quaternion.identity, true, true);
			this.m_hitFx = hitFx;
			if (!string.IsNullOrEmpty(this.m_impactAudioEvent))
			{
				AudioManager.PostEvent(this.m_impactAudioEvent, this.m_fx.gameObject);
			}
		}
		Vector3 position = this.m_joint.m_jointObject.transform.position;
		Vector3 forward = this.m_fx.transform.forward;
		forward.Normalize();
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, forward);
		base.Source.OnSequenceHit(this, base.Target, impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
	}

	private void UpdateShot()
	{
		if (this.m_fx != null && this.m_fx.transform != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(HomingProjectileSequence.UpdateShot()).MethodHandle;
			}
			if (this.m_joint != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_joint.m_jointObject != null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_joint.m_jointObject.transform != null)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						Vector3 position = this.m_fx.transform.position;
						Vector3 forward = this.m_fx.transform.forward;
						this.m_curTurnSpeed += this.m_projectileTurnAcceleration * GameTime.deltaTime;
						Vector3 vector = this.m_joint.m_jointObject.transform.position - this.m_fx.transform.position;
						float magnitude = vector.magnitude;
						Quaternion rotation = Quaternion.RotateTowards(Quaternion.LookRotation(forward), Quaternion.LookRotation(vector), this.m_curTurnSpeed * GameTime.deltaTime);
						Vector3 vector2 = rotation * Vector3.forward;
						Vector3 vector3 = position + vector2 * this.m_projectileSpeed * GameTime.deltaTime;
						Vector3 lhs = this.m_joint.m_jointObject.transform.position - vector3;
						bool flag;
						if (magnitude > 0.5f)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							if (Vector3.Dot(lhs, vector) <= 0f)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								flag = (magnitude < 2f);
							}
							else
							{
								flag = false;
							}
						}
						else
						{
							flag = true;
						}
						bool flag2 = flag;
						if (flag2)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							this.SpawnHitFx();
							UnityEngine.Object.Destroy(this.m_fx);
						}
						else
						{
							this.m_fx.transform.position = vector3;
							this.m_fx.transform.forward = vector2;
						}
					}
				}
			}
		}
	}

	private void Update()
	{
		this.UpdateShot();
	}
}
