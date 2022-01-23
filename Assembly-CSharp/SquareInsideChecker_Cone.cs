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
		m_coneStart = startPos;
		m_coneWidthAngle = widthAngle;
		m_coneRadius = radiusInSquares;
		m_coneBackwardsOffset = backwardsOffset;
		m_coneCenterAngle = centerAngle;
		m_caster = caster;
	}

	public void SetLosPosOverride(bool useOverride, Vector3 overridePos, bool useConeLosCheckRules)
	{
		m_useLosPosOverride = useOverride;
		m_losPosOverride = overridePos;
		m_useConeLosRulesForOverride = useConeLosCheckRules;
	}

	public virtual bool IsSquareInside(BoardSquare square, out bool inLos)
	{
		inLos = false;
		Vector3 coneStart = m_coneStart;
		float coneCenterAngle = m_coneCenterAngle;
		float coneWidthAngle = m_coneWidthAngle;
		float coneRadius = m_coneRadius;
		float coneBackwardsOffset = m_coneBackwardsOffset;
		int ignoreLoS;
		if (!m_useLosPosOverride)
		{
			ignoreLoS = (m_penetrateLos ? 1 : 0);
		}
		else
		{
			ignoreLoS = 1;
		}
		bool flag = AreaEffectUtils.IsSquareInConeByActorRadius(square, coneStart, coneCenterAngle, coneWidthAngle, coneRadius, coneBackwardsOffset, (byte)ignoreLoS != 0, m_caster);
		if (flag)
		{
			if (m_penetrateLos)
			{
				inLos = true;
			}
			else
			{
				Vector3 a = VectorUtils.AngleDegreesToVector(m_coneCenterAngle);
				float d = m_coneBackwardsOffset * Board.Get().squareSize;
				Vector3 vector2D = m_coneStart - a * d;
				BoardSquare boardSquare = Board.Get().GetSquareFromVec3(vector2D);
				Vector3 vector = m_coneStart;
				if (m_useLosPosOverride)
				{
					vector = m_losPosOverride;
					boardSquare = Board.Get().GetSquareFromVec3(vector);
				}
				if (boardSquare != null)
				{
					if (m_useConeLosRulesForOverride)
					{
						inLos = AreaEffectUtils.SquareHasLosForCone(vector, boardSquare, square, m_caster);
					}
					else
					{
						inLos = AreaEffectUtils.SquaresHaveLoSForAbilities(boardSquare, square, m_caster);
					}
				}
			}
		}
		return flag;
	}
}
