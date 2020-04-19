using System;
using System.Collections.Generic;
using UnityEngine;

public class GrydCardinalBombSequence : Sequence
{
	[Separator("Initial Projectile for spawn, ground portion starts when it lands", true)]
	public GenericSequenceProjectileAuthoredInfo m_initiatingProjectileInfo;

	[Separator("Projectile for hit area (joints not used)", true)]
	public GenericSequenceProjectileAuthoredInfo m_groundProjectileInfo;

	[Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
	[AnimEventPicker]
	public UnityEngine.Object m_startEvent;

	private bool m_startedInitialProjectile;

	private bool m_startedGroundProjectile;

	private bool m_didFinalPosHit;

	private List<GrydCardinalBombSequence.SegmentDataEntry> m_segmentData = new List<GrydCardinalBombSequence.SegmentDataEntry>();

	private Dictionary<int, List<ActorData>> m_indexToHitActors = new Dictionary<int, List<ActorData>>();

	private GenericSequenceProjectileInfo m_initialProjectileContainer;

	private List<GenericSequenceProjectileInfo> m_projectContainers = new List<GenericSequenceProjectileInfo>();

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (extraSequenceParams is GrydCardinalBombSequence.SegmentExtraParams)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalBombSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				GrydCardinalBombSequence.SegmentExtraParams segmentExtraParams = extraSequenceParams as GrydCardinalBombSequence.SegmentExtraParams;
				if (segmentExtraParams.m_segmentData != null)
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
					using (List<GrydCardinalBombSequence.SegmentDataEntry>.Enumerator enumerator = segmentExtraParams.m_segmentData.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GrydCardinalBombSequence.SegmentDataEntry segmentDataEntry = enumerator.Current;
							if (segmentDataEntry.m_startSquare != null)
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
								if (segmentDataEntry.m_endSquare != null && segmentDataEntry.m_startSquare != segmentDataEntry.m_endSquare)
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
									this.m_segmentData.Add(segmentDataEntry);
									continue;
								}
							}
							if (Application.isEditor)
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
								Debug.LogError(base.name + " has bad segment data for projectile");
							}
						}
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
				if (segmentExtraParams.m_hitActors != null)
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
					using (List<GrydCardinalBombSequence.HitActorEntry>.Enumerator enumerator2 = segmentExtraParams.m_hitActors.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							GrydCardinalBombSequence.HitActorEntry hitActorEntry = enumerator2.Current;
							int key = (int)hitActorEntry.m_segmentIndex;
							int actorIndex = (int)hitActorEntry.m_actorIndex;
							ActorData item = GameFlowData.Get().FindActorByActorIndex(actorIndex);
							if (!this.m_indexToHitActors.ContainsKey(key))
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
								this.m_indexToHitActors[key] = new List<ActorData>();
							}
							this.m_indexToHitActors[key].Add(item);
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
				return;
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	private void OnDisable()
	{
		foreach (GenericSequenceProjectileInfo genericSequenceProjectileInfo in this.m_projectContainers)
		{
			if (genericSequenceProjectileInfo != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalBombSequence.OnDisable()).MethodHandle;
				}
				genericSequenceProjectileInfo.OnSequenceDisable();
			}
		}
	}

	public override void FinishSetup()
	{
		base.FinishSetup();
		if (this.m_startEvent == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalBombSequence.FinishSetup()).MethodHandle;
			}
			this.m_startedInitialProjectile = true;
			if (this.m_initiatingProjectileInfo.m_fxPrefab != null)
			{
				this.StartInitialProjectile();
			}
			else
			{
				this.StartGroundProjectileChain();
			}
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_startEvent == parameter)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalBombSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			this.m_startedInitialProjectile = true;
			if (this.m_initiatingProjectileInfo.m_fxPrefab != null)
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
				this.StartInitialProjectile();
			}
			else
			{
				this.StartGroundProjectileChain();
			}
		}
	}

	private void Update()
	{
		base.ProcessSequenceVisibility();
		if (this.m_startedInitialProjectile && !this.m_didFinalPosHit)
		{
			if (this.m_initialProjectileContainer != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalBombSequence.Update()).MethodHandle;
				}
				this.m_initialProjectileContainer.OnUpdate();
			}
			if (!this.m_startedGroundProjectile)
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
				if (this.m_initialProjectileContainer != null)
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
					if (!this.m_initialProjectileContainer.m_finished)
					{
						goto IL_83;
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
				}
				this.StartGroundProjectileChain();
			}
			IL_83:
			if (this.m_startedGroundProjectile)
			{
				bool flag = false;
				int i = 0;
				while (i < this.m_segmentData.Count)
				{
					GrydCardinalBombSequence.SegmentDataEntry segmentDataEntry = this.m_segmentData[i];
					if (!segmentDataEntry.m_markedChildrenToStart)
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
						if (segmentDataEntry.m_projectileContainer != null && segmentDataEntry.m_projectileContainer.m_finished)
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
							this.StartChildrenProjectile(segmentDataEntry);
						}
					}
					if (segmentDataEntry.m_projectileContainer == null)
					{
						goto IL_107;
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!segmentDataEntry.m_projectileContainer.m_finished)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							goto IL_107;
						}
					}
					IL_109:
					i++;
					continue;
					IL_107:
					flag = true;
					goto IL_109;
				}
				for (int j = 0; j < this.m_projectContainers.Count; j++)
				{
					this.m_projectContainers[j].OnUpdate();
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
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
					base.Source.OnSequenceHit(this, base.TargetPos, null);
					this.m_didFinalPosHit = true;
				}
			}
		}
	}

	private void StartInitialProjectile()
	{
		if (this.m_initialProjectileContainer == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalBombSequence.StartInitialProjectile()).MethodHandle;
			}
			GameObject referenceModel = base.GetReferenceModel(base.Caster, this.m_initiatingProjectileInfo.m_jointReferenceType);
			Vector3 startPos = base.Caster.\u0016();
			Vector3 targetPos = base.TargetPos;
			if (referenceModel != null)
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
				this.m_initiatingProjectileInfo.m_fxJoint.Initialize(referenceModel);
				startPos = this.m_initiatingProjectileInfo.m_fxJoint.m_jointObject.transform.position;
			}
			this.m_initialProjectileContainer = new GenericSequenceProjectileInfo(this, this.m_initiatingProjectileInfo, startPos, targetPos, null);
			this.m_initialProjectileContainer.m_positionForSequenceHit = targetPos + Vector3.up;
		}
	}

	private void StartGroundProjectileChain()
	{
		for (int i = 0; i < this.m_segmentData.Count; i++)
		{
			if (this.m_segmentData[i].m_projectileContainer == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalBombSequence.StartGroundProjectileChain()).MethodHandle;
				}
				if ((int)this.m_segmentData[i].m_prevSegmentIndex < 0)
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
					this.SpawnProjectileForSegment(this.m_segmentData[i]);
				}
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
		this.m_startedGroundProjectile = true;
	}

	private void StartChildrenProjectile(GrydCardinalBombSequence.SegmentDataEntry parentSegment)
	{
		for (int i = 0; i < this.m_segmentData.Count; i++)
		{
			GrydCardinalBombSequence.SegmentDataEntry segmentDataEntry = this.m_segmentData[i];
			if (segmentDataEntry.m_projectileContainer == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalBombSequence.StartChildrenProjectile(GrydCardinalBombSequence.SegmentDataEntry)).MethodHandle;
				}
				if ((int)segmentDataEntry.m_prevSegmentIndex == (int)parentSegment.m_segmentIndex)
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
					this.SpawnProjectileForSegment(segmentDataEntry);
				}
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		parentSegment.m_markedChildrenToStart = true;
	}

	private void SpawnProjectileForSegment(GrydCardinalBombSequence.SegmentDataEntry segEntry)
	{
		Vector3 startPos = segEntry.m_startSquare.ToVector3();
		Vector3 endPos = segEntry.m_endSquare.ToVector3();
		ActorData[] targetActors = null;
		if (this.m_indexToHitActors.ContainsKey((int)segEntry.m_segmentIndex))
		{
			targetActors = this.m_indexToHitActors[(int)segEntry.m_segmentIndex].ToArray();
		}
		GenericSequenceProjectileInfo genericSequenceProjectileInfo = new GenericSequenceProjectileInfo(this, this.m_groundProjectileInfo, startPos, endPos, targetActors);
		segEntry.m_projectileContainer = genericSequenceProjectileInfo;
		this.m_projectContainers.Add(genericSequenceProjectileInfo);
	}

	public class SegmentExtraParams : Sequence.IExtraSequenceParams
	{
		public List<GrydCardinalBombSequence.SegmentDataEntry> m_segmentData;

		public List<GrydCardinalBombSequence.HitActorEntry> m_hitActors;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			sbyte b;
			if (this.m_segmentData != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalBombSequence.SegmentExtraParams.XSP_SerializeToStream(IBitStream)).MethodHandle;
				}
				b = (sbyte)this.m_segmentData.Count;
			}
			else
			{
				b = 0;
			}
			sbyte b2 = b;
			stream.Serialize(ref b2);
			for (int i = 0; i < (int)b2; i++)
			{
				GrydCardinalBombSequence.SegmentDataEntry.Serialize(stream, this.m_segmentData[i]);
			}
			sbyte b3;
			if (this.m_hitActors != null)
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
				b3 = (sbyte)this.m_hitActors.Count;
			}
			else
			{
				b3 = 0;
			}
			sbyte b4 = b3;
			stream.Serialize(ref b4);
			for (int j = 0; j < (int)b4; j++)
			{
				GrydCardinalBombSequence.HitActorEntry.Serialize(stream, this.m_hitActors[j]);
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte b = 0;
			stream.Serialize(ref b);
			this.m_segmentData = new List<GrydCardinalBombSequence.SegmentDataEntry>();
			for (int i = 0; i < (int)b; i++)
			{
				GrydCardinalBombSequence.SegmentDataEntry segmentDataEntry = new GrydCardinalBombSequence.SegmentDataEntry();
				GrydCardinalBombSequence.SegmentDataEntry.Deserialize(stream, segmentDataEntry);
				this.m_segmentData.Add(segmentDataEntry);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalBombSequence.SegmentExtraParams.XSP_DeserializeFromStream(IBitStream)).MethodHandle;
			}
			sbyte b2 = 0;
			stream.Serialize(ref b2);
			this.m_hitActors = new List<GrydCardinalBombSequence.HitActorEntry>();
			for (int j = 0; j < (int)b2; j++)
			{
				GrydCardinalBombSequence.HitActorEntry hitActorEntry = new GrydCardinalBombSequence.HitActorEntry();
				GrydCardinalBombSequence.HitActorEntry.Deserialize(stream, hitActorEntry);
				this.m_hitActors.Add(hitActorEntry);
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

	public class SegmentDataEntry
	{
		public sbyte m_segmentIndex;

		public sbyte m_prevSegmentIndex;

		public BoardSquare m_startSquare;

		public BoardSquare m_endSquare;

		public bool m_markedChildrenToStart;

		public GenericSequenceProjectileInfo m_projectileContainer;

		public static void Serialize(IBitStream stream, GrydCardinalBombSequence.SegmentDataEntry entry)
		{
			stream.Serialize(ref entry.m_segmentIndex);
			stream.Serialize(ref entry.m_prevSegmentIndex);
			sbyte b;
			if (entry.m_startSquare != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalBombSequence.SegmentDataEntry.Serialize(IBitStream, GrydCardinalBombSequence.SegmentDataEntry)).MethodHandle;
				}
				b = (sbyte)entry.m_startSquare.x;
			}
			else
			{
				b = 0;
			}
			sbyte b2 = b;
			sbyte b3;
			if (entry.m_startSquare != null)
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
				b3 = (sbyte)entry.m_startSquare.y;
			}
			else
			{
				b3 = 0;
			}
			sbyte b4 = b3;
			sbyte b5;
			if (entry.m_endSquare != null)
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
				b5 = (sbyte)entry.m_endSquare.x;
			}
			else
			{
				b5 = 0;
			}
			sbyte b6 = b5;
			sbyte b7;
			if (entry.m_endSquare != null)
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
				b7 = (sbyte)entry.m_endSquare.y;
			}
			else
			{
				b7 = 0;
			}
			sbyte b8 = b7;
			stream.Serialize(ref b2);
			stream.Serialize(ref b4);
			stream.Serialize(ref b6);
			stream.Serialize(ref b8);
		}

		public static void Deserialize(IBitStream stream, GrydCardinalBombSequence.SegmentDataEntry entry)
		{
			stream.Serialize(ref entry.m_segmentIndex);
			stream.Serialize(ref entry.m_prevSegmentIndex);
			sbyte b = 0;
			sbyte b2 = 0;
			sbyte b3 = 0;
			sbyte b4 = 0;
			stream.Serialize(ref b);
			stream.Serialize(ref b2);
			stream.Serialize(ref b3);
			stream.Serialize(ref b4);
			entry.m_startSquare = Board.\u000E().\u0016((int)b, (int)b2);
			entry.m_endSquare = Board.\u000E().\u0016((int)b3, (int)b4);
		}
	}

	public class HitActorEntry
	{
		public sbyte m_segmentIndex;

		public sbyte m_actorIndex;

		public static void Serialize(IBitStream stream, GrydCardinalBombSequence.HitActorEntry entry)
		{
			stream.Serialize(ref entry.m_segmentIndex);
			stream.Serialize(ref entry.m_actorIndex);
		}

		public static void Deserialize(IBitStream stream, GrydCardinalBombSequence.HitActorEntry entry)
		{
			stream.Serialize(ref entry.m_segmentIndex);
			stream.Serialize(ref entry.m_actorIndex);
		}
	}
}
