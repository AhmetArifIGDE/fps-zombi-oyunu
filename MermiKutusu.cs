using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MermiKutusu : MonoBehaviour
{
    public string OlusanSilahTuru;
    public int OlusanMermiSayisi;
    public List<Sprite> SilahResimleri = new List<Sprite>();
    public Image SilahResmiImage;

    void Start()
    {
        int gelenanahtar = Random.Range(0, silahlar.Length);
        OlusanSilahTuru = silahlar[gelenanahtar];
        OlusanMermiSayisi = MermiSayisi[Random.Range(0, MermiSayisi.Length)];


        SilahResmiImage.sprite = SilahResimleri[gelenanahtar];
        //OlusanSilahTuru = "Taramali";
        //OlusanMermiSayisi = MermiSayisi[Random.Range(0, MermiSayisi.Length - 1)];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    string[] silahlar =
    {
        "Magnum",
        "Pompali",
        "Sniper",
        "Taramali"
    };
    int[] MermiSayisi =
    {
        10,
        20,
        5,
        30
    };
    
}
