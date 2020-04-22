using System.Collections.Generic;
using UnityEngine;

public class SparkHealingBeam : Ability
{
	public enum TargetingMode
	{
		BoardSquare,
		Laser
	}

	[Header("-- Targeting")]
	public TargetingMode m_targetingMode;

	[Header("-- Targeting: If Using Laser targeting mode")]
	public LaserTargetingInfo m_laserInfo;

	public StandardEffectInfo m_laserHitEffect;

	[Separator("-- Tether --", true)]
	public float m_tetherDistance = 5f;

	public bool m_checkTetherRemovalBetweenPhases;

	[Header("-- Tether Duration")]
	public int m_tetherDuration;

	[Header("-- Healing")]
	public int m_laserHealingAmount;

	public int m_additionalEnergizedHealing = 2;

	public int m_healOnSelfOnTick;

	public bool m_healSelfOnInitialAttach;

	public AbilityPriority m_healingPhase = AbilityPriority.Prep_Offense;

	[Header("-- Energy on Caster Per Turn")]
	public int m_energyOnCasterPerTurn = 3;

	[Header("-- Animation on Pulse")]
	public int m_pulseAnimIndex;

	public int m_energizedPulseAnimIndex;

	[Header("-- Sequences")]
	public GameObject m_castSequence;

	public GameObject m_pulseSequence;

	public GameObject m_energizedPulseSequence;

	public GameObject m_beamSequence;

	public GameObject m_targetPersistentSequence;

	private AbilityMod_SparkHealingBeam m_abilityMod;

