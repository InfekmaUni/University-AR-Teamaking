using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;

public class TeaMaking : MonoBehaviour, ISpeechHandler {

    /*
        Pickup Water Bottle and Pour water into kettle
        turn on kettle
        put teabag in the cup
        put sugar in the cup
        put milk in the cup
        fill the cup with hot water
        remove tea bag from cup
        mix tea with spoon
     */

    struct OriginalTransform
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;


       public OriginalTransform(Transform trans)
        {
            position = trans.position;
            rotation = trans.rotation;
            scale = trans.localScale;
        }

        public void Copy(ref Transform trans)
        {
            trans.position = position;
            trans.rotation = rotation;
            trans.localScale = scale;
        }
    }

    public enum TeaActionEnum
    {
        KETTLE_ADD_WATER = 0,
        KETTLE_ON = 1,
        ADD_TEABAG = 2,
        ADD_SUGAR = 3,
        ADD_MILK = 4,
        ADD_HOT_WATER = 5,
        REMOVE_TEABAG = 6,
        MIX = 7,
        DONE = 8
    }
    public TeaActionEnum TeaAction { get; private set; }

    [SerializeField] GameObject teaset;
    [SerializeField] GameObject waterBottle;
    [SerializeField] GameObject kettle;
    [SerializeField] GameObject teabag;
    [SerializeField] GameObject sugar;
    [SerializeField] GameObject milk;
    [SerializeField] GameObject spoon;
    [SerializeField] GameObject cup;
    [SerializeField] GameObject checklist;

    [SerializeField] Text avatarInform;

    private Cup cupScript;
    private Kettle kettleScript;

    Vector3 sugarCubePosition;
    Quaternion sugarCubeRotation;

    OriginalTransform origSugarCube;
    Transform origSugarCubeTransform;
    OriginalTransform origTeabag;
    Transform origTeabagTransform;

    // Use this for initialization
    void Start () {
        TeaAction = TeaActionEnum.KETTLE_ADD_WATER;

        if (cup != null)
            cupScript = cup.GetComponent<Cup>();

        if (kettle != null)
            kettleScript = kettle.GetComponent<Kettle>();

        origSugarCube = new OriginalTransform(sugar.transform);
        origTeabag = new OriginalTransform(teabag.transform);

        origSugarCubeTransform = sugar.GetComponent<Transform>();
        origTeabagTransform = teabag.GetComponent<Transform>();

        AvatarInform("Tap the water bottle");
    }

    public void IncrementTeaMakingStep()
    {
        TeaAction++;
    }

    public void DecrementTeaMakingStep()
    {
        TeaAction--;
    }

    public void ResetTeaMaking()
    {
        TeaAction = 0;
        cupScript.ResetCup();
        kettleScript.ResetKettle();
        teabag.SetActive(true);
        origTeabag.Copy(ref origTeabagTransform);
        origSugarCube.Copy(ref origSugarCubeTransform);

        Debug.Log("Reset");
        ResetCheckList();
    }

    private void TickCheckBox(int id)
    {
        if (!checklist.activeSelf)
        {
            return;
        }
        // get list (contains all steps for the checklist)
        GameObject list = checklist.transform.Find("List").gameObject;

        // get step
        GameObject stepObject = list.transform.GetChild(id).gameObject;
        
        // get status
        GameObject status = stepObject.transform.GetChild(0).gameObject;

        // enable check
        GameObject finished = status.transform.GetChild(1).gameObject;
        finished.SetActive(true);
        
        //disable box
        GameObject notFinished = status.transform.GetChild(0).gameObject;
        notFinished.SetActive(false);
    }

    private void ResetCheckList()
    {
        if (!checklist.activeSelf)
        {
            return;
        }

        // get list (contains all steps for the checklist)
        GameObject list = checklist.transform.Find("List").gameObject;

        foreach (Transform step in list.transform)
        {
            // get status
            GameObject status = step.GetChild(0).gameObject;

            // enable check
            GameObject finished = status.transform.GetChild(1).gameObject;
            finished.SetActive(false);

            //disable box
            GameObject notFinished = status.transform.GetChild(0).gameObject;
            notFinished.SetActive(true);
        }
    }

    public void ObjectSelected(GameObject obj)
    {
        string objName = obj.name;
        if(objName == "Spoon")
        {
            MixTeaCup();
        }
        else if(objName == "Cup")
        {
            RemoveTeabagFromCup();
        }
        else if(objName == "Kettle")
        {
            AddHotWaterToCup();
            TurnKettleOn();
        }
        else if(objName == "Milk")
        {
            AddMilkToCup();
        }
        else if(objName == "Sugar")
        {
            AddSugarToCup();
        }
        else if(objName == "Tray")
        {
        }
        else if(objName == "Water Bottle")
        {
            AddWaterToKettle();
        }
        else if(objName == "Teabag")
        {
            AddTeabagToCup();
            RemoveTeabagFromCup();
        }
    }

    public void TeaIsDone()
    {
        // what happens when tea has been completed
        AvatarInform("Well done you finished making the cup of tea!, Say Reset to make another cup!");
    }

    public void AvatarInform(string inform)
    {
        avatarInform.text = inform;
    }

    private void AddWaterToKettle()
    {
        waterBottle.gameObject.GetComponent<Animation>().Play();
        Debug.Log("Water is filling kettle");
        TickCheckBox(0);
        kettleScript.hasWater = true;
        AvatarInform("Tap the kettle to turn it on");
    }

    private void TurnKettleOn()
    {
        // restrict turning kettle on without water
        if (!kettleScript.hasWater)
        {
            // kettle does not have water
            return;
        }
        else if (kettleScript.hasHotWater)
        {
            // water is ready -> inform user
            return;
        }

        TickCheckBox(1);
        kettleScript.TurnKettleOn();
        AvatarInform("Tap the teabag");
    }

    private void AddTeabagToCup()
    {
        if (!cupScript.hasTeaBag)
        {
            TickCheckBox(2);
            cupScript.CheckStepID(2);
            teabag.GetComponent<Animation>().Play();
            AvatarInform("Tap the sugar cube");
        }
    }

    private void AddSugarToCup()
    {
        if (!cupScript.hasSugar)
        {
            sugar.GetComponent<Animation>().Play();
            TickCheckBox(3);
            cupScript.CheckStepID(3);
            AvatarInform("Tap the milk carton");
        }
    }

    private void AddMilkToCup()
    {
        if (!cupScript.hasMilk)
        {
            milk.GetComponent<Animation>().Play();
            TickCheckBox(4);
            cupScript.CheckStepID(4);
            AvatarInform("Tap the kettle again");
        }
    }

    private void AddHotWaterToCup()
    {
        if (!cupScript.hasHotWater && kettleScript.hasHotWater)
        {
            TickCheckBox(5);
            cupScript.CheckStepID(5);
            kettle.GetComponent<Animation>().Play();
            kettleScript.PourKettle();
            AvatarInform("Tap the cup to remove the teabag");
        }
    }

    private void RemoveTeabagFromCup()
    {
        if (cupScript.hasTeaBag && cupScript.isPrepared)
        {
            TickCheckBox(6);
            cupScript.CheckStepID(6);
            teabag.SetActive(false);
            AvatarInform("Tap the spoon to finish the tea");
        }
    }

    private void MixTeaCup()
    {
        if(cupScript.isPrepared && !cupScript.hasTeaBag)
        {
            cupScript.CheckStepID(7);
            TickCheckBox(7);
            spoon.GetComponent<Animation>().Play();
            AvatarInform("Well done, you finished making a cup of tea");
        }
    }

    void ISpeechHandler.OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        if (eventData.RecognizedText.Equals("Add Water"))
        {
            AddWaterToKettle();
        }
        else if (eventData.RecognizedText.Equals("Kettle On"))
        {
            TurnKettleOn();
        }
        else if (eventData.RecognizedText.Equals("Add Teabag"))
        {
            AddTeabagToCup();
        }
        else if (eventData.RecognizedText.Equals("Add Sugar"))
        {
            AddSugarToCup();
        }
        else if (eventData.RecognizedText.Equals("Add Milk"))
        {
            AddMilkToCup();
        }
        else if (eventData.RecognizedText.Equals("Add Hot Water"))
        {
            AddHotWaterToCup();
        }
        else if (eventData.RecognizedText.Equals("Remove Teabag"))
        {
            RemoveTeabagFromCup();
        }
        else if (eventData.RecognizedText.Equals("Mix"))
        {
            MixTeaCup();
        }
        //else if (eventData.RecognizedText.Equals("Expand"))
        //{
        //    teaset.GetComponent<BoxCollider>().enabled = false;
        //}
        //else if (eventData.RecognizedText.Equals("Collapse"))
        //{
        //    teaset.GetComponent<BoxCollider>().enabled = true;
        //}
        else if (eventData.RecognizedText.Equals("Reset"))
        {
            ResetTeaMaking();
        }
        else
        {
            return;
        }
        
        if(cupScript.isDone)
        {
            TeaIsDone();
        }

        eventData.Use();
    }
}
