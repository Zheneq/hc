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
            new EvosActorStatusData(EvosActorStatusType.BazookaGirl_StickyBomb, BasePath + "status_effect_bazookagirl_sticky.png", true),
            new EvosActorStatusData(EvosActorStatusType.Blaster_Overcharged, BasePath + "status_effect_blaster_overcharged.png", false),
            new EvosActorStatusData(EvosActorStatusType.Claymore_DirtyFighting, BasePath + "status_effect_claymore_dirty.png", true),
            new EvosActorStatusData(EvosActorStatusType.Dino_PowerDrive_1, BasePath + "status_effect_dino_powerdrive.png", false), // TODO
            new EvosActorStatusData(EvosActorStatusType.Dino_PowerDrive_2, BasePath + "status_effect_dino_powerdrive.png", false), // TODO
            new EvosActorStatusData(EvosActorStatusType.Dino_PowerDrive_3, BasePath + "status_effect_dino_powerdrive.png", false), // TODO
            new EvosActorStatusData(EvosActorStatusType.Fireborg_Ignited, BasePath + "status_effect_fireborg_ignited.png", true),
            new EvosActorStatusData(EvosActorStatusType.Iceborg_IceCore, BasePath + "status_effect_iceborg_icecore.png", true),
            new EvosActorStatusData(EvosActorStatusType.Ninja_VoidMark, BasePath + "status_effect_ninja_voidmark.png", true),
            new EvosActorStatusData(EvosActorStatusType.Samurai_Fury, BasePath + "status_effect_samurai_fury.png", false),
        };

        private static readonly Dictionary<EvosActorStatusType, EvosActorStatusData> Data = List.ToDictionary(x => x.Type);

        private static readonly Dictionary<int, SequenceStatusInfo> PrefabIdToStatusType = new Dictionary<int, SequenceStatusInfo>
        {
            { 57, new SequenceStatusInfo(EvosActorStatusType.BazookaGirl_StickyBomb) }, // TODO it would be cool if it removed on explosion
            { 59, new SequenceStatusInfo(EvosActorStatusType.BazookaGirl_StickyBomb, SequenceType.RemoveOnHit, primaryPrefabId: 57) }, // is added and hits on explosion
            { 67, new SequenceStatusInfo(EvosActorStatusType.Blaster_Overcharged) },
            { 109, new SequenceStatusInfo(EvosActorStatusType.Claymore_DirtyFighting) },
            // { 0, EvosStatusType.Dino_PowerDrive },
            { 216, new SequenceStatusInfo(EvosActorStatusType.Fireborg_Ignited) },
            { 272, new SequenceStatusInfo(EvosActorStatusType.Iceborg_IceCore) },
            { 389, new SequenceStatusInfo(EvosActorStatusType.Ninja_VoidMark, useCaster: true) },
            // { 481, new SequenceStatusInfo(EvosStatusType.Samurai_Fury, false) },
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
                result.popupText = StringUtil.GetStatusIconPopupText(1000 + (int)data.Type); // TODO magic numbers
                result.buffDescription = StringUtil.GetStatusIconBuffDesc(1000 + (int)data.Type);
                result.buffName = StringUtil.GetStatusIconBuffName(1000 + (int)data.Type);
            }

            return result;
        }

        public static SequenceStatusInfo GetSequenceStatusInfoByPrefabId(int id)
        {
            return PrefabIdToStatusType.TryGetValue(id, out var status) ? status : null;
        }
    }

    public enum SequenceType
    {
        NONE,
        
        Normal,
        RemoveOnHit,
    }

    public class SequenceStatusInfo
    {
        public readonly EvosActorStatusType Type;
        public readonly SequenceType SequenceType; // TODO something more straightforward
        public readonly bool UseCaster; // instead of target
        public readonly int PrimaryPrefabId; // TODO something more straightforward

        public SequenceStatusInfo(
            EvosActorStatusType type,
            SequenceType sequenceType = SequenceType.Normal,
            bool useCaster = false,
            int primaryPrefabId = -1)
        {
            Type = type;
            SequenceType = sequenceType;
            UseCaster = useCaster;
            PrimaryPrefabId = primaryPrefabId;
        }

        public override string ToString()
        {
            return $"<{Type.ToString()} {SequenceType} RemoveOnHit={SequenceType} UseCaster={UseCaster}>";
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