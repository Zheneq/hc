using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Evos.ActorStatus
{
#if EVOS
    // TODO Sticky doesn't disappear immediately after exploding (Ice Core does) (react to seq 59? react to sequence hit?)
    // TODO statuses do not show in lower left corner (and when they will, will it work in 4lancer?)
    // TODO how does it work with duplicate characters? (Can we put casters' names in debuffs?)
    public class EvosActorStatusManager: MonoBehaviour
    {
        private static EvosActorStatusManager s_instance;

        private readonly Dictionary<int, AppliedStatusInfo> AppliedStatuses = new Dictionary<int, AppliedStatusInfo>();
    
        public static EvosActorStatusManager Get()
        {
            return s_instance;
        }
    
        public void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
            }
        }

        private void Start()
        {
            GameFlowData.s_onGameStateChanged += OnGameStateChanged;
        }

        public void OnDestroy()
        {
            GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
        }

        public void OnGameStateChanged(GameState newState)
        {
            if (newState == GameState.StartingGame || newState == GameState.EndingGame)
            {
                AppliedStatuses.Clear();
            }
        }

        public void OnTurnTick()
        {
            // List<ActorData> actorDatas = GameFlowData.Get().GetActors().Where(a => a != null).ToList();
            // foreach (ActorData actorData in actorDatas.Where(a => a.m_characterType == CharacterType.Dino))
            // {
            //     UpdateDinoPowerLevel(actorData);
            // } 
        }

        public void OnSequenceAdded(Sequence[] sequences, int prefabID)
        {
            SequenceStatusInfo statusInfo = EvosActorStatusRepo.GetSequenceStatusInfoByPrefabId(prefabID);
            if (statusInfo is null || sequences.IsNullOrEmpty())
            {
                return;
            }
            Log.Info($"OnSequenceAdded: {statusInfo} {string.Join(", ", sequences.Select(x => x.ToString()).ToArray())}");
            StartEffect(sequences.First(), statusInfo);
        }

        private void StartEffect(Sequence sequence, SequenceStatusInfo statusInfo)
        {
            if (sequence.Targets.IsNullOrEmpty())
            {
                return;
            }

            ActorData[] sequenceTargets = statusInfo.UseCaster ? new [] { sequence.Caster } : sequence.Targets;
            if (sequenceTargets.Contains(null))
            {
                Log.Warning($"EvosActorStatusManager.OnSequenceAdded: Target actors for {statusInfo.Type} contains nulls");
                sequenceTargets = sequenceTargets.Where(x => !(x is null)).ToArray();
            }

            if (AddStatus(sequenceTargets, statusInfo.Type))
            {
                AppliedStatuses[sequence.Id] = new AppliedStatusInfo(sequenceTargets, statusInfo.Type);
            }
        }

        public void OnSequenceHit(Sequence sequence)
        {
            SequenceStatusInfo statusInfo =
                EvosActorStatusRepo.GetSequenceStatusInfoByPrefabId(sequence.PrefabLookupId);
            if (statusInfo is null || !statusInfo.RemoveOnHit)
            {
                return;
            }

            Log.Info($"OnSequenceHit: {statusInfo} {sequence}");

            EndEffect(sequence);
        }

        public void OnSequenceRemoved(Sequence sequence)
        {
            EndEffect(sequence);
        }

        private void EndEffect(Sequence sequence)
        {
            int id = sequence.Id;
            if (!AppliedStatuses.TryGetValue(id, out AppliedStatusInfo info))
            {
                return;
            }
            Log.Info($"EndEffect: {sequence}");

            if (RemoveStatus(info.Actors, info.Status))
            {
                AppliedStatuses.Remove(id);
            }
        }
        
        // TODO optimize?
        public int GetStatusCount(ActorData actor, EvosActorStatusType status)
        {
            return AppliedStatuses.Values
                .Count(appliedStatusInfo =>
                    appliedStatusInfo.Status == status
                    && appliedStatusInfo.Actors.Contains(actor));
        }

        public static bool AddStatus(ActorData[] targetActors, EvosActorStatusType evosStatusType)
        {
            Log.Info($"EvosActorStatusManager.AddStatus: {evosStatusType} {string.Join(",", targetActors.Select(a => a.ToString()).ToArray())}");
            return UpdateStatus(targetActors, evosStatusType, (nameplate, type) => nameplate.AddStatus(type));
        }

        public static bool RemoveStatus(ActorData[] targetActors, EvosActorStatusType evosStatusType)
        {
            Log.Info($"EvosActorStatusManager.RemoveStatus: {evosStatusType} {string.Join(",", targetActors.Select(a => a.ToString()).ToArray())}");
            return UpdateStatus(targetActors, evosStatusType, (nameplate, type) => nameplate.RemoveStatus(type));
        }

        private static bool UpdateStatus(
            ActorData[] targetActors,
            EvosActorStatusType evosStatusType,
            Action<UINameplateItem, EvosActorStatusType> method)
        {
            var panel = UIMainScreenPanel.Get();
            if (panel is null)
            {
                Log.Error("EvosActorStatusManager.UpdateStatus: Failed to get UIMainScreenPanel");
                return false;
            }

            foreach (ActorData targetActor in targetActors)
            {
                if (targetActor is null)
                {
                    Log.Error($"EvosActorStatusManager.UpdateStatus: Target actor is null for status {evosStatusType}");
                    continue;
                }

                if (panel.m_nameplatePanel.GetNameplates().TryGetValue(targetActor, out var nameplateItem))
                {
                    method.Invoke(nameplateItem, evosStatusType);
                }
                else
                {
                    Log.Error($"EvosActorStatusManager.UpdateStatus: Failed to get nameplate panel for {targetActor}");
                }
            }

            return true;
        }
    
        private static readonly Dictionary<int, EvosActorStatusType> Statuses = new Dictionary<int, EvosActorStatusType>
        {
            { 0, EvosActorStatusType.Dino_PowerDrive_1 },
            { 1, EvosActorStatusType.Dino_PowerDrive_2 },
            { 2, EvosActorStatusType.Dino_PowerDrive_3 },
        };
        
        public static void UpdateDinoPowerLevel(ActorData dino)
        {
            if (dino == null)
            {
                return;
            }
            
            ActorData[] targetActors = { dino };
            
            var syncComp = dino.GetComponent<Dino_SyncComponent>();
            var ability =
                dino.GetAbilityData()?.GetAbilityOfActionType(AbilityData.ActionType.ABILITY_0) as DinoLayerCones;
            if (syncComp == null || ability == null)
            {
                Log.Error($"EvosActorStatusManager.UpdateDinoPowerLevel: SyncComp or ability for {dino} not found!");
                foreach (EvosActorStatusType statusToRemove in Statuses.Values)
                {
                    RemoveStatus(targetActors, statusToRemove);
                }
                return;
            }
            
            int powerLevel = Math.Min(syncComp.m_layerConePowerLevel, ability.GetLayerCount() - 1);
            Log.Info($"EvosActorStatusManager.UpdateDinoPowerLevel: Level {powerLevel} - {dino}");
            Statuses.TryGetValue(powerLevel, out EvosActorStatusType status);
            
            foreach (EvosActorStatusType statusToRemove in Statuses.Values)
            {
                RemoveStatus(targetActors, statusToRemove);
            }

            if (status != EvosActorStatusType.NONE)
            {
                AddStatus(targetActors, status);
            }
        }
    }

    internal class AppliedStatusInfo
    {
        public readonly ActorData[] Actors;
        public readonly EvosActorStatusType Status;

        public AppliedStatusInfo(ActorData[] actors, EvosActorStatusType status)
        {
            Actors = actors;
            Status = status;
        }
    }
#endif
}