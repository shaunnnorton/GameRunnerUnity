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


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerCollider = Player.GetComponent<Collider>();
        ProjectileCollider = Projectile.GetComponent<Collider>();
        Physics.IgnoreCollision(PlayerCollider, ProjectileCollider);
        StartCoroutine(DestroyAfter());
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ProjectileRB.AddRelativeForce(Vector3.forward * 80);
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
            Destroy(collision.gameObject);
            Destroy(Projectile);
        }
    }

}
