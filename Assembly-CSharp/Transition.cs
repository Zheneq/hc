using System;

[Serializable]
public enum Transition
{
	NullTransition,
	DoNothing,
	Patrol,
	TookDamage,
	Healed,
	Buffed
}
