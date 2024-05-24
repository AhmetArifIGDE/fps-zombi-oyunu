using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class boskovan : MonoBehaviour
{
    public AudioSource Yeredusmesesi;
    void Start()
    {
        Yeredusmesesi = GetComponent<AudioSource>();
        Destroy(gameObject, 1.5f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Yol"))
        {
            Yeredusmesesi.Play();
            if (!Yeredusmesesi.isPlaying)
            {
                Destroy(gameObject, 1f);
            }
            
        }
    }
}
