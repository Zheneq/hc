using System;
using UnityEngine;

public class SquareInsideChecker_Cone : ISquareInsideChecker
{
	protected Vector3 m_coneStart;

	protected float m_coneWidthAngle;

	protected float m_coneRadius;

	protected float m_coneBackwardsOffset;

	protected float m_coneCenterAngle;

	protected bool m_penetrateLos;

	protected ActorData m_caster;

	protected bool m_useLosPosOverride;

	protected bool m_useConeLosRulesForOverride = true;

	protected Vector3 m_losPosOverride = Vector3.zero;

	public virtual void UpdateConeProperties(Vector3 startPos, float widthAngle, float radiusInSquares, float backwardsOffset, float centerAngle, ActorData caster)
	{
		this.m_coneStart = startPos;
		this.m_coneWidthAngle = widthAngle;
		this.m_coneRadius = radiusInSquares;
		this.m_coneBackwardsOffset = backwardsOffset;
		this.m_coneCenterAngle = centerAngle;
		this.m_caster = caster;
	}

	public void SetLosPosOverride(bool useOverride, Vector3 overridePos, bool useConeLosCheckRules)
	{
		this.m_useLosPosOverride = useOverride;
		this.m_losPosOverride = overridePos;
		this.m_useConeLosRulesForOverride = useConeLosCheckRules;
	}

	public unsafe virtual bool IsSquareInside(BoardSquare square, out bool inLos)
	{
		inLos = false;
		Vector3 coneStart = this.m_coneStart;
		float coneCenterAngle = this.m_coneCenterAngle;
		float coneWidthAngle = this.m_coneWidthAngle;
		float coneRadius = this.m_coneRadius;
		float coneBackwardsOffset = this.m_coneBackwardsOffset;
		bool ignoreLoS;
		if (!this.m_useLosPosOverride)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SquareInsideChecker_Cone.IsSquareInside(BoardSquare, bool*)).MethodHandle;
			}
			ignoreLoS = this.m_penetrateLos;
		}
		else
		{
			ignoreLoS = true;
		}
		bool flag = AreaEffectUtils.IsSquareInConeByActorRadius(square, coneStart, coneCenterAngle, coneWidthAngle, coneRadius, coneBackwardsOffset, ignoreLoS, this.m_caster, false, default(Vector3));
		if (flag)
		{
			if (this.m_penetrateLos)
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
				inLos = true;
			}
			else
			{
				Vector3 a = VectorUtils.AngleDegreesToVector(this.m_coneCenterAngle);
				float d = this.m_coneBackwardsOffset * Board.\u000E().squareSize;
				Vector3 u001D = this.m_coneStart - a * d;
				BoardSquare boardSquare = Board.\u000E().\u000E(u001D);
				Vector3 vector = this.m_coneStart;
				if (this.m_useLosPosOverride)
				{
					vector = this.m_losPosOverride;
					boardSquare = Board.\u000E().\u000E(vector);
				}
				if (boardSquare != null)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_useConeLosRulesForOverride)
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
						inLos = AreaEffectUtils.SquareHasLosForCone(vector, boardSquare, square, this.m_caster);
					}
					else
					{
						inLos = AreaEffectUtils.SquaresHaveLoSForAbilities(boardSquare, square, this.m_caster, true, null);
					}
				}
			}
		}
		return flag;
	}
}
