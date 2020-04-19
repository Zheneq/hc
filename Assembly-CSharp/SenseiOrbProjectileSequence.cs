using System;
using UnityEngine;

public class SenseiOrbProjectileSequence : ArcingProjectileSequence
{
	internal override Vector3 GetStartPos()
	{
		if (base.Caster.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiOrbProjectileSequence.GetStartPos()).MethodHandle;
			}
			SenseiOrbVfxController component = base.Caster.\u000E().gameObject.GetComponent<SenseiOrbVfxController>();
			if (component != null)
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
				return component.GetSpawnPosAndAdvanceCounter();
			}
		}
		return base.GetStartPos();
	}
}
