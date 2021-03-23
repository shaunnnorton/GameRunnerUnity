using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyController : MonoBehaviour
{
    public GameObject Enemy;
    public Rigidbody EnemyRB;
    public MeshRenderer EnemyRenderer;
    public int id;
    public string image_url;
    public Material matPrefab;
    public Material thismaterial;
    public Texture texture;
    public GameObject Player;
    public float speed;
    public GameManager gameManager;
    public bool gamestate;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gamestate = gameManager.gamestate;


        Player = GameObject.FindGameObjectWithTag("Player");
        EnemyRenderer = Enemy.GetComponent<MeshRenderer>();
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gamestate = gameManager.gamestate;
        Enemy.transform.LookAt(Player.transform);
        if(speed == 0)
        {
            EnemyRB.velocity = Vector3.zero; 
        }
        
        if (EnemyRB.velocity.magnitude < 2)
        {
            /*EnemyRB.AddRelativeForce(Vector3.forward * speed) ;*/
            Enemy.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    IEnumerator SetImage()
    {
        /*Debug.Log(image_url+" THIS SHOLD PRINt");*/
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(image_url);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        { 
            texture = DownloadHandlerTexture.GetContent(request);
        }

        thismaterial = new Material(matPrefab);
        thismaterial.mainTexture = texture;
        EnemyRenderer.material = thismaterial;
    }

    IEnumerator StartGame()
    {
        float temp = speed;
        speed = 0;
        yield return new WaitUntil(() => gamestate == true);
        yield return new WaitForSeconds(1);
        speed = temp;
    }

}
