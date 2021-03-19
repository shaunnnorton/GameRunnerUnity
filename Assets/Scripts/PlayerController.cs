using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject Player;
    public Rigidbody PlayerRB;
    public Vector2 MoveDirection;
    public GameObject Projectile;
    public CharacterController Controller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*if (PlayerRB.velocity.magnitude < 15) {
            PlayerRB.AddForce(new Vector3(-MoveDirection.x, 0, -MoveDirection.y) * 30);
        }*/
        Controller.Move(new Vector3(-MoveDirection.x, 0, -MoveDirection.y));

    }


    public void OnFire()
    {
        Instantiate(Projectile, Player.transform.position, Player.transform.rotation);
    }
    public void OnMove(InputValue input)
    {
        Vector2 inputValue = input.Get<Vector2>();
        MoveDirection = inputValue;
        Debug.Log(new Vector3(-inputValue.x, 0, -inputValue.y) * 30);
    }
    public void OnLook(InputValue input)
    {
        Vector2 LookVector = input.Get<Vector2>();
        Vector3 LookPoint;

        Plane plane = new Plane(Vector3.up,Player.transform.position);
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(LookVector);
        if (plane.Raycast(ray,out distance))
        {
            LookPoint = ray.GetPoint(distance);
            Player.transform.LookAt(LookPoint);
            /*Debug.Log(LookPoint);*/
        }


        /*Debug.Log(input.Get<Vector2>());*/
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            SceneManager.LoadScene("WorldScene");
        }
    }
}

