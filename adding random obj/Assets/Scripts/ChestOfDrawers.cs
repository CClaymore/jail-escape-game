using UnityEngine;

//Team Members: Corey Clayborn, Xander Covalan, Ben Johnson
public class ChestOfDrawers : MonoBehaviour {

    // Toggles the position of the drawer between in and out
    public void ToggleDrawer(GameObject drawer) {
        Vector3 pos = drawer.transform.localPosition;
        if (pos.z == 0)
            pos.z = .8f;
        else
            pos.z = 0;
        drawer.transform.localPosition = pos;
    }

    // Unlocks the drawer given
    public void UnlockDrawer(GameObject drawerLock) {
        string name = drawerLock.name.Replace("Lock ", "");
        GameObject drawer = transform.Find($"Drawer {name} Locked").gameObject;
        if (drawer != null)
            drawer.name = drawer.name.Replace("Locked", "");
        Destroy(drawerLock);
    }
}
