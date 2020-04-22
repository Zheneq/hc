using UnityEngine;

public class SenseiOrbProjectileSequence : ArcingProjectileSequence
{
	internal override Vector3 GetStartPos()
	{
		if (base.Caster.GetActorModelData() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			SenseiOrbVfxController component = base.Caster.GetActorModelData().gameObject.GetComponent<SenseiOrbVfxController>();
			if (component != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return component.GetSpawnPosAndAdvanceCounter();
					}
				}
			}
		}
		return base.GetStartPos();
	}
}
