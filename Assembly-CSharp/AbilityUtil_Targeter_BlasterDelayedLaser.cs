using UnityEngine;

public class AbilityUtil_Targeter_BlasterDelayedLaser : AbilityUtil_Targeter_Laser
{
	private Blaster_SyncComponent m_syncComp;

	private bool m_aimAtCasterOnDetonate;

	public AbilityUtil_Targeter_BlasterDelayedLaser(Ability ability, Blaster_SyncComponent syncComp, bool aimAtCasterOnDetonate, float width, float distance, bool penetrateLoS, int maxTargets = -1, bool affectsAllies = false, bool affectsCaster = false)
		: base(ability, width, distance, penetrateLoS, maxTargets, affectsAllies, affectsCaster)
	{
		m_syncComp = syncComp;
		m_aimAtCasterOnDetonate = aimAtCasterOnDetonate;
	}

	public override Vector3 GetStartLosPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (m_syncComp.m_canActivateDelayedLaser)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_syncComp.m_delayedLaserStartPos;
				}
			}
		}
		return base.GetStartLosPos(currentTarget, targetingActor);
	}

	public override Vector3 GetAimDirection(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (m_syncComp.m_canActivateDelayedLaser)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Vector3 result = m_syncComp.m_delayedLaserAimDir;
					if (m_aimAtCasterOnDetonate && targetingActor.GetCurrentBoardSquare() != null)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						Vector3 vector = targetingActor.GetCurrentBoardSquare().ToVector3() - m_syncComp.m_delayedLaserStartPos;
						vector.y = 0f;
						vector.Normalize();
						if (vector.magnitude > 0f)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							result = vector;
						}
					}
					return result;
				}
				}
			}
		}
		return base.GetAimDirection(currentTarget, targetingActor);
	}
}
