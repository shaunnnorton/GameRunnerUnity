using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemies;
    public GameObject enemyPrefab;
    public GameObject Player;

    void Start()
    {
        SpawnEnemies(10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemies(int enemy_amount)
    {
        Vector3 PlayerPos = Player.transform.position;
        int counter = enemy_amount;
        while (counter < 0)
        {
            Vector2 spawnPos = Random.insideUnitCircle * 15;
            Vector3 actualSpawn = new Vector3(PlayerPos.x + spawnPos.x, PlayerPos.y, PlayerPos.z + spawnPos.y);
            Instantiate(enemyPrefab, actualSpawn,Quaternion.identity);
            counter--;
        }
    }

}
