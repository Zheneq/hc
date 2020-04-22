using System.Collections.Generic;
using UnityEngine;

public class StrafingLaserSequence : Sequence
{
	private class ShotInfo
	{
		public GameObject m_fx;

		public GameObject m_hitFx;

		public ActorData m_target;

		public JointPopupProperty m_joint;

		public float m_despawnTime = -1f;

		public float m_curTurnSpeed;
	}

	[Tooltip("Main FX prefab.")]
	public GameObject m_fxPrefab;

	public GameObject m_hitFxPrefab;

	[JointPopup("FX attach joint on the caster")]
	public JointPopupProperty m_fxCasterJoint;

	public ReferenceModelType m_fxCasterJointReferenceType;

	[JointPopup("FX attach joint on the target")]
	public JointPopupProperty m_fxTargetJoint;

	[AnimEventPicker]
	public Object m_startActionEvent;

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

	private Dictionary<ActorData, ShotInfo> m_targetToShotInfoMap = new Dictionary<ActorData, ShotInfo>();

	private Vector3 m_modelToTargetDir = Vector3.zero;

	private void OnDisable()
	{
		foreach (KeyValuePair<ActorData, ShotInfo> item in m_targetToShotInfoMap)
		{
			Object.Destroy(item.Value.m_fx);
			Object.Destroy(item.Value.m_hitFx);
		}
	}

	public override void FinishSetup()
	{
		GameObject referenceModel = GetReferenceModel(base.Caster, m_fxCasterJointReferenceType);
		if (referenceModel != null)
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
			m_modelToTargetDir = base.TargetPos - referenceModel.transform.position;
			m_modelToTargetDir.Normalize();
			m_fxCasterJoint.Initialize(referenceModel);
		}
		if (m_startActionEvent == null)
		{
			m_satelliteActionStarted = true;
		}
	}

	private void FireAtTarget(ActorData curTarget)
	{
		Vector3 modelToTargetDir = m_modelToTargetDir;
		modelToTargetDir.y = 0f;
		GameObject fx = InstantiateFX(m_fxPrefab, m_fxCasterJoint.m_jointObject.transform.position, Quaternion.LookRotation(modelToTargetDir));
		ShotInfo shotInfo = new ShotInfo();
		shotInfo.m_fx = fx;
		shotInfo.m_curTurnSpeed = m_initialProjectileTurnSpeed;
		JointPopupProperty jointPopupProperty = new JointPopupProperty();
		jointPopupProperty.m_joint = m_fxTargetJoint.m_joint;
		jointPopupProperty.m_jointCharacter = m_fxTargetJoint.m_jointCharacter;
		jointPopupProperty.Initialize(curTarget.gameObject);
		shotInfo.m_joint = jointPopupProperty;
		shotInfo.m_target = curTarget;
		m_targetToShotInfoMap[curTarget] = shotInfo;
		if (string.IsNullOrEmpty(m_shotStartAudioEvent))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			GameObject referenceModel = GetReferenceModel(base.Caster, m_fxCasterJointReferenceType);
			string shotStartAudioEvent = m_shotStartAudioEvent;
			object parentGameObject;
			if ((bool)referenceModel)
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
				parentGameObject = referenceModel;
			}
			else
			{
				parentGameObject = null;
			}
			AudioManager.PostEvent(shotStartAudioEvent, (GameObject)parentGameObject);
			return;
		}
	}

	private void SpawnHitFx(ShotInfo shotInfo)
	{
		if (m_hitFxPrefab != null)
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
			GameObject gameObject = shotInfo.m_hitFx = InstantiateFX(m_hitFxPrefab, shotInfo.m_joint.m_jointObject.transform.position, Quaternion.identity);
		}
		if (!string.IsNullOrEmpty(m_shotImpactAudioEvent))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			string shotImpactAudioEvent = m_shotImpactAudioEvent;
			object parentGameObject;
			if ((bool)shotInfo.m_target)
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
				parentGameObject = shotInfo.m_target.gameObject;
			}
			else
			{
				parentGameObject = null;
			}
			AudioManager.PostEvent(shotImpactAudioEvent, (GameObject)parentGameObject);
		}
		shotInfo.m_despawnTime = GameTime.time + m_despawnDelay;
		Vector3 position = shotInfo.m_joint.m_jointObject.transform.position;
		Vector3 forward = shotInfo.m_fx.transform.forward;
		forward.Normalize();
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, forward);
		base.Source.OnSequenceHit(this, shotInfo.m_target, impulseInfo);
	}

	private void UpdateShot(ShotInfo shotInfo)
	{
		if (!(shotInfo.m_fx != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Vector3 position = shotInfo.m_fx.transform.position;
			Vector3 forward = shotInfo.m_fx.transform.forward;
			shotInfo.m_curTurnSpeed += m_projectileTurnAcceleration * GameTime.deltaTime;
			Vector3 vector = shotInfo.m_joint.m_jointObject.transform.position - shotInfo.m_fx.transform.position;
			float magnitude = vector.magnitude;
			Quaternion rotation = Quaternion.RotateTowards(Quaternion.LookRotation(forward), Quaternion.LookRotation(vector), shotInfo.m_curTurnSpeed * GameTime.deltaTime);
			Vector3 vector2 = rotation * Vector3.forward;
			Vector3 vector3 = position + vector2 * m_projectileSpeed * GameTime.deltaTime;
			Vector3 lhs = shotInfo.m_joint.m_jointObject.transform.position - vector3;
			int num;
			if (!(magnitude <= 0.5f))
			{
				if (Vector3.Dot(lhs, vector) <= 0f)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					num = ((magnitude < 2f) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
			}
			else
			{
				num = 1;
			}
			if (num != 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						SpawnHitFx(shotInfo);
						Object.Destroy(shotInfo.m_fx);
						return;
					}
				}
			}
			shotInfo.m_fx.transform.position = vector3;
			shotInfo.m_fx.transform.forward = vector2;
			return;
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (parameter == m_startActionEvent)
		{
			m_satelliteActionStarted = true;
		}
	}

	private void Update()
	{
		if (!m_satelliteActionStarted)
		{
			return;
		}
		GameObject referenceModel = GetReferenceModel(base.Caster, m_fxCasterJointReferenceType);
		if (base.Targets != null)
		{
			while (true)
			{
				switch (2)
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
			if (referenceModel != null)
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
				for (int i = 0; i < base.Targets.Length; i++)
				{
					if (m_targetToShotInfoMap.ContainsKey(base.Targets[i]))
					{
						continue;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					Vector3 lhs = base.Targets[i].transform.position - referenceModel.transform.position;
					int num;
					if (!(Vector3.Dot(lhs, m_modelToTargetDir) < 0f))
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						num = ((lhs.magnitude < m_shotRange) ? 1 : 0);
					}
					else
					{
						num = 1;
					}
					if (num == 0)
					{
						goto IL_0116;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!(m_fireShotStartDelay <= 0f))
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!(m_timeSinceActionStarted >= m_fireShotStartDelay))
						{
							goto IL_0116;
						}
					}
					goto IL_013b;
					IL_013b:
					FireAtTarget(base.Targets[i]);
					continue;
					IL_0116:
					if (!(m_maxWaitBeforeFireShots > 0f) || !(m_timeSinceActionStarted >= m_maxWaitBeforeFireShots))
					{
						continue;
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					goto IL_013b;
				}
				using (Dictionary<ActorData, ShotInfo>.Enumerator enumerator = m_targetToShotInfoMap.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UpdateShot(enumerator.Current.Value);
					}
					while (true)
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
		m_timeSinceActionStarted += GameTime.deltaTime;
	}
}
