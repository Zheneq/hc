using UnityEngine;

public class StylizedFogOverride : MonoBehaviour, IGameEventListener
{
	public StylizedFog m_stylizedFog;

	private void OnEnable()
	{
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameCameraCreated);
	}

	private void OnDisable()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameCameraCreated);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.GameCameraCreated)
		{
			return;
		}
		StylizedFog stylizedFog = Camera.main.gameObject.GetComponent<StylizedFog>();
		if (!stylizedFog)
		{
			stylizedFog = Camera.main.gameObject.AddComponent<StylizedFog>();
		}
		stylizedFog.enabled = true;
		stylizedFog.fogMode = m_stylizedFog.fogMode;
		stylizedFog.blend = m_stylizedFog.blend;
		stylizedFog.gradientSource = m_stylizedFog.gradientSource;
		stylizedFog.rampGradient = m_stylizedFog.rampGradient;
		stylizedFog.rampBlendGradient = m_stylizedFog.rampBlendGradient;
		stylizedFog.rampTexture = m_stylizedFog.rampTexture;
		stylizedFog.rampBlendTexture = m_stylizedFog.rampBlendTexture;
		stylizedFog.noiseTexture = m_stylizedFog.noiseTexture;
		stylizedFog.noiseSpeed = m_stylizedFog.noiseSpeed;
		stylizedFog.noiseTiling = m_stylizedFog.noiseTiling;
	}
}
