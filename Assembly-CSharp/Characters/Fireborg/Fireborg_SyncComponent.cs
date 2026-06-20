using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AbilityContextNamespace;
using UnityEngine;
using UnityEngine.Networking;

// TODO FIREBORG what if you spawn in one ground fire and walk into another?
public class Fireborg_SyncComponent : NetworkBehaviour
{
    [Separator("Ignited Effect")]
    public StandardActorEffectData m_ignitedEffectData;
    public int m_ignitedTriggerDamage = 5;
    public StandardEffectInfo m_ignitedTriggerEffect;
    public int m_ignitedTriggerEnergyOnCaster;
    [Separator("Ground Fire Effect")]
    public int m_groundFireDamageNormal = 6;
    public int m_groundFireDamageSuperheated = 8;
    public StandardEffectInfo m_groundFireEffect;
    public bool m_groundFireAddsIgniteIfSuperheated = true;
    [Separator("Sequences")]
    public GameObject m_groundFirePerSquareSeqPrefab;
    public GameObject m_groundFireOnHitSeqPrefab; // null
    [Header("-- Superheated versions")]
    public GameObject m_superheatedGroundFireSquareSeqPrefab; // null

    [SyncVar]
    internal int m_superheatLastCastTurn;

    internal SyncListUInt m_actorsInGroundFireOnTurnStart = new SyncListUInt(); // for visuals
    // (if somebody is already burning, do not show additional damage as it is implied by them being in a state of burning)

    private AbilityData m_abilityData;
    private FireborgSuperheat m_superheatAbility;
    private AbilityData.ActionType m_superheatActionType = AbilityData.ActionType.INVALID_ACTION;
    public static ContextNameKeyPair s_cvarSuperheated = new ContextNameKeyPair("Superheated");
    private HashSet<ActorData> m_ignitedActorsThisTurn = new HashSet<ActorData>();
    private static int kListm_actorsInGroundFireOnTurnStart = 1427115255;

#if SERVER
    // custom
    internal HashSet<ActorData> m_actorsIgnitedThisTurn = new HashSet<ActorData>();
    // custom
    internal HashSet<ActorData> m_actorsIgnitedThisTurn_Fake = new HashSet<ActorData>();
    // custom
    internal HashSet<ActorData> m_actorsHitByGroundFireThisTurn = new HashSet<ActorData>();
    // custom
    internal HashSet<ActorData> m_actorsHitByGroundFireThisTurn_Fake = new HashSet<ActorData>();
    // custom
    internal int m_pendingShield;
#endif

    public int Networkm_superheatLastCastTurn
    {
        get => m_superheatLastCastTurn;
        [param: In]
        set => SetSyncVar(value, ref m_superheatLastCastTurn, 1u);
    }

    static Fireborg_SyncComponent()
    {
        RegisterSyncListDelegate(
            typeof(Fireborg_SyncComponent),
            kListm_actorsInGroundFireOnTurnStart,
            InvokeSyncListm_actorsInGroundFireOnTurnStart);
        NetworkCRC.RegisterBehaviour("Fireborg_SyncComponent", 0);
    }

    public void ResetIgnitedActorsTrackingThisTurn()
    {
        m_ignitedActorsThisTurn.Clear();
    }

    private void Start()
    {
        m_abilityData = GetComponent<AbilityData>();
        m_superheatAbility = m_abilityData.GetAbilityOfType<FireborgSuperheat>();
        if (m_superheatAbility != null)
        {
            m_superheatActionType = m_abilityData.GetActionTypeOfAbility(m_superheatAbility);
        }
    }

    public static string GetSuperheatedCvarUsage()
    {
        return ContextVars.GetContextUsageStr(
            s_cvarSuperheated.GetName(),
            "1 if caster is in Superheated mode, 0 otherwise",
            false);
    }

    public bool InSuperheatMode()
    {
        if (m_superheatAbility == null)
        {
            return false;
        }

        if (m_superheatLastCastTurn > 0
            && GameFlowData.Get().CurrentTurn < m_superheatLastCastTurn + m_superheatAbility.GetSuperheatDuration())
        {
            return true;
        }

        return m_abilityData.HasQueuedAction(m_superheatActionType);
    }

    public void SetSuperheatedContextVar(ContextVars abilityContext)
    {
        bool flag = InSuperheatMode();
        abilityContext.SetValue(s_cvarSuperheated.GetKey(), flag ? 1 : 0);
    }

