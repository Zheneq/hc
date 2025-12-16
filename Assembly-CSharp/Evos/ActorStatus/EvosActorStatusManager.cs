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

        public void OnSequenceAdded(Sequence[] sequences, int prefabID)
        {
            SequenceStatusInfo statusInfo = EvosActorStatusRepo.GetSequenceStatusInfoByPrefabId(prefabID);
            if (statusInfo is null || sequences.IsNullOrEmpty())
            {
                return;
            }
            Log.Info($"OnSequenceAdded: {statusInfo} {string.Join(", ", sequences.Select(x => x.ToString()).ToArray())}");
            StartEffect(sequences.First(), statusInfo.Type);
        }

        private void StartEffect(Sequence sequence, StatusType status)
        {
            if (sequence.Targets.IsNullOrEmpty())
            {
                return;
            }
            var panel = UIMainScreenPanel.Get();
            if (panel is null)
            {
                Log.Error("EvosActorStatusManager.OnSequenceAdded: Failed to get UIMainScreenPanel");
                return;
            }
            foreach (ActorData targetActor in sequence.Targets)
            {
                if (panel.m_nameplatePanel.GetNameplates().TryGetValue(targetActor, out var nameplateItem))
                {
                    nameplateItem.AddStatus(status);
                }
                else
                {
                    Log.Error($"EvosActorStatusManager.OnSequenceAdded: Failed to get nameplate panel for {targetActor}");
                }
            }
            AppliedStatuses[sequence.Id] = new AppliedStatusInfo(sequence.Targets, status);
        }

        public void OnSequenceHit(Sequence sequence)
        {
            SequenceStatusInfo statusInfo = EvosActorStatusRepo.GetSequenceStatusInfoByPrefabId(sequence.PrefabLookupId);
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
            
            var panel = UIMainScreenPanel.Get();
            if (panel is null)
            {
                Log.Error("EvosActorStatusManager.OnSequenceRemoved: Failed to get UIMainScreenPanel");
                return;
            }
            
            foreach (ActorData targetActor in info.Actors)
            {
                if (panel.m_nameplatePanel.GetNameplates().TryGetValue(targetActor, out var nameplateItem))
                {
                    nameplateItem.RemoveStatus(info.Status);
                }
                else
                {
                    Log.Error($"EvosActorStatusManager.OnSequenceRemoved: Failed to get nameplate panel for {targetActor}");
                }
            }

            AppliedStatuses.Remove(id);
        }
    }

    internal class AppliedStatusInfo
    {
        public readonly ActorData[] Actors;
        public readonly StatusType Status;

        public AppliedStatusInfo(ActorData[] actors, StatusType status)
        {
            Actors = actors;
            Status = status;
        }
    }
#endif
}