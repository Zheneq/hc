using System.Collections.Generic;
using UnityEngine;

public class Ninja360Attack : Ability
{
	public enum TargetingMode
	{
		Shape,
		Cone,
		Laser
	}

	[Separator("Targeting", true)]
	public TargetingMode m_targetingMode = TargetingMode.Laser;

	public bool m_penetrateLineOfSight;

	[Header("  (( if using Laser ))")]
	public LaserTargetingInfo m_laserInfo;

	[Header("  (( if using Cone ))")]
	public ConeTargetingInfo m_coneInfo;

	public float m_innerConeAngle;

	[Header("  (( if using Shape ))")]
	public AbilityAreaShape m_targeterShape = AbilityAreaShape.Three_x_Three;

	[Separator("On Hit", true)]
	public int m_damageAmount = 15;

	[Header("-- Damage for Inner Area Hit --")]
	public int m_innerAreaDamage = 30;

	[Space(10f)]
	public StandardEffectInfo m_enemyHitEffect;

	public bool m_useDifferentEffectForInnerCone;

	public StandardEffectInfo m_innerConeEnemyHitEffect;

	[Header("-- Energy Gain on Marked Target --")]
	public int m_energyGainOnMarkedHit;

	public int m_selfHealOnMarkedHit;

	[Separator("[Deathmark] Effect", "magenta")]
	public bool m_applyDeathmarkEffect = true;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private const int c_innerConeIdentifier = 1;

	private Ninja_SyncComponent m_syncComp;

	private AbilityMod_Ninja360Attack m_abilityMod;

	private LaserTargetingInfo m_cachedLaserInfo;

