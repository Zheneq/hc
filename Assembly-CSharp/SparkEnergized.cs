using System;
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
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		this.m_damageBeamAbility = base.GetComponent<SparkBasicAttack>();
		this.m_healBeamAbility = base.GetComponent<SparkHealingBeam>();
		this.m_beamSyncComp = base.GetComponent<SparkBeamTrackerComponent>();
		if (this.NeedToChooseActor())
		{
			base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, true, true, AbilityUtil_Targeter.AffectsActor.Always, AbilityUtil_Targeter.AffectsActor.Possible);
		}
		else
		{
			AbilityUtil_Targeter_AllVisible abilityUtil_Targeter_AllVisible = new AbilityUtil_Targeter_AllVisible(this, true, true, false, AbilityUtil_Targeter_AllVisible.DamageOriginType.TargetPos);
			abilityUtil_Targeter_AllVisible.SetAffectedGroups(true, true, this.m_cachedCanTargetSelf);
			abilityUtil_Targeter_AllVisible.m_shouldAddActorDelegate = delegate(ActorData potentialTarget, ActorData caster)
			{
				if (this.m_beamSyncComp != null)
				{
					if (potentialTarget != null)
					{
						if (potentialTarget == caster)
						{
							return this.m_cachedCanTargetSelf;
						}
						return this.m_beamSyncComp.IsActorIndexTracked(potentialTarget.ActorIndex);
					}
				}
				return false;
			};
			base.Targeter = abilityUtil_Targeter_AllVisible;
		}
	}

	public override TargetData[] GetTargetData()
	{
		if (this.NeedToChooseActor())
		{
			return base.GetTargetData();
		}
		return this.m_emptyTargetData;
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedAllyBuffEffect;
		if (this.m_abilityMod)
		{
			cachedAllyBuffEffect = this.m_abilityMod.m_allyBuffEffectMod.GetModifiedValue(this.m_allyBuffEffect);
		}
		else
		{
			cachedAllyBuffEffect = this.m_allyBuffEffect;
		}
		this.m_cachedAllyBuffEffect = cachedAllyBuffEffect;
		this.m_cachedEnemyDebuffEffect = ((!this.m_abilityMod) ? this.m_enemyDebuffEffect : this.m_abilityMod.m_enemyDebuffEffectMod.GetModifiedValue(this.m_enemyDebuffEffect));
		this.m_cachedCanTargetSelf = false;
		StandardEffectInfo moddedEffectForSelf = base.GetModdedEffectForSelf();
		if (this.GetHealAmtPerBeam() <= 0)
		{
			if (moddedEffectForSelf == null)
			{
				goto IL_DC;
			}
			if (!moddedEffectForSelf.m_applyEffect)
			{
				goto IL_DC;
			}
			if (moddedEffectForSelf.m_effectData.m_absorbAmount <= 0)
			{
				goto IL_DC;
			}
		}
		this.m_cachedCanTargetSelf = true;
		IL_DC:
		StandardEffectInfo cachedBothTetherExtraEffectOnSelf;
		if (this.m_abilityMod)
		{
			cachedBothTetherExtraEffectOnSelf = this.m_abilityMod.m_bothTetherExtraEffectOnSelfMod.GetModifiedValue(this.m_bothTetherExtraEffectOnSelf);
		}
		else
		{
			cachedBothTetherExtraEffectOnSelf = this.m_bothTetherExtraEffectOnSelf;
		}
		this.m_cachedBothTetherExtraEffectOnSelf = cachedBothTetherExtraEffectOnSelf;
		StandardEffectInfo cachedBothTetherAllyEffect;
		if (this.m_abilityMod)
		{
			cachedBothTetherAllyEffect = this.m_abilityMod.m_bothTetherAllyEffectMod.GetModifiedValue(this.m_bothTetherAllyEffect);
		}
		else
		{
			cachedBothTetherAllyEffect = this.m_bothTetherAllyEffect;
		}
		this.m_cachedBothTetherAllyEffect = cachedBothTetherAllyEffect;
		this.m_cachedBothTetherEnemyEffect = ((!this.m_abilityMod) ? this.m_bothTetherEnemyEffect : this.m_abilityMod.m_bothTetherEnemyEffectMod.GetModifiedValue(this.m_bothTetherEnemyEffect));
	}

	public bool NeedToChooseActor()
	{
		return (!this.m_abilityMod) ? this.m_needToSelectTarget : this.m_abilityMod.m_needToChooseTargetMod.GetModifiedValue(this.m_needToSelectTarget);
	}

	public int CalcHealOnSelfPerTurn(int baseAmount)
	{
		return (!this.m_abilityMod) ? baseAmount : this.m_abilityMod.m_healOnSelfFromTetherMod.GetModifiedValue(baseAmount);
	}

	public int CalcEnergyOnSelfPerTurn(int baseAmount)
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_energyOnSelfFromTetherMod.GetModifiedValue(baseAmount);
		}
		else
		{
			result = baseAmount;
		}
		return result;
	}

	public int CalcAdditionalHealOnCast(int baseAmount)
	{
		return (!this.m_abilityMod) ? baseAmount : this.m_abilityMod.m_additionalHealMod.GetModifiedValue(baseAmount);
	}

	public int CalcAdditonalDamageOnCast(int baseAmount)
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_additionalDamageMod.GetModifiedValue(baseAmount);
		}
		else
		{
			result = baseAmount;
		}
		return result;
	}

	public bool HasEnemyEffectForTurnStart()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_effectOnEnemyOnNextTurn.m_applyEffect;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyEffectForTurnStart()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_effectOnEnemyOnNextTurn;
		}
		else
		{
			result = null;
		}
		return result;
	}

	public StandardActorEffectData GetAllyBuffEffect()
	{
		return (this.m_cachedAllyBuffEffect == null) ? this.m_allyBuffEffect : this.m_cachedAllyBuffEffect;
	}

	public StandardActorEffectData GetEnemyDebuffEffect()
	{
		StandardActorEffectData result;
		if (this.m_cachedEnemyDebuffEffect != null)
		{
			result = this.m_cachedEnemyDebuffEffect;
		}
		else
		{
			result = this.m_enemyDebuffEffect;
		}
		return result;
	}

	public int GetHealAmtPerBeam()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_healAmtPerBeamMod.GetModifiedValue(this.m_healAmtPerBeam);
		}
		else
		{
			result = this.m_healAmtPerBeam;
		}
		return result;
	}

	public StandardEffectInfo GetBothTetherExtraEffectOnSelf()
	{
		StandardEffectInfo result;
		if (this.m_cachedBothTetherExtraEffectOnSelf != null)
		{
			result = this.m_cachedBothTetherExtraEffectOnSelf;
		}
		else
		{
			result = this.m_bothTetherExtraEffectOnSelf;
		}
		return result;
	}

	public StandardEffectInfo GetBothTetherAllyEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedBothTetherAllyEffect != null)
		{
			result = this.m_cachedBothTetherAllyEffect;
		}
		else
		{
			result = this.m_bothTetherAllyEffect;
		}
		return result;
	}

	public StandardEffectInfo GetBothTetherEnemyEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedBothTetherEnemyEffect != null)
		{
			result = this.m_cachedBothTetherEnemyEffect;
		}
		else
		{
			result = this.m_bothTetherEnemyEffect;
		}
		return result;
	}

	public int GetBothTetherExtraHeal()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_bothTetherExtraHealMod.GetModifiedValue(this.m_bothTetherExtraHeal);
		}
		else
		{
			result = this.m_bothTetherExtraHeal;
		}
		return result;
	}

	public int GetBothTetherExtraDamage()
	{
		return (!this.m_abilityMod) ? this.m_bothTetherExtraDamage : this.m_abilityMod.m_bothTetherExtraDamageMod.GetModifiedValue(this.m_bothTetherExtraDamage);
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		if (this.GetDamageBeamAbility() != null)
		{
			if (animIndex == this.GetDamageBeamAbility().m_energizedPulseAnimIndex)
			{
				goto IL_80;
			}
		}
		if (this.GetHealBeamAbility() != null)
		{
			if (animIndex == this.GetHealBeamAbility().m_energizedPulseAnimIndex)
			{
				goto IL_80;
			}
		}
		return base.CanTriggerAnimAtIndexForTaunt(animIndex);
		IL_80:
		return true;
	}

	private SparkBasicAttack GetDamageBeamAbility()
	{
		if (this.m_damageBeamAbility == null)
		{
			this.m_damageBeamAbility = base.GetComponent<SparkBasicAttack>();
		}
		return this.m_damageBeamAbility;
	}

	private SparkHealingBeam GetHealBeamAbility()
	{
		if (this.m_healBeamAbility == null)
		{
			this.m_healBeamAbility = base.GetComponent<SparkHealingBeam>();
		}
		return this.m_healBeamAbility;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return caster.GetComponent<SparkBeamTrackerComponent>().BeamIsActive();
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (!this.NeedToChooseActor())
		{
			return true;
		}
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
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
						ActorData actorData = enumerator.Current;
						if (actorData.GetCurrentBoardSquare() != null)
						{
							list.Add(actorData.GetCurrentBoardSquare());
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
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, 1);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, 1);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, 1);
		this.GetAllyBuffEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		this.GetEnemyDebuffEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		base.AppendTooltipNumbersFromBaseModEffects(ref result, AbilityTooltipSubject.Primary, AbilityTooltipSubject.Ally, AbilityTooltipSubject.Self);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		ActorData actorData = base.ActorData;
		if (tooltipSubjectTypes != null)
		{
			if (this.m_beamSyncComp != null)
			{
				if (actorData != null)
				{
					SparkHealingBeam healBeamAbility = this.GetHealBeamAbility();
					SparkBasicAttack damageBeamAbility = this.GetDamageBeamAbility();
					int tetherAgeOnActor = this.m_beamSyncComp.GetTetherAgeOnActor(targetActor.ActorIndex);
					if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
					{
						if (damageBeamAbility != null)
						{
							int num = damageBeamAbility.GetPerTurnDamage();
							num += this.CalcAdditonalDamageOnCast(damageBeamAbility.GetAdditionalDamageOnRadiated());
							num += damageBeamAbility.GetBonusDamageFromTetherAge(tetherAgeOnActor);
							if (this.m_beamSyncComp.HasBothTethers())
							{
								num += this.GetBothTetherExtraDamage();
							}
							dictionary[AbilityTooltipSymbol.Damage] = num;
						}
					}
					else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
					{
						if (healBeamAbility != null)
						{
							int num2 = healBeamAbility.GetHealOnAllyPerTurn();
							num2 += this.CalcAdditionalHealOnCast(healBeamAbility.GetAdditionalHealOnRadiated());
							num2 += healBeamAbility.GetBonusHealFromTetherAge(tetherAgeOnActor);
							if (this.m_beamSyncComp.HasBothTethers())
							{
								num2 += this.GetBothTetherExtraHeal();
							}
							dictionary[AbilityTooltipSymbol.Healing] = num2;
						}
					}
					else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
					{
						List<int> beamActorIndices = this.m_beamSyncComp.GetBeamActorIndices();
						int num3 = 0;
						int num4 = 0;
						foreach (int actorIndex in beamActorIndices)
						{
							ActorData actorData2 = GameFlowData.Get().FindActorByActorIndex(actorIndex);
							if (actorData2 != null)
							{
								if (actorData2.GetTeam() == actorData.GetTeam())
								{
									num3++;
								}
								else
								{
									num4++;
								}
							}
						}
						int value = (num3 + num4) * this.GetHealAmtPerBeam();
						dictionary[AbilityTooltipSymbol.Healing] = value;
					}
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SparkEnergized abilityMod_SparkEnergized = modAsBase as AbilityMod_SparkEnergized;
		SparkBasicAttack component = base.GetComponent<SparkBasicAttack>();
		SparkHealingBeam component2 = base.GetComponent<SparkHealingBeam>();
		if (component != null)
		{
			base.AddTokenInt(tokens, "RadiatedDamage_Total", string.Empty, component.m_laserDamageAmount + component.m_additionalEnergizedDamage, false);
			base.AddTokenInt(tokens, "RadiatedDamage_Diff", string.Empty, component.m_additionalEnergizedDamage, false);
		}
		if (component2 != null)
		{
			base.AddTokenInt(tokens, "RadiatedHeal_Total", string.Empty, component2.m_laserHealingAmount + component2.m_additionalEnergizedHealing, false);
			base.AddTokenInt(tokens, "RadiatedHeal_Diff", string.Empty, component2.m_additionalEnergizedHealing, false);
		}
		StandardActorEffectData standardActorEffectData;
		if (abilityMod_SparkEnergized)
		{
			standardActorEffectData = abilityMod_SparkEnergized.m_allyBuffEffectMod.GetModifiedValue(this.m_allyBuffEffect);
		}
		else
		{
			standardActorEffectData = this.m_allyBuffEffect;
		}
		StandardActorEffectData standardActorEffectData2 = standardActorEffectData;
		standardActorEffectData2.AddTooltipTokens(tokens, "AllyBuffEffect", abilityMod_SparkEnergized != null, this.m_allyBuffEffect);
		StandardActorEffectData standardActorEffectData3 = (!abilityMod_SparkEnergized) ? this.m_enemyDebuffEffect : abilityMod_SparkEnergized.m_enemyDebuffEffectMod.GetModifiedValue(this.m_enemyDebuffEffect);
		standardActorEffectData3.AddTooltipTokens(tokens, "EnemyDebuffEffect", abilityMod_SparkEnergized != null, this.m_enemyDebuffEffect);
		string name = "HealAmtPerBeam";
		string empty = string.Empty;
		int val;
		if (abilityMod_SparkEnergized)
		{
			val = abilityMod_SparkEnergized.m_healAmtPerBeamMod.GetModifiedValue(this.m_healAmtPerBeam);
		}
		else
		{
			val = this.m_healAmtPerBeam;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_bothTetherExtraEffectOnSelf, "BothTetherExtraEffectOnSelf", this.m_bothTetherExtraEffectOnSelf, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_bothTetherAllyEffect, "BothTetherAllyEffect", this.m_bothTetherAllyEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_bothTetherEnemyEffect, "BothTetherEnemyEffect", this.m_bothTetherEnemyEffect, true);
		base.AddTokenInt(tokens, "BothTetherExtraHeal", string.Empty, this.m_bothTetherExtraHeal, false);
		base.AddTokenInt(tokens, "BothTetherExtraDamage", string.Empty, this.m_bothTetherExtraDamage, false);
	}

	public override List<int> symbol_001D()
	{
		List<int> list = base.symbol_001D();
		SparkBasicAttack component = base.GetComponent<SparkBasicAttack>();
		SparkHealingBeam component2 = base.GetComponent<SparkHealingBeam>();
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
		if (abilityMod.GetType() == typeof(AbilityMod_SparkEnergized))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SparkEnergized);
			this.Setup();
			this.ReinitTetherAbilities();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
		this.ReinitTetherAbilities();
	}

	private void ReinitTetherAbilities()
	{
		AbilityData component = base.GetComponent<AbilityData>();
		if (component != null)
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
				sparkHealingBeam.Setup();
				sparkHealingBeam.ResetNameplateTargetingNumbers();
			}
		}
	}
}
