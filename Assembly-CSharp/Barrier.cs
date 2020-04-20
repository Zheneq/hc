using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Barrier
{
	private string m_name;

	private Vector3 m_center;

	private Vector3 m_endpoint1;

	private Vector3 m_endpoint2;

	private Vector3 m_facingDir;

	private bool m_bidirectional;

	private bool m_makeClientGeo;

	private GameObject m_generatedClientGeometry;

	private Team m_team;

	private ActorData m_owner;

	public SpoilsSpawnData m_spoilsSpawnOnEnemyMovedThrough;

	public SpoilsSpawnData m_spoilsSpawnOnAllyMovedThrough;

	public bool m_removeAtTurnEndIfEnemyMovedThrough;

	public bool m_removeAtTurnEndIfAllyMovedThrough;

	public bool m_removeAtPhaseEndIfEnemyMovedThrough;

	public bool m_removeAtPhaseEndIfAllyMovedThrough;

	public AbilityPriority m_customEndPhase = AbilityPriority.INVALID;

	public bool m_removeAtPhaseEndIfCasterKnockedBack;

	private int m_maxHits;

	public EffectDuration m_time;

	public int m_guid;

	public List<Sequence> m_barrierSequences;

	private List<GameObject> m_barrierSequencePrefabs;

	private bool m_playSequences;

	private bool m_considerAsCover;

	public Barrier(int guid, string name, Vector3 center, Vector3 facingDir, float width, bool bidirectional, BlockingRules blocksVision, BlockingRules blocksAbilities, BlockingRules blocksMovement, BlockingRules blocksPositionTargeting, bool considerAsCover, int maxDuration, ActorData owner, List<GameObject> barrierSequencePrefabs = null, bool playSequences = true, GameplayResponseForActor onEnemyMovedThrough = null, GameplayResponseForActor onAllyMovedThrough = null, int maxHits = -1, bool endOnCasterDeath = false, SequenceSource parentSequenceSource = null, Team barrierTeam = Team.Invalid)
	{
		this.InitBarrier(guid, name, center, facingDir, width, bidirectional, blocksVision, blocksAbilities, blocksMovement, BlockingRules.ForNobody, blocksPositionTargeting, considerAsCover, maxDuration, owner, barrierSequencePrefabs, playSequences, onEnemyMovedThrough, onAllyMovedThrough, maxHits, endOnCasterDeath, parentSequenceSource, barrierTeam);
	}

	public string Name
	{
		get
		{
			return this.m_name;
		}
		private set
		{
			this.m_name = value;
		}
	}

	public ActorData Caster
	{
		get
		{
			return this.m_owner;
		}
		private set
		{
			this.m_owner = value;
		}
	}

	public Vector3 GetCenterPos()
	{
		return this.m_center;
	}

	public Vector3 GetEndPos1()
	{
		return this.m_endpoint1;
	}

	public Vector3 GetEndPos2()
	{
		return this.m_endpoint2;
	}

	public Team GetBarrierTeam()
	{
		return this.m_team;
	}

	private bool UnlimitedHits()
	{
		return this.m_maxHits < 0;
	}

	public SequenceSource BarrierSequenceSource { get; protected set; }

	public BlockingRules BlocksVision { get; private set; }

	public BlockingRules BlocksAbilities { get; private set; }

	public BlockingRules BlocksMovement { get; private set; }

	public BlockingRules BlocksMovementOnCrossover { get; private set; }

	public BlockingRules BlocksPositionTargeting { get; private set; }

	public bool ConsiderAsCover
	{
		get
		{
			return this.m_considerAsCover;
		}
		set
		{
			this.m_considerAsCover = value;
		}
	}

	private void InitBarrier(int guid, string name, Vector3 center, Vector3 facingDir, float width, bool bidirectional, BlockingRules blocksVision, BlockingRules blocksAbilities, BlockingRules blocksMovement, BlockingRules blocksMovementOnCrossover, BlockingRules blocksPositionTargeting, bool considerAsCover, int maxDuration, ActorData owner, List<GameObject> barrierSequencePrefabs, bool playSequences, GameplayResponseForActor onEnemyMovedThrough, GameplayResponseForActor onAllyMovedThrough, int maxHits, bool endOnCasterDeath, SequenceSource parentSequenceSource, Team barrierTeam)
	{
		this.m_guid = guid;
		this.m_name = name;
		this.m_center = center;
		this.m_facingDir = facingDir;
		this.m_bidirectional = bidirectional;
		Vector3 a = Vector3.Cross(facingDir, Vector3.up);
		a.Normalize();
		float d = width * Board.Get().squareSize;
		this.m_endpoint1 = center + a * d / 2f;
		this.m_endpoint2 = center - a * d / 2f;
		this.BlocksVision = blocksVision;
		this.BlocksAbilities = blocksAbilities;
		this.BlocksMovement = blocksMovement;
		this.BlocksMovementOnCrossover = blocksMovementOnCrossover;
		this.BlocksPositionTargeting = blocksPositionTargeting;
		this.m_considerAsCover = considerAsCover;
		this.m_owner = owner;
		if (this.m_owner != null)
		{
			this.m_team = this.m_owner.GetTeam();
		}
		else
		{
			this.m_team = barrierTeam;
		}
		this.m_time = new EffectDuration();
		this.m_time.duration = maxDuration;
		this.m_barrierSequencePrefabs = barrierSequencePrefabs;
		bool playSequences2;
		if (playSequences)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Barrier.InitBarrier(int, string, Vector3, Vector3, float, bool, BlockingRules, BlockingRules, BlockingRules, BlockingRules, BlockingRules, bool, int, ActorData, List<GameObject>, bool, GameplayResponseForActor, GameplayResponseForActor, int, bool, SequenceSource, Team)).MethodHandle;
			}
			playSequences2 = (this.m_barrierSequencePrefabs != null);
		}
		else
		{
			playSequences2 = false;
		}
		this.m_playSequences = playSequences2;
		this.m_barrierSequences = new List<Sequence>();
		if (this.m_playSequences)
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
			this.BarrierSequenceSource = new SequenceSource(null, null, false, parentSequenceSource, null);
		}
		this.m_maxHits = maxHits;
	}

	public bool CanBeSeenThroughBy(ActorData viewer)
	{
		bool flag = this.IsBlocked(viewer, this.BlocksVision);
		return !flag;
	}

	public bool CanBeShotThroughBy(ActorData shooter)
	{
		if (BarrierManager.Get().SuppressingAbilityBlocks())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Barrier.CanBeShotThroughBy(ActorData)).MethodHandle;
			}
			return true;
		}
		bool flag = this.IsBlocked(shooter, this.BlocksAbilities);
		return !flag;
	}

	public bool CanBeMovedThroughBy(ActorData mover)
	{
		bool flag = this.IsBlocked(mover, this.BlocksMovement);
		return !flag;
	}

	public bool CanMoveThroughAfterCrossoverBy(ActorData mover)
	{
		bool flag = this.IsBlocked(mover, this.BlocksMovementOnCrossover);
		return !flag;
	}

	public bool IsPositionTargetingBlockedFor(ActorData caster)
	{
		return this.IsBlocked(caster, this.BlocksPositionTargeting);
	}

	private bool IsBlocked(ActorData actor, BlockingRules rules)
	{
		bool result;
		switch (rules)
		{
		case BlockingRules.ForNobody:
			result = false;
			break;
		case BlockingRules.ForEnemies:
			result = (actor == null || actor.GetTeam() != this.m_team);
			break;
		case BlockingRules.ForEverybody:
			result = true;
			break;
		default:
			result = false;
			break;
		}
		return result;
	}

	public unsafe virtual void OnStart(bool delayVisionUpdate, out List<ActorData> visionUpdaters)
	{
		visionUpdaters = new List<ActorData>();
		if (NetworkClient.active && this.m_makeClientGeo)
		{
			float squareSize = Board.Get().squareSize;
			Vector3 a = this.m_endpoint2 - this.m_endpoint1;
			bool flag = Mathf.Abs(a.z) > Mathf.Abs(a.x);
			Vector3 vector = this.m_endpoint1 + 0.5f * a;
			this.m_generatedClientGeometry = GameObject.CreatePrimitive(PrimitiveType.Cube);
			this.m_generatedClientGeometry.transform.position = new Vector3(vector.x, 1.5f * squareSize, vector.z);
			if (flag)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Barrier.OnStart(bool, List<ActorData>*)).MethodHandle;
				}
				this.m_generatedClientGeometry.transform.localScale = new Vector3(0.25f, 2f * squareSize, a.magnitude);
			}
			else
			{
				this.m_generatedClientGeometry.transform.localScale = new Vector3(a.magnitude, 2f * squareSize, 0.25f);
			}
		}
	}

	public virtual void OnEnd()
	{
		if (NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Barrier.OnEnd()).MethodHandle;
			}
			using (List<Sequence>.Enumerator enumerator = this.m_barrierSequences.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Sequence sequence = enumerator.Current;
					if (sequence != null)
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
						sequence.MarkForRemoval();
					}
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
		if (NetworkClient.active)
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
			if (this.m_makeClientGeo)
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
				if (this.m_generatedClientGeometry != null)
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
					UnityEngine.Object.DestroyObject(this.m_generatedClientGeometry);
				}
				this.m_generatedClientGeometry = null;
			}
		}
	}

	public bool CanAffectVision()
	{
		bool result;
		if (this.BlocksVision != BlockingRules.ForEnemies)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Barrier.CanAffectVision()).MethodHandle;
			}
			result = (this.BlocksVision == BlockingRules.ForEverybody);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool CanAffectMovement()
	{
		bool result;
		if (this.BlocksMovement != BlockingRules.ForEnemies)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Barrier.CanAffectMovement()).MethodHandle;
			}
			result = (this.BlocksMovement == BlockingRules.ForEverybody);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool CrossingBarrier(Vector3 src, Vector3 dest)
	{
		bool flag = VectorUtils.IsPointInLaser(src, this.m_endpoint1, this.m_endpoint2, 0.001f);
		bool flag2 = VectorUtils.IsPointInLaser(dest, this.m_endpoint1, this.m_endpoint2, 0.001f);
		bool result;
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Barrier.CrossingBarrier(Vector3, Vector3)).MethodHandle;
			}
			result = false;
		}
		else
		{
			if (!flag2)
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
				if (VectorUtils.OnSameSideOfLine(src, dest, this.m_endpoint1, this.m_endpoint2))
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
					return false;
				}
			}
			if (!flag2)
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
				if (VectorUtils.OnSameSideOfLine(this.m_endpoint1, this.m_endpoint2, src, dest))
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
					return false;
				}
			}
			if (this.m_bidirectional)
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
				result = true;
			}
			else
			{
				Vector3 lhs = src - this.m_center;
				float num = Vector3.Dot(lhs, this.m_facingDir);
				if (num > 0f)
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
				else
				{
					result = false;
				}
			}
		}
		return result;
	}

	public bool CrossingBarrierForVision(Vector3 src, Vector3 dest)
	{
		return this.SegmentsIntersectForVision(src, dest, this.m_endpoint1, this.m_endpoint2);
	}

	private bool SegmentsIntersectForVision(Vector3 startA, Vector3 endA, Vector3 startB, Vector3 endB)
	{
		return Barrier.PointsAreCounterClockwise(startA, startB, endB) != Barrier.PointsAreCounterClockwise(endA, startB, endB) && Barrier.PointsAreCounterClockwise(startA, endA, startB) != Barrier.PointsAreCounterClockwise(startA, endA, endB);
	}

	private static bool PointsAreCounterClockwise(Vector3 a, Vector3 b, Vector3 c)
	{
		return (c.z - a.z) * (b.x - a.x) > (b.z - a.z) * (c.x - a.x);
	}

	public Vector3 GetIntersectionPoint(Vector3 src, Vector3 dest)
	{
		Vector3 vector = dest - src;
		Vector3 directionOfSecond = this.m_endpoint2 - this.m_endpoint1;
		bool flag;
		Vector3 vector2 = VectorUtils.GetLineLineIntersection(src, vector, this.m_endpoint1, directionOfSecond, out flag);
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Barrier.GetIntersectionPoint(Vector3, Vector3)).MethodHandle;
			}
			vector2.y = src.y;
			Vector3 normalized = (-vector).normalized;
			vector2 += normalized * 0.05f;
		}
		return vector2;
	}

	public Vector3 GetCollisionNormal(Vector3 incomingDir)
	{
		if (this.m_bidirectional)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Barrier.GetCollisionNormal(Vector3)).MethodHandle;
			}
			if (Vector3.Dot(incomingDir, this.m_facingDir) > 0f)
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
				return -this.m_facingDir;
			}
		}
		return this.m_facingDir;
	}

	public Vector3 GetFacingDir()
	{
		return this.m_facingDir;
	}

	internal static BarrierSerializeInfo BarrierToSerializeInfo(Barrier barrier)
	{
		BarrierSerializeInfo barrierSerializeInfo = new BarrierSerializeInfo();
		barrierSerializeInfo.m_guid = barrier.m_guid;
		barrierSerializeInfo.m_center = barrier.m_center;
		barrierSerializeInfo.m_widthInWorld = (barrier.m_endpoint1 - barrier.m_endpoint2).magnitude;
		barrierSerializeInfo.m_facingHorizontalAngle = VectorUtils.HorizontalAngle_Deg(barrier.m_facingDir);
		barrierSerializeInfo.m_bidirectional = barrier.m_bidirectional;
		barrierSerializeInfo.m_blocksVision = (sbyte)barrier.BlocksVision;
		barrierSerializeInfo.m_blocksAbilities = (sbyte)barrier.BlocksAbilities;
		barrierSerializeInfo.m_blocksMovement = (sbyte)barrier.BlocksMovement;
		barrierSerializeInfo.m_blocksMovementOnCrossover = (sbyte)barrier.BlocksMovementOnCrossover;
		barrierSerializeInfo.m_blocksPositionTargeting = (sbyte)barrier.BlocksPositionTargeting;
		barrierSerializeInfo.m_considerAsCover = barrier.m_considerAsCover;
		barrierSerializeInfo.m_team = (sbyte)barrier.m_team;
		int ownerIndex = ActorData.s_invalidActorIndex;
		if (barrier.m_owner != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Barrier.BarrierToSerializeInfo(Barrier)).MethodHandle;
			}
			ownerIndex = barrier.m_owner.ActorIndex;
		}
		barrierSerializeInfo.m_ownerIndex = ownerIndex;
		barrierSerializeInfo.m_makeClientGeo = barrier.m_makeClientGeo;
		return barrierSerializeInfo;
	}

	internal static Barrier CreateBarrierFromSerializeInfo(BarrierSerializeInfo info)
	{
		BlockingRules blocksVision = (BlockingRules)info.m_blocksVision;
		BlockingRules blocksAbilities = (BlockingRules)info.m_blocksAbilities;
		BlockingRules blocksMovement = (BlockingRules)info.m_blocksMovement;
		BlockingRules blocksMovementOnCrossover = (BlockingRules)info.m_blocksMovementOnCrossover;
		BlockingRules blocksPositionTargeting = (BlockingRules)info.m_blocksPositionTargeting;
		ActorData owner = null;
		if (info.m_ownerIndex != ActorData.s_invalidActorIndex)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Barrier.CreateBarrierFromSerializeInfo(BarrierSerializeInfo)).MethodHandle;
			}
			owner = GameFlowData.Get().FindActorByActorIndex(info.m_ownerIndex);
		}
		Vector3 facingDir = VectorUtils.AngleDegreesToVector(info.m_facingHorizontalAngle);
		float width = info.m_widthInWorld / Board.Get().squareSize;
		return new Barrier(info.m_guid, string.Empty, info.m_center, facingDir, width, info.m_bidirectional, blocksVision, blocksAbilities, blocksMovement, blocksPositionTargeting, info.m_considerAsCover, -1, owner, null, true, null, null, -1, false, null, Team.Invalid)
		{
			BlocksMovementOnCrossover = blocksMovementOnCrossover,
			m_makeClientGeo = info.m_makeClientGeo
		};
	}

	public virtual List<ServerClientUtils.SequenceStartData> GetSequenceStartDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (this.m_barrierSequencePrefabs != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Barrier.GetSequenceStartDataList()).MethodHandle;
			}
			if (this.m_playSequences)
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
				Quaternion targetRotation = Quaternion.LookRotation(this.m_facingDir);
				ActorData[] targetActorArray = new ActorData[0];
				using (List<GameObject>.Enumerator enumerator = this.m_barrierSequencePrefabs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject gameObject = enumerator.Current;
						if (gameObject != null)
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
							Sequence[] components = gameObject.GetComponents<Sequence>();
							bool flag = false;
							Sequence[] array = components;
							int i = 0;
							while (i < array.Length)
							{
								Sequence sequence = array[i];
								if (!(sequence is OverwatchScanSequence))
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
									if (!(sequence is GroundLineSequence))
									{
										if (!(sequence is ExoLaserHittingWallSequence))
										{
											i++;
											continue;
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
								flag = true;
								break;
							}
							Sequence.IExtraSequenceParams[] extraParams = null;
							if (flag)
							{
								extraParams = new Sequence.IExtraSequenceParams[]
								{
									new GroundLineSequence.ExtraParams
									{
										startPos = this.m_endpoint2,
										endPos = this.m_endpoint1
									}
								};
							}
							ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(gameObject, null, targetRotation, targetActorArray, this.m_owner, this.BarrierSequenceSource, extraParams);
							list.Add(item);
						}
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
		}
		return list;
	}

	public void DrawGizmos()
	{
		Vector3 vector = new Vector3(0f, 0f, 0f);
		for (int i = 0; i < 3; i++)
		{
			vector += new Vector3(0f, 0.3f, 0f);
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(this.m_endpoint1 + vector, this.m_endpoint2 + vector);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(this.m_center + vector, this.m_center + this.m_facingDir + vector);
		}
	}
}
