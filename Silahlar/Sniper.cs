using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    Animator animatorum;
    [Header("Ayarlar")]
    public bool atesEdebilirmi;
    float İceridenAtesEtmeSikligi;
    public float disaridanAtesEtmeSikligi;
    public float menzil;
    public GameObject Cross;
    public GameObject Scope;
    [Header("Sesler")]
    public AudioSource AtesSesi;
    public AudioSource SarjorSesi;
    public AudioSource MermiBittiSes;
    public AudioSource MermiAlmaSesi;
    [Header("Efektler")]
    public ParticleSystem AtesEfekt;
    public ParticleSystem MermiIzi;
    public ParticleSystem KanEfekti;
    [Header("Diger")]
    public Camera benimCam;
    float CamFieldPov;
    float YaklasmaPov=20;
    [Header("Silah Ayarlari")]
    int ToplamMermiSayisi;
    public int SarjorKapasitesi;
    int KalanMermi;
    public string SilahinAdi;
    public float DarbeGucu;
    public TextMeshProUGUI KalanMermi_Text;
    public TextMeshProUGUI ToplamMermi_Text;
    public bool Kovan_Ciksinmi;
    public GameObject Kovan_cikis_noktasi;
    public GameObject BosKovan;
    public GameObject Mermi_cikis_noktasi;
    public GameObject Mermi;

    public MermiKutusuOlustur MermiKutusuOlusturYonetim;
    public Mermi mermi;
    
    void Start()
    {
        mermi = new Mermi();
        ToplamMermiSayisi = PlayerPrefs.GetInt(SilahinAdi + "_Mermi");
        Kovan_Ciksinmi = true;
        BaslangicMermiOlustur();
        SarjorDoldurmaTeknikFonksiyon("NormalYaz");
        animatorum = GetComponent<Animator>();
        CamFieldPov = benimCam.fieldOfView;
        }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (atesEdebilirmi && Time.time > İceridenAtesEtmeSikligi && KalanMermi != 0)
            {
                if (!GameKontrol.OyunDurdumu)
                {
                    AtesEt();
                    İceridenAtesEtmeSikligi = Time.time + disaridanAtesEtmeSikligi;
                }
            }
            if (KalanMermi == 0 && !MermiBittiSes.isPlaying)
            {
                MermiBittiSes.Play();
            }



        }
        if (Input.GetKeyDown(KeyCode.R))
        {

            if (KalanMermi < SarjorKapasitesi && ToplamMermiSayisi != 0)
            {
                animatorum.Play("SarjorDegis");
            }

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            MermiAl();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            KameraYaklastirVeScopeAc(true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            KameraYaklastirVeScopeAc(false);
        }

    }
    IEnumerator CameraTitre(float titremesuresi,float magnitude)
    {
        Vector3 orijinalpozisyon = benimCam.transform.localPosition;
        float gecensure = 0.0f;
        while (gecensure < titremesuresi)
        {
            float x = Random.Range(-1f, 1)*magnitude;

            benimCam.transform.localPosition=new Vector3(x,orijinalpozisyon.y,orijinalpozisyon.z);
            gecensure += Time.deltaTime;
            yield return null;
        }
        benimCam.transform.localPosition = orijinalpozisyon;
    }
    void SarjorDegistir()
    {
        SarjorSesi.Play();
        if (KalanMermi < SarjorKapasitesi && ToplamMermiSayisi != 0)
        {
            if (KalanMermi != 0)
            {
                SarjorDoldurmaTeknikFonksiyon("MermiVar");
            }
            else
            {
                SarjorDoldurmaTeknikFonksiyon("MermiYok");
            }

        }
    }
    
    void MermiAl()
    {
        RaycastHit hit;

        if (Physics.Raycast(benimCam.transform.position, benimCam.transform.forward, out hit, 4))
        {

            if (hit.transform.gameObject.CompareTag("Mermi"))
            {
                MermiKaydet(hit.transform.gameObject.GetComponent<MermiKutusu>().OlusanSilahTuru, hit.transform.gameObject.GetComponent<MermiKutusu>().OlusanMermiSayisi);
                MermiKutusuOlustur.MermiKutusuVarMi = false;
                Destroy(hit.transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mermi"))
        {
            MermiKaydet(other.transform.gameObject.GetComponent<MermiKutusu>().OlusanSilahTuru, other.transform.gameObject.GetComponent<MermiKutusu>().OlusanMermiSayisi);
            MermiKutusuOlustur.MermiKutusuVarMi = false;
            Destroy(other.transform.parent.gameObject);
        }
        if (other.gameObject.CompareTag("CanKutusu"))
        {
            MermiKutusuOlusturYonetim.GetComponent<GameKontrol>().SaglikAl();
            HealthKutusuOlustur1.HealthKutusuVarMi = false;
            Destroy(other.transform.gameObject);
        }
        if (other.gameObject.CompareTag("BombaKutusu"))
        {
            MermiKutusuOlusturYonetim.GetComponent<GameKontrol>().BombaAl();
            BombaKutusuOlustur.BombaKutusuVarMi = false;
            Destroy(other.transform.gameObject);
        }
    }

    void AtesEt()
    {

        AtesEtmeTeknikİslemleri();

        RaycastHit hit;

        if (Physics.Raycast(benimCam.transform.position, benimCam.transform.forward, out hit, menzil))
        {

            if (hit.transform.gameObject.CompareTag("Dusman"))
            {
                Instantiate(KanEfekti, hit.point, Quaternion.LookRotation(hit.normal));
                hit.transform.gameObject.GetComponent<Dusman>().DarbeAl(DarbeGucu);
            }
            else
            {
                Instantiate(MermiIzi, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }

    void BaslangicMermiOlustur()
    {
        if (ToplamMermiSayisi <= SarjorKapasitesi)
        {
            KalanMermi = SarjorKapasitesi;
            ToplamMermiSayisi = 0;
            PlayerPrefs.SetInt(SilahinAdi + "_Mermi", ToplamMermiSayisi);
        }
        else
        {
            KalanMermi = SarjorKapasitesi;
            ToplamMermiSayisi -= SarjorKapasitesi;
            PlayerPrefs.SetInt(SilahinAdi + "_Mermi", ToplamMermiSayisi);
        }
    }
    
    void SarjorDoldurmaTeknikFonksiyon(string tur)
    {
        switch (tur)
        {
            case "MermiVar":
                if (ToplamMermiSayisi <= SarjorKapasitesi)
                {
                    int olusanDeger = KalanMermi + ToplamMermiSayisi;
                    if (olusanDeger > SarjorKapasitesi)
                    {
                        KalanMermi = SarjorKapasitesi;
                        ToplamMermiSayisi = olusanDeger - SarjorKapasitesi;
                        PlayerPrefs.SetInt(SilahinAdi + "_Mermi", ToplamMermiSayisi);
                    }
                    else
                    {
                        KalanMermi += ToplamMermiSayisi;
                        ToplamMermiSayisi = 0;
                        PlayerPrefs.SetInt(SilahinAdi + "_Mermi", ToplamMermiSayisi);
                    }
                }
                else
                {
                    ToplamMermiSayisi -= SarjorKapasitesi - KalanMermi;
                    KalanMermi = SarjorKapasitesi;
                    PlayerPrefs.SetInt(SilahinAdi + "_Mermi", ToplamMermiSayisi);
                }

                KalanMermi_Text.text = KalanMermi.ToString();
                ToplamMermi_Text.text = "/" + ToplamMermiSayisi.ToString();
                break;
            case "MermiYok":
                if (ToplamMermiSayisi <= SarjorKapasitesi)
                {
                    ToplamMermiSayisi = 0;
                    KalanMermi = ToplamMermiSayisi;
                    PlayerPrefs.SetInt(SilahinAdi + "_Mermi", ToplamMermiSayisi);
                }
                else
                {
                    ToplamMermiSayisi -= SarjorKapasitesi;
                    KalanMermi = SarjorKapasitesi;
                    PlayerPrefs.SetInt(SilahinAdi + "_Mermi", ToplamMermiSayisi);
                }

                KalanMermi_Text.text = KalanMermi.ToString();
                ToplamMermi_Text.text = "/" + ToplamMermiSayisi.ToString();
                break;
            case "NormalYaz":
                KalanMermi_Text.text = KalanMermi.ToString();
                ToplamMermi_Text.text = "/" + ToplamMermiSayisi.ToString();
                break;
        }
    }
    void AtesEtmeTeknikİslemleri()
    {
        if (Kovan_Ciksinmi)
        {
            GameObject obje = Instantiate(BosKovan, Kovan_cikis_noktasi.transform.position, Kovan_cikis_noktasi.transform.rotation);
            Rigidbody rb = obje.GetComponent<Rigidbody>();
            rb.AddRelativeForce(new Vector3(-10, 1, 0) * 50);
        }
        StartCoroutine(CameraTitre(0.10f, .2f));
        Instantiate(Mermi, Mermi_cikis_noktasi.transform.position, Mermi_cikis_noktasi.transform.rotation);
        AtesSesi.Play();
        AtesEfekt.Play();
        animatorum.Play("ateset");
        KalanMermi--;
        KalanMermi_Text.text = KalanMermi.ToString();

        
    }
    void MermiKaydet(string silahturu,int mermisayisi)
    {
        MermiAlmaSesi.Play();
        switch (silahturu)
        {
            case "Taramali":
                PlayerPrefs.SetInt("Taramali_Mermi", PlayerPrefs.GetInt("Taramali_Mermi") + mermisayisi);

                break;
            case "Pompali":

                PlayerPrefs.SetInt("Pompali_Mermi", PlayerPrefs.GetInt("Pompali_Mermi") + mermisayisi);

                break;
            case "Magnum":

                PlayerPrefs.SetInt("Magnum_Mermi", PlayerPrefs.GetInt("Magnum_Mermi") + mermisayisi);

                break;
            case "Sniper":
                
                ToplamMermiSayisi += mermisayisi;
                PlayerPrefs.SetInt(SilahinAdi + "_Mermi", ToplamMermiSayisi);
                SarjorDoldurmaTeknikFonksiyon("NormalYaz");
                Debug.Log(silahturu + " " + mermisayisi);
                break;
        }
    }

    void KameraYaklastirVeScopeAc(bool durum)
    {
        if (durum)
        {
            Cross.SetActive(false);
            benimCam.cullingMask = ~(1<<6);
            animatorum.SetBool("zoomyap", durum);
            benimCam.fieldOfView = YaklasmaPov;
            Scope.SetActive(true);
        }
        else
        {
            benimCam.cullingMask = -1;
            Scope.SetActive(false);
            animatorum.SetBool("zoomyap", durum);
            benimCam.fieldOfView = CamFieldPov;
            Cross.SetActive(true);
        }

        
    }
}
//if (Input.GetKeyDown(KeyCode.R) || KalanMermi <= 0) //Alternatif kendi yazdığım
//{

//    ToplamMermiSayisi -= SarjorKapasitesi - KalanMermi;
//    if (ToplamMermiSayisi >= SarjorKapasitesi)
//    {
//        KalanMermi = SarjorKapasitesi;
//    }
//    else if (ToplamMermiSayisi < SarjorKapasitesi)
//    {
//        KalanMermi = ToplamMermiSayisi;
//        ToplamMermiSayisi = 0;
//        if (KalanMermi == 0)
//        {
//            //şarjor değiştirme ve ateş edeme yazmalıyım
//        }

//    }
//    if (KalanMermi < SarjorKapasitesi)
//    {
//        animatorum.Play("SarjorDegis");
//    }