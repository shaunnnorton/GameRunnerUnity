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
        PlayerRB.AddForce(Vector3.up * 90);
    }
    public void OnMove(InputValue input)
    {
        Vector2 inputValue = input.Get<Vector2>();
    }
    public void OnLook(InputValue input)
    {
        Debug.Log(input.Get<Vector2>());
        Player.transform.LookAt(input.Get<Vector2>());
    }
}

