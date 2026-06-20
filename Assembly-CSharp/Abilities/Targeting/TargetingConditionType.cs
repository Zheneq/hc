// ROGUES
// SERVER
// added in rogues
#if SERVER
public enum TargetingConditionType
{
	Condition_None,
	Condition_SelfHP,
	Condition_TargetLowestHP,
	Condition_TargetHighestHP,
	Condition_TargetCount,
	Condition_HitChance,
	Condition_CritChance,
	Condition_TotalDamage,
	Condition_TotalHealing,
	Condition_SelfStatus,
	Condition_TargetStatus,
	Condition_LethalCount,
	Condition_DistanceToNearestTarget,
	Condition_DistanceToFarthestTarget,
	Condition_DistanceToNearestEnemy,
	Condition_DistanceToNearestAlly,
	Condition_DistanceToFarthestEnemy,
	Condition_DistanceToFarthestAlly
}
#endif
