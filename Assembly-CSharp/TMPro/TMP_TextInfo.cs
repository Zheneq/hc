using System;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public class TMP_TextInfo
	{
		private static Vector2 k_InfinityVectorPositive = new Vector2(32767f, 32767f);

		private static Vector2 k_InfinityVectorNegative = new Vector2(-32767f, -32767f);

		public TMP_Text textComponent;

		public int characterCount;

		public int spriteCount;

		public int spaceCount;

		public int wordCount;

		public int linkCount;

		public int lineCount;

		public int pageCount;

		public int materialCount;

		public TMP_CharacterInfo[] characterInfo;

		public TMP_WordInfo[] wordInfo;

		public TMP_LinkInfo[] linkInfo;

		public TMP_LineInfo[] lineInfo;

		public TMP_PageInfo[] pageInfo;

		public TMP_MeshInfo[] meshInfo;

		private TMP_MeshInfo[] m_CachedMeshInfo;

		public TMP_TextInfo()
		{
			this.characterInfo = new TMP_CharacterInfo[8];
			this.wordInfo = new TMP_WordInfo[0x10];
			this.linkInfo = new TMP_LinkInfo[0];
			this.lineInfo = new TMP_LineInfo[2];
			this.pageInfo = new TMP_PageInfo[4];
			this.meshInfo = new TMP_MeshInfo[1];
		}

		public TMP_TextInfo(TMP_Text textComponent)
		{
			this.textComponent = textComponent;
			this.characterInfo = new TMP_CharacterInfo[8];
			this.wordInfo = new TMP_WordInfo[4];
			this.linkInfo = new TMP_LinkInfo[0];
			this.lineInfo = new TMP_LineInfo[2];
			this.pageInfo = new TMP_PageInfo[4];
			this.meshInfo = new TMP_MeshInfo[1];
			this.meshInfo[0].mesh = textComponent.mesh;
			this.materialCount = 1;
		}

		public void Clear()
		{
			this.characterCount = 0;
			this.spaceCount = 0;
			this.wordCount = 0;
			this.linkCount = 0;
			this.lineCount = 0;
			this.pageCount = 0;
			this.spriteCount = 0;
			for (int i = 0; i < this.meshInfo.Length; i++)
			{
				this.meshInfo[i].vertexCount = 0;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_TextInfo.Clear()).MethodHandle;
			}
		}

		public void ClearMeshInfo(bool updateMesh)
		{
			for (int i = 0; i < this.meshInfo.Length; i++)
			{
				this.meshInfo[i].Clear(updateMesh);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_TextInfo.ClearMeshInfo(bool)).MethodHandle;
			}
		}

		public void ClearAllMeshInfo()
		{
			for (int i = 0; i < this.meshInfo.Length; i++)
			{
				this.meshInfo[i].Clear(true);
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_TextInfo.ClearAllMeshInfo()).MethodHandle;
			}
		}

		public void ResetVertexLayout(bool isVolumetric)
		{
			for (int i = 0; i < this.meshInfo.Length; i++)
			{
				this.meshInfo[i].ResizeMeshInfo(0, isVolumetric);
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_TextInfo.ResetVertexLayout(bool)).MethodHandle;
			}
		}

		public void ClearUnusedVertices(MaterialReference[] materials)
		{
			for (int i = 0; i < this.meshInfo.Length; i++)
			{
				int startIndex = 0;
				this.meshInfo[i].ClearUnusedVertices(startIndex);
			}
		}

		public void ClearLineInfo()
		{
			if (this.lineInfo == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_TextInfo.ClearLineInfo()).MethodHandle;
				}
				this.lineInfo = new TMP_LineInfo[2];
			}
			for (int i = 0; i < this.lineInfo.Length; i++)
			{
				this.lineInfo[i].characterCount = 0;
				this.lineInfo[i].spaceCount = 0;
				this.lineInfo[i].width = 0f;
				this.lineInfo[i].ascender = TMP_TextInfo.k_InfinityVectorNegative.x;
				this.lineInfo[i].descender = TMP_TextInfo.k_InfinityVectorPositive.x;
				this.lineInfo[i].lineExtents.min = TMP_TextInfo.k_InfinityVectorPositive;
				this.lineInfo[i].lineExtents.max = TMP_TextInfo.k_InfinityVectorNegative;
				this.lineInfo[i].maxAdvance = 0f;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}

		public TMP_MeshInfo[] CopyMeshInfoVertexData()
		{
			if (this.m_CachedMeshInfo != null)
			{
				if (this.m_CachedMeshInfo.Length == this.meshInfo.Length)
				{
					goto IL_D3;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_TextInfo.CopyMeshInfoVertexData()).MethodHandle;
				}
			}
			this.m_CachedMeshInfo = new TMP_MeshInfo[this.meshInfo.Length];
			for (int i = 0; i < this.m_CachedMeshInfo.Length; i++)
			{
				int num = this.meshInfo[i].vertices.Length;
				this.m_CachedMeshInfo[i].vertices = new Vector3[num];
				this.m_CachedMeshInfo[i].uvs0 = new Vector2[num];
				this.m_CachedMeshInfo[i].uvs2 = new Vector2[num];
				this.m_CachedMeshInfo[i].colors32 = new Color32[num];
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
			IL_D3:
			for (int j = 0; j < this.m_CachedMeshInfo.Length; j++)
			{
				int num2 = this.meshInfo[j].vertices.Length;
				if (this.m_CachedMeshInfo[j].vertices.Length != num2)
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
					this.m_CachedMeshInfo[j].vertices = new Vector3[num2];
					this.m_CachedMeshInfo[j].uvs0 = new Vector2[num2];
					this.m_CachedMeshInfo[j].uvs2 = new Vector2[num2];
					this.m_CachedMeshInfo[j].colors32 = new Color32[num2];
				}
				Array.Copy(this.meshInfo[j].vertices, this.m_CachedMeshInfo[j].vertices, num2);
				Array.Copy(this.meshInfo[j].uvs0, this.m_CachedMeshInfo[j].uvs0, num2);
				Array.Copy(this.meshInfo[j].uvs2, this.m_CachedMeshInfo[j].uvs2, num2);
				Array.Copy(this.meshInfo[j].colors32, this.m_CachedMeshInfo[j].colors32, num2);
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
			return this.m_CachedMeshInfo;
		}

		public unsafe static void Resize<T>(ref T[] array, int size)
		{
			int num;
			if (size > 0x400)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_TextInfo.Resize(T[]*, int)).MethodHandle;
				}
				num = size + 0x100;
			}
			else
			{
				num = Mathf.NextPowerOfTwo(size);
			}
			int newSize = num;
			Array.Resize<T>(ref array, newSize);
		}

		public unsafe static void Resize<T>(ref T[] array, int size, bool isBlockAllocated)
		{
			if (isBlockAllocated)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_TextInfo.Resize(T[]*, int, bool)).MethodHandle;
				}
				int num;
				if (size > 0x400)
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
					num = size + 0x100;
				}
				else
				{
					num = Mathf.NextPowerOfTwo(size);
				}
				size = num;
			}
			if (size == array.Length)
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
				return;
			}
			Array.Resize<T>(ref array, size);
		}
	}
}
