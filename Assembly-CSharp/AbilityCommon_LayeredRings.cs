using System.Collections.Generic;
using UnityEngine;

public class AbilityCommon_LayeredRings
{
	public static T GetBestMatchingData<T>(List<T> radiusToDataList, BoardSquare testSquare, Vector3 centerPos, ActorData caster, bool useLastAsFailsafe) where T : RadiusToDataBase
	{
		if (radiusToDataList != null)
		{
			while (true)
			{
				switch (6)
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
			if (radiusToDataList.Count > 0)
			{
				while (true)
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
					while (true)
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
						T val = radiusToDataList[i];
						if (useLastAsFailsafe && i == radiusToDataList.Count - 1)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									return val;
								}
							}
						}
						if (!AreaEffectUtils.IsSquareInConeByActorRadius(testSquare, centerPos, 0f, 360f, val.m_radius, 0f, true, caster))
						{
							continue;
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							return val;
						}
					}
				}
			}
		}
		return (T)null;
	}

	public static Sequence.IExtraSequenceParams[] GetAdjustableRingSequenceParams(float radiusInSquares)
	{
		Sequence.FxAttributeParam fxAttributeParam = new Sequence.FxAttributeParam();
		fxAttributeParam.m_paramNameCode = Sequence.FxAttributeParam.ParamNameCode.ScaleControl;
		fxAttributeParam.m_paramTarget = Sequence.FxAttributeParam.ParamTarget.MainVfx;
		fxAttributeParam.m_paramValue = 2f * radiusInSquares;
		Sequence.FxAttributeParam fxAttributeParam2 = new Sequence.FxAttributeParam();
		fxAttributeParam2.m_paramNameCode = Sequence.FxAttributeParam.ParamNameCode.ScaleControl;
		fxAttributeParam2.m_paramTarget = Sequence.FxAttributeParam.ParamTarget.ImpactVfx;
		fxAttributeParam2.m_paramValue = 2f * radiusInSquares;
		return new Sequence.IExtraSequenceParams[2]
		{
			fxAttributeParam,
			fxAttributeParam2
		};
	}
}
