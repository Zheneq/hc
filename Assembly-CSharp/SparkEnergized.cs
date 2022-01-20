using System.Collections.Generic;
using UnityEngine;

public class SparkEnergized : Ability
{
	[Header("-- Cast sequences (if Need To Select Target is true)")]
	public GameObject m_castOnAllySequence;

	public GameObject m_castOnEnemySequence;

	public GameObject m_castOnAllSequence;

	public StandardActorEffectData m_allyBuffEffect;

	public StandardActorEffectData m_enemyDebuffEffect;

	public int m_healAmtPerBeam;

	[Header("-- Whether to choose which target to select (need 1 TargetData entry if true, 0 otherwise)")]
	public bool m_needToSelectTarget = true;

	[Separator("Effect and Boosted Heal/Damage when both tethers are attached", true)]
	public StandardEffectInfo m_bothTetherExtraEffectOnSelf;

	public StandardEffectInfo m_bothTetherAllyEffect;

	public StandardEffectInfo m_bothTetherEnemyEffect;

	[Space(10f)]
	public int m_bothTetherExtraHeal;

	public int m_bothTetherExtraDamage;

	private AbilityMod_SparkEnergized m_abilityMod;

	private SparkBasicAttack m_damageBeamAbility;

	private SparkHealingBeam m_healBeamAbility;

	private SparkBeamTrackerComponent m_beamSyncComp;

	private bool m_cachedCanTargetSelf;

	private TargetData[] m_emptyTargetData = new TargetData[0];

	private StandardActorEffectData m_cachedAllyBuffEffect;

	private StandardActorEffectData m_cachedEnemyDebuffEffect;

	private StandardEffectInfo m_cachedBothTetherExtraEffectOnSelf;

	private StandardEffectInfo m_cachedBothTetherAllyEffect;

