using UnityEngine;

public interface IAnimationEvents
{
	[AnimationEvent(G = 1f, R = 1f, B = 1f)]
	void NewEvent(Object eventObject);

	[AnimationEvent(G = 0f, R = 1f, B = 0f)]
	void VFXEvent(Object eventObject);

	[AnimationEvent(G = 1f, R = 0f, B = 0f)]
	void GameplayEvent(Object eventObject);

	[AnimationEvent(G = 0f, R = 0f, B = 1f, IsAudio = true)]
	void AudioEvent(string eventName);
}
