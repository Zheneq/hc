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
				this.m_sequenceNameHashToIndex.Add(key, num);
			}
			else
			{
				Log.Error("SequenceLookup contains duplicate sequence name hash", new object[0]);
			}
			num += 1;
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
			return this.m_sequencesLoaded[(int)sequenceId];
		}
		GameObject gameObject;
		if (this.m_sequences[(int)sequenceId] != null)
		{
			gameObject = this.m_sequences[(int)sequenceId].GetPrefab(true);
		}
		else
		{
			gameObject = null;
		}
		GameObject gameObject2 = gameObject;
		if (gameObject2 == null)
		{
			Debug.LogError("SequenceLookup contains Null sequence prefabs, please update BootstrapSingleton");
			return null;
		}
		this.m_sequencesLoaded[(int)sequenceId] = gameObject2;
		foreach (Sequence sequence in gameObject2.GetComponents<Sequence>())
		{
			sequence.InitPrefabLookupId(sequenceId);
		}
		return this.m_sequencesLoaded[(int)sequenceId];
	}

	public short GetSequenceIdOfPrefab(GameObject sequencePrefab)
	{
		if (sequencePrefab == null)
		{
			return -1;
		}
		int sequenceNameHash = SequenceLookup.GetSequenceNameHash(sequencePrefab.name);
		if (this.m_sequenceNameHashToIndex.ContainsKey(sequenceNameHash))
		{
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
			result = SequenceLookup.SequenceExtraParamEnum.Powerup;
		}
		else if (extraParam is BouncingShotSequence.ExtraParams)
		{
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
			result = SequenceLookup.SequenceExtraParamEnum.HealLaser;
		}
		else if (extraParam is HitOnAnimationEventSequence.ExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.HitOnAnimationEvent;
		}
		else if (extraParam is NanosmithBoltLaserSequence.ExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.NanosmithBoltLaser;
		}
		else if (extraParam is NinjaMultiAttackSequence.ExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.NinjaMultiAttack;
		}
		else if (extraParam is ProximityMineGroundSequence.ExtraParams)
		{
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
			result = SequenceLookup.SequenceExtraParamEnum.SimpleVFXAtTargetPos;
		}
		else if (extraParam is ThiefPowerupReturnProjectileSequence.PowerupTypeExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.ThiefPowerupReturnProjectile;
		}
		else if (extraParam is ScoundrelBlindFireSequence.ConeExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.ScoundrelBlindFireCone;
		}
		else if (extraParam is SimpleAttachedVFXSequence.ImpactDelayParams)
		{
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
			result = SequenceLookup.SequenceExtraParamEnum.SequenceFxAttribute;
		}
		else if (extraParam is Sequence.PhaseTimingExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.SequencePhaseTiming;
		}
		else if (extraParam is ValkyrieDirectionalShieldSequence.ExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.ValkyrieDirectionalShield;
		}
		else if (extraParam is Sequence.ActorIndexExtraParam)
		{
			result = SequenceLookup.SequenceExtraParamEnum.ActorIndexParam;
		}
		else if (extraParam is HitActorGroupOnAnimEventSequence.ActorParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.HitActorGroupActorsParam;
		}
		else if (extraParam is GrydCardinalBombSequence.SegmentExtraParams)
		{
			result = SequenceLookup.SequenceExtraParamEnum.GrydCardinalBombParam;
		}
		else if (extraParam is SimpleVFXAtTargetPosSequence.PositionOverrideParam)
		{
			result = SequenceLookup.SequenceExtraParamEnum.VFXSpawnPosOverride;
		}
		else if (extraParam is Sequence.GenericIntParam)
		{
			result = SequenceLookup.SequenceExtraParamEnum.GenericIntParam;
		}
		else if (extraParam is Sequence.GenericActorListParam)
		{
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
			result = new ExplosionSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.GroundLine)
		{
			result = new GroundLineSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.HealLaser)
		{
			result = new HealLaserSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.HitOnAnimationEvent)
		{
			result = new HitOnAnimationEventSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.NanosmithBoltLaser)
		{
			result = new NanosmithBoltLaserSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.NinjaMultiAttack)
		{
			result = new NinjaMultiAttackSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.ProximityMineGround)
		{
			result = new ProximityMineGroundSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.DelayedProjectile)
		{
			result = new SplineProjectileSequence.DelayedProjectileExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.MultiEventProjectile)
		{
			result = new SplineProjectileSequence.MultiEventExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.MultiEventAttachedVFX)
		{
			result = new SimpleAttachedVFXSequence.MultiEventExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.ExoSweepLaser)
		{
			result = new ExoSweepLaserSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.BlasterStretchingCone)
		{
			result = new BlasterStretchConeSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.SimpleVFXAtTargetPos)
		{
			result = new SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.ThiefPowerupReturnProjectile)
		{
			result = new ThiefPowerupReturnProjectileSequence.PowerupTypeExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.ScoundrelBlindFireCone)
		{
			result = new ScoundrelBlindFireSequence.ConeExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.ImpactDelayAttachedVFX)
		{
			result = new SimpleAttachedVFXSequence.ImpactDelayParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.SoldierProjectilesInLineHitArea)
		{
			result = new SoldierProjectilesInLineSequence.HitAreaExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.SimpleTiming)
		{
			result = new SimpleTimingSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.SplineProjectileProperty)
		{
			result = new SplineProjectileSequence.ProjectilePropertyParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.SequenceFxAttribute)
		{
			result = new Sequence.FxAttributeParam();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.SequencePhaseTiming)
		{
			result = new Sequence.PhaseTimingExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.ValkyrieDirectionalShield)
		{
			result = new ValkyrieDirectionalShieldSequence.ExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.ActorIndexParam)
		{
			result = new Sequence.ActorIndexExtraParam();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.HitActorGroupActorsParam)
		{
			result = new HitActorGroupOnAnimEventSequence.ActorParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.GrydCardinalBombParam)
		{
			result = new GrydCardinalBombSequence.SegmentExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.VFXSpawnPosOverride)
		{
			result = new SimpleVFXAtTargetPosSequence.PositionOverrideParam();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.DiscReturnProjectile)
		{
			result = new NekoDiscReturnProjectileSequence.DiscReturnProjectileExtraParams();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.GenericIntParam)
		{
			result = new Sequence.GenericIntParam();
		}
		else if (paramEnum == SequenceLookup.SequenceExtraParamEnum.GenericActorListParam)
		{
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
