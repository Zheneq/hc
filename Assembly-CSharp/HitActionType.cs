// ROGUES
// SERVER
// server-only, missing in reactor
#if SERVER
public enum HitActionType
{
	Damage,
	Healing,
	TechPointsGain,
	TechPointsLoss,
	ObjectivePointChange
}
#endif
