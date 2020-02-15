using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    public float distance = 50;
    Ray viewRay;
    RaycastHit viewHit;

    public ActionUIManager actionUIManager;

    public HealBox healBox;

    //public Tile currentTile;

    void Start() {
        Init();
        characterAnimator.SetBool("HasGun",true);
        characterAnimator.SetBool("HasKnife",false);
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update() {

        Debug.DrawRay(transform.position,transform.forward);
        Debug.DrawRay(transform.position + new Vector3(0.0f,0.5f,0.0f),-transform.up);

        CheckDead();
        
        if(TurnManager.instance.holdOn || TurnManager.instance.pause) {
            return;
        }

        if(!turn) { 
            return;
        }

        if( characterState == CharacterState.Begin) {
            FindAttackableTiles(this.gameObject.transform.tag);
            CheckForItem();
            Debug.Log(name + ": Begin->Idle");
            characterState = CharacterState.Idle;
        }
        if( characterState == CharacterState.Idle) {
            //Waiting for events
        }
        if(!selected) {
            return;
        }
        if( characterState == CharacterState.PreStandbyPhase1 ) {
            GetCurrentTile();
            ClearAttackableTiles();
            FindSelectableTiles();
            
            selected = true;
            Debug.Log(name + ": PSB1->SB1");
            characterState = CharacterState.StandbyPhase1;
        }
        else if( characterState == CharacterState.StandbyPhase1 ) {
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
                ClearSelectableTiles();
                TurnManager.instance.CollidersEnabled(true);
                GetCurrentTile();
                FindAttackableTiles(this.gameObject.transform.tag);
                CheckForItem();
                Debug.Log(name + ": Move->Idle2");
                characterState = CharacterState.Idle2;
                //TurnManager.EndTurn();
            }
            if (tmpTile.target && tmpTile.current) {
                ClearSelectableTiles();
                TurnManager.instance.CollidersEnabled(true);
                GetCurrentTile();
                FindAttackableTiles(this.gameObject.transform.tag);
                CheckForItem();
                Debug.Log(name + ": Move->Idle2");
                characterState = CharacterState.Idle2;
            }
        }
        else if(characterState == CharacterState.Idle2) {
            //Wait for button input
        }
        else if (characterState == CharacterState.PreStandbyPhase2) {
            Debug.Log(name + ": PSB2->SB2");
            characterState = CharacterState.StandbyPhase2;

        }
        else if (characterState == CharacterState.StandbyPhase2) {
            if(Input.GetMouseButtonDown(0)) {
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
            Debug.Log(name + ": End->Begin");
            characterState = CharacterState.Begin;
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
//                        Debug.Log("Target: " + targetTransform.position);
                        characterAgent.SetDestination(targetTransform.position);
                        GetComponentInChildren<ParticleSystem>().Play();
                        characterAgent.isStopped = false;
                        characterAnimator.SetBool("Move",true);
                    }
                    //MoveToTile(tmpTile);
                    Debug.Log(name + ": SB1->Move");
                    characterState = CharacterState.Move;
                    
                }
            }
        } 
    }

    void StopMove() {
        characterAgent.isStopped = true;
        characterAnimator.SetBool("Move", false);
        GetComponentInChildren<ParticleSystem>().Stop();
    }



    void Attack(){
        viewRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(viewRay, out viewHit)) {
            if (viewHit.transform.tag == "Enemy") {
                Debug.Log(name + ": SBX->Attack");
                characterState = CharacterState.Attack;
                //Debug.Log("Pew Pew");
                characterAnimator.SetTrigger("Attack");
                audioSource.clip = SFX[0];
                audioSource.time = 0.7f;
                audioSource.loop = false;
                audioSource.Play();
                target = viewHit.transform.gameObject;
                transform.forward = target.transform.position - transform.position;
                //FindSelectableTiles();
            }
        }
    }

    void CheckForItem(){
        currentTile = GetCurrentTile();
        currentTile.GetItemTiles();
        foreach(Tile tile in currentTile.itemTiles) {
            healBox = tile.GetItem();
            tile.attackable = true;
        }
    }

    public void UseItem() {
        GetComponent<CharacterStats>().Heal(healBox.healAmount);
        healBox.PlayParticles();
        Destroy(healBox.gameObject,1.0f);
        foreach(Tile tile in currentTile.adjacentTiles) {
            tile.attackable = false;
        }
        healBox = null;
    }
}
