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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieNarrowingProjectileSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
				}
				Sequence.FxAttributeParam fxAttributeParam = extraSequenceParams as Sequence.FxAttributeParam;
				if (fxAttributeParam != null)
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
					if (fxAttributeParam.m_paramNameCode != Sequence.FxAttributeParam.ParamNameCode.None && fxAttributeParam.m_paramTarget == Sequence.FxAttributeParam.ParamTarget.MainVfx)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (fxAttributeParam.m_paramNameCode == Sequence.FxAttributeParam.ParamNameCode.LengthInSquares)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_totalLengthInSquares = fxAttributeParam.m_paramValue;
						}
						else if (fxAttributeParam.m_paramNameCode == Sequence.FxAttributeParam.ParamNameCode.WidthInSquares)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_totalWidthInSquares = fxAttributeParam.m_paramValue;
						}
					}
				}
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.m_fx != null && this.m_fx.activeSelf)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieNarrowingProjectileSequence.OnUpdate()).MethodHandle;
			}
			if (this.m_totalTravelDist2D > 0f)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_totalLengthInSquares > 0f && this.m_totalWidthInSquares > 0f)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					float num = Mathf.Min(1f, VectorUtils.HorizontalPlaneDistInWorld(this.m_fxSpawnPos, this.m_fx.transform.position) / this.m_totalTravelDist2D);
					float value = (1f - num) * this.m_totalLengthInSquares * (this.m_totalWidthInSquares / this.m_totalLengthInSquares);
					Sequence.SetAttribute(this.m_fx, "widthInSquares", value);
				}
			}
		}
	}
}
