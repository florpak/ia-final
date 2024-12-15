using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.back * Time.deltaTime * velocity;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.left * Time.deltaTime * velocity;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.forward * Time.deltaTime * velocity;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.right * Time.deltaTime * velocity;
        }
    }
}
