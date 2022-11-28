using UnityEngine;

public class Bed : MonoBehaviour {
    // The interactable pillow
    private GameObject Pillow { get; set; }
    // The pillow but moved and not interactable
    private GameObject PillowMoved { get; set; }

    // Start is called before the first frame update
    void Start() {
        Pillow = transform.Find("Pillow").gameObject;
        PillowMoved = transform.Find("PillowMoved").gameObject;
    }

    // Moves the pillow to expose a key
    public void MovePillow() {
        Pillow.SetActive(false);
        PillowMoved.SetActive(true);
    }
}
