using System;
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
		if (eventType == GameEventManager.EventType.GameCameraCreated)
		{
			StylizedFog stylizedFog = Camera.main.gameObject.GetComponent<StylizedFog>();
			if (!stylizedFog)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(StylizedFogOverride.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
				}
				stylizedFog = Camera.main.gameObject.AddComponent<StylizedFog>();
			}
			stylizedFog.enabled = true;
			stylizedFog.fogMode = this.m_stylizedFog.fogMode;
			stylizedFog.blend = this.m_stylizedFog.blend;
			stylizedFog.gradientSource = this.m_stylizedFog.gradientSource;
			stylizedFog.rampGradient = this.m_stylizedFog.rampGradient;
			stylizedFog.rampBlendGradient = this.m_stylizedFog.rampBlendGradient;
			stylizedFog.rampTexture = this.m_stylizedFog.rampTexture;
			stylizedFog.rampBlendTexture = this.m_stylizedFog.rampBlendTexture;
			stylizedFog.noiseTexture = this.m_stylizedFog.noiseTexture;
			stylizedFog.noiseSpeed = this.m_stylizedFog.noiseSpeed;
			stylizedFog.noiseTiling = this.m_stylizedFog.noiseTiling;
		}
	}
}
