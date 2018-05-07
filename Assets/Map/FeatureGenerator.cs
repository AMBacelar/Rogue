using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureGenerator
{
    static int count;
    private bool playerPlaced = false;
    /*
    * things to look out for
    * what depth the player is on
    * what features have already been placed on the map
    * has the stairs down been placed?
    * do you need stairs going up
    * has the player been placed
    * how dense should the map be? moster-wise?
    */

    public void Populate(IntVector2 pos, int monsterDensity, int depth)
    {
        // if player hasn't been placed, place player

        // place a monster
        if (Random.Range(0, 3000) < monsterDensity + (depth / 4))
        {
            if (playerPlaced == false)
            {
                GameObject player = new GameObject("player");
                player.tag = "Player";
                player.AddComponent<LinearWeapon>();
                player.GetComponent<LinearWeapon>().weapomName = "wooden sword";
                player.GetComponent<LinearWeapon>().range = 1;
                player.GetComponent<LinearWeapon>().damage = 1;
                player.AddComponent<PlayerActor>();
                player.AddComponent<MoveAction>();
                player.AddComponent<Destructible>();
                player.GetComponent<Destructible>().startingHP = 12;
                player.GetComponent<BoardPosition>().Initialize(pos);
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.GetComponent<Renderer>().material = Resources.Load("Materials/Player", typeof(Material)) as Material;
                cube.transform.localScale = new Vector3(.5f, .5f, .5f);
                cube.transform.position = new Vector3();
                cube.transform.parent = player.transform;
                player.transform.position = BoardManager.instance.TileCoordToWorldCoord(pos.X, pos.Y) + new Vector3(0, .29f, 0);

                playerPlaced = true;
            }
            else
            {
                count++;
                GameObject enemyGO = new GameObject("enemy_" + count);
                enemyGO.tag = "Enemy";
                enemyGO.AddComponent<EnemyActor>();
                enemyGO.AddComponent<Destructible>();
                enemyGO.GetComponent<Destructible>().startingHP = 2;
                enemyGO.GetComponent<BoardPosition>().Initialize(pos);
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.GetComponent<Renderer>().material = Resources.Load("Materials/Enemy", typeof(Material)) as Material;
                sphere.transform.localScale = new Vector3(.5f, .5f, .5f);
                sphere.transform.position = new Vector3();
                sphere.transform.parent = enemyGO.transform;
                enemyGO.transform.position = BoardManager.instance.TileCoordToWorldCoord(pos.X, pos.Y) + new Vector3(0, .29f, 0);
            }
        }
    }

}
