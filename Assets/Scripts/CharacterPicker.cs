using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPicker : MonoBehaviour
{
    public RaycastHit hit;
    public Ray ray;

    public CharacterDataFiller dataFiller;

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.tag == "Player" || hit.transform.tag == "Enemy") {
                dataFiller.selectedCharacter = hit.transform.GetComponent<CharacterStats>();
            }
        }  
    }
}
