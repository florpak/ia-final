using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderFieldOfView : MonoBehaviour
{
    [SerializeField] private float viewRadius;
    [SerializeField] private float viewAngle;
    [SerializeField] GameObject player;
    [SerializeField] LayerMask wallLayer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject FieldOfView()
    {
        Vector3 dir = player.transform.position - transform.position;
        if (dir.magnitude < viewRadius)
        {
            if (Vector3.Angle(transform.forward, dir) < viewAngle / 2)
            {
                Debug.DrawLine(transform.position, transform.position + dir);
                if (!Physics.Raycast(transform.position, dir, out RaycastHit hit, dir.magnitude, wallLayer))
                {
                    return player;
                }
                else
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    return null;
                }
            }
            return null;
        }
        else
        {
            return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, viewRadius);
        Vector3 lineA = GetVectorFromAngle(viewAngle / 2 + transform.eulerAngles.y);
        Vector3 lineB = GetVectorFromAngle(-viewAngle / 2 + transform.eulerAngles.y);

        Gizmos.DrawLine(transform.position, transform.position + lineA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + lineB * viewRadius);
    }

    public Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
