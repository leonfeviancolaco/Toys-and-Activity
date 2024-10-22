using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickObject : MonoBehaviour
{
    public GameObject Ball;
    public GameObject Lever;
    private int flag = 1;
    void Start()
    {
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Ball == GetClickedObject(out RaycastHit hit))
            {
                if (flag == 1)
                {
                    Ball.transform.position = new Vector3 (182,360,-1128);
                    flag = 2;
                }
                else if (flag == 2)
                {
                    Ball.transform.position = new Vector3(1076, 360, -1128);
                    flag = 1;
                }
            }
            
        }      
    }

    GameObject GetClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            if (!isPointerOverObject()) { target = hit.collider.gameObject; }
        }
        return target;
    }
    private bool isPointerOverObject()
    {
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);
        return results.Count > 0;
    }
}