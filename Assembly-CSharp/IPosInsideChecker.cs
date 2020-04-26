using System.Collections.Generic;
using UnityEngine;

public interface IPosInsideChecker
{
	bool IsPositionInside(Vector3 testPos);

	bool AddTestPosForBarrier(List<Vector3> existingCheckPoints, Barrier barrier);
}
