using System.Collections.Generic;
using UnityEngine;

public class GrydCardinalBombSequence : Sequence
{
	public class SegmentExtraParams : IExtraSequenceParams
	{
		public List<SegmentDataEntry> m_segmentData;

		public List<HitActorEntry> m_hitActors;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			int num;
			if (m_segmentData != null)
			{
				num = m_segmentData.Count;
			}
			else
			{
				num = 0;
			}
			sbyte value = (sbyte)num;
			stream.Serialize(ref value);
			for (int i = 0; i < value; i++)
			{
				SegmentDataEntry.Serialize(stream, m_segmentData[i]);
			}
			int num2;
			if (m_hitActors != null)
			{
				num2 = m_hitActors.Count;
			}
			else
			{
				num2 = 0;
			}
			sbyte value2 = (sbyte)num2;
			stream.Serialize(ref value2);
			for (int j = 0; j < value2; j++)
			{
				HitActorEntry.Serialize(stream, m_hitActors[j]);
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte value = 0;
			stream.Serialize(ref value);
			m_segmentData = new List<SegmentDataEntry>();
			for (int i = 0; i < value; i++)
			{
				SegmentDataEntry segmentDataEntry = new SegmentDataEntry();
				SegmentDataEntry.Deserialize(stream, segmentDataEntry);
				m_segmentData.Add(segmentDataEntry);
			}
			while (true)
			{
				sbyte value2 = 0;
				stream.Serialize(ref value2);
				m_hitActors = new List<HitActorEntry>();
				for (int j = 0; j < value2; j++)
				{
					HitActorEntry hitActorEntry = new HitActorEntry();
					HitActorEntry.Deserialize(stream, hitActorEntry);
					m_hitActors.Add(hitActorEntry);
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

	public class SegmentDataEntry
	{
		public sbyte m_segmentIndex;

		public sbyte m_prevSegmentIndex;

		public BoardSquare m_startSquare;

		public BoardSquare m_endSquare;

		public bool m_markedChildrenToStart;

		public GenericSequenceProjectileInfo m_projectileContainer;

		public static void Serialize(IBitStream stream, SegmentDataEntry entry)
		{
			stream.Serialize(ref entry.m_segmentIndex);
			stream.Serialize(ref entry.m_prevSegmentIndex);
			int num;
			if (entry.m_startSquare != null)
			{
				num = entry.m_startSquare.x;
			}
			else
			{
				num = 0;
			}
			sbyte value = (sbyte)num;
			int num2;
			if (entry.m_startSquare != null)
			{
				num2 = entry.m_startSquare.y;
			}
			else
			{
				num2 = 0;
			}
			sbyte value2 = (sbyte)num2;
			int num3;
			if (entry.m_endSquare != null)
			{
				num3 = entry.m_endSquare.x;
			}
			else
			{
				num3 = 0;
			}
			sbyte value3 = (sbyte)num3;
			int num4;
			if (entry.m_endSquare != null)
			{
				num4 = entry.m_endSquare.y;
			}
			else
			{
				num4 = 0;
			}
			sbyte value4 = (sbyte)num4;
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			stream.Serialize(ref value3);
			stream.Serialize(ref value4);
		}

		public static void Deserialize(IBitStream stream, SegmentDataEntry entry)
		{
			stream.Serialize(ref entry.m_segmentIndex);
			stream.Serialize(ref entry.m_prevSegmentIndex);
			sbyte value = 0;
			sbyte value2 = 0;
			sbyte value3 = 0;
			sbyte value4 = 0;
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			stream.Serialize(ref value3);
			stream.Serialize(ref value4);
			entry.m_startSquare = Board.Get().GetBoardSquare(value, value2);
			entry.m_endSquare = Board.Get().GetBoardSquare(value3, value4);
		}
	}

	public class HitActorEntry
	{
		public sbyte m_segmentIndex;

		public sbyte m_actorIndex;

		public static void Serialize(IBitStream stream, HitActorEntry entry)
		{
			stream.Serialize(ref entry.m_segmentIndex);
			stream.Serialize(ref entry.m_actorIndex);
		}

		public static void Deserialize(IBitStream stream, HitActorEntry entry)
		{
			stream.Serialize(ref entry.m_segmentIndex);
			stream.Serialize(ref entry.m_actorIndex);
		}
	}

	[Separator("Initial Projectile for spawn, ground portion starts when it lands", true)]
	public GenericSequenceProjectileAuthoredInfo m_initiatingProjectileInfo;

	[Separator("Projectile for hit area (joints not used)", true)]
	public GenericSequenceProjectileAuthoredInfo m_groundProjectileInfo;

	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public Object m_startEvent;

	private bool m_startedInitialProjectile;

	private bool m_startedGroundProjectile;

	private bool m_didFinalPosHit;

	private List<SegmentDataEntry> m_segmentData = new List<SegmentDataEntry>();

	private Dictionary<int, List<ActorData>> m_indexToHitActors = new Dictionary<int, List<ActorData>>();

	private GenericSequenceProjectileInfo m_initialProjectileContainer;

	private List<GenericSequenceProjectileInfo> m_projectContainers = new List<GenericSequenceProjectileInfo>();

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (!(extraSequenceParams is SegmentExtraParams))
			{
				continue;
			}
			while (true)
			{
				SegmentExtraParams segmentExtraParams = extraSequenceParams as SegmentExtraParams;
				if (segmentExtraParams.m_segmentData != null)
				{
					using (List<SegmentDataEntry>.Enumerator enumerator = segmentExtraParams.m_segmentData.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SegmentDataEntry current = enumerator.Current;
							if (current.m_startSquare != null)
							{
								if (current.m_endSquare != null && current.m_startSquare != current.m_endSquare)
								{
									m_segmentData.Add(current);
									continue;
								}
							}
							if (Application.isEditor)
							{
								Debug.LogError(base.name + " has bad segment data for projectile");
							}
						}
					}
				}
				if (segmentExtraParams.m_hitActors != null)
				{
					while (true)
					{
						using (List<HitActorEntry>.Enumerator enumerator2 = segmentExtraParams.m_hitActors.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								HitActorEntry current2 = enumerator2.Current;
								int key = current2.m_segmentIndex;
								int actorIndex = current2.m_actorIndex;
								ActorData item = GameFlowData.Get().FindActorByActorIndex(actorIndex);
								if (!m_indexToHitActors.ContainsKey(key))
								{
									m_indexToHitActors[key] = new List<ActorData>();
								}
								m_indexToHitActors[key].Add(item);
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
				return;
			}
		}
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void OnDisable()
	{
		foreach (GenericSequenceProjectileInfo projectContainer in m_projectContainers)
		{
			if (projectContainer != null)
			{
				projectContainer.OnSequenceDisable();
			}
		}
	}

	public override void FinishSetup()
	{
		base.FinishSetup();
		if (!(m_startEvent == null))
		{
			return;
		}
		while (true)
		{
			m_startedInitialProjectile = true;
			if (m_initiatingProjectileInfo.m_fxPrefab != null)
			{
				StartInitialProjectile();
			}
			else
			{
				StartGroundProjectileChain();
			}
			return;
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
			m_startedInitialProjectile = true;
			if (m_initiatingProjectileInfo.m_fxPrefab != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						StartInitialProjectile();
						return;
					}
				}
			}
			StartGroundProjectileChain();
			return;
		}
	}

	private void Update()
	{
		ProcessSequenceVisibility();
		if (!m_startedInitialProjectile || m_didFinalPosHit)
		{
			return;
		}
		if (m_initialProjectileContainer != null)
		{
			m_initialProjectileContainer.OnUpdate();
		}
		if (!m_startedGroundProjectile)
		{
			if (m_initialProjectileContainer != null)
			{
				if (!m_initialProjectileContainer.m_finished)
				{
					goto IL_0083;
				}
			}
			StartGroundProjectileChain();
		}
		goto IL_0083;
		IL_0083:
		if (!m_startedGroundProjectile)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < m_segmentData.Count; i++)
		{
			SegmentDataEntry segmentDataEntry = m_segmentData[i];
			if (!segmentDataEntry.m_markedChildrenToStart)
			{
				if (segmentDataEntry.m_projectileContainer != null && segmentDataEntry.m_projectileContainer.m_finished)
				{
					StartChildrenProjectile(segmentDataEntry);
				}
			}
			if (segmentDataEntry.m_projectileContainer != null)
			{
				if (segmentDataEntry.m_projectileContainer.m_finished)
				{
					continue;
				}
			}
			flag = true;
		}
		for (int j = 0; j < m_projectContainers.Count; j++)
		{
			m_projectContainers[j].OnUpdate();
		}
		while (true)
		{
			if (!flag)
			{
				while (true)
				{
					base.Source.OnSequenceHit(this, base.TargetPos);
					m_didFinalPosHit = true;
					return;
				}
			}
			return;
		}
	}

	private void StartInitialProjectile()
	{
		if (m_initialProjectileContainer != null)
		{
			return;
		}
		while (true)
		{
			GameObject referenceModel = GetReferenceModel(base.Caster, m_initiatingProjectileInfo.m_jointReferenceType);
			Vector3 startPos = base.Caster.GetTravelBoardSquareWorldPosition();
			Vector3 targetPos = base.TargetPos;
			if (referenceModel != null)
			{
				m_initiatingProjectileInfo.m_fxJoint.Initialize(referenceModel);
				startPos = m_initiatingProjectileInfo.m_fxJoint.m_jointObject.transform.position;
			}
			m_initialProjectileContainer = new GenericSequenceProjectileInfo(this, m_initiatingProjectileInfo, startPos, targetPos, null);
			m_initialProjectileContainer.m_positionForSequenceHit = targetPos + Vector3.up;
			return;
		}
	}

	private void StartGroundProjectileChain()
	{
		for (int i = 0; i < m_segmentData.Count; i++)
		{
			if (m_segmentData[i].m_projectileContainer != null)
			{
				continue;
			}
			if (m_segmentData[i].m_prevSegmentIndex < 0)
			{
				SpawnProjectileForSegment(m_segmentData[i]);
			}
		}
		while (true)
		{
			m_startedGroundProjectile = true;
			return;
		}
	}

	private void StartChildrenProjectile(SegmentDataEntry parentSegment)
	{
		for (int i = 0; i < m_segmentData.Count; i++)
		{
			SegmentDataEntry segmentDataEntry = m_segmentData[i];
			if (segmentDataEntry.m_projectileContainer != null)
			{
				continue;
			}
			if (segmentDataEntry.m_prevSegmentIndex == parentSegment.m_segmentIndex)
			{
				SpawnProjectileForSegment(segmentDataEntry);
			}
		}
		while (true)
		{
			parentSegment.m_markedChildrenToStart = true;
			return;
		}
	}

	private void SpawnProjectileForSegment(SegmentDataEntry segEntry)
	{
		Vector3 startPos = segEntry.m_startSquare.ToVector3();
		Vector3 endPos = segEntry.m_endSquare.ToVector3();
		ActorData[] targetActors = null;
		if (m_indexToHitActors.ContainsKey(segEntry.m_segmentIndex))
		{
			targetActors = m_indexToHitActors[segEntry.m_segmentIndex].ToArray();
		}
		GenericSequenceProjectileInfo item = segEntry.m_projectileContainer = new GenericSequenceProjectileInfo(this, m_groundProjectileInfo, startPos, endPos, targetActors);
		m_projectContainers.Add(item);
	}
}
