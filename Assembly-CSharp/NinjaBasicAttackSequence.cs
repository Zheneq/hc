using System;
using System.Collections.Generic;
using UnityEngine;

public class NinjaBasicAttackSequence : Sequence
{
	[AnimEventPicker]
	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	public UnityEngine.Object m_startEvent;

	[Tooltip("Animation event (if any) to wait for playing a hitreact. Search project for EventObjects.")]
	[AnimEventPicker]
	public UnityEngine.Object m_hitEvent;

	[Tooltip("Main FX prefab.")]
	public GameObject m_fxHitPrefab;

	[JointPopup("hit FX attach joint")]
	public JointPopupProperty m_hitFxJoint;

	public float m_spawnDelay = 0.5f;

	public float m_attackDelay = 0.25f;

	public GameObject m_tempSatellitePrefab;

	private Dictionary<ActorData, NinjaBasicAttackSequence.HitInfo> m_targetToHitInfo;

	private List<GameObject> m_hitFXInstances;

	private float m_lastTempSatelliteSpawnTime = -1f;

	private int m_numTempSatellitesSpawned;

	private bool m_setFinishTrigger;

	public int m_numAttacksForSingleTarget = 7;

	public int m_numAttacksForMultiTarget = 3;

	public override void FinishSetup()
	{
		this.m_hitFXInstances = new List<GameObject>();
		this.m_targetToHitInfo = new Dictionary<ActorData, NinjaBasicAttackSequence.HitInfo>();
		if (this.m_startEvent == null)
		{
			this.SpawnTempSatellite();
		}
	}

