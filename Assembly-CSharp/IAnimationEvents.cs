using System;
using UnityEngine;

public interface IAnimationEvents
{
	[AnimationEvent(G = 1f, R = 1f, B = 1f)]
	void NewEvent(UnityEngine.Object eventObject);

	[AnimationEvent(G = 0f, R = 1f, B = 0f)]
	void VFXEvent(UnityEngine.Object eventObject);

	[AnimationEvent(G = 1f, R = 0f, B = 0f)]
	void GameplayEvent(UnityEngine.Object eventObject);

	[AnimationEvent(G = 0f, R = 0f, B = 1f, IsAudio = true)]
	void AudioEvent(string eventName);
}
