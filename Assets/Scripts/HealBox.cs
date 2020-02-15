using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBox : MonoBehaviour
{
    public int healAmount;
    public ParticleSystem particles;
    // Start is called before the first frame update

    public void Destroy() {
        Destroy(this.gameObject);
    }
    public void PlayParticles() {
        particles.Play();
    }

    public void StopParticles() {
        particles.Stop();
    }
}
