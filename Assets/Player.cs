using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = .1f;

    void Update()
    {
		if (Input.GetKey(KeyCode.F))
		{
            speed += .001f;
		}
        else if (Input.GetKey(KeyCode.G))
        {
            speed -= .001f;
        }

        float horizontal = Input.GetAxis("Horizontal") * speed;
        float vertical = Input.GetAxis("Vertical") * speed;

        transform.position += new Vector3(horizontal, vertical, 0);
    }
}