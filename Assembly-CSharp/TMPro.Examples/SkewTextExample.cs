using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class SkewTextExample : MonoBehaviour
	{
		private TMP_Text _001D;

		public AnimationCurve _000E = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.25f, 2f),
			new Keyframe(0.5f, 0f),
			new Keyframe(0.75f, 2f),
			new Keyframe(1f, 0f)
		});

		public float _0012 = 1f;

		public float _0015 = 1f;

		private void _0016()
		{
			this._001D = base.gameObject.GetComponent<TMP_Text>();
		}

		private void _0013()
		{
			base.StartCoroutine(this.coroutine0016_0016());
		}

		private AnimationCurve _0016(AnimationCurve _001D)
		{
			return new AnimationCurve
			{
				keys = _001D.keys
			};
		}

		private IEnumerator coroutine0016_0016()
		{
			this._000E.preWrapMode = WrapMode.Once;
			this._000E.postWrapMode = WrapMode.Once;
			this._001D.havePropertiesChanged = true;
			this._0012 *= 10f;
			float u = this._0012;
			float u2 = this._0015;
			AnimationCurve animationCurve = this._0016(this._000E);
			for (;;)
			{
				if (!this._001D.havePropertiesChanged && u == this._0012)
				{
					if (animationCurve.keys[1].value == this._000E.keys[1].value)
					{
						if (u2 == this._0015)
						{
							yield return null;
							continue;
						}
					}
				}
				u = this._0012;
				animationCurve = this._0016(this._000E);
				u2 = this._0015;
				this._001D.ForceMeshUpdate();
				TMP_TextInfo textInfo = this._001D.textInfo;
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
				}
				else
				{
					float x = this._001D.bounds.min.x;
					float x2 = this._001D.bounds.max.x;
					for (int i = 0; i < characterCount; i++)
					{
						if (!textInfo.characterInfo[i].isVisible)
						{
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
							float num = this._0015 * 0.01f;
							Vector3 b = new Vector3(num * (textInfo.characterInfo[i].topRight.y - textInfo.characterInfo[i].baseLine), 0f, 0f);
							Vector3 a = new Vector3(num * (textInfo.characterInfo[i].baseLine - textInfo.characterInfo[i].bottomRight.y), 0f, 0f);
							vertices[vertexIndex] += -a;
							vertices[vertexIndex + 1] += b;
							vertices[vertexIndex + 2] += b;
							vertices[vertexIndex + 3] += -a;
							float num2 = (vector.x - x) / (x2 - x);
							float num3 = num2 + 0.0001f;
							float y = this._000E.Evaluate(num2) * this._0012;
							float y2 = this._000E.Evaluate(num3) * this._0012;
							Vector3 lhs = new Vector3(1f, 0f, 0f);
							Vector3 rhs = new Vector3(num3 * (x2 - x) + x, y2) - new Vector3(vector.x, y);
							float num4 = Mathf.Acos(Vector3.Dot(lhs, rhs.normalized)) * 57.29578f;
							float num5;
							if (Vector3.Cross(lhs, rhs).z > 0f)
							{
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
					this._001D.UpdateVertexData();
					yield return null;
				}
			}
			yield break;
		}
	}
}
