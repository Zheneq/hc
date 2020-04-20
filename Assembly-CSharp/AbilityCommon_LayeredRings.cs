using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCommon_LayeredRings
{
	public static T GetBestMatchingData<T>(List<T> radiusToDataList, BoardSquare testSquare, Vector3 centerPos, ActorData caster, bool useLastAsFailsafe) where T : RadiusToDataBase
	{
		if (radiusToDataList != null)
		{
			if (radiusToDataList.Count > 0)
			{
				if (testSquare != null)
				{
					for (int i = 0; i < radiusToDataList.Count; i++)
					{
						T t = radiusToDataList[i];
						if (useLastAsFailsafe && i == radiusToDataList.Count - 1)
						{
							return t;
						}
						bool flag = AreaEffectUtils.IsSquareInConeByActorRadius(testSquare, centerPos, 0f, 360f, t.m_radius, 0f, true, caster, false, default(Vector3));
						if (flag)
						{
							return t;
						}
					}
				}
			}
		}
		return (T)((object)null);
	}

	public static Sequence.IExtraSequenceParams[] GetAdjustableRingSequenceParams(float radiusInSquares)
	{
		return new Sequence.IExtraSequenceParams[]
		{
			new Sequence.FxAttributeParam
			{
				m_paramNameCode = Sequence.FxAttributeParam.ParamNameCode.ScaleControl,
				m_paramTarget = Sequence.FxAttributeParam.ParamTarget.MainVfx,
				m_paramValue = 2f * radiusInSquares
			},
			new Sequence.FxAttributeParam
			{
				m_paramNameCode = Sequence.FxAttributeParam.ParamNameCode.ScaleControl,
				m_paramTarget = Sequence.FxAttributeParam.ParamTarget.ImpactVfx,
				m_paramValue = 2f * radiusInSquares
			}
		};
	}
}
