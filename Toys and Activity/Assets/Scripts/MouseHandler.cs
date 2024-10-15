using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickObject : MonoBehaviour
{
    public GameObject Lever;
    public GameObject Ball;
    //private int flag = 1;
    //private Vector3 PointA;
    //private Vector3 PointB;

    private int Rotations;
    private int count = 0;
    void Start()
    {
        Rotations = Random.Range(5, 21);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Lever == GetClickedObject(out RaycastHit hit))
            {
                if (count == Rotations)
                {

                }
                else
                {
                    count++;
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