using UnityEngine;

// The controller for all game logic
public class Game : MonoBehaviour {
    // The instance that other scripts can access
    public static Game Instance { get; private set; }

    // Start is called before the first frame update
    void Start() {
        // Set instance
        Instance = this;

        // Hide mouse and lock it
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        TryExit();
    }

    // Allow exit
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
