using UnityEngine;

public class RobotAnimalStealth_Charge : Ability
{
	[Header("-- Anim")]
	public float m_recoveryTime = 0.5f;

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}
}
