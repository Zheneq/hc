using System.Collections.Generic;
using UnityEngine;

public class SequenceLookup : MonoBehaviour
{
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
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		m_sequencesLoaded = new GameObject[m_sequences.Count];
		for (short num = 0; num < m_sequenceNameHashList.Count; num = (short)(num + 1))
		{
			int key = m_sequenceNameHashList[num];
			if (!m_sequenceNameHashToIndex.ContainsKey(key))
			{
				m_sequenceNameHashToIndex.Add(key, num);
			}
			else
			{
				Log.Error("SequenceLookup contains duplicate sequence name hash");
			}
		}
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public static int GetSequenceNameHash(string name)
	{
		return Animator.StringToHash(name.ToLower());
	}

	public GameObject GetPrefabOfSequenceId(short sequenceId)
	{
		if (sequenceId < 0 || sequenceId >= m_sequences.Count)
		{
			return null;
		}
		if (m_sequencesLoaded[sequenceId] != null)
		{
			return m_sequencesLoaded[sequenceId];
		}

		GameObject sequencePrefab = m_sequences[sequenceId] != null
			? m_sequences[sequenceId].GetPrefab(true)
			: null;
		if (sequencePrefab == null)
		{
			Debug.LogError("SequenceLookup contains Null sequence prefabs, please update BootstrapSingleton");
			return null;
		}
		m_sequencesLoaded[sequenceId] = sequencePrefab;
		Sequence[] components = sequencePrefab.GetComponents<Sequence>();
		foreach (Sequence sequence in components)
		{
			sequence.InitPrefabLookupId(sequenceId);
		}
		return m_sequencesLoaded[sequenceId];
	}

	public short GetSequenceIdOfPrefab(GameObject sequencePrefab)
	{
		if (sequencePrefab == null)
		{
			return -1;
		}
		
		if (m_sequenceNameHashToIndex.TryGetValue(GetSequenceNameHash(sequencePrefab.name), out short prefabId))
		{
			return prefabId;
		}
		
		Debug.LogError("Did not find name hash of sequence prefab: " + sequencePrefab.name + ", please update sequence lookup");
		return -1;
	}

	internal static void UnloadAll()
	{
		if (s_instance != null)
		{
			s_instance.m_sequencesLoaded = new GameObject[s_instance.m_sequences.Count];
		}
	}

	public GameObject GetSimpleHitSequencePrefab()
	{
		return m_simpleHitSequencePrefab;
	}

	public GameObject GetDebugVfxOnPosHitSequence()
	{
		return m_debugVfxOnPositionSequencePrefab;
	}

	public static SequenceExtraParamEnum GetEnumOfExtraParam(Sequence.IExtraSequenceParams extraParam)
	{
		switch (extraParam)
		{
			case PowerUp.ExtraParams _:
				return SequenceExtraParamEnum.Powerup;
			case BouncingShotSequence.ExtraParams _:
				return SequenceExtraParamEnum.BouncingShot;
			case ExplosionSequence.ExtraParams _:
				return SequenceExtraParamEnum.Explosion;
			case GroundLineSequence.ExtraParams _:
				return SequenceExtraParamEnum.GroundLine;
			case HealLaserSequence.ExtraParams _:
				return SequenceExtraParamEnum.HealLaser;
			case HitOnAnimationEventSequence.ExtraParams _:
				return SequenceExtraParamEnum.HitOnAnimationEvent;
			case NanosmithBoltLaserSequence.ExtraParams _:
				return SequenceExtraParamEnum.NanosmithBoltLaser;
			case NinjaMultiAttackSequence.ExtraParams _:
				return SequenceExtraParamEnum.NinjaMultiAttack;
			case ProximityMineGroundSequence.ExtraParams _:
				return SequenceExtraParamEnum.ProximityMineGround;
			case SplineProjectileSequence.DelayedProjectileExtraParams _:
				return SequenceExtraParamEnum.DelayedProjectile;
			case SplineProjectileSequence.MultiEventExtraParams _:
				return SequenceExtraParamEnum.MultiEventProjectile;
			case SimpleAttachedVFXSequence.MultiEventExtraParams _:
				return SequenceExtraParamEnum.MultiEventAttachedVFX;
			case ExoSweepLaserSequence.ExtraParams _:
				return SequenceExtraParamEnum.ExoSweepLaser;
			case BlasterStretchConeSequence.ExtraParams _:
				return SequenceExtraParamEnum.BlasterStretchingCone;
			case SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam _:
				return SequenceExtraParamEnum.SimpleVFXAtTargetPos;
			case ThiefPowerupReturnProjectileSequence.PowerupTypeExtraParams _:
				return SequenceExtraParamEnum.ThiefPowerupReturnProjectile;
			case ScoundrelBlindFireSequence.ConeExtraParams _:
				return SequenceExtraParamEnum.ScoundrelBlindFireCone;
			case SimpleAttachedVFXSequence.ImpactDelayParams _:
				return SequenceExtraParamEnum.ImpactDelayAttachedVFX;
			case SoldierProjectilesInLineSequence.HitAreaExtraParams _:
				return SequenceExtraParamEnum.SoldierProjectilesInLineHitArea;
			case SimpleTimingSequence.ExtraParams _:
				return SequenceExtraParamEnum.SimpleTiming;
			case NekoDiscReturnProjectileSequence.DiscReturnProjectileExtraParams _:
				return SequenceExtraParamEnum.DiscReturnProjectile;
			case SplineProjectileSequence.ProjectilePropertyParams _:
				return SequenceExtraParamEnum.SplineProjectileProperty;
			case Sequence.FxAttributeParam _:
				return SequenceExtraParamEnum.SequenceFxAttribute;
			case Sequence.PhaseTimingExtraParams _:
				return SequenceExtraParamEnum.SequencePhaseTiming;
			case ValkyrieDirectionalShieldSequence.ExtraParams _:
				return SequenceExtraParamEnum.ValkyrieDirectionalShield;
			case Sequence.ActorIndexExtraParam _:
				return SequenceExtraParamEnum.ActorIndexParam;
			case HitActorGroupOnAnimEventSequence.ActorParams _:
				return SequenceExtraParamEnum.HitActorGroupActorsParam;
			case GrydCardinalBombSequence.SegmentExtraParams _:
				return SequenceExtraParamEnum.GrydCardinalBombParam;
			case SimpleVFXAtTargetPosSequence.PositionOverrideParam _:
				return SequenceExtraParamEnum.VFXSpawnPosOverride;
			case Sequence.GenericIntParam _:
				return SequenceExtraParamEnum.GenericIntParam;
			case Sequence.GenericActorListParam _:
				return SequenceExtraParamEnum.GenericActorListParam;
			default:
				return SequenceExtraParamEnum.Invalid;
		}
	}

