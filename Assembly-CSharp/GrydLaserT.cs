using System.Collections.Generic;
using UnityEngine;

public class GrydLaserT : Ability
{
	[Header("-- Targeting")]
	public bool m_lockToCardinalDirections = true;

	public bool m_discreteStepsForRange = true;

	public float m_minForwardLength = 2.5f;

	public float m_maxForwardLength = 4.5f;

	public float m_laserWidth = 1f;

	public float m_branchLength = 2f;

	[Tooltip("Calculated multiplying against current distance, but only subtracted in whole integer increments")]
	public float m_branchLengthDecreaseOverDistance;

	public int m_maxTargets = 1;

	[Header("-- Damage")]
	public int m_damageAmount = 30;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Laser T";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_Cross(this, GetMinForwardLength(), GetMaxForwardLength(), GetBranchLength(), GetLaserWidth(), false, m_maxTargets, m_lockToCardinalDirections, m_discreteStepsForRange, false, false, m_branchLengthDecreaseOverDistance);
	}

	public float GetMinForwardLength()
	{
		return m_minForwardLength;
	}

	public float GetMaxForwardLength()
	{
		return m_maxForwardLength;
	}

	public float GetLaserWidth()
	{
		return m_laserWidth;
	}

	public float GetBranchLength()
	{
		return m_branchLength;
	}

	public int GetDamageAmount()
	{
		return m_damageAmount;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamageAmount());
		return numbers;
	}

	private Vector3 GetClampedTargeterRange(AbilityTarget currentTarget, Vector3 startPos, Vector3 aimDir, ref float distInWorld, ref float branchLengthInWorld)
	{
		return GetClampedTargeterRangeStatic(currentTarget, startPos, aimDir, GetMinForwardLength(), GetMaxForwardLength(), m_discreteStepsForRange, m_branchLengthDecreaseOverDistance, ref distInWorld, ref branchLengthInWorld);
	}

	public static Vector3 GetClampedTargeterRangeStatic(AbilityTarget currentTarget, Vector3 startPos, Vector3 aimDir, float minForwardLenInSquares, float maxForwardLenInSquares, bool discreteStepsForRange, float branchLenDecreaseOverDist, ref float dist, ref float branchLengthInWorld)
	{
		Vector3 vector = currentTarget.FreePos;
		float squareSize = Board.Get().squareSize;
		float num = minForwardLenInSquares * squareSize;
		float num2 = maxForwardLenInSquares * squareSize;
		Vector3 vector2 = vector - startPos;
		vector2.y = 0f;
		dist = vector2.magnitude;
		if (dist < num)
		{
			vector = startPos + aimDir * num;
		}
		else if (dist > num2)
		{
			vector = startPos + aimDir * num2;
			int num3 = Mathf.RoundToInt(maxForwardLenInSquares - minForwardLenInSquares);
			float num4 = Mathf.Floor(branchLenDecreaseOverDist * (float)num3) * squareSize;
			branchLengthInWorld = Mathf.Max(0f, branchLengthInWorld - num4);
		}
		else if (discreteStepsForRange)
		{
			float num5 = Mathf.Max(0f, dist - num);
			int num6 = Mathf.RoundToInt(num5 / squareSize);
			vector = startPos + aimDir * (num + (float)num6 * squareSize);
			num6 -= num6 % 2;
			float num7 = Mathf.Floor(branchLenDecreaseOverDist * (float)num6) * squareSize;
			branchLengthInWorld = Mathf.Max(0f, branchLengthInWorld - num7);
		}
		vector2 = vector - startPos;
		vector2.y = 0f;
		dist = vector2.magnitude;
		return vector;
	}
}
