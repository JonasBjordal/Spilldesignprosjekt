#if UNITY_EDITOR
using UnityEngine;
using UnityEditorInternal;
using System.Reflection;

public class SortingLayer : MonoBehaviour {

	public string currentSortingLayerName = "";
	public string[] sortingLayerNames;

	public void LoadSortingLayers() {
		System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		sortingLayerNames = (string[])sortingLayersProperty.GetValue(null, new object[0]);
	}

	public int GetLayerIndex(string layer = "Default") {
		if (sortingLayerNames.Length == 0 || sortingLayerNames == null)
			return -1;
		for (int i = 0; i < sortingLayerNames.Length; i++) {
			if (sortingLayerNames[i].Equals(layer))
				return i;
		}
		return -1;
	}

	public void SetSortingLayers() {
		foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>()) {
			renderer.sortingLayerName = currentSortingLayerName;
		}
	}

}
#endif