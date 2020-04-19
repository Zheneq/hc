using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	[DisallowMultipleComponent]
	public class TMP_SpriteAnimator : MonoBehaviour
	{
		private Dictionary<int, bool> m_animations = new Dictionary<int, bool>(0x10);

		private TMP_Text m_TextComponent;

		private void Awake()
		{
			this.m_TextComponent = base.GetComponent<TMP_Text>();
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		public void StopAllAnimations()
		{
			base.StopAllCoroutines();
			this.m_animations.Clear();
		}

		public void DoSpriteAnimation(int currentCharacter, TMP_SpriteAsset spriteAsset, int start, int end, int framerate)
		{
			bool flag = false;
			if (!this.m_animations.TryGetValue(currentCharacter, out flag))
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SpriteAnimator.DoSpriteAnimation(int, TMP_SpriteAsset, int, int, int)).MethodHandle;
				}
				base.StartCoroutine(this.DoSpriteAnimationInternal(currentCharacter, spriteAsset, start, end, framerate));
				this.m_animations.Add(currentCharacter, true);
			}
		}

		private IEnumerator DoSpriteAnimationInternal(int currentCharacter, TMP_SpriteAsset spriteAsset, int start, int end, int framerate)
		{
			if (this.m_TextComponent == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SpriteAnimator.<DoSpriteAnimationInternal>c__Iterator0.MoveNext()).MethodHandle;
				}
				yield break;
			}
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
			int currentFrame = start;
			if (end > spriteAsset.spriteInfoList.Count)
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
				end = spriteAsset.spriteInfoList.Count - 1;
			}
			TMP_CharacterInfo charInfo = this.m_TextComponent.textInfo.characterInfo[currentCharacter];
			int materialIndex = charInfo.materialReferenceIndex;
			int vertexIndex = charInfo.vertexIndex;
			TMP_MeshInfo meshInfo = this.m_TextComponent.textInfo.meshInfo[materialIndex];
			float elapsedTime = 0f;
			float targetTime = 1f / (float)Mathf.Abs(framerate);
			for (;;)
			{
				if (elapsedTime > targetTime)
				{
					elapsedTime = 0f;
					TMP_Sprite tmp_Sprite = spriteAsset.spriteInfoList[currentFrame];
					Vector3[] vertices = meshInfo.vertices;
					Vector2 vector = new Vector2(charInfo.origin, charInfo.baseLine);
					float num = charInfo.fontAsset.fontInfo.Ascender / tmp_Sprite.height * tmp_Sprite.scale * charInfo.scale;
					Vector3 vector2 = new Vector3(vector.x + tmp_Sprite.xOffset * num, vector.y + (tmp_Sprite.yOffset - tmp_Sprite.height) * num);
					Vector3 vector3 = new Vector3(vector2.x, vector.y + tmp_Sprite.yOffset * num);
					Vector3 vector4 = new Vector3(vector.x + (tmp_Sprite.xOffset + tmp_Sprite.width) * num, vector3.y);
					Vector3 vector5 = new Vector3(vector4.x, vector2.y);
					vertices[vertexIndex] = vector2;
					vertices[vertexIndex + 1] = vector3;
					vertices[vertexIndex + 2] = vector4;
					vertices[vertexIndex + 3] = vector5;
					Vector2[] uvs = meshInfo.uvs0;
					Vector2 vector6 = new Vector2(tmp_Sprite.x / (float)spriteAsset.spriteSheet.width, tmp_Sprite.y / (float)spriteAsset.spriteSheet.height);
					Vector2 vector7 = new Vector2(vector6.x, (tmp_Sprite.y + tmp_Sprite.height) / (float)spriteAsset.spriteSheet.height);
					Vector2 vector8 = new Vector2((tmp_Sprite.x + tmp_Sprite.width) / (float)spriteAsset.spriteSheet.width, vector7.y);
					Vector2 vector9 = new Vector2(vector8.x, vector6.y);
					uvs[vertexIndex] = vector6;
					uvs[vertexIndex + 1] = vector7;
					uvs[vertexIndex + 2] = vector8;
					uvs[vertexIndex + 3] = vector9;
					meshInfo.mesh.vertices = vertices;
					meshInfo.mesh.uv = uvs;
					this.m_TextComponent.UpdateGeometry(meshInfo.mesh, materialIndex);
					if (framerate > 0)
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
						if (currentFrame < end)
						{
							currentFrame++;
						}
						else
						{
							currentFrame = start;
						}
					}
					else if (currentFrame > start)
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
						currentFrame--;
					}
					else
					{
						currentFrame = end;
					}
				}
				elapsedTime += Time.deltaTime;
				yield return null;
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
			yield break;
		}
	}
}
