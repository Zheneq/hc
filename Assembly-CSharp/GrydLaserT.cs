﻿using System;
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
	public int m_damageAmount = 0x1E;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Laser T";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_Cross(this, this.GetMinForwardLength(), this.GetMaxForwardLength(), this.GetBranchLength(), this.GetLaserWidth(), false, this.m_maxTargets, this.m_lockToCardinalDirections, this.m_discreteStepsForRange, false, false, this.m_branchLengthDecreaseOverDistance);
	}

	public float GetMinForwardLength()
	{
		return this.m_minForwardLength;
	}

	public float GetMaxForwardLength()
	{
		return this.m_maxForwardLength;
	}

	public float GetLaserWidth()
	{
		return this.m_laserWidth;
	}

	public float GetBranchLength()
	{
		return this.m_branchLength;
	}

	public int GetDamageAmount()
	{
		return this.m_damageAmount;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetDamageAmount());
		return result;
	}

	private Vector3 GetClampedTargeterRange(AbilityTarget currentTarget, Vector3 startPos, Vector3 aimDir, ref float distInWorld, ref float branchLengthInWorld)
	{
		return GrydLaserT.GetClampedTargeterRangeStatic(currentTarget, startPos, aimDir, this.GetMinForwardLength(), this.GetMaxForwardLength(), this.m_discreteStepsForRange, this.m_branchLengthDecreaseOverDistance, ref distInWorld, ref branchLengthInWorld);
	}

	public unsafe static Vector3 GetClampedTargeterRangeStatic(AbilityTarget currentTarget, Vector3 startPos, Vector3 aimDir, float minForwardLenInSquares, float maxForwardLenInSquares, bool discreteStepsForRange, float branchLenDecreaseOverDist, ref float dist, ref float branchLengthInWorld)
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydLaserT.GetClampedTargeterRangeStatic(AbilityTarget, Vector3, Vector3, float, float, bool, float, float*, float*)).MethodHandle;
			}
			vector = startPos + aimDir * num;
		}
		else if (dist > num2)
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
			vector = startPos + aimDir * num2;
			int num3 = Mathf.RoundToInt(maxForwardLenInSquares - minForwardLenInSquares);
			float num4 = Mathf.Floor(branchLenDecreaseOverDist * (float)num3) * squareSize;
			branchLengthInWorld = Mathf.Max(0f, branchLengthInWorld - num4);
		}
		else if (discreteStepsForRange)
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
