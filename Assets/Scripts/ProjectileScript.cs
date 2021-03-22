using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public GameObject Player;
    private Collider PlayerCollider;
    public GameObject Projectile;
    private Collider ProjectileCollider;
    public Rigidbody ProjectileRB;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerCollider = Player.GetComponent<Collider>();
        ProjectileCollider = Projectile.GetComponent<Collider>();
        Physics.IgnoreCollision(PlayerCollider, ProjectileCollider);
        StartCoroutine(DestroyAfter());
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int layerMask = 1 << 6;
        ProjectileRB.AddRelativeForce(Vector3.forward * 30f);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1, layerMask))
        {
            gameManager.score += 50;
            gameManager.enemyCount--;
            gameManager.SendMessage("UpdateUI");
            Destroy(hit.transform.gameObject);
            Destroy(Projectile);
            Debug.Log(hit.transform.gameObject.tag);
        }
        
    }
    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(3);
        Destroy(Projectile);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            gameManager.score += 50;
            gameManager.enemyCount--;
            gameManager.SendMessage("UpdateUI");
            Destroy(collision.gameObject);
            Destroy(Projectile);

        }
    }

}