	private ConeTargetingInfo m_cachedConeInfo;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedInnerConeEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Ninja Basic Attack";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Ninja_SyncComponent>();
		}
		if (m_targetingMode == TargetingMode.Laser)
		{
			LaserTargetingInfo laserInfo = GetLaserInfo();
			base.Targeter = new AbilityUtil_Targeter_Laser(this, laserInfo.width, laserInfo.range, PenetrateLineOfSight(), laserInfo.maxTargets);
			return;
		}
		if (m_targetingMode == TargetingMode.Cone)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					ConeTargetingInfo coneInfo = GetConeInfo();
					float radiusInSquares = coneInfo.m_radiusInSquares;
					List<AbilityUtil_Targeter_MultipleCones.ConeDimensions> list = new List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>();
					list.Add(new AbilityUtil_Targeter_MultipleCones.ConeDimensions(coneInfo.m_widthAngleDeg, radiusInSquares));
					if (GetInnerConeAngle() > 0f)
					{
						list.Add(new AbilityUtil_Targeter_MultipleCones.ConeDimensions(GetInnerConeAngle(), radiusInSquares));
					}
					AbilityUtil_Targeter_MultipleCones abilityUtil_Targeter_MultipleCones = new AbilityUtil_Targeter_MultipleCones(this, list, coneInfo.m_backwardsOffset, PenetrateLineOfSight(), true, true, false, GetSelfHealOnMarkedHit() > 0);
					if (GetSelfHealOnMarkedHit() > 0)
					{
						abilityUtil_Targeter_MultipleCones.m_affectCasterDelegate = IncludeCasterForTargeter;
					}
					base.Targeter = abilityUtil_Targeter_MultipleCones;
					return;
				}
				}
			}
		}
		base.Targeter = new AbilityUtil_Targeter_Shape(this, GetTargeterShape(), PenetrateLineOfSight());
	}

	private bool IncludeCasterForTargeter(ActorData caster, List<ActorData> addedSoFar)
	{
		if (GetSelfHealOnMarkedHit() > 0)
		{
			for (int i = 0; i < addedSoFar.Count; i++)
			{
				if (!IsActorMarked(addedSoFar[i]))
				{
					continue;
				}
				while (true)
				{
					return true;
				}
			}
		}
		return false;
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Design --</color>\nPlease edit [Deathmark] info on Ninja sync component.\n<color=cyan>-- For Art --</color>\nOn Sequence, for HitActorGroupOnAnimEventSequence components, use:\n" + 1 + " for Inner cone group identifier\n";
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		if (m_targetingMode == TargetingMode.Laser)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return GetLaserInfo().range;
				}
			}
		}
		if (m_targetingMode == TargetingMode.Cone)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return GetConeInfo().m_radiusInSquares;
				}
			}
		}
		return 0f;
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo cachedLaserInfo;
		if ((bool)m_abilityMod)
		{
			cachedLaserInfo = m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo);
		}
		else
		{
			cachedLaserInfo = m_laserInfo;
		}
		m_cachedLaserInfo = cachedLaserInfo;
		m_cachedConeInfo = ((!m_abilityMod) ? m_coneInfo : m_abilityMod.m_coneInfoMod.GetModifiedValue(m_coneInfo));
		StandardEffectInfo cachedEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedEnemyHitEffect = m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = m_enemyHitEffect;
		}
		m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		StandardEffectInfo cachedInnerConeEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedInnerConeEnemyHitEffect = m_abilityMod.m_innerConeEnemyHitEffectMod.GetModifiedValue(m_innerConeEnemyHitEffect);
		}
		else
		{
			cachedInnerConeEnemyHitEffect = m_innerConeEnemyHitEffect;
		}
		m_cachedInnerConeEnemyHitEffect = cachedInnerConeEnemyHitEffect;
	}

	public bool PenetrateLineOfSight()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight);
		}
		else
		{
			result = m_penetrateLineOfSight;
		}
		return result;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (m_cachedLaserInfo != null)
		{
			result = m_cachedLaserInfo;
		}
		else
		{
			result = m_laserInfo;
		}
		return result;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		return (m_cachedConeInfo == null) ? m_coneInfo : m_cachedConeInfo;
	}

	public float GetInnerConeAngle()
	{
		return (!m_abilityMod) ? m_innerConeAngle : m_abilityMod.m_innerConeAngleMod.GetModifiedValue(m_innerConeAngle);
	}

	public AbilityAreaShape GetTargeterShape()
	{
		AbilityAreaShape result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_targeterShapeMod.GetModifiedValue(m_targeterShape);
		}
		else
		{
			result = m_targeterShape;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			result = m_damageAmount;
		}
		return result;
	}

	public int GetInnerAreaDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_innerAreaDamageMod.GetModifiedValue(m_innerAreaDamage);
		}
		else
		{
			result = m_innerAreaDamage;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffect != null)
		{
			result = m_cachedEnemyHitEffect;
		}
		else
		{
			result = m_enemyHitEffect;
		}
		return result;
	}

	public bool UseDifferentEffectForInnerCone()
	{
		return (!m_abilityMod) ? m_useDifferentEffectForInnerCone : m_abilityMod.m_useDifferentEffectForInnerConeMod.GetModifiedValue(m_useDifferentEffectForInnerCone);
	}

	public StandardEffectInfo GetInnerConeEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedInnerConeEnemyHitEffect != null)
		{
			result = m_cachedInnerConeEnemyHitEffect;
		}
		else
		{
			result = m_innerConeEnemyHitEffect;
		}
		return result;
	}

	public int GetEnergyGainOnMarkedHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyGainOnMarkedHitMod.GetModifiedValue(m_energyGainOnMarkedHit);
		}
		else
		{
			result = m_energyGainOnMarkedHit;
		}
		return result;
	}

	public int GetSelfHealOnMarkedHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_selfHealOnMarkedHitMod.GetModifiedValue(m_selfHealOnMarkedHit);
		}
		else
		{
			result = m_selfHealOnMarkedHit;
		}
		return result;
	}

	public bool ApplyDeathmarkEffect()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_applyDeathmarkEffectMod.GetModifiedValue(m_applyDeathmarkEffect);
		}
		else
		{
			result = m_applyDeathmarkEffect;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageAmount());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetSelfHealOnMarkedHit());
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (m_targetingMode == TargetingMode.Cone)
		{
			if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
			{
				while (true)
				{
					int damage = GetDamageAmount();
					if (GetInnerAreaDamage() > 0 && GetInnerConeAngle() > 0f)
					{
						ActorData actorData = base.ActorData;
						Vector3 lastUpdateAimDir = base.Targeter.LastUpdateAimDir;
						float coneForwardAngle = VectorUtils.HorizontalAngle_Deg(lastUpdateAimDir);
						if (IsActorInInnerCone(targetActor, actorData, coneForwardAngle))
						{
							damage = GetInnerAreaDamage();
						}
					}
					results.m_damage = damage;
					return true;
				}
			}
			if (targetActor == base.ActorData && GetSelfHealOnMarkedHit() > 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						int num = 0;
						List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
						for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
						{
							ActorData targetActor2 = visibleActorsInRangeByTooltipSubject[i];
							if (IsActorMarked(targetActor2))
							{
								num += GetSelfHealOnMarkedHit();
							}
						}
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								results.m_healing = num;
								return true;
							}
						}
					}
					}
				}
			}
		}
		return false;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetEnergyGainOnMarkedHit() > 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					int num = 0;
					List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
					for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
					{
						ActorData targetActor = visibleActorsInRangeByTooltipSubject[i];
						if (IsActorMarked(targetActor))
						{
							num += GetEnergyGainOnMarkedHit();
						}
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return num;
						}
					}
				}
				}
			}
		}
		return 0;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (symbolType == AbilityTooltipSymbol.Damage)
		{
			if (m_syncComp != null)
			{
				if (m_syncComp.m_deathmarkOnTriggerDamage > 0)
				{
					if (IsActorMarked(targetActor))
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return "\n+ " + AbilityUtils.CalculateDamageForTargeter(base.ActorData, targetActor, this, m_syncComp.m_deathmarkOnTriggerDamage, false);
							}
						}
					}
				}
			}
		}
		return null;
	}

	public bool IsActorInInnerCone(ActorData targetActor, ActorData caster, float coneForwardAngle)
	{
		if (m_targetingMode == TargetingMode.Cone)
		{
			if (GetInnerConeAngle() > 0f)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						ConeTargetingInfo coneInfo = GetConeInfo();
						return AreaEffectUtils.IsSquareInConeByActorRadius(targetActor.GetCurrentBoardSquare(), caster.GetFreePos(), coneForwardAngle, GetInnerConeAngle(), coneInfo.m_radiusInSquares, coneInfo.m_backwardsOffset, PenetrateLineOfSight(), caster);
					}
					}
				}
			}
		}
		return false;
	}

	public bool IsActorMarked(ActorData targetActor)
	{
		int result;
		if (m_syncComp != null)
		{
			result = (m_syncComp.ActorHasDeathmark(targetActor) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "InnerAreaDamage", string.Empty, m_innerAreaDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_innerConeEnemyHitEffect, "InnerConeEnemyHitEffect", m_innerConeEnemyHitEffect);
		AddTokenInt(tokens, "EnergyGainOnMarkedHit", string.Empty, m_energyGainOnMarkedHit);
		AddTokenInt(tokens, "SelfHealOnMarkedHit", string.Empty, m_selfHealOnMarkedHit);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_Ninja360Attack))
		{
			m_abilityMod = (abilityMod as AbilityMod_Ninja360Attack);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
