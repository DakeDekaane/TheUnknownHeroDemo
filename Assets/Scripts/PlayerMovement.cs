using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    public float distance = 50;
    Ray viewRay;
    RaycastHit viewHit;

    void Start() {
        Init();
    }

    // Update is called once per frame
    void Update() {

        Debug.DrawRay(transform.position,transform.forward);

        if(!turn) {
            return;
        }

        if(Input.GetMouseButtonDown(0)) {
            viewRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(viewRay, out viewHit)) {
                if (characterState == CharacterState.Idle && viewHit.transform.tag == "Player") {
                    Debug.Log("Player Selected");
                    characterState = CharacterState.StandbyPhase1;
                    FindSelectableTiles();
                }
            } 
        }
        if(Input.GetMouseButtonDown(1)) {
            viewRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(viewRay, out viewHit)) {
                if (characterState == CharacterState.StandbyPhase1 && viewHit.transform.tag == "Tile") {
                    Debug.Log("Destiny Tile Selected");
                    tmpTile = viewHit.transform.GetComponent<Tile>();
                    if(tmpTile.selectable) {
                        MoveToTile(tmpTile);
                        characterState = CharacterState.Move;
                    }
                    
                }
                else if (characterState == CharacterState.StandbyPhase1 && viewHit.transform.tag == "Enemy") {
                    Debug.Log("Enemy Selected");
                    characterState = CharacterState.Idle;
                }
            } 
        }
        if(characterState == CharacterState.Move) {
            Move();
        }
    }
}
