using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFieldOfView : MonoBehaviour
{
    [SerializeField] private float viewRadius;
    [SerializeField] private float viewAngle;
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    [SerializeField] LayerMask wallLayer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var enemy in enemies)
        {
            if (FieldOfView(enemy))
            {
                enemy.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                enemy.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    private bool FieldOfView(GameObject obj)
    {
        Vector3 dir = obj.transform.position - transform.position;
        if (dir.magnitude < viewRadius)
        {
            if(Vector3.Angle(transform.forward, dir)< viewAngle/2)
            {
                Debug.DrawLine(transform.position, transform.position + dir);
                if (!Physics.Raycast(transform.position, dir, out RaycastHit hit, dir.magnitude, wallLayer))
                {
                    return true;
                }
                else
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                }
            }
            return false;
        }
        else
        {
            return false;
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
