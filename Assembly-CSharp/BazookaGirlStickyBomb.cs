using System;
using System.Collections.Generic;
using UnityEngine;

public class BazookaGirlStickyBomb : Ability
{
	[Header("-- Targeting")]
	public BazookaGirlStickyBomb.TargeterType m_targeterType = BazookaGirlStickyBomb.TargeterType.Laser;

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
		if (this.m_abilityName == "Base Ability")
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlStickyBomb.Start()).MethodHandle;
			}
			this.m_abilityName = "Sticky Bomb";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (this.m_targeterType == BazookaGirlStickyBomb.TargeterType.Laser)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlStickyBomb.SetupTargeter()).MethodHandle;
			}
			base.Targeter = new AbilityUtil_Targeter_Laser(this, this.m_laserWidth, this.m_laserRange, this.m_targeterPenetrateLos, this.m_maxTargets, false, false);
		}
		else if (this.m_targeterType == BazookaGirlStickyBomb.TargeterType.Shape)
		{
			base.Targeter = new AbilityUtil_Targeter_Shape(this, this.m_targeterShape, this.m_targeterPenetrateLos, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_DirectionCone(this, this.m_coneWidthAngle, this.m_coneLength, 0f, this.m_targeterPenetrateLos, true, true, false, false, -1, false);
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.m_coneLength;
	}

	public int GetEnergyGainOnCastPerEnemyHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlStickyBomb.GetEnergyGainOnCastPerEnemyHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_energyGainOnCastPerEnemyHitMod.GetModifiedValue(this.m_energyGainOnCastPerEnemyHit);
		}
		else
		{
			result = this.m_energyGainOnCastPerEnemyHit;
		}
		return result;
	}

	private StandardEffectInfo GetEnemyOnCastHitEffect()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_enemyOnCastHitEffectOverride.GetModifiedValue(this.m_enemyOnCastHitEffect) : this.m_enemyOnCastHitEffect;
	}

	private bool HasCooldownModification()
	{
		bool result;
		if (this.m_abilityMod == null)
		{
			result = false;
		}
		else if (this.m_abilityMod.m_cooldownModOnAction != AbilityData.ActionType.INVALID_ACTION)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlStickyBomb.HasCooldownModification()).MethodHandle;
			}
			result = (this.m_abilityMod.m_cooldownAddAmount != 0);
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_bombInfo.damageAmount);
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlStickyBomb abilityMod = this.m_abilityMod;
		base.AddTokenInt(tokens, "Damage", string.Empty, this.m_bombInfo.damageAmount, false);
		string name = "EnergyGainOnCastPerEnemyHit";
		string empty = string.Empty;
		int val;
		if (abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlStickyBomb.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod.m_energyGainOnCastPerEnemyHitMod.GetModifiedValue(this.m_energyGainOnCastPerEnemyHit);
		}
		else
		{
			val = this.m_energyGainOnCastPerEnemyHit;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		StandardEffectInfo effectInfo;
		if (abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo = abilityMod.m_enemyOnCastHitEffectOverride.GetModifiedValue(this.m_enemyOnCastHitEffect);
		}
		else
		{
			effectInfo = this.m_enemyOnCastHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyOnCastHitEffect", this.m_enemyOnCastHitEffect, true);
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (base.Targeter != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlStickyBomb.GetAdditionalTechPointGainForNameplateItem(ActorData, int)).MethodHandle;
			}
			if (this.GetEnergyGainOnCastPerEnemyHit() > 0)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Primary);
				return this.GetEnergyGainOnCastPerEnemyHit() * visibleActorsCountByTooltipSubject;
			}
		}
		return 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BazookaGirlStickyBomb))
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BazookaGirlStickyBomb.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_BazookaGirlStickyBomb);
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
	}

	public enum TargeterType
	{
		Shape,
		Laser,
		Cone
	}
}
