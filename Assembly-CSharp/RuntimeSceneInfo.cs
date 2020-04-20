using System;
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
		if (this.RuntimeStaticSceneContainer != null)
		{
			UIManager.SetGameObjectActive(this.RuntimeStaticSceneContainer, visible, null);
		}
		if (this.RuntimeSemiStaticSceneContainer != null)
		{
			UIManager.SetGameObjectActive(this.RuntimeSemiStaticSceneContainer, visible, null);
		}
		if (this.RuntimeCameraMovementSceneContainer != null)
		{
			UIManager.SetGameObjectActive(this.RuntimeCameraMovementSceneContainer, visible, null);
		}
		if (this.RuntimePerFrameSceneContainer != null)
		{
			UIManager.SetGameObjectActive(this.RuntimePerFrameSceneContainer, visible, null);
		}
	}
}
