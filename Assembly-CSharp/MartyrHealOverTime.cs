using System.Collections.Generic;
using UnityEngine;

public class MartyrHealOverTime : Ability
{
	[Header("-- Targeting --")]
	public bool m_canTargetAlly = true;

	public bool m_targetingPenetrateLos;

	public int m_healBase = 5;

	public int m_healPerCrystal = 3;

	[Header("  (( base effect data for healing, no need to specify healing here ))")]
	public StandardActorEffectData m_healEffectData;

	[Header("-- Extra healing if has Aoe on React effect")]
	public int m_extraHealingIfHasAoeOnReact;

	[Header("-- Extra Effect for low health --")]
	public bool m_onlyAddExtraEffecForFirstTurn;

	public float m_lowHealthThreshold;

	public StandardEffectInfo m_extraEffectForLowHealth;

	[Header("-- Heal/Effect on Caster if targeting Ally")]
	public int m_baseSelfHealIfTargetAlly;

	public int m_selfHealPerCrystalIfTargetAlly;

	public bool m_addHealEffectOnSelfIfTargetAlly;

	public StandardActorEffectData m_healEffectOnSelfIfTargetAlly;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private Martyr_SyncComponent m_syncComponent;

	private AbilityMod_MartyrHealOverTime m_abilityMod;

	private StandardActorEffectData m_cachedHealEffectData;

	private StandardEffectInfo m_cachedExtraEffectForLowHealth;

