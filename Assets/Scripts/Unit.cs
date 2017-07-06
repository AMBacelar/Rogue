using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

    public int tileX;
    public int tileY;
    public MapHandler map;

    public List<Node> CurrentPath = null;

    int moveSpeed = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (CurrentPath != null)
        {
            int currNode = 0;

            while (currNode < CurrentPath.Count - 1)
            {

                Vector3 start = map.TileCoordToWorldCoord(CurrentPath[currNode].x, CurrentPath[currNode].y) +
                    new Vector3(0, 0.25f, 0);
                Vector3 end = map.TileCoordToWorldCoord(CurrentPath[currNode + 1].x, CurrentPath[currNode + 1].y) +
                    new Vector3(0, 0.25f, 0);

                Debug.DrawLine(start, end, Color.red);

                currNode++;
            }
        }

        // Smoothly animate towards the correct map tile.
        transform.position = Vector3.Lerp(transform.position, map.TileCoordToWorldCoord(tileX, tileY), 5f * Time.deltaTime);
    }

    public void MoveNextTile()
    { 
        float remainingMovement = moveSpeed;

        while (remainingMovement > 0)
        {
            if (CurrentPath == null)
                return;

            // Get cost from current tile to next tile
            remainingMovement -= map.CostToEnterTile(CurrentPath[0].x, CurrentPath[0].y, CurrentPath[1].x, CurrentPath[1].y);

            // Move us to the next tile in the sequence            

            tileX = CurrentPath[1].x;
            tileY = CurrentPath[1].y;

            //transform.position = map.TileCoordToWorldCoord(tileX, tileY);   // Update our unity world position

            // Remove the old "current" tile
            CurrentPath.RemoveAt(0);

            if (CurrentPath.Count == 1)
            {
                // We only have one tile left in the path, and that tile MUST be our ultimate
                // destination -- and we are standing on it!
                // So let's just clear our pathfinding info.
                CurrentPath = null;
            }
        }
        
    }
}
