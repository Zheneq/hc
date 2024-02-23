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
				return m_Name;
			}
			set
			{
				if (!(value != m_Name))
				{
					return;
				}
				while (true)
				{
					m_Name = value;
					return;
				}
			}
		}

		public int hashCode
		{
			get
			{
				return m_HashCode;
			}
			set
			{
				if (value == m_HashCode)
				{
					return;
				}
				while (true)
				{
					m_HashCode = value;
					return;
				}
			}
		}

		public string styleOpeningDefinition
		{
			get { return m_OpeningDefinition; }
		}

		public string styleClosingDefinition
		{
			get { return m_ClosingDefinition; }
		}

		public int[] styleOpeningTagArray
		{
			get { return m_OpeningTagArray; }
		}

		public int[] styleClosingTagArray
		{
			get { return m_ClosingTagArray; }
		}

		public void RefreshStyle()
		{
			m_HashCode = TMP_TextUtilities.GetSimpleHashCode(m_Name);
			m_OpeningTagArray = new int[m_OpeningDefinition.Length];
			for (int i = 0; i < m_OpeningDefinition.Length; i++)
			{
				m_OpeningTagArray[i] = m_OpeningDefinition[i];
			}
			while (true)
			{
				m_ClosingTagArray = new int[m_ClosingDefinition.Length];
				for (int j = 0; j < m_ClosingDefinition.Length; j++)
				{
					m_ClosingTagArray[j] = m_ClosingDefinition[j];
				}
				return;
			}
		}
	}
}