	private void Update()
	{
		if (this.m_initialized)
		{
			if (this.m_lastTempSatelliteSpawnTime > 0f)
			{
				if (GameTime.time > this.m_lastTempSatelliteSpawnTime + this.m_spawnDelay)
				{
					if (this.m_numTempSatellitesSpawned < base.Targets.Length)
					{
						this.SpawnTempSatellite();
					}
				}
			}
			using (Dictionary<ActorData, NinjaBasicAttackSequence.HitInfo>.Enumerator enumerator = this.m_targetToHitInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ActorData, NinjaBasicAttackSequence.HitInfo> keyValuePair = enumerator.Current;
					NinjaBasicAttackSequence.HitInfo value = keyValuePair.Value;
					if (value.m_didSpawnSatellite)
					{
						if (value.m_spawnedTempSatellite == null)
						{
							if (!value.m_hitReceived)
							{
								this.SpawnHitFX(keyValuePair.Key);
							}
						}
					}
				}
			}
			if (this.AllTempSatellitesDespawned())
			{
				if (!this.m_setFinishTrigger)
				{
					this.m_setFinishTrigger = true;
					base.Caster.GetActorModelData().GetModelAnimator().SetTrigger("FinishAttack");
				}
			}
		}
	}

	private bool AllTempSatellitesDespawned()
	{
		bool result = true;
		if (this.m_numTempSatellitesSpawned != base.Targets.Length)
		{
			result = false;
		}
		else
		{
			foreach (KeyValuePair<ActorData, NinjaBasicAttackSequence.HitInfo> keyValuePair in this.m_targetToHitInfo)
			{
				if (keyValuePair.Value.m_didSpawnSatellite)
				{
					if (!(keyValuePair.Value.m_spawnedTempSatellite != null))
					{
						continue;
					}
					if (keyValuePair.Value.m_spawnedTempSatellite.GetComponent<NinjaCloneSatellite>().IsDespawning())
					{
						continue;
					}
				}
				result = false;
			}
		}
		return result;
	}

	private bool SquareInUseByTempSatellite(BoardSquare square)
	{
		bool result = false;
		using (Dictionary<ActorData, NinjaBasicAttackSequence.HitInfo>.Enumerator enumerator = this.m_targetToHitInfo.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, NinjaBasicAttackSequence.HitInfo> keyValuePair = enumerator.Current;
				NinjaBasicAttackSequence.HitInfo value = keyValuePair.Value;
				if (value.m_spawnSquare == square && value.m_spawnedTempSatellite != null)
				{
					result = true;
				}
			}
		}
		return result;
	}

	private BoardSquare GetClosestUnoccupiedSquare(BoardSquare center)
	{
		BoardSquare boardSquare = null;
		for (int i = 1; i < 3; i++)
		{
			int j = -i;
			while (j <= i)
			{
				BoardSquare boardSquare2 = Board.Get().GetBoardSquare(center.x + j, center.y);
				if (boardSquare2.occupant == null && !this.SquareInUseByTempSatellite(boardSquare2))
				{
					boardSquare = boardSquare2;
					break;
				}
				else
				{
					j += i * 2;
				}
			}
			for (int k = -i; k <= i; k += i * 2)
			{
				BoardSquare boardSquare3 = Board.Get().GetBoardSquare(center.x, center.y + k);
				if (boardSquare3.occupant == null && !this.SquareInUseByTempSatellite(boardSquare3))
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
		if (this.m_numTempSatellitesSpawned < base.Targets.Length)
		{
			result = base.Targets[this.m_numTempSatellitesSpawned];
		}
		return result;
	}

	private void SpawnTempSatellite()
	{
		this.m_lastTempSatelliteSpawnTime = GameTime.time;
		ActorData nextTarget = this.GetNextTarget();
		if (nextTarget != null)
		{
			BoardSquare closestUnoccupiedSquare = this.GetClosestUnoccupiedSquare(nextTarget.GetCurrentBoardSquare());
			Vector3 forward = Vector3.forward;
			if (closestUnoccupiedSquare != nextTarget.GetCurrentBoardSquare())
			{
				forward = nextTarget.GetCurrentBoardSquare().ToVector3() - closestUnoccupiedSquare.ToVector3();
			}
			GameObject gameObject = base.InstantiateFX(this.m_tempSatellitePrefab, closestUnoccupiedSquare.ToVector3(), Quaternion.LookRotation(forward), true, true);
			gameObject.GetComponent<NinjaCloneSatellite>().Setup(this);
			if (base.Targets.Length == 1)
			{
				gameObject.GetComponent<NinjaCloneSatellite>().TriggerMultiAttack(nextTarget.gameObject, this.m_numAttacksForSingleTarget, this.m_attackDelay);
			}
			else
			{
				gameObject.GetComponent<NinjaCloneSatellite>().TriggerMultiAttack(nextTarget.gameObject, this.m_numAttacksForMultiTarget, this.m_attackDelay);
			}
			NinjaBasicAttackSequence.HitInfo hitInfo = new NinjaBasicAttackSequence.HitInfo();
			hitInfo.m_spawnedTempSatellite = gameObject;
			hitInfo.m_spawnSquare = closestUnoccupiedSquare;
			hitInfo.m_didSpawnSatellite = true;
			hitInfo.m_hitReceived = false;
			this.m_targetToHitInfo.Add(nextTarget, hitInfo);
			this.m_numTempSatellitesSpawned++;
		}
	}

	private ActorData GetTargetFromTempSatellite(GameObject tempSatellite)
	{
		ActorData result = null;
		foreach (KeyValuePair<ActorData, NinjaBasicAttackSequence.HitInfo> keyValuePair in this.m_targetToHitInfo)
		{
			if (keyValuePair.Value.m_spawnedTempSatellite == tempSatellite)
			{
				result = keyValuePair.Key;
				break;
			}
		}
		return result;
	}

	private void SpawnHitFX(ActorData target)
	{
		NinjaBasicAttackSequence.HitInfo hitInfo = null;
		if (this.m_targetToHitInfo.TryGetValue(target, out hitInfo))
		{
			hitInfo.m_hitReceived = true;
			this.m_hitFxJoint.Initialize(target.gameObject);
			GameObject item = base.InstantiateFX(this.m_fxHitPrefab, this.m_hitFxJoint.m_jointObject.transform.position, Quaternion.identity, true, true);
			this.m_hitFXInstances.Add(item);
			Vector3 position = this.m_hitFxJoint.m_jointObject.transform.position;
			Vector3 hitDirection = position - hitInfo.m_spawnSquare.ToVector3();
			hitDirection.Normalize();
			ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, hitDirection);
			base.Source.OnSequenceHit(this, target, impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
		}
	}

	private void SpawnHitFX(GameObject sourceObject)
	{
		ActorData targetFromTempSatellite = this.GetTargetFromTempSatellite(sourceObject);
		if (targetFromTempSatellite != null)
		{
			this.SpawnHitFX(targetFromTempSatellite);
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
		{
			this.SpawnTempSatellite();
		}
		else if (this.m_hitEvent == parameter)
		{
			this.SpawnHitFX(sourceObject);
		}
	}

	private void OnDisable()
	{
		if (this.m_hitFXInstances != null)
		{
			for (int i = 0; i < this.m_hitFXInstances.Count; i++)
			{
				UnityEngine.Object.Destroy(this.m_hitFXInstances[i]);
			}
			this.m_hitFXInstances = null;
		}
		this.m_initialized = false;
	}

	public class HitInfo
	{
		public GameObject m_spawnedTempSatellite;

		public bool m_hitReceived;

		public bool m_didSpawnSatellite;

		public BoardSquare m_spawnSquare;
	}
}
