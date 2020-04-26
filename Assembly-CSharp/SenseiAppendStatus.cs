using System.Collections.Generic;
using UnityEngine;

public class SenseiAppendStatus : Ability
{
	public enum TargetingMode
	{
		ActorSquare,
		Laser
	}

	[Separator("Targeting", true)]
	public TargetingMode m_targetingMode;

	[Header("    (( Targeting: If using ActorSquare mode ))")]
	public bool m_canTargetAlly = true;

	public bool m_canTargetEnemy = true;

	public bool m_canTagetSelf;

	public bool m_targetingIgnoreLos;

	[Header("-- Whether to check barriers for enemy targeting")]
	public bool m_checkBarrierForLosIfTargetEnemy = true;

	[Header("    (( Targeting: If using Laser mode ))")]
	public LaserTargetingInfo m_laserInfo;

	[Separator("On Cast Hit Stuff", true)]
	public int m_energyToAllyTargetOnCast;

	public StandardActorEffectData m_enemyCastHitEffectData;

	public StandardActorEffectData m_allyCastHitEffectData;

	[Separator("For Append Effect", true)]
	public bool m_endEffectIfAppendedStatus = true;

	public AbilityPriority m_earliestPriorityToConsider;

	public bool m_delayEffectApply = true;

	public bool m_requireDamageToTransfer = true;

	[Header("-- Effect to append --")]
	public StandardEffectInfo m_effectAddedOnEnemyAttack;

	public StandardEffectInfo m_effectAddedOnAllyAttack;

	[Space(10f)]
	public int m_energyGainOnAllyAppendHit;

	[Header("-- Sequences --")]
	public GameObject m_castOnEnemySequencePrefab;

	public GameObject m_castOnAllySequencePrefab;

	public GameObject m_statusApplyOnAllySequencePrefab;

	public GameObject m_statusApplyOnEnemySequencePrefab;

	private AbilityMod_SenseiAppendStatus m_abilityMod;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardActorEffectData m_cachedEnemyCastHitEffectData;

	private StandardActorEffectData m_cachedAllyCastHitEffectData;

	private StandardEffectInfo m_cachedEffectAddedOnEnemyAttack;

