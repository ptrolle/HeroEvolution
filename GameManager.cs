using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    
    public GameObject[] enemyCharacters;
    //array inimigos sprites
    
    //array de stages backgrounds
    public Sprite[] spritesBackgrounds;
    public GameObject Gbackground;
    private int currentBackground = 0;

    private int[] posX = {4,-4};
    //private int currentStage = 1; //atual fase mapa

    private float nextEnemyTime = 0.0f;
    private float spawnRate = 1.5f;

    public int minRandEnemyStg = 0; //inicializa o min e max de inimigos a serem randomizados
    public int maxRandEnemyStg = 3;

    public int minRandLvlEnemy = 1;
    public int maxRandLvlEnemy = 5;

    private float controlSpawn = 0.97f;

    private int controlFirsTimeLvlEnemy = 1;

    void Awake () {
        
        if (PlayerPrefs.GetInt("MinLvlEnemy") != 0)
        {
            minRandLvlEnemy = PlayerPrefs.GetInt("MinLvlEnemy");
            maxRandLvlEnemy = PlayerPrefs.GetInt("MaxLvlEnemy");
            minRandEnemyStg = PlayerPrefs.GetInt("MinEnemyStage");
            maxRandEnemyStg = PlayerPrefs.GetInt("MaxEnemyStage");
            currentBackground = PlayerPrefs.GetInt("StageNumb");
            //Debug.Log("CurrentBackground:" + currentBackground);
            controlFirsTimeLvlEnemy = PlayerPrefs.GetInt("AuxLvlEnemy");

            spawnRate = PlayerPrefs.GetFloat("SpawnRate");
            controlSpawn = PlayerPrefs.GetFloat("ControlSpawn");

            Gbackground.gameObject.GetComponent<SpriteRenderer>().sprite = spritesBackgrounds[currentBackground];
        }

	}

	void Update () 
    {
        if (nextEnemyTime < Time.time)
        {
            InstanceEnemyMap();
            nextEnemyTime = Time.time + spawnRate;

            //Speed up the spawnrate for the next egg
            //spawnRate *= 0.97f;
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
        Debug.Log(randEnemy);

        // Instantiate(enemy, new Vector3(posX[randPosX], posY, transform.position.z), Quaternion.identity); //instancia inimigo

        GameObject gEnemy = (GameObject)Instantiate(enemyCharacters[randEnemy], new Vector3(posX[randPosX], posY, transform.position.z), Quaternion.identity); //instancia inimigo
        Vector3 setMoveEnemy = randPosX > 0 ? Vector3.right : Vector3.left; 
        gEnemy.GetComponent<EnemyController>().moveEnemy = setMoveEnemy;
        int randLvlEnemy = Random.Range(minRandLvlEnemy, maxRandLvlEnemy); //sorteia level a ser colocado no inimigo
        gEnemy.GetComponent<EnemyController>().enemyLevel = randLvlEnemy;
    }

    //funcao para controle de fases, novos inimigos, level de inimigo, criacao mais rapida para maior dificuldade
    public void ChangeMap()
    {
        currentBackground++;

        minRandEnemyStg += 3;
        maxRandEnemyStg += 3;

        if (currentBackground > 10) //se chegou no ultimo background disponivel volta no primeiro, inimigos tbm, mas level continua a crescer
        {
            Debug.Log("Entrou aqui zerou array personagens e mapas");
            currentBackground = 0;
            minRandEnemyStg = 0;
            maxRandEnemyStg = 3;
        }

        Gbackground.gameObject.GetComponent<SpriteRenderer>().sprite = spritesBackgrounds[currentBackground];
        
        minRandLvlEnemy += (5 - controlFirsTimeLvlEnemy); //para novos levels do inimigo
        maxRandLvlEnemy += 5;
        controlFirsTimeLvlEnemy = 0; //para funcionar apenas uma vez depois de criado, auxiliar para controle de level inimigo
        
        /*para controle de instanciamento de inimigos*/
        if(controlSpawn >= 0.88f) //para limitar um max de qtd de inimigos a serem lancados na fase
        {
            controlSpawn -= 0.01f; 
            spawnRate *= controlSpawn;
            SaveDatas.SaveSpawnRate(spawnRate, controlSpawn);
        }


        //salvando dados
        SaveDatas.SaveStageEnemies(currentBackground, minRandLvlEnemy, maxRandLvlEnemy, minRandEnemyStg, maxRandEnemyStg, controlFirsTimeLvlEnemy); 
        
    }

}
