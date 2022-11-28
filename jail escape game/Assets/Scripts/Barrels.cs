using UnityEngine;

//Team Members: Corey Clayborn, Xander Covalan, Ben Johnson

public class Barrels : MonoBehaviour {
    // The key that is under one of the barrels
    private GameObject Key { get; set; }

    // Finds all the necessary children and components for the Barrels to work
    private void Start() {
        Key = transform.Find("Key").gameObject;
    }

    // Destroys the barrel, tests if the key is in it, returns true and makes the key visible if it is
    public bool DestroyBarrelAndTryDropKey(GameObject barrel) {
        Destroy(barrel);
        if (barrel.name.EndsWith("With Key")) {
            Key.SetActive(true);
            return true;
        }
        return false;
    }
}
