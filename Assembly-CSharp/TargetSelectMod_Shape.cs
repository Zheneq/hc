using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TargetSelectMod_Shape : TargetSelectModBase
{
	public AbilityModPropertyShape m_shapeMod;

	public bool m_useAdditionalShapeOverrides;

	public List<AbilityAreaShape> m_additionalShapesOverrides = new List<AbilityAreaShape>();

	[Header("-- For require targeting on actors")]
	public AbilityModPropertyBool m_requireTargetingOnActorMod;

	public AbilityModPropertyBool m_canTargetOnEnemiesMod;

	public AbilityModPropertyBool m_canTargetOnAlliesMod;

	public AbilityModPropertyBool m_canTargetOnSelfMod;

	public AbilityModPropertyBool m_ignoreLosToTargetActorMod;

	[Separator("Use Move Shape Targeter? (for moving a shape similar to Grey drone)", true)]
	public AbilityModPropertyFloat m_moveLineWidthMod;

	public override string GetModSpecificInEditorDesc(GenericAbility_TargetSelectBase targetSelectBase, string header)
	{
		string text = string.Empty;
		TargetSelect_Shape targetSelect_Shape = targetSelectBase as TargetSelect_Shape;
		if (targetSelect_Shape != null)
		{
			text += AbilityModHelper.GetModPropertyDesc(this.m_shapeMod, "[Shape]", true, targetSelect_Shape.m_shape);
			if (this.m_useAdditionalShapeOverrides)
			{
				if (this.m_additionalShapesOverrides != null)
				{
					text += "-- Using additional shape overrides --\n";
					using (List<AbilityAreaShape>.Enumerator enumerator = this.m_additionalShapesOverrides.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							AbilityAreaShape abilityAreaShape = enumerator.Current;
							text = text + "\t" + abilityAreaShape.ToString() + "\n";
						}
					}
				}
			}
			text += AbilityModHelper.GetModPropertyDesc(this.m_requireTargetingOnActorMod, "[RequireTargetingOnActor]", true, targetSelect_Shape.m_requireTargetingOnActor);
			text += AbilityModHelper.GetModPropertyDesc(this.m_canTargetOnEnemiesMod, "[CanTargetOnEnemies]", true, targetSelect_Shape.m_canTargetOnEnemies);
			text += AbilityModHelper.GetModPropertyDesc(this.m_canTargetOnAlliesMod, "[CanTargetOnAllies]", true, targetSelect_Shape.m_canTargetOnAllies);
			text += AbilityModHelper.GetModPropertyDesc(this.m_canTargetOnSelfMod, "[CanTargetOnSelf]", true, targetSelect_Shape.m_canTargetOnSelf);
			text += AbilityModHelper.GetModPropertyDesc(this.m_ignoreLosToTargetActorMod, "[IgnoreLosToTargetActor]", true, targetSelect_Shape.m_ignoreLosToTargetActor);
			text += AbilityModHelper.GetModPropertyDesc(this.m_moveLineWidthMod, "[MoveLineWidth]", true, targetSelect_Shape.m_moveLineWidth);
		}
		return text;
	}
}
