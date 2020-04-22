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
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_sequencesLoaded[sequenceId];
				}
			}
		}
		object obj;
		if (m_sequences[sequenceId] != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
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
			switch (6)
			{
			case 0:
				continue;
			}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
		Debug.LogError("Did not find name hash of sequence prefab: " + sequencePrefab.name + ", please update sequence lookup");
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
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = SequenceExtraParamEnum.Powerup;
		}
		else if (extraParam is BouncingShotSequence.ExtraParams)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			result = SequenceExtraParamEnum.HealLaser;
		}
		else if (extraParam is HitOnAnimationEventSequence.ExtraParams)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			result = SequenceExtraParamEnum.HitOnAnimationEvent;
		}
		else if (extraParam is NanosmithBoltLaserSequence.ExtraParams)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			result = SequenceExtraParamEnum.NanosmithBoltLaser;
		}
		else if (extraParam is NinjaMultiAttackSequence.ExtraParams)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			result = SequenceExtraParamEnum.NinjaMultiAttack;
		}
		else if (extraParam is ProximityMineGroundSequence.ExtraParams)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			result = SequenceExtraParamEnum.SimpleVFXAtTargetPos;
		}
		else if (extraParam is ThiefPowerupReturnProjectileSequence.PowerupTypeExtraParams)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			result = SequenceExtraParamEnum.ThiefPowerupReturnProjectile;
		}
		else if (extraParam is ScoundrelBlindFireSequence.ConeExtraParams)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			result = SequenceExtraParamEnum.ScoundrelBlindFireCone;
		}
		else if (extraParam is SimpleAttachedVFXSequence.ImpactDelayParams)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			result = SequenceExtraParamEnum.SequenceFxAttribute;
		}
		else if (extraParam is Sequence.PhaseTimingExtraParams)
		{
			result = SequenceExtraParamEnum.SequencePhaseTiming;
		}
		else if (extraParam is ValkyrieDirectionalShieldSequence.ExtraParams)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			result = SequenceExtraParamEnum.ValkyrieDirectionalShield;
		}
		else if (extraParam is Sequence.ActorIndexExtraParam)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			result = SequenceExtraParamEnum.ActorIndexParam;
		}
		else if (extraParam is HitActorGroupOnAnimEventSequence.ActorParams)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			result = SequenceExtraParamEnum.HitActorGroupActorsParam;
		}
		else if (extraParam is GrydCardinalBombSequence.SegmentExtraParams)
		{
			result = SequenceExtraParamEnum.GrydCardinalBombParam;
		}
		else if (extraParam is SimpleVFXAtTargetPosSequence.PositionOverrideParam)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			result = SequenceExtraParamEnum.VFXSpawnPosOverride;
		}
		else if (extraParam is Sequence.GenericIntParam)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			result = SequenceExtraParamEnum.GenericIntParam;
		}
		else if (extraParam is Sequence.GenericActorListParam)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = new ExplosionSequence.ExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.GroundLine)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.HealLaser)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.HitOnAnimationEvent)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.NanosmithBoltLaser)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.NinjaMultiAttack)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.ProximityMineGround)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.DelayedProjectile)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.MultiEventProjectile)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.MultiEventAttachedVFX)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.ExoSweepLaser)
		{
			result = new ExoSweepLaserSequence.ExtraParams();
		}
		else if (paramEnum == SequenceExtraParamEnum.BlasterStretchingCone)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.SimpleVFXAtTargetPos)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.ThiefPowerupReturnProjectile)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.ScoundrelBlindFireCone)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.ImpactDelayAttachedVFX)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.SoldierProjectilesInLineHitArea)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.SimpleTiming)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.SplineProjectileProperty)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.SequenceFxAttribute)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.SequencePhaseTiming)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.ValkyrieDirectionalShield)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.ActorIndexParam)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.HitActorGroupActorsParam)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.GrydCardinalBombParam)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.VFXSpawnPosOverride)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.DiscReturnProjectile)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.GenericIntParam)
		{
			while (true)
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
		else if (paramEnum == SequenceExtraParamEnum.GenericActorListParam)
		{
			while (true)
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
}
