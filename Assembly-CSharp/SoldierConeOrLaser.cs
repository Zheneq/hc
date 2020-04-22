using System.Collections.Generic;
using UnityEngine;

public class SoldierConeOrLaser : Ability
{
	public enum LastUsedModeFlag
	{
		None,
		Cone,
		Laser
	}

	[Separator("Targeting", true)]
	public float m_coneDistThreshold = 4f;

	[Header("  Targeting: For Cone")]
	public ConeTargetingInfo m_coneInfo;

	[Header("  Targeting: For Laser")]
	public LaserTargetingInfo m_laserInfo;

	[Separator("On Hit", true)]
	public int m_coneDamage = 10;

	public StandardEffectInfo m_coneEnemyHitEffect;

	[Space(10f)]
	public int m_laserDamage = 20;

	public StandardEffectInfo m_laserEnemyHitEffect;

	[Separator("Extra Damage", true)]
	public int m_extraDamageForAlternating;

	[Space(5f)]
	public float m_closeDistThreshold = -1f;

	public int m_extraDamageForNearTarget;

	public int m_extraDamageForFromCover;

	[Space(5f)]
	public int m_extraDamageToEvaders;

	[Separator("Extra Energy (per target hit)", true)]
	public int m_extraEnergyForCone;

	public int m_extraEnergyForLaser;

	[Separator("Animation Indices", true)]
	public int m_onCastLaserAnimIndex = 1;

	public int m_onCastConeAnimIndex = 6;

	[Separator("Sequences", true)]
	public GameObject m_coneSequencePrefab;

	public GameObject m_laserSequencePrefab;

	public AbilityMod_SoldierConeOrLaser m_abilityMod;

	private Soldier_SyncComponent m_syncComp;

	private AbilityData m_abilityData;

	private SoldierStimPack m_stimAbility;

	private ConeTargetingInfo m_cachedConeInfo;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardEffectInfo m_cachedConeEnemyHitEffect;

