// ROGUES
// SERVER
using System.Collections.Generic;

#if SERVER
// added in rogues
public class ThiefSmokeBombEffect : StandardMultiAreaGroundEffect
{
	public ThiefSmokeBombEffect(
		EffectSource parent,
		List<GroundAreaInfo> areaInfoList,
		ActorData caster,
		GroundEffectField fieldInfo)
		: base(parent, areaInfoList, caster, fieldInfo)
	{
	}
}
#endif
