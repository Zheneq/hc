using System.Collections.Generic;
using System.Text;
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
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
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
		if ((bool)m_sequencesLoaded[sequenceId])
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_sequencesLoaded[sequenceId];
				}
			}
		}
		object obj;
		if (m_sequences[sequenceId] != null)
		{
			obj = m_sequences[sequenceId].GetPrefab(true);
		}
		else
		{
			obj = null;
		}
		GameObject gameObject = (GameObject)obj;
		if (gameObject == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Debug.LogError("SequenceLookup contains Null sequence prefabs, please update BootstrapSingleton");
					return null;
				}
			}
		}
		m_sequencesLoaded[sequenceId] = gameObject;
		Sequence[] components = gameObject.GetComponents<Sequence>();
		foreach (Sequence sequence in components)
		{
			sequence.InitPrefabLookupId(sequenceId);
		}
		while (true)
		{
			return m_sequencesLoaded[sequenceId];
		}
	}

	public short GetSequenceIdOfPrefab(GameObject sequencePrefab)
	{
		if (sequencePrefab == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return -1;
				}
			}
		}
		int sequenceNameHash = GetSequenceNameHash(sequencePrefab.name);
		if (m_sequenceNameHashToIndex.ContainsKey(sequenceNameHash))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_sequenceNameHashToIndex[sequenceNameHash];
				}
			}
		}
		Debug.LogError(new StringBuilder().Append("Did not find name hash of sequence prefab: ").Append(sequencePrefab.name).Append(", please update sequence lookup").ToString());
		return -1;
	}

	internal static void UnloadAll()
	{
		if ((bool)s_instance)
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
		SequenceExtraParamEnum result = SequenceExtraParamEnum.Invalid;
		if (extraParam is PowerUp.ExtraParams)
		{
			result = SequenceExtraParamEnum.Powerup;
		}
		else if (extraParam is BouncingShotSequence.ExtraParams)
		{
			result = SequenceExtraParamEnum.BouncingShot;
		}
		else if (extraParam is ExplosionSequence.ExtraParams)
		{
			result = SequenceExtraParamEnum.Explosion;
		}
		else if (extraParam is GroundLineSequence.ExtraParams)
		{
			result = SequenceExtraParamEnum.GroundLine;
		}
		else if (extraParam is HealLaserSequence.ExtraParams)
		{
			result = SequenceExtraParamEnum.HealLaser;
		}
		else if (extraParam is HitOnAnimationEventSequence.ExtraParams)
		{
			result = SequenceExtraParamEnum.HitOnAnimationEvent;
		}
		else if (extraParam is NanosmithBoltLaserSequence.ExtraParams)
		{
			result = SequenceExtraParamEnum.NanosmithBoltLaser;
		}
		else if (extraParam is NinjaMultiAttackSequence.ExtraParams)
		{
			result = SequenceExtraParamEnum.NinjaMultiAttack;
		}
		else if (extraParam is ProximityMineGroundSequence.ExtraParams)
		{
			result = SequenceExtraParamEnum.ProximityMineGround;
		}
		else if (extraParam is SplineProjectileSequence.DelayedProjectileExtraParams)
		{
			result = SequenceExtraParamEnum.DelayedProjectile;
		}
		else if (extraParam is SplineProjectileSequence.MultiEventExtraParams)
		{
			result = SequenceExtraParamEnum.MultiEventProjectile;
		}
		else if (extraParam is SimpleAttachedVFXSequence.MultiEventExtraParams)
		{
			result = SequenceExtraParamEnum.MultiEventAttachedVFX;
		}
		else if (extraParam is ExoSweepLaserSequence.ExtraParams)
		{
			result = SequenceExtraParamEnum.ExoSweepLaser;
		}
		else if (extraParam is BlasterStretchConeSequence.ExtraParams)
		{
			result = SequenceExtraParamEnum.BlasterStretchingCone;
		}
		else if (extraParam is SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam)
		{
			result = SequenceExtraParamEnum.SimpleVFXAtTargetPos;
		}
		else if (extraParam is ThiefPowerupReturnProjectileSequence.PowerupTypeExtraParams)
		{
			result = SequenceExtraParamEnum.ThiefPowerupReturnProjectile;
		}
		else if (extraParam is ScoundrelBlindFireSequence.ConeExtraParams)
		{
			result = SequenceExtraParamEnum.ScoundrelBlindFireCone;
		}
		else if (extraParam is SimpleAttachedVFXSequence.ImpactDelayParams)
		{
			result = SequenceExtraParamEnum.ImpactDelayAttachedVFX;
		}
		else if (extraParam is SoldierProjectilesInLineSequence.HitAreaExtraParams)
		{
			result = SequenceExtraParamEnum.SoldierProjectilesInLineHitArea;
		}
		else if (extraParam is SimpleTimingSequence.ExtraParams)
		{
			result = SequenceExtraParamEnum.SimpleTiming;
		}
		else if (extraParam is NekoDiscReturnProjectileSequence.DiscReturnProjectileExtraParams)
		{
			result = SequenceExtraParamEnum.DiscReturnProjectile;
		}
		else if (extraParam is SplineProjectileSequence.ProjectilePropertyParams)
		{
			result = SequenceExtraParamEnum.SplineProjectileProperty;
		}
		else if (extraParam is Sequence.FxAttributeParam)
		{
			result = SequenceExtraParamEnum.SequenceFxAttribute;
		}
		else if (extraParam is Sequence.PhaseTimingExtraParams)
		{
			result = SequenceExtraParamEnum.SequencePhaseTiming;
		}
		else if (extraParam is ValkyrieDirectionalShieldSequence.ExtraParams)
		{
			result = SequenceExtraParamEnum.ValkyrieDirectionalShield;
		}
		else if (extraParam is Sequence.ActorIndexExtraParam)
		{
			result = SequenceExtraParamEnum.ActorIndexParam;
		}
		else if (extraParam is HitActorGroupOnAnimEventSequence.ActorParams)
		{
			result = SequenceExtraParamEnum.HitActorGroupActorsParam;
		}
		else if (extraParam is GrydCardinalBombSequence.SegmentExtraParams)
		{
			result = SequenceExtraParamEnum.GrydCardinalBombParam;
		}
		else if (extraParam is SimpleVFXAtTargetPosSequence.PositionOverrideParam)
		{
			result = SequenceExtraParamEnum.VFXSpawnPosOverride;
		}
		else if (extraParam is Sequence.GenericIntParam)
		{
			result = SequenceExtraParamEnum.GenericIntParam;
		}
		else if (extraParam is Sequence.GenericActorListParam)
		{
			result = SequenceExtraParamEnum.GenericActorListParam;
		}
		return result;
	}

	public Sequence.IExtraSequenceParams CreateExtraParamOfEnum(SequenceExtraParamEnum paramEnum)
	{
		Sequence.IExtraSequenceParams result = null;
		if (paramEnum == SequenceExtraParamEnum.Powerup)
		{
			result = new PowerUp.ExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.BouncingShot)
		{
			result = new BouncingShotSequence.ExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.Explosion)
		{
			result = new ExplosionSequence.ExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.GroundLine)
		{
			result = new GroundLineSequence.ExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.HealLaser)
		{
			result = new HealLaserSequence.ExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.HitOnAnimationEvent)
		{
			result = new HitOnAnimationEventSequence.ExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.NanosmithBoltLaser)
		{
			result = new NanosmithBoltLaserSequence.ExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.NinjaMultiAttack)
		{
			result = new NinjaMultiAttackSequence.ExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.ProximityMineGround)
		{
			result = new ProximityMineGroundSequence.ExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.DelayedProjectile)
		{
			result = new SplineProjectileSequence.DelayedProjectileExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.MultiEventProjectile)
		{
			result = new SplineProjectileSequence.MultiEventExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.MultiEventAttachedVFX)
		{
			result = new SimpleAttachedVFXSequence.MultiEventExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.ExoSweepLaser)
		{
			result = new ExoSweepLaserSequence.ExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.BlasterStretchingCone)
		{
			result = new BlasterStretchConeSequence.ExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.SimpleVFXAtTargetPos)
		{
			result = new SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam();
		}
		else if (paramEnum == SequenceExtraParamEnum.ThiefPowerupReturnProjectile)
		{
			result = new ThiefPowerupReturnProjectileSequence.PowerupTypeExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.ScoundrelBlindFireCone)
		{
			result = new ScoundrelBlindFireSequence.ConeExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.ImpactDelayAttachedVFX)
		{
			result = new SimpleAttachedVFXSequence.ImpactDelayParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.SoldierProjectilesInLineHitArea)
		{
			result = new SoldierProjectilesInLineSequence.HitAreaExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.SimpleTiming)
		{
			result = new SimpleTimingSequence.ExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.SplineProjectileProperty)
		{
			result = new SplineProjectileSequence.ProjectilePropertyParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.SequenceFxAttribute)
		{
			result = new Sequence.FxAttributeParam();
		}
		else if (paramEnum == SequenceExtraParamEnum.SequencePhaseTiming)
		{
			result = new Sequence.PhaseTimingExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.ValkyrieDirectionalShield)
		{
			result = new ValkyrieDirectionalShieldSequence.ExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.ActorIndexParam)
		{
			result = new Sequence.ActorIndexExtraParam();
		}
		else if (paramEnum == SequenceExtraParamEnum.HitActorGroupActorsParam)
		{
			result = new HitActorGroupOnAnimEventSequence.ActorParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.GrydCardinalBombParam)
		{
			result = new GrydCardinalBombSequence.SegmentExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.VFXSpawnPosOverride)
		{
			result = new SimpleVFXAtTargetPosSequence.PositionOverrideParam();
		}
		else if (paramEnum == SequenceExtraParamEnum.DiscReturnProjectile)
		{
			result = new NekoDiscReturnProjectileSequence.DiscReturnProjectileExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.GenericIntParam)
		{
			result = new Sequence.GenericIntParam();
		}
		else if (paramEnum == SequenceExtraParamEnum.GenericActorListParam)
		{
			result = new Sequence.GenericActorListParam();
		}
		return result;
	}
}
