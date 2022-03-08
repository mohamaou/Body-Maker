using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer, obstacleMask;
    [SerializeField] [Range(0, 360)] private float angele = 90f;
    [SerializeField] private float range = 4f;
    [SerializeField] private float resolution = 100f;
    [HideInInspector] public List<Transform> visibleTargets;


    private void Update()
    {
        FindVisibleTargets();
    }


    private void FindVisibleTargets()
    {
        visibleTargets.Clear ();
        var position = transform.position + Vector3.up / 2;
        var targetsInViewRadius = new Collider[10];
        var size = Physics.OverlapSphereNonAlloc(position, range, targetsInViewRadius, enemyLayer);
        if (size == 0) return;
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            if (targetsInViewRadius[i] != null)
            {
                var target = targetsInViewRadius[i].transform;
                var targetPosition = target.position + Vector3.up / 2;
                Vector3 dirToTarget = ( targetPosition - position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < angele / 2)
                {
                    var dstToTarget = Vector3.Distance(transform.position, targetPosition);
                    if (!Physics.Raycast(position, dirToTarget, dstToTarget, obstacleMask))
                    {
                        visibleTargets.Add(target);
                    }
                }   
            }
        }
    }
    
    
    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal = false) 
    { 
        if (!angleIsGlobal) 
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var viewPoint = transform.position + Vector3.up / 2;
       
        for (int i = 0; i < resolution; i++)
        {
            var delta = i / resolution;
            angele *= -1;
            Gizmos.DrawLine(viewPoint, viewPoint + DirFromAngle(angele/2 * delta) * range);
        }
    }
}
