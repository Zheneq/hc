using System.Collections.Generic;
using UnityEngine;

public class BazookaGirlStickyBomb : Ability
{
	public enum TargeterType
	{
		Shape,
		Laser,
		Cone
	}

	[Header("-- Targeting")]
	public TargeterType m_targeterType = TargeterType.Laser;

	public bool m_targeterPenetrateLos;

	public int m_maxTargets = -1;

	[Header("-- Targeting: If Using Laser Targeting")]
	public float m_laserWidth = 1f;

	public float m_laserRange = 5f;

	[Header("-- Targeting: If Using Shape Targeter")]
	public AbilityAreaShape m_targeterShape = AbilityAreaShape.Five_x_Five;

	[Header("-- Targeting: If Using Cone Targeter")]
	public float m_coneWidthAngle = 270f;

	public float m_coneLength = 2.5f;

	[Header("-- Bomb Info")]
	public int m_energyGainOnCastPerEnemyHit;

	public StandardEffectInfo m_enemyOnCastHitEffect;

	public ThiefPartingGiftBombInfo m_bombInfo;

	public SpoilsSpawnData m_spoilSpawnOnExplosion;

	private AbilityMod_BazookaGirlStickyBomb m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Sticky Bomb";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (m_targeterType == TargeterType.Laser)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					base.Targeter = new AbilityUtil_Targeter_Laser(this, m_laserWidth, m_laserRange, m_targeterPenetrateLos, m_maxTargets);
					return;
				}
			}
		}
		if (m_targeterType == TargeterType.Shape)
		{
			base.Targeter = new AbilityUtil_Targeter_Shape(this, m_targeterShape, m_targeterPenetrateLos);
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_DirectionCone(this, m_coneWidthAngle, m_coneLength, 0f, m_targeterPenetrateLos, true);
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return m_coneLength;
	}

	public int GetEnergyGainOnCastPerEnemyHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyGainOnCastPerEnemyHitMod.GetModifiedValue(m_energyGainOnCastPerEnemyHit);
		}
		else
		{
			result = m_energyGainOnCastPerEnemyHit;
		}
		return result;
	}

	private StandardEffectInfo GetEnemyOnCastHitEffect()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_enemyOnCastHitEffectOverride.GetModifiedValue(m_enemyOnCastHitEffect) : m_enemyOnCastHitEffect;
	}

	private bool HasCooldownModification()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = 0;
		}
		else if (m_abilityMod.m_cooldownModOnAction != AbilityData.ActionType.INVALID_ACTION)
		{
			result = ((m_abilityMod.m_cooldownAddAmount != 0) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_bombInfo.damageAmount);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlStickyBomb abilityMod = m_abilityMod;
		AddTokenInt(tokens, "Damage", string.Empty, m_bombInfo.damageAmount);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod)
		{
			val = abilityMod.m_energyGainOnCastPerEnemyHitMod.GetModifiedValue(m_energyGainOnCastPerEnemyHit);
		}
		else
		{
			val = m_energyGainOnCastPerEnemyHit;
		}
		AddTokenInt(tokens, "EnergyGainOnCastPerEnemyHit", empty, val);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod)
		{
			effectInfo = abilityMod.m_enemyOnCastHitEffectOverride.GetModifiedValue(m_enemyOnCastHitEffect);
		}
		else
		{
			effectInfo = m_enemyOnCastHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyOnCastHitEffect", m_enemyOnCastHitEffect);
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (base.Targeter != null)
		{
			if (GetEnergyGainOnCastPerEnemyHit() > 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Primary);
						return GetEnergyGainOnCastPerEnemyHit() * visibleActorsCountByTooltipSubject;
					}
					}
				}
			}
		}
		return 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_BazookaGirlStickyBomb))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_BazookaGirlStickyBomb);
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
	}
}
