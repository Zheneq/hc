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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIAnimationEventManager.Update()).MethodHandle;
				}
				if (animationEventTracker.callbackWithGameObjectParam == null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!animationEventTracker.AnimatorWasInactive)
					{
						goto IL_13E;
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			if (animationController.gameObject.activeInHierarchy)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (animationEventTracker.AnimationCheckTime <= Time.time)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (animationEventTracker.AnimatorWasInactive)
					{
						animationController.Play(animationEventTracker.AnimationToPlay, animationEventTracker.AnimationLayerToPlayOn, animationEventTracker.NormalizedTimeToStartPlay);
						animationEventTracker.AnimatorWasInactive = false;
					}
					if (UIAnimationEventManager.IsAnimationDone(animationController, animationEventTracker.AnimNameToTrackForDoneCallback, animationEventTracker.AnimationLayerToPlayOn))
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (animationEventTracker.callback != null)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							animationEventTracker.callback();
						}
						if (animationEventTracker.callbackWithGameObjectParam != null)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
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
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	public void PlayAnimation(Animator animator, string AnimToPlay, UIAnimationEventManager.AnimationDoneCallback callbackOnDone, string AnimNameForDoneCallback = "", int Layer = 0, float NormalizedTime = 0f, bool setAnimatorGameObjectActive = true, bool checkCurrentState = false, UIAnimationEventManager.AnimationDoneCallbackWithGameObjectParam callbackOnDoneWithGameObjectParam = null, GameObject gameObjectParamForCallback = null)
	{
		if (animator == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAnimationEventManager.PlayAnimation(Animator, string, UIAnimationEventManager.AnimationDoneCallback, string, int, float, bool, bool, UIAnimationEventManager.AnimationDoneCallbackWithGameObjectParam, GameObject)).MethodHandle;
			}
			Log.Error("Animator is null", new object[0]);
			return;
		}
		if (AnimToPlay.IsNullOrEmpty())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			Log.Error("Attempted to play null animation", new object[0]);
			return;
		}
		bool flag = false;
		for (int i = this.ActiveAnimations.Count - 1; i >= 0; i--)
		{
			if (this.ActiveAnimations[i].AnimationController == animator)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (Layer == this.ActiveAnimations[i].AnimationLayerToPlayOn)
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
					this.ActiveAnimations.RemoveAt(i);
					flag = true;
				}
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		if (checkCurrentState)
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
			if (!flag)
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
				if (animator.isActiveAndEnabled)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (animator.isInitialized)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (animator.GetCurrentAnimatorStateInfo(Layer).IsName(AnimToPlay))
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							return;
						}
					}
				}
			}
		}
		if (setAnimatorGameObjectActive)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(animator, true, null);
		}
		bool flag2 = true;
		if (animator.gameObject.activeInHierarchy)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (animator.isInitialized)
			{
				goto IL_178;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		flag2 = false;
		IL_178:
		if (AnimNameForDoneCallback.IsNullOrEmpty())
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			AnimNameForDoneCallback = AnimToPlay;
		}
		float num = 0f;
		for (int j = 0; j < animator.runtimeAnimatorController.animationClips.Length; j++)
		{
			if (animator.runtimeAnimatorController.animationClips[j].name == AnimToPlay)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			animator.Play(AnimToPlay, Layer, NormalizedTime);
		}
		this.ActiveAnimations.Add(item);
	}

	public static bool IsAnimationDone(Animator animator, string animName, int layer)
	{
		if (!animator.isInitialized)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAnimationEventManager.IsAnimationDone(Animator, string, int)).MethodHandle;
			}
			return false;
		}
		AnimatorClipInfo[] currentAnimatorClipInfo = animator.GetCurrentAnimatorClipInfo(layer);
		if (currentAnimatorClipInfo.Length == 0)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			return false;
		}
		AnimatorClipInfo animatorClipInfo = currentAnimatorClipInfo[0];
		AnimationClip clip = animatorClipInfo.clip;
		if (clip == null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			return false;
		}
		if (animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1f)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			return false;
		}
		if (clip.name != animName)
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