    public void AddGroundFireTargetingNumber(ActorData target, ActorData caster, TargetingNumberUpdateScratch results)
    {
        if (target.GetTeam() == caster.GetTeam())
        {
            return;
        }

        int groundFireDamage = InSuperheatMode()
            ? m_groundFireDamageSuperheated
            : m_groundFireDamageNormal;
        if (groundFireDamage > 0)
        {
            results.m_damage += groundFireDamage;
        }
    }

    public string GetTargetPreviewAccessoryString(
        AbilityTooltipSymbol symbolType,
        Ability ability,
        ActorData targetActor,
        ActorData caster)
    {
        if (symbolType != AbilityTooltipSymbol.Damage)
        {
            return null;
        }

        int groundFireDamage = InSuperheatMode()
            ? m_groundFireDamageSuperheated
            : m_groundFireDamageNormal;
        if (groundFireDamage > 0)
        {
            return "\n+ " + AbilityUtils.CalculateDamageForTargeter(
                caster,
                targetActor,
                ability,
                groundFireDamage,
                false);
        }

        return null;
    }

    private void UNetVersion()
    {
    }

    protected static void InvokeSyncListm_actorsInGroundFireOnTurnStart(NetworkBehaviour obj, NetworkReader reader)
    {
        if (!NetworkClient.active)
        {
            Debug.LogError("SyncList m_actorsInGroundFireOnTurnStart called on server.");
            return;
        }

        ((Fireborg_SyncComponent)obj).m_actorsInGroundFireOnTurnStart.HandleMsg(reader);
    }

    private void Awake()
    {
        m_actorsInGroundFireOnTurnStart.InitializeBehaviour(this, kListm_actorsInGroundFireOnTurnStart);
    }

    public override bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
        if (forceAll)
        {
            writer.WritePackedUInt32((uint)m_superheatLastCastTurn);
            SyncListUInt.WriteInstance(writer, m_actorsInGroundFireOnTurnStart);
            return true;
        }

        bool flag = false;
        if ((syncVarDirtyBits & 1) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            writer.WritePackedUInt32((uint)m_superheatLastCastTurn);
        }

        if ((syncVarDirtyBits & 2) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            SyncListUInt.WriteInstance(writer, m_actorsInGroundFireOnTurnStart);
        }

        if (!flag)
        {
            writer.WritePackedUInt32(syncVarDirtyBits);
        }

        return flag;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        if (initialState)
        {
            m_superheatLastCastTurn = (int)reader.ReadPackedUInt32();
            SyncListUInt.ReadReference(reader, m_actorsInGroundFireOnTurnStart);
            return;
        }

        int num = (int)reader.ReadPackedUInt32();
        if ((num & 1) != 0)
        {
            m_superheatLastCastTurn = (int)reader.ReadPackedUInt32();
        }

        if ((num & 2) != 0)
        {
            SyncListUInt.ReadReference(reader, m_actorsInGroundFireOnTurnStart);
        }
    }

