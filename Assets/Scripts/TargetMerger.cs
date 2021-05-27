#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class TargetMerger : MonoBehaviour
{
    public Transform targetParent;
}

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
                //filteredTarget.GetComponent<MeshRenderer>().enabled = false;
            }
            /*foreach(Transform filteredTarget in merger.transform)
            {
                string name = filteredTarget.gameObject.name.Split(' ')[0];
                GameObject toCopy = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/CityVoxelPack/Assets/buildings/medium/Prefabs/" + name + ".prefab");
                if (toCopy == null) Debug.Log("Assets/CityVoxelPack/Assets/buildings/medium/Prefabs/" + name + ".prefab");
                GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(toCopy, merger.targetParent);
                obj.transform.position = filteredTarget.position;
                obj.transform.rotation = filteredTarget.rotation;
                //Destroy(filteredTarget.gameObject);
            }*/
        }


    }
}
#endif
