using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheriffMovement : MonoBehaviour
{
    bool direction = false;
    bool rotate180 = false;
    bool rotate90 = false;
    int numCallFunction = 0;
    int pause = 0;
    int pauseTime = 50;
    float movementSpeed = 0.3875f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Movement", 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Movement()
    {
        Vector3 corrpos; // new vector for position
        corrpos = transform.position; // set vector to position

        Vector3 myRot; // new vector for rotation
        myRot = transform.eulerAngles; //set the vector3 to the rotation values

        //print(numCallFunction);
        if (pause == 0)
        {
            if (numCallFunction == 0)
            {
                if (corrpos.x <= -22.0f) // check the bounds the player can go to
                {
                    direction = true; // change direction if past those bounds
                    //pause = pausetime;
                    //rotate180 = true;
                }
                else if (corrpos.x >= 20.5f) // check the bounds the player can go to
                {
                    direction = false; //change direction if past those bounds
                    //pause = pausetime;
                    //rotate180 = true;
                }

                if (!direction)
                {
                    float xPos = corrpos.x;
                    //print("xpos: " + xPos);

                    float maxDisplacement = (-22.0f -xPos) * -1;
                    //print("maxDisplacement: " + maxDisplacement);

                    float maxIntervals = maxDisplacement / movementSpeed;
                    //print("maxIntervals: " + maxIntervals);

                    float randomInterval = Random.value * maxIntervals;
                    //print("randomInterval: " + randomInterval);

                    int times = Mathf.CeilToInt(randomInterval);
                    //print("times: " + times);

                    if(randomInterval < 5)
                    {
                        times = 5;
                    }

                    if (maxIntervals < 30)
                    {
                        times = Mathf.CeilToInt(maxIntervals);
                    }

                    numCallFunction = times;
                    //print("numCallFunction: " + numCallFunction);

                }
                else if (direction)
                {
                    float xPos = corrpos.x;
                    //print("xpos: " + xPos);

                    float maxDisplacement = 20.5f - xPos;
                    //print("maxDisplacement: " + maxDisplacement);

                    float maxIntervals = maxDisplacement / movementSpeed;
                    //print("maxIntervals: " + maxIntervals);

                    float randomInterval = Random.value * maxIntervals;
                    //print("randomInterval: " + randomInterval);

                    int times = Mathf.CeilToInt(randomInterval);
                    //print("times: " + times);

                    if (randomInterval < 5)
                    {
                        times = 5;
                    }

                    if (maxIntervals < 30)
                    {
                        times = Mathf.CeilToInt(maxIntervals);
                    }

                    numCallFunction = times;
                    //print("numCallFunction: " + numCallFunction);

                }
            }
            else if (numCallFunction != 0)
            {
                if (!direction)
                {
                    corrpos.x = corrpos.x - movementSpeed; // going right
                }
                else if (direction)
                {
                    corrpos.x = corrpos.x + movementSpeed; // going left
                }
                numCallFunction = numCallFunction - 1;
                if (numCallFunction == 0)
                {
                    if (corrpos.x <= -22.0f) // check the bounds the player can go to
                    {
                        rotate180 = true;
                        //direction = !direction;
                    }
                    else if (corrpos.x >= 20.5f) // check the bounds the player can go to
                    {
                        rotate180 = true;
                        //direction = !direction;
                    }else
                    {
                        rotate90 = true;
                    }


                    pause = pauseTime;
                }
            }
        }
        else if(pause != 0){
            pause = pause - 1;
            float rotation180 = 180.0f / 50;
            float rotation90 = 180.0f / 20;

            if (rotate180)
            {
                if (direction)
                {
                    myRot.y += rotation180;
                }
                else if (!direction)
                {
                    myRot.y -= rotation180;
                }
                
            }
            else if (rotate90)
            {
                if (direction)
                {
                    if (pause >= 40 )
                    {
                        myRot.y += rotation90;
                    }else if (pause < 10)
                    {
                        myRot.y -= rotation90;
                    }
                }
                else if (!direction)
                {
                    if (pause >= 40)
                    {
                        myRot.y -= rotation90;
                    }else if (pause < 10)
                    {
                        myRot.y += rotation90;
                    }
                }
            }
            if(pause ==0)
            {
                rotate180 = false;
                rotate90 = false;
            }
        }


        this.transform.eulerAngles = myRot;
        this.transform.position = corrpos; //set position to changed vector

        Invoke("Movement", 0.05f);
    }

    
}
