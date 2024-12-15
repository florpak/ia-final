using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int speed;
    bool _redAgent;
    Vector3 target;
    Vector3 direction;
    
    void Start()
    {
        direction = (target - transform.position).normalized;
    }
    public void Initialize(bool redAgent, Vector3 attackTarget)
    {
        target = attackTarget;
        _redAgent = redAgent;
    }

    void Update()
    {
        transform.position += direction.normalized * speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        MinionNPC colliderAgent = other.gameObject.GetComponent<MinionNPC>();
        if (colliderAgent != null)
        {
            if (colliderAgent.redNPC != _redAgent)
            {
                colliderAgent.Hurt();
                Destroy(this.gameObject);
            }
        }
        if (other.gameObject.layer == 6)
        {
            Destroy(this.gameObject);
        }
    }
}
