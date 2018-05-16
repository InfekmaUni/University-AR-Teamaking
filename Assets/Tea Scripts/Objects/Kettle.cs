using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Kettle : MonoBehaviour {

    [SerializeField] public bool hasWater = false;
    [SerializeField] public bool isTurnedOn = false;
    [SerializeField] public bool hasHotWater = false;
    public Stopwatch stopWatch;

    [SerializeField] public ParticleSystem heatVapour;

    public int kettleTimer = 10;
	// Use this for initialization
	void Start () {
        stopWatch = new Stopwatch();
	}
	
	// Update is called once per frame
	void Update () {
		if(stopWatch.Elapsed.Seconds > kettleTimer)
        {
            hasHotWater = true;
            hasWater = false;
            isTurnedOn = false;
            stopWatch.Stop();
            heatVapour.gameObject.SetActive(false);
        }
    }

    public void PourKettle()
    {
        ResetKettle();
    }

    public void ResetKettle()
    {
        hasWater = false;
        hasHotWater = false;
        isTurnedOn = false;
    }

    public void TurnKettleOn()
    {
        if (!isTurnedOn)
        {
            stopWatch.Start();
            heatVapour.gameObject.SetActive(true);
            isTurnedOn = true;
        }
    }
}
