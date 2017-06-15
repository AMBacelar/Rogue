using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if( Physics.Raycast(ray, out hitInfo) )
        {
            Debug.Log("Raycast hit: " + hitInfo.collider.transform.parent.name);
        }
	}
}
