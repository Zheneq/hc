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
		if (!(m_startEvent == null))
		{
			return;
		}
		while (true)
		{
			SpawnTempSatellite();
			return;
		}
	}

	private void Update()
	{
		if (!m_initialized)
		{
			return;
		}
		if (m_lastTempSatelliteSpawnTime > 0f)
		{
			if (GameTime.time > m_lastTempSatelliteSpawnTime + m_spawnDelay)
			{
				if (m_numTempSatellitesSpawned < base.Targets.Length)
				{
					SpawnTempSatellite();
				}
			}
		}
		using (Dictionary<ActorData, HitInfo>.Enumerator enumerator = m_targetToHitInfo.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, HitInfo> current = enumerator.Current;
				HitInfo value = current.Value;
				if (value.m_didSpawnSatellite)
				{
					if (value.m_spawnedTempSatellite == null)
					{
						if (!value.m_hitReceived)
						{
							SpawnHitFX(current.Key);
						}
					}
				}
			}
		}
		if (!AllTempSatellitesDespawned())
		{
			return;
		}
		while (true)
		{
			if (!m_setFinishTrigger)
			{
				while (true)
				{
					m_setFinishTrigger = true;
					base.Caster.GetActorModelData().GetModelAnimator().SetTrigger("FinishAttack");
					return;
				}
			}
			return;
		}
	}

	private bool AllTempSatellitesDespawned()
	{
		bool result = true;
		if (m_numTempSatellitesSpawned != base.Targets.Length)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		foreach (KeyValuePair<ActorData, HitInfo> item in m_targetToHitInfo)
		{
			if (item.Value.m_didSpawnSatellite)
			{
				if (!(item.Value.m_spawnedTempSatellite != null))
				{
					continue;
				}
				if (item.Value.m_spawnedTempSatellite.GetComponent<NinjaCloneSatellite>().IsDespawning())
				{
					continue;
				}
			}
			result = false;
		}
		return result;
	}

	private bool SquareInUseByTempSatellite(BoardSquare square)
	{
		bool result = false;
		using (Dictionary<ActorData, HitInfo>.Enumerator enumerator = m_targetToHitInfo.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				HitInfo value = enumerator.Current.Value;
				if (value.m_spawnSquare == square && value.m_spawnedTempSatellite != null)
				{
					result = true;
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return result;
				}
			}
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
					BoardSquare boardSquare2 = Board.Get().GetSquareFromIndex(center.x + num, center.y);
					if (boardSquare2.occupant == null && !SquareInUseByTempSatellite(boardSquare2))
					{
						boardSquare = boardSquare2;
						break;
					}
					num += i * 2;
					continue;
				}
				break;
			}
			for (int j = -i; j <= i; j += i * 2)
			{
				BoardSquare boardSquare3 = Board.Get().GetSquareFromIndex(center.x, center.y + j);
				if (boardSquare3.occupant == null && !SquareInUseByTempSatellite(boardSquare3))
				{
					boardSquare = boardSquare3;
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
		if (m_numTempSatellitesSpawned < base.Targets.Length)
		{
			result = base.Targets[m_numTempSatellitesSpawned];
		}
		return result;
	}

	private void SpawnTempSatellite()
	{
		m_lastTempSatelliteSpawnTime = GameTime.time;
		ActorData nextTarget = GetNextTarget();
		if (!(nextTarget != null))
		{
			return;
		}
		while (true)
		{
			BoardSquare closestUnoccupiedSquare = GetClosestUnoccupiedSquare(nextTarget.GetCurrentBoardSquare());
			Vector3 forward = Vector3.forward;
			if (closestUnoccupiedSquare != nextTarget.GetCurrentBoardSquare())
			{
				forward = nextTarget.GetCurrentBoardSquare().ToVector3() - closestUnoccupiedSquare.ToVector3();
			}
			GameObject gameObject = InstantiateFX(m_tempSatellitePrefab, closestUnoccupiedSquare.ToVector3(), Quaternion.LookRotation(forward));
			gameObject.GetComponent<NinjaCloneSatellite>().Setup(this);
			if (base.Targets.Length == 1)
			{
				gameObject.GetComponent<NinjaCloneSatellite>().TriggerMultiAttack(nextTarget.gameObject, m_numAttacksForSingleTarget, m_attackDelay);
			}
			else
			{
				gameObject.GetComponent<NinjaCloneSatellite>().TriggerMultiAttack(nextTarget.gameObject, m_numAttacksForMultiTarget, m_attackDelay);
			}
			HitInfo hitInfo = new HitInfo();
			hitInfo.m_spawnedTempSatellite = gameObject;
			hitInfo.m_spawnSquare = closestUnoccupiedSquare;
			hitInfo.m_didSpawnSatellite = true;
			hitInfo.m_hitReceived = false;
			m_targetToHitInfo.Add(nextTarget, hitInfo);
			m_numTempSatellitesSpawned++;
			return;
		}
	}

	private ActorData GetTargetFromTempSatellite(GameObject tempSatellite)
	{
		ActorData result = null;
		foreach (KeyValuePair<ActorData, HitInfo> item in m_targetToHitInfo)
		{
			if (item.Value.m_spawnedTempSatellite == tempSatellite)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return item.Key;
					}
				}
			}
		}
		return result;
	}

	private void SpawnHitFX(ActorData target)
	{
		HitInfo value = null;
		if (!m_targetToHitInfo.TryGetValue(target, out value))
		{
			return;
		}
		while (true)
		{
			value.m_hitReceived = true;
			m_hitFxJoint.Initialize(target.gameObject);
			GameObject item = InstantiateFX(m_fxHitPrefab, m_hitFxJoint.m_jointObject.transform.position, Quaternion.identity);
			m_hitFXInstances.Add(item);
			Vector3 position = m_hitFxJoint.m_jointObject.transform.position;
			Vector3 hitDirection = position - value.m_spawnSquare.ToVector3();
			hitDirection.Normalize();
			ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, hitDirection);
			base.Source.OnSequenceHit(this, target, impulseInfo);
			return;
		}
	}

	private void SpawnHitFX(GameObject sourceObject)
	{
		ActorData targetFromTempSatellite = GetTargetFromTempSatellite(sourceObject);
		if (!(targetFromTempSatellite != null))
		{
			return;
		}
		while (true)
		{
			SpawnHitFX(targetFromTempSatellite);
			return;
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (m_startEvent == parameter)
		{
			SpawnTempSatellite();
		}
		else
		{
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
