using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class ShaderPropAnimator : MonoBehaviour
	{
		private Renderer renderer;

		private Material material;

		public AnimationCurve curve;

		public float alpha;

		private void \u0016()
		{
			this.renderer = base.GetComponent<Renderer>();
			this.material = this.renderer.material;
		}

		private void Start()
		{
			base.StartCoroutine(this.Animate());
		}

		private IEnumerator Animate()
		{
			this.alpha = UnityEngine.Random.Range(0f, 1f);
			for (;;)
			{
				float value = this.curve.Evaluate(this.alpha);
				this.material.SetFloat(ShaderUtilities.ID_GlowPower, value);
				this.alpha += Time.deltaTime * UnityEngine.Random.Range(0.2f, 0.3f);
				yield return new WaitForEndOfFrame();
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ShaderPropAnimator.<AnimateProperties>c__Iterator0.MoveNext()).MethodHandle;
				}
			}
			yield break;
		}
	}
}
