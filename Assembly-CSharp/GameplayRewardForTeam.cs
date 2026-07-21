// SERVER
// ROGUES
using System;
using UnityEngine;

[Serializable]
public class GameplayRewardForTeam
{
    public string m_name = "Gameplay Reward";
    public int m_objectivePointAdjust;
    public StandardEffectInfo m_effectOnMembers;
    public int m_creditsToMembers; // TODO LOW removed in rogues
    public int m_healingToMembers;
    public int m_techPointsToMembers;
    public AbilityStatMod[] m_permanentStatMods;
    public StatusType[] m_permanentStatusChanges;

#if SERVER
    // added in rogues
    public void ApplyRewardTo(Team team)
    {
        if (m_objectivePointAdjust != 0)
        {
            ObjectivePoints objectivePoints = ObjectivePoints.Get();
            if (objectivePoints != null)
            {
                objectivePoints.AdjustPoints(m_objectivePointAdjust, team);
            }
        }

        foreach (ActorData actorData in GameFlowData.Get().GetAllTeamMembers(team))
        {
            if (m_healingToMembers > 0)
            {
                DamageSource src = new DamageSource(m_name, true, actorData.GetFreePos());
                ServerCombatManager.Get().Heal(
                    src,
                    actorData,
                    actorData,
                    m_healingToMembers,
                    ServerCombatManager.HealingType.Ability);
            }

            if (m_techPointsToMembers > 0)
            {
                DamageSource src = new DamageSource(m_name, true, actorData.GetFreePos());
                ServerCombatManager.Get().TechPointGain(
                    src,
                    actorData,
                    actorData,
                    m_techPointsToMembers,
                    ServerCombatManager.TechPointChangeType.Ability);
            }
            else if (m_techPointsToMembers < 0)
            {
                DamageSource src = new DamageSource(m_name, true, actorData.GetFreePos());
                ServerCombatManager.Get().TechPointLoss(
                    src,
                    actorData,
                    actorData,
                    Mathf.Abs(m_techPointsToMembers),
                    ServerCombatManager.TechPointChangeType.Ability);
            }

            if (m_permanentStatMods.Length != 0)
            {
                ActorStats actorStats = actorData.GetActorStats();
                foreach (AbilityStatMod statMod in m_permanentStatMods)
                {
                    actorStats.AddStatMod(statMod);
                }
            }

            if (m_permanentStatusChanges.Length != 0)
            {
                ActorStatus actorStatus = actorData.GetActorStatus();
                foreach (StatusType status in m_permanentStatusChanges)
                {
                    actorStatus.AddStatus(status, 0);
                }
            }

            if (m_effectOnMembers.m_applyEffect)
            {
                Effect effect = new StandardActorEffect(
                    new EffectSource(m_name, null, null),
                    actorData.GetCurrentBoardSquare(),
                    actorData,
                    actorData,
                    m_effectOnMembers.m_effectData);
                ServerEffectManager.Get().ApplyEffect(effect);
            }
        }
    }
#endif

    public void ClientApplyRewardTo(Team team)
    {
        if (m_objectivePointAdjust == 0)
        {
            return;
        }

        ObjectivePoints objectivePoints = ObjectivePoints.Get();
        if (objectivePoints != null)
        {
            objectivePoints.AdjustUnresolvedPoints(m_objectivePointAdjust, team);
        }
    }
}