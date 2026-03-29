// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;

public class GrydKnockbackTrap : Ability
{
    [Header("-- Trap Ground Field")]
    public GroundEffectField m_trapFieldInfo;
    [Header("-- Extra Damage")]
    public int m_extraDamagePerTurn;
    public int m_maxExtraDamage;
    public int m_knockbackAmount = 2;
    public bool m_lockToCardinalDirs = true;
    [Header("-- Sequences --")]
    public GameObject m_castSequencePrefab;

    private GroundEffectField m_cachedTrapFieldInfo;

    private void Start()
    {
        if (m_abilityName == "Base Ability")
        {
            m_abilityName = "Knockback Trap";
        }

        Setup();
    }

    private void Setup()
    {
        SetCachedFields();
        GroundEffectField trapFieldInfo = GetTrapFieldInfo();
        AbilityUtil_Targeter.AffectsActor affectsCaster = trapFieldInfo.IncludeAllies()
            ? AbilityUtil_Targeter.AffectsActor.Possible
            : AbilityUtil_Targeter.AffectsActor.Never;
        Targeters.Clear();
        for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
        {
            AbilityUtil_Targeter_KnockbackAoE targeter = new AbilityUtil_Targeter_KnockbackAoE(
                this,
                trapFieldInfo.shape,
                trapFieldInfo.penetrateLos,
                AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
                trapFieldInfo.IncludeEnemies(),
                trapFieldInfo.IncludeAllies(),
                affectsCaster,
                AbilityUtil_Targeter.AffectsActor.Never,
                m_knockbackAmount,
                KnockbackType.ForwardAlongAimDir);
            targeter.SetUseMultiTargetUpdate(true);
            targeter.m_lockToCardinalDirs = m_lockToCardinalDirs;
            targeter.m_showArrowHighlight = true;
            Targeters.Add(targeter);
        }
    }

    public override int GetExpectedNumberOfTargeters()
    {
        return 2;
    }

    private void SetCachedFields()
    {
        m_cachedTrapFieldInfo = m_trapFieldInfo;
    }

    // reactor
    public GroundEffectField GetTrapFieldInfo()
    {
        return m_cachedTrapFieldInfo != null
            ? m_cachedTrapFieldInfo
            : m_trapFieldInfo;
    }

    // rogues
    // public GroundEffectField GetTrapFieldInfo()
    // {
    //     if (m_trapFieldInfo == null)
    //     {
    //         m_trapFieldInfo = ScriptableObject.CreateInstance<GroundEffectField>();
    //     }
    //
    //     return m_trapFieldInfo;
    // }

    public int GetExtraDamagePerTurn()
    {
        return m_extraDamagePerTurn;
    }

    public int GetMaxExtraDamage()
    {
        return m_maxExtraDamage;
    }

    protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
    {
        List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
        int damageAmount = GetTrapFieldInfo().damageAmount;
        AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, damageAmount);
        return numbers;
    }

    protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
    {
        AbilityMod_ThiefHiddenTrap mod = modAsBase as AbilityMod_ThiefHiddenTrap;
        
        // reactor
        // m_trapFieldInfo.AddTooltipTokens(tokens, "GroundEffect");
        // rogues
        GetTrapFieldInfo().AddTooltipTokens(tokens, "GroundEffect");
        
        AddTokenInt(
            tokens,
            "ExtraDamagePerTurn",
            string.Empty,
            mod
                ? mod.m_extraDamagePerTurnMod.GetModifiedValue(m_extraDamagePerTurn)
                : m_extraDamagePerTurn);
        AddTokenInt(
            tokens,
            "MaxExtraDamage",
            string.Empty,
            mod
                ? mod.m_maxExtraDamageMod.GetModifiedValue(m_maxExtraDamage)
                : m_maxExtraDamage);
    }
    
#if SERVER
    // added in rogues
    public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
        return new List<ServerClientUtils.SequenceStartData>
        {
            new ServerClientUtils.SequenceStartData(
                m_castSequencePrefab,
                targetSquare,
                additionalData.m_abilityResults.HitActorsArray(),
                caster,
                additionalData.m_sequenceSource)
        };
    }

    // added in rogues
    public override void GatherAbilityResults(
        List<AbilityTarget> targets,
        ActorData caster,
        ref AbilityResults abilityResults)
    {
        BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
        PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(targetSquare.ToVector3()));
        Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetTrapFieldInfo().shape, targets[0]);
        Vector3 knockbackDir = targets[1].FreePos - centerOfShape;
        if (m_lockToCardinalDirs)
        {
            knockbackDir = VectorUtils.HorizontalAngleToClosestCardinalDirection(
                Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(knockbackDir)));
        }

        GrydKnockbackTrapEffect effect = new GrydKnockbackTrapEffect(
            AsEffectSource(),
            targetSquare,
            targets[0].FreePos,
            null,
            caster,
            GetTrapFieldInfo(),
            GetExtraDamagePerTurn(),
            GetMaxExtraDamage(),
            m_knockbackAmount,
            knockbackDir);
        positionHitResults.AddEffect(effect);
        abilityResults.StorePositionHit(positionHitResults);
    }
#endif
}