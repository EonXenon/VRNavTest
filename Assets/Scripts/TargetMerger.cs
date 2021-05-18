#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class TargetMerger : MonoBehaviour { }

[CustomEditor(typeof(TargetMerger))]
public class TargetMergerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TargetMerger merger = (TargetMerger)target;

        if (GUILayout.Button("Clone targets"))
        {
            foreach(FilterTarget filteredTarget in FindObjectsOfType<FilterTarget>())
            {
                Instantiate(filteredTarget.gameObject, merger.transform, true);
                //filteredTarget.gameObject.SetActive(false);
                filteredTarget.GetComponent<MeshRenderer>().enabled = false;
            }
        }


    }
}
#endif
