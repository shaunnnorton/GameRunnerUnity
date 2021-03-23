using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemies;
    public GameObject enemyPrefab;
    public GameObject Player;
    public List<string> images;
    private string username;
    private string password;
    public TMP_Text scoreText;
    public TMP_Text enemyText;
    public TMP_Text levelText;
    [System.NonSerialized]
    public int score;
    [System.NonSerialized]
    public int enemyCount;
    [System.NonSerialized]
    private int level;
    public bool gamestate;
    private int enemySpawn = 10;
    public TMP_Text Loading_Text;
    public GameObject Loading_Screen;
    public GameObject LoadingBG;
    public GameObject GameOverScreen;
    public GameObject PauseScreen;
    public bool isPaused;

    public PlayerController playerController;



    void Start()
    {
        StartCoroutine(RunGame());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(enemyCount);
        if (enemyCount == 0 && level > 0)
        {
            StartCoroutine(NextLevel());
        }
    }

    public void SpawnEnemies(int enemy_amount)
    { 
        Vector3 PlayerPos = Player.transform.position;
        int counter = enemy_amount;
        while (counter > 0)
        {
            int spawnDistance = Random.Range(15, 20);
            Vector3 spawnPos = Random.onUnitSphere * spawnDistance;
            Vector3 actualSpawn = new Vector3(PlayerPos.x + spawnPos.x, PlayerPos.y + 0.5f, PlayerPos.z + spawnPos.z);
            Instantiate(enemyPrefab, actualSpawn,Quaternion.identity);
            counter--;
        }
    }

    public void EnemyImages()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            if(images.Count == 0)
            {
                return;
            }


            EnemyController enemyScript;
            enemyScript = enemy.GetComponent<EnemyController>();
            int ImageNumber = Random.Range(0, images.Count - 1);
            //Debug.Log(ImageNumber);
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
            //Debug.Log(ggg["Data"]);
            foreach (var image in ggg["Data"][0]["USER_GAMES"])
            {
                var values = JSON.Parse(image.ToString());
                images.Add(values[0]["image"].Value);

           
            }
            
        }
        EnemyImages();
    }
    IEnumerator RunGame()
    {
        username = PlayerPrefs.GetString("Username");
        password = PlayerPrefs.GetString("Password");
        SpawnEnemies(enemySpawn);
        yield return new WaitForEndOfFrame();
        yield return StartCoroutine(GetImages());
        Debug.Log("GOTIMAGES");
        enemyCount = enemies.Length;
        score = 0;
        level = 1;
        enemyText.text = "Enemies Left: " + enemyCount.ToString();
        scoreText.text = "Score: " + score.ToString();
        levelText.text = level.ToString();
        yield return new WaitForSeconds(3);
        int counter = 10;
        while (counter > 0)
        {
            Loading_Text.text = counter.ToString();
            yield return new WaitForSeconds(1);
            counter--;
        }
        Loading_Screen.SetActive(false);
        gamestate = true;
    }

    public void UpdateUI()
    {
        enemyText.text = "Enemies Left: " + enemyCount.ToString();
        scoreText.text = "Score: " + score.ToString();
        levelText.text = level.ToString();
        
    }

    

    IEnumerator NextLevel()
    {
        level++;
        enemyCount = 1111;
        Debug.Log("Starting Next Level");
        gamestate = false;
        Loading_Text.text = "LEVEL COMPLETE";
        Loading_Text.color = new Color(255, 215, 0, 1);
        Image Loadingimage = LoadingBG.GetComponent<Image>();
        Loadingimage.color = new Color(0,0,0,0);
        Loading_Screen.SetActive(true);
        Loadingimage.color = new Color(0, 0, 0, 0);
        while (Loadingimage.color.a < 1)
        {
            yield return new WaitForSeconds(0.05f);
            Loadingimage.color = new Color(0,0,0,Loadingimage.color.a +0.01f);
        }
        enemySpawn++;
        SpawnEnemies(enemySpawn);
        yield return new WaitForEndOfFrame();
        yield return StartCoroutine(GetImages());
        Debug.Log("GOTIMAGES");
        enemyCount = enemies.Length;
        UpdateUI();
        int counter = 10;
        while (counter > 0)
        {
            Loading_Text.text = counter.ToString();
            yield return new WaitForSeconds(1);
            counter--;
        }
        Loading_Screen.SetActive(false);
        gamestate = true;
    }

    public void gameOver()
    {
        gamestate = false;
        GameOverScreen.SetActive(true);

    }
    public void restart()
    {
        SceneManager.LoadScene("WorldScene");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Title Scene");
    }

    public void Pause(bool method)
    { 
        foreach (GameObject enemy in enemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (method == true)
            {
                enemyController.speed = 0;
            }
            else
            {
                enemyController.speed = 0.5f;
            }
        }
        if(method == true)
        {
            isPaused = true;
            playerController.speed = 0;
            PauseScreen.SetActive(true);
        }
        else
        {
            PauseScreen.SetActive(false);
            isPaused = false;
            playerController.speed = 5;
        }
    }
}

