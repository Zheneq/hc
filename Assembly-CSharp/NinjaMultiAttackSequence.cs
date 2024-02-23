using System.Collections.Generic;
using UnityEngine;

public class NinjaMultiAttackSequence : Sequence
{
	public class ExtraParams : IExtraSequenceParams
	{
		public Dictionary<ActorData, int> actorToHits;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			int count = actorToHits != null ? actorToHits.Count : 0;
			stream.Serialize(ref count);
			foreach (KeyValuePair<ActorData, int> actorToHit in actorToHits)
			{
				ActorData actor = actorToHit.Key;
				int actorIndex = actor != null ? actor.ActorIndex : ActorData.s_invalidActorIndex;
				int hits = actorToHit.Value;
				stream.Serialize(ref actorIndex);
				stream.Serialize(ref hits);
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			int count = 0;
			stream.Serialize(ref count);
			actorToHits = new Dictionary<ActorData, int>(count);
			for (int i = 0; i < count; i++)
			{
				int actorIndex = ActorData.s_invalidActorIndex;
				int hits = 0;
				stream.Serialize(ref actorIndex);
				stream.Serialize(ref hits);
				ActorData actor = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				actorToHits.Add(actor, hits);
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
		if (m_startEvent == null)
		{
			SetupHitInfoList();
			SpawnTempSatellite();
		}
	}

	private bool Finished()
	{
		if (m_hitInfos == null)
		{
			return false;
		}
		foreach (HitInfo hitInfo in m_hitInfos)
		{
			if (hitInfo.m_numHits > hitInfo.m_numHitsReceived)
			{
				return false;
			}
		}
		return true;
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
	}

	private bool SpawnedAllTempSatellites()
	{
		foreach (HitInfo hitInfo in m_hitInfos)
		{
			if (hitInfo.m_numHitsReceived < hitInfo.m_numHits)
			{
				return false;
			}
		}
		return true;
	}

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		if (m_lastTempSatelliteSpawnTime > 0f && GameTime.time > m_lastTempSatelliteSpawnTime + m_spawnDelay && !SpawnedAllTempSatellites())
		{
			SpawnTempSatellite();
		}
		foreach (HitInfo hitInfo in m_hitInfos)
		{
			if (hitInfo.m_didSpawnSatellite && hitInfo.m_spawnedSatellite == null)
			{
				int num = hitInfo.m_numHits - hitInfo.m_numHitsReceived;
				for (int j = 0; j < num; j++)
				{
					SpawnHitFX(hitInfo);
				}
			}
		}
	}

	private bool SquareInUseByTempSatellite(BoardSquare square)
	{
		bool result = false;
		foreach (HitInfo hitInfo in m_hitInfos)
		{
			if (hitInfo.m_boardSquare == square && hitInfo.m_spawnedSatellite != null)
			{
				result = true;
			}
		}
		return result;
	}

	private BoardSquare GetClosestUnoccupiedSquare(BoardSquare center)
	{
		BoardSquare boardSquare = null;
		for (int i = 1; i < 3; i++)
		{
			for (int j = -i; j <= i; j += i * 2)
			{
				BoardSquare square = Board.Get().GetSquareFromIndex(center.x + j, center.y);
				if (CanUseSquareForTempSatellite(square))
				{
					boardSquare = square;
					break;
				}
			}
			for (int j = -i; j <= i; j += i * 2)
			{
				BoardSquare square = Board.Get().GetSquareFromIndex(center.x, center.y + j);
				if (CanUseSquareForTempSatellite(square))
				{
					boardSquare = square;
					break;
				}
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
		return square != null
		       && square.IsValidForGameplay()
		       && square.occupant == null
		       && !SquareInUseByTempSatellite(square);
	}

	private HitInfo GetNextTarget()
	{
		foreach (HitInfo hitInfo in m_hitInfos)
		{
			if (!hitInfo.m_didSpawnSatellite)
			{
				return hitInfo;
			}
		}
		return null;
	}

	private void SpawnTempSatellite()
	{
		m_lastTempSatelliteSpawnTime = GameTime.time;
		HitInfo nextTarget = GetNextTarget();
		if (nextTarget == null)
		{
			return;
		}
		BoardSquare closestUnoccupiedSquare = GetClosestUnoccupiedSquare(nextTarget.m_target.GetCurrentBoardSquare());
		Vector3 forward = Vector3.forward;
		if (closestUnoccupiedSquare != nextTarget.m_target.GetCurrentBoardSquare())
		{
			forward = nextTarget.m_target.GetCurrentBoardSquare().ToVector3() - closestUnoccupiedSquare.ToVector3();
		}
		GameObject fxObject = InstantiateFX(m_tempSatellitePrefab, closestUnoccupiedSquare.ToVector3(), Quaternion.LookRotation(forward));
		fxObject.GetComponent<NinjaCloneSatellite>().Setup(this);
		fxObject.GetComponent<NinjaCloneSatellite>().TriggerMultiAttack(nextTarget.m_target.gameObject, nextTarget.m_numHits, m_attackDelay);
		nextTarget.m_spawnedSatellite = fxObject;
		nextTarget.m_spawnTime = GameTime.time;
		nextTarget.m_boardSquare = closestUnoccupiedSquare;
		nextTarget.m_didSpawnSatellite = true;
	}

	private void SetupHitInfoList()
	{
		m_hitFXInstances = new List<GameObject>();
		m_hitInfos = new List<HitInfo>();
		foreach (KeyValuePair<ActorData, int> current in m_actorsToHits)
		{
			HitInfo hitInfo = new HitInfo
			{
				m_numHits = current.Value,
				m_target = current.Key,
				m_spawnedSatellite = null,
				m_didSpawnSatellite = false,
				m_numHitsReceived = 0
			};
			JointPopupProperty jointPopupProperty = new JointPopupProperty
			{
				m_joint = m_hitFxJoint.m_joint,
				m_jointCharacter = m_hitFxJoint.m_jointCharacter
			};
			jointPopupProperty.Initialize(hitInfo.m_target.gameObject);
			hitInfo.m_hitJoint = jointPopupProperty;
			m_hitInfos.Add(hitInfo);
		}
	}

	private HitInfo GetHitInfoFromTempSatellite(GameObject tempSatellite)
	{
		foreach (HitInfo hitInfo in m_hitInfos)
		{
			if (hitInfo.m_spawnedSatellite == tempSatellite)
			{
				return hitInfo;
			}
		}
		return null;
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
			Source.OnSequenceHit(this, hitInfo.m_target, impulseInfo, ActorModelData.RagdollActivation.None);
		}
		else
		{
			Source.OnSequenceHit(this, hitInfo.m_target, impulseInfo);
		}
	}

	private void SpawnHitFX(GameObject sourceObject)
	{
		HitInfo hitInfoFromTempSatellite = GetHitInfoFromTempSatellite(sourceObject);
		if (hitInfoFromTempSatellite != null)
		{
			SpawnHitFX(hitInfoFromTempSatellite);
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (m_startEvent == parameter)
		{
			SetupHitInfoList();
			SpawnTempSatellite();
		}
		else if (m_hitEvent == parameter)
		{
			SpawnHitFX(sourceObject);
		}
	}

	private void OnDisable()
	{
		if (m_hitFXInstances != null)
		{
			foreach (GameObject hitFx in m_hitFXInstances)
			{
				Destroy(hitFx);
			}
			m_hitFXInstances = null;
		}
		m_initialized = false;
	}
}
