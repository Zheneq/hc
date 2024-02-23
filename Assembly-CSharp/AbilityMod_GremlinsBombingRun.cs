using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_GremlinsBombingRun : AbilityMod
{
	public AbilityModPropertyInt m_damageMod;
	[Header("-- Range and turn angle mods")]
	public AbilityModPropertyInt m_minSquaresPerExplosionMod;
	public AbilityModPropertyInt m_maxSquaresPerExplosionMod;
	public AbilityModPropertyFloat m_angleWithFirstStepMod;
	[Space(10f)]
	public AbilityModPropertyShape m_explosionShapeMod;
	[Space(10f)]
	[Header("-- Global Mine Data Mods")]
	public AbilityModPropertyInt m_mineDamageMod;
	public AbilityModPropertyInt m_mineDurationMod;
	public AbilityModPropertyEffectInfo m_effectOnEnemyOverride;
	public AbilityModPropertyInt m_energyOnMineExplosionMod;
	public AbilityModPropertyBool m_shouldLeaveMinesAtTouchedSquares;

	public override Type GetTargetAbilityType()
	{
		return typeof(GremlinsBombingRun);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		GremlinsBombingRun gremlinsBombingRun = targetAbility as GremlinsBombingRun;
		if (gremlinsBombingRun != null)
		{
			AddToken(tokens, m_minSquaresPerExplosionMod, "SquaresPerExplosion", string.Empty, gremlinsBombingRun.m_squaresPerExplosion);
			AddToken(tokens, m_maxSquaresPerExplosionMod, "MaxSquaresPerStep", string.Empty, gremlinsBombingRun.m_maxSquaresPerStep);
			AddToken(tokens, m_angleWithFirstStepMod, "MaxAngleWithFirstStep", string.Empty, gremlinsBombingRun.m_maxAngleWithFirstStep);
			AddToken(tokens, m_damageMod, "ExplosionDamageAmount", string.Empty, gremlinsBombingRun.m_explosionDamageAmount);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		GremlinsBombingRun gremlinsBombingRun = GetTargetAbilityOnAbilityData(abilityData) as GremlinsBombingRun;
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent = gremlinsBombingRun != null
			? gremlinsBombingRun.GetComponent<GremlinsLandMineInfoComponent>()
			: null;
		bool isAbilityPresent = gremlinsLandMineInfoComponent != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isAbilityPresent, isAbilityPresent ? gremlinsBombingRun.m_explosionDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_minSquaresPerExplosionMod, "[Min Squares Per Explosion]", isAbilityPresent, isAbilityPresent ? gremlinsBombingRun.m_squaresPerExplosion : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_maxSquaresPerExplosionMod, "[Max Squares Per Explosion]", isAbilityPresent, isAbilityPresent ? gremlinsBombingRun.m_maxSquaresPerStep : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_angleWithFirstStepMod, "[Turn Angle (with first segment)]", isAbilityPresent, isAbilityPresent ? gremlinsBombingRun.m_maxAngleWithFirstStep : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_explosionShapeMod, "[Explosion Shape]", isAbilityPresent, isAbilityPresent ? gremlinsBombingRun.m_explosionShape : AbilityAreaShape.SingleSquare);
		desc += AbilityModHelper.GetModPropertyDesc(m_mineDamageMod, "[Mine Damage]", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_mineDurationMod, "[Mine Duration]", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_mineDuration : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_effectOnEnemyOverride, "{ Effect on Enemy Hit Override }", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_enemyHitEffect : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_energyOnMineExplosionMod, "[Energy Gain on Mine Explosion (on splort and mines left behind from primary/ult)]", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_energyGainOnExplosion : 0);
		return new StringBuilder().Append(desc).Append(PropDesc(m_shouldLeaveMinesAtTouchedSquares, "[Leave Mines At Each Touched Square?]", isAbilityPresent)).ToString();
	}
}