	private SparkEnergized m_energizedAbility;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardActorEffectData m_cachedAllyEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Spark Healing Beam";
		}
		Setup();
	}

	public void Setup()
	{
		SetCachedFields();
		if (m_energizedAbility == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityData component = GetComponent<AbilityData>();
			if (component != null)
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
				m_energizedAbility = (component.GetAbilityOfType(typeof(SparkEnergized)) as SparkEnergized);
			}
		}
		if (base.Targeter != null)
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
			base.Targeter.ResetTargeter(true);
		}
		bool flag = m_healSelfOnInitialAttach && GetHealOnSelfPerTurn() > 0;
		if (m_targetingMode == TargetingMode.Laser)
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
			AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = new AbilityUtil_Targeter_Laser(this, GetLaserInfo());
			abilityUtil_Targeter_Laser.SetAffectedGroups(false, true, flag);
			abilityUtil_Targeter_Laser.m_affectCasterDelegate = ((ActorData caster, List<ActorData> actorsSoFar) => actorsSoFar.Count > 0);
			base.Targeter = abilityUtil_Targeter_Laser;
		}
		if (m_targetingMode != 0)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			int num;
			if (flag)
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
				num = 1;
			}
			else
			{
				num = 0;
			}
			AbilityUtil_Targeter.AffectsActor affectsCaster = (AbilityUtil_Targeter.AffectsActor)num;
			AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, true, affectsCaster);
			abilityUtil_Targeter_Shape.SetAffectedGroups(false, true, flag);
			abilityUtil_Targeter_Shape.m_affectCasterDelegate = ((ActorData caster, List<ActorData> actorsSoFar, bool casterInShape) => actorsSoFar.Count > 0);
			base.Targeter = abilityUtil_Targeter_Shape;
			return;
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserInfo().range;
	}

	public int GetHealOnAllyPerTurn()
	{
		return GetAllyTetherEffectData().m_healingPerTurn;
	}

	public int GetHealingOnAttach()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_initialHealingMod.GetModifiedValue(m_laserHealingAmount);
		}
		else
		{
			result = m_laserHealingAmount;
		}
		return result;
	}

	public int GetAdditionalHealOnRadiated()
	{
		int result;
		if ((bool)m_abilityMod)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_additionalHealOnRadiatedMod.GetModifiedValue(m_additionalEnergizedHealing);
		}
		else
		{
			result = m_additionalEnergizedHealing;
		}
		return result;
	}

	public int GetEnergyOnCasterPerTurn()
	{
		int num;
		if ((bool)m_abilityMod)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = m_abilityMod.m_energyOnCasterPerTurnMod.GetModifiedValue(m_energyOnCasterPerTurn);
		}
		else
		{
			num = m_energyOnCasterPerTurn;
		}
		int num2 = num;
		if (m_energizedAbility != null)
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
			num2 = m_energizedAbility.CalcEnergyOnSelfPerTurn(num2);
		}
		return num2;
	}

	public int GetHealOnSelfPerTurn()
	{
		int num = (!m_abilityMod) ? m_healOnSelfOnTick : m_abilityMod.m_healOnCasterOnTickMod.GetModifiedValue(m_healOnSelfOnTick);
		if (m_energizedAbility != null)
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
			num = m_energizedAbility.CalcHealOnSelfPerTurn(num);
		}
		return num;
	}

	public float GetTetherDistance()
	{
		float result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_tetherDistanceMod.GetModifiedValue(m_tetherDistance);
		}
		else
		{
			result = m_tetherDistance;
		}
		return result;
	}

	public int GetTetherDuration()
	{
		int result;
		if (m_abilityMod != null)
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
			result = m_abilityMod.m_tetherDurationMod.GetModifiedValue(m_tetherDuration);
		}
		else
		{
			result = m_tetherDuration;
		}
		return result;
	}

	public bool UseBonusHealing()
	{
		return (bool)m_abilityMod && m_abilityMod.m_useBonusHealOverTime;
	}

	public int GetBonusHealGrowRate()
	{
		return m_abilityMod ? m_abilityMod.m_bonusAllyHealIncreaseRate.GetModifiedValue(0) : 0;
	}

	public int GetMaxBonusHealing()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_maxAllyBonusHealAmount.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetBonusHealFromTetherAge(int age)
	{
		int num = 0;
		if (UseBonusHealing())
		{
			int maxBonusHealing = GetMaxBonusHealing();
			int bonusHealGrowRate = GetBonusHealGrowRate();
			if (bonusHealGrowRate > 0)
			{
				num = age * bonusHealGrowRate;
			}
			if (maxBonusHealing > 0)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num = Mathf.Min(maxBonusHealing, num);
			}
		}
		return num;
	}

	public bool ShouldApplyTargetEffectForXDamage()
	{
		int result;
		if (GetXDamageThreshold() > 0)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = ((GetTargetEffectForXDamage() != null) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public int GetXDamageThreshold()
	{
		return (!m_abilityMod) ? (-1) : m_abilityMod.m_xDamageThreshold;
	}

	public StandardEffectInfo GetTargetEffectForXDamage()
	{
		object result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_effectOnTargetForTakingXDamage;
		}
		else
		{
			result = null;
		}
		return (StandardEffectInfo)result;
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo laserInfo = m_laserInfo;
		object mod;
		if ((bool)m_abilityMod)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			mod = m_abilityMod.m_laserInfoMod;
		}
		else
		{
			mod = null;
		}
		m_cachedLaserInfo = laserInfo.GetModifiedCopy((AbilityModPropertyLaserInfo)mod);
		StandardEffectInfo standardEffectInfo = (!m_abilityMod) ? m_laserHitEffect.GetShallowCopy() : m_abilityMod.m_tetherBaseEffectOverride.GetModifiedValue(m_laserHitEffect);
		m_cachedAllyEffect = standardEffectInfo.m_effectData;
		m_cachedAllyEffect.m_sequencePrefabs = new GameObject[2]
		{
			m_targetPersistentSequence,
			m_beamSequence
		};
	}

	public StandardActorEffectData GetAllyTetherEffectData()
	{
		StandardActorEffectData result;
		if (m_cachedAllyEffect != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedAllyEffect;
		}
		else
		{
			result = m_laserHitEffect.m_effectData;
		}
		return result;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (m_cachedLaserInfo != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedLaserInfo;
		}
		else
		{
			result = m_laserInfo;
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (m_targetingMode == TargetingMode.Laser)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return true;
				}
			}
		}
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		object obj;
		if ((bool)boardSquareSafe)
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
			obj = boardSquareSafe.OccupantActor;
		}
		else
		{
			obj = null;
		}
		ActorData targetActor = (ActorData)obj;
		return CanTargetActorInDecision(caster, targetActor, false, true, false, ValidateCheckPath.Ignore, true, false);
	}

	public override bool CustomCanCastValidation(ActorData caster)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return true;
				}
			}
		}
		bool flag = false;
		TargetingParadigm targetingParadigm = GetTargetingParadigm(0);
		if (targetingParadigm != TargetingParadigm.BoardSquare)
		{
			if (targetingParadigm != TargetingParadigm.Position)
			{
				return true;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return HasTargetableActorsInDecision(caster, false, true, false, ValidateCheckPath.Ignore, true, false);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Heal_FirstTurn", "damage on initial attach", m_laserHealingAmount);
		AddTokenInt(tokens, "Heal_PerTurnAfterFirst", "damage on subsequent turns", m_laserHitEffect.m_effectData.m_healingPerTurn);
		AddTokenInt(tokens, "Heal_AdditionalOnRadiated", "additional damage on radiated", m_additionalEnergizedHealing);
		AddTokenInt(tokens, "Heal_OnCasterPerTurn", "heal on caster per turn", m_healOnSelfOnTick);
		AddTokenInt(tokens, "EnergyOnCasterPerTurn", string.Empty, m_energyOnCasterPerTurn);
		AddTokenInt(tokens, "TetherDuration", string.Empty, m_tetherDuration);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		int healingOnAttach = GetHealingOnAttach();
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Ally, healingOnAttach);
		if (m_healSelfOnInitialAttach)
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
			if (GetHealOnSelfPerTurn() > 0)
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
				AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Self, GetHealOnSelfPerTurn());
			}
		}
		return number;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
		int result;
		if (visibleActorsCountByTooltipSubject > 0)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = GetEnergyOnCasterPerTurn();
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public override List<int> _001D()
	{
		List<int> list = base._001D();
		list.Add(m_laserHitEffect.m_effectData.m_healingPerTurn);
		return list;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SparkHealingBeam))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_SparkHealingBeam);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
