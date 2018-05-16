using UnityEngine;

public class TeaInteractible : MonoBehaviour
{

    [SerializeField] TeaMaking teamaking;
    [SerializeField] GameObject selection;

    void OnSelect()
    {
        teamaking.ObjectSelected(this.gameObject);
    }

    void OnFocusEnter()
    {
        selection.SetActive(true);

        Vector3 pos = this.transform.Find("TextPoint").transform.position;
        selection.transform.position = pos;
    }

    void OnFocusExit()
    {
        selection.SetActive(false);
    }
}