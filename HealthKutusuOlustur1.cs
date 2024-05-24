using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKutusuOlustur1 : MonoBehaviour
{
    public List<GameObject> HealthKutusuPoint = new List<GameObject>();
    public GameObject HealthKutusu;
    public static bool HealthKutusuVarMi;


    void Start()
    {
        HealthKutusuVarMi = false;
        StartCoroutine(HealthKutusuYap());
    }


    IEnumerator HealthKutusuYap()
    {
        while (true)
        {
            yield return null;

            if (!HealthKutusuVarMi)
            {
                yield return new WaitForSeconds(5f);
                
                    int randomsayim = Random.Range(0, 4);
                    Instantiate(HealthKutusu, HealthKutusuPoint[randomsayim].transform.position, HealthKutusuPoint[randomsayim].transform.rotation);
                    HealthKutusuVarMi = true;
                
            }
        }

    }
    
}
