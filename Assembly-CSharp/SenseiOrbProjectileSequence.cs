// ROGUES
// SERVER
using UnityEngine;

// identical in reactor and rogues
public class SenseiOrbProjectileSequence : ArcingProjectileSequence
{
	internal override Vector3 GetStartPos()
	{
		if (Caster.GetActorModelData() != null)
		{
			SenseiOrbVfxController component = Caster.GetActorModelData().gameObject.GetComponent<SenseiOrbVfxController>();
			if (component != null)
			{
				return component.GetSpawnPosAndAdvanceCounter();
			}
		}
		return base.GetStartPos();
	}
}