	private StandardEffectInfo m_cachedEffectAddedOnAllyAttack;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "SenseiAppendStatus";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		if (m_targetingMode == TargetingMode.ActorSquare)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					int num;
					if (CanTagetSelf())
					{
						num = 1;
					}
					else
					{
						num = 0;
					}
					AbilityUtil_Targeter.AffectsActor affectsCaster = (AbilityUtil_Targeter.AffectsActor)num;
					base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, CanTargetAlly(), CanTargetEnemy(), affectsCaster);
					return;
				}
				}
			}
		}
		base.Targeter = new AbilityUtil_Targeter_Laser(this, GetLaserInfo());
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Art --</color>\n" + Ability.SetupNoteVarName("Cast On Enemy Sequence Prefab") + "\nFor initial cast, it targeting Enemy\n\n" + Ability.SetupNoteVarName("Cast On Ally Sequence Prefab") + "\nFor initial casst, if targeting Ally ...\n\n" + Ability.SetupNoteVarName("Status Apply On Ally Sequence Prefab") + "\nFor impact on target that actually adds buff/debuff\n\n" + Ability.SetupNoteVarName("Status Apply On Enemy Sequence Prefab") + "\nFor impact on target that actually adds buff/debuff\n\n";
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = ((!m_abilityMod) ? m_laserInfo : m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo));
		StandardActorEffectData cachedEnemyCastHitEffectData;
		if ((bool)m_abilityMod)
		{
			cachedEnemyCastHitEffectData = m_abilityMod.m_enemyCastHitEffectDataMod.GetModifiedValue(m_enemyCastHitEffectData);
		}
		else
		{
			cachedEnemyCastHitEffectData = m_enemyCastHitEffectData;
		}
		m_cachedEnemyCastHitEffectData = cachedEnemyCastHitEffectData;
		StandardActorEffectData cachedAllyCastHitEffectData;
		if ((bool)m_abilityMod)
		{
			cachedAllyCastHitEffectData = m_abilityMod.m_allyCastHitEffectDataMod.GetModifiedValue(m_allyCastHitEffectData);
		}
		else
		{
			cachedAllyCastHitEffectData = m_allyCastHitEffectData;
		}
		m_cachedAllyCastHitEffectData = cachedAllyCastHitEffectData;
		m_cachedEffectAddedOnEnemyAttack = ((!m_abilityMod) ? m_effectAddedOnEnemyAttack : m_abilityMod.m_effectAddedOnEnemyAttackMod.GetModifiedValue(m_effectAddedOnEnemyAttack));
		StandardEffectInfo cachedEffectAddedOnAllyAttack;
		if ((bool)m_abilityMod)
		{
			cachedEffectAddedOnAllyAttack = m_abilityMod.m_effectAddedOnAllyAttackMod.GetModifiedValue(m_effectAddedOnAllyAttack);
		}
		else
		{
			cachedEffectAddedOnAllyAttack = m_effectAddedOnAllyAttack;
		}
		m_cachedEffectAddedOnAllyAttack = cachedEffectAddedOnAllyAttack;
	}

	public bool CanTargetAlly()
	{
		return (!m_abilityMod) ? m_canTargetAlly : m_abilityMod.m_canTargetAllyMod.GetModifiedValue(m_canTargetAlly);
	}

	public bool CanTargetEnemy()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_canTargetEnemyMod.GetModifiedValue(m_canTargetEnemy);
		}
		else
		{
			result = m_canTargetEnemy;
		}
		return result;
	}

	public bool CanTagetSelf()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_canTagetSelfMod.GetModifiedValue(m_canTagetSelf);
		}
		else
		{
			result = m_canTagetSelf;
		}
		return result;
	}

	public bool TargetingIgnoreLos()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_targetingIgnoreLosMod.GetModifiedValue(m_targetingIgnoreLos);
		}
		else
		{
			result = m_targetingIgnoreLos;
		}
		return result;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return (m_cachedLaserInfo == null) ? m_laserInfo : m_cachedLaserInfo;
	}

	public StandardActorEffectData GetEnemyCastHitEffectData()
	{
		StandardActorEffectData result;
		if (m_cachedEnemyCastHitEffectData != null)
		{
			result = m_cachedEnemyCastHitEffectData;
		}
		else
		{
			result = m_enemyCastHitEffectData;
		}
		return result;
	}

	public StandardActorEffectData GetAllyCastHitEffectData()
	{
		return (m_cachedAllyCastHitEffectData == null) ? m_allyCastHitEffectData : m_cachedAllyCastHitEffectData;
	}

	public int GetEnergyToAllyTargetOnCast()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyToAllyTargetOnCastMod.GetModifiedValue(m_energyToAllyTargetOnCast);
		}
		else
		{
			result = m_energyToAllyTargetOnCast;
		}
		return result;
	}

	public bool EndEffectIfAppendedStatus()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_endEffectIfAppendedStatusMod.GetModifiedValue(m_endEffectIfAppendedStatus);
		}
		else
		{
			result = m_endEffectIfAppendedStatus;
		}
		return result;
	}

	public StandardEffectInfo GetEffectAddedOnEnemyAttack()
	{
		StandardEffectInfo result;
		if (m_cachedEffectAddedOnEnemyAttack != null)
		{
			result = m_cachedEffectAddedOnEnemyAttack;
		}
		else
		{
			result = m_effectAddedOnEnemyAttack;
		}
		return result;
	}

	public StandardEffectInfo GetEffectAddedOnAllyAttack()
	{
		StandardEffectInfo result;
		if (m_cachedEffectAddedOnAllyAttack != null)
		{
			result = m_cachedEffectAddedOnAllyAttack;
		}
		else
		{
			result = m_effectAddedOnAllyAttack;
		}
		return result;
	}

	public int GetEnergyGainOnAllyAppendHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyGainOnAllyAppendHitMod.GetModifiedValue(m_energyGainOnAllyAppendHit);
		}
		else
		{
			result = m_energyGainOnAllyAppendHit;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportEnergy(ref number, AbilityTooltipSubject.Ally, GetEnergyToAllyTargetOnCast());
		return number;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (m_targetingMode == TargetingMode.ActorSquare)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					bool flag = false;
					ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
					return CanTargetActorInDecision(caster, currentBestActorTarget, CanTargetEnemy(), CanTargetAlly(), CanTagetSelf(), ValidateCheckPath.Ignore, !TargetingIgnoreLos(), true);
				}
				}
			}
		}
		return true;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_targetingMode == TargetingMode.ActorSquare)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return HasTargetableActorsInDecision(caster, CanTargetEnemy(), CanTargetAlly(), CanTagetSelf(), ValidateCheckPath.Ignore, !TargetingIgnoreLos(), true);
				}
			}
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		m_enemyCastHitEffectData.AddTooltipTokens(tokens, "EnemyCastHitEffectData");
		m_allyCastHitEffectData.AddTooltipTokens(tokens, "AllyCastHitEffectData");
		AddTokenInt(tokens, "EnergyToAllyTargetOnCast", string.Empty, m_energyToAllyTargetOnCast);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectAddedOnEnemyAttack, "EffectAddedOnEnemyAttack", m_effectAddedOnEnemyAttack);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectAddedOnAllyAttack, "EffectAddedOnAllyAttack", m_effectAddedOnAllyAttack);
		AddTokenInt(tokens, "EnergyGainOnAllyAppendHit", string.Empty, m_energyGainOnAllyAppendHit);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SenseiAppendStatus))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_SenseiAppendStatus);
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
