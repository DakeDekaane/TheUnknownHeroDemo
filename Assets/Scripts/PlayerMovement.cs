﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    public float distance = 50;
    Ray viewRay;
    RaycastHit viewHit;

    public ActionUIManager actionUIManager;


    void Start() {
        Init();
        characterAnimator.SetBool("HasGun",true);
        characterAnimator.SetBool("HasKnife",false);
    }

    // Update is called once per frame
    void Update() {

        Debug.DrawRay(transform.position,transform.forward);
        Debug.DrawRay(transform.position + new Vector3(0.0f,0.5f,0.0f),-transform.up);

        CheckDead();

        if(!turn) { 
            return;
        }
        if(!selected) {
            return;
        }

        if( characterState == CharacterState.Idle) {
            FindAttackableTiles();
            
        }
        if( characterState == CharacterState.StandbyPhase1 ) {
            selected = true;
            ClearAttackableTiles();
            GetCurrentTile();
            FindSelectableTiles();
            if(Input.GetMouseButtonDown(0)) {
                if (selectableTiles.Count > 0){
                    TurnManager.instance.CollidersEnabled(false);
                    Move();
                }
            }
        }     
        else if (characterState == CharacterState.Move) {
            if (characterAgent.remainingDistance <= 0.37f && characterAgent.hasPath) {
                StopMove();
                //TurnManager.EndTurn();
            }
            if (tmpTile.target && tmpTile.current) {
                ClearSelectableTiles();
                characterState = CharacterState.StandbyPhase2;
            }
        }
        else if (characterState == CharacterState.StandbyPhase2) {
            FindAttackableTiles();
            if(Input.GetMouseButton(0)) {
                TurnManager.instance.CollidersEnabled(true);
                if(attackableTiles.Count > 0) {
                    Attack();
                }
            }
        }
        else if (characterState == CharacterState.End){
            ClearAttackableTiles();
            actionUIManager.activePlayer = null; 
            TurnManager.instance.activePlayer = null;
            selected = false;
            characterState = CharacterState.Idle;
            TurnManager.instance.EndTurn(this);
        }
    }
    

    void Move() {
        viewRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(viewRay, out viewHit)) {
            if (viewHit.transform.tag == "Tile") {
                Debug.Log("Destiny Tile Selected");
                tmpTile = viewHit.transform.GetComponent<Tile>();
                if(tmpTile.selectable) {
                    tmpTile.target = true;
                     if(!(tmpTile.target && tmpTile.current)) {
                        targetTransform = tmpTile.transform;
                        targetTransform.position += new Vector3(0.0f,0.5f,0.0f);
                        Debug.Log("Target: " + targetTransform.position);
                        characterAgent.SetDestination(targetTransform.position);
                        characterAgent.isStopped = false;
                        characterAnimator.SetBool("Move",true);
                    }
                    //MoveToTile(tmpTile);
                    characterState = CharacterState.Move;
                    
                }
            }
        } 
    }

    void StopMove() {
        characterAgent.isStopped = true;
        characterAnimator.SetBool("Move", false);
        ClearSelectableTiles();
        characterState = CharacterState.StandbyPhase2;
    }



    void Attack(){
        viewRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(viewRay, out viewHit)) {
            if (viewHit.transform.tag == "Enemy") {
                characterState = CharacterState.Attack;
                Debug.Log("Pew Pew");
                characterAnimator.SetTrigger("Attack");
                target = viewHit.transform.gameObject;
                transform.forward = target.transform.position - transform.position;
                //FindSelectableTiles();
            }
        }
    }
}
