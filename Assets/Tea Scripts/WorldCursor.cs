using UnityEngine;
using UnityEngine.UI;

public class WorldCursor : MonoBehaviour
{
    [SerializeField] MeshRenderer validMeshRenderer;
    [SerializeField] Text uiText;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;

        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram...
            // Display the cursor mesh.
            validMeshRenderer.enabled = true;

            // Move thecursor to the point where the raycast hit.
            this.transform.position = hitInfo.point;

            // Rotate the cursor to hug the surface of the hologram.
            this.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

            uiText.enabled = true;

            // face uitext towards our gaze
            uiText.transform.forward = gazeDirection;
            uiText.text = hitInfo.collider.gameObject.name;

            // position text above object head
            Vector3 heightOffset = new Vector3(0, 1.5f, 0);
            uiText.transform.position = hitInfo.collider.gameObject.transform.position + heightOffset;
        }
        else
        {
            // If the raycast did not hit a hologram, hide the cursor mesh.
            validMeshRenderer.enabled = false;

            // disable text
            uiText.enabled = false;
        }
    }
}