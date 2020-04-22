using UnityEngine;

public interface IUIScene
{
	Transform[] GetSceneContainers();

	SceneType GetSceneType();

	void SetVisible(bool visible, SceneVisibilityParameters parameters);

	void NotifyGameStateChange(SceneStateParameters newState);

	bool DoesHandleParameter(SceneStateParameters newState);

	void HandleNewSceneStateParameter(SceneStateParameters parameters);
}
