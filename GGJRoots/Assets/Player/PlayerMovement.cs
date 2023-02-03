using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector2 GetInput() {
        float vertInput = Input.GetAxisRaw("Vertical");
        float horzInput = Input.GetAxisRaw("Horizontal");
        Vector2 input = new Vector2(horzInput, vertInput);
        return input;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
