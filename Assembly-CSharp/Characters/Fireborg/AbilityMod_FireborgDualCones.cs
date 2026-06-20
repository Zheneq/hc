using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_FireborgDualCones : GenericAbility_AbilityMod
{
    [Separator("Target Select Mod")]
    public TargetSelectMod_FanCones m_targetSelectMod;
    [Separator("Extra Damage for overlap state")]
    public AbilityModPropertyInt m_extraDamageIfOverlapMod;
    public AbilityModPropertyInt m_extraDamageNonOverlapMod;
    [Separator("Add Ignited Effect If Overlap Hit")]
    public AbilityModPropertyBool m_igniteTargetIfOverlapHitMod;
    public AbilityModPropertyBool m_igniteTargetIfSuperheatedMod;
    [Separator("Ground Fire")]
    public AbilityModPropertyBool m_groundFireOnAllIfNormalMod;
    public AbilityModPropertyBool m_groundFireOnOverlapIfNormalMod;
    [Space(10f)]
    public AbilityModPropertyBool m_groundFireOnAllIfSuperheatedMod;
    public AbilityModPropertyBool m_groundFireOnOverlapIfSuperheatedMod;

    public override Type GetTargetAbilityType()
    {
        return typeof(FireborgDualCones);
    }

    public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
    {
        targetSelect.SetTargetSelectMod(m_targetSelectMod);
    }

    protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
    {
        FireborgDualCones fireborgDualCones = targetAbility as FireborgDualCones;
        if (fireborgDualCones != null)
        {
            base.AddModSpecificTooltipTokens(tokens, targetAbility);
            AddToken(
                tokens,
                m_extraDamageIfOverlapMod,
                "ExtraDamageIfOverlap",
                string.Empty,
                fireborgDualCones.m_extraDamageIfOverlap);
            AddToken(
                tokens,
                m_extraDamageNonOverlapMod,
                "ExtraDamageNonOverlap",
                string.Empty,
                fireborgDualCones.m_extraDamageNonOverlap);
        }
    }

    protected override string ModSpecificAutogenDesc(AbilityData abilityData)
    {
        FireborgDualCones fireborgDualCones = GetTargetAbilityOnAbilityData(abilityData) as FireborgDualCones;
        bool isValid = fireborgDualCones != null;
        string desc = base.ModSpecificAutogenDesc(abilityData);
        if (fireborgDualCones != null)
        {
            desc += GetTargetSelectModDesc(
                m_targetSelectMod,
                fireborgDualCones.m_targetSelectComp,
                "-- Target Select --");
            desc += PropDesc(
                m_extraDamageIfOverlapMod,
                "[ExtraDamageIfOverlap]",
                isValid,
                isValid ? fireborgDualCones.m_extraDamageIfOverlap : 0);
            desc += PropDesc(
                m_extraDamageNonOverlapMod,
                "[ExtraDamageNonOverlap]",
                isValid,
                isValid ? fireborgDualCones.m_extraDamageNonOverlap : 0);
            desc += PropDesc(
                m_igniteTargetIfOverlapHitMod,
                "[IgniteTargetIfOverlapHit]",
                isValid,
                isValid && fireborgDualCones.m_igniteTargetIfOverlapHit);
            desc += PropDesc(
                m_igniteTargetIfSuperheatedMod,
                "[IgniteTargetIfSuperheated]",
                isValid,
                isValid && fireborgDualCones.m_igniteTargetIfSuperheated);
            desc += PropDesc(
                m_groundFireOnAllIfNormalMod,
                "[GroundFireOnAllIfNormal]",
                isValid,
                isValid && fireborgDualCones.m_groundFireOnAllIfNormal);
            desc += PropDesc(
                m_groundFireOnOverlapIfNormalMod,
                "[GroundFireOnOverlapIfNormal]",
                isValid,
                isValid && fireborgDualCones.m_groundFireOnOverlapIfNormal);
            desc += PropDesc(
                m_groundFireOnAllIfSuperheatedMod,
                "[GroundFireOnAllIfSuperheated]",
                isValid,
                isValid && fireborgDualCones.m_groundFireOnAllIfSuperheated);
            desc += PropDesc(
                m_groundFireOnOverlapIfSuperheatedMod,
                "[GroundFireOnOverlapIfSuperheated]",
                isValid,
                isValid && fireborgDualCones.m_groundFireOnOverlapIfSuperheated);
        }

        return desc;
    }
}