using System;

public enum MyMsgType : short
{
	ReplayManagerFile = 0x30,
	DisplayAlert,
	CastAbility,
	LoginRequest,
	LoginResponse,
	AssetsLoadedNotification,
	SpawningObjectsNotification,
	ClientPreparedForGameStartNotification,
	ReconnectReplayStatus,
	ObserverMessage,
	StartResolutionPhase,
	ClientResolutionPhaseCompleted,
	ResolveKnockbacksForActor,
	ClientAssetsLoadingProgressUpdate,
	ServerAssetsLoadingProgressUpdate,
	RunResolutionActionsOutsideResolve,
	SingleResolutionAction,
	ClientRequestTimeUpdate,
	Failsafe_HurryResolutionPhase,
	LeaveGameNotification,
	EndGameNotification,
	ServerMovementStarting,
	ClientMovementPhaseCompleted,
	Failsafe_HurryMovementPhase,
	ClashesAtEndOfMovement,
	ClientFakeActionRequest = 0x7D00,
	ServerFakeActionResponse
}
