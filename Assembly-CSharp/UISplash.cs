using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISplash : MonoBehaviour
{
	public string m_sceneToLoad;

	private int m_updateCount;

	private AsyncOperation m_loadLevelOperation;

	private void Start()
	{
		Application.backgroundLoadingPriority = ThreadPriority.Low;
	}

	private void Update()
	{
		this.m_updateCount++;
		if (this.m_updateCount == 5)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISplash.Update()).MethodHandle;
			}
			this.m_loadLevelOperation = SceneManager.LoadSceneAsync(this.m_sceneToLoad);
		}
	}
}
