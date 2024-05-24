using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaKutusuOlustur : MonoBehaviour
{
    public List<GameObject> BombaKutusuPoint = new List<GameObject>();
    public GameObject BombaKutusu;
    public static bool BombaKutusuVarMi;


    void Start()
    {
        BombaKutusuVarMi = false;
        StartCoroutine(BombaKutusuYap());
    }


    IEnumerator BombaKutusuYap()
    {
        while (true)
        {
            yield return null;

            if (!BombaKutusuVarMi)
            {
                yield return new WaitForSeconds(5f);
                
                    int randomsayim = Random.Range(0, 4);
                    Instantiate(BombaKutusu, BombaKutusuPoint[randomsayim].transform.position, BombaKutusuPoint[randomsayim].transform.rotation);
                    BombaKutusuVarMi = true;
                
            }
        }

    }
    
}
