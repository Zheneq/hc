using System;
using System.Collections.Generic;
using System.Text;
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
			text += AbilityModHelper.GetModPropertyDesc(m_shapeMod, "[Shape]", true, targetSelect_Shape.m_shape);
			if (m_useAdditionalShapeOverrides)
			{
				if (m_additionalShapesOverrides != null)
				{
					text += "-- Using additional shape overrides --\n";
					using (List<AbilityAreaShape>.Enumerator enumerator = m_additionalShapesOverrides.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							text = new StringBuilder().Append(text).Append("\t").Append(enumerator.Current.ToString()).Append("\n").ToString();
						}
					}
				}
			}
			text += AbilityModHelper.GetModPropertyDesc(m_requireTargetingOnActorMod, "[RequireTargetingOnActor]", true, targetSelect_Shape.m_requireTargetingOnActor);
			text += AbilityModHelper.GetModPropertyDesc(m_canTargetOnEnemiesMod, "[CanTargetOnEnemies]", true, targetSelect_Shape.m_canTargetOnEnemies);
			text += AbilityModHelper.GetModPropertyDesc(m_canTargetOnAlliesMod, "[CanTargetOnAllies]", true, targetSelect_Shape.m_canTargetOnAllies);
			text += AbilityModHelper.GetModPropertyDesc(m_canTargetOnSelfMod, "[CanTargetOnSelf]", true, targetSelect_Shape.m_canTargetOnSelf);
			text += AbilityModHelper.GetModPropertyDesc(m_ignoreLosToTargetActorMod, "[IgnoreLosToTargetActor]", true, targetSelect_Shape.m_ignoreLosToTargetActor);
			text += AbilityModHelper.GetModPropertyDesc(m_moveLineWidthMod, "[MoveLineWidth]", true, targetSelect_Shape.m_moveLineWidth);
		}
		return text;
	}
}
