using UnityEngine;

public class SenseiOrbProjectileSequence : ArcingProjectileSequence
{
	internal override Vector3 GetStartPos()
	{
		if (base.Caster.GetActorModelData() != null)
		{
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
