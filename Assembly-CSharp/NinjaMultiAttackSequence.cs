using System;
using System.Collections.Generic;
using UnityEngine;

public class NinjaMultiAttackSequence : Sequence
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

	public float m_attackDelay = 0.1f;

	public GameObject m_tempSatellitePrefab;

	private Dictionary<ActorData, int> m_actorsToHits;

	private List<GameObject> m_hitFXInstances;

	private float m_lastTempSatelliteSpawnTime = -1f;

	private List<NinjaMultiAttackSequence.HitInfo> m_hitInfos;

	public override void FinishSetup()
	{
		if (this.m_startEvent == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.FinishSetup()).MethodHandle;
			}
			this.SetupHitInfoList();
			this.SpawnTempSatellite();
		}
	}

	private bool Finished()
	{
		bool result = false;
		if (this.m_hitInfos != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.Finished()).MethodHandle;
			}
			result = true;
			for (int i = 0; i < this.m_hitInfos.Count; i++)
			{
				NinjaMultiAttackSequence.HitInfo hitInfo = this.m_hitInfos[i];
				if (hitInfo.m_numHits > hitInfo.m_numHitsReceived)
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
					return false;
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
		return result;
	}

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			NinjaMultiAttackSequence.ExtraParams extraParams2 = extraSequenceParams as NinjaMultiAttackSequence.ExtraParams;
			if (extraParams2 != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				this.m_actorsToHits = extraParams2.actorToHits;
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

	private bool SpawnedAllTempSatellites()
	{
		bool result = true;
		for (int i = 0; i < this.m_hitInfos.Count; i++)
		{
			NinjaMultiAttackSequence.HitInfo hitInfo = this.m_hitInfos[i];
			if (hitInfo.m_numHitsReceived < hitInfo.m_numHits)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.SpawnedAllTempSatellites()).MethodHandle;
				}
				result = false;
				return result;
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			return result;
		}
	}

	private void Update()
	{
		if (this.m_initialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.Update()).MethodHandle;
			}
			if (this.m_lastTempSatelliteSpawnTime > 0f && GameTime.time > this.m_lastTempSatelliteSpawnTime + this.m_spawnDelay && !this.SpawnedAllTempSatellites())
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
				this.SpawnTempSatellite();
			}
			for (int i = 0; i < this.m_hitInfos.Count; i++)
			{
				if (this.m_hitInfos[i].m_didSpawnSatellite)
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
					if (this.m_hitInfos[i].m_spawnedSatellite == null)
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
						int num = this.m_hitInfos[i].m_numHits - this.m_hitInfos[i].m_numHitsReceived;
						for (int j = 0; j < num; j++)
						{
							this.SpawnHitFX(this.m_hitInfos[i]);
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
			}
		}
	}

	private bool SquareInUseByTempSatellite(BoardSquare square)
	{
		bool result = false;
		for (int i = 0; i < this.m_hitInfos.Count; i++)
		{
			if (this.m_hitInfos[i].m_boardSquare == square)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.SquareInUseByTempSatellite(BoardSquare)).MethodHandle;
				}
				if (this.m_hitInfos[i].m_spawnedSatellite != null)
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
					result = true;
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
		return result;
	}

	private BoardSquare GetClosestUnoccupiedSquare(BoardSquare center)
	{
		BoardSquare boardSquare = null;
		int i = 1;
		IL_D7:
		while (i < 3)
		{
			int j = -i;
			while (j <= i)
			{
				BoardSquare boardSquare2 = Board.Get().GetBoardSquare(center.x + j, center.y);
				if (this.CanUseSquareForTempSatellite(boardSquare2))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.GetClosestUnoccupiedSquare(BoardSquare)).MethodHandle;
					}
					boardSquare = boardSquare2;
					IL_65:
					int k = -i;
					while (k <= i)
					{
						BoardSquare boardSquare3 = Board.Get().GetBoardSquare(center.x, center.y + k);
						if (this.CanUseSquareForTempSatellite(boardSquare3))
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
							boardSquare = boardSquare3;
							IL_BC:
							if (boardSquare != null)
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
								goto IL_DE;
							}
							i++;
							goto IL_D7;
						}
						else
						{
							k += i * 2;
						}
					}
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						goto IL_BC;
					}
				}
				else
				{
					j += i * 2;
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				goto IL_65;
			}
		}
		IL_DE:
		if (boardSquare == null)
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
			boardSquare = center;
		}
		return boardSquare;
	}

	private bool CanUseSquareForTempSatellite(BoardSquare square)
	{
		if (square != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.CanUseSquareForTempSatellite(BoardSquare)).MethodHandle;
			}
			if (square.IsBaselineHeight() && square.occupant == null)
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
				return !this.SquareInUseByTempSatellite(square);
			}
		}
		return false;
	}

	private NinjaMultiAttackSequence.HitInfo GetNextTarget()
	{
		NinjaMultiAttackSequence.HitInfo result = null;
		for (int i = 0; i < this.m_hitInfos.Count; i++)
		{
			NinjaMultiAttackSequence.HitInfo hitInfo = this.m_hitInfos[i];
			if (!hitInfo.m_didSpawnSatellite)
			{
				result = hitInfo;
				return result;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.GetNextTarget()).MethodHandle;
			return result;
		}
		return result;
	}

	private void SpawnTempSatellite()
	{
		this.m_lastTempSatelliteSpawnTime = GameTime.time;
		NinjaMultiAttackSequence.HitInfo nextTarget = this.GetNextTarget();
		if (nextTarget != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.SpawnTempSatellite()).MethodHandle;
			}
			BoardSquare closestUnoccupiedSquare = this.GetClosestUnoccupiedSquare(nextTarget.m_target.GetCurrentBoardSquare());
			Vector3 forward = Vector3.forward;
			if (closestUnoccupiedSquare != nextTarget.m_target.GetCurrentBoardSquare())
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
				forward = nextTarget.m_target.GetCurrentBoardSquare().ToVector3() - closestUnoccupiedSquare.ToVector3();
			}
			GameObject gameObject = base.InstantiateFX(this.m_tempSatellitePrefab, closestUnoccupiedSquare.ToVector3(), Quaternion.LookRotation(forward), true, true);
			gameObject.GetComponent<NinjaCloneSatellite>().Setup(this);
			gameObject.GetComponent<NinjaCloneSatellite>().TriggerMultiAttack(nextTarget.m_target.gameObject, nextTarget.m_numHits, this.m_attackDelay);
			nextTarget.m_spawnedSatellite = gameObject;
			nextTarget.m_spawnTime = GameTime.time;
			nextTarget.m_boardSquare = closestUnoccupiedSquare;
			nextTarget.m_didSpawnSatellite = true;
		}
	}

	private void SetupHitInfoList()
	{
		this.m_hitFXInstances = new List<GameObject>();
		this.m_hitInfos = new List<NinjaMultiAttackSequence.HitInfo>();
		using (Dictionary<ActorData, int>.Enumerator enumerator = this.m_actorsToHits.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, int> keyValuePair = enumerator.Current;
				NinjaMultiAttackSequence.HitInfo hitInfo = new NinjaMultiAttackSequence.HitInfo();
				hitInfo.m_numHits = keyValuePair.Value;
				hitInfo.m_target = keyValuePair.Key;
				hitInfo.m_spawnedSatellite = null;
				hitInfo.m_didSpawnSatellite = false;
				hitInfo.m_numHitsReceived = 0;
				JointPopupProperty jointPopupProperty = new JointPopupProperty();
				jointPopupProperty.m_joint = this.m_hitFxJoint.m_joint;
				jointPopupProperty.m_jointCharacter = this.m_hitFxJoint.m_jointCharacter;
				jointPopupProperty.Initialize(hitInfo.m_target.gameObject);
				hitInfo.m_hitJoint = jointPopupProperty;
				this.m_hitInfos.Add(hitInfo);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.SetupHitInfoList()).MethodHandle;
			}
		}
	}

	private NinjaMultiAttackSequence.HitInfo GetHitInfoFromTempSatellite(GameObject tempSatellite)
	{
		NinjaMultiAttackSequence.HitInfo result = null;
		for (int i = 0; i < this.m_hitInfos.Count; i++)
		{
			NinjaMultiAttackSequence.HitInfo hitInfo = this.m_hitInfos[i];
			if (hitInfo.m_spawnedSatellite == tempSatellite)
			{
				result = hitInfo;
				return result;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.GetHitInfoFromTempSatellite(GameObject)).MethodHandle;
			return result;
		}
		return result;
	}

	private void SpawnHitFX(NinjaMultiAttackSequence.HitInfo hitInfo)
	{
		hitInfo.m_numHitsReceived++;
		if (this.m_fxHitPrefab != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.SpawnHitFX(NinjaMultiAttackSequence.HitInfo)).MethodHandle;
			}
			GameObject item = base.InstantiateFX(this.m_fxHitPrefab, hitInfo.m_hitJoint.m_jointObject.transform.position, Quaternion.identity, true, true);
			this.m_hitFXInstances.Add(item);
		}
		Vector3 position = hitInfo.m_hitJoint.m_jointObject.transform.position;
		Vector3 hitDirection = position - hitInfo.m_boardSquare.ToVector3();
		hitDirection.Normalize();
		ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(position, hitDirection);
		if (hitInfo.m_numHits > hitInfo.m_numHitsReceived)
		{
			base.Source.OnSequenceHit(this, hitInfo.m_target, impulseInfo, ActorModelData.RagdollActivation.None, true);
		}
		else
		{
			base.Source.OnSequenceHit(this, hitInfo.m_target, impulseInfo, ActorModelData.RagdollActivation.HealthBased, true);
		}
	}

	private void SpawnHitFX(GameObject sourceObject)
	{
		NinjaMultiAttackSequence.HitInfo hitInfoFromTempSatellite = this.GetHitInfoFromTempSatellite(sourceObject);
		if (hitInfoFromTempSatellite != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.SpawnHitFX(GameObject)).MethodHandle;
			}
			this.SpawnHitFX(hitInfoFromTempSatellite);
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.SetupHitInfoList();
			this.SpawnTempSatellite();
		}
		else if (this.m_hitEvent == parameter)
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
			this.SpawnHitFX(sourceObject);
		}
	}

	private void OnDisable()
	{
		if (this.m_hitFXInstances != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.OnDisable()).MethodHandle;
			}
			for (int i = 0; i < this.m_hitFXInstances.Count; i++)
			{
				UnityEngine.Object.Destroy(this.m_hitFXInstances[i]);
			}
			this.m_hitFXInstances = null;
		}
		this.m_initialized = false;
	}

	public class ExtraParams : Sequence.IExtraSequenceParams
	{
		public Dictionary<ActorData, int> actorToHits;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			int num;
			if (this.actorToHits != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.ExtraParams.XSP_SerializeToStream(IBitStream)).MethodHandle;
				}
				num = this.actorToHits.Count;
			}
			else
			{
				num = 0;
			}
			int num2 = num;
			stream.Serialize(ref num2);
			foreach (KeyValuePair<ActorData, int> keyValuePair in this.actorToHits)
			{
				ActorData key = keyValuePair.Key;
				int num3;
				if (key != null)
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
					num3 = key.ActorIndex;
				}
				else
				{
					num3 = ActorData.s_invalidActorIndex;
				}
				int num4 = num3;
				int value = keyValuePair.Value;
				stream.Serialize(ref num4);
				stream.Serialize(ref value);
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			int num = 0;
			stream.Serialize(ref num);
			this.actorToHits = new Dictionary<ActorData, int>(num);
			for (int i = 0; i < num; i++)
			{
				int s_invalidActorIndex = ActorData.s_invalidActorIndex;
				int value = 0;
				stream.Serialize(ref s_invalidActorIndex);
				stream.Serialize(ref value);
				ActorData key = GameFlowData.Get().FindActorByActorIndex(s_invalidActorIndex);
				this.actorToHits.Add(key, value);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaMultiAttackSequence.ExtraParams.XSP_DeserializeFromStream(IBitStream)).MethodHandle;
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
}
