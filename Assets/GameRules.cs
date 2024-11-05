using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;

public class GameRules : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Tilemap collisionMap;

//This is the max coordinates of the spawnable area. There are non spawnable walls within, but we will account for those
    int xMin = -25;
    int xMax = 31;
    int yMin = -14;
    int yMax = 9;

    public int spawnedZombies = 0;

    public int maxZombies = 100;//This will be set to 100 at start of wave 1

    public int deadZombies;

    float TimeSinceLastSpawn = -30f;

    float spawnDelay = 1f;

    public GameObject CleetusPrefab;

    public GameObject player;
    public CinemachineVirtualCamera virtualCamera;


    void Start()
    {
        TimeSinceLastSpawn = Time.time;
        player = Instantiate(CleetusPrefab, Vector2.zero, Quaternion.identity);
        virtualCamera.Follow = player.transform;
    }

    void FixedUpdate()
    {

        Vector3Int spawnHere = getRandomSpawnLoc();//This puts a random location and will check if it's okay to spawn there
        Tile randTile = collisionMap.GetTile<Tile>(spawnHere);//Here we will check if the tile is good

        Vector2 adjustedszie = new Vector2((spawnHere.x * 0.48f) +.15f, (spawnHere.y * .48f) +.33f);

        if(!(randTile != null) && (TimeSinceLastSpawn + spawnDelay < Time.time) && !Physics2D.OverlapCircle(adjustedszie, 1f) && (spawnedZombies < maxZombies))
        //if the randomly selected tile is not a collision tile and it's 
        //been a little since the last zombie and that there is no other collider nearby
        //and there are still more zombies that need to spawn this wave
        {
            //Vector2 spawnHere = new Vector2(randTile.x, randTile.y);
            spawnedZombies++;
            Instantiate(zombiePrefab, adjustedszie, Quaternion.identity);
            TimeSinceLastSpawn = Time.time;
        }

    }

    Vector3Int getRandomSpawnLoc()
    {
        int xCoord= Random.Range(xMin, xMax);
        int yCoord= Random.Range(yMin, yMax);

        Vector3Int spawnLoc = new Vector3Int(xCoord, yCoord, 0);
        return spawnLoc;
    }


}
