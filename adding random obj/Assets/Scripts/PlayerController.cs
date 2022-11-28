using UnityEngine;
using UnityEngine.UI;

// Unity Component that controls the player's movement
public class PlayerController : MonoBehaviour {
    // Different speeds of planar movement
    private const int MoveSpeed = 5;
    private const int RunSpeed = 10;

    // Different speeds of rotation
    private const int HRotSpeed = 250;
    private const int VRotSpeed = 250;

    // Different mins/maxs of player rotation
    private const int MinVRot = -80;
    private const int MaxVRot = 80;

    // Default position and rotation of player on game start
    private readonly Vector3 DefaultPos = new Vector3(-22, 2.125f, -6);
    private readonly Vector3 DefaultRot = new Vector3(0, 90, 0);

    // Head of the player, used for vertical rotation
    private GameObject Head { get; set; }
    // "Eyes" of the player
    private Camera Camera { get; set; }
    // Movement controller for the player that is wrapped around
    private CharacterController Controller { get; set; }
    // The pointer to show what the player is looking at
    private Image Pointer { get; set; }
    // The key item for when the player is holding it
    private GameObject Key { get; set; }
    // The blue key
    private GameObject BlueKey { get; set; }

    // Whether the player is currently moving
    public bool Moving { get; private set; } = false;

    // Finds all the necessary children and components for the PlayerController to work
    void Start() {
        // Collect necessary GameObjects and Components
        Head = transform.Find("Head").gameObject;
        Camera = Head.GetComponent<Camera>();
        Controller = GetComponent<CharacterController>();
        Pointer = transform.Find("Screen").Find("Pointer").GetComponent<Image>();
        Key = Head.transform.Find("Key").gameObject;
        BlueKey = Head.transform.Find("KeyBlue").gameObject;
        
        // Hide mouse and lock it
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Updates the player's position and rotation, and tests for the player interacting with things
    void Update() {
        float delta = Time.deltaTime;

        Rotation(delta);
        Movement(delta);
        TestForInteractions();

        TryExit();
    }

    // Controls where the player is facing
    void Rotation(float delta) {
        // Get user input for mouse movement
        float dx = Input.GetAxis("Mouse X");
        float dy = Input.GetAxis("Mouse Y");

        // Calc rotation changes
        float dhrot = delta * HRotSpeed * dx;
        float dvrot = delta * VRotSpeed * dy;

        // Apply horizontal rotation
        transform.localEulerAngles += new Vector3(0, dhrot);

        // Calc vertical rotation
        float vrot = Head.transform.localEulerAngles.x;
        if (vrot > 180) // Change domain from 0 to 360 to -180 to 180
            vrot -= 360;
        vrot -= dvrot;
        // Make vertical rotation within bounds
        vrot = Mathf.Max(Mathf.Min(vrot, MaxVRot), MinVRot);
        // Apply vrot
        Head.transform.localEulerAngles = new Vector3(vrot, 0);
    }

    // Controls how the player moves
    void Movement(float delta) {
        // Movement vector
        Vector3 movement = new Vector3();

        // Get movement from user input
        if (Input.GetKey(KeyCode.W))
            movement.z += 1;
        if (Input.GetKey(KeyCode.S))
            movement.z -= 1;
        if (Input.GetKey(KeyCode.A))
            movement.x -= 1;
        if (Input.GetKey(KeyCode.D))
            movement.x += 1;

        movement.Normalize();

        // Get speed from user input
        int speed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : MoveSpeed;

        // Rotate movement vector by the direction the player is looking
        movement = Quaternion.Euler(0, transform.localEulerAngles.y, 0) * movement;

        // Stretch movement by speed
        movement *= speed;

        // Move player
        Controller.Move(delta * movement);

        // Set if player is moving
        Moving = movement != Vector3.zero;
    }

    void TestForInteractions() {
        // Sends out a ray to see what the player is looking at
        if (Physics.Raycast(Head.transform.position, Head.transform.forward, out RaycastHit hit, Mathf.Infinity, ~LayerMask.GetMask("Player"))) {
            // Gets the type of object the ray just hit
            string hitLayer = LayerMask.LayerToName(hit.transform.gameObject.layer);
            // Checks the type
            switch (hitLayer) {
                // These are able to be interacted with by the player when clicked
                case "Interactable": {
                    Pointer.color = Color.red;
                    Interact(hit.transform.gameObject);
                    break;
                }
                // This is the sheriff
                case "Sheriff": {
                    Pointer.color = Color.yellow;
                    break;
                }
                // This is everything else
                default: {
                    Pointer.color = Color.white;
                    break;
                }
            }
        } else { // Gets here if the player is looking at the skybox
            Pointer.color = Color.blue;
        }
    }

    // Does the interaction logic
    void Interact(GameObject hitObject) {
        // Checks if clicked
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            // Gets the interactable object from the raycasted object
            GameObject interactable = hitObject;
            while (interactable.tag.Equals("Untagged"))
                interactable = interactable.transform.parent.gameObject;

            // Gets the type of interactable object
            string tag = interactable.tag;
            // Checks the type
            switch (tag) {
                case "tutorialLock": {
                    // Checks if the key is held
                    if (Key.activeSelf) {
                        Destroy(interactable.transform.parent.gameObject);
                        Key.SetActive(false);
                    }
                    break;
                }
                case "key":
                case "tutorialKey": {
                    // Hold key
                    Key.SetActive(true);
                    Destroy(interactable);
                    break;
                }
                case "drawer": {
                    ChestOfDrawers cod = interactable.GetComponentInParent<ChestOfDrawers>();
                    // Checks if the drawer is locked
                    if (!interactable.name.EndsWith("Locked"))
                        cod.ToggleDrawer(interactable);
                    break;
                }
                case "drawerLock": {
                    ChestOfDrawers cod = interactable.GetComponentInParent<ChestOfDrawers>();
                    // Checks if blue key is held
                    if (BlueKey.activeSelf) {
                        cod.UnlockDrawer(interactable);
                        BlueKey.SetActive(false);
                    }
                    break;
                }
                case "pillow": {
                    Bed bed = interactable.GetComponentInParent<Bed>();
                    bed.MovePillow();
                    break;
                }
            }
        }
    }

    // Resets the player position and rotation to original
    public void Reset() {
        transform.localPosition = DefaultPos;
        transform.eulerAngles = DefaultRot;
        Head.transform.eulerAngles = Vector3.zero;
    }

    // Sees if the player wants to exit
    void TryExit() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
