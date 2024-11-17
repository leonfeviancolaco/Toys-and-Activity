using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class ClickObject : MonoBehaviour
{
    [SerializeField] GameObject Ball;

    private int Flag = 1;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Ball == GetClickedObject(out RaycastHit hit))
            {
                if (Flag == 1)
                {
                    Ball.transform.position = new Vector3(1103, 634, -638);
                    Flag = 2;
                }
                else if (Flag == 2)
                {
                    Ball.transform.position = new Vector3(265, 637, -638);
                    Flag = 3;
                }
                else if (Flag == 3)
                {
                    Ball.transform.position = new Vector3(258, 192, -638);
                    Flag = 1;
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
            if (!isPointerOverUIObject()) { target = hit.collider.gameObject; }
        }
        return target;
    }
    private bool isPointerOverUIObject()
    {
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);
        return results.Count > 0;
    }
}
