using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MermiKutusuOlustur : MonoBehaviour
{
    public List<GameObject> MermiKutusuPoint = new List<GameObject>();
    public GameObject MermiKutusu;
    public static bool MermiKutusuVarMi;


    void Start()
    {
        MermiKutusuVarMi = false;
        StartCoroutine(MermiKutusuYap());
    }


    IEnumerator MermiKutusuYap()
    {
        while (true)
        {
            yield return null;

            if (!MermiKutusuVarMi)
            {
                yield return new WaitForSeconds(5f);
                
                    int randomsayim = Random.Range(0, 5);
                    Instantiate(MermiKutusu, MermiKutusuPoint[randomsayim].transform.position, MermiKutusuPoint[randomsayim].transform.rotation);
                    MermiKutusuVarMi = true;
                
            }
        }
        //while (true)
        //{
        //    yield return new WaitForSeconds(5f);
        //    int randomsayim = Random.Range(0, 3);
        //    Instantiate(MermiKutusu, MermiKutusuPoint[randomsayim].transform.position, MermiKutusuPoint[randomsayim].transform.rotation);
        //    Debug.Log("slm");

        //}

    }
}
