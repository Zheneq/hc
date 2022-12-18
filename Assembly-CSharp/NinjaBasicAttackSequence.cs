using System.Collections.Generic;
using UnityEngine;

public class NinjaBasicAttackSequence : Sequence
{
	public class HitInfo
	{
		public GameObject m_spawnedTempSatellite;
		public bool m_hitReceived;
		public bool m_didSpawnSatellite;
		public BoardSquare m_spawnSquare;
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
	public float m_attackDelay = 0.25f;
	public GameObject m_tempSatellitePrefab;

	private Dictionary<ActorData, HitInfo> m_targetToHitInfo;
	private List<GameObject> m_hitFXInstances;
	private float m_lastTempSatelliteSpawnTime = -1f;
	private int m_numTempSatellitesSpawned;
	private bool m_setFinishTrigger;

	public int m_numAttacksForSingleTarget = 7;
	public int m_numAttacksForMultiTarget = 3;

	public override void FinishSetup()
	{
		m_hitFXInstances = new List<GameObject>();
		m_targetToHitInfo = new Dictionary<ActorData, HitInfo>();
		if (m_startEvent == null)
		{
			SpawnTempSatellite();
		}
	}

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		if (m_lastTempSatelliteSpawnTime > 0f
		    && GameTime.time > m_lastTempSatelliteSpawnTime + m_spawnDelay
		    && m_numTempSatellitesSpawned < Targets.Length)
		{
			SpawnTempSatellite();
		}
		foreach (KeyValuePair<ActorData, HitInfo> current in m_targetToHitInfo)
		{
			HitInfo value = current.Value;
			if (value.m_didSpawnSatellite && value.m_spawnedTempSatellite == null && !value.m_hitReceived)
			{
				SpawnHitFX(current.Key);
			}
		}
		if (AllTempSatellitesDespawned() && !m_setFinishTrigger)
		{
			m_setFinishTrigger = true;
			Caster.GetActorModelData().GetModelAnimator().SetTrigger("FinishAttack");
		}
	}

	private bool AllTempSatellitesDespawned()
	{
		bool result = true;
		if (m_numTempSatellitesSpawned != Targets.Length)
		{
			return false;
		}
		foreach (KeyValuePair<ActorData, HitInfo> item in m_targetToHitInfo)
		{
			if (!item.Value.m_didSpawnSatellite
			    || (item.Value.m_spawnedTempSatellite != null
			        && !item.Value.m_spawnedTempSatellite.GetComponent<NinjaCloneSatellite>().IsDespawning()))
			{
				result = false;
			}
		}
		return result;
	}

	private bool SquareInUseByTempSatellite(BoardSquare square)
	{
		bool result = false;
		foreach (HitInfo value in m_targetToHitInfo.Values)
		{
			if (value.m_spawnSquare == square && value.m_spawnedTempSatellite != null)
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
				BoardSquare adjacentSquare = Board.Get().GetSquareFromIndex(center.x + j, center.y);
				if (adjacentSquare.occupant == null && !SquareInUseByTempSatellite(adjacentSquare))
				{
					boardSquare = adjacentSquare;
					break;
				}
			}
			for (int j = -i; j <= i; j += i * 2)
			{
				BoardSquare adjacentSquare = Board.Get().GetSquareFromIndex(center.x, center.y + j);
				if (adjacentSquare.occupant == null && !SquareInUseByTempSatellite(adjacentSquare))
				{
					boardSquare = adjacentSquare;
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

	private ActorData GetNextTarget()
	{
		ActorData result = null;
		if (m_numTempSatellitesSpawned < Targets.Length)
		{
			result = Targets[m_numTempSatellitesSpawned];
		}
		return result;
	}

	private void SpawnTempSatellite()
	{
		m_lastTempSatelliteSpawnTime = GameTime.time;
		ActorData nextTarget = GetNextTarget();
		if (nextTarget == null)
		{
			return;
		}
		BoardSquare closestUnoccupiedSquare = GetClosestUnoccupiedSquare(nextTarget.GetCurrentBoardSquare());
		Vector3 forward = Vector3.forward;
		if (closestUnoccupiedSquare != nextTarget.GetCurrentBoardSquare())
		{
			forward = nextTarget.GetCurrentBoardSquare().ToVector3() - closestUnoccupiedSquare.ToVector3();
		}

		GameObject fxObject = InstantiateFX(
			m_tempSatellitePrefab, 
			closestUnoccupiedSquare.ToVector3(),
			Quaternion.LookRotation(forward));
		fxObject.GetComponent<NinjaCloneSatellite>().Setup(this);
		fxObject.GetComponent<NinjaCloneSatellite>().TriggerMultiAttack(
			nextTarget.gameObject,
			Targets.Length == 1
				? m_numAttacksForSingleTarget
				: m_numAttacksForMultiTarget, m_attackDelay);

		HitInfo hitInfo = new HitInfo
		{
			m_spawnedTempSatellite = fxObject,
			m_spawnSquare = closestUnoccupiedSquare,
			m_didSpawnSatellite = true,
			m_hitReceived = false
		};
		m_targetToHitInfo.Add(nextTarget, hitInfo);
		m_numTempSatellitesSpawned++;
	}

	private ActorData GetTargetFromTempSatellite(GameObject tempSatellite)
	{
		foreach (KeyValuePair<ActorData, HitInfo> item in m_targetToHitInfo)
		{
			if (item.Value.m_spawnedTempSatellite == tempSatellite)
			{
				return item.Key;
			}
		}
		return null;
	}

	private void SpawnHitFX(ActorData target)
	{
		if (!m_targetToHitInfo.TryGetValue(target, out HitInfo value))
		{
			return;
		}
		value.m_hitReceived = true;
		m_hitFxJoint.Initialize(target.gameObject);
		GameObject item = InstantiateFX(m_fxHitPrefab, m_hitFxJoint.m_jointObject.transform.position, Quaternion.identity);
		m_hitFXInstances.Add(item);
		Vector3 position = m_hitFxJoint.m_jointObject.transform.position;
		Vector3 hitDirection = position - value.m_spawnSquare.ToVector3();
		hitDirection.Normalize();
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, hitDirection);
		Source.OnSequenceHit(this, target, impulseInfo);
	}

	private void SpawnHitFX(GameObject sourceObject)
	{
		ActorData targetFromTempSatellite = GetTargetFromTempSatellite(sourceObject);
		if (targetFromTempSatellite != null)
		{
			SpawnHitFX(targetFromTempSatellite);
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (m_startEvent == parameter)
		{
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
