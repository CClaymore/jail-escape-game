using System.Linq;
using UnityEngine;

//Team Members: Corey Clayborn, Xander Covalan, Ben Johnson

public class SheriffController : MonoBehaviour {
    // The bounds of sheriff movement
    private const float MinXPos = -22;
    private const float MaxXPos = 20.5f;
    // The speed that the sheriff moves
    private const float MovementSpeed = 0.3875f * (20 / 50f);
    // The time that the sheriff takes to pause
    private const int PauseTime = 50;
    // The speeds that the sheriff should rotate
    private const float RotSpeed180 = 180 / 50f;
    private const float RotSpeed90 = 180 / 20f;

    // The direction the sheriff is moving
    private bool direction = false;
    // If the sheriff should be rotating 180 degrees
    private bool isRotating180 = false;
    // If the sheriff should be rotating 90 degrees
    private bool isRotating90 = false;
    // How many times to call the Movement function with the current action
    private int numCurrentMovement = 0;
    // How long the the sheriff should be pausing
    private int pauseTimeLeft = 0;

    // The head of the sheriff
    private GameObject Head { get; set; }

    // Start is called before the first frame update
    void Start() {
        Head = transform.Find("Head").gameObject;
    }

    // Update is called once per frame
    void FixedUpdate() {
        Vector3 currPos = transform.position; // get current position

        Vector3 myRot = transform.eulerAngles; //get current rotation

        //print(numCallFunction);
        if (pauseTimeLeft == 0) {
            if (numCurrentMovement == 0) {
                if (currPos.x <= MinXPos) { // check the bounds the player can go to
                    direction = true; // change direction if past those bounds
                    //pause = pausetime;
                    //rotate180 = true;
                } else if (currPos.x >= MaxXPos) { // check the bounds the player can go to
                    direction = false; //change direction if past those bounds
                    //pause = pausetime;
                    //rotate180 = true;
                }
                
                if (direction) { // Positive x movement
                    float xPos = currPos.x;
                    //print("xpos: " + xPos);

                    float maxDisplacement = MaxXPos - xPos;
                    //print("maxDisplacement: " + maxDisplacement);

                    float maxIntervals = maxDisplacement / MovementSpeed;
                    //print("maxIntervals: " + maxIntervals);

                    float randomInterval = Random.value * maxIntervals;
                    //print("randomInterval: " + randomInterval);

                    int times = Mathf.CeilToInt(randomInterval);
                    //print("times: " + times);

                    if (randomInterval < 5)
                        times = 5;

                    if (maxIntervals < 30)
                        times = Mathf.CeilToInt(maxIntervals);

                    numCurrentMovement = times;
                    //print("numCallFunction: " + numCallFunction);

                } else { // Negative x movement
                    float xPos = currPos.x;
                    //print("xpos: " + xPos);

                    float maxDisplacement = (MinXPos - xPos) * -1;
                    //print("maxDisplacement: " + maxDisplacement);

                    float maxIntervals = maxDisplacement / MovementSpeed;
                    //print("maxIntervals: " + maxIntervals);

                    float randomInterval = Random.value * maxIntervals;
                    //print("randomInterval: " + randomInterval);

                    int times = Mathf.CeilToInt(randomInterval);
                    //print("times: " + times);

                    if (randomInterval < 5)
                        times = 5;

                    if (maxIntervals < 30)
                        times = Mathf.CeilToInt(maxIntervals);

                    numCurrentMovement = times;
                    //print("numCallFunction: " + numCallFunction);
                }
            } else {
                if (direction) // Positive x movement
                    currPos.x = currPos.x + MovementSpeed; // going left
                else // Negative x movement
                    currPos.x = currPos.x - MovementSpeed; // going right

                numCurrentMovement--; // Decrement how many times to move left

                if (numCurrentMovement == 0) {
                    if (currPos.x <= MinXPos) { // check the bounds the player can go to
                        isRotating180 = true;
                        //direction = !direction;
                    } else if (currPos.x >= MaxXPos) { // check the bounds the player can go to
                        isRotating180 = true;
                        //direction = !direction;
                    } else
                        isRotating90 = true;

                    pauseTimeLeft = PauseTime;
                }
            }
        } else {
            pauseTimeLeft--; // Decrement pause time

            if (isRotating180) { // Turn around
                if (direction) // Positive x movement
                    myRot.y += RotSpeed180; // Turn left
                else // Negative x movement
                    myRot.y -= RotSpeed180; // Turn right
            } else if (isRotating90) { // Look at cells
                if (direction) { // Positive x movement
                    if (pauseTimeLeft >= 40)
                        myRot.y += RotSpeed90; // Turn left
                    else if (pauseTimeLeft < 10)
                        myRot.y -= RotSpeed90; // Turn right
                } else { // Negative x movement
                    if (pauseTimeLeft >= 40)
                        myRot.y -= RotSpeed90; // Turn right
                    else if (pauseTimeLeft < 10)
                        myRot.y += RotSpeed90; // Turn left
                }
            }

            TestForPlayerMovement();

            if(pauseTimeLeft == 0) { // When pause is over, cancel rotation
                isRotating180 = false;
                isRotating90 = false;
            }
        }


        transform.eulerAngles = myRot;
        transform.position = currPos; //set position to changed vector
    }

    void TestForPlayerMovement() {
        // Sends out rays from -30 to 30 degs to find the player
        if (SendRays(out PlayerController player, -30, -25, -20, -15, -10, -5, 0, 5, 10, 15, 20, 25, 30)) {
            // If the player if moving then reset them
            if (player.Moving)
                player.Reset();
        }
    }

    // Sends out a multiple rays at different directions from the sheriff's head and returns true if it hit the player
    bool SendRays(out PlayerController player, params float[] rotations) {
        foreach (float rotation in rotations) {
            Vector3 direction = Head.transform.forward;
            direction = Quaternion.Euler(0, rotation + 90, 0) * direction; // The direction vector to send the ray
            LayerMask layers = ~LayerMask.GetMask("Sheriff", "Bars"); // every layer except the sheriff and the bars
            // Send out ray
            if (Physics.Raycast(Head.transform.position, direction, out RaycastHit hit, Mathf.Infinity, layers)) {
                // If the ray hit the player, out the hit player
                if (LayerMask.LayerToName(hit.transform.gameObject.layer).Equals("Player")) {
                    player = hit.transform.GetComponentInParent<PlayerController>();
                    if (player != null)
                        return true;
                }
            }
        }
        player = default;
        return false; // Return false if not found
    }
}
