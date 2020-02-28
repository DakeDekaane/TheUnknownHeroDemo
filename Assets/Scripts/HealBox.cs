using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBox : MonoBehaviour
{
    public int healAmount;
    public ParticleSystem particles;
    public Material usedMaterial;
    public bool used;
    // Start is called before the first frame update

    public void Destroy() {
        Destroy(this.gameObject);
    }

    public void SetUsedMaterial(){
        transform.Find("Cube.002").GetComponent<MeshRenderer>().material = usedMaterial;
        //GameObject.FindGameObjectWithTag("BoxMarker").GetComponent<MeshRenderer>().material = usedMaterial;
    }
    public void PlayParticles() {
        particles.Play();
    }

    public void StopParticles() {
        particles.Stop();
    }
}
