using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Evos.ActorStatus
{
#if EVOS
    public static class EvosActorStatusRepo
    {
        public const string BasePath = "assets/evos/statusicons/";
        private const int LOC_INDEX_START = 1000;
        
        private static readonly List<EvosActorStatusData> List = new List<EvosActorStatusData>
        {
            new EvosActorStatusData(EvosActorStatusType.BazookaGirl_StickyBomb, BasePath + "status_effect_bazookagirl_sticky.png", true),
            new EvosActorStatusData(EvosActorStatusType.Blaster_Overcharged, BasePath + "status_effect_blaster_overcharged.png", false),
            new EvosActorStatusData(EvosActorStatusType.Claymore_DirtyFighting, BasePath + "status_effect_claymore_dirty.png", true),
            new EvosActorStatusData(EvosActorStatusType.Dino_PowerDrive_1, BasePath + "status_effect_dino_powerdrive_1.png", false),
            new EvosActorStatusData(EvosActorStatusType.Dino_PowerDrive_2, BasePath + "status_effect_dino_powerdrive_2.png", false),
            new EvosActorStatusData(EvosActorStatusType.Dino_PowerDrive_3, BasePath + "status_effect_dino_powerdrive_3.png", false),
            new EvosActorStatusData(EvosActorStatusType.Fireborg_Ignited, BasePath + "status_effect_fireborg_ignited.png", true),
            new EvosActorStatusData(EvosActorStatusType.Iceborg_IceCore, BasePath + "status_effect_iceborg_icecore.png", true),
            new EvosActorStatusData(EvosActorStatusType.Ninja_VoidMark, BasePath + "status_effect_ninja_voidmark.png", true),
            new EvosActorStatusData(EvosActorStatusType.Samurai_Fury, BasePath + "status_effect_samurai_fury.png", false),
            new EvosActorStatusData(EvosActorStatusType.Scamp_Ballistic, BasePath + "status_effect_scamp_ballistic.png", false),
            new EvosActorStatusData(EvosActorStatusType.SpaceMarine_Barrage, BasePath + "status_effect_spacemarine_barrage.png", false),
            new EvosActorStatusData(EvosActorStatusType.Manta_PutridSpray, BasePath + "status_effect_manta_putrid_spray.png", true),
            new EvosActorStatusData(EvosActorStatusType.Tracker_Tracked, BasePath + "status_effect_tracker_tracked.png", true),
        };

        private static readonly Dictionary<EvosActorStatusType, EvosActorStatusData> Data = List.ToDictionary(x => x.Type);

        private static readonly Dictionary<int, SequenceStatusInfo> PrefabIdToStatusType = new Dictionary<int, SequenceStatusInfo>
        {
            { 57, new SequenceStatusInfo(EvosActorStatusType.BazookaGirl_StickyBomb) },
            { 59, new SequenceStatusInfo(EvosActorStatusType.BazookaGirl_StickyBomb, SequenceEventType.RemoveOnHit, primarySequencePrefabId: 57) },
            { 67, new SequenceStatusInfo(EvosActorStatusType.Blaster_Overcharged) },
            { 109, new SequenceStatusInfo(EvosActorStatusType.Claymore_DirtyFighting) },
            { 216, new SequenceStatusInfo(EvosActorStatusType.Fireborg_Ignited) },
            { 272, new SequenceStatusInfo(EvosActorStatusType.Iceborg_IceCore) },
            { 389, new SequenceStatusInfo(EvosActorStatusType.Ninja_VoidMark, useCasterAsTarget: true) },
            { 497, new SequenceStatusInfo(EvosActorStatusType.Scamp_Ballistic) },
            { 301, new SequenceStatusInfo(EvosActorStatusType.SpaceMarine_Barrage) },
            { 291, new SequenceStatusInfo(EvosActorStatusType.Manta_PutridSpray) },
            { 617, new SequenceStatusInfo(EvosActorStatusType.Tracker_Tracked) },
        };
    
        // see HUD_UIResources.GetIconForStatusType
        public static HUD_UIResources.StatusTypeIcon GetIconForStatusType(EvosActorStatusType statusType)
        {
            HUD_UIResources.StatusTypeIcon result = new HUD_UIResources.StatusTypeIcon
            {
                icon = null,
                type = StatusType.INVALID,
                isDebuff = false,
                displayIcon = false,
                displayInStatusList = false,
                popupText = string.Empty,
                buffDescription = string.Empty,
                buffName = string.Empty
            };

            var evosAssetBundleManager = EvosAssetBundleManager.Get();
            if (evosAssetBundleManager != null && Data.TryGetValue(statusType, out var data))
            {
                result.icon = evosAssetBundleManager.LoadAsset<Sprite>(data.AssetPath);
                result.type = StatusType.INVALID;
                result.isDebuff = data.IsDebuff;
                result.displayIcon = true;
                result.displayInStatusList = true;
                result.popupText = StringUtil.GetStatusIconPopupText(LOC_INDEX_START + (int)data.Type);
                result.buffDescription = StringUtil.GetStatusIconBuffDesc(LOC_INDEX_START + (int)data.Type);
                result.buffName = StringUtil.GetStatusIconBuffName(LOC_INDEX_START + (int)data.Type);
            }

            return result;
        }

        public static SequenceStatusInfo GetSequenceStatusInfoByPrefabId(int id)
        {
            return PrefabIdToStatusType.TryGetValue(id, out var status) ? status : null;
        }
    }

    public enum SequenceEventType
    {
        NONE,
        
        Normal,
        RemoveOnHit,
    }

    public class SequenceStatusInfo
    {
        public readonly EvosActorStatusType Type;
        public readonly SequenceEventType SequenceEventType;
        public readonly bool UseCasterAsTarget;
        public readonly int PrimarySequencePrefabId;

        public SequenceStatusInfo(
            EvosActorStatusType type,
            SequenceEventType sequenceEventType = SequenceEventType.Normal,
            bool useCasterAsTarget = false,
            int primarySequencePrefabId = -1)
        {
            Type = type;
            SequenceEventType = sequenceEventType;
            UseCasterAsTarget = useCasterAsTarget;
            PrimarySequencePrefabId = primarySequencePrefabId;
        }

        public override string ToString()
        {
            return $"<{Type.ToString()} {SequenceEventType} UseCasterAsTarget={UseCasterAsTarget} PrimarySequencePrefabId={PrimarySequencePrefabId}>";
        }
    }

    public class EvosActorStatusData
    {
        public readonly EvosActorStatusType Type;
        public readonly string AssetPath;
        public readonly bool IsDebuff;

        public EvosActorStatusData(EvosActorStatusType type, string assetPath, bool isDebuff)
        {
            Type = type;
            AssetPath = assetPath;
            IsDebuff = isDebuff;
        }
    }
#endif
}