using System;
using UnityEngine;

public class BattleMonkThrowSwordSequence : SplineProjectileSequence
{
	[Header("-- Prefabs")]
	public GameObject m_swordTempSatellitePrefab;

	public GameObject m_linePrefab;

	[Header("-- Sword / Satellite Info")]
	[AnimEventPicker]
	public UnityEngine.Object m_swordRemoveEvent;

	public float m_splineMaxHeight;

	public float m_satelliteHeightOffset;

	[Header("-- Line Info")]
	public bool m_lineSpawnOnProjectileImpact;

	[AnimEventPicker]
	public UnityEngine.Object m_lineStartEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_lineRemoveEvent;

	public float m_lineDuration = -1f;

	[AudioEvent(false)]
	public string m_lineAudioEvent;

	[JointPopup("Line Start Joint Caster")]
	public JointPopupProperty m_lineFxCasterStartJoint;

	[JointPopup("Line End Joint Sword Satellite")]
	public JointPopupProperty m_lineFxSatelliteJoint;

	private GameObject m_swordTempSatelliteInstance;

	private GameObject m_lineFx;

	private float m_lineDespawnTime;

	private bool m_lineSpawned;

	public override void FinishSetup()
	{
		base.FinishSetup();
		if (this.m_lineStartEvent == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkThrowSwordSequence.FinishSetup()).MethodHandle;
			}
			if (!this.m_lineSpawnOnProjectileImpact)
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
				this.SpawnLineFX();
			}
		}
		this.m_markForRemovalAfterImpact = false;
		this.m_doHitsAsProjectileTravels = false;
	}

	protected override void SpawnImpactFX(Vector3 impactPos, Quaternion impactRot)
	{
		base.SpawnImpactFX(impactPos, impactRot);
		this.m_swordTempSatelliteInstance = base.InstantiateFX(this.m_swordTempSatellitePrefab, base.TargetPos + new Vector3(0f, this.m_satelliteHeightOffset, 0f), Quaternion.identity, true, true);
		if (this.m_swordTempSatelliteInstance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkThrowSwordSequence.SpawnImpactFX(Vector3, Quaternion)).MethodHandle;
			}
			TempSatellite component = this.m_swordTempSatelliteInstance.GetComponent<TempSatellite>();
			component.Setup(this);
			component.SetNotifyOwnerOnAnimEvent(false);
			if (this.m_lineSpawned)
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
				if (!this.m_lineFxSatelliteJoint.IsInitialized())
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
					GameObject swordTempSatelliteInstance = this.m_swordTempSatelliteInstance;
					if (swordTempSatelliteInstance != null)
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
						this.m_lineFxSatelliteJoint.Initialize(swordTempSatelliteInstance);
					}
				}
			}
		}
		if (this.m_lineSpawnOnProjectileImpact)
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
			this.SpawnLineFX();
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		base.OnAnimationEvent(parameter, sourceObject);
		if (this.m_lineStartEvent == parameter)
		{
			this.SpawnLineFX();
		}
		if (this.m_swordRemoveEvent == parameter)
		{
			this.DestroySatellite();
		}
		if (this.m_lineRemoveEvent == parameter)
		{
			this.DestroyLine();
		}
	}

	private void SpawnLineFX()
	{
		if (this.m_lineSpawned)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkThrowSwordSequence.SpawnLineFX()).MethodHandle;
			}
			return;
		}
		if (!this.m_lineFxCasterStartJoint.IsInitialized())
		{
			GameObject referenceModel = base.GetReferenceModel(base.Caster, Sequence.ReferenceModelType.Actor);
			if (referenceModel != null)
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
				this.m_lineFxCasterStartJoint.Initialize(referenceModel);
			}
		}
		if (!this.m_lineFxSatelliteJoint.IsInitialized())
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
			GameObject swordTempSatelliteInstance = this.m_swordTempSatelliteInstance;
			if (swordTempSatelliteInstance != null)
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
				this.m_lineFxSatelliteJoint.Initialize(swordTempSatelliteInstance);
			}
		}
		if (this.m_linePrefab != null)
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
			Vector3 position = this.m_lineFxCasterStartJoint.m_jointObject.transform.position;
			Quaternion rotation = default(Quaternion);
			this.m_lineFx = base.InstantiateFX(this.m_linePrefab, position, rotation, true, true);
		}
		if (!string.IsNullOrEmpty(this.m_lineAudioEvent))
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
			AudioManager.PostEvent(this.m_lineAudioEvent, base.Caster.gameObject);
		}
		if (this.m_lineDuration > 0f)
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
			this.m_lineDespawnTime = GameTime.time + this.m_lineDuration;
		}
		else
		{
			this.m_lineDespawnTime = -1f;
		}
		this.m_lineSpawned = true;
	}

	private void Update()
	{
		this.OnUpdate();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.m_initialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkThrowSwordSequence.OnUpdate()).MethodHandle;
			}
			if (this.m_lineFxCasterStartJoint.m_jointObject != null)
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
				Sequence.SetAttribute(this.m_lineFx, "startPoint", this.m_lineFxCasterStartJoint.m_jointObject.transform.position);
			}
			if (this.m_lineFxSatelliteJoint.m_jointObject != null)
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
				Sequence.SetAttribute(this.m_lineFx, "endPoint", this.m_lineFxSatelliteJoint.m_jointObject.transform.position);
			}
			else if (this.m_lineFxCasterStartJoint.m_jointObject != null)
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
				if (this.m_fx != null)
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
					Sequence.SetAttribute(this.m_lineFx, "endPoint", this.m_fx.transform.position);
				}
			}
			if (this.m_lineDespawnTime < GameTime.time)
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
				if (this.m_lineDespawnTime > 0f)
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
					this.DestroyLine();
				}
			}
		}
	}

	protected override void OnSequenceDisable()
	{
		base.OnSequenceDisable();
		this.DestroyLine();
		this.DestroySatellite();
	}

	private void DestroyLine()
	{
		if (this.m_lineFx != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BattleMonkThrowSwordSequence.DestroyLine()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_lineFx);
			this.m_lineFx = null;
		}
	}

	private void DestroySatellite()
	{
		if (this.m_swordTempSatelliteInstance != null)
		{
			UnityEngine.Object.Destroy(this.m_swordTempSatelliteInstance);
			this.m_swordTempSatelliteInstance = null;
		}
	}

	internal override Vector3[] GetSplinePath(int curIndex, int maxIndex)
	{
		Vector3 vector = this.m_fxJoint.m_jointObject.transform.position;
		if (this.m_useOverrideStartPos)
		{
			vector = this.m_overrideStartPos;
		}
		Vector3[] array = new Vector3[5];
		Vector3 targetPos = base.TargetPos;
		if (this.m_splineMaxHeight == 0f)
		{
			Vector3 b = targetPos - vector;
			array[0] = vector - b;
			array[1] = vector;
			array[2] = (vector + targetPos) * 0.5f;
			array[3] = targetPos;
			array[4] = targetPos + b;
		}
		else
		{
			array[0] = vector + Vector3.down * this.m_splineMaxHeight;
			array[1] = vector;
			array[2] = (vector + targetPos) * 0.5f + Vector3.up * this.m_splineMaxHeight;
			array[3] = targetPos;
			array[4] = targetPos + Vector3.down * this.m_splineMaxHeight;
		}
		return array;
	}
}
