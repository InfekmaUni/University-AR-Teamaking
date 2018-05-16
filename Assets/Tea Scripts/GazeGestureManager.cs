using UnityEngine;
using UnityEngine.XR.WSA.Input;
using UnityEngine.UI;

public class GazeGestureManager : MonoBehaviour
{

    [SerializeField] Text uiText;
    public static GazeGestureManager Instance { get; private set; }

    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject { get; private set; }

    GestureRecognizer recognizer;

    public Vector3 PreviousPosition { get; private set; }
    public Vector3 PreviousForward { get; private set; }

    // Use this for initialization
    void Start()
    {
        Instance = this;

        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
        recognizer.Tapped += (args) =>
        {
            // Send an OnSelect message to the focused object and its ancestors.
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
            }
        };
        recognizer.StartCapturingGestures();
    }

    // Update is called once per frame
    void Update()
    {
        // Figure out which hologram is focused this frame.
        GameObject oldFocusObject = FocusedObject;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.collider.gameObject;

            if(hitInfo.collider.gameObject.transform.parent != null && hitInfo.collider.gameObject.GetComponentInParent<TeaInteractible>() != null)
            {
                uiText.enabled = true;
                uiText.text = hitInfo.collider.gameObject.transform.parent.name;

                Vector3 pos = hitInfo.collider.gameObject.transform.parent.Find("TextPoint").transform.position;
                uiText.transform.position = pos;

                if (FocusedObject != null) // if there is a valid object
                {
                    FocusedObject.SendMessageUpwards("OnFocusEnter", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
        else
        {
            if(FocusedObject != null) // if there was a valid previous focused object
            {
                FocusedObject.SendMessageUpwards("OnFocusExit", SendMessageOptions.DontRequireReceiver);
            }
            // If the raycast did not hit a hologram, clear the focused object.

            FocusedObject = null;
            uiText.enabled = false;
        }

        // If the focused object changed this frame,
        // start detecting fresh gestures again.
        if (FocusedObject != oldFocusObject)
        {
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }

        PreviousPosition = headPosition;
        PreviousForward = gazeDirection;

        if( Input.GetKeyDown(KeyCode.Space) && FocusedObject != null)
        {
            FocusedObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
        }
    }
}