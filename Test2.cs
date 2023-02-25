using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Test2 : MonoBehaviour 
{

    Vector2 initPos;
    Vector2 MinPos;
    Vector2 MaxPos;
    Vector2 targetPos;

    //array de sprites d evolucoes do player
    public Sprite[] spritesCharacters;
    public Button BtnPowerUps; 

    public Slider slider;

    public Sprite spriteShield;

    public float Speed = 3;
    public float thresholdStopDistance = 0.3f;

    public GameObject textLevel;
    public int currentLevel;
    public int nextBackground = 5;
    public float xp; //para controlar quando irá ganhar a cada inimigo derrotado
    private int nextEvolution = 9;
    private int currentEvolution = 0;
    private int powerUpsActiveControl = 0;
    private int maxLevel = 54; //para controlar max de level para evoluir personagem e ganhar power-Ups

    //cadeado controle abrir novos levels
    public Image lockLevel;
    public Text lockText;
    
    public GameObject gmc;
    public GameObject powerUpsGM; //para acesso a script powerUps

    #region HUD health
    public bool damaged; //se recebe dano
    public float flash = 1f; //efeito na tela quando recebe dano
    public Color flashColour = new Color(1f, 0f, 0f, 1f);
    public Image damageImage; //sprite vermelho (dano)
    #endregion

    private string[] descriptions = {"Bombs: gonna explode in 10 seconds", "XP: earn more per enemy defeated", 
                                       "To shoot arrows at all directions", "Shield: you can defeat all enemies that touch you",
                                       "Lazer: lazers up and down"};

    public Sprite[] powerUpsButtons;

    #region CanvasEvolutionRegion
    public Canvas mainCanvas;
    public Canvas canvasEvolution;
    public Canvas pauseCanvas;
    public Image playerEvolution;
    public Image newPower;
    public Text levelEvolution;
    public Text descriptionEvolution;
    #endregion

    #region sound
    public AudioSource sound;
    public AudioClip[] soundsPlayer;
    #endregion

    // Use this for initialization
	void Awake () {
        xp = 0.1f;
        damaged = false;
        //textLevel.GetComponent<TextMesh>().text = "Level";

        mainCanvas.gameObject.SetActive(true);
        canvasEvolution.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(false);

        //INICIALIZANDO NUMERO DE LEVEL A ABRIR NO CADEADO
        if (PlayerPrefs.GetInt("NextBackgroundNumb") == 0)
            nextBackground = 5;
        else
            nextBackground = PlayerPrefs.GetInt("NextBackgroundNumb");

        lockText.text = nextBackground.ToString();
        //Debug.Log("LOCK TEST:" + lockText.text);

        //INICIALIZANDO LOAD SAVES
        if (PlayerPrefs.GetInt("LvlPlayer") > 0)
        {
            //currentLevel = PlayerPrefs.GetInt("LvlPlayer") > 0 ? PlayerPrefs.GetInt("LvlPlayer") : 1; //inicializa com ultimo level salvo
            currentLevel = PlayerPrefs.GetInt("LvlPlayer");
            textLevel.gameObject.GetComponent<TextMesh>().text = "Player Lvl: " + currentLevel.ToString(); //mostra level atual


            currentEvolution = PlayerPrefs.GetInt("CurrentEvolution");
            gameObject.GetComponent<SpriteRenderer>().sprite = spritesCharacters[currentEvolution]; //troca personagem player

            powerUpsActiveControl = PlayerPrefs.GetInt("BtnPowerUpsActive");
        }

        //INICIALIZANDO LOAD NEXTEVOLUTION
        if (PlayerPrefs.GetInt("NextEvolution") == 0)
            nextEvolution = 9;
        else
            nextEvolution = PlayerPrefs.GetInt("NextEvolution");

        //testa se esta habilitado botao de powerUps
        if (powerUpsActiveControl > 0)
            BtnPowerUps.gameObject.SetActive(true);
        else
            BtnPowerUps.gameObject.SetActive(false);

        initPos = transform.position;
        targetPos = initPos;

        Vector2 Size = GetComponent<SpriteRenderer>().bounds.extents;
        MinPos = (Vector2)Camera.main.ViewportToWorldPoint(new Vector2(0, 0)) + Size;
        MaxPos = (Vector2)Camera.main.ViewportToWorldPoint(new Vector2(1, 1)) - Size;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0)) 
        {
            
            if (EventSystem.current.currentSelectedGameObject != null) //para nao movimentar o personagem quando aperta em algum botao UI
            {
                return;
            }
            else
            {

                Vector2 mousePos = (Input.mousePosition);
                Vector2 targetPos = new Vector2(Camera.main.ScreenToWorldPoint(mousePos).x, Camera.main.ScreenToWorldPoint(mousePos).y);

                targetPos.x = Mathf.Clamp(targetPos.x, MinPos.x, MaxPos.x);
                targetPos.y = Mathf.Clamp(targetPos.y, MinPos.y, MaxPos.y);


                transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * Speed);
            }
            
        }
        

        //se player é atacado da um flash na tela de ataque
        if (damaged)
        {
            sound.PlayOneShot(soundsPlayer[1]); //toca som de dano
            damageImage.color = flashColour;
        }
        else
        {
            //se nao volta aos poucos tela 0 alpha
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flash * Time.deltaTime);
        }
        damaged = false;

        //para controlar level que quero colocar
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentLevel++;
            textLevel.gameObject.GetComponent<TextMesh>().text = "Player Lvl: " + currentLevel.ToString();
        }

	}

    //testa colisao do player com o inimigo
    void OnTriggerEnter2D(Collider2D p_collider)
    {
        
        
        if (p_collider.gameObject.tag == "Enemy" && p_collider.gameObject.GetComponent<EnemyController>().enemyLevel <= currentLevel) //testo tag inimigo e level inimigo
        {
            
            //slider.value += 0.1f;
            sound.PlayOneShot(soundsPlayer[0], 0.5f); //toca som ganho xp
            Destroy(p_collider.gameObject);
            SliderController(xp); //incrementar valor xp, estava 0.1f 
        }
        else if(p_collider.gameObject.tag == "Enemy" && p_collider.gameObject.GetComponent<EnemyController>().enemyLevel > currentLevel)
        {
            SliderDamageController(0.3f);
        }
        
    }

    //funcao controle xp para aumentar level player
    public void SliderController(float p_xp)
    {
        slider.value += p_xp;

        //Debug.Log(slider.value+"/"+slider.maxValue);
        if (slider.value >= slider.maxValue)
        {
            currentLevel++;
            slider.value = 0;
            //Debug.Log("entrou aqui" + slider.value + "/levelP:" + currentLevel);
            textLevel.gameObject.GetComponent<TextMesh>().text = "Player Lvl: " + currentLevel.ToString();
            SaveDatas.SaveLvl(currentLevel); //salva novo level do Player
            BackgroundController(currentLevel);
            
            //testo se player esta evoluindo
            ChangePlayer();
        }
    }

    //cada vez que recebe dano de inimigo perde pontos de xp 0.3f
    public void SliderDamageController(float p_xp)
    {
        damaged = true;

        slider.value -= p_xp;
        if (slider.value <= 0)
        {
            slider.value = 0;
        }
    }

    //funcao para modificar background
    public void BackgroundController(int p_level)
    {
        if (p_level >= nextBackground)
        {
            nextBackground += 5; //para ver se pode passar para proximo mapa
            lockText.text = nextBackground.ToString(); //coloco no cadeado proximo level para abrir novo mapa
            
            SaveDatas.SaveLockNumber(nextBackground);
            
            gmc.GetComponent<GameManager>().ChangeMap(); //chamo funcao para que mude background e possa controlar novos inimigos e levels
        }
    }

    //funcao que testa caso chegou a level de trocar personagem
    public void ChangePlayer()
    {
        //testando porque level 3 esta trocando personagem
        Debug.Log("CurrentLevel:" + currentLevel);
        Debug.Log("Proxima Evo Level:"+nextEvolution);
        if (currentLevel >= nextEvolution)
        {
            Debug.Log("ENTROU AQUI LEVEL UP:" + currentLevel);
            nextEvolution += 9; //proxima evolucao
            
            if (currentLevel < maxLevel) //se level atual é menor maxLevel faz evolucao
            {
                currentEvolution++; //proximo personagem
                gameObject.GetComponent<SpriteRenderer>().sprite = spritesCharacters[currentEvolution]; //troca personagem player
                GetPowerUp(); //para ver se será habilitado mais algum power Up 
                ControllerPageEvolution();
            }

            SaveDatas.SaveEvolution(nextEvolution, currentEvolution);
        }
    }

    //funcao que testa se chegou na evolucao para habilitar botao de power up
    public void GetPowerUp()
    {
        
        BtnPowerUps.gameObject.SetActive(true); //habilita botão de power-ups
        powerUpsActiveControl = 1; //para salvar se botao de powerUps esta ativo
        powerUpsGM.gameObject.GetComponent<PowerUpsScript>().powerUpsAvailables++;
        int powerAvailables = powerUpsGM.gameObject.GetComponent<PowerUpsScript>().powerUpsAvailables;

        SaveDatas.SavePowerUps(powerUpsActiveControl, powerAvailables); //salva que esta habilitado botao de powerUps
        
    }

    //mostra pagina de evolucao personagem e novo poder
    public void ControllerPageEvolution()
    {
        mainCanvas.gameObject.SetActive(false); //desliga canvas principal game
        canvasEvolution.gameObject.SetActive(true); //liga canvas de evolucao personagem
        Time.timeScale = 0; //pause o game interno
        descriptionEvolution.text = descriptions[currentEvolution-1]; //descricao do poder do novo personagem
        levelEvolution.text = "Level:" + currentLevel.ToString(); //pegar level personagem
        playerEvolution.sprite = spritesCharacters[currentEvolution];//pegar imagem personagem nova
        newPower.sprite = powerUpsButtons[currentEvolution - 1];  //pegar imagem poder
    }

    //aperta botao para continuar jogo apos evolucao
    public void ContinueGame()
    {
        mainCanvas.gameObject.SetActive(true); //novamente libera canvas principal
        canvasEvolution.gameObject.SetActive(false); //desliga canvas de evolucao
        pauseCanvas.gameObject.SetActive(false); 
        Time.timeScale = 1; //tira do pause game principal
    }
}
