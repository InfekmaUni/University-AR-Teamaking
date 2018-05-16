using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTeaMaking : MonoBehaviour {
	
    public void ResetGame()
    {
        GameObject.Instantiate(Resources.Load("Tea Resources/Prefabs/Tea Set"));
    }
}