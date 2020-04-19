using System;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public class TMP_Style
	{
		[SerializeField]
		private string m_Name;

		[SerializeField]
		private int m_HashCode;

		[SerializeField]
		private string m_OpeningDefinition;

		[SerializeField]
		private string m_ClosingDefinition;

		[SerializeField]
		private int[] m_OpeningTagArray;

		[SerializeField]
		private int[] m_ClosingTagArray;

		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				if (value != this.m_Name)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_Style.set_name(string)).MethodHandle;
					}
					this.m_Name = value;
				}
			}
		}

		public int hashCode
		{
			get
			{
				return this.m_HashCode;
			}
			set
			{
				if (value != this.m_HashCode)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_Style.set_hashCode(int)).MethodHandle;
					}
					this.m_HashCode = value;
				}
			}
		}

		public string styleOpeningDefinition
		{
			get
			{
				return this.m_OpeningDefinition;
			}
		}

		public string styleClosingDefinition
		{
			get
			{
				return this.m_ClosingDefinition;
			}
		}

		public int[] styleOpeningTagArray
		{
			get
			{
				return this.m_OpeningTagArray;
			}
		}

		public int[] styleClosingTagArray
		{
			get
			{
				return this.m_ClosingTagArray;
			}
		}

		public void RefreshStyle()
		{
			this.m_HashCode = TMP_TextUtilities.GetSimpleHashCode(this.m_Name);
			this.m_OpeningTagArray = new int[this.m_OpeningDefinition.Length];
			for (int i = 0; i < this.m_OpeningDefinition.Length; i++)
			{
				this.m_OpeningTagArray[i] = (int)this.m_OpeningDefinition[i];
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_Style.RefreshStyle()).MethodHandle;
			}
			this.m_ClosingTagArray = new int[this.m_ClosingDefinition.Length];
			for (int j = 0; j < this.m_ClosingDefinition.Length; j++)
			{
				this.m_ClosingTagArray[j] = (int)this.m_ClosingDefinition[j];
			}
		}
	}
}
