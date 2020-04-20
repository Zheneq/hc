using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBurstConeSequence : Sequence
{
	[Header("-- Projectile Info --")]
	public SoldierBurstConeSequence.BurstVfxAuthoredData m_projectileInfo;

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

	private List<SoldierBurstConeSequence.BurstProjectilesGroup> m_burstProjGroups = new List<SoldierBurstConeSequence.BurstProjectilesGroup>();

	private bool m_didSetDataFromExtraParams;

	private Vector3 m_aimDirection;

	private float m_middleStartTime = -1f;

	private float m_endSideStartTime = -1f;

	private const int c_numSections = 3;

	private const int c_beginSideIdx = 0;

	private const int c_middleIdx = 1;

	private const int c_endSideIdx = 2;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			BlasterStretchConeSequence.ExtraParams extraParams2 = extraSequenceParams as BlasterStretchConeSequence.ExtraParams;
			if (extraParams2 != null)
			{
				this.m_didSetDataFromExtraParams = true;
				if (extraParams2.forwardAngle >= 0f)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierBurstConeSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
					}
					this.m_aimDirection = VectorUtils.AngleDegreesToVector(extraParams2.forwardAngle);
				}
				if (extraParams2.angleInDegrees > 0f)
				{
					this.m_coneAngle = extraParams2.angleInDegrees;
				}
				if (extraParams2.lengthInSquares > 0f)
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
					this.m_maxProjectileDistInWorld = extraParams2.lengthInSquares * Board.Get().squareSize;
				}
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
	}

	public override void FinishSetup()
	{
		this.m_projectilePerBurst = Mathf.Max(1, this.m_projectilePerBurst);
		if (!this.m_didSetDataFromExtraParams)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierBurstConeSequence.FinishSetup()).MethodHandle;
			}
			this.m_aimDirection = base.Caster.transform.forward;
			this.m_aimDirection.y = 0f;
		}
		Vector3 travelBoardSquareWorldPosition = base.Caster.GetTravelBoardSquareWorldPosition();
		float num = VectorUtils.HorizontalAngle_Deg(this.m_aimDirection);
		float num2 = this.m_coneAngle / 3f;
		float num3 = num + num2 * 1f;
		List<ActorData> list;
		if (base.Targets == null)
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
			list = new List<ActorData>();
		}
		else
		{
			list = new List<ActorData>(base.Targets);
		}
		List<ActorData> list2 = list;
		for (int i = 0; i < 3; i++)
		{
			SoldierBurstConeSequence.BurstProjectilesGroup burstProjectilesGroup = new SoldierBurstConeSequence.BurstProjectilesGroup();
			bool flag = i == 2;
			float num4 = num3 - (float)i * num2;
			num4 = (num4 + 360f) % 360f;
			List<ActorData> list3 = new List<ActorData>();
			int j = list2.Count - 1;
			while (j >= 0)
			{
				if (flag)
				{
					goto IL_187;
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
				if (list2[j].GetCurrentBoardSquare() == null)
				{
					goto IL_187;
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
				bool flag2 = AreaEffectUtils.IsSquareInConeByActorRadius(list2[j].GetCurrentBoardSquare(), base.Caster.GetTravelBoardSquareWorldPosition(), num4, num2 + 5f, this.m_maxProjectileDistInWorld, 0f, true, base.Caster, false, default(Vector3));
				IL_188:
				bool flag3 = flag2;
				if (flag3)
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
					list3.Add(list2[j]);
					list2.RemoveAt(j);
				}
				j--;
				continue;
				IL_187:
				flag2 = true;
				goto IL_188;
			}
			float num5 = num4;
			float num6 = 0f;
			if (this.m_projectilePerBurst > 1)
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
				float burstConePortion = this.m_burstConePortion;
				num5 = num4 + 0.5f * burstConePortion * num2;
				num6 = burstConePortion * num2 / (float)(this.m_projectilePerBurst - 1);
			}
			for (int k = 0; k < this.m_projectilePerBurst; k++)
			{
				float angle = num5 - (float)k * num6;
				Vector3 vector = VectorUtils.AngleDegreesToVector(angle);
				float maxProjectileDistInWorld = this.m_maxProjectileDistInWorld;
				Vector3 vector2 = travelBoardSquareWorldPosition + maxProjectileDistInWorld * vector;
				vector2.y = travelBoardSquareWorldPosition.y;
				bool flag4 = k == this.m_projectilePerBurst - 1;
				SoldierBurstConeSequence.BurstVfxAuthoredData projectileInfo = this.m_projectileInfo;
				Vector3 impactDir = vector;
				float maxProjectileDistInWorld2 = this.m_maxProjectileDistInWorld;
				Vector3 endPos = vector2;
				ActorData caster = base.Caster;
				List<ActorData> hitActors;
				if (flag4)
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
					hitActors = list3;
				}
				else
				{
					hitActors = null;
				}
				SoldierBurstConeSequence.BurstVfxContainer burstVfxContainer = new SoldierBurstConeSequence.BurstVfxContainer(this, projectileInfo, impactDir, maxProjectileDistInWorld2, endPos, caster, hitActors);
				burstVfxContainer.m_timeTillSpawn = (float)k * this.m_timeBetweenProjInBurst;
				if (flag)
				{
					burstVfxContainer.m_posForSequenceHit = base.TargetPos;
				}
				burstProjectilesGroup.m_projectilesList.Add(burstVfxContainer);
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
			this.m_burstProjGroups.Add(burstProjectilesGroup);
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
		if (this.m_beginSideStartEvent == null)
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
			this.m_burstProjGroups[0].m_startUpdate = true;
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < this.m_burstProjGroups.Count; i++)
		{
			SoldierBurstConeSequence.BurstProjectilesGroup burstProjectilesGroup = this.m_burstProjGroups[i];
			if (burstProjectilesGroup.m_startUpdate)
			{
				for (int j = 0; j < burstProjectilesGroup.m_projectilesList.Count; j++)
				{
					burstProjectilesGroup.m_projectilesList[j].OnSequenceDisable();
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierBurstConeSequence.OnDisable()).MethodHandle;
				}
			}
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
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_beginSideStartEvent == parameter)
		{
			this.m_burstProjGroups[0].m_startUpdate = true;
			this.m_middleStartTime = GameTime.time + this.m_maxWaitTimeBetweenBursts;
			this.m_endSideStartTime = GameTime.time + 2f * this.m_maxWaitTimeBetweenBursts;
		}
		if (this.m_middleSideStartEvent == parameter)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierBurstConeSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.m_burstProjGroups[1].m_startUpdate = true;
		}
		if (this.m_endSideStartEvent == parameter)
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
			this.m_burstProjGroups[2].m_startUpdate = true;
		}
	}

	private void Update()
	{
		base.ProcessSequenceVisibility();
		for (int i = 0; i < this.m_burstProjGroups.Count; i++)
		{
			SoldierBurstConeSequence.BurstProjectilesGroup burstProjectilesGroup = this.m_burstProjGroups[i];
			if (!burstProjectilesGroup.m_startUpdate)
			{
				if (i == 1)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierBurstConeSequence.Update()).MethodHandle;
					}
					if (this.m_middleStartTime > 0f)
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
						if (GameTime.time > this.m_middleStartTime)
						{
							goto IL_AA;
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
				}
				if (i != 2)
				{
					goto IL_B1;
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
				if (this.m_endSideStartTime <= 0f)
				{
					goto IL_B1;
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
				if (GameTime.time <= this.m_endSideStartTime)
				{
					goto IL_B1;
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
				IL_AA:
				burstProjectilesGroup.m_startUpdate = true;
			}
			IL_B1:
			if (burstProjectilesGroup.m_startUpdate)
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
				for (int j = 0; j < burstProjectilesGroup.m_projectilesList.Count; j++)
				{
					burstProjectilesGroup.m_projectilesList[j].OnUpdate();
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

	public class BurstProjectilesGroup
	{
		public bool m_startUpdate;

		public List<SoldierBurstConeSequence.BurstVfxContainer> m_projectilesList = new List<SoldierBurstConeSequence.BurstVfxContainer>();
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

		public SoldierBurstConeSequence.BurstVfxAuthoredData m_authoredData;

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

		public BurstVfxContainer(Sequence parentSequence, SoldierBurstConeSequence.BurstVfxAuthoredData authoredData, Vector3 impactDir, float maxDistInWorld, Vector3 endPos, ActorData caster, List<ActorData> hitActors)
		{
			this.m_parentSequence = parentSequence;
			this.m_authoredData = authoredData;
			this.m_impactDir = impactDir;
			this.m_maxDistInWorld = maxDistInWorld;
			this.m_caster = caster;
			this.m_hitActors = hitActors;
			this.m_endPos = endPos;
			this.m_posForSequenceHit = endPos;
		}

		public void OnUpdate()
		{
			if (this.m_timeTillSpawn > 0f)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierBurstConeSequence.BurstVfxContainer.OnUpdate()).MethodHandle;
				}
				this.m_timeTillSpawn -= GameTime.deltaTime;
			}
			else if (this.m_fxInstance == null)
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
				if (this.m_authoredData.m_fxImpactPrefab != null)
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
					this.SpawnFX();
				}
			}
			if (this.m_impactSpawnTimestamp > 0f)
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
				if (GameTime.time >= this.m_impactSpawnTimestamp)
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
					this.SpawnImpactFX();
					this.m_impactSpawnTimestamp = -1f;
				}
			}
		}

		private float GetProjectileDistance(Vector3 start, Vector3 forward, float maxDist)
		{
			Vector3 vector = start;
			vector.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
			Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, forward, maxDist, false, this.m_caster, null, true);
			return (vector - laserEndPoint).magnitude;
		}

		private void SpawnFX()
		{
			JointPopupProperty spawnFxJoint = this.m_authoredData.m_spawnFxJoint;
			if (!spawnFxJoint.IsInitialized())
			{
				spawnFxJoint.Initialize(this.m_caster.gameObject);
			}
			Vector3 forward = this.m_endPos - spawnFxJoint.m_jointObject.transform.position;
			forward.y = 0f;
			this.m_fxInstance = this.m_parentSequence.InstantiateFX(this.m_authoredData.m_fxPrefab, spawnFxJoint.m_jointObject.transform.position, Quaternion.LookRotation(forward), true, true);
			float projectileDistance = this.GetProjectileDistance(this.m_caster.transform.position, this.m_impactDir, this.m_maxDistInWorld);
			Sequence.SetAttribute(this.m_fxInstance, "projectileDistance", projectileDistance);
			if (!string.IsNullOrEmpty(this.m_authoredData.m_spawnAudioEvent))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierBurstConeSequence.BurstVfxContainer.SpawnFX()).MethodHandle;
				}
				AudioManager.PostEvent(this.m_authoredData.m_spawnAudioEvent, this.m_caster.gameObject);
			}
			if (this.m_authoredData.m_hitDelayTime > 0f)
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
				this.m_impactSpawnTimestamp = GameTime.time + this.m_authoredData.m_hitDelayTime;
			}
			else
			{
				this.SpawnImpactFX();
			}
		}

		private void SpawnImpactFX()
		{
			if (this.m_hitActors != null)
			{
				for (int i = 0; i < this.m_hitActors.Count; i++)
				{
					ActorData actorData = this.m_hitActors[i];
					if (actorData == null)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierBurstConeSequence.BurstVfxContainer.SpawnImpactFX()).MethodHandle;
						}
					}
					else
					{
						GameObject gameObject = this.m_authoredData.m_impactFxJoint.FindJointObject(actorData.gameObject);
						Vector3 position;
						if (gameObject != null)
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
							position = gameObject.transform.position;
						}
						else
						{
							position = actorData.transform.position;
						}
						Vector3 vector = position;
						ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(vector, this.m_impactDir);
						if (this.m_authoredData.m_fxImpactPrefab)
						{
							Quaternion rotation = Quaternion.LookRotation(this.m_impactDir);
							this.m_impactFxInstances.Add(this.m_parentSequence.InstantiateFX(this.m_authoredData.m_fxImpactPrefab, vector, rotation, true, true));
						}
						if (!string.IsNullOrEmpty(this.m_authoredData.m_impactAudioEvent))
						{
							AudioManager.PostEvent(this.m_authoredData.m_impactAudioEvent, actorData.gameObject);
						}
						this.m_parentSequence.Source.OnSequenceHit(this.m_parentSequence, actorData, impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
					}
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.m_parentSequence.Source.OnSequenceHit(this.m_parentSequence, this.m_posForSequenceHit, null);
		}

		public void OnSequenceDisable()
		{
			if (this.m_fxInstance != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierBurstConeSequence.BurstVfxContainer.OnSequenceDisable()).MethodHandle;
				}
				UnityEngine.Object.Destroy(this.m_fxInstance.gameObject);
				this.m_fxInstance = null;
			}
			if (this.m_impactFxInstances != null)
			{
				for (int i = 0; i < this.m_impactFxInstances.Count; i++)
				{
					if (this.m_impactFxInstances[i] != null)
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
						UnityEngine.Object.Destroy(this.m_impactFxInstances[i].gameObject);
					}
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_impactFxInstances.Clear();
			}
		}
	}
}
