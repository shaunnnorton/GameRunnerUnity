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
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        EnemyRenderer = Enemy.GetComponent<MeshRenderer>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Enemy.transform.LookAt(Player.transform);

        if (EnemyRB.velocity.magnitude < 25)
        {
            EnemyRB.AddRelativeForce(Vector3.forward * 0.5f);
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
}
