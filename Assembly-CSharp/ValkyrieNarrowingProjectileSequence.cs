using UnityEngine;

public class ValkyrieNarrowingProjectileSequence : ArcingProjectileSequence
{
	private float m_totalLengthInSquares = -1f;

	private float m_totalWidthInSquares = -1f;

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (extraSequenceParams is FxAttributeParam)
			{
				FxAttributeParam fxAttributeParam = extraSequenceParams as FxAttributeParam;
				if (fxAttributeParam != null && fxAttributeParam.m_paramNameCode != Sequence.FxAttributeParam.ParamNameCode.None && fxAttributeParam.m_paramTarget == Sequence.FxAttributeParam.ParamTarget.MainVfx)
				{
					if (fxAttributeParam.m_paramNameCode == Sequence.FxAttributeParam.ParamNameCode.LengthInSquares)
					{
						m_totalLengthInSquares = fxAttributeParam.m_paramValue;
					}
					else if (fxAttributeParam.m_paramNameCode == Sequence.FxAttributeParam.ParamNameCode.WidthInSquares)
					{
						m_totalWidthInSquares = fxAttributeParam.m_paramValue;
					}
				}
			}
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (m_fx != null && m_fx.activeSelf && m_totalTravelDist2D > 0f && m_totalLengthInSquares > 0f && m_totalWidthInSquares > 0f)
		{
			float num = Mathf.Min(1f, VectorUtils.HorizontalPlaneDistInWorld(m_fxSpawnPos, m_fx.transform.position) / m_totalTravelDist2D);
			float value = (1f - num) * m_totalLengthInSquares * (m_totalWidthInSquares / m_totalLengthInSquares);
			Sequence.SetAttribute(m_fx, "widthInSquares", value);
		}
	}
}
