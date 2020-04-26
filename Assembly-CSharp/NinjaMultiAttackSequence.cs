using System.Collections.Generic;
using UnityEngine;

public class NinjaMultiAttackSequence : Sequence
{
	public class ExtraParams : IExtraSequenceParams
	{
		public Dictionary<ActorData, int> actorToHits;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			int num;
			if (actorToHits != null)
			{
				num = actorToHits.Count;
			}
			else
			{
				num = 0;
			}
			int value = num;
			stream.Serialize(ref value);
			foreach (KeyValuePair<ActorData, int> actorToHit in actorToHits)
			{
				ActorData key = actorToHit.Key;
				int num2;
				if (key != null)
				{
					num2 = key.ActorIndex;
				}
				else
				{
					num2 = ActorData.s_invalidActorIndex;
				}
				int value2 = num2;
				int value3 = actorToHit.Value;
				stream.Serialize(ref value2);
				stream.Serialize(ref value3);
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			int value = 0;
			stream.Serialize(ref value);
			actorToHits = new Dictionary<ActorData, int>(value);
			for (int i = 0; i < value; i++)
			{
				int value2 = ActorData.s_invalidActorIndex;
				int value3 = 0;
				stream.Serialize(ref value2);
				stream.Serialize(ref value3);
				ActorData key = GameFlowData.Get().FindActorByActorIndex(value2);
				actorToHits.Add(key, value3);
			}
			while (true)
			{
				return;
			}
		}
	}

	private class HitInfo
	{
		public GameObject m_spawnedSatellite;

		public float m_spawnTime = -1f;

		public BoardSquare m_boardSquare;

		public ActorData m_target;

		public JointPopupProperty m_hitJoint;

		public int m_numHits;

		public int m_numHitsReceived;

		public bool m_didSpawnSatellite;
	}

	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public Object m_startEvent;

	[Tooltip("Animation event (if any) to wait for playing a hitreact. Search project for EventObjects.")]
	[AnimEventPicker]
	public Object m_hitEvent;

	[Tooltip("Main FX prefab.")]
	public GameObject m_fxHitPrefab;

	[JointPopup("hit FX attach joint")]
	public JointPopupProperty m_hitFxJoint;

	public float m_spawnDelay = 0.5f;

	public float m_attackDelay = 0.1f;

	public GameObject m_tempSatellitePrefab;

	private Dictionary<ActorData, int> m_actorsToHits;

	private List<GameObject> m_hitFXInstances;

	private float m_lastTempSatelliteSpawnTime = -1f;

	private List<HitInfo> m_hitInfos;

	public override void FinishSetup()
	{
		if (!(m_startEvent == null))
		{
			return;
		}
		while (true)
		{
			SetupHitInfoList();
			SpawnTempSatellite();
			return;
		}
	}

