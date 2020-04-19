using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCommon_LayeredRings
{
	public static T GetBestMatchingData<T>(List<T> radiusToDataList, BoardSquare testSquare, Vector3 centerPos, ActorData caster, bool useLastAsFailsafe) where T : RadiusToDataBase
	{
		if (radiusToDataList != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityCommon_LayeredRings.GetBestMatchingData(List<T>, BoardSquare, Vector3, ActorData, bool)).MethodHandle;
			}
			if (radiusToDataList.Count > 0)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (testSquare != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					for (int i = 0; i < radiusToDataList.Count; i++)
					{
						T t = radiusToDataList[i];
						if (useLastAsFailsafe && i == radiusToDataList.Count - 1)
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
							return t;
						}
						bool flag = AreaEffectUtils.IsSquareInConeByActorRadius(testSquare, centerPos, 0f, 360f, t.m_radius, 0f, true, caster, false, default(Vector3));
						if (flag)
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
