using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{
    public int rotationOffset = 90;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;      //subtracting the position of the player from the mouse position
        difference.Normalize();         //normalizing the vector. This means that all the sum of the vector will be equal to 1.
        Vector3 Pointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; //find the angle in degrees


        //Playing around with max and min angles here
        /*
        if (Pointer.x > transform.position.x)
        {
            rotZ = Mathf.Clamp(rotZ, -35, 35);
        }
        else if (Pointer.x < transform.position.x)
        {
            //rotZ = Mathf.Clamp(rotZ, 135, 225);
          
            if ((rotZ  < 145) && (rotZ > 90))
            {
                rotZ = 145;
            }else if ((rotZ > -145) && (rotZ < -90))
            {
                rotZ = -145;
            }


        }*/
        //Debug.Log("RotZ is:" + rotZ);
        
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
    }
}
