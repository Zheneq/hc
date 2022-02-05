// ROGUES
// server-only, missing in reactor
#if SERVER
public enum MiscHitEventType
{
	ClearCharacterAbilityCooldowns,
	CasterForceChaseTarget,
	TargetForceChaseCaster,
	OverrideCasterAbilityCooldown,
	GroundEffectStatusChange,
	AddToCasterAbilityCooldown,
	ProgressCasterStockRefreshTime,
	AddToCasterStockRemaining,
	ResetCasterStockRefreshTime,
	UpdateEffect,
	UpdatePassive,
	UpdateFreelancerStats
}
#endif
