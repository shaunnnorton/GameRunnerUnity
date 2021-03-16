using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject Player;
    public Rigidbody PlayerRB;
    public Vector2 MoveDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerRB.velocity.magnitude < 15) {
            PlayerRB.AddForce(new Vector3(-MoveDirection.x, 0, -MoveDirection.y) * 30);
        }
        

    }


    public void OnFire()
    {

        Vector3 newVector = Vector3.forward;
        PlayerRB.AddForce(Vector3.up);
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
}

