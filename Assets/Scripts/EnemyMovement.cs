using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : CharacterMovement
{
    public enum EnemyBehaviour {
        Nearest,
        HighestDamage,
        //KillFirst
    }
    public EnemyBehaviour behaviour;

    private float distance;
    private float damage;
    // Start is called before the first frame update
    void Start()
    { 
        Init();
        characterAnimator.SetBool("HasGun",true);
        characterAnimator.SetBool("HasKnife",false);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position,transform.forward);
        Debug.DrawRay(transform.position + new Vector3(0.0f,0.5f,0.0f),-transform.up);

        CheckDead();

        if(TurnManager.instance.holdOn || TurnManager.instance.pause) {
            return;
        }

        if(!turn) {
            return;
        }

        if(characterState == CharacterState.Begin) {
            FindAttackableTiles(this.gameObject.transform.tag);
            FindNearestTarget();
            if (attackableTiles.Count > 0) {
                foreach(Tile tile in attackableTiles) {
                    Debug.Log("Attackables:" + tile.name);
                }
                Debug.Log(name + ": Begin->Attack");
                characterState = CharacterState.Attack;
                return;
            }
            CalculatePath();
            FindSelectableTiles();
            actualTargetTile.target = true;
            if(!(actualTargetTile.target && actualTargetTile.current)) {
                Debug.Log(name + ": Begin->Move");
                characterState = CharacterState.Move;
                TurnManager.instance.CollidersEnabled(false);
                targetTransform = actualTargetTile.transform;
                targetTransform.position += new Vector3(0.0f,0.5f,0.0f);
                Debug.Log("Target: " + targetTransform.position);
                characterAgent.SetDestination(targetTransform.position);
                GetComponentInChildren<ParticleSystem>().Play();
                characterAgent.isStopped = false;
                characterAnimator.SetBool("Move",true);
            }
            else {
                Debug.Log(name + ": Begin->Attack");
                characterState = CharacterState.Attack;
            }
        }
        
        else if(characterState == CharacterState.Move) {
            //Debug.Log("Distance to target: "+ characterAgent.remainingDistance);
            if (characterAgent.remainingDistance <= 0.37f && characterAgent.hasPath) {
                TurnManager.instance.CollidersEnabled(true);
                characterAgent.isStopped = true;
                characterAnimator.SetBool("Move", false);
                ClearSelectableTiles();
                Debug.Log(name + ": Move->Attack");
                characterState = CharacterState.Attack;
                GetComponentInChildren<ParticleSystem>().Stop();
                
            } 
            if (actualTargetTile.target && actualTargetTile.current) {
                ClearSelectableTiles();
                characterState = CharacterState.Attack;
                GetComponentInChildren<ParticleSystem>().Stop();
            }
            //Move();
        }
        else if(characterState == CharacterState.Attack) {
            FindAttackableTiles(this.gameObject.transform.tag);
            if(attackableTiles.Count > 0) {
                //Debug.Log("Pew Pew del enemigo");
                characterAnimator.SetTrigger("Attack");
                audioSource.clip = SFX[0];
                //audioSource.time = 0.5f;
                audioSource.loop = false;
                //audioSource.Play();
                audioSource.PlayDelayed(1.2f);
                transform.forward = target.transform.position - transform.position;
            }
            else {
                Debug.Log(name + ": Attack->End (Did not attack)");
                characterState = CharacterState.End;
            }
        }
        else if(characterState == CharacterState.End) {
            ClearAttackableTiles();
            Debug.Log(name + ": End->Begin");
            characterState = CharacterState.Begin;
            TurnManager.instance.EndTurn();
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
        GameObject highest = null;

        if(behaviour == EnemyBehaviour.Nearest) {
            distance = Mathf.Infinity;
            foreach(GameObject obj in targets) {
                float d = Vector3.Distance(transform.position,obj.transform.position);
                if(d < distance) {
                    distance = d;
                    nearest = obj;
                }
            }
            target = nearest;
            //target.transform.GetComponentInChildren<Renderer>().material.color = Color.black;
        }
        if(behaviour == EnemyBehaviour.HighestDamage) {
            damage = -Mathf.Infinity;
            foreach(GameObject obj in targets) {
                float h = GetComponent<CharacterStats>().currentAtk - obj.GetComponent<CharacterStats>().currentDef;
                if(h > damage) {
                    damage = h;
                    highest = obj;
                }
            }
            target = highest;
            //target.transform.GetComponentInChildren<Renderer>().material.color = Color.white;
        }
    }
}
