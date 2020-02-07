using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool walkable = true;
    

    public HashSet<Tile> adjacencyTiles = new HashSet<Tile>();
    public HashSet<Tile> attackTiles = new HashSet<Tile>();
    public bool visited = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(current) {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (selectable) {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (target) {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    private void GetAdjacentTilesInDirection(Vector3 direction, int movementCost, int attackRange) {
        Collider[] frontColliders = Physics.OverlapBox(transform.position + direction * 2.0f, transform.localScale * 0.1f);
        foreach(Collider c in frontColliders) {
            if(c.transform.tag == "Tile"){
                c.GetComponent<Tile>().selectable = true;
                adjacencyTiles.Add(c.GetComponent<Tile>());
                c.GetComponent<Tile>().GetMovementTiles(movementCost - 1, attackRange); 
            }
        }
    }

    public void GetMovementTiles(int movementCost,int attackRange){
        //Frente
        if (movementCost == 0) {
            GetAttackTiles(attackRange);
            return;
        }       
        GetAdjacentTilesInDirection(Vector3.forward,movementCost, attackRange);
        GetAdjacentTilesInDirection(Vector3.back,movementCost, attackRange);
        GetAdjacentTilesInDirection(Vector3.left,movementCost, attackRange);
        GetAdjacentTilesInDirection(Vector3.right,movementCost, attackRange);
    }

    public void GetAttackTiles(int attackRange) {
        if (attackRange == 0) {
            return;
        }
        Collider[] frontColliders = Physics.OverlapBox(transform.position + Vector3.forward * 2.0f, transform.localScale * 0.1f);
        foreach(Collider c in frontColliders) {
            if(c.transform.tag == "Tile"){
                c.GetComponent<Tile>().target = true;
                attackTiles.Add(c.GetComponent<Tile>());
                c.GetComponent<Tile>().GetAttackTiles(attackRange - 1);
            }
        }
        Collider[] backColliders = Physics.OverlapBox(transform.position + Vector3.back * 2.0f, transform.localScale * 0.1f);
        foreach(Collider c in backColliders) {
            if(c.transform.tag == "Tile"){
                c.GetComponent<Tile>().target = true;
                attackTiles.Add(c.GetComponent<Tile>());
                c.GetComponent<Tile>().GetAttackTiles(attackRange - 1);
            }
        }
        Collider[] rightColliders = Physics.OverlapBox(transform.position + Vector3.right * 2.0f, transform.localScale * 0.1f);
        foreach(Collider c in rightColliders) {
            if(c.transform.tag == "Tile"){
                c.GetComponent<Tile>().target = true;
                attackTiles.Add(c.GetComponent<Tile>());
                c.GetComponent<Tile>().GetAttackTiles(attackRange - 1);
            }
        }
        Collider[] leftColliders = Physics.OverlapBox(transform.position + Vector3.left * 2.0f, transform.localScale * 0.1f);
        foreach(Collider c in leftColliders) {
            if(c.transform.tag == "Tile"){
                c.GetComponent<Tile>().target = true;
                attackTiles.Add(c.GetComponent<Tile>());
                c.GetComponent<Tile>().GetAttackTiles(attackRange - 1);
            }
        }
    }

    /*public void GetAdjacentTiles(int movementCost) {
        Collider[] colliders = Physics.OverlapBox(transform.position + Vector3.forward, new Vector3(0.1f,0.1f,0.1f));
        foreach (Collider item in colliders) {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable) {
                tile.selectable = true;
            }
        }
    }*/
}
