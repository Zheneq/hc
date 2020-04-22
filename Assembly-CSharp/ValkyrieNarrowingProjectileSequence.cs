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
			if (!(extraSequenceParams is FxAttributeParam))
			{
				continue;
			}
			while (true)
			{
				switch (3)
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
			FxAttributeParam fxAttributeParam = extraSequenceParams as FxAttributeParam;
			if (fxAttributeParam == null)
			{
				continue;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (fxAttributeParam.m_paramNameCode == FxAttributeParam.ParamNameCode.None || fxAttributeParam.m_paramTarget != FxAttributeParam.ParamTarget.MainVfx)
			{
				continue;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (fxAttributeParam.m_paramNameCode == FxAttributeParam.ParamNameCode.LengthInSquares)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				m_totalLengthInSquares = fxAttributeParam.m_paramValue;
			}
			else if (fxAttributeParam.m_paramNameCode == FxAttributeParam.ParamNameCode.WidthInSquares)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				m_totalWidthInSquares = fxAttributeParam.m_paramValue;
			}
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (!(m_fx != null) || !m_fx.activeSelf)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(m_totalTravelDist2D > 0f))
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (m_totalLengthInSquares > 0f && m_totalWidthInSquares > 0f)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						float num = Mathf.Min(1f, VectorUtils.HorizontalPlaneDistInWorld(m_fxSpawnPos, m_fx.transform.position) / m_totalTravelDist2D);
						float value = (1f - num) * m_totalLengthInSquares * (m_totalWidthInSquares / m_totalLengthInSquares);
						Sequence.SetAttribute(m_fx, "widthInSquares", value);
						return;
					}
				}
				return;
			}
		}
	}
}
