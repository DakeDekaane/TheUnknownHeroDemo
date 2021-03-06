﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterState {
    Begin,
    Idle,
    PreStandbyPhase1,
    StandbyPhase1,
    Idle2,
    PreStandbyPhase2,
    StandbyPhase2,
    Move,
    Attack,
    End
}

public enum SelectStatus {
    Selected,
    Unselected,
    Acted
}

public class CharacterMovement : MonoBehaviour
{

    public GameObject target;
    public bool turn = false;
    public bool selected = false;

    public SelectStatus status = SelectStatus.Unselected;

    public CharacterState characterState = CharacterState.Begin;

    public List<Tile> selectableTiles = new List<Tile>();
    public List<Tile> attackableTiles = new List<Tile>();
    GameObject[] tiles;

    Queue<Tile> process = new Queue<Tile>();
    Stack<Tile> path = new Stack<Tile>();
    
    public Tile currentTile;
    public int movementCost = 4;
    public int attackRange = 1;
    public float moveSpeed = 2;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    RaycastHit hit;

    protected Tile tmpTile = null;
    //Vector3 targetPosition = new Vector3();
    public Tile actualTargetTile;

    protected Animator characterAnimator;
    protected NavMeshAgent characterAgent;
    protected Transform targetTransform;

    public AudioClip[] SFX;
    protected AudioSource audioSource;


    public Material selectedMaterial;
    public Material unselectedMaterial;
    public Material actedMaterial;


    protected void Init() {

        characterAnimator = GetComponent<Animator>();
        characterAgent = GetComponent<NavMeshAgent>();

        tiles = GameObject.FindGameObjectsWithTag("Tile");
    }

    public Tile GetCurrentTile() {
        currentTile = GetTargetTile(this.gameObject);
        //currentTile.current = true;
        return currentTile;
    }

    public void ShowCurrentTile() {
        currentTile.current = true;
    }

    public void ClearCurrentTile(){
        currentTile.current = false;
    }

    public Tile GetTargetTile(GameObject target) {

        tmpTile = null;

        if(Physics.Raycast(target.transform.position + new Vector3(0.0f,1.0f,0.0f), Vector3.down, out hit, 3.0f)){
            //Debug.Log("Current Tile: " + hit.transform.tag);
            tmpTile = hit.transform.GetComponent<Tile>();
        }
        else {
            Debug.Log("No encontré nada xd");
            
        }
        return tmpTile;
    }

    public void ComputeAdjacentTiles(Tile target) {
        foreach(GameObject tile in tiles) {
            tmpTile = tile.GetComponent<Tile>();
            tmpTile.GetAdjacentTiles(target);
        }
    }

    public void ComputeAttackableTiles() {
        foreach(GameObject tile in tiles) {
            tmpTile = tile.GetComponent<Tile>();
            tmpTile.GetAttackableTiles();
        }
    }

    public void GetTerrainBonus(){
        GetComponent<CharacterStats>().currentAvoid = GetCurrentTile().terrainData.bonusAvo;
        GetComponent<CharacterStats>().currentDef = GetComponent<CharacterStats>().baseDef + GetComponent<CharacterMovement>().GetCurrentTile().terrainData.bonusDef;
        Debug.Log("Bonus Avo:  " + GetComponent<CharacterStats>().currentAvoid);
        Debug.Log("Bonus Def:  " + GetComponent<CharacterStats>().currentDef);
    }

    public void FindSelectableTiles(){
        if (selectableTiles.Count > 0) {
            return;
        }
        ComputeAdjacentTiles(null);
        GetCurrentTile();
        ShowCurrentTile();
        process.Enqueue(currentTile);
        currentTile.visited = true;
        //currentTile.parent = null;
        
        while(process.Count > 0) {
            tmpTile = process.Dequeue();
            selectableTiles.Add(tmpTile);
            tmpTile.selectable = true;
            if(tmpTile.distance < movementCost) {
                foreach(Tile tile in tmpTile.adjacentTiles) {
                    if(!tile.visited) {
                        tile.parent = tmpTile;
                        tile.visited = true;
                        tile.distance = tmpTile.distance + 1;
                        process.Enqueue(tile);
                    }
                }
            }          
        }
    }

    public void ShowAttackableTiles() {
        foreach(Tile tile in attackableTiles) {
            tile.attackable = true;
        }
    }

    public void FindAttackableTiles(string player){
        string opponent = "";
        if (attackableTiles.Count > 0) {
            return;
        }
        ComputeAttackableTiles();
        currentTile = GetCurrentTile();
        ShowCurrentTile();
        process.Enqueue(currentTile);
        currentTile.visited = true;
        //currentTile.parent = null;
        
        while(process.Count > 0) {
            tmpTile = process.Dequeue();
            attackableTiles.Add(tmpTile);
            //tmpTile.attackable = true;
            if(tmpTile.distance < attackRange) {
                foreach(Tile tile in tmpTile.attackableTiles) {
                    if(!tile.visited) {
                        tile.parent = tmpTile;
                        tile.visited = true;
                        tile.distance = tmpTile.distance + 1;
                        process.Enqueue(tile);
                    }
                }
            }          
        }
        if(player == "Player") {
            opponent = "Enemy";
        }
        else if (player == "Enemy") {
            opponent = "Player";
        }
        for(int i = attackableTiles.Count - 1; i >= 0 ;i--) {
            if(attackableTiles[i].GetObject() != opponent) {
//                Debug.Log("Not " + opponent + " in " + attackableTiles[i].name);
                //attackableTiles[i].attackable = false;
                attackableTiles.RemoveAt(i);
            }
        }
        attackableTiles.Remove(GetCurrentTile());
    }

