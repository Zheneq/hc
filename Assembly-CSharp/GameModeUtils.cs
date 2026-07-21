// SERVER
// ROGUES
using UnityEngine;

public static class GameModeUtils
{
#if SERVER
    // added in rogues
    public static bool IsCtfGameModeEvent(GameModeEvent gameModeEvent)
    {
        return gameModeEvent != null && IsCtfGameModeEventType(gameModeEvent.m_eventType);
    }

    // added in rogues
    public static bool IsCtcGameModeEvent(GameModeEvent gameModeEvent)
    {
        return gameModeEvent != null && IsCtcGameModeEventType(gameModeEvent.m_eventType);
    }
#endif

    public static bool IsCtfGameModeEvent(ClientGameModeEvent gameModeEvent)
    {
        return gameModeEvent != null && IsCtfGameModeEventType(gameModeEvent.m_eventType);
    }

    public static bool IsCtcGameModeEvent(ClientGameModeEvent gameModeEvent)
    {
        return gameModeEvent != null && IsCtcGameModeEventType(gameModeEvent.m_eventType);
    }

    public static bool IsCtfGameModeEventType(GameModeEventType gameModeEventType)
    {
        switch (gameModeEventType)
        {
            case GameModeEventType.Ctf_FlagPickedUp:
            case GameModeEventType.Ctf_FlagDropped:
            case GameModeEventType.Ctf_FlagTurnedIn:
            case GameModeEventType.Ctf_FlagSentToSpawn:
                return true;
            default:
                return false;
        }
    }

    public static bool IsCtcGameModeEventType(GameModeEventType gameModeEventType)
    {
        switch (gameModeEventType)
        {
            case GameModeEventType.Ctc_CoinPickedUp:
            case GameModeEventType.Ctc_CoinsDropped:
            case GameModeEventType.Ctc_NonCoinPowerupTouched:
            case GameModeEventType.Ctc_CoinPowerupTouched:
                return true;
            default:
                return false;
        }
    }

#if SERVER
    // added in rogues
    public static MovementResults BuildGameModeEventMovementResults(
        ActorData mover,
        BoardSquarePathInfo triggeringPathSegment,
        MovementStage movementStage,
        GameModeEvent gameModeEvent,
        GameObject sequencePrefab,
        StandardEffectInfo effectInfo)
    {
        var triggeringPathInfo = new ServerAbilityUtils.TriggeringPathInfo(mover, triggeringPathSegment);
        
        ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(triggeringPathInfo));
        actorHitResults.CanBeReactedTo = false;
        actorHitResults.AddGameModeEvent(gameModeEvent);
        actorHitResults.AddStandardEffectInfo(effectInfo);
        
        MovementResults movementResults = new MovementResults(movementStage);
        movementResults.SetupTriggerData(mover, triggeringPathSegment);
        movementResults.SetupGameplayData(GameWideData.Get().m_gameModeAbility, actorHitResults);
        
        SequenceSource sequenceSource = new SequenceSource(null, null, false);
        ServerClientUtils.SequenceStartData startData = new ServerClientUtils.SequenceStartData(
            sequencePrefab,
            triggeringPathSegment.square,
            null,
            mover,
            sequenceSource,
            null);
        movementResults.AddSequenceStartOverride(startData, sequenceSource, false);
        
        return movementResults;
    }
#endif
}