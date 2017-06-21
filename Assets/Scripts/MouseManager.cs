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
            Debug.Log("1");
            selectedUnit.GetComponent<Unit>().MoveNextTile();
        }

        if (Input.GetMouseButtonDown(0))
        {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log("Raycast hit: " + hitInfo.collider.transform.parent.name);
                Debug.Log("Hexagon Array position = " + hitInfo.collider.gameObject.transform.parent.GetComponent<Hex>().xPos + "," + hitInfo.collider.gameObject.transform.parent.GetComponent<Hex>().yPos);
                Debug.Log("Object position = " + hitInfo.collider.gameObject.transform.position);

                CurrentMap.GeneratePathTo(hitInfo.collider.gameObject.transform.parent.GetComponent<Hex>().xPos, hitInfo.collider.gameObject.transform.parent.GetComponent<Hex>().yPos);
            }
            
        }
    }

}