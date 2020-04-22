using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBurstConeSequence : Sequence
{
	public class BurstProjectilesGroup
	{
		public bool m_startUpdate;

		public List<BurstVfxContainer> m_projectilesList = new List<BurstVfxContainer>();
	}

	[Serializable]
	public class BurstVfxAuthoredData
	{
		[Tooltip("Main FX prefab.")]
		public GameObject m_fxPrefab;

		[JointPopup("FX attach joint (or start position for projectiles).")]
		public JointPopupProperty m_spawnFxJoint;

		[Tooltip("FX at point(s) of impact")]
		public GameObject m_fxImpactPrefab;

		[JointPopup("Impact FX attach joint (or start position for projectiles).")]
		public JointPopupProperty m_impactFxJoint;

		public float m_hitDelayTime;

		[AudioEvent(false)]
		public string m_spawnAudioEvent;

		[AudioEvent(false)]
		public string m_impactAudioEvent;
	}

	public class BurstVfxContainer
	{
		public Sequence m_parentSequence;

		public BurstVfxAuthoredData m_authoredData;

		public Vector3 m_endPos;

		public Vector3 m_posForSequenceHit;

		public float m_timeTillSpawn = -1f;

		private float m_impactSpawnTimestamp = -1f;

		private GameObject m_fxInstance;

		private List<GameObject> m_impactFxInstances = new List<GameObject>();

		private Vector3 m_impactDir;

		private float m_maxDistInWorld = 8f;

		private ActorData m_caster;

		private List<ActorData> m_hitActors;

		public BurstVfxContainer(Sequence parentSequence, BurstVfxAuthoredData authoredData, Vector3 impactDir, float maxDistInWorld, Vector3 endPos, ActorData caster, List<ActorData> hitActors)
		{
			m_parentSequence = parentSequence;
			m_authoredData = authoredData;
			m_impactDir = impactDir;
			m_maxDistInWorld = maxDistInWorld;
			m_caster = caster;
			m_hitActors = hitActors;
			m_endPos = endPos;
			m_posForSequenceHit = endPos;
		}

		public void OnUpdate()
		{
			if (m_timeTillSpawn > 0f)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_timeTillSpawn -= GameTime.deltaTime;
			}
			else if (m_fxInstance == null)
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
				if (m_authoredData.m_fxImpactPrefab != null)
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
					SpawnFX();
				}
			}
			if (!(m_impactSpawnTimestamp > 0f))
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
				if (GameTime.time >= m_impactSpawnTimestamp)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						SpawnImpactFX();
						m_impactSpawnTimestamp = -1f;
						return;
					}
				}
				return;
			}
		}

		private float GetProjectileDistance(Vector3 start, Vector3 forward, float maxDist)
		{
			Vector3 vector = start;
			vector.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
			Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, forward, maxDist, false, m_caster);
			return (vector - laserEndPoint).magnitude;
		}

		private void SpawnFX()
		{
			JointPopupProperty spawnFxJoint = m_authoredData.m_spawnFxJoint;
			if (!spawnFxJoint.IsInitialized())
			{
				spawnFxJoint.Initialize(m_caster.gameObject);
			}
			Vector3 forward = m_endPos - spawnFxJoint.m_jointObject.transform.position;
			forward.y = 0f;
			m_fxInstance = m_parentSequence.InstantiateFX(m_authoredData.m_fxPrefab, spawnFxJoint.m_jointObject.transform.position, Quaternion.LookRotation(forward));
			float projectileDistance = GetProjectileDistance(m_caster.transform.position, m_impactDir, m_maxDistInWorld);
			Sequence.SetAttribute(m_fxInstance, "projectileDistance", projectileDistance);
			if (!string.IsNullOrEmpty(m_authoredData.m_spawnAudioEvent))
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
				AudioManager.PostEvent(m_authoredData.m_spawnAudioEvent, m_caster.gameObject);
			}
			if (m_authoredData.m_hitDelayTime > 0f)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						m_impactSpawnTimestamp = GameTime.time + m_authoredData.m_hitDelayTime;
						return;
					}
				}
			}
			SpawnImpactFX();
		}

		private void SpawnImpactFX()
		{
			if (m_hitActors != null)
			{
				for (int i = 0; i < m_hitActors.Count; i++)
				{
					ActorData actorData = m_hitActors[i];
					if (actorData == null)
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
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						continue;
					}
					GameObject gameObject = m_authoredData.m_impactFxJoint.FindJointObject(actorData.gameObject);
					Vector3 position;
					if (gameObject != null)
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
						position = gameObject.transform.position;
					}
					else
					{
						position = actorData.transform.position;
					}
					Vector3 vector = position;
					ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(vector, m_impactDir);
					if ((bool)m_authoredData.m_fxImpactPrefab)
					{
						Quaternion rotation = Quaternion.LookRotation(m_impactDir);
						m_impactFxInstances.Add(m_parentSequence.InstantiateFX(m_authoredData.m_fxImpactPrefab, vector, rotation));
					}
					if (!string.IsNullOrEmpty(m_authoredData.m_impactAudioEvent))
					{
						AudioManager.PostEvent(m_authoredData.m_impactAudioEvent, actorData.gameObject);
					}
					m_parentSequence.Source.OnSequenceHit(m_parentSequence, actorData, impulseInfo);
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
			}
			m_parentSequence.Source.OnSequenceHit(m_parentSequence, m_posForSequenceHit);
		}

		public void OnSequenceDisable()
		{
			if (m_fxInstance != null)
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
				UnityEngine.Object.Destroy(m_fxInstance.gameObject);
				m_fxInstance = null;
			}
			if (m_impactFxInstances == null)
			{
				return;
			}
			for (int i = 0; i < m_impactFxInstances.Count; i++)
			{
				if (m_impactFxInstances[i] != null)
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
					UnityEngine.Object.Destroy(m_impactFxInstances[i].gameObject);
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				m_impactFxInstances.Clear();
				return;
			}
		}
	}

	[Header("-- Projectile Info --")]
	public BurstVfxAuthoredData m_projectileInfo;

	[Header("-- Anim Events --")]
	[AnimEventPicker]
	public UnityEngine.Object m_beginSideStartEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_middleSideStartEvent;

	[AnimEventPicker]
	public UnityEngine.Object m_endSideStartEvent;

	[Header("-- Other Timing Params --")]
	public float m_timeBetweenProjInBurst = 0.2f;

	public float m_maxWaitTimeBetweenBursts = 3f;

	[Header("-- Projectile Placement --")]
	public float m_coneAngle = 90f;

	public float m_maxProjectileDistInWorld = 8f;

	public int m_projectilePerBurst = 3;

	[Header("-- Portion of cone to cover with this burst --")]
	public float m_burstConePortion = 0.85f;

	private List<BurstProjectilesGroup> m_burstProjGroups = new List<BurstProjectilesGroup>();

	private bool m_didSetDataFromExtraParams;

	private Vector3 m_aimDirection;

	private float m_middleStartTime = -1f;

	private float m_endSideStartTime = -1f;

	private const int c_numSections = 3;

	private const int c_beginSideIdx = 0;

	private const int c_middleIdx = 1;

	private const int c_endSideIdx = 2;

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			BlasterStretchConeSequence.ExtraParams extraParams2 = extraSequenceParams as BlasterStretchConeSequence.ExtraParams;
			if (extraParams2 == null)
			{
				continue;
			}
			m_didSetDataFromExtraParams = true;
			if (extraParams2.forwardAngle >= 0f)
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
				m_aimDirection = VectorUtils.AngleDegreesToVector(extraParams2.forwardAngle);
			}
			if (extraParams2.angleInDegrees > 0f)
			{
				m_coneAngle = extraParams2.angleInDegrees;
			}
			if (extraParams2.lengthInSquares > 0f)
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
				m_maxProjectileDistInWorld = extraParams2.lengthInSquares * Board.Get().squareSize;
			}
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public override void FinishSetup()
	{
		m_projectilePerBurst = Mathf.Max(1, m_projectilePerBurst);
		if (!m_didSetDataFromExtraParams)
		{
			while (true)
			{
				switch (4)
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
			m_aimDirection = base.Caster.transform.forward;
			m_aimDirection.y = 0f;
		}
		Vector3 travelBoardSquareWorldPosition = base.Caster.GetTravelBoardSquareWorldPosition();
		float num = VectorUtils.HorizontalAngle_Deg(m_aimDirection);
		float num2 = m_coneAngle / 3f;
		float num3 = num + num2 * 1f;
		List<ActorData> list;
		if (base.Targets == null)
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
			list = new List<ActorData>();
		}
		else
		{
			list = new List<ActorData>(base.Targets);
		}
		List<ActorData> list2 = list;
		for (int i = 0; i < 3; i++)
		{
			BurstProjectilesGroup burstProjectilesGroup = new BurstProjectilesGroup();
			bool flag = i == 2;
			float num4 = num3 - (float)i * num2;
			num4 = (num4 + 360f) % 360f;
			List<ActorData> list3 = new List<ActorData>();
			for (int num5 = list2.Count - 1; num5 >= 0; num5--)
			{
				int num6;
				if (!flag)
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
					if (!(list2[num5].GetCurrentBoardSquare() == null))
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
						num6 = (AreaEffectUtils.IsSquareInConeByActorRadius(list2[num5].GetCurrentBoardSquare(), base.Caster.GetTravelBoardSquareWorldPosition(), num4, num2 + 5f, m_maxProjectileDistInWorld, 0f, true, base.Caster) ? 1 : 0);
						goto IL_0188;
					}
				}
				num6 = 1;
				goto IL_0188;
				IL_0188:
				if (num6 != 0)
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
					list3.Add(list2[num5]);
					list2.RemoveAt(num5);
				}
			}
			float num7 = num4;
			float num8 = 0f;
			if (m_projectilePerBurst > 1)
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
				float burstConePortion = m_burstConePortion;
				num7 = num4 + 0.5f * burstConePortion * num2;
				num8 = burstConePortion * num2 / (float)(m_projectilePerBurst - 1);
			}
			for (int j = 0; j < m_projectilePerBurst; j++)
			{
				float angle = num7 - (float)j * num8;
				Vector3 vector = VectorUtils.AngleDegreesToVector(angle);
				float maxProjectileDistInWorld = m_maxProjectileDistInWorld;
				Vector3 vector2 = travelBoardSquareWorldPosition + maxProjectileDistInWorld * vector;
				vector2.y = travelBoardSquareWorldPosition.y;
				bool flag2 = j == m_projectilePerBurst - 1;
				BurstVfxAuthoredData projectileInfo = m_projectileInfo;
				float maxProjectileDistInWorld2 = m_maxProjectileDistInWorld;
				Vector3 endPos = vector2;
				ActorData caster = base.Caster;
				object hitActors;
				if (flag2)
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
					hitActors = list3;
				}
				else
				{
					hitActors = null;
				}
				BurstVfxContainer burstVfxContainer = new BurstVfxContainer(this, projectileInfo, vector, maxProjectileDistInWorld2, endPos, caster, (List<ActorData>)hitActors);
				burstVfxContainer.m_timeTillSpawn = (float)j * m_timeBetweenProjInBurst;
				if (flag)
				{
					burstVfxContainer.m_posForSequenceHit = base.TargetPos;
				}
				burstProjectilesGroup.m_projectilesList.Add(burstVfxContainer);
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					goto end_IL_02cf;
				}
				continue;
				end_IL_02cf:
				break;
			}
			m_burstProjGroups.Add(burstProjectilesGroup);
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (m_beginSideStartEvent == null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					m_burstProjGroups[0].m_startUpdate = true;
					return;
				}
			}
			return;
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < m_burstProjGroups.Count; i++)
		{
			BurstProjectilesGroup burstProjectilesGroup = m_burstProjGroups[i];
			if (burstProjectilesGroup.m_startUpdate)
			{
				for (int j = 0; j < burstProjectilesGroup.m_projectilesList.Count; j++)
				{
					burstProjectilesGroup.m_projectilesList[j].OnSequenceDisable();
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
			}
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (m_beginSideStartEvent == parameter)
		{
			m_burstProjGroups[0].m_startUpdate = true;
			m_middleStartTime = GameTime.time + m_maxWaitTimeBetweenBursts;
			m_endSideStartTime = GameTime.time + 2f * m_maxWaitTimeBetweenBursts;
		}
		if (m_middleSideStartEvent == parameter)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_burstProjGroups[1].m_startUpdate = true;
		}
		if (!(m_endSideStartEvent == parameter))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			m_burstProjGroups[2].m_startUpdate = true;
			return;
		}
	}

	private void Update()
	{
		ProcessSequenceVisibility();
		int num = 0;
		while (num < m_burstProjGroups.Count)
		{
			BurstProjectilesGroup burstProjectilesGroup = m_burstProjGroups[num];
			if (!burstProjectilesGroup.m_startUpdate)
			{
				if (num == 1)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (m_middleStartTime > 0f)
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
						if (GameTime.time > m_middleStartTime)
						{
							goto IL_00aa;
						}
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
				if (num == 2)
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
					if (m_endSideStartTime > 0f)
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
						if (GameTime.time > m_endSideStartTime)
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
							goto IL_00aa;
						}
					}
				}
			}
			goto IL_00b1;
			IL_00b1:
			if (burstProjectilesGroup.m_startUpdate)
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
				for (int i = 0; i < burstProjectilesGroup.m_projectilesList.Count; i++)
				{
					burstProjectilesGroup.m_projectilesList[i].OnUpdate();
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
			num++;
			continue;
			IL_00aa:
			burstProjectilesGroup.m_startUpdate = true;
			goto IL_00b1;
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}
}
