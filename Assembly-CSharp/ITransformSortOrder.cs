using UnityEngine;

public interface ITransformSortOrder
{
	int GetTransformPriority();

	Transform GetTransform();
}