    public void MoveToTile(Tile tile) {
        path.Clear();
        tile.target = true;
        Tile next = tile;
        while(next != null) {
            path.Push(next);
            next = next.parent;
        }
    }

    /*public void Move(){
        if (path.Count > 0) {
            tmpTile = path.Peek();
            targetPosition = tmpTile.transform.position;
            targetPosition.y += transform.position.y;

            if(Vector3.Distance(transform.position,targetPosition) >= 0.05f ) {
                CalculateHeading(targetPosition);
                SetHorizontalVelocity();
                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;

            }
            else {
                transform.position = targetPosition;
                path.Pop();
            }
        }
        else {
            ClearSelectableTiles();
            characterState = CharacterState.Idle;
            TurnManager.EndTurn();
        }
}*/

    protected void ClearSelectableTiles(){
        if(currentTile != null) {
            currentTile.current = false;
            currentTile = null;
        }
        foreach(Tile tile in selectableTiles) {
            tile.Reset();
        }
        selectableTiles.Clear();
    }

    protected void ClearAttackableTiles(){
        if(currentTile != null) {
            currentTile.current = false;
            currentTile = null;
        }
        foreach(Tile tile in attackableTiles) {
            tile.Reset();
        }
        attackableTiles.Clear();
    }

    void CalculateHeading(Vector3 target) {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity(){
        velocity = heading * moveSpeed;
    }

    public void BeginTurn() {
        turn = true;
    }

    public void EndTurn() {
        turn = false;
    }

    protected void FindPath(Tile target){
        ComputeAdjacentTiles(target);
        GetCurrentTile();
        ShowCurrentTile();
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);
        //currentTile.parent = null;

        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while(openList.Count > 0) {
            tmpTile = FindLowestF(openList);
            closedList.Add(tmpTile);
            if(tmpTile == target) {
                actualTargetTile = FindEndTile(tmpTile);
/*                 if (actualTargetTile.GetTerrain() == "Enemy") {
                    Debug.Log(name + ": Move->End");
                    characterState = CharacterState.End;
                    return;
                } */
                MoveToTile(actualTargetTile);
                return;
            }
            else {
                
            }
            foreach (Tile tile in tmpTile.adjacentTiles) {
                if(closedList.Contains(tile)) {

                }

                else if(openList.Contains(tile)) {
                    float tempG = tile.g + Vector3.Distance(tile.transform.position, tmpTile.transform.position);

                    if(tempG < tile.g) {
                        tile.parent = tmpTile;
                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                else {
                    tile.parent = tmpTile;
                    tile.g = tmpTile.g + Vector3.Distance(tile.transform.position,tmpTile.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position,target.transform.position);
                    tile.f = tile.h + tile.g;

                    openList.Add(tile);
                }
            }
        }

        //BFS for next nearest tile
    }

    protected Tile FindEndTile(Tile target) {
        Stack<Tile> tmpPath = new Stack<Tile>();

        Tile next = target.parent;
        while(next != null) {
            tmpPath.Push(next);
            next = next.parent;
        }
        /*for (int i = 1; i < attackRange; i++) {
            tmpPath.Pop();
        }*/

        if(tmpPath.Count <= movementCost) {
            return target.parent;
        }

        Tile endTile = null;
        for (int i = 0; i <= movementCost; i++) {
            endTile = tmpPath.Pop();
        }
        return endTile;
    }

    protected Tile FindLowestF(List<Tile> tiles) {
        Tile lowest = tiles[0];

        foreach(Tile tile in tiles) {
            if(tile.f < lowest.f) {
                lowest = tile;
            }
        }

        tiles.Remove(lowest);
        

        return lowest;
    }

    protected void CheckDead(){
        if (GetComponent<CharacterStats>().currentHealth <= 0) {
            //Debug.Log("I'm dead");
            characterAnimator.SetTrigger("Death");
        }
    }

    public void SetStatus(SelectStatus status) {
        this.status = status;
        if(status == SelectStatus.Selected) {
            GetComponentInChildren<SkinnedMeshRenderer>().material = selectedMaterial;
        }
        if(status == SelectStatus.Unselected) {
            GetComponentInChildren<SkinnedMeshRenderer>().material = unselectedMaterial;
        }
        if(status == SelectStatus.Acted) {
            GetComponentInChildren<SkinnedMeshRenderer>().material = actedMaterial;
        }
    }
}

