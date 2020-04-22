using System.Collections.Generic;
using UnityEngine;

public class SoldierProjectilesInLineSequence : Sequence
{
	public class HitAreaExtraParams : IExtraSequenceParams
	{
		public Vector3 fromPos;

		public Vector3 toPos;

		public float areaWidthInSquares;

		public bool ignoreStartEvent;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			stream.Serialize(ref fromPos);
			stream.Serialize(ref toPos);
			stream.Serialize(ref areaWidthInSquares);
			stream.Serialize(ref ignoreStartEvent);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			stream.Serialize(ref fromPos);
			stream.Serialize(ref toPos);
			stream.Serialize(ref areaWidthInSquares);
			stream.Serialize(ref ignoreStartEvent);
		}
	}

	[Header("-- Projectile Info --")]
	public GenericSequenceProjectileAuthoredInfo m_projectileInfo;

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public Object m_startEvent;

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

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			HitAreaExtraParams hitAreaExtraParams = extraSequenceParams as HitAreaExtraParams;
			if (hitAreaExtraParams != null)
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
				m_didSetDataFromExtraParams = true;
				m_fromPos = hitAreaExtraParams.fromPos;
				m_toPos = hitAreaExtraParams.toPos;
				m_areaWidthInSquares = hitAreaExtraParams.areaWidthInSquares;
				m_ignoreStartEvent = hitAreaExtraParams.ignoreStartEvent;
			}
		}
		while (true)
		{
			switch (4)
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
		bool flag = false;
		if (m_didSetDataFromExtraParams)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Vector3 vector = m_toPos - m_fromPos;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			vector.Normalize();
			Vector3 normalized = Vector3.Cross(vector, Vector3.up).normalized;
			float squareSize = Board.Get().squareSize;
			float num = 0.5f * m_areaWidthInSquares * squareSize;
			float num2 = m_maxVariationForDistBetween * squareSize;
			float num3 = Mathf.Max(0.3f, m_distBetweenProjectile * Board.Get().squareSize);
			int num4 = Mathf.CeilToInt(magnitude / num3);
			if (num4 > 0)
			{
				flag = true;
				List<ActorData> list = new List<ActorData>();
				for (int i = 0; i <= num4; i++)
				{
					Vector3 vector2 = m_fromPos + vector * ((float)i * num3);
					List<ActorData> list2 = new List<ActorData>();
					if (base.Targets != null)
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
						for (int j = 0; j < base.Targets.Length; j++)
						{
							ActorData actorData = base.Targets[j];
							if (list.Contains(actorData))
							{
								continue;
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
							Vector3 rhs = actorData.GetTravelBoardSquareWorldPosition() - vector2;
							rhs.y = 0f;
							if (i != num4)
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
								if (!(Vector3.Dot(vector, rhs) <= 0f))
								{
									continue;
								}
							}
							list2.Add(actorData);
							list.Add(actorData);
						}
					}
					Vector3 vector3 = vector2;
					Vector3 startPos = vector3;
					startPos.y += m_startHeightFromFloor * Board.Get().squareSize;
					startPos -= m_backwardsOffset * Board.Get().squareSize * vector;
					vector3 += Random.Range(0f - num, num) * normalized;
					vector3 += Random.Range(0f - num2, num2) * vector;
					vector3.y = Board.Get().BaselineHeight;
					GenericSequenceProjectileInfo genericSequenceProjectileInfo = new GenericSequenceProjectileInfo(this, m_projectileInfo, startPos, vector3, list2.ToArray());
					genericSequenceProjectileInfo.m_startDelay = Mathf.Clamp((float)i * m_timeBetweenSpawns + Random.Range(0f, m_timeMaxVariation), 0f, 3f);
					if (i == num4)
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
						genericSequenceProjectileInfo.m_positionForSequenceHit = base.TargetPos;
					}
					m_projectilesList.Add(genericSequenceProjectileInfo);
				}
			}
		}
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
			CallHitSequenceOnTargets(base.TargetPos);
		}
		if (!(m_startEvent == null))
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
			if (!m_ignoreStartEvent)
			{
				return;
			}
		}
		m_startProjectileUpdate = true;
	}

	private void OnDisable()
	{
		foreach (GenericSequenceProjectileInfo projectiles in m_projectilesList)
		{
			if (projectiles != null)
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
				projectiles.OnSequenceDisable();
			}
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (!(m_startEvent == parameter))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_startProjectileUpdate = true;
			return;
		}
	}

	private void Update()
	{
		ProcessSequenceVisibility();
		if (!m_startProjectileUpdate)
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
			for (int i = 0; i < m_projectilesList.Count; i++)
			{
				m_projectilesList[i].OnUpdate();
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
	}
}