#if SERVER
    // custom 
    public bool GroundFireAddsIgnite => InSuperheatMode() && m_groundFireAddsIgniteIfSuperheated;

    // custom
    public FireborgIgnitedEffect MakeIgnitedEffect(EffectSource parent, ActorData caster, ActorData target)
    {
        HashSet<ActorData> set = ServerActionBuffer.Get().GatheringFakeResults
            ? m_actorsIgnitedThisTurn_Fake
            : m_actorsIgnitedThisTurn;

        if (set.Add(target))
        {
            return new FireborgIgnitedEffect(
                parent,
                target,
                caster,
                m_ignitedEffectData,
                m_ignitedTriggerDamage,
                m_ignitedTriggerEffect,
                m_ignitedTriggerEnergyOnCaster);
        }

        return null;
    }

    // custom
    private FireborgGroundFireEffect MakeGroundFireEffect(
        EffectSource parent,
        ActorData caster,
        List<BoardSquare> affectedSquares,
        int duration,
        out List<FireborgGroundFireEffect> oldEffectsForRemoval)
    {
        if (duration > 2)
        {
            Log.Error($"FireborgGroundFireEffect with duration {duration} is not supported! Duration might be overridden with the next effect created!");
        }
        
        HashSet<BoardSquare> allAffectedSquares = new HashSet<BoardSquare>(affectedSquares);
        oldEffectsForRemoval = new List<FireborgGroundFireEffect>();
        foreach (Effect effect in ServerEffectManager.Get().GetWorldEffectsByCaster(caster, typeof(FireborgGroundFireEffect)))
        {
            if (effect is FireborgGroundFireEffect oldEffect)
            {
                oldEffectsForRemoval.Add(oldEffect);
                allAffectedSquares.UnionWith(oldEffect.GetSquaresInShape());
            }
        }
        
        return new FireborgGroundFireEffect(
            parent,
            allAffectedSquares
                .Select(
                    s => new StandardMultiAreaGroundEffect.GroundAreaInfo(
                        s,
                        s.ToVector3(),
                        AbilityAreaShape.SingleSquare))
                .ToList(),
            caster,
            new GroundEffectField
            {
                duration = duration,
                shape = AbilityAreaShape.SingleSquare,
                damageAmount = InSuperheatMode()
                    ? m_groundFireDamageSuperheated
                    : m_groundFireDamageNormal,
                effectOnEnemies = m_groundFireEffect,
                effectOnAllies = new StandardEffectInfo
                {
                    m_applyEffect = false,
                    m_effectData = new StandardActorEffectData()
                },
                perSquareSequences = true,
                persistentSequencePrefab = InSuperheatMode() && m_superheatedGroundFireSquareSeqPrefab != null
                    ? m_superheatedGroundFireSquareSeqPrefab
                    : m_groundFirePerSquareSeqPrefab,
                enemyHitSequencePrefab = m_groundFireOnHitSeqPrefab
            });
    }

    // custom
    // TODO DASH FireborgDash applies all of this on dash phase and
    // can hit other dashes with ground fire if they stop in it.
    // Currently it works correctly but can break if we change what current board square means when you are dashing.
    public PositionHitResults MakeGroundFireEffectResults(
        Ability ability,
        ActorData caster,
        List<BoardSquare> affectedSquares,
        Vector3 posForHit,
        int duration,
        bool isReal,
        out FireborgGroundFireEffect groundFireEffect,
        out List<FireborgGroundFireEffect> oldEffectsForRemoval)
    {
        affectedSquares = affectedSquares.Where(s => s.IsValidForGameplay()).ToList();
        groundFireEffect = MakeGroundFireEffect(
            ability.AsEffectSource(),
            caster,
            affectedSquares,
            duration,
            out oldEffectsForRemoval);
        List<ActorData> hitActors = GetActorsHitByGroundFireThisTurn(isReal).ToList();
        groundFireEffect.AddToActorsHitThisTurn(hitActors);

        PositionHitResults posHitResults = new PositionHitResults(new PositionHitParameters(posForHit));
        EffectResults effectResults = new EffectResults(groundFireEffect, caster, isReal);
        groundFireEffect.GatherEffectResults(ref effectResults, isReal);
        SequenceSource sequenceSource = new SequenceSource(null, null);
        foreach (ActorHitResults hitResults in effectResults.m_actorToHitResults.Values)
        {
            ActorData hitActor = hitResults.m_hitParameters.Target;
            MovementResults movementResults = new MovementResults(MovementStage.INVALID);
            movementResults.SetupTriggerData(caster, null);
            movementResults.SetupGameplayDataForAbility(ability, caster);
            movementResults.SetupSequenceData(null, hitActor.GetCurrentBoardSquare(), sequenceSource);
            movementResults.AddActorHitResultsForReaction(hitResults);
            posHitResults.AddReactionOnPositionHit(movementResults);
        }

        return posHitResults;
    }

    // custom
    public HashSet<ActorData> GetActorsHitByGroundFireThisTurn(bool isReal)
    {
        return isReal ? m_actorsHitByGroundFireThisTurn : m_actorsHitByGroundFireThisTurn_Fake;
    }

    // custom
    public void AddPendingShield(int shieldAmount)
    {
        m_pendingShield += shieldAmount;
    }

    // custom
    public void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
    {
        if (caster.GetTeam() != target.GetTeam())
        {
            if (results.m_hitParameters.Effect is FireborgGroundFireEffect)
            {
                caster.GetFreelancerStats().AddToValueOfStat(
                    FreelancerStats.FireborgStats.GroundFireDamage,
                    results.FinalDamage);
            }
            else if (results.m_hitParameters.Effect is FireborgIgnitedEffect)
            {
                caster.GetFreelancerStats().AddToValueOfStat(
                    FreelancerStats.FireborgStats.IgniteDamage,
                    results.FinalDamage);
            }
        }
    }
#endif
}