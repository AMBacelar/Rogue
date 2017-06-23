using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour
{
    public MapHandler CurrentMap;
    public GameObject MapHolder;

    GameObject selectedUnit;



    // Use this for initialization
    void Start()
    {
        GameObject MapHolder = GameObject.Find("Map");
        CurrentMap = MapHolder.GetComponent<MapHandler>();

        selectedUnit = GameObject.FindGameObjectWithTag("Player");
    }
    

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {            
            selectedUnit.GetComponent<Unit>().MoveNextTile();
        }

        if (Input.GetMouseButtonDown(0))
        {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                IEnumerator pathfinder = CurrentMap.GeneratePathTo(hitInfo.collider.gameObject.transform.parent.GetComponent<Hex>().xPos, hitInfo.collider.gameObject.transform.parent.GetComponent<Hex>().yPos, selectedUnit);
                CurrentMap.StartCoroutine(pathfinder);
                //CurrentMap.GeneratePathTo(hitInfo.collider.gameObject.transform.parent.GetComponent<Hex>().xPos, hitInfo.collider.gameObject.transform.parent.GetComponent<Hex>().yPos, selectedUnit);
            }
            
        }
    }

}