using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemies;
    public GameObject enemyPrefab;
    public GameObject Player;
    public List<string> images;
    public string username;
    public string password;
    

    void Start()
    {
        username = PlayerPrefs.GetString("Username");
        password = PlayerPrefs.GetString("Password");
        SpawnEnemies(10);
        StartCoroutine(GetImages());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemies(int enemy_amount)
    { 
        Vector3 PlayerPos = Player.transform.position;
        int counter = enemy_amount;
        while (counter > 0)
        {
            int spawnDistance = Random.Range(15, 20);
            Vector3 spawnPos = Random.onUnitSphere * spawnDistance;
            Vector3 actualSpawn = new Vector3(PlayerPos.x + spawnPos.x, PlayerPos.y, PlayerPos.z + spawnPos.z);
            Instantiate(enemyPrefab, actualSpawn,Quaternion.identity);
            counter--;
        }
    }

    public void EnemyImages()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {

            EnemyController enemyScript;
            enemyScript = enemy.GetComponent<EnemyController>();
            int ImageNumber = Random.Range(0, images.Count - 1);
            Debug.Log(ImageNumber);
            enemyScript.image_url = images[ImageNumber];
            enemy.SendMessage("SetImage");
        }
    }

    IEnumerator GetImages()
    {
        string API_KEY = "TURKEYDAY";
        string NAME = username;
        string PASSWORD =password;
        WWWForm data = new WWWForm();
        data.AddField("KEY", API_KEY);
        data.AddField("NAME", NAME);
        data.AddField("PASSWORD", PASSWORD);




        UnityWebRequest request;
        request = UnityWebRequest.Post("http://127.0.0.1:5000/API/GET/USERIMAGES", data);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            // Show results as text
            /*Debug.Log(request.downloadHandler.text);*/
            var ggg = JSON.Parse( request.downloadHandler.text);
            Debug.Log(ggg["Data"]);
            foreach (var image in ggg["Data"][0]["USER_GAMES"])
            {
                var values = JSON.Parse(image.ToString());
                images.Add(values[0]["image"].Value);

           
            }
            
        }
        EnemyImages();
    }
}
