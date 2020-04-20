using System;
using UnityEngine;

public class AbilityUtil_Targeter_BlasterDelayedLaser : AbilityUtil_Targeter_Laser
{
	private Blaster_SyncComponent m_syncComp;

	private bool m_aimAtCasterOnDetonate;

	public AbilityUtil_Targeter_BlasterDelayedLaser(Ability ability, Blaster_SyncComponent syncComp, bool aimAtCasterOnDetonate, float width, float distance, bool penetrateLoS, int maxTargets = -1, bool affectsAllies = false, bool affectsCaster = false) : base(ability, width, distance, penetrateLoS, maxTargets, affectsAllies, affectsCaster)
	{
		this.m_syncComp = syncComp;
		this.m_aimAtCasterOnDetonate = aimAtCasterOnDetonate;
	}

	public override Vector3 GetStartLosPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (this.m_syncComp.m_canActivateDelayedLaser)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BlasterDelayedLaser.GetStartLosPos(AbilityTarget, ActorData)).MethodHandle;
			}
			return this.m_syncComp.m_delayedLaserStartPos;
		}
		return base.GetStartLosPos(currentTarget, targetingActor);
	}

	public override Vector3 GetAimDirection(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (this.m_syncComp.m_canActivateDelayedLaser)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BlasterDelayedLaser.GetAimDirection(AbilityTarget, ActorData)).MethodHandle;
			}
			Vector3 result = this.m_syncComp.m_delayedLaserAimDir;
			if (this.m_aimAtCasterOnDetonate && targetingActor.GetCurrentBoardSquare() != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 vector = targetingActor.GetCurrentBoardSquare().ToVector3() - this.m_syncComp.m_delayedLaserStartPos;
				vector.y = 0f;
				vector.Normalize();
				if (vector.magnitude > 0f)
				{
					for (;;)
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
		return base.GetAimDirection(currentTarget, targetingActor);
	}
}