	public Sequence.IExtraSequenceParams CreateExtraParamOfEnum(SequenceExtraParamEnum paramEnum)
	{
		switch (paramEnum)
		{
			case SequenceExtraParamEnum.Powerup:
				return new PowerUp.ExtraParams();
			case SequenceExtraParamEnum.BouncingShot:
				return new BouncingShotSequence.ExtraParams();
			case SequenceExtraParamEnum.Explosion:
				return new ExplosionSequence.ExtraParams();
			case SequenceExtraParamEnum.GroundLine:
				return new GroundLineSequence.ExtraParams();
			case SequenceExtraParamEnum.HealLaser:
				return new HealLaserSequence.ExtraParams();
			case SequenceExtraParamEnum.HitOnAnimationEvent:
				return new HitOnAnimationEventSequence.ExtraParams();
			case SequenceExtraParamEnum.NanosmithBoltLaser:
				return new NanosmithBoltLaserSequence.ExtraParams();
			case SequenceExtraParamEnum.NinjaMultiAttack:
				return new NinjaMultiAttackSequence.ExtraParams();
			case SequenceExtraParamEnum.ProximityMineGround:
				return new ProximityMineGroundSequence.ExtraParams();
			case SequenceExtraParamEnum.DelayedProjectile:
				return new SplineProjectileSequence.DelayedProjectileExtraParams();
			case SequenceExtraParamEnum.MultiEventProjectile:
				return new SplineProjectileSequence.MultiEventExtraParams();
			case SequenceExtraParamEnum.MultiEventAttachedVFX:
				return new SimpleAttachedVFXSequence.MultiEventExtraParams();
			case SequenceExtraParamEnum.ExoSweepLaser:
				return new ExoSweepLaserSequence.ExtraParams();
			case SequenceExtraParamEnum.BlasterStretchingCone:
				return new BlasterStretchConeSequence.ExtraParams();
			case SequenceExtraParamEnum.SimpleVFXAtTargetPos:
				return new SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam();
			case SequenceExtraParamEnum.ThiefPowerupReturnProjectile:
				return new ThiefPowerupReturnProjectileSequence.PowerupTypeExtraParams();
			case SequenceExtraParamEnum.ScoundrelBlindFireCone:
				return new ScoundrelBlindFireSequence.ConeExtraParams();
			case SequenceExtraParamEnum.ImpactDelayAttachedVFX:
				return new SimpleAttachedVFXSequence.ImpactDelayParams();
			case SequenceExtraParamEnum.SoldierProjectilesInLineHitArea:
				return new SoldierProjectilesInLineSequence.HitAreaExtraParams();
			case SequenceExtraParamEnum.SimpleTiming:
				return new SimpleTimingSequence.ExtraParams();
			case SequenceExtraParamEnum.SplineProjectileProperty:
				return new SplineProjectileSequence.ProjectilePropertyParams();
			case SequenceExtraParamEnum.SequenceFxAttribute:
				return new Sequence.FxAttributeParam();
			case SequenceExtraParamEnum.SequencePhaseTiming:
				return new Sequence.PhaseTimingExtraParams();
			case SequenceExtraParamEnum.ValkyrieDirectionalShield:
				return new ValkyrieDirectionalShieldSequence.ExtraParams();
			case SequenceExtraParamEnum.ActorIndexParam:
				return new Sequence.ActorIndexExtraParam();
			case SequenceExtraParamEnum.HitActorGroupActorsParam:
				return new HitActorGroupOnAnimEventSequence.ActorParams();
			case SequenceExtraParamEnum.GrydCardinalBombParam:
				return new GrydCardinalBombSequence.SegmentExtraParams();
			case SequenceExtraParamEnum.VFXSpawnPosOverride:
				return new SimpleVFXAtTargetPosSequence.PositionOverrideParam();
			case SequenceExtraParamEnum.DiscReturnProjectile:
				return new NekoDiscReturnProjectileSequence.DiscReturnProjectileExtraParams();
			case SequenceExtraParamEnum.GenericIntParam:
				return new Sequence.GenericIntParam();
			case SequenceExtraParamEnum.GenericActorListParam:
				return new Sequence.GenericActorListParam();
			default:
				return null;
		}
	}
}
