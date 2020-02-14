using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public string terrain;
    //Tile States

    private MeshRenderer meshRenderer;
    public Material originalMaterial;

    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool attackable = false;
    
    public Material currentMaterial;
    public Material targetMaterial;
    public Material selectableMaterial;
    public Material attackableMaterial;

    public List<Tile> adjacentTiles = new List<Tile>();
    public List<Tile> attackableTiles = new List<Tile>();
    public List<Tile> itemTiles = new List<Tile>();

    //BFS for optimized pathfinding
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    private Tile tmpTile;
    
    //Raytracing to detect objects above
    private RaycastHit hit;

    //A*
    public float f = 0;
    public float g = 0;
    public float h = 0;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        GetTerrain();
        //GetAdjacentTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(current) {
            meshRenderer.material = currentMaterial;
        }
        else if (target) {
            meshRenderer.material = targetMaterial;
        }
        else if (selectable) {
            meshRenderer.material = selectableMaterial;
        }
        else if (attackable) {
            meshRenderer.material = attackableMaterial;

        }
        else {
            meshRenderer.material = originalMaterial;
        }
        
    }

    public void Reset(){
        adjacentTiles.Clear();
        attackableTiles.Clear();
        current = false;
        target = false;
        selectable = false;
        attackable = false;

        visited = false;
        parent = null;
        distance = 0;

        f = g = h = 0;
    }

    private void GetAdjacentTilesInDirection(Vector3 direction, Tile target) {
        Collider[] frontColliders = Physics.OverlapBox(transform.position + direction * 2.0f, transform.localScale * 0.1f);
        foreach(Collider c in frontColliders) {
            Tile tmpTile = c.GetComponent<Tile>();
            if (tmpTile != null && tmpTile.walkable) {
                if (!Physics.Raycast(tmpTile.transform.position - new Vector3(0.0f,0.5f,0.0f), Vector3.up, out hit, 3.0f) || (tmpTile == target)) {
                    adjacentTiles.Add(tmpTile);
                }
            }
        }
    }

    private void GetAttackableTilesInDirection(Vector3 direction) {
        Collider[] frontColliders = Physics.OverlapBox(transform.position + direction * 2.0f, transform.localScale * 0.1f);
        foreach(Collider c in frontColliders) {
            Tile tmpTile = c.GetComponent<Tile>();
            if (tmpTile != null && tmpTile.walkable) {
                //if (GetTerrain() == "Enemy") {
                //    Debug.Log("Enemy in range!");
                    attackableTiles.Add(tmpTile);
                //}
            }
        }
    }

    private void GetItemTilesInDirection(Vector3 direction) {
        Collider[] frontColliders = Physics.OverlapBox(transform.position + direction * 2.0f, transform.localScale * 0.1f);
        foreach(Collider c in frontColliders) {
            Tile tmpTile = c.GetComponent<Tile>();
            if (tmpTile != null && tmpTile.GetTerrain() == "Item") {
                    Debug.Log("Item found!");
                    itemTiles.Add(tmpTile);
            }
        }
    }

    public void GetAdjacentTiles(Tile target){
        Reset();
        GetAdjacentTilesInDirection(Vector3.forward,target);
        GetAdjacentTilesInDirection(Vector3.back,target);
        GetAdjacentTilesInDirection(Vector3.left,target);
        GetAdjacentTilesInDirection(Vector3.right,target);
    }

    public void GetAttackableTiles(){
        Reset();
        GetAttackableTilesInDirection(Vector3.forward);
        GetAttackableTilesInDirection(Vector3.back);
        GetAttackableTilesInDirection(Vector3.left);
        GetAttackableTilesInDirection(Vector3.right);
    }

    public void GetItemTiles(){
        Reset();
        GetItemTilesInDirection(Vector3.forward);
        GetItemTilesInDirection(Vector3.back);
        GetItemTilesInDirection(Vector3.left);
        GetItemTilesInDirection(Vector3.right);
    }

    public string GetTerrain() {
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1)) {
            Debug.Log("Terrain: " + hit.transform.tag);
            terrain = hit.transform.tag;
            return hit.transform.tag;
            //adjacentTiles.Add(tmpTile);
        }
        else {
            return null;
        }
    }


    /*
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
