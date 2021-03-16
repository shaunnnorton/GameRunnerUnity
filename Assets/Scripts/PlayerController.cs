using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject Player;
    public Rigidbody PlayerRB;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void OnFire()
    {

        Vector3 newVector = Vector3.forward;
        PlayerRB.AddForce(Vector3.up);
    }
    public void OnMove(InputValue input)
    {
        Vector2 inputValue = input.Get<Vector2>();
        PlayerRB.AddForce(new Vector3(-inputValue.x, Player.transform.position.y, -inputValue.y)*30);
        Debug.Log(new Vector3(inputValue.x, Player.transform.position.y, inputValue.y) * 30);
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

