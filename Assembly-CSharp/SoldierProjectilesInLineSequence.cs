using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierProjectilesInLineSequence : Sequence
{
	[Header("-- Projectile Info --")]
	public GenericSequenceProjectileAuthoredInfo m_projectileInfo;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_startEvent;

	[Header("-- Projectile Placement --")]
	public float m_distBetweenProjectile = 1.5f;

	public float m_maxVariationForDistBetween = 0.2f;

	[Header("-- For where to start relative to end position --")]
	public float m_startHeightFromFloor = 10f;

	public float m_backwardsOffset = 5f;

	[Header("-- Projectile Timing --")]
	public float m_timeBetweenSpawns = 0.35f;

	public float m_timeMaxVariation = 0.1f;

	private List<GenericSequenceProjectileInfo> m_projectilesList = new List<GenericSequenceProjectileInfo>();

	private bool m_didSetDataFromExtraParams;

	private Vector3 m_fromPos;

	private Vector3 m_toPos;

	private float m_areaWidthInSquares = 3f;

	private bool m_ignoreStartEvent;

	private bool m_startProjectileUpdate;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			SoldierProjectilesInLineSequence.HitAreaExtraParams hitAreaExtraParams = extraSequenceParams as SoldierProjectilesInLineSequence.HitAreaExtraParams;
			if (hitAreaExtraParams != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierProjectilesInLineSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				this.m_didSetDataFromExtraParams = true;
				this.m_fromPos = hitAreaExtraParams.fromPos;
				this.m_toPos = hitAreaExtraParams.toPos;
				this.m_areaWidthInSquares = hitAreaExtraParams.areaWidthInSquares;
				this.m_ignoreStartEvent = hitAreaExtraParams.ignoreStartEvent;
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

	public override void FinishSetup()
	{
		bool flag = false;
		if (this.m_didSetDataFromExtraParams)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierProjectilesInLineSequence.FinishSetup()).MethodHandle;
			}
			Vector3 vector = this.m_toPos - this.m_fromPos;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			vector.Normalize();
			Vector3 normalized = Vector3.Cross(vector, Vector3.up).normalized;
			float squareSize = Board.Get().squareSize;
			float num = 0.5f * this.m_areaWidthInSquares * squareSize;
			float num2 = this.m_maxVariationForDistBetween * squareSize;
			float num3 = Mathf.Max(0.3f, this.m_distBetweenProjectile * Board.Get().squareSize);
			int num4 = Mathf.CeilToInt(magnitude / num3);
			if (num4 > 0)
			{
				flag = true;
				List<ActorData> list = new List<ActorData>();
				for (int i = 0; i <= num4; i++)
				{
					Vector3 vector2 = this.m_fromPos + vector * ((float)i * num3);
					List<ActorData> list2 = new List<ActorData>();
					if (base.Targets != null)
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
						for (int j = 0; j < base.Targets.Length; j++)
						{
							ActorData actorData = base.Targets[j];
							if (!list.Contains(actorData))
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
								Vector3 rhs = actorData.GetTravelBoardSquareWorldPosition() - vector2;
								rhs.y = 0f;
								if (i != num4)
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
									if (Vector3.Dot(vector, rhs) > 0f)
									{
										goto IL_18D;
									}
								}
								list2.Add(actorData);
								list.Add(actorData);
							}
							IL_18D:;
						}
					}
					Vector3 vector3 = vector2;
					Vector3 vector4 = vector3;
					vector4.y += this.m_startHeightFromFloor * Board.Get().squareSize;
					vector4 -= this.m_backwardsOffset * Board.Get().squareSize * vector;
					vector3 += UnityEngine.Random.Range(-num, num) * normalized;
					vector3 += UnityEngine.Random.Range(-num2, num2) * vector;
					vector3.y = (float)Board.Get().BaselineHeight;
					GenericSequenceProjectileInfo genericSequenceProjectileInfo = new GenericSequenceProjectileInfo(this, this.m_projectileInfo, vector4, vector3, list2.ToArray());
					genericSequenceProjectileInfo.m_startDelay = Mathf.Clamp((float)i * this.m_timeBetweenSpawns + UnityEngine.Random.Range(0f, this.m_timeMaxVariation), 0f, 3f);
					if (i == num4)
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
						genericSequenceProjectileInfo.m_positionForSequenceHit = base.TargetPos;
					}
					this.m_projectilesList.Add(genericSequenceProjectileInfo);
				}
			}
		}
		if (!flag)
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
			base.CallHitSequenceOnTargets(base.TargetPos, 1f, null, true);
		}
		if (!(this.m_startEvent == null))
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
			if (!this.m_ignoreStartEvent)
			{
				return;
			}
		}
		this.m_startProjectileUpdate = true;
	}

	private void OnDisable()
	{
		foreach (GenericSequenceProjectileInfo genericSequenceProjectileInfo in this.m_projectilesList)
		{
			if (genericSequenceProjectileInfo != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierProjectilesInLineSequence.OnDisable()).MethodHandle;
				}
				genericSequenceProjectileInfo.OnSequenceDisable();
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierProjectilesInLineSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.m_startProjectileUpdate = true;
		}
	}

	private void Update()
	{
		base.ProcessSequenceVisibility();
		if (this.m_startProjectileUpdate)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierProjectilesInLineSequence.Update()).MethodHandle;
			}
			for (int i = 0; i < this.m_projectilesList.Count; i++)
			{
				this.m_projectilesList[i].OnUpdate();
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
	}

	public class HitAreaExtraParams : Sequence.IExtraSequenceParams
	{
		public Vector3 fromPos;

		public Vector3 toPos;

		public float areaWidthInSquares;

		public bool ignoreStartEvent;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref this.fromPos);
			stream.Serialize(ref this.toPos);
			stream.Serialize(ref this.areaWidthInSquares);
			stream.Serialize(ref this.ignoreStartEvent);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref this.fromPos);
			stream.Serialize(ref this.toPos);
			stream.Serialize(ref this.areaWidthInSquares);
			stream.Serialize(ref this.ignoreStartEvent);
		}
	}
}
