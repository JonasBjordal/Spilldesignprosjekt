using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SortingLayer))]
public class SortingLayerEditor : Editor {

	// HELLO READ THIS!
	// You need to put this script in a folder called 'Editor'

	private SortingLayer sortingLayer;

	void OnEnable() {
		sortingLayer = (SortingLayer)target;
		sortingLayer.LoadSortingLayers();
	}

	public override void OnInspectorGUI() {
		int index = sortingLayer.GetLayerIndex(sortingLayer.currentSortingLayerName);
		index = EditorGUILayout.Popup("Sorting Layer", index, sortingLayer.sortingLayerNames, GUILayout.ExpandWidth(true));
		if (index >= 0)
			sortingLayer.currentSortingLayerName = sortingLayer.sortingLayerNames[index];
		sortingLayer.SetSortingLayers();
		EditorUtility.SetDirty(sortingLayer);
	}

}