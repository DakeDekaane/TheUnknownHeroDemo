using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{

    public Transform[] cam;
    private Vector3 start;
    
    // Update is called once per frame
    void Start() {
        start = cam[0].transform.position;
        Debug.Log(start);
    }
    // Update is called once per frame
     void Update()
    {
       if(TurnManager.instance.turnTeam == TurnManager.Faction.Enemy || TurnManager.instance.turnTeam == TurnManager.Faction.Ally) {
           cam[0].gameObject.SetActive(false);
           cam[1].gameObject.SetActive(true);
           //cam[0].GetComponentInChildren<AudioListener>().enabled = false;
           //cam[1].GetComponent<AudioListener>().enabled = true;
       }
       else {
           cam[0].gameObject.SetActive(true);
           cam[1].gameObject.SetActive(false);
           //cam[0].GetComponentInChildren<AudioListener>().enabled = true;
           //cam[1].GetComponent<AudioListener>().enabled = false;
           

       }
    }
}