	private bool Finished()
	{
		bool result = false;
		if (m_hitInfos != null)
		{
			result = true;
			int num = 0;
			while (true)
			{
				if (num < m_hitInfos.Count)
				{
					HitInfo hitInfo = m_hitInfos[num];
					if (hitInfo.m_numHits > hitInfo.m_numHitsReceived)
					{
						result = false;
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}
		return result;
	}

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ExtraParams extraParams2 = extraSequenceParams as ExtraParams;
			if (extraParams2 != null)
			{
				m_actorsToHits = extraParams2.actorToHits;
			}
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

	private bool SpawnedAllTempSatellites()
	{
		bool result = true;
		int num = 0;
		while (true)
		{
			if (num < m_hitInfos.Count)
			{
				HitInfo hitInfo = m_hitInfos[num];
				if (hitInfo.m_numHitsReceived < hitInfo.m_numHits)
				{
					result = false;
					break;
				}
				num++;
				continue;
			}
			break;
		}
		return result;
	}

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			if (m_lastTempSatelliteSpawnTime > 0f && GameTime.time > m_lastTempSatelliteSpawnTime + m_spawnDelay && !SpawnedAllTempSatellites())
			{
				SpawnTempSatellite();
			}
			for (int i = 0; i < m_hitInfos.Count; i++)
			{
				if (!m_hitInfos[i].m_didSpawnSatellite)
				{
					continue;
				}
				if (m_hitInfos[i].m_spawnedSatellite == null)
				{
					int num = m_hitInfos[i].m_numHits - m_hitInfos[i].m_numHitsReceived;
					for (int j = 0; j < num; j++)
					{
						SpawnHitFX(m_hitInfos[i]);
					}
				}
			}
			return;
		}
	}

	private bool SquareInUseByTempSatellite(BoardSquare square)
	{
		bool result = false;
		for (int i = 0; i < m_hitInfos.Count; i++)
		{
			if (!(m_hitInfos[i].m_boardSquare == square))
			{
				continue;
			}
			if (m_hitInfos[i].m_spawnedSatellite != null)
			{
				result = true;
			}
		}
		while (true)
		{
			return result;
		}
	}

	private BoardSquare GetClosestUnoccupiedSquare(BoardSquare center)
	{
		BoardSquare boardSquare = null;
		for (int i = 1; i < 3; i++)
		{
			int num = -i;
			while (true)
			{
				if (num <= i)
				{
					BoardSquare boardSquare2 = Board.Get().GetBoardSquare(center.x + num, center.y);
					if (CanUseSquareForTempSatellite(boardSquare2))
					{
						boardSquare = boardSquare2;
						break;
					}
					num += i * 2;
					continue;
				}
				break;
			}
			int num2 = -i;
			while (true)
			{
				if (num2 <= i)
				{
					BoardSquare boardSquare3 = Board.Get().GetBoardSquare(center.x, center.y + num2);
					if (CanUseSquareForTempSatellite(boardSquare3))
					{
						boardSquare = boardSquare3;
						break;
					}
					num2 += i * 2;
					continue;
				}
				break;
			}
			if (boardSquare != null)
			{
				break;
			}
		}
		if (boardSquare == null)
		{
			boardSquare = center;
		}
		return boardSquare;
	}

	private bool CanUseSquareForTempSatellite(BoardSquare square)
	{
		int result;
		if (square != null)
		{
			if (square.IsBaselineHeight() && square.occupant == null)
			{
				result = ((!SquareInUseByTempSatellite(square)) ? 1 : 0);
				goto IL_004d;
			}
		}
		result = 0;
		goto IL_004d;
		IL_004d:
		return (byte)result != 0;
	}

	private HitInfo GetNextTarget()
	{
		HitInfo result = null;
		int num = 0;
		while (true)
		{
			if (num < m_hitInfos.Count)
			{
				HitInfo hitInfo = m_hitInfos[num];
				if (!hitInfo.m_didSpawnSatellite)
				{
					result = hitInfo;
					break;
				}
				num++;
				continue;
			}
			break;
		}
		return result;
	}

	private void SpawnTempSatellite()
	{
		m_lastTempSatelliteSpawnTime = GameTime.time;
		HitInfo nextTarget = GetNextTarget();
		if (nextTarget == null)
		{
			return;
		}
		while (true)
		{
			BoardSquare closestUnoccupiedSquare = GetClosestUnoccupiedSquare(nextTarget.m_target.GetCurrentBoardSquare());
			Vector3 forward = Vector3.forward;
			if (closestUnoccupiedSquare != nextTarget.m_target.GetCurrentBoardSquare())
			{
				forward = nextTarget.m_target.GetCurrentBoardSquare().ToVector3() - closestUnoccupiedSquare.ToVector3();
			}
			GameObject gameObject = InstantiateFX(m_tempSatellitePrefab, closestUnoccupiedSquare.ToVector3(), Quaternion.LookRotation(forward));
			gameObject.GetComponent<NinjaCloneSatellite>().Setup(this);
			gameObject.GetComponent<NinjaCloneSatellite>().TriggerMultiAttack(nextTarget.m_target.gameObject, nextTarget.m_numHits, m_attackDelay);
			nextTarget.m_spawnedSatellite = gameObject;
			nextTarget.m_spawnTime = GameTime.time;
			nextTarget.m_boardSquare = closestUnoccupiedSquare;
			nextTarget.m_didSpawnSatellite = true;
			return;
		}
	}

	private void SetupHitInfoList()
	{
		m_hitFXInstances = new List<GameObject>();
		m_hitInfos = new List<HitInfo>();
		using (Dictionary<ActorData, int>.Enumerator enumerator = m_actorsToHits.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, int> current = enumerator.Current;
				HitInfo hitInfo = new HitInfo();
				hitInfo.m_numHits = current.Value;
				hitInfo.m_target = current.Key;
				hitInfo.m_spawnedSatellite = null;
				hitInfo.m_didSpawnSatellite = false;
				hitInfo.m_numHitsReceived = 0;
				JointPopupProperty jointPopupProperty = new JointPopupProperty();
				jointPopupProperty.m_joint = m_hitFxJoint.m_joint;
				jointPopupProperty.m_jointCharacter = m_hitFxJoint.m_jointCharacter;
				jointPopupProperty.Initialize(hitInfo.m_target.gameObject);
				hitInfo.m_hitJoint = jointPopupProperty;
				m_hitInfos.Add(hitInfo);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
	}

	private HitInfo GetHitInfoFromTempSatellite(GameObject tempSatellite)
	{
		HitInfo result = null;
		int num = 0;
		while (true)
		{
			if (num < m_hitInfos.Count)
			{
				HitInfo hitInfo = m_hitInfos[num];
				if (hitInfo.m_spawnedSatellite == tempSatellite)
				{
					result = hitInfo;
					break;
				}
				num++;
				continue;
			}
			break;
		}
		return result;
	}

	private void SpawnHitFX(HitInfo hitInfo)
	{
		hitInfo.m_numHitsReceived++;
		if (m_fxHitPrefab != null)
		{
			GameObject item = InstantiateFX(m_fxHitPrefab, hitInfo.m_hitJoint.m_jointObject.transform.position, Quaternion.identity);
			m_hitFXInstances.Add(item);
		}
		Vector3 position = hitInfo.m_hitJoint.m_jointObject.transform.position;
		Vector3 hitDirection = position - hitInfo.m_boardSquare.ToVector3();
		hitDirection.Normalize();
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, hitDirection);
		if (hitInfo.m_numHits > hitInfo.m_numHitsReceived)
		{
			base.Source.OnSequenceHit(this, hitInfo.m_target, impulseInfo, ActorModelData.RagdollActivation.None);
		}
		else
		{
			base.Source.OnSequenceHit(this, hitInfo.m_target, impulseInfo);
		}
	}

	private void SpawnHitFX(GameObject sourceObject)
	{
		HitInfo hitInfoFromTempSatellite = GetHitInfoFromTempSatellite(sourceObject);
		if (hitInfoFromTempSatellite == null)
		{
			return;
		}
		while (true)
		{
			SpawnHitFX(hitInfoFromTempSatellite);
			return;
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (m_startEvent == parameter)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					SetupHitInfoList();
					SpawnTempSatellite();
					return;
				}
			}
		}
		if (!(m_hitEvent == parameter))
		{
			return;
		}
		while (true)
		{
			SpawnHitFX(sourceObject);
			return;
		}
	}

	private void OnDisable()
	{
		if (m_hitFXInstances != null)
		{
			for (int i = 0; i < m_hitFXInstances.Count; i++)
			{
				Object.Destroy(m_hitFXInstances[i]);
			}
			m_hitFXInstances = null;
		}
		m_initialized = false;
	}
}
