using System;
using UnityEngine;

public class TripleTwistProjectileSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	[Tooltip("FX at point(s) of impact")]
	public GameObject m_fxImpactPrefab;

	[JointPopup("FX attach joint (or start position for projectiles).")]
	public JointPopupProperty m_fxJoint;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_startEvent;

	[Tooltip("Starting projectile speed.")]
	public float m_projectileSpeed;

	private float m_impactDuration;

	private float m_impactDurationLeft;

	public int m_numProjectiles = 3;

	public float m_projectileOffset = 0.5f;

	private float m_projectileTravelTime;

	private GameObject[] m_projectileFXs;

	private GameObject m_fxImpact;

	private Vector3 m_startPos;

	private Vector3 m_projectileDir;

	[AudioEvent(false)]
	public string m_audioEvent;

	[AudioEvent(false)]
	public string m_impactAudioEvent;

	public float m_rotationSpeed = 180f;

	public override void FinishSetup()
	{
		if (this.m_startEvent == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TripleTwistProjectileSequence.FinishSetup()).MethodHandle;
			}
			this.SpawnFX();
		}
		this.m_impactDuration = Sequence.GetFXDuration(this.m_fxImpactPrefab);
	}

	private void SpawnFX()
	{
		this.m_fxJoint.Initialize(base.Caster.gameObject);
		this.m_startPos = this.m_fxJoint.m_jointObject.transform.position;
		this.m_projectileDir = (base.TargetPos - this.m_startPos).normalized;
		Quaternion rotation = Quaternion.LookRotation(this.m_projectileDir);
		this.m_projectileFXs = new GameObject[this.m_numProjectiles];
		for (int i = 0; i < this.m_numProjectiles; i++)
		{
			float angle = (float)i * 360f / (float)this.m_numProjectiles;
			Quaternion rotation2 = Quaternion.AngleAxis(angle, this.m_projectileDir);
			Vector3 b = (rotation2 * Vector3.up).normalized * this.m_projectileOffset;
			this.m_projectileFXs[i] = base.InstantiateFX(this.m_fxPrefab, this.m_startPos + b, rotation, true, true);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(TripleTwistProjectileSequence.SpawnFX()).MethodHandle;
		}
		if (!string.IsNullOrEmpty(this.m_audioEvent))
		{
			AudioManager.PostEvent(this.m_audioEvent, base.Caster.gameObject);
		}
	}

	private void SpawnImpactFX()
	{
		if (this.m_fxImpactPrefab)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TripleTwistProjectileSequence.SpawnImpactFX()).MethodHandle;
			}
			this.m_fxImpact = base.InstantiateFX(this.m_fxImpactPrefab, base.TargetPos, Quaternion.identity, true, true);
			this.m_impactDurationLeft = this.m_impactDuration;
		}
		if (!string.IsNullOrEmpty(this.m_impactAudioEvent))
		{
			AudioManager.PostEvent(this.m_impactAudioEvent, this.m_fxImpact.gameObject);
		}
		base.CallHitSequenceOnTargets(base.TargetPos, 1f, null, true);
	}

	private void DeactivateProjectiles()
	{
		if (this.m_projectileFXs != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TripleTwistProjectileSequence.DeactivateProjectiles()).MethodHandle;
			}
			for (int i = 0; i < this.m_projectileFXs.Length; i++)
			{
				this.m_projectileFXs[i].SetActive(false);
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
		}
	}

	private bool ProjectilesActive()
	{
		bool result = false;
		if (this.m_projectileFXs != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TripleTwistProjectileSequence.ProjectilesActive()).MethodHandle;
			}
			for (int i = 0; i < this.m_projectileFXs.Length; i++)
			{
				if (this.m_projectileFXs[i].activeSelf)
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
					return true;
				}
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
		}
		return result;
	}

	private void Update()
	{
		if (this.m_initialized && this.m_projectileFXs != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TripleTwistProjectileSequence.Update()).MethodHandle;
			}
			if (this.ProjectilesActive())
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
				this.m_projectileTravelTime += GameTime.deltaTime;
				Vector3 a = this.m_startPos + this.m_projectileDir * this.m_projectileTravelTime * this.m_projectileSpeed;
				if (Vector3.Dot(a - this.m_startPos, a - base.TargetPos) <= 0f)
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
					for (int i = 0; i < this.m_numProjectiles; i++)
					{
						float angle = (float)i * 360f / (float)this.m_numProjectiles + this.m_projectileTravelTime * this.m_rotationSpeed;
						Quaternion rotation = Quaternion.AngleAxis(angle, this.m_projectileDir);
						Vector3 b = (rotation * Vector3.up).normalized * this.m_projectileOffset;
						this.m_projectileFXs[i].transform.position = a + b;
					}
				}
				else
				{
					this.DeactivateProjectiles();
					if (this.m_fxImpactPrefab != null)
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
						this.SpawnImpactFX();
					}
					else
					{
						base.MarkForRemoval();
					}
				}
			}
			if (this.m_fxImpact != null)
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
				if (this.m_fxImpact.activeSelf)
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
					if (this.m_impactDurationLeft > 0f)
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
						this.m_impactDurationLeft -= GameTime.deltaTime;
					}
					else
					{
						base.MarkForRemoval();
					}
				}
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (parameter == this.m_startEvent)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TripleTwistProjectileSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.SpawnFX();
		}
	}

	private void OnDisable()
	{
		if (this.m_projectileFXs != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TripleTwistProjectileSequence.OnDisable()).MethodHandle;
			}
			for (int i = 0; i < this.m_projectileFXs.Length; i++)
			{
				if (this.m_projectileFXs[i] != null)
				{
					UnityEngine.Object.Destroy(this.m_projectileFXs[i].gameObject);
				}
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_projectileFXs = null;
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
}