	private StandardEffectInfo m_cachedLaserEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			m_abilityName = "Soldier Cone Or Laser";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Soldier_SyncComponent>();
		}
		if (m_abilityData == null)
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
			m_abilityData = GetComponent<AbilityData>();
		}
		if (m_abilityData != null && m_stimAbility == null)
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
			m_stimAbility = (m_abilityData.GetAbilityOfType(typeof(SoldierStimPack)) as SoldierStimPack);
		}
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_ConeOrLaser(this, GetConeInfo(), GetLaserInfo(), m_coneDistThreshold);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserInfo().range;
	}

	private void SetCachedFields()
	{
		ConeTargetingInfo cachedConeInfo;
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
			cachedConeInfo = m_abilityMod.m_coneInfoMod.GetModifiedValue(m_coneInfo);
		}
		else
		{
			cachedConeInfo = m_coneInfo;
		}
		m_cachedConeInfo = cachedConeInfo;
		LaserTargetingInfo cachedLaserInfo;
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
			cachedLaserInfo = m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo);
		}
		else
		{
			cachedLaserInfo = m_laserInfo;
		}
		m_cachedLaserInfo = cachedLaserInfo;
		StandardEffectInfo cachedConeEnemyHitEffect;
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
			cachedConeEnemyHitEffect = m_abilityMod.m_coneEnemyHitEffectMod.GetModifiedValue(m_coneEnemyHitEffect);
		}
		else
		{
			cachedConeEnemyHitEffect = m_coneEnemyHitEffect;
		}
		m_cachedConeEnemyHitEffect = cachedConeEnemyHitEffect;
		StandardEffectInfo cachedLaserEnemyHitEffect;
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
			cachedLaserEnemyHitEffect = m_abilityMod.m_laserEnemyHitEffectMod.GetModifiedValue(m_laserEnemyHitEffect);
		}
		else
		{
			cachedLaserEnemyHitEffect = m_laserEnemyHitEffect;
		}
		m_cachedLaserEnemyHitEffect = cachedLaserEnemyHitEffect;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		ConeTargetingInfo result;
		if (m_cachedConeInfo != null)
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
			result = m_cachedConeInfo;
		}
		else
		{
			result = m_coneInfo;
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
			result = m_cachedLaserInfo;
		}
		else
		{
			result = m_laserInfo;
		}
		return result;
	}

	public int GetConeDamage()
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
			result = m_abilityMod.m_coneDamageMod.GetModifiedValue(m_coneDamage);
		}
		else
		{
			result = m_coneDamage;
		}
		return result;
	}

	public StandardEffectInfo GetConeEnemyHitEffect()
	{
		return (m_cachedConeEnemyHitEffect == null) ? m_coneEnemyHitEffect : m_cachedConeEnemyHitEffect;
	}

	public int GetLaserDamage()
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
			result = m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamage);
		}
		else
		{
			result = m_laserDamage;
		}
		return result;
	}

	public StandardEffectInfo GetLaserEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedLaserEnemyHitEffect != null)
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
			result = m_cachedLaserEnemyHitEffect;
		}
		else
		{
			result = m_laserEnemyHitEffect;
		}
		return result;
	}

	public int GetExtraDamageForAlternating()
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
			result = m_abilityMod.m_extraDamageForAlternatingMod.GetModifiedValue(m_extraDamageForAlternating);
		}
		else
		{
			result = m_extraDamageForAlternating;
		}
		return result;
	}

	public float GetCloseDistThreshold()
	{
		return (!m_abilityMod) ? m_closeDistThreshold : m_abilityMod.m_closeDistThresholdMod.GetModifiedValue(m_closeDistThreshold);
	}

	public int GetExtraDamageForNearTarget()
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
			result = m_abilityMod.m_extraDamageForNearTargetMod.GetModifiedValue(m_extraDamageForNearTarget);
		}
		else
		{
			result = m_extraDamageForNearTarget;
		}
		return result;
	}

	public int GetExtraDamageForFromCover()
	{
		return (!m_abilityMod) ? m_extraDamageForFromCover : m_abilityMod.m_extraDamageForFromCoverMod.GetModifiedValue(m_extraDamageForFromCover);
	}

	public int GetExtraDamageToEvaders()
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
			result = m_abilityMod.m_extraDamageToEvadersMod.GetModifiedValue(m_extraDamageToEvaders);
		}
		else
		{
			result = m_extraDamageToEvaders;
		}
		return result;
	}

	public int GetExtraEnergyForCone()
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
			result = m_abilityMod.m_extraEnergyForConeMod.GetModifiedValue(m_extraEnergyForCone);
		}
		else
		{
			result = m_extraEnergyForCone;
		}
		return result;
	}

	public int GetExtraEnergyForLaser()
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
			result = m_abilityMod.m_extraEnergyForLaserMod.GetModifiedValue(m_extraEnergyForLaser);
		}
		else
		{
			result = m_extraEnergyForLaser;
		}
		return result;
	}

	public bool ShouldUseExtraDamageForNearTarget(ActorData target, ActorData caster)
	{
		if (GetExtraDamageForNearTarget() > 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Vector3 vector = target.GetTravelBoardSquareWorldPosition() - caster.GetTravelBoardSquareWorldPosition();
					vector.y = 0f;
					return vector.magnitude < GetCloseDistThreshold() * Board.Get().squareSize;
				}
				}
			}
		}
		return false;
	}

	public bool HasConeDamageMod()
	{
		int result;
		if (m_abilityMod != null)
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
			result = ((m_abilityMod.m_coneDamageMod.operation != AbilityModPropertyInt.ModOp.Ignore) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool HasLaserDamageMod()
	{
		int result;
		if (m_abilityMod != null)
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
			result = ((m_abilityMod.m_laserDamageMod.operation != AbilityModPropertyInt.ModOp.Ignore) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool HasNearDistThresholdMod()
	{
		int result;
		if (m_abilityMod != null)
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
			result = ((m_abilityMod.m_closeDistThresholdMod.operation != AbilityModPropertyFloat.ModOp.Ignore) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool HasExtraDamageForNearTargetMod()
	{
		int result;
		if (m_abilityMod != null)
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
			result = ((m_abilityMod.m_extraDamageForNearTargetMod.operation != AbilityModPropertyInt.ModOp.Ignore) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool HasConeEnergyMod()
	{
		return m_abilityMod != null && m_abilityMod.m_extraEnergyForConeMod.operation != AbilityModPropertyInt.ModOp.Ignore;
	}

	public bool HasLaserEnergyMod()
	{
		return m_abilityMod != null && m_abilityMod.m_extraEnergyForLaserMod.operation != AbilityModPropertyInt.ModOp.Ignore;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetConeDamage());
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetLaserDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
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
				dictionary = new Dictionary<AbilityTooltipSymbol, int>();
				int num = 0;
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
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
					num = GetConeDamage();
					if (GetExtraDamageForAlternating() > 0)
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
						if ((bool)m_syncComp)
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
							if (m_syncComp.m_lastPrimaryUsedMode == 2)
							{
								num += GetExtraDamageForAlternating();
							}
						}
					}
				}
				else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
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
					num = GetLaserDamage();
					if (GetExtraDamageForAlternating() > 0)
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
						if ((bool)m_syncComp && m_syncComp.m_lastPrimaryUsedMode == 1)
						{
							num += GetExtraDamageForAlternating();
						}
					}
				}
				ActorData actorData = base.ActorData;
				if (actorData != null)
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
					if (ShouldUseExtraDamageForNearTarget(targetActor, actorData))
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
						num += GetExtraDamageForNearTarget();
					}
					if (GetExtraDamageForFromCover() > 0)
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
						if (actorData.GetActorCover().HasAnyCover())
						{
							num += GetExtraDamageForFromCover();
						}
					}
				}
				dictionary[AbilityTooltipSymbol.Damage] = num;
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if ((GetExtraEnergyForCone() > 0 || GetExtraEnergyForLaser() > 0) && base.Targeter is AbilityUtil_Targeter_ConeOrLaser)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					AbilityUtil_Targeter_ConeOrLaser abilityUtil_Targeter_ConeOrLaser = base.Targeter as AbilityUtil_Targeter_ConeOrLaser;
					int visibleActorsCountByTooltipSubject = abilityUtil_Targeter_ConeOrLaser.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
					if (abilityUtil_Targeter_ConeOrLaser.m_updatingWithCone)
					{
						return visibleActorsCountByTooltipSubject * GetExtraEnergyForCone();
					}
					return visibleActorsCountByTooltipSubject * GetExtraEnergyForLaser();
				}
				}
			}
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "ConeDamage", string.Empty, m_coneDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_coneEnemyHitEffect, "ConeEnemyHitEffect");
		AddTokenInt(tokens, "LaserDamage", string.Empty, m_laserDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserEnemyHitEffect, "LaserEnemyHitEffect");
		AddTokenInt(tokens, "ExtraDamageForAlternating", string.Empty, m_extraDamageForAlternating);
		AddTokenInt(tokens, "ExtraDamageForNearTarget", string.Empty, m_extraDamageForNearTarget);
		AddTokenInt(tokens, "ExtraDamageForFromCover", string.Empty, m_extraDamageForFromCover);
		AddTokenInt(tokens, "ExtraDamageToEvaders", string.Empty, m_extraDamageToEvaders);
		AddTokenInt(tokens, "ExtraEnergyForCone", string.Empty, m_extraEnergyForCone);
		AddTokenInt(tokens, "ExtraEnergyForLaser", string.Empty, m_extraEnergyForLaser);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = m_coneDistThreshold - 0.1f;
		max = m_coneDistThreshold + 0.1f;
		return true;
	}

	public override bool ForceIgnoreCover(ActorData targetActor)
	{
		if (m_abilityData != null)
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
			if (m_stimAbility != null)
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
				if (m_stimAbility.BasicAttackIgnoreCover())
				{
					return m_abilityData.HasQueuedAbilityOfType(typeof(SoldierStimPack));
				}
			}
		}
		return false;
	}

	public override bool ForceReduceCoverEffectiveness(ActorData targetActor)
	{
		if (m_abilityData != null)
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
			if (m_stimAbility != null)
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
				if (m_stimAbility.BasicAttackReduceCoverEffectiveness())
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return m_abilityData.HasQueuedAbilityOfType(typeof(SoldierStimPack));
						}
					}
				}
			}
		}
		return false;
	}

	private bool ShouldUseCone(Vector3 freePos, ActorData caster)
	{
		Vector3 vector = freePos - caster.GetTravelBoardSquareWorldPosition();
		vector.y = 0f;
		float magnitude = vector.magnitude;
		return magnitude <= m_coneDistThreshold;
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		int result;
		if (animIndex != m_onCastConeAnimIndex)
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
			result = ((animIndex == m_onCastLaserAnimIndex) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType(List<AbilityTarget> targets, ActorData caster)
	{
		if (targets != null)
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
			if (caster != null)
			{
				int result;
				if (ShouldUseCone(targets[0].FreePos, caster))
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
					result = m_onCastConeAnimIndex;
				}
				else
				{
					result = m_onCastLaserAnimIndex;
				}
				return (ActorModelData.ActionAnimationType)result;
			}
		}
		return base.GetActionAnimType();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SoldierConeOrLaser))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_SoldierConeOrLaser);
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
