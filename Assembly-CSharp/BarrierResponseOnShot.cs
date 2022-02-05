// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BarrierResponseOnShot
{
	[Header("-- Sequences for On Shot Visual --")]
	public GameObject m_onShotSequencePrefab;
	public bool m_useShooterPosAsReactionSequenceTargetPos;
	[Header("-- On Owner: For Enemy Shot --")]
	public int m_healOnOwnerFromEnemyShot;
	public int m_energyGainOnOwnerFromEnemyShot;
	public StandardEffectInfo m_effectOnOwnerFromEnemyShot;
	[Header("-- On Shooter: For Enemy Shot --")]
	public int m_damageOnEnemyOnShot;
	public int m_energyLossOnEnemyOnShot;
	public StandardEffectInfo m_effectOnEnemyOnShot;

	public bool HasResponses()
	{
		return m_healOnOwnerFromEnemyShot > 0
			|| m_energyGainOnOwnerFromEnemyShot > 0
			|| m_effectOnOwnerFromEnemyShot.m_applyEffect
			|| m_damageOnEnemyOnShot > 0
			|| m_energyLossOnEnemyOnShot > 0
			|| m_effectOnEnemyOnShot.m_applyEffect;
	}

	public BarrierResponseOnShot GetShallowCopy()
	{
		return (BarrierResponseOnShot)MemberwiseClone();
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens, string name, bool addCompare, BarrierResponseOnShot other)
	{
		bool addDiff = addCompare && other != null;
		AbilityMod.AddToken_IntDiff(tokens, name + "_OnShot_HealOnOwner", "", m_healOnOwnerFromEnemyShot, addDiff, addDiff ? other.m_healOnOwnerFromEnemyShot : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_OnShot_EnergyOnOwner", "", m_energyGainOnOwnerFromEnemyShot, addDiff, addDiff ? other.m_energyGainOnOwnerFromEnemyShot : 0);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnOwnerFromEnemyShot, name + "_OnShot_EffectOnOwner", addDiff ? other.m_effectOnOwnerFromEnemyShot : null, addDiff);
		AbilityMod.AddToken_IntDiff(tokens, name + "_OnShot_DamageOnEnemy", "", m_damageOnEnemyOnShot, addDiff, addDiff ? other.m_damageOnEnemyOnShot : 0);
		AbilityMod.AddToken_IntDiff(tokens, name + "_OnShot_EnergyLossOnEnemy", "", m_energyLossOnEnemyOnShot, addDiff, addDiff ? other.m_energyLossOnEnemyOnShot : 0);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnEnemyOnShot, name + "_OnShot_EffectOnEnemy", addDiff ? other.m_effectOnEnemyOnShot : null, addDiff);
	}

	public string GetInEditorDescription(string header = "- Response -", string indent = "", bool showDiff = false, BarrierResponseOnShot other = null)
	{
		bool addDiff = showDiff && other != null;
		string otherSep = "\t        \t | in base  =";
		string desc = "\n" + InEditorDescHelper.BoldedStirng(header) + "\n";
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Heal on Owner on Enemy shot ] = ", indent, otherSep, m_healOnOwnerFromEnemyShot, addDiff, addDiff ? other.m_healOnOwnerFromEnemyShot : 0);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ EnergyGain on Owner on Enemy shot ] = ", indent, otherSep, m_energyGainOnOwnerFromEnemyShot, addDiff, addDiff ? other.m_energyGainOnOwnerFromEnemyShot : 0);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnOwnerFromEnemyShot, "{ Effect on Owner on Enemy shot }", indent, addDiff, addDiff ? other.m_effectOnOwnerFromEnemyShot : null);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ Damage on Enemy on shot ] = ", indent, otherSep, m_damageOnEnemyOnShot, addDiff, addDiff ? other.m_damageOnEnemyOnShot : 0);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ EnergyLoss on Enemy on shot ] = ", indent, otherSep, m_energyLossOnEnemyOnShot, addDiff, addDiff ? other.m_energyLossOnEnemyOnShot : 0);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnEnemyOnShot, "{ Effect on Enemy on shot }", indent, addDiff, addDiff ? other.m_effectOnEnemyOnShot : null);
		desc += InEditorDescHelper.AssembleFieldWithDiff("[ On Shot Sequence Prefab ]", indent, otherSep, m_onShotSequencePrefab, addDiff, addDiff ? other.m_onShotSequencePrefab : null);
		return desc + InEditorDescHelper.AssembleFieldWithDiff("[ Use Shooter Pos As Target Pos For Sequence ] = ", indent, otherSep, m_useShooterPosAsReactionSequenceTargetPos, addDiff, addDiff && other.m_useShooterPosAsReactionSequenceTargetPos);
	}

	// server-only
#if SERVER
	public bool HasReactionOnOwner(ActorData shooter, ActorData owner)
	{
		return shooter != null
			&& owner != null
			&& shooter.GetTeam() != owner.GetTeam()
			&& (m_healOnOwnerFromEnemyShot > 0 || m_energyGainOnOwnerFromEnemyShot > 0 || m_effectOnOwnerFromEnemyShot.m_applyEffect);
	}
#endif

	// server-only
#if SERVER
	public bool HasReactionOnShooter(ActorData shooter, ActorData owner)
	{
		return shooter != null
			&& owner != null
			&& shooter.GetTeam() != owner.GetTeam()
			&& (m_damageOnEnemyOnShot > 0 || m_energyLossOnEnemyOnShot > 0 || m_effectOnEnemyOnShot.m_applyEffect);
	}
#endif

	// server-only
#if SERVER
	public void AddToReactionResult(MovementResults reactResults, ActorData shooter, ActorData owner)
	{
		if (reactResults != null && shooter != null && owner != null)
		{
			if (HasReactionOnOwner(shooter, owner))
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(owner, owner.GetFreePos()));
				actorHitResults.SetBaseHealing(m_healOnOwnerFromEnemyShot);
				actorHitResults.SetTechPointGain(m_energyGainOnOwnerFromEnemyShot);
				actorHitResults.AddStandardEffectInfo(m_effectOnOwnerFromEnemyShot);
				actorHitResults.SetIgnoreTechpointInteractionForHit(true);
				reactResults.AddActorHitResultsForReaction(actorHitResults);
			}
			if (HasReactionOnShooter(shooter, owner))
			{
				ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(shooter, shooter.GetFreePos()));
				actorHitResults2.SetBaseDamage(m_damageOnEnemyOnShot);
				actorHitResults2.SetTechPointLoss(m_energyLossOnEnemyOnShot);
				actorHitResults2.AddStandardEffectInfo(m_effectOnEnemyOnShot);
				actorHitResults2.SetIgnoreTechpointInteractionForHit(true);
				reactResults.AddActorHitResultsForReaction(actorHitResults2);
			}
		}
	}
#endif
}
