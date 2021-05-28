using System.Collections.Generic;
using UnityEngine;

public class LiteFrustumCulling : MonoBehaviour
{
    #region
    public GameObject batchRoot;

    private Camera headCamera;
    [Range(-1f,1f)]
    public float cullingPoint;

    private int totalObj;
    private int visObj;
    public int hiddenObjects;

    private Vector3      LP; // Last Position
    private Quaternion   LR; // Last Rotation

    private OrientationCulling[] cullingList;
    private GameObject[] goList;
#endregion

    private void Start()
    {
        headCamera = Camera.main;

        cullingList = FindObjectsOfType<OrientationCulling>();

        List<GameObject> tempGOs = new List<GameObject>();

        System.Random rng = new System.Random(0);

        foreach (OrientationCulling obj in cullingList)
        {
            Mesh nMesh = obj.GetComponent<MeshFilter>().mesh;
            tempGOs.Add(obj.gameObject);

            Color32[] colors = new Color32[nMesh.vertexCount];
            Color32 color = new Color(Mathf.Repeat(obj.transform.parent.position.x + 12345.6f * obj.transform.parent.position.y,1f), 0f,0f, 1f);
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = color;//Color.Lerp(Color.black, Color.red, color);
            }
            nMesh.colors32 = colors;
        }

        goList = tempGOs.ToArray();
        StaticBatchingUtility.Combine(goList, batchRoot);

        totalObj = cullingList.Length;
    }

    private void LateUpdate()
    {
        if (LP != headCamera.transform.position || LR != headCamera.transform.rotation)
        {
            LP = headCamera.transform.position; LR = headCamera.transform.rotation;

            visObj = 0;

            foreach (OrientationCulling obj in cullingList)
            {
                Vector3 faceDirection = obj.transform.rotation * obj.faceOrientation;
                bool prevState = obj.objRenderer.enabled;
                obj.objRenderer.enabled = (
                    (obj.neverCull
                    || Vector3.Dot(faceDirection, ((obj.objRenderer.bounds.ClosestPoint(headCamera.transform.position) - ((obj.thicknessCompensation) ? faceDirection : Vector3.zero)) - headCamera.transform.position).normalized) < cullingPoint)
                    && GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(headCamera), obj.objRenderer.bounds));
                visObj += (obj.objRenderer.enabled) ? 1 : 0;

                /*if(obj.objRenderer.enabled != prevState)
                    foreach (Renderer extra in obj.extraRenderers)
                    {
                        extra.enabled = obj.objRenderer.enabled;
                    }*/
            }

            hiddenObjects = totalObj - visObj;
        }
    }
}