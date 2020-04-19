using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class ShaderPropAnimator : MonoBehaviour
	{
		private Renderer \u001D;

		private Material \u000E;

		public AnimationCurve \u0012;

		public float \u0015;

		private void \u0016()
		{
			this.\u001D = base.GetComponent<Renderer>();
			this.\u000E = this.\u001D.material;
		}

		private void \u0013()
		{
			base.StartCoroutine(this.\u0016());
		}

		private IEnumerator \u0016()
		{
			this.\u0015 = UnityEngine.Random.Range(0f, 1f);
			for (;;)
			{
				float value = this.\u0012.Evaluate(this.\u0015);
				this.\u000E.SetFloat(ShaderUtilities.ID_GlowPower, value);
				this.\u0015 += Time.deltaTime * UnityEngine.Random.Range(0.2f, 0.3f);
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
