using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerMovement : CharacterMovement
{
    public enum PlayerState{
        Idle,
        StandbyPhase1,
        StandbyPhase2,
        End
    }

    public PlayerState playerState = PlayerState.Idle;

    public float distance;
    Ray viewRay;
    RaycastHit viewHit;

    public LayerMask layerMask;

    bool m_Started;

    int movementCost = 5;
    int attackRange = 2;
    void Start() {
        m_Started = true;
    }

    void FixedUpdate(){
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        
    }

    void OnDrawGizmos(){
         Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            viewRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(viewRay, out viewHit, distance)) {
                if (playerState == PlayerState.Idle && viewHit.transform.tag == "Player") {
                    Debug.Log("Player Selected");
                    playerState = PlayerState.StandbyPhase1;
                    Collider[] hitColliders = Physics.OverlapBox(viewHit.transform.position, transform.localScale * 0.5f);
                    foreach(Collider c in hitColliders) {
                        if(c.transform.tag == "Tile"){
                            c.GetComponent<Tile>().current = true;
                            c.GetComponent<Tile>().GetMovementTiles(movementCost,attackRange);
                            c.GetComponent<Tile>().target = false;
                        }
                    }
                }
            } 
        }
        if(Input.GetMouseButtonDown(1)) {
            viewRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(viewRay, out viewHit, distance)) {
                if (playerState == PlayerState.StandbyPhase1 && viewHit.transform.tag == "Tile") {
                    Debug.Log("Destiny Tile Selected");
                    playerState = PlayerState.Idle;
                }
                else if (playerState == PlayerState.StandbyPhase1 && viewHit.transform.tag == "Enemy") {
                    Debug.Log("Enemy Selected");
                    playerState = PlayerState.Idle;
                }
            } 
        }
    }

    

    // void OnCollisionEnter(Collision c) {
    //     if (c.collider.transform.tag == "Tile") {
    //          Debug.Log("Entering Tile");
    //          c.transform.GetComponent<Tile>().current = true;
    //      }
    // }

    // void OnCollisionExit(Collision c) {
    //     if (c.collider.transform.tag == "Tile") {
    //         Debug.Log("Leaving Tile");
    //         c.transform.GetComponent<Tile>().current = false;
    //     }
    // }
}
