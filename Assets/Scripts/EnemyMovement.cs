using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : CharacterMovement
{
    // Start is called before the first frame update
    void Start()
    { 
        Init();
        characterAnimator.SetBool("HasGun",true);
        characterAnimator.SetBool("HasKnife",false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position,transform.forward);
        Debug.DrawRay(transform.position + new Vector3(0.0f,0.5f,0.0f),-transform.up);

        CheckDead();

        if(!turn) {
            return;
        }

        
        if(characterState == CharacterState.Idle) {
            FindNearestTarget();
            CalculatePath();
            FindSelectableTiles();
            actualTargetTile.target = true;
            if(!(actualTargetTile.target && actualTargetTile.current)) {
                targetTransform = actualTargetTile.transform;
                targetTransform.position += new Vector3(0.0f,0.5f,0.0f);
                Debug.Log("Target: " + targetTransform.position);
                characterAgent.SetDestination(targetTransform.position);
                characterAgent.isStopped = false;
                characterAnimator.SetBool("Move",true);
            }

            

            //target.transform.GetComponent<Tile>().target = true;
            // characterState = CharacterState.Move;



            //MoveToTile(tmpTile);
            //characterState = CharacterState.Move;
            //tmpTile.target = true;
        }
        if(characterState == CharacterState.Move) {
            //Debug.Log("Distance to target: "+ characterAgent.remainingDistance);
             if (characterAgent.remainingDistance <= 0.37f && characterAgent.hasPath) {
                characterAgent.isStopped = true;
                characterAnimator.SetBool("Move", false);
                ClearSelectableTiles();
                characterState = CharacterState.Idle;
                TurnManager.EndTurn();
            } 
            if (actualTargetTile.target && actualTargetTile.current) {
                ClearSelectableTiles();
                characterState = CharacterState.Idle;
                TurnManager.EndTurn();
            }
            //Move();
        }
    }

    void CalculatePath() {
        Tile targetTile = GetTargetTile(target);
        //targetTile.transform.GetComponent<Renderer>().material.color = Color.green;
        FindPath(targetTile);
    }

    void FindNearestTarget(){
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearest = null;

        float distance = Mathf.Infinity;

        foreach(GameObject obj in targets) {
            float d = Vector3.Distance(transform.position,obj.transform.position);
            if(d < distance) {
                distance = d;
                nearest = obj;
            }
        }
        target = nearest;
        target.transform.GetComponentInChildren<Renderer>().material.color = Color.green;
    }
}
