using System;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationEventManager : MonoBehaviour
{
	private List<UIAnimationEventManager.AnimationEventTracker> ActiveAnimations = new List<UIAnimationEventManager.AnimationEventTracker>();

	private static UIAnimationEventManager s_instance;

	public static UIAnimationEventManager Get()
	{
		return UIAnimationEventManager.s_instance;
	}

	private void Awake()
	{
		UIAnimationEventManager.s_instance = this;
	}

	private void Update()
	{
		int i = 0;
		while (i < this.ActiveAnimations.Count)
		{
			UIAnimationEventManager.AnimationEventTracker animationEventTracker = this.ActiveAnimations[i];
			Animator animationController = animationEventTracker.AnimationController;
			if (!(animationController != null))
			{
				goto IL_13E;
			}
			if (animationEventTracker.callback == null)
			{
				if (animationEventTracker.callbackWithGameObjectParam == null)
				{
					if (!animationEventTracker.AnimatorWasInactive)
					{
						goto IL_13E;
					}
				}
			}
			if (animationController.gameObject.activeInHierarchy)
			{
				if (animationEventTracker.AnimationCheckTime <= Time.time)
				{
					if (animationEventTracker.AnimatorWasInactive)
					{
						animationController.Play(animationEventTracker.AnimationToPlay, animationEventTracker.AnimationLayerToPlayOn, animationEventTracker.NormalizedTimeToStartPlay);
						animationEventTracker.AnimatorWasInactive = false;
					}
					if (UIAnimationEventManager.IsAnimationDone(animationController, animationEventTracker.AnimNameToTrackForDoneCallback, animationEventTracker.AnimationLayerToPlayOn))
					{
						if (animationEventTracker.callback != null)
						{
							animationEventTracker.callback();
						}
						if (animationEventTracker.callbackWithGameObjectParam != null)
						{
							animationEventTracker.callbackWithGameObjectParam(animationEventTracker.gameObjectParam);
						}
						this.ActiveAnimations.RemoveAt(i);
						i--;
					}
				}
			}
			IL_14E:
			i++;
			continue;
			IL_13E:
			this.ActiveAnimations.RemoveAt(i);
			i--;
			goto IL_14E;
		}
	}

	public void PlayAnimation(Animator animator, string AnimToPlay, UIAnimationEventManager.AnimationDoneCallback callbackOnDone, string AnimNameForDoneCallback = "", int Layer = 0, float NormalizedTime = 0f, bool setAnimatorGameObjectActive = true, bool checkCurrentState = false, UIAnimationEventManager.AnimationDoneCallbackWithGameObjectParam callbackOnDoneWithGameObjectParam = null, GameObject gameObjectParamForCallback = null)
	{
		if (animator == null)
		{
			Log.Error("Animator is null", new object[0]);
			return;
		}
		if (AnimToPlay.IsNullOrEmpty())
		{
			Log.Error("Attempted to play null animation", new object[0]);
			return;
		}
		bool flag = false;
		for (int i = this.ActiveAnimations.Count - 1; i >= 0; i--)
		{
			if (this.ActiveAnimations[i].AnimationController == animator)
			{
				if (Layer == this.ActiveAnimations[i].AnimationLayerToPlayOn)
				{
					this.ActiveAnimations.RemoveAt(i);
					flag = true;
				}
			}
		}
		if (checkCurrentState)
		{
			if (!flag)
			{
				if (animator.isActiveAndEnabled)
				{
					if (animator.isInitialized)
					{
						if (animator.GetCurrentAnimatorStateInfo(Layer).IsName(AnimToPlay))
						{
							return;
						}
					}
				}
			}
		}
		if (setAnimatorGameObjectActive)
		{
			UIManager.SetGameObjectActive(animator, true, null);
		}
		bool flag2 = true;
		if (animator.gameObject.activeInHierarchy)
		{
			if (animator.isInitialized)
			{
				goto IL_178;
			}
		}
		flag2 = false;
		IL_178:
		if (AnimNameForDoneCallback.IsNullOrEmpty())
		{
			AnimNameForDoneCallback = AnimToPlay;
		}
		float num = 0f;
		for (int j = 0; j < animator.runtimeAnimatorController.animationClips.Length; j++)
		{
			if (animator.runtimeAnimatorController.animationClips[j].name == AnimToPlay)
			{
				num = animator.runtimeAnimatorController.animationClips[j].length;
			}
		}
		UIAnimationEventManager.AnimationEventTracker item = new UIAnimationEventManager.AnimationEventTracker
		{
			AnimationController = animator,
			AnimationToPlay = AnimToPlay,
			AnimNameToTrackForDoneCallback = AnimNameForDoneCallback,
			callback = callbackOnDone,
			AnimatorWasInactive = !flag2,
			AnimationLayerToPlayOn = Layer,
			NormalizedTimeToStartPlay = NormalizedTime,
			AnimationCheckTime = Time.time + num,
			callbackWithGameObjectParam = callbackOnDoneWithGameObjectParam,
			gameObjectParam = gameObjectParamForCallback
		};
		if (flag2)
		{
			animator.Play(AnimToPlay, Layer, NormalizedTime);
		}
		this.ActiveAnimations.Add(item);
	}

	public static bool IsAnimationDone(Animator animator, string animName, int layer)
	{
		if (!animator.isInitialized)
		{
			return false;
		}
		AnimatorClipInfo[] currentAnimatorClipInfo = animator.GetCurrentAnimatorClipInfo(layer);
		if (currentAnimatorClipInfo.Length == 0)
		{
			return false;
		}
		AnimatorClipInfo animatorClipInfo = currentAnimatorClipInfo[0];
		AnimationClip clip = animatorClipInfo.clip;
		if (clip == null)
		{
			return false;
		}
		if (animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1f)
		{
			return false;
		}
		if (clip.name != animName)
		{
			return false;
		}
		return true;
	}

	public delegate void AnimationDoneCallback();

	public delegate void AnimationDoneCallbackWithGameObjectParam(GameObject gameObject);

	public class AnimationEventTracker
	{
		public Animator AnimationController;

		public string AnimationToPlay;

		public int AnimationLayerToPlayOn;

		public float NormalizedTimeToStartPlay;

		public string AnimNameToTrackForDoneCallback;

		public UIAnimationEventManager.AnimationDoneCallback callback;

		public UIAnimationEventManager.AnimationDoneCallbackWithGameObjectParam callbackWithGameObjectParam;

		public GameObject gameObjectParam;

		public bool AnimatorWasInactive;

		public float AnimationCheckTime;
	}
}
