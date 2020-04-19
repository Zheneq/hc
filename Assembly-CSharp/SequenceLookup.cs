using System;
using System.Collections.Generic;
using UnityEngine;

public class SequenceLookup : MonoBehaviour
{
	public GameObject m_simpleHitSequencePrefab;

	public GameObject m_debugVfxOnPositionSequencePrefab;

	public const short c_invalidSequence = -1;

	private static SequenceLookup s_instance;

	public List<PrefabResourceLink> m_sequences;

	public List<int> m_sequenceNameHashList;

	private GameObject[] m_sequencesLoaded;

	private Dictionary<int, short> m_sequenceNameHashToIndex = new Dictionary<int, short>();

	public static SequenceLookup Get()
	{
		return SequenceLookup.s_instance;
	}

	private void Awake()
	{
		SequenceLookup.s_instance = this;
		this.m_sequencesLoaded = new GameObject[this.m_sequences.Count];
		short num = 0;
		while ((int)num < this.m_sequenceNameHashList.Count)
		{
			int key = this.m_sequenceNameHashList[(int)num];
			if (!this.m_sequenceNameHashToIndex.ContainsKey(key))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SequenceLookup.Awake()).MethodHandle;
				}
				this.m_sequenceNameHashToIndex.Add(key, num);
			}
			else
			{
				Log.Error("SequenceLookup contains duplicate sequence name hash", new object[0]);
			}
			num += 1;
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
	}

	private void OnDestroy()
	{
		SequenceLookup.s_instance = null;
	}

	public static int GetSequenceNameHash(string name)
	{
		return Animator.StringToHash(name.ToLower());
	}

	public GameObject GetPrefabOfSequenceId(short sequenceId)
	{
		if (sequenceId < 0 || (int)sequenceId >= this.m_sequences.Count)
		{
			return null;
		}
		if (this.m_sequencesLoaded[(int)sequenceId])
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SequenceLookup.GetPrefabOfSequenceId(short)).MethodHandle;
			}
			return this.m_sequencesLoaded[(int)sequenceId];
		}
		GameObject gameObject;
		if (this.m_sequences[(int)sequenceId] != null)
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
			gameObject = this.m_sequences[(int)sequenceId].GetPrefab(true);
		}
		else
		{
			gameObject = null;
		}
		GameObject gameObject2 = gameObject;
		if (gameObject2 == null)
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
			Debug.LogError("SequenceLookup contains Null sequence prefabs, please update BootstrapSingleton");
			return null;
		}
		this.m_sequencesLoaded[(int)sequenceId] = gameObject2;
		foreach (Sequence sequence in gameObject2.GetComponents<Sequence>())
		{
			sequence.InitPrefabLookupId(sequenceId);
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
		return this.m_sequencesLoaded[(int)sequenceId];
	}

	public short GetSequenceIdOfPrefab(GameObject sequencePrefab)
	{
		if (sequencePrefab == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SequenceLookup.GetSequenceIdOfPrefab(GameObject)).MethodHandle;
			}
			return -1;
		}
		int sequenceNameHash = SequenceLookup.GetSequenceNameHash(sequencePrefab.name);
		if (this.m_sequenceNameHashToIndex.ContainsKey(sequenceNameHash))
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
			return this.m_sequenceNameHashToIndex[sequenceNameHash];
		}
		Debug.LogError("Did not find name hash of sequence prefab: " + sequencePrefab.name + ", please update sequence lookup");
		return -1;
	}

	internal static void UnloadAll()
	{
		if (SequenceLookup.s_instance)
		{
			SequenceLookup.s_instance.m_sequencesLoaded = new GameObject[SequenceLookup.s_instance.m_sequences.Count];
		}
	}

	public GameObject GetSimpleHitSequencePrefab()
	{
		return this.m_simpleHitSequencePrefab;
	}

	public GameObject GetDebugVfxOnPosHitSequence()
	{
		return this.m_debugVfxOnPositionSequencePrefab;
	}

	public static SequenceLookup.SequenceExtraParamEnum GetEnumOfExtraParam(Sequence.IExtraSequenceParams extraParam)
	{
		SequenceLookup.SequenceExtraParamEnum result = SequenceLookup.SequenceExtraParamEnum.Invalid;
		if (extraParam is PowerUp.ExtraParams)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SequenceLookup.GetEnumOfExtraParam(Sequence.IExtraSequenceParams)).MethodHandle;
			}
			result = SequenceLookup.SequenceExtraParamEnum.Powerup;
		}
		else if (extraParam is BouncingShotSequence.ExtraParams)
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
			result = SequenceLookup.SequenceExtraParamEnum.BouncingShot;
		}
		else if (extraParam is ExplosionSequence.ExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.Explosion;
		}
		else if (extraParam is GroundLineSequence.ExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.GroundLine;
		}
		else if (extraParam is HealLaserSequence.ExtraParams)
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
			result = SequenceLookup.SequenceExtraParamEnum.HealLaser;
		}
		else if (extraParam is HitOnAnimationEventSequence.ExtraParams)
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
			result = SequenceLookup.SequenceExtraParamEnum.HitOnAnimationEvent;
		}
		else if (extraParam is NanosmithBoltLaserSequence.ExtraParams)
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
			result = SequenceLookup.SequenceExtraParamEnum.NanosmithBoltLaser;
		}
		else if (extraParam is NinjaMultiAttackSequence.ExtraParams)
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
			result = SequenceLookup.SequenceExtraParamEnum.NinjaMultiAttack;
		}
		else if (extraParam is ProximityMineGroundSequence.ExtraParams)
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
			result = SequenceLookup.SequenceExtraParamEnum.ProximityMineGround;
		}
		else if (extraParam is SplineProjectileSequence.DelayedProjectileExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.DelayedProjectile;
		}
		else if (extraParam is SplineProjectileSequence.MultiEventExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.MultiEventProjectile;
		}
		else if (extraParam is SimpleAttachedVFXSequence.MultiEventExtraParams)
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
			result = SequenceLookup.SequenceExtraParamEnum.MultiEventAttachedVFX;
		}
		else if (extraParam is ExoSweepLaserSequence.ExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.ExoSweepLaser;
		}
		else if (extraParam is BlasterStretchConeSequence.ExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.BlasterStretchingCone;
		}
		else if (extraParam is SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam)
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
			result = SequenceLookup.SequenceExtraParamEnum.SimpleVFXAtTargetPos;
		}
		else if (extraParam is ThiefPowerupReturnProjectileSequence.PowerupTypeExtraParams)
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
			result = SequenceLookup.SequenceExtraParamEnum.ThiefPowerupReturnProjectile;
		}
		else if (extraParam is ScoundrelBlindFireSequence.ConeExtraParams)
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
			result = SequenceLookup.SequenceExtraParamEnum.ScoundrelBlindFireCone;
		}
		else if (extraParam is SimpleAttachedVFXSequence.ImpactDelayParams)
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
			result = SequenceLookup.SequenceExtraParamEnum.ImpactDelayAttachedVFX;
		}
		else if (extraParam is SoldierProjectilesInLineSequence.HitAreaExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.SoldierProjectilesInLineHitArea;
		}
		else if (extraParam is SimpleTimingSequence.ExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.SimpleTiming;
		}
		else if (extraParam is NekoDiscReturnProjectileSequence.DiscReturnProjectileExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.DiscReturnProjectile;
		}
		else if (extraParam is SplineProjectileSequence.ProjectilePropertyParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.SplineProjectileProperty;
		}
		else if (extraParam is Sequence.FxAttributeParam)
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
			result = SequenceLookup.SequenceExtraParamEnum.SequenceFxAttribute;
		}
		else if (extraParam is Sequence.PhaseTimingExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.SequencePhaseTiming;
		}
		else if (extraParam is ValkyrieDirectionalShieldSequence.ExtraParams)
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
			result = SequenceLookup.SequenceExtraParamEnum.ValkyrieDirectionalShield;
		}
		else if (extraParam is Sequence.ActorIndexExtraParam)
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
			result = SequenceLookup.SequenceExtraParamEnum.ActorIndexParam;
		}
		else if (extraParam is HitActorGroupOnAnimEventSequence.ActorParams)
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
			result = SequenceLookup.SequenceExtraParamEnum.HitActorGroupActorsParam;
		}
		else if (extraParam is GrydCardinalBombSequence.SegmentExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.GrydCardinalBombParam;
		}
		else if (extraParam is SimpleVFXAtTargetPosSequence.PositionOverrideParam)
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
			result = SequenceLookup.SequenceExtraParamEnum.VFXSpawnPosOverride;
		}
		else if (extraParam is Sequence.GenericIntParam)
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
			result = SequenceLookup.SequenceExtraParamEnum.GenericIntParam;
		}
		else if (extraParam is Sequence.GenericActorListParam)
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
			result = SequenceLookup.SequenceExtraParamEnum.GenericActorListParam;
		}
		return result;
	}

	public Sequence.IExtraSequenceParams CreateExtraParamOfEnum(SequenceLookup.SequenceExtraParamEnum paramEnum)
	{
		Sequence.IExtraSequenceParams result = null;
		if (paramEnum == SequenceLookup.SequenceExtraParamEnum.Powerup)
		{
			result = new PowerUp.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.BouncingShot)
		{
			result = new BouncingShotSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.Explosion)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SequenceLookup.CreateExtraParamOfEnum(SequenceLookup.SequenceExtraParamEnum)).MethodHandle;
			}
			result = new ExplosionSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.GroundLine)
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
			result = new GroundLineSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.HealLaser)
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
			result = new HealLaserSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.HitOnAnimationEvent)
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
			result = new HitOnAnimationEventSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.NanosmithBoltLaser)
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
			result = new NanosmithBoltLaserSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.NinjaMultiAttack)
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
			result = new NinjaMultiAttackSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.ProximityMineGround)
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
			result = new ProximityMineGroundSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.DelayedProjectile)
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
			result = new SplineProjectileSequence.DelayedProjectileExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.MultiEventProjectile)
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
			result = new SplineProjectileSequence.MultiEventExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.MultiEventAttachedVFX)
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
			result = new SimpleAttachedVFXSequence.MultiEventExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.ExoSweepLaser)
		{
			result = new ExoSweepLaserSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.BlasterStretchingCone)
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
			result = new BlasterStretchConeSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.SimpleVFXAtTargetPos)
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
			result = new SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.ThiefPowerupReturnProjectile)
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
			result = new ThiefPowerupReturnProjectileSequence.PowerupTypeExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.ScoundrelBlindFireCone)
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
			result = new ScoundrelBlindFireSequence.ConeExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.ImpactDelayAttachedVFX)
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
			result = new SimpleAttachedVFXSequence.ImpactDelayParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.SoldierProjectilesInLineHitArea)
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
			result = new SoldierProjectilesInLineSequence.HitAreaExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.SimpleTiming)
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
			result = new SimpleTimingSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.SplineProjectileProperty)
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
			result = new SplineProjectileSequence.ProjectilePropertyParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.SequenceFxAttribute)
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
			result = new Sequence.FxAttributeParam();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.SequencePhaseTiming)
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
			result = new Sequence.PhaseTimingExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.ValkyrieDirectionalShield)
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
			result = new ValkyrieDirectionalShieldSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.ActorIndexParam)
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
			result = new Sequence.ActorIndexExtraParam();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.HitActorGroupActorsParam)
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
			result = new HitActorGroupOnAnimEventSequence.ActorParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.GrydCardinalBombParam)
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
			result = new GrydCardinalBombSequence.SegmentExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.VFXSpawnPosOverride)
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
			result = new SimpleVFXAtTargetPosSequence.PositionOverrideParam();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.DiscReturnProjectile)
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
			result = new NekoDiscReturnProjectileSequence.DiscReturnProjectileExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.GenericIntParam)
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
			result = new Sequence.GenericIntParam();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.GenericActorListParam)
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
			result = new Sequence.GenericActorListParam();
		}
		return result;
	}

	public enum SequenceExtraParamEnum
	{
		Invalid = 1,
		Powerup,
		BouncingShot,
		Explosion,
		GroundLine,
		HealLaser,
		HitOnAnimationEvent,
		NanosmithBoltLaser,
		NinjaMultiAttack,
		ProximityMineGround,
		DelayedProjectile,
		MultiEventProjectile,
		MultiEventAttachedVFX,
		BlasterStretchingCone,
		SimpleVFXAtTargetPos,
		ThiefPowerupReturnProjectile,
		ScoundrelBlindFireCone,
		ImpactDelayAttachedVFX,
		SoldierProjectilesInLineHitArea,
		SimpleTiming,
		ExoSweepLaser,
		SplineProjectileProperty,
		SequenceFxAttribute,
		SequencePhaseTiming,
		ValkyrieDirectionalShield,
		ActorIndexParam,
		HitActorGroupActorsParam,
		GrydCardinalBombParam,
		VFXSpawnPosOverride,
		DiscReturnProjectile,
		GenericIntParam,
		GenericActorListParam
	}
}
