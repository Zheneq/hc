using System;
using UnityEngine;

public class SimpleVerticalProjectileSequence : Sequence
{
	[Separator("Fx Prefabs", true)]
	public GameObject m_fxPrefab;

	[Tooltip("FX at point of impact")]
	public GameObject m_fxImpactPrefab;

	[JointPopup("Start position for projectiles that are traveling up")]
	public JointPopupProperty m_fxJoint;

	[Separator("Travel Direction. If [Traveling Up] is set, Height Offset is how high projectile up go upward", true)]
	public bool m_travelingUp;

	[Header("-- Whether to align projectile to joint's direction on moment of spawn")]
	public bool m_useJointDirectionForProjectile;

	public float m_heightOffset;

	public float m_projectileSpeed;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_startEvent;

	[Tooltip("Spawn FX if Caster is Dead on Start")]
	public bool m_spawnFxOnStartIfCasterIsDead;

	[Separator("Audio", true)]
	[AudioEvent(false)]
	public string m_audioEvent;

	[AudioEvent(false)]
	public string m_impactAudioEvent;

	private GameObject m_fx;

	private GameObject m_fxImpact;

	private Vector3 m_startPos;

	private float m_impactDurationLeft;

	private float m_impactDuration;

	private Vector3 m_endPos = Vector3.zero;

	private Vector3 m_travelDir = Vector3.up;

	public override void FinishSetup()
	{
		this.m_impactDurationLeft = 0f;
		this.m_fxJoint.Initialize(base.Caster.gameObject);
		this.m_impactDuration = Sequence.GetFXDuration(this.m_fxImpactPrefab);
		if (!(this.m_startEvent == null))
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleVerticalProjectileSequence.FinishSetup()).MethodHandle;
			}
			if (!this.m_spawnFxOnStartIfCasterIsDead)
			{
				return;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!(base.Caster != null))
			{
				return;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!base.Caster.IsDead())
			{
				return;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.SpawnFX();
	}

	private void Update()
	{
		base.ProcessSequenceVisibility();
		if (this.m_initialized)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleVerticalProjectileSequence.Update()).MethodHandle;
			}
			if (this.m_fx != null)
			{
				if (this.m_fx.activeSelf)
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
					Vector3 vector = this.m_fx.transform.position + this.m_travelDir * this.m_projectileSpeed * GameTime.deltaTime;
					Vector3 lhs = this.m_endPos - vector;
					Vector3 rhs = this.m_startPos - vector;
					if (Vector3.Dot(lhs, rhs) > 0f)
					{
						this.m_fx.SetActive(false);
						if (this.m_fxImpactPrefab != null)
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
							this.SpawnImpactFX(this.m_endPos);
						}
						else
						{
							base.Source.OnSequenceHit(this, base.TargetPos, null);
							if (base.Targets != null)
							{
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								foreach (ActorData target in base.Targets)
								{
									base.Source.OnSequenceHit(this, target, Sequence.CreateImpulseInfoWithObjectPose(this.m_fx), ActorModelData.RagdollActivation.HealthBased, true);
								}
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							base.MarkForRemoval();
						}
					}
					else
					{
						this.m_fx.transform.position = vector;
					}
				}
				if (this.m_fxImpact != null && this.m_fxImpact.activeSelf)
				{
					if (this.m_impactDurationLeft > 0f)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_impactDurationLeft -= GameTime.deltaTime;
					}
					if (this.m_impactDurationLeft <= 0f)
					{
						base.MarkForRemoval();
					}
				}
			}
		}
	}

	private void OnDisable()
	{
		if (this.m_fx != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleVerticalProjectileSequence.OnDisable()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_fx);
			this.m_fx = null;
		}
		if (this.m_fxImpact != null)
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
			UnityEngine.Object.Destroy(this.m_fxImpact);
			this.m_fxImpact = null;
		}
		this.m_initialized = false;
	}

	private void SpawnImpactFX(Vector3 impactPos)
	{
		if (this.m_fxImpactPrefab)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleVerticalProjectileSequence.SpawnImpactFX(Vector3)).MethodHandle;
			}
			this.m_fxImpact = base.InstantiateFX(this.m_fxImpactPrefab, impactPos, Quaternion.identity, true, true);
			this.m_impactDurationLeft = this.m_impactDuration;
		}
		if (!string.IsNullOrEmpty(this.m_impactAudioEvent))
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
			AudioManager.PostEvent(this.m_impactAudioEvent, this.m_fx.gameObject);
		}
		base.CallHitSequenceOnTargets(impactPos, 1f, null, true);
		CameraManager.Get().PlayCameraShake(CameraManager.CameraShakeIntensity.Large);
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			this.SpawnFX();
		}
	}

	private void SpawnFX()
	{
		if (this.m_fx == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SimpleVerticalProjectileSequence.SpawnFX()).MethodHandle;
			}
			if (this.m_fxPrefab != null)
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
				Quaternion rotation = default(Quaternion);
				if (this.m_travelingUp)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_startPos = this.m_fxJoint.m_jointObject.transform.position;
					this.m_travelDir = Vector3.up;
					if (this.m_useJointDirectionForProjectile)
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
						if (!base.Caster.IsDead())
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_travelDir = this.m_fxJoint.m_jointObject.transform.forward;
						}
					}
					rotation.SetLookRotation(this.m_travelDir);
					this.m_endPos = this.m_travelDir * this.m_heightOffset;
					Debug.DrawRay(this.m_startPos, 10f * this.m_travelDir, Color.green, 10f);
				}
				else
				{
					this.m_startPos = base.TargetPos;
					this.m_startPos.y = this.m_startPos.y + this.m_heightOffset;
					this.m_endPos = base.TargetPos;
					this.m_travelDir = -1f * Vector3.up;
					rotation.SetLookRotation(this.m_travelDir);
				}
				this.m_fx = base.InstantiateFX(this.m_fxPrefab, this.m_startPos, rotation, true, true);
				if (this.m_fx != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					FriendlyEnemyVFXSelector component = this.m_fx.GetComponent<FriendlyEnemyVFXSelector>();
					if (component != null)
					{
						component.Setup(base.Caster.GetTeam());
					}
				}
				if (!string.IsNullOrEmpty(this.m_audioEvent))
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
					AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
				}
			}
		}
	}
}
