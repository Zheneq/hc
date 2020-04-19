using System;
using System.Collections.Generic;
using UnityEngine;

public class StrafingLaserSequence : Sequence
{
	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	public GameObject m_hitFxPrefab;

	[JointPopup("FX attach joint on the caster")]
	public JointPopupProperty m_fxCasterJoint;

	public Sequence.ReferenceModelType m_fxCasterJointReferenceType;

	[JointPopup("FX attach joint on the target")]
	public JointPopupProperty m_fxTargetJoint;

	[AnimEventPicker]
	public UnityEngine.Object m_startActionEvent;

	private bool m_satelliteActionStarted;

	public float m_shotRange = 4f;

	private float m_despawnDelay = 1f;

	public float m_fireShotStartDelay = 0.5f;

	[Tooltip("If positive, and shot haven't been fired at target after this time, fire remaining shots")]
	public float m_maxWaitBeforeFireShots = 3f;

	private float m_timeSinceActionStarted;

	public float m_projectileSpeed = 10f;

	public float m_initialProjectileTurnSpeed = 180f;

	public float m_projectileTurnAcceleration = 600f;

	[Tooltip("For when a projectile is fired")]
	[AudioEvent(false)]
	public string m_shotStartAudioEvent;

	[Tooltip("For when a projectile hits target")]
	[AudioEvent(false)]
	public string m_shotImpactAudioEvent;

	private Dictionary<ActorData, StrafingLaserSequence.ShotInfo> m_targetToShotInfoMap = new Dictionary<ActorData, StrafingLaserSequence.ShotInfo>();

	private Vector3 m_modelToTargetDir = Vector3.zero;

