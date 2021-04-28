using UnityEngine;

public class CourseObjective: MonoBehaviour
{
    public virtual bool CheckTrigger(Transform target) => false;
    public virtual void SetColor(Color color){}
}
