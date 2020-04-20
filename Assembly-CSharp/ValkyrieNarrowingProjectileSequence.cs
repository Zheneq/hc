using System;
using UnityEngine;

public class ValkyrieNarrowingProjectileSequence : ArcingProjectileSequence
{
	private float m_totalLengthInSquares = -1f;

	private float m_totalWidthInSquares = -1f;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		base.Initialize(extraParams);
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			if (extraSequenceParams is Sequence.FxAttributeParam)
			{
				Sequence.FxAttributeParam fxAttributeParam = extraSequenceParams as Sequence.FxAttributeParam;
				if (fxAttributeParam != null)
				{
					if (fxAttributeParam.m_paramNameCode != Sequence.FxAttributeParam.ParamNameCode.None && fxAttributeParam.m_paramTarget == Sequence.FxAttributeParam.ParamTarget.MainVfx)
					{
						if (fxAttributeParam.m_paramNameCode == Sequence.FxAttributeParam.ParamNameCode.LengthInSquares)
						{
							this.m_totalLengthInSquares = fxAttributeParam.m_paramValue;
						}
						else if (fxAttributeParam.m_paramNameCode == Sequence.FxAttributeParam.ParamNameCode.WidthInSquares)
						{
							this.m_totalWidthInSquares = fxAttributeParam.m_paramValue;
						}
					}
				}
			}
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.m_fx != null && this.m_fx.activeSelf)
		{
			if (this.m_totalTravelDist2D > 0f)
			{
				if (this.m_totalLengthInSquares > 0f && this.m_totalWidthInSquares > 0f)
				{
					float num = Mathf.Min(1f, VectorUtils.HorizontalPlaneDistInWorld(this.m_fxSpawnPos, this.m_fx.transform.position) / this.m_totalTravelDist2D);
					float value = (1f - num) * this.m_totalLengthInSquares * (this.m_totalWidthInSquares / this.m_totalLengthInSquares);
					Sequence.SetAttribute(this.m_fx, "widthInSquares", value);
				}
			}
		}
	}
}
