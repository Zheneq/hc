using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Evos.ActorStatus
{
#if EVOS
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
            var gameManager = GameManager.Get();
            if (gameManager)
            {
                gameManager.OnGameStarted += OnGameStarted;
                gameManager.OnGameStopped += OnGameStopped;
                Log.Info("EvosActorStatusManager started");
            }
            else
            {
                Log.Error("EvosActorStatusManager failed to start");
            }
        }

        private void OnGameStarted()
        {
            ResetEffects();
        }

        private void OnGameStopped(GameResult gameResult)
        {
            ResetEffects();
        }

        public void OnDestroy()
        {
            var gameManager = GameManager.Get();
            if (gameManager)
            {
                gameManager.OnGameAssembling -= OnGameStarted;
                gameManager.OnGameStopped -= OnGameStopped;
            }
        }

        public void OnToggle()
        {
            if (IsEnabled)
            {
                foreach (var appliedStatusInfo in AppliedStatuses.Values)
                {
                    UpdateStatus(appliedStatusInfo.Actors, appliedStatusInfo.Status, (nameplate, type) => nameplate.AddStatus(type));
                }
            }
            else
            {
                foreach (var appliedStatusInfo in AppliedStatuses.Values)
                {
                    UpdateStatus(appliedStatusInfo.Actors, appliedStatusInfo.Status, (nameplate, type) => nameplate.RemoveStatus(type), force: true);
                }
            }
        }

        private static bool IsEnabled => EvosOptions.Get()?.GetOption(EvosOptions.EnableUniqueStatusEffectIcons) ?? false;

        public void ResetEffects()
        {
            AppliedStatuses.Clear();
            Log.Info("EvosActorStatusManager reset");
        }

        public void OnSequenceAdded(Sequence[] sequences, int prefabID)
        {
            SequenceStatusInfo statusInfo = EvosActorStatusRepo.GetSequenceStatusInfoByPrefabId(prefabID);
            if (statusInfo is null || sequences.IsNullOrEmpty())
            {
                return;
            }
            Log.Info($"OnSequenceAdded: {statusInfo} {string.Join(", ", sequences.Select(x => x.ToString()).ToArray())}");
            switch (statusInfo.SequenceEventType)
            {
                case SequenceEventType.Normal:
                    StartEffect(sequences.First(), statusInfo);
                    break;
            }
        }

        private void StartEffect(Sequence sequence, SequenceStatusInfo statusInfo)
        {
            ActorData[] sequenceTargets = statusInfo.UseCasterAsTarget ? new [] { sequence.Caster } : sequence.Targets;
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
                && statusInfo.PrimarySequencePrefabId > 0
                && !(SequenceManager.Get() is null))
            {
                Log.Info($"OnSequenceHit: looking for initial sequence for {sequence}");
                foreach (Sequence initialSequence in SequenceManager.Get().GetSequencesForSource(source))
                {
                    if (initialSequence.PrefabLookupId == statusInfo.PrimarySequencePrefabId
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

            switch (statusInfo.SequenceEventType)
            {
                case SequenceEventType.RemoveOnHit:
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

        public int GetStatusCount(ActorData actor, EvosActorStatusType status)
        {
            if (!IsEnabled || actor.IsDead())
            {
                return 0;
            }
            
            return AppliedStatuses.Values
                .Count(appliedStatusInfo =>
                    appliedStatusInfo.Status == status
                    && appliedStatusInfo.Actors.Contains(actor));
        }

        public void AddStatus(ActorData[] targetActors, EvosActorStatusType evosStatusType, int sequenceId)
        {
            Log.Info($"EvosActorStatusManager.AddStatus: {evosStatusType} {string.Join(",", targetActors.Select(a => a.ToString()).ToArray())}");
            UpdateStatus(targetActors, evosStatusType, (nameplate, type) => nameplate.AddStatus(type));
            AppliedStatuses[sequenceId] = new AppliedStatusInfo(targetActors, evosStatusType);
        }

        public void RemoveStatus(ActorData[] targetActors, EvosActorStatusType evosStatusType, int sequenceId)
        {
            Log.Info($"EvosActorStatusManager.RemoveStatus: {evosStatusType} {string.Join(",", targetActors.Select(a => a.ToString()).ToArray())}");
            UpdateStatus(targetActors, evosStatusType, (nameplate, type) => nameplate.RemoveStatus(type));
            AppliedStatuses.Remove(sequenceId);
        }

        private static void UpdateStatus(
            ActorData[] targetActors,
            EvosActorStatusType evosStatusType,
            Action<UINameplateItem, EvosActorStatusType> method,
            bool force = false)
        {
            if (!IsEnabled && !force)
            {
                return;
            }

            var panel = UIMainScreenPanel.Get();
            if (panel is null)
            {
                Log.Error("EvosActorStatusManager.UpdateStatus: Failed to get UIMainScreenPanel");
                return;
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
            int nonSequenceId = -dino.ActorIndex;
            if (syncComp == null || ability == null)
            {
                Log.Error($"EvosActorStatusManager.UpdateDinoPowerLevel: SyncComp or ability for {dino} not found!");
                foreach (EvosActorStatusType statusToRemove in Statuses.Values)
                {
                    RemoveStatus(targetActors, statusToRemove, nonSequenceId);
                }
                return;
            }
            
            int powerLevel = Math.Min(syncComp.m_layerConePowerLevel, ability.GetLayerCount() - 1);
            Statuses.TryGetValue(powerLevel, out EvosActorStatusType status);
            
            foreach (EvosActorStatusType statusToRemove in Statuses.Values)
            {
                RemoveStatus(targetActors, statusToRemove, nonSequenceId);
            }

            if (status != EvosActorStatusType.NONE)
            {
                AddStatus(targetActors, status, nonSequenceId);
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