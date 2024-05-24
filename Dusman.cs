using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dusman : MonoBehaviour
{

    NavMeshAgent ajan;
    GameObject hedef;
    public float health;
    public float DusmanDarbeGucu;
    GameObject anakontrolcum;
    Animator Animatorum;

    // Start is called before the first frame update
    void Start()
    {
        Animatorum = GetComponent<Animator>();
        anakontrolcum = GameObject.FindWithTag("AnaKontrolcum");
        ajan =GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        ajan.SetDestination(hedef.transform.position);
    }
    public void HedefBelirle(GameObject objem)
    {
        hedef=objem;
    }

    public void DarbeAl(float darbegucu)
    {
        health-=darbegucu;
        if (health <=0)
        {
            oldun();
            gameObject.tag = "Untagged";
        }
    }
    public void oldun()
    {
        Animatorum.SetTrigger("Olme");
        anakontrolcum.GetComponent<GameKontrol>().DusmanSayisiGuncelle();
        Destroy(gameObject,5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Korumam_Gerekli"))
        {
            
            anakontrolcum.GetComponent<GameKontrol>().DarbeAl(DusmanDarbeGucu);
            oldun();
        }
    }
}
