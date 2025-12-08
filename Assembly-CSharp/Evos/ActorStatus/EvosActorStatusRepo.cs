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

        private static readonly Dictionary<int, StatusType> PrefabIdToStatusType = new Dictionary<int, StatusType>
        {
            { 57, StatusType.BazookaGirl_StickyBomb },
            { 67, StatusType.Blaster_Overcharged },
            { 109, StatusType.Claymore_DirtyFighting },
            // { 0, StatusType.Dino_PowerDrive },
            { 216, StatusType.Fireborg_Ignited },
            { 272, StatusType.Iceborg_IceCore },
            { 389, StatusType.Ninja_VoidMark },
            { 481, StatusType.Samurai_Fury },
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

        public static StatusType GetStatusTypeBySequencePrefabId(int id)
        {
            return PrefabIdToStatusType.TryGetValue(id, out var status) ? status : StatusType.INVALID;
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