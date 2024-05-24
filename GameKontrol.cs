using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class GameKontrol : MonoBehaviour
{
    [Header("Silah Ayarlarý")]
    float Health = 100;
    public Image HealthBar;
    [Header("Silah Ayarlarý")]
    public GameObject[] silahlar;
    public AudioSource SilahDegisimSesi;
    public GameObject Bomba;
    public GameObject BombaPoint;
    public Camera BenimCam;
    [Header("Dusman Ayarlarý")]
    public GameObject[] dusmanlar;
    public GameObject[] cikisNoktalarý;
    public GameObject[] hedefNoktalari;
    public float DusmanCikmaSuresi;
    public TextMeshProUGUI ToplamDusman_Text;
    [Header("Genel Ayarlar")]
    public GameObject GameOverCanvas;
    public GameObject KazandinCanvas;
    public GameObject PauseCanvas;
    public int BaslangicDusmanSayisi;
    public static int KalanDusmanSayisi;
    public AudioSource OyunIciSes;
    public AudioSource ItemYokSes;
    public TextMeshProUGUI Saglik_Sayisi_Text;
    public TextMeshProUGUI Bomba_Sayisi_Text;
    public static bool OyunDurdumu;



    void Start()
    {
        BaslangicIslemleri();
    }
    void BaslangicIslemleri()
    {
        OyunDurdumu = false;
        
            if (!PlayerPrefs.HasKey("OyunBasladiMi"))
            {
                PlayerPrefs.SetInt("Taramali_Mermi", 200);
                PlayerPrefs.SetInt("Pompali_Mermi", 60);
                PlayerPrefs.SetInt("Magnum_Mermi", 70);
                PlayerPrefs.SetInt("Sniper_Mermi", 30);
                PlayerPrefs.SetInt("Saglik_Sayisi", 1);
                PlayerPrefs.SetInt("Bomba_Sayisi", 2);
                PlayerPrefs.SetInt("OyunBasladiMi", 1);
                
            }
        

        
        OyunIciSes = GetComponent<AudioSource>();
        OyunIciSes.Play();
        KalanDusmanSayisi = BaslangicDusmanSayisi;
        ToplamDusman_Text.text = BaslangicDusmanSayisi.ToString();

        Saglik_Sayisi_Text.text = PlayerPrefs.GetInt("Saglik_Sayisi").ToString();
        Bomba_Sayisi_Text.text = PlayerPrefs.GetInt("Bomba_Sayisi").ToString();

        StartCoroutine(DusmanCikar());
    }
    IEnumerator DusmanCikar()
    {

        while (true)
        {
            yield return null;
            if (BaslangicDusmanSayisi != 0)
            {
                yield return new WaitForSeconds(DusmanCikmaSuresi);

                int dusman = Random.Range(0, 5);
                int cikisnoktasi = Random.Range(0, 2);
                int hedefnoktasi = Random.Range(0, 2);
                GameObject obje = Instantiate(dusmanlar[dusman], cikisNoktalarý[cikisnoktasi].transform.position, Quaternion.identity);
                obje.GetComponent<Dusman>().HedefBelirle(hedefNoktalari[hedefnoktasi]);
                BaslangicDusmanSayisi--;
            }

        }
    }
    public void DusmanSayisiGuncelle()
    {
        KalanDusmanSayisi--;

        if (KalanDusmanSayisi <= 0)
        {
            Kazandin();
            ToplamDusman_Text.text = "0";
        }
        else
        {
            ToplamDusman_Text.text = KalanDusmanSayisi.ToString();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !OyunDurdumu)
        {
            SilahDegistir(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !OyunDurdumu)
        {
            SilahDegistir(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && !OyunDurdumu)
        {
            SilahDegistir(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && !OyunDurdumu)
        {
            SilahDegistir(3);
        }
        if (Input.GetKeyDown(KeyCode.G) && !OyunDurdumu)
        {

            BombaAt();
        }
        if (Input.GetKeyDown(KeyCode.E) && !OyunDurdumu)
        {
            SaglikDoldur();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !OyunDurdumu)
        {
            Pause();
        }
    }
    void BombaAt()
    {
        if (PlayerPrefs.GetInt("Bomba_Sayisi") != 0)
        {
            GameObject objem = Instantiate(Bomba, BombaPoint.transform.position, BombaPoint.transform.rotation);
            Rigidbody rg = objem.GetComponent<Rigidbody>();
            Vector3 acimiz = Quaternion.AngleAxis(90, BenimCam.transform.forward) * BenimCam.transform.forward;
            rg.AddForce(acimiz * 250f);
            PlayerPrefs.SetInt("Bomba_Sayisi", PlayerPrefs.GetInt("Bomba_Sayisi") - 1);
            Bomba_Sayisi_Text.text = PlayerPrefs.GetInt("Bomba_Sayisi").ToString();
        }
        else
        {
            ItemYokSes.Play();
        }
    }
    void SilahDegistir(int siraNumarasi)
    {
        foreach (GameObject silah in silahlar)
        {
            SilahDegisimSesi.Play();
            silah.SetActive(false);
        }
        silahlar[siraNumarasi].SetActive(true);
    }
    public void DarbeAl(float darbegucu)
    {
        Health -= darbegucu;
        HealthBar.fillAmount = Health / 100;
        if (Health <= 0)
        {
            GameOver();
        }
    }
    public void SaglikDoldur()
    {
        if (PlayerPrefs.GetInt("Saglik_Sayisi") != 0 && Health != 100)
        {
            Health = 100;
            HealthBar.fillAmount = Health / 100;
            PlayerPrefs.SetInt("Saglik_Sayisi", PlayerPrefs.GetInt("Saglik_Sayisi") - 1);
            Saglik_Sayisi_Text.text = PlayerPrefs.GetInt("Saglik_Sayisi").ToString();
        }
        else
        {
            //saðlýk dolu sesi
            ItemYokSes.Play();

        }
    }
    public void SaglikAl()
    {

        PlayerPrefs.SetInt("Saglik_Sayisi", PlayerPrefs.GetInt("Saglik_Sayisi") + 1);
        Saglik_Sayisi_Text.text = PlayerPrefs.GetInt("Saglik_Sayisi").ToString();
    }
    public void BombaAl()
    {

        PlayerPrefs.SetInt("Bomba_Sayisi", PlayerPrefs.GetInt("Bomba_Sayisi") + 1);
        Bomba_Sayisi_Text.text = PlayerPrefs.GetInt("Bomba_Sayisi").ToString();
    }
    public void AnaMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void GameOver()
    {
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        OyunDurdumu = true;
        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor=false;
        Cursor.lockState= CursorLockMode.None;
    }

    public void Pause()
    {

        PauseCanvas.SetActive(true);
        Time.timeScale = 0;
        OyunDurdumu = true;
        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
    }
    public void DevamEt()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        OyunDurdumu = false;
        Cursor.visible = false;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Kazandin()
    {
        KazandinCanvas.SetActive(true);
        Time.timeScale = 0;
        OyunDurdumu = true;
        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
    }
    public void BastanBasla()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
