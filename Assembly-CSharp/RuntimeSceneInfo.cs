using UnityEngine;

public class RuntimeSceneInfo
{
	public UISceneDisplayInfo DisplayInfo;

	public IUIScene RuntimeScene;

	public GameObject RuntimeSceneContainer;

	public GameObject RuntimeStaticSceneContainer;

	public GameObject RuntimeSemiStaticSceneContainer;

	public GameObject RuntimeCameraMovementSceneContainer;

	public GameObject RuntimePerFrameSceneContainer;

	public void SetBatchScenesVisible(bool visible)
	{
		if (RuntimeStaticSceneContainer != null)
		{
			UIManager.SetGameObjectActive(RuntimeStaticSceneContainer, visible);
		}
		if (RuntimeSemiStaticSceneContainer != null)
		{
			UIManager.SetGameObjectActive(RuntimeSemiStaticSceneContainer, visible);
		}
		if (RuntimeCameraMovementSceneContainer != null)
		{
			UIManager.SetGameObjectActive(RuntimeCameraMovementSceneContainer, visible);
		}
		if (!(RuntimePerFrameSceneContainer != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(RuntimePerFrameSceneContainer, visible);
			return;
		}
	}
}
