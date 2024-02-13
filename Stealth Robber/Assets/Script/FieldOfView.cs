using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Range(0, 360)]
    public float viewAngle;
    public float viewRadius;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();
    public float meshResolution;
    public int edgeResultIteration;
    public float edgeDistanceThreshold;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while(true)
        {
            FindVisibleTarget();
            yield return new WaitForSeconds(delay);
        }
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    private void FindVisibleTarget()
    {
        visibleTargets.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for(int i = 0; i < colliders.Length; i++)
        {
            Transform target = colliders[i].transform;
            Vector3 dirToTarget = target.position - transform.position;
            if(Vector3.Angle(transform.forward, dirToTarget) <= viewAngle/2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    private void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldCastInfo = new ViewCastInfo();

        for(int i = 0; i <= stepCount; i++ )
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newCast = ViewCast(angle);

            if(i > 0)
            {
                bool edgeDistanceThresholdExceed = Mathf.Abs(oldCastInfo.distance - newCast.distance) > edgeDistanceThreshold;
                if((oldCastInfo.hit != newCast.hit) || (oldCastInfo.hit && newCast.hit && edgeDistanceThresholdExceed))
                {
                    EdgeInfo info = FindEdge(oldCastInfo, newCast);

                    if(info.pointA != Vector3.zero)
                    {
                        viewPoints.Add(info.pointA);
                    }

                    if (info.pointB != Vector3.zero)
                    {
                        viewPoints.Add(info.pointB);
                    }
                }
            }

            viewPoints.Add(newCast.point);
            oldCastInfo = newCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for(int i = 0; i < vertices.Length - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if(i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private EdgeInfo FindEdge(ViewCastInfo minCastInfo, ViewCastInfo maxCastInfo)
    {
        float minAngle = minCastInfo.angle;
        float maxAngle = maxCastInfo.angle;

        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for(int i = 0; i < edgeResultIteration; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newCastInfo = ViewCast(angle);

            bool edgeDistanceThresholdExceed = Mathf.Abs(minCastInfo.distance - newCastInfo.distance) > edgeDistanceThreshold;
            if (newCastInfo.point == minCastInfo.point && !edgeDistanceThresholdExceed)
            {
                minAngle = angle;
                minPoint = newCastInfo.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newCastInfo.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;

        if(Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirectionFromAngle(float angleInDegree, bool isGlobalAngle)
    {
        if(!isGlobalAngle)
        {
            angleInDegree += transform.localEulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegree * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegree * Mathf.Deg2Rad));
    }
}

public struct ViewCastInfo
{
    public bool hit;
    public Vector3 point;
    public float distance;
    public float angle;

    public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
    {
        hit = _hit;
        point = _point;
        distance = _distance;
        angle = _angle;
    }
}

public struct EdgeInfo
{
    public Vector3 pointA;
    public Vector3 pointB;

    public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
    {
        pointA = _pointA;
        pointB = _pointB;
    }
}
