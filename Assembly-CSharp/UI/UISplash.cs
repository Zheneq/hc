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
		m_updateCount++;
		if (m_updateCount != 5)
		{
			return;
		}
		while (true)
		{
			m_loadLevelOperation = SceneManager.LoadSceneAsync(m_sceneToLoad);
			return;
		}
	}
}
