using System.Collections.Generic;
using UnityEngine;

namespace Wasabimole.InspectorNavigator
{
	public class InspectorBreadcrumbs : MonoBehaviour
	{
		[HideInInspector]
		public int DataVersion;

		public ObjectReference CurrentSelection;

		public List<ObjectReference> Back = new List<ObjectReference>();

		public List<ObjectReference> Forward = new List<ObjectReference>();
	}
}
