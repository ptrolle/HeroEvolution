using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour 
{

    public GameObject[] enemyCharacters;

    private int[] posX = { 4, -4 };

    private float nextEnemyTime = 0.0f;
    private float spawnRate = 1.5f;

    public int minRandEnemyStg = 0; //inicializa o min e max de inimigos a serem randomizados
    public int maxRandEnemyStg = 3;

    public int minRandLvlEnemy = 1;
    public int maxRandLvlEnemy = 5;

    private float controlSpawn = 0.97f;

    public Sprite soundOn;
    public Sprite soundOff;
    public Button btnSound;
    private bool soundController = true;
    private int auxSound;

    public Slider slLoad;
    public Text txtProgress;

    public AudioSource sound;

    void Start()
    {
        //para ver se som esta habilitado ou nao
        auxSound = PlayerPrefs.GetInt("SaveSound");
        Time.timeScale = 1; //volta ao normal sem pause

        if (auxSound == 0)
        {
            btnSound.gameObject.GetComponent<Image>().sprite = soundOn;
            sound.Play();
            soundController = true;
        }
        else
        {
            btnSound.gameObject.GetComponent<Image>().sprite = soundOff;
            sound.Pause();
            soundController = false;
        }
    }

    void Update()
    {
        if (nextEnemyTime < Time.time)
        {
            InstanceEnemyMap();
            nextEnemyTime = Time.time + spawnRate;

            spawnRate = Mathf.Clamp(spawnRate, 0.3f, 99f); //99f
            //Debug.Log("SPAWN_RATE:"+spawnRate);
        }
    }

    //funcao para instanciar um inimigo no mapa 
    public void InstanceEnemyMap()
    {
        //para instanciar inimigo no mapa
        float posY = Random.RandomRange(-4.5f, 4.15f); //definir pos que o inimigo vai nascer em Y
        int randPosX = Random.Range(0, 2); //defini qual posicao vou usar no array posX

        int randEnemy = Random.Range(minRandEnemyStg, maxRandEnemyStg); //randomizar sprites de inimigo dentro do stage
        //Debug.Log(randEnemy);

        // Instantiate(enemy, new Vector3(posX[randPosX], posY, transform.position.z), Quaternion.identity); //instancia inimigo

        GameObject gEnemy = (GameObject)Instantiate(enemyCharacters[randEnemy], new Vector3(posX[randPosX], posY, transform.position.z), Quaternion.identity); //instancia inimigo
        Vector3 setMoveEnemy = randPosX > 0 ? Vector3.right : Vector3.left;
        gEnemy.GetComponent<EnemyController>().moveEnemy = setMoveEnemy;
        int randLvlEnemy = Random.Range(minRandLvlEnemy, maxRandLvlEnemy); //sorteia level a ser colocado no inimigo
        gEnemy.GetComponent<EnemyController>().enemyLevel = randLvlEnemy;
    }

    public void BtnPlay()
    {
        //IR PARA JOGO
        //SceneManager.LoadScene("Map01");
        StartCoroutine(ProgressScene());
    }

    public void SoundController()
    {
        Debug.Log("entrou aqui");
        soundController = !soundController;
        if (soundController)
        {
            Debug.Log("soundON");
            sound.Play();
            btnSound.gameObject.GetComponent<Image>().sprite = soundOn;
            SaveDatas.SaveSound(0);
        }
        else
        {
            Debug.Log("soundOFF");
            sound.Pause();
            btnSound.gameObject.GetComponent<Image>().sprite = soundOff;
            SaveDatas.SaveSound(1);
        }
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();   
    }

    IEnumerator ProgressScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Map01");
        slLoad.gameObject.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.8f);
            slLoad.value = progress;
            txtProgress.text = (progress * 100f).ToString("##.00") + "%";
            yield return null;
        }
    }
}
