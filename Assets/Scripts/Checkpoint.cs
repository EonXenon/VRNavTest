using UnityEngine;
using UnityEngine.VFX;

public class Checkpoint: CourseObjective
{
    [SerializeField]
    Transform cube;
    [SerializeField]
    VisualEffect vfx;

    public float radius = 5f;
    public float height = 5f;

    float cubeHeightVariation = 0.5f;
    float cubeHeightCenter = 2f;
    float cubeHeightLoopDuration = 5f;
    float cubeRotationRate = 15f;

    // Start is called before the first frame update
    void Start()
    {
        vfx.SetFloat("Height", height);
        vfx.SetFloat("Radius", radius);
    }

    // Update is called once per frame
    void Update()
    {
        float cubeHeight = Mathf.Sin((Time.time * Mathf.PI * 2f)/ cubeHeightLoopDuration) * cubeHeightVariation + cubeHeightCenter;
        cube.localPosition = Vector3.up * cubeHeight;
        cube.localRotation = Quaternion.Euler(45f, Time.time * cubeRotationRate, 45f);
        vfx.SetFloat("Cube Height", cubeHeight);
    }

    public override bool CheckTrigger(Transform target)
    {
        Vector3 diff = target.position - transform.position;
        float verDiff = diff.y;
        float horDiff = Mathf.Sqrt(diff.x * diff.x + diff.z * diff.z);
        return verDiff > 0f && verDiff < height && horDiff < radius;
    }

    public override void SetColor(Color color) => vfx.SetVector4("Color", color);
}