	private void OnDisable()
	{
		foreach (KeyValuePair<ActorData, StrafingLaserSequence.ShotInfo> keyValuePair in this.m_targetToShotInfoMap)
		{
			UnityEngine.Object.Destroy(keyValuePair.Value.m_fx);
			UnityEngine.Object.Destroy(keyValuePair.Value.m_hitFx);
		}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(StrafingLaserSequence.FinishSetup()).MethodHandle;
			}
			this.m_modelToTargetDir = base.TargetPos - referenceModel.transform.position;
			this.m_modelToTargetDir.Normalize();
			this.m_fxCasterJoint.Initialize(referenceModel);
		}
		if (this.m_startActionEvent == null)
		{
			this.m_satelliteActionStarted = true;
		}
	}

	private void FireAtTarget(ActorData curTarget)
	{
		Vector3 modelToTargetDir = this.m_modelToTargetDir;
		modelToTargetDir.y = 0f;
		GameObject fx = base.InstantiateFX(this.m_fxPrefab, this.m_fxCasterJoint.m_jointObject.transform.position, Quaternion.LookRotation(modelToTargetDir), true, true);
		StrafingLaserSequence.ShotInfo shotInfo = new StrafingLaserSequence.ShotInfo();
		shotInfo.m_fx = fx;
		shotInfo.m_curTurnSpeed = this.m_initialProjectileTurnSpeed;
		JointPopupProperty jointPopupProperty = new JointPopupProperty();
		jointPopupProperty.m_joint = this.m_fxTargetJoint.m_joint;
		jointPopupProperty.m_jointCharacter = this.m_fxTargetJoint.m_jointCharacter;
		jointPopupProperty.Initialize(curTarget.gameObject);
		shotInfo.m_joint = jointPopupProperty;
		shotInfo.m_target = curTarget;
		this.m_targetToShotInfoMap[curTarget] = shotInfo;
		if (!string.IsNullOrEmpty(this.m_shotStartAudioEvent))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(StrafingLaserSequence.FireAtTarget(ActorData)).MethodHandle;
			}
			GameObject referenceModel = base.GetReferenceModel(base.Caster, this.m_fxCasterJointReferenceType);
			string shotStartAudioEvent = this.m_shotStartAudioEvent;
			GameObject parentGameObject;
			if (referenceModel)
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
				parentGameObject = referenceModel;
			}
			else
			{
				parentGameObject = null;
			}
			AudioManager.PostEvent(shotStartAudioEvent, parentGameObject);
		}
	}

	private void SpawnHitFx(StrafingLaserSequence.ShotInfo shotInfo)
	{
		if (this.m_hitFxPrefab != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(StrafingLaserSequence.SpawnHitFx(StrafingLaserSequence.ShotInfo)).MethodHandle;
			}
			GameObject hitFx = base.InstantiateFX(this.m_hitFxPrefab, shotInfo.m_joint.m_jointObject.transform.position, Quaternion.identity, true, true);
			shotInfo.m_hitFx = hitFx;
		}
		if (!string.IsNullOrEmpty(this.m_shotImpactAudioEvent))
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
			string shotImpactAudioEvent = this.m_shotImpactAudioEvent;
			GameObject parentGameObject;
			if (shotInfo.m_target)
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
				parentGameObject = shotInfo.m_target.gameObject;
			}
			else
			{
				parentGameObject = null;
			}
			AudioManager.PostEvent(shotImpactAudioEvent, parentGameObject);
		}
		shotInfo.m_despawnTime = GameTime.time + this.m_despawnDelay;
		Vector3 position = shotInfo.m_joint.m_jointObject.transform.position;
		Vector3 forward = shotInfo.m_fx.transform.forward;
		forward.Normalize();
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, forward);
		base.Source.OnSequenceHit(this, shotInfo.m_target, impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
	}

	private void UpdateShot(StrafingLaserSequence.ShotInfo shotInfo)
	{
		if (shotInfo.m_fx != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(StrafingLaserSequence.UpdateShot(StrafingLaserSequence.ShotInfo)).MethodHandle;
			}
			Vector3 position = shotInfo.m_fx.transform.position;
			Vector3 forward = shotInfo.m_fx.transform.forward;
			shotInfo.m_curTurnSpeed += this.m_projectileTurnAcceleration * GameTime.deltaTime;
			Vector3 vector = shotInfo.m_joint.m_jointObject.transform.position - shotInfo.m_fx.transform.position;
			float magnitude = vector.magnitude;
			Quaternion rotation = Quaternion.RotateTowards(Quaternion.LookRotation(forward), Quaternion.LookRotation(vector), shotInfo.m_curTurnSpeed * GameTime.deltaTime);
			Vector3 vector2 = rotation * Vector3.forward;
			Vector3 vector3 = position + vector2 * this.m_projectileSpeed * GameTime.deltaTime;
			Vector3 lhs = shotInfo.m_joint.m_jointObject.transform.position - vector3;
			bool flag;
			if (magnitude > 0.5f)
			{
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
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				this.SpawnHitFx(shotInfo);
				UnityEngine.Object.Destroy(shotInfo.m_fx);
			}
			else
			{
				shotInfo.m_fx.transform.position = vector3;
				shotInfo.m_fx.transform.forward = vector2;
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (parameter == this.m_startActionEvent)
		{
			this.m_satelliteActionStarted = true;
		}
	}

	private void Update()
	{
		if (this.m_satelliteActionStarted)
		{
			GameObject referenceModel = base.GetReferenceModel(base.Caster, this.m_fxCasterJointReferenceType);
			if (base.Targets != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(StrafingLaserSequence.Update()).MethodHandle;
				}
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
					for (int i = 0; i < base.Targets.Length; i++)
					{
						if (!this.m_targetToShotInfoMap.ContainsKey(base.Targets[i]))
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
							Vector3 lhs = base.Targets[i].transform.position - referenceModel.transform.position;
							bool flag;
							if (Vector3.Dot(lhs, this.m_modelToTargetDir) >= 0f)
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
								flag = (lhs.magnitude < this.m_shotRange);
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
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (this.m_fireShotStartDelay <= 0f)
								{
									goto IL_13B;
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
								if (this.m_timeSinceActionStarted >= this.m_fireShotStartDelay)
								{
									goto IL_13B;
								}
							}
							if (this.m_maxWaitBeforeFireShots <= 0f || this.m_timeSinceActionStarted < this.m_maxWaitBeforeFireShots)
							{
								goto IL_14B;
							}
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							IL_13B:
							this.FireAtTarget(base.Targets[i]);
						}
						IL_14B:;
					}
					using (Dictionary<ActorData, StrafingLaserSequence.ShotInfo>.Enumerator enumerator = this.m_targetToShotInfoMap.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<ActorData, StrafingLaserSequence.ShotInfo> keyValuePair = enumerator.Current;
							this.UpdateShot(keyValuePair.Value);
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
			}
			this.m_timeSinceActionStarted += GameTime.deltaTime;
		}
	}

	private class ShotInfo
	{
		public GameObject m_fx;

		public GameObject m_hitFx;

		public ActorData m_target;

		public JointPopupProperty m_joint;

		public float m_despawnTime = -1f;

		public float m_curTurnSpeed;
	}
}
