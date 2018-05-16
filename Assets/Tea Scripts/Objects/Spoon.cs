using UnityEngine;

public class Spoon : MonoBehaviour {

    private Vector3 origPos;
    private Quaternion origRot;
    private bool isSelected = false;
    private GazeGestureManager gestureManager;

	// Use this for initialization
	void Start () {
        origPos = this.transform.position;
        origRot = this.transform.rotation;
        gestureManager = GameObject.Find("Game").GetComponent<GazeGestureManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(isSelected)
        {
            Vector3 prevPos = gestureManager.PreviousPosition;
            Vector3 curPos = Camera.main.transform.position;
            Vector3 dif = curPos - prevPos;

            if(Vector4.Distance(curPos, prevPos) > 0) // move object based on gaze update
              GetComponent<Rigidbody>().velocity = dif;
        }
	}

    // selected by gesture
    void OnSelect()
    {
        isSelected = true;
    }

    void OnDeselect()
    {
        isSelected = false;
    }

    void OnReset()
    {
        this.transform.position = origPos;
        this.transform.rotation = origRot;
    }
}
