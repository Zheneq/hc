using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Evos.ActorStatus
{
#if EVOS
    // TODO Remove statuses on death
    // TODO Can we put casters' names in debuffs?
    public class EvosActorStatusManager: MonoBehaviour
    {
        private static EvosActorStatusManager s_instance;

        private readonly Dictionary<int, AppliedStatusInfo> AppliedStatuses = new Dictionary<int, AppliedStatusInfo>();

        private readonly Dictionary<ActorData, List<EvosActorStatusType>> PendingRemoval = new Dictionary<ActorData, List<EvosActorStatusType>>(); // TODO remove?

        private const int NO_SEQUENCE_ID = -1; // TODO can't use same id for multiple actors
    
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
                PendingRemoval.Clear();
                foreach (ActorData actorData in GameFlowData.Get().GetActors())
                {
                    Log.Info($"EvosActorStatusManager: Initializing pending removal for {actorData}");
                    PendingRemoval.Add(actorData, new List<EvosActorStatusType>());
                }
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
            switch (statusInfo.SequenceType)
            {
                case SequenceType.Normal:
                    StartEffect(sequences.First(), statusInfo);
                    break;
            }
        }

        private void StartEffect(Sequence sequence, SequenceStatusInfo statusInfo)
        {
            ActorData[] sequenceTargets = statusInfo.UseCaster ? new [] { sequence.Caster } : sequence.Targets;
            if (sequenceTargets.IsNullOrEmpty())
            {
                Log.Warning($"StartEffect: no targets for {statusInfo}");
                return;
            }

            if (sequenceTargets.Contains(null))
            {
                Log.Warning($"EvosActorStatusManager.OnSequenceAdded: Target actors for {statusInfo.Type} contains nulls");
                sequenceTargets = sequenceTargets.Where(x => !(x is null)).ToArray();
            }

            AddStatus(sequenceTargets, statusInfo.Type, sequence.Id);
        }

        public void OnSequenceHit(Sequence sequence, SequenceSource source, ActorData target)
        {
            SequenceStatusInfo statusInfo =
                EvosActorStatusRepo.GetSequenceStatusInfoByPrefabId(sequence.PrefabLookupId);
            
            if (!(statusInfo is null)
                && statusInfo.PrimaryPrefabId > 0
                && !(SequenceManager.Get() is null))
            {
                Log.Info($"OnSequenceHit: looking for initial sequence for {sequence}");
                foreach (Sequence initialSequence in SequenceManager.Get().GetSequencesForSource(source))
                {
                    if (initialSequence.PrefabLookupId == statusInfo.PrimaryPrefabId
                        && initialSequence.Targets.Contains(target))
                    {
                        sequence = initialSequence;
                        break;
                    }
                }
            }

            Log.Info($"OnSequenceHit: {statusInfo} {sequence}");
            if (statusInfo is null)
            {
                return;
            }

            switch (statusInfo.SequenceType)
            {
                case SequenceType.RemoveOnHit:
                    EndEffect(sequence);
                    break;
            }
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

            RemoveStatus(info.Actors, info.Status, sequence.Id);
        }
        
        // TODO optimize?
        public int GetStatusCount(ActorData actor, EvosActorStatusType status)
        {
            return AppliedStatuses.Values
                .Count(appliedStatusInfo =>
                    appliedStatusInfo.Status == status
                    && appliedStatusInfo.Actors.Contains(actor));
        }

        public bool AddStatus(ActorData[] targetActors, EvosActorStatusType evosStatusType, int sequenceId)
        {
            Log.Info($"EvosActorStatusManager.AddStatus: {evosStatusType} {string.Join(",", targetActors.Select(a => a.ToString()).ToArray())}");
            bool result = UpdateStatus(targetActors, evosStatusType, (nameplate, type) => nameplate.AddStatus(type));
            if (result)
            {
                AppliedStatuses[sequenceId] = new AppliedStatusInfo(targetActors,evosStatusType);
            }

            return result;
        }

        public bool RemoveStatus(ActorData[] targetActors, EvosActorStatusType evosStatusType, int sequenceId)
        {
            Log.Info($"EvosActorStatusManager.RemoveStatus: {evosStatusType} {string.Join(",", targetActors.Select(a => a.ToString()).ToArray())}");
            bool result = UpdateStatus(targetActors, evosStatusType, (nameplate, type) => nameplate.RemoveStatus(type));
            if (AppliedStatuses.Remove(sequenceId))
            {
                foreach (ActorData targetActor in targetActors)
                {
                    GetPendingRemovalFor(targetActor).Add(evosStatusType);
                }
            }

            return result;
        }

        private List<EvosActorStatusType> GetPendingRemovalFor(ActorData actor)
        {
            if (!PendingRemoval.TryGetValue(actor, out var result))
            {
                result = new List<EvosActorStatusType>();
                PendingRemoval.Add(actor, new List<EvosActorStatusType>());
                Log.Error($"EvosActorStatusManager: Pending removal not initialized for {actor}");
            }

            return result;
        }

        public bool IsPendingRemoval(ActorData actor, EvosActorStatusType status)
        {
            return GetPendingRemovalFor(actor).Contains(status);
        }

        public bool PendingRemovalProcessed(ActorData actor, EvosActorStatusType status)
        {
            return GetPendingRemovalFor(actor).Remove(status);
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
        
        public void UpdateDinoPowerLevel(ActorData dino)
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
                    RemoveStatus(targetActors, statusToRemove, NO_SEQUENCE_ID);
                }
                return;
            }
            
            int powerLevel = Math.Min(syncComp.m_layerConePowerLevel, ability.GetLayerCount() - 1);
            Log.Info($"EvosActorStatusManager.UpdateDinoPowerLevel: Level {powerLevel} - {dino}");
            Statuses.TryGetValue(powerLevel, out EvosActorStatusType status);
            
            foreach (EvosActorStatusType statusToRemove in Statuses.Values)
            {
                RemoveStatus(targetActors, statusToRemove, NO_SEQUENCE_ID);
            }

            if (status != EvosActorStatusType.NONE)
            {
                AddStatus(targetActors, status, NO_SEQUENCE_ID);
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