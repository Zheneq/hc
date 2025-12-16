using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Evos.ActorStatus
{
#if EVOS
    public static class EvosActorStatusRepo
    {
        private const string BasePath = "assets/statusicons/";
        
        private static readonly List<EvosActorStatusData> List = new List<EvosActorStatusData>
        {
            new EvosActorStatusData(StatusType.BazookaGirl_StickyBomb, BasePath + "status_effect_bazookagirl_sticky.png", true),
            new EvosActorStatusData(StatusType.Blaster_Overcharged, BasePath + "status_effect_blaster_overcharged.png", false),
            new EvosActorStatusData(StatusType.Claymore_DirtyFighting, BasePath + "status_effect_claymore_dirty.png", true),
            new EvosActorStatusData(StatusType.Dino_PowerDrive, BasePath + "status_effect_dino_powerdrive.png", false),
            new EvosActorStatusData(StatusType.Fireborg_Ignited, BasePath + "status_effect_fireborg_ignited.png", true),
            new EvosActorStatusData(StatusType.Iceborg_IceCore, BasePath + "status_effect_iceborg_icecore.png", true),
            new EvosActorStatusData(StatusType.Ninja_VoidMark, BasePath + "status_effect_ninja_voidmark.png", true),
            new EvosActorStatusData(StatusType.Samurai_Fury, BasePath + "status_effect_samurai_fury.png", false),
        };

        private static readonly Dictionary<StatusType, EvosActorStatusData> Data = List.ToDictionary(x => x.Type);

        private static readonly Dictionary<int, SequenceStatusInfo> PrefabIdToStatusType = new Dictionary<int, SequenceStatusInfo>
        {
            { 57, new SequenceStatusInfo(StatusType.BazookaGirl_StickyBomb, true) },
            { 59, new SequenceStatusInfo(StatusType.BazookaGirl_StickyBomb, false) },
            { 67, new SequenceStatusInfo(StatusType.Blaster_Overcharged, false) },
            { 109, new SequenceStatusInfo(StatusType.Claymore_DirtyFighting, false) },
            // { 0, StatusType.Dino_PowerDrive },
            { 216, new SequenceStatusInfo(StatusType.Fireborg_Ignited, false) },
            { 272, new SequenceStatusInfo(StatusType.Iceborg_IceCore, false) },
            { 389, new SequenceStatusInfo(StatusType.Ninja_VoidMark, false) },
            { 481, new SequenceStatusInfo(StatusType.Samurai_Fury, false) },
        };
    
        // see HUD_UIResources.GetIconForStatusType
        public static HUD_UIResources.StatusTypeIcon GetIconForStatusType(StatusType statusType)
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
                result.type = data.Type;
                result.isDebuff = data.IsDebuff;
                result.displayIcon = true;
                result.displayInStatusList = true;
                result.popupText = StringUtil.GetStatusIconPopupText((int)data.Type);
                result.buffDescription = StringUtil.GetStatusIconBuffDesc((int)data.Type);
                result.buffName = StringUtil.GetStatusIconBuffName((int)data.Type);
            }

            return result;
        }

        public static SequenceStatusInfo GetSequenceStatusInfoByPrefabId(int id)
        {
            return PrefabIdToStatusType.TryGetValue(id, out var status) ? status : null;
        }
    }

    public class SequenceStatusInfo
    {
        public readonly StatusType Type;
        public readonly bool RemoveOnHit; // TODO something more straightforward

        public SequenceStatusInfo(StatusType type, bool removeOnHit)
        {
            Type = type;
            RemoveOnHit = removeOnHit;
        }

        public override string ToString()
        {
            return $"<{Type.ToString()} RemoveOnHit={RemoveOnHit}>";
        }
    }

    public class EvosActorStatusData
    {
        public readonly StatusType Type;
        public readonly string AssetPath;
        public readonly bool IsDebuff;

        public EvosActorStatusData(StatusType type, string assetPath, bool isDebuff)
        {
            Type = type;
            AssetPath = assetPath;
            IsDebuff = isDebuff;
        }
    }
#endif
}