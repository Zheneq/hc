using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class SkewTextExample : MonoBehaviour
	{
		private TMP_Text \u001D;

		public AnimationCurve \u000E = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.25f, 2f),
			new Keyframe(0.5f, 0f),
			new Keyframe(0.75f, 2f),
			new Keyframe(1f, 0f)
		});

		public float \u0012 = 1f;

		public float \u0015 = 1f;

		private void \u0016()
		{
			this.\u001D = base.gameObject.GetComponent<TMP_Text>();
		}

		private void \u0013()
		{
			base.StartCoroutine(this.coroutine0016\u0016());
		}

		private AnimationCurve \u0016(AnimationCurve \u001D)
		{
			return new AnimationCurve
			{
				keys = \u001D.keys
			};
		}

		private IEnumerator coroutine0016\u0016()
		{
			this.\u000E.preWrapMode = WrapMode.Once;
			this.\u000E.postWrapMode = WrapMode.Once;
			this.\u001D.havePropertiesChanged = true;
			this.\u0012 *= 10f;
			float u = this.\u0012;
			float u2 = this.\u0015;
			AnimationCurve animationCurve = this.\u0016(this.\u000E);
			for (;;)
			{
				if (!this.\u001D.havePropertiesChanged && u == this.\u0012)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(SkewTextExample.<WarpText>c__Iterator0.MoveNext()).MethodHandle;
					}
					if (animationCurve.keys[1].value == this.\u000E.keys[1].value)
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
						if (u2 == this.\u0015)
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
							yield return null;
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							continue;
						}
					}
				}
				u = this.\u0012;
				animationCurve = this.\u0016(this.\u000E);
				u2 = this.\u0015;
				this.\u001D.ForceMeshUpdate();
				TMP_TextInfo textInfo = this.\u001D.textInfo;
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
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
				}
				else
				{
					float x = this.\u001D.bounds.min.x;
					float x2 = this.\u001D.bounds.max.x;
					for (int i = 0; i < characterCount; i++)
					{
						if (!textInfo.characterInfo[i].isVisible)
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
						}
						else
						{
							int vertexIndex = textInfo.characterInfo[i].vertexIndex;
							int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
							Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
							Vector3 vector = new Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, textInfo.characterInfo[i].baseLine);
							vertices[vertexIndex] += -vector;
							vertices[vertexIndex + 1] += -vector;
							vertices[vertexIndex + 2] += -vector;
							vertices[vertexIndex + 3] += -vector;
							float num = this.\u0015 * 0.01f;
							Vector3 b = new Vector3(num * (textInfo.characterInfo[i].topRight.y - textInfo.characterInfo[i].baseLine), 0f, 0f);
							Vector3 a = new Vector3(num * (textInfo.characterInfo[i].baseLine - textInfo.characterInfo[i].bottomRight.y), 0f, 0f);
							vertices[vertexIndex] += -a;
							vertices[vertexIndex + 1] += b;
							vertices[vertexIndex + 2] += b;
							vertices[vertexIndex + 3] += -a;
							float num2 = (vector.x - x) / (x2 - x);
							float num3 = num2 + 0.0001f;
							float y = this.\u000E.Evaluate(num2) * this.\u0012;
							float y2 = this.\u000E.Evaluate(num3) * this.\u0012;
							Vector3 lhs = new Vector3(1f, 0f, 0f);
							Vector3 rhs = new Vector3(num3 * (x2 - x) + x, y2) - new Vector3(vector.x, y);
							float num4 = Mathf.Acos(Vector3.Dot(lhs, rhs.normalized)) * 57.29578f;
							float num5;
							if (Vector3.Cross(lhs, rhs).z > 0f)
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
								num5 = num4;
							}
							else
							{
								num5 = 360f - num4;
							}
							float z = num5;
							Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, y, 0f), Quaternion.Euler(0f, 0f, z), Vector3.one);
							vertices[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex]);
							vertices[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 1]);
							vertices[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 2]);
							vertices[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 3]);
							vertices[vertexIndex] += vector;
							vertices[vertexIndex + 1] += vector;
							vertices[vertexIndex + 2] += vector;
							vertices[vertexIndex + 3] += vector;
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
					this.\u001D.UpdateVertexData();
					yield return null;
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			yield break;
		}
	}
}
