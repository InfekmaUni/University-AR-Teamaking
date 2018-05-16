using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour {

    [SerializeField] public bool hasTeaBag = false;
    [SerializeField] public bool hasSugar = false;
    [SerializeField] public bool hasMilk = false;
    [SerializeField] public bool hasHotWater = false;
    [SerializeField] public bool isPrepared = false;
    [SerializeField] public bool isMixed = false;
    [SerializeField] public bool isDone = false;
    [SerializeField] public GameObject teaMesh;
    [SerializeField] public GameObject waterMesh;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetCup()
    {
        hasTeaBag = false;
        hasSugar = false;
        hasMilk = false;
        hasHotWater = false;
        isPrepared = false;
        isMixed = false;
        isDone = false;
        teaMesh.SetActive(false);
        waterMesh.SetActive(false);
    }

    public void CheckStepID(int id)
    {
        if(id == 2)
        {
            hasTeaBag = true;
        }else if(id == 3)
        {
            hasSugar = true;
        }else if (id == 4)
        {
            hasMilk = true;
        }else if (id == 5)
        {
            hasHotWater = true;
        }else if(id == 6)
        {
            hasTeaBag = false;
        }else if(id == 7)
        {
            if (isPrepared)
                isMixed = true;
        }
        else
        {
            Debug.Log("[Cup Script] Invalid step id given");
        }

        if(hasTeaBag && hasSugar && hasMilk && hasHotWater)
        {
            isPrepared = true;
            waterMesh.SetActive(true);
        }
        if (isMixed)
            isDone = true;

        if(isDone)
        {
            teaMesh.SetActive(true);
            waterMesh.SetActive(false);
        }
    }

    ///
    /// Deprecated
    /// 

    //bool CheckIfValidObject(GameObject obj)
    //{
    //    switch(obj.name)
    //    {
    //        case "Spoon":
    //        case "Milk":
    //        case "Sugar":
    //        case "Teabag":
    //        case "Kettle": // hot water
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //void OnTriggerEnter(Collider collider)
    //{
    //    GameObject other = collider.gameObject;

    //    bool isValidObject = CheckIfValidObject(other);
    //    if(isValidObject)
    //    {
    //        Debug.Log("Object inside trigger: "+isValidObject+" name: "+other.name);
            
    //        switch(other.name)
    //        {
    //            case "Sugar":
    //                hasSugar = true;

    //                other.transform.parent.GetComponent<Rigidbody>().useGravity = true;
    //                other.transform.parent.GetComponent<Rigidbody>().isKinematic = false;
    //                other.transform.parent.GetComponent<BaseGestureAction>().enabled = false;

    //                break;
    //            case "Teabag":
    //                hasTeaBag = true;

    //                other.transform.parent.GetComponent<Rigidbody>().useGravity = true;
    //                other.transform.parent.GetComponent<Rigidbody>().isKinematic = false;

    //                //if(hasTeaBag && hasHotWater)
    //                //{
    //                //    hasTeaBag = false;
    //                //}

    //                break;
    //            case "Milk":
    //                hasMilk = true;
    //            break;
    //            case "HotWater":
    //                hasHotWater = true;
    //             break;
    //            case "Spoon":
    //                isMixed = true;
    //            break;
    //        }
    //    }

    //void OnTriggerExit(Collider collider)
    //{
    //    GameObject other = collider.gameObject;

    //    switch (other.name)
    //    {
    //        case "Sugar":
    //            break;
    //        case "Teabag":
    //            hasTeaBag = false;

    //            //other.transform.parent.GetComponent<Rigidbody>().useGravity = false;
    //            //other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;

    //            //if(hasTeaBag && hasHotWater)
    //            //{
    //            //    hasTeaBag = false;
    //            //}

    //            break;
    //        case "Milk":
    //            hasMilk = true;
    //            break;
    //        case "HotWater":
    //            hasHotWater = true;
    //            break;
    //        case "Spoon":
    //            isMixed = true;
    //            break;
    //    }
    //}
}