	private StandardEffectInfo m_cachedBothTetherEnemyEffect;

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		m_damageBeamAbility = GetComponent<SparkBasicAttack>();
		m_healBeamAbility = GetComponent<SparkHealingBeam>();
		m_beamSyncComp = GetComponent<SparkBeamTrackerComponent>();
		if (NeedToChooseActor())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, true, true, AbilityUtil_Targeter.AffectsActor.Always);
					return;
				}
			}
		}
		AbilityUtil_Targeter_AllVisible abilityUtil_Targeter_AllVisible = new AbilityUtil_Targeter_AllVisible(this, true, true, false, AbilityUtil_Targeter_AllVisible.DamageOriginType.TargetPos);
		abilityUtil_Targeter_AllVisible.SetAffectedGroups(true, true, m_cachedCanTargetSelf);
		abilityUtil_Targeter_AllVisible.m_shouldAddActorDelegate = delegate(ActorData potentialTarget, ActorData caster)
		{
			if (m_beamSyncComp != null)
			{
				if (potentialTarget != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							if (potentialTarget == caster)
							{
								return m_cachedCanTargetSelf;
							}
							return m_beamSyncComp.IsActorIndexTracked(potentialTarget.ActorIndex);
						}
					}
				}
			}
			return false;
		};
		base.Targeter = abilityUtil_Targeter_AllVisible;
	}

	public override TargetData[] GetTargetData()
	{
		if (NeedToChooseActor())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return base.GetTargetData();
				}
			}
		}
		return m_emptyTargetData;
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedAllyBuffEffect;
		if ((bool)m_abilityMod)
		{
			cachedAllyBuffEffect = m_abilityMod.m_allyBuffEffectMod.GetModifiedValue(m_allyBuffEffect);
		}
		else
		{
			cachedAllyBuffEffect = m_allyBuffEffect;
		}
		m_cachedAllyBuffEffect = cachedAllyBuffEffect;
		m_cachedEnemyDebuffEffect = ((!m_abilityMod) ? m_enemyDebuffEffect : m_abilityMod.m_enemyDebuffEffectMod.GetModifiedValue(m_enemyDebuffEffect));
		m_cachedCanTargetSelf = false;
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		if (GetHealAmtPerBeam() > 0)
		{
			goto IL_00d5;
		}
		if (moddedEffectForSelf != null)
		{
			if (moddedEffectForSelf.m_applyEffect)
			{
				if (moddedEffectForSelf.m_effectData.m_absorbAmount > 0)
				{
					goto IL_00d5;
				}
			}
		}
		goto IL_00dc;
		IL_00dc:
		StandardEffectInfo cachedBothTetherExtraEffectOnSelf;
		if ((bool)m_abilityMod)
		{
			cachedBothTetherExtraEffectOnSelf = m_abilityMod.m_bothTetherExtraEffectOnSelfMod.GetModifiedValue(m_bothTetherExtraEffectOnSelf);
		}
		else
		{
			cachedBothTetherExtraEffectOnSelf = m_bothTetherExtraEffectOnSelf;
		}
		m_cachedBothTetherExtraEffectOnSelf = cachedBothTetherExtraEffectOnSelf;
		StandardEffectInfo cachedBothTetherAllyEffect;
		if ((bool)m_abilityMod)
		{
			cachedBothTetherAllyEffect = m_abilityMod.m_bothTetherAllyEffectMod.GetModifiedValue(m_bothTetherAllyEffect);
		}
		else
		{
			cachedBothTetherAllyEffect = m_bothTetherAllyEffect;
		}
		m_cachedBothTetherAllyEffect = cachedBothTetherAllyEffect;
		m_cachedBothTetherEnemyEffect = ((!m_abilityMod) ? m_bothTetherEnemyEffect : m_abilityMod.m_bothTetherEnemyEffectMod.GetModifiedValue(m_bothTetherEnemyEffect));
		return;
		IL_00d5:
		m_cachedCanTargetSelf = true;
		goto IL_00dc;
	}

	public bool NeedToChooseActor()
	{
		return (!m_abilityMod) ? m_needToSelectTarget : m_abilityMod.m_needToChooseTargetMod.GetModifiedValue(m_needToSelectTarget);
	}

	public int CalcHealOnSelfPerTurn(int baseAmount)
	{
		return (!m_abilityMod) ? baseAmount : m_abilityMod.m_healOnSelfFromTetherMod.GetModifiedValue(baseAmount);
	}

	public int CalcEnergyOnSelfPerTurn(int baseAmount)
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyOnSelfFromTetherMod.GetModifiedValue(baseAmount);
		}
		else
		{
			result = baseAmount;
		}
		return result;
	}

	public int CalcAdditionalHealOnCast(int baseAmount)
	{
		return (!m_abilityMod) ? baseAmount : m_abilityMod.m_additionalHealMod.GetModifiedValue(baseAmount);
	}

	public int CalcAdditonalDamageOnCast(int baseAmount)
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_additionalDamageMod.GetModifiedValue(baseAmount);
		}
		else
		{
			result = baseAmount;
		}
		return result;
	}

	public bool HasEnemyEffectForTurnStart()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = (m_abilityMod.m_effectOnEnemyOnNextTurn.m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public StandardEffectInfo GetEnemyEffectForTurnStart()
	{
		object result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_effectOnEnemyOnNextTurn;
		}
		else
		{
			result = null;
		}
		return (StandardEffectInfo)result;
	}

	public StandardActorEffectData GetAllyBuffEffect()
	{
		return (m_cachedAllyBuffEffect == null) ? m_allyBuffEffect : m_cachedAllyBuffEffect;
	}

	public StandardActorEffectData GetEnemyDebuffEffect()
	{
		StandardActorEffectData result;
		if (m_cachedEnemyDebuffEffect != null)
		{
			result = m_cachedEnemyDebuffEffect;
		}
		else
		{
			result = m_enemyDebuffEffect;
		}
		return result;
	}

	public int GetHealAmtPerBeam()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healAmtPerBeamMod.GetModifiedValue(m_healAmtPerBeam);
		}
		else
		{
			result = m_healAmtPerBeam;
		}
		return result;
	}

	public StandardEffectInfo GetBothTetherExtraEffectOnSelf()
	{
		StandardEffectInfo result;
		if (m_cachedBothTetherExtraEffectOnSelf != null)
		{
			result = m_cachedBothTetherExtraEffectOnSelf;
		}
		else
		{
			result = m_bothTetherExtraEffectOnSelf;
		}
		return result;
	}

	public StandardEffectInfo GetBothTetherAllyEffect()
	{
		StandardEffectInfo result;
		if (m_cachedBothTetherAllyEffect != null)
		{
			result = m_cachedBothTetherAllyEffect;
		}
		else
		{
			result = m_bothTetherAllyEffect;
		}
		return result;
	}

	public StandardEffectInfo GetBothTetherEnemyEffect()
	{
		StandardEffectInfo result;
		if (m_cachedBothTetherEnemyEffect != null)
		{
			result = m_cachedBothTetherEnemyEffect;
		}
		else
		{
			result = m_bothTetherEnemyEffect;
		}
		return result;
	}

	public int GetBothTetherExtraHeal()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_bothTetherExtraHealMod.GetModifiedValue(m_bothTetherExtraHeal);
		}
		else
		{
			result = m_bothTetherExtraHeal;
		}
		return result;
	}

	public int GetBothTetherExtraDamage()
	{
		return (!m_abilityMod) ? m_bothTetherExtraDamage : m_abilityMod.m_bothTetherExtraDamageMod.GetModifiedValue(m_bothTetherExtraDamage);
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		if (GetDamageBeamAbility() != null)
		{
			if (animIndex == GetDamageBeamAbility().m_energizedPulseAnimIndex)
			{
				goto IL_0080;
			}
		}
		if (GetHealBeamAbility() != null)
		{
			if (animIndex == GetHealBeamAbility().m_energizedPulseAnimIndex)
			{
				goto IL_0080;
			}
		}
		int result = base.CanTriggerAnimAtIndexForTaunt(animIndex) ? 1 : 0;
		goto IL_0081;
		IL_0080:
		result = 1;
		goto IL_0081;
		IL_0081:
		return (byte)result != 0;
	}

	private SparkBasicAttack GetDamageBeamAbility()
	{
		if (m_damageBeamAbility == null)
		{
			m_damageBeamAbility = GetComponent<SparkBasicAttack>();
		}
		return m_damageBeamAbility;
	}

	private SparkHealingBeam GetHealBeamAbility()
	{
		if (m_healBeamAbility == null)
		{
			m_healBeamAbility = GetComponent<SparkHealingBeam>();
		}
		return m_healBeamAbility;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return caster.GetComponent<SparkBeamTrackerComponent>().BeamIsActive();
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (!NeedToChooseActor())
		{
			return true;
		}
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (!(boardSquareSafe == null))
		{
			if (boardSquareSafe.IsBaselineHeight())
			{
				List<ActorData> beamActors = caster.GetComponent<SparkBeamTrackerComponent>().GetBeamActors();
				List<BoardSquare> list = new List<BoardSquare>();
				using (List<ActorData>.Enumerator enumerator = beamActors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData current = enumerator.Current;
						if (current.GetCurrentBoardSquare() != null)
						{
							list.Add(current.GetCurrentBoardSquare());
						}
					}
				}
				return list.Contains(boardSquareSafe);
			}
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Self, 1);
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Ally, 1);
		AbilityTooltipHelper.ReportDamage(ref number, AbilityTooltipSubject.Enemy, 1);
		GetAllyBuffEffect().ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Ally);
		GetEnemyDebuffEffect().ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Enemy);
		AppendTooltipNumbersFromBaseModEffects(ref number);
		return number;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		ActorData actorData = base.ActorData;
		if (tooltipSubjectTypes != null)
		{
			if (m_beamSyncComp != null)
			{
				if (actorData != null)
				{
					SparkHealingBeam healBeamAbility = GetHealBeamAbility();
					SparkBasicAttack damageBeamAbility = GetDamageBeamAbility();
					int tetherAgeOnActor = m_beamSyncComp.GetTetherAgeOnActor(targetActor.ActorIndex);
					if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
					{
						if (damageBeamAbility != null)
						{
							int perTurnDamage = damageBeamAbility.GetPerTurnDamage();
							perTurnDamage += CalcAdditonalDamageOnCast(damageBeamAbility.GetAdditionalDamageOnRadiated());
							perTurnDamage += damageBeamAbility.GetBonusDamageFromTetherAge(tetherAgeOnActor);
							if (m_beamSyncComp.HasBothTethers())
							{
								perTurnDamage += GetBothTetherExtraDamage();
							}
							dictionary[AbilityTooltipSymbol.Damage] = perTurnDamage;
						}
					}
					else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
					{
						if (healBeamAbility != null)
						{
							int healOnAllyPerTurn = healBeamAbility.GetHealOnAllyPerTurn();
							healOnAllyPerTurn += CalcAdditionalHealOnCast(healBeamAbility.GetAdditionalHealOnRadiated());
							healOnAllyPerTurn += healBeamAbility.GetBonusHealFromTetherAge(tetherAgeOnActor);
							if (m_beamSyncComp.HasBothTethers())
							{
								healOnAllyPerTurn += GetBothTetherExtraHeal();
							}
							dictionary[AbilityTooltipSymbol.Healing] = healOnAllyPerTurn;
						}
					}
					else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
					{
						List<int> beamActorIndices = m_beamSyncComp.GetBeamActorIndices();
						int num = 0;
						int num2 = 0;
						foreach (int item in beamActorIndices)
						{
							ActorData actorData2 = GameFlowData.Get().FindActorByActorIndex(item);
							if (actorData2 != null)
							{
								if (actorData2.GetTeam() == actorData.GetTeam())
								{
									num++;
								}
								else
								{
									num2++;
								}
							}
						}
						int num4 = dictionary[AbilityTooltipSymbol.Healing] = (num + num2) * GetHealAmtPerBeam();
					}
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SparkEnergized abilityMod_SparkEnergized = modAsBase as AbilityMod_SparkEnergized;
		SparkBasicAttack component = GetComponent<SparkBasicAttack>();
		SparkHealingBeam component2 = GetComponent<SparkHealingBeam>();
		if (component != null)
		{
			AddTokenInt(tokens, "RadiatedDamage_Total", string.Empty, component.m_laserDamageAmount + component.m_additionalEnergizedDamage);
			AddTokenInt(tokens, "RadiatedDamage_Diff", string.Empty, component.m_additionalEnergizedDamage);
		}
		if (component2 != null)
		{
			AddTokenInt(tokens, "RadiatedHeal_Total", string.Empty, component2.m_laserHealingAmount + component2.m_additionalEnergizedHealing);
			AddTokenInt(tokens, "RadiatedHeal_Diff", string.Empty, component2.m_additionalEnergizedHealing);
		}
		StandardActorEffectData standardActorEffectData;
		if ((bool)abilityMod_SparkEnergized)
		{
			standardActorEffectData = abilityMod_SparkEnergized.m_allyBuffEffectMod.GetModifiedValue(m_allyBuffEffect);
		}
		else
		{
			standardActorEffectData = m_allyBuffEffect;
		}
		StandardActorEffectData standardActorEffectData2 = standardActorEffectData;
		standardActorEffectData2.AddTooltipTokens(tokens, "AllyBuffEffect", abilityMod_SparkEnergized != null, m_allyBuffEffect);
		StandardActorEffectData standardActorEffectData3 = (!abilityMod_SparkEnergized) ? m_enemyDebuffEffect : abilityMod_SparkEnergized.m_enemyDebuffEffectMod.GetModifiedValue(m_enemyDebuffEffect);
		standardActorEffectData3.AddTooltipTokens(tokens, "EnemyDebuffEffect", abilityMod_SparkEnergized != null, m_enemyDebuffEffect);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_SparkEnergized)
		{
			val = abilityMod_SparkEnergized.m_healAmtPerBeamMod.GetModifiedValue(m_healAmtPerBeam);
		}
		else
		{
			val = m_healAmtPerBeam;
		}
		AddTokenInt(tokens, "HealAmtPerBeam", empty, val);
		AbilityMod.AddToken_EffectInfo(tokens, m_bothTetherExtraEffectOnSelf, "BothTetherExtraEffectOnSelf", m_bothTetherExtraEffectOnSelf);
		AbilityMod.AddToken_EffectInfo(tokens, m_bothTetherAllyEffect, "BothTetherAllyEffect", m_bothTetherAllyEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_bothTetherEnemyEffect, "BothTetherEnemyEffect", m_bothTetherEnemyEffect);
		AddTokenInt(tokens, "BothTetherExtraHeal", string.Empty, m_bothTetherExtraHeal);
		AddTokenInt(tokens, "BothTetherExtraDamage", string.Empty, m_bothTetherExtraDamage);
	}

	public override List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = base.Debug_GetExpectedNumbersInTooltip();
		SparkBasicAttack component = GetComponent<SparkBasicAttack>();
		SparkHealingBeam component2 = GetComponent<SparkHealingBeam>();
		if (component != null)
		{
			list.Add(component.m_laserHitEffect.m_effectData.m_damagePerTurn + component.GetAdditionalDamageOnRadiated());
		}
		if (component2 != null)
		{
			list.Add(component2.m_laserHitEffect.m_effectData.m_healingPerTurn + component2.GetAdditionalHealOnRadiated());
		}
		return list;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SparkEnergized))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_SparkEnergized);
			Setup();
			ReinitTetherAbilities();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
		ReinitTetherAbilities();
	}

	private void ReinitTetherAbilities()
	{
		AbilityData component = GetComponent<AbilityData>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			SparkBasicAttack sparkBasicAttack = component.GetAbilityOfType(typeof(SparkBasicAttack)) as SparkBasicAttack;
			SparkHealingBeam sparkHealingBeam = component.GetAbilityOfType(typeof(SparkHealingBeam)) as SparkHealingBeam;
			if (sparkBasicAttack != null)
			{
				sparkBasicAttack.Setup();
				sparkBasicAttack.ResetNameplateTargetingNumbers();
			}
			if (sparkHealingBeam != null)
			{
				while (true)
				{
					sparkHealingBeam.Setup();
					sparkHealingBeam.ResetNameplateTargetingNumbers();
					return;
				}
			}
			return;
		}
	}
}
