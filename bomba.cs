using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomba : MonoBehaviour
{
    public float guc = 10f;
    public float menzil = 5f;
    public float yukariguc = 1f;
    public ParticleSystem patlamaEfekti;
    public AudioSource patlamaSesi;
   // private bool hasExploded = false;
    void Start()
    {
        patlamaSesi = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    private void OnCollisionEnter(Collision collision)
    {
        
            if (collision != null)
            {
                Patlama();
            }

        
    }
    void Patlama()
    {
        Vector3 patlamaPozisyonu = transform.position;
        Instantiate(patlamaEfekti, transform.position, transform.rotation);
        Destroy(gameObject, 1f);
        patlamaSesi.Play();
        Collider[] colliders = Physics.OverlapSphere(patlamaPozisyonu, menzil);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb= hit.GetComponent<Rigidbody>();
            if(hit!=null && rb)
            {
                if (hit.gameObject.CompareTag("Dusman"))
                {
                    hit.transform.gameObject.GetComponent<Dusman>().oldun();
                }
                rb.AddExplosionForce(guc, patlamaPozisyonu, menzil, yukariguc, ForceMode.Impulse);
                Destroy(gameObject, 7f);
            }
        }
    }
    
}
