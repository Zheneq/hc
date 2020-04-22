using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArcherShieldGeneratorArrow : Ability
{
	[Header("-- Ground effect")]
	public bool m_penetrateLoS;

	public bool m_affectsEnemies;

	public bool m_affectsAllies;

	public bool m_affectsCaster;

	public int m_lessAbsorbPerTurn = 5;

	public StandardGroundEffectInfo m_groundEffectInfo;

	public StandardEffectInfo m_directHitEnemyEffect;

	public StandardEffectInfo m_directHitAllyEffect;

	[Header("-- Extra effect for shielding that last different number of turns from main effect, etc")]
	public StandardEffectInfo m_extraAllyHitEffect;

	[Header("-- Sequences -------------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ArcherShieldGeneratorArrow m_abilityMod;

	private Archer_SyncComponent m_syncComp;

	private StandardGroundEffectInfo m_cachedGroundEffect;

	private StandardEffectInfo m_cachedDirectHitEnemyEffect;

	private StandardEffectInfo m_cachedDirectHitAllyEffect;

	private StandardEffectInfo m_cachedExtraAllyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Shield Generator Arrow";
		}
		m_syncComp = GetComponent<Archer_SyncComponent>();
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, GetGroundEffectInfo().m_groundEffectData.shape, PenetrateLoS(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, AffectsEnemies(), AffectsAllies(), AffectsCaster() ? AbilityUtil_Targeter.AffectsActor.Possible : AbilityUtil_Targeter.AffectsActor.Never);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ArcherShieldGeneratorArrow))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_ArcherShieldGeneratorArrow);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	private void SetCachedFields()
	{
		m_cachedGroundEffect = m_groundEffectInfo;
		StandardEffectInfo cachedDirectHitEnemyEffect;
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
			cachedDirectHitEnemyEffect = m_abilityMod.m_directHitEnemyEffectMod.GetModifiedValue(m_directHitEnemyEffect);
		}
		else
		{
			cachedDirectHitEnemyEffect = m_directHitEnemyEffect;
		}
		m_cachedDirectHitEnemyEffect = cachedDirectHitEnemyEffect;
		StandardEffectInfo cachedDirectHitAllyEffect;
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
			cachedDirectHitAllyEffect = m_abilityMod.m_directHitAllyEffectMod.GetModifiedValue(m_directHitAllyEffect);
		}
		else
		{
			cachedDirectHitAllyEffect = m_directHitAllyEffect;
		}
		m_cachedDirectHitAllyEffect = cachedDirectHitAllyEffect;
		StandardEffectInfo cachedExtraAllyHitEffect;
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
			cachedExtraAllyHitEffect = m_abilityMod.m_extraAllyHitEffectMod.GetModifiedValue(m_extraAllyHitEffect);
		}
		else
		{
			cachedExtraAllyHitEffect = m_extraAllyHitEffect;
		}
		m_cachedExtraAllyHitEffect = cachedExtraAllyHitEffect;
	}

	public bool PenetrateLoS()
	{
		bool result;
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
			result = m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS);
		}
		else
		{
			result = m_penetrateLoS;
		}
		return result;
	}

	public bool AffectsEnemies()
	{
		bool result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_affectsEnemiesMod.GetModifiedValue(m_affectsEnemies);
		}
		else
		{
			result = m_affectsEnemies;
		}
		return result;
	}

	public bool AffectsAllies()
	{
		bool result;
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
			result = m_abilityMod.m_affectsAlliesMod.GetModifiedValue(m_affectsAllies);
		}
		else
		{
			result = m_affectsAllies;
		}
		return result;
	}

	public bool AffectsCaster()
	{
		bool result;
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
			result = m_abilityMod.m_affectsCasterMod.GetModifiedValue(m_affectsCaster);
		}
		else
		{
			result = m_affectsCaster;
		}
		return result;
	}

	private StandardGroundEffectInfo GetGroundEffectInfo()
	{
		StandardGroundEffectInfo result;
		if (m_cachedGroundEffect != null)
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
			result = m_cachedGroundEffect;
		}
		else
		{
			result = m_groundEffectInfo;
		}
		return result;
	}

	public int GetLessAbsorbPerTurn()
	{
		return (!m_abilityMod) ? m_lessAbsorbPerTurn : m_abilityMod.m_lessAbsorbPerTurnMod.GetModifiedValue(m_lessAbsorbPerTurn);
	}

	public StandardEffectInfo GetDirectHitEnemyEffect()
	{
		StandardEffectInfo result;
		if (m_cachedDirectHitEnemyEffect != null)
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
			result = m_cachedDirectHitEnemyEffect;
		}
		else
		{
			result = m_directHitEnemyEffect;
		}
		return result;
	}

	public StandardEffectInfo GetDirectHitAllyEffect()
	{
		return (m_cachedDirectHitAllyEffect == null) ? m_directHitAllyEffect : m_cachedDirectHitAllyEffect;
	}

	public StandardEffectInfo GetExtraAllyHitEffect()
	{
		return (m_cachedExtraAllyHitEffect == null) ? m_extraAllyHitEffect : m_cachedExtraAllyHitEffect;
	}

	public int GetCooldownReductionOnDash()
	{
		return m_abilityMod ? m_abilityMod.m_cooldownReductionOnDash.GetModifiedValue(0) : 0;
	}

	public int GetExtraAbsorbPerEnemyHit()
	{
		int result;
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
			result = m_abilityMod.m_extraAbsorbPerEnemyHit.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraAbsorbIfEnemyHit()
	{
		return m_abilityMod ? m_abilityMod.m_extraAbsorbIfEnemyHit.GetModifiedValue(0) : 0;
	}

	public int GetExtraAbsorbIfOnlyOneAllyHit()
	{
		int result;
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
			result = m_abilityMod.m_extraAbsorbIfOnlyOneAllyHit.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "LessAbsorbPerTurn", string.Empty, m_lessAbsorbPerTurn);
		AbilityMod.AddToken_EffectInfo(tokens, m_directHitEnemyEffect, "DirectHitEnemyEffect", m_directHitEnemyEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_directHitAllyEffect, "DirectHitAllyEffect", m_directHitAllyEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_extraAllyHitEffect, "ExtraAllyHitEffect", m_extraAllyHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_groundEffectInfo.m_applyGroundEffect)
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
			m_groundEffectInfo.m_groundEffectData.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally);
		}
		if (AffectsAllies())
		{
			GetDirectHitAllyEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		}
		if (AffectsCaster())
		{
			GetDirectHitAllyEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		}
		if (AffectsEnemies())
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
			GetDirectHitEnemyEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (m_syncComp != null)
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
			if (targetActor.GetTeam() == base.ActorData.GetTeam())
			{
				int num = m_syncComp.m_extraAbsorbForShieldGenerator;
				List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeters[currentTargeterIndex].GetActorsInRange();
				if (!actorsInRange.IsNullOrEmpty())
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
					int num2 = actorsInRange.Count((AbilityUtil_Targeter.ActorTarget t) => t.m_actor.GetTeam() != base.ActorData.GetTeam());
					if (actorsInRange.Count - num2 == 1)
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
						num += GetExtraAbsorbIfOnlyOneAllyHit();
					}
					num += GetExtraAbsorbPerEnemyHit() * num2;
					if (num2 > 0)
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
						num += GetExtraAbsorbIfEnemyHit();
					}
				}
				int num3 = GetDirectHitAllyEffect().m_effectData.m_absorbAmount + num;
				StandardEffectInfo extraAllyHitEffect = GetExtraAllyHitEffect();
				if (extraAllyHitEffect.m_applyEffect)
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
					if (extraAllyHitEffect.m_effectData.m_absorbAmount > 0)
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
						num3 += extraAllyHitEffect.m_effectData.m_absorbAmount;
					}
				}
				dictionary[AbilityTooltipSymbol.Absorb] = num3;
			}
		}
		return dictionary;
	}
}
