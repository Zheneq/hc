// ROGUES
// SERVER
using UnityEngine;

// same in reactor & rogues
public class ValkyrieNarrowingProjectileSequence : ArcingProjectileSequence
{
	private float m_totalLengthInSquares = -1f;
	private float m_totalWidthInSquares = -1f;

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (!(extraSequenceParams is FxAttributeParam fxAttributeParam)
			    || fxAttributeParam.m_paramNameCode == FxAttributeParam.ParamNameCode.None
			    || fxAttributeParam.m_paramTarget != FxAttributeParam.ParamTarget.MainVfx)
			{
				continue;
			}
			if (fxAttributeParam.m_paramNameCode == FxAttributeParam.ParamNameCode.LengthInSquares)
			{
				m_totalLengthInSquares = fxAttributeParam.m_paramValue;
			}
			else if (fxAttributeParam.m_paramNameCode == FxAttributeParam.ParamNameCode.WidthInSquares)
			{
				m_totalWidthInSquares = fxAttributeParam.m_paramValue;
			}
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (m_fx == null
		    || !m_fx.activeSelf
		    || m_totalTravelDist2D <= 0f
		    || m_totalLengthInSquares <= 0f
		    || m_totalWidthInSquares <= 0f)
		{
			return;
		}
		float num = Mathf.Min(1f, VectorUtils.HorizontalPlaneDistInWorld(m_fxSpawnPos, m_fx.transform.position) / m_totalTravelDist2D);
		float width = (1f - num) * m_totalLengthInSquares * (m_totalWidthInSquares / m_totalLengthInSquares);
		SetAttribute(m_fx, "widthInSquares", width);
	}
}