	private StandardActorEffectData m_cachedHealEffectOnSelfIfTargetAlly;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			m_abilityName = "MartyrHealOverTime";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_syncComponent == null)
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
			m_syncComponent = GetComponent<Martyr_SyncComponent>();
		}
		SetCachedFields();
		int num;
		if (HasSelfHitIfTargetingAlly())
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
			num = 2;
		}
		else
		{
			num = 1;
		}
		AbilityUtil_Targeter.AffectsActor affectsCaster = (AbilityUtil_Targeter.AffectsActor)num;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, CanTargetAlly(), affectsCaster);
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedHealEffectData;
		if ((bool)m_abilityMod)
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
			cachedHealEffectData = m_abilityMod.m_healEffectDataMod.GetModifiedValue(m_healEffectData);
		}
		else
		{
			cachedHealEffectData = m_healEffectData;
		}
		m_cachedHealEffectData = cachedHealEffectData;
		StandardEffectInfo cachedExtraEffectForLowHealth;
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
			cachedExtraEffectForLowHealth = m_abilityMod.m_extraEffectForLowHealthMod.GetModifiedValue(m_extraEffectForLowHealth);
		}
		else
		{
			cachedExtraEffectForLowHealth = m_extraEffectForLowHealth;
		}
		m_cachedExtraEffectForLowHealth = cachedExtraEffectForLowHealth;
		m_cachedHealEffectOnSelfIfTargetAlly = ((!m_abilityMod) ? m_healEffectOnSelfIfTargetAlly : m_abilityMod.m_healEffectOnSelfIfTargetAllyMod.GetModifiedValue(m_healEffectOnSelfIfTargetAlly));
	}

	public bool CanTargetAlly()
	{
		bool result;
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
			result = m_abilityMod.m_canTargetAllyMod.GetModifiedValue(m_canTargetAlly);
		}
		else
		{
			result = m_canTargetAlly;
		}
		return result;
	}

	public bool TargetingPenetrateLos()
	{
		return (!m_abilityMod) ? m_targetingPenetrateLos : m_abilityMod.m_targetingPenetrateLosMod.GetModifiedValue(m_targetingPenetrateLos);
	}

	public StandardActorEffectData GetHealEffectData()
	{
		StandardActorEffectData result;
		if (m_cachedHealEffectData != null)
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
			result = m_cachedHealEffectData;
		}
		else
		{
			result = m_healEffectData;
		}
		return result;
	}

	public int GetHealBase()
	{
		return (!m_abilityMod) ? m_healBase : m_abilityMod.m_healBaseMod.GetModifiedValue(m_healBase);
	}

	public int GetHealPerCrystal()
	{
		return (!m_abilityMod) ? m_healPerCrystal : m_abilityMod.m_healPerCrystalMod.GetModifiedValue(m_healPerCrystal);
	}

	public int GetExtraHealingIfHasAoeOnReact()
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
			result = m_abilityMod.m_extraHealingIfHasAoeOnReactMod.GetModifiedValue(m_extraHealingIfHasAoeOnReact);
		}
		else
		{
			result = m_extraHealingIfHasAoeOnReact;
		}
		return result;
	}

	public bool OnlyAddExtraEffecForFirstTurn()
	{
		return (!m_abilityMod) ? m_onlyAddExtraEffecForFirstTurn : m_abilityMod.m_onlyAddExtraEffecForFirstTurnMod.GetModifiedValue(m_onlyAddExtraEffecForFirstTurn);
	}

	public float GetLowHealthThreshold()
	{
		float result;
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
			result = m_abilityMod.m_lowHealthThresholdMod.GetModifiedValue(m_lowHealthThreshold);
		}
		else
		{
			result = m_lowHealthThreshold;
		}
		return result;
	}

	public StandardEffectInfo GetExtraEffectForLowHealth()
	{
		StandardEffectInfo result;
		if (m_cachedExtraEffectForLowHealth != null)
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
			result = m_cachedExtraEffectForLowHealth;
		}
		else
		{
			result = m_extraEffectForLowHealth;
		}
		return result;
	}

	public int GetBaseSelfHealIfTargetAlly()
	{
		return (!m_abilityMod) ? m_baseSelfHealIfTargetAlly : m_abilityMod.m_baseSelfHealIfTargetAllyMod.GetModifiedValue(m_baseSelfHealIfTargetAlly);
	}

	public int GetSelfHealPerCrystalIfTargetAlly()
	{
		int result;
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
			result = m_abilityMod.m_selfHealPerCrystalIfTargetAllyMod.GetModifiedValue(m_selfHealPerCrystalIfTargetAlly);
		}
		else
		{
			result = m_selfHealPerCrystalIfTargetAlly;
		}
		return result;
	}

	public bool AddHealEffectOnSelfIfTargetAlly()
	{
		bool result;
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
			result = m_abilityMod.m_addHealEffectOnSelfIfTargetAllyMod.GetModifiedValue(m_addHealEffectOnSelfIfTargetAlly);
		}
		else
		{
			result = m_addHealEffectOnSelfIfTargetAlly;
		}
		return result;
	}

	public StandardActorEffectData GetHealEffectOnSelfIfTargetAlly()
	{
		StandardActorEffectData result;
		if (m_cachedHealEffectOnSelfIfTargetAlly != null)
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
			result = m_cachedHealEffectOnSelfIfTargetAlly;
		}
		else
		{
			result = m_healEffectOnSelfIfTargetAlly;
		}
		return result;
	}

	public int GetCurrentHealing(ActorData caster)
	{
		return GetHealBase() + GetHealPerCrystal() * m_syncComponent.SpentDamageCrystals(caster);
	}

	public int GetSelfHealingIfTargetingAlly(ActorData caster)
	{
		int num = GetBaseSelfHealIfTargetAlly();
		if (GetSelfHealPerCrystalIfTargetAlly() > 0)
		{
			num += GetSelfHealPerCrystalIfTargetAlly() * m_syncComponent.SpentDamageCrystals(caster);
		}
		return num;
	}

	public bool HasSelfHitIfTargetingAlly()
	{
		int result;
		if (GetBaseSelfHealIfTargetAlly() <= 0)
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
			if (GetSelfHealPerCrystalIfTargetAlly() <= 0)
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
				result = (AddHealEffectOnSelfIfTargetAlly() ? 1 : 0);
				goto IL_003c;
			}
		}
		result = 1;
		goto IL_003c;
		IL_003c:
		return (byte)result != 0;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(caster, currentBestActorTarget, false, CanTargetAlly(), true, ValidateCheckPath.Ignore, TargetingPenetrateLos(), true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Primary, 1);
		AbilityTooltipHelper.ReportAbsorb(ref number, AbilityTooltipSubject.Primary, 1);
		return number;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
			ActorData actorData = base.ActorData;
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
			if (actorData != null)
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
				if (actorData == targetActor)
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
						int num = GetSelfHealingIfTargetingAlly(actorData);
						if (m_syncComponent != null)
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
							if (m_syncComponent.ActorHasAoeOnReactEffect(targetActor))
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
								if (GetExtraHealingIfHasAoeOnReact() > 0)
								{
									num += GetExtraHealingIfHasAoeOnReact();
								}
							}
						}
						dictionary[AbilityTooltipSymbol.Healing] = num;
						dictionary[AbilityTooltipSymbol.Absorb] = 0;
						goto IL_01b6;
					}
				}
			}
			int num2 = GetCurrentHealing(actorData);
			if (m_syncComponent != null)
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
				if (m_syncComponent.ActorHasAoeOnReactEffect(targetActor))
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
					if (GetExtraHealingIfHasAoeOnReact() > 0)
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
						num2 += GetExtraHealingIfHasAoeOnReact();
					}
				}
			}
			dictionary[AbilityTooltipSymbol.Healing] = num2;
			dictionary[AbilityTooltipSymbol.Absorb] = 0;
			if (GetLowHealthThreshold() > 0f)
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
				if (targetActor.GetHitPointShareOfMax() <= GetLowHealthThreshold())
				{
					StandardEffectInfo extraEffectForLowHealth = GetExtraEffectForLowHealth();
					if (extraEffectForLowHealth.m_applyEffect)
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
						dictionary[AbilityTooltipSymbol.Absorb] = extraEffectForLowHealth.m_effectData.m_absorbAmount;
					}
				}
			}
		}
		goto IL_01b6;
		IL_01b6:
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_MartyrHealOverTime abilityMod_MartyrHealOverTime = modAsBase as AbilityMod_MartyrHealOverTime;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_MartyrHealOverTime)
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
			val = abilityMod_MartyrHealOverTime.m_healBaseMod.GetModifiedValue(m_healBase);
		}
		else
		{
			val = m_healBase;
		}
		AddTokenInt(tokens, "HealBase", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_MartyrHealOverTime)
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
			val2 = abilityMod_MartyrHealOverTime.m_healPerCrystalMod.GetModifiedValue(m_healPerCrystal);
		}
		else
		{
			val2 = m_healPerCrystal;
		}
		AddTokenInt(tokens, "HealPerCrystal", empty2, val2);
		StandardActorEffectData standardActorEffectData;
		if ((bool)abilityMod_MartyrHealOverTime)
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
			standardActorEffectData = abilityMod_MartyrHealOverTime.m_healEffectDataMod.GetModifiedValue(m_healEffectData);
		}
		else
		{
			standardActorEffectData = m_healEffectData;
		}
		StandardActorEffectData standardActorEffectData2 = standardActorEffectData;
		standardActorEffectData2.AddTooltipTokens(tokens, "HealEffectData", abilityMod_MartyrHealOverTime != null, m_healEffectData);
		AddTokenInt(tokens, "ExtraHealingIfHasAoeOnReact", string.Empty, (!abilityMod_MartyrHealOverTime) ? m_extraHealingIfHasAoeOnReact : abilityMod_MartyrHealOverTime.m_extraHealingIfHasAoeOnReactMod.GetModifiedValue(m_extraHealingIfHasAoeOnReact));
		AddTokenFloatAsPct(tokens, "LowHealthThreshold_Pct", string.Empty, (!abilityMod_MartyrHealOverTime) ? m_lowHealthThreshold : abilityMod_MartyrHealOverTime.m_lowHealthThresholdMod.GetModifiedValue(m_lowHealthThreshold));
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_MartyrHealOverTime)
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
			effectInfo = abilityMod_MartyrHealOverTime.m_extraEffectForLowHealthMod.GetModifiedValue(m_extraEffectForLowHealth);
		}
		else
		{
			effectInfo = m_extraEffectForLowHealth;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "ExtraEffectForLowHealth", m_extraEffectForLowHealth);
		AddTokenInt(tokens, "BaseSelfHealIfTargetAlly", string.Empty, (!abilityMod_MartyrHealOverTime) ? m_baseSelfHealIfTargetAlly : abilityMod_MartyrHealOverTime.m_baseSelfHealIfTargetAllyMod.GetModifiedValue(m_baseSelfHealIfTargetAlly));
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_MartyrHealOverTime)
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
			val3 = abilityMod_MartyrHealOverTime.m_selfHealPerCrystalIfTargetAllyMod.GetModifiedValue(m_selfHealPerCrystalIfTargetAlly);
		}
		else
		{
			val3 = m_selfHealPerCrystalIfTargetAlly;
		}
		AddTokenInt(tokens, "SelfHealPerCrystalIfTargetAlly", empty3, val3);
		StandardActorEffectData standardActorEffectData3;
		if ((bool)abilityMod_MartyrHealOverTime)
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
			standardActorEffectData3 = abilityMod_MartyrHealOverTime.m_healEffectOnSelfIfTargetAllyMod.GetModifiedValue(m_healEffectOnSelfIfTargetAlly);
		}
		else
		{
			standardActorEffectData3 = m_healEffectOnSelfIfTargetAlly;
		}
		StandardActorEffectData standardActorEffectData4 = standardActorEffectData3;
		standardActorEffectData4.AddTooltipTokens(tokens, "HealEffectOnSelfIfTargetAlly", abilityMod_MartyrHealOverTime != null, m_healEffectOnSelfIfTargetAlly);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_MartyrHealOverTime))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_MartyrHealOverTime);
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
