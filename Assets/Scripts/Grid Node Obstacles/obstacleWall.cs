using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class obstacleWall : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (gameObject.layer == 0)
                    {
                        gameObject.layer = 3;
                        PaintGameObject(this.gameObject, Color.yellow);

                    }
                    else
                    {
                        gameObject.layer = 0;
                        PaintGameObject(this.gameObject, Color.cyan);
                    }
                    EventManager.TriggerEvent(EventsType.WALL_BLOCK);
                }
            }
        }
    }

    public void PaintGameObject(GameObject obj, Color color)
    {
        obj.GetComponent<Renderer>().material.color = color;
    }
}
