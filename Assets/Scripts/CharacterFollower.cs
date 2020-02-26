using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFollower : MonoBehaviour
{
    public Transform cam;
    public Transform follow;
    public Vector3 start;
    // Update is called once per frame

    void Start() {
        start = cam.transform.position;
        Debug.Log(start);
    }
     void Update()
    {
       if(TurnManager.instance.turnTeam == TurnManager.Faction.Enemy || TurnManager.instance.turnTeam == TurnManager.Faction.Ally) {
           follow = TurnManager.instance.activeCharacter.transform;
           cam.position = follow.position + start;
       } 
    }
}
