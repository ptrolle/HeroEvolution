using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpsScript : MonoBehaviour 
{
    public enum TYPE_POWER
    {
        NORMAL,
        SHIELD,
        BOMB,
        ARROWS,
        PLUSXP,
        LAZER,
        INACTIVE
    }

    public TYPE_POWER tp;

    public Sprite[] spritesPowerIcon;
    public Sprite spritePowerUpNormal;
    public Sprite spritePowerUpInactive;

    public int auxPowerUps = 0;

    public Button powerUpsBtn;

    public int powerUpsAvailables = 0;

    //array de GameObjectsPowerUps
    public GameObject[] GmPowerUps; //para arrows e bombas

    public GameObject GmPlayer;

    public Image imgPlusXp;

    #region sound
    public AudioSource sound;
    public AudioClip[] soundsPowerUps;
    #endregion

	void Start () 
    {
        powerUpsAvailables = PlayerPrefs.GetInt("PowerUpsAvailables"); //pega powerUps disponiveis salvos
        Debug.Log("POWER_availables:"+powerUpsAvailables);
        imgPlusXp.gameObject.SetActive(false);
        tp = TYPE_POWER.NORMAL;
	}
	
	void Update () 
    {
        //da reset em todos saves
        if (Input.GetKeyDown(KeyCode.C))
        {
            InstantiatePowerUpsTests();
        }
	}

    public void SelectPowerUps()
    {

        if (tp == TYPE_POWER.NORMAL)
        {
            int getRandNumb = Random.Range(0, powerUpsAvailables); //sorteia qual poder irá utilizar
            Debug.Log("Sorteiar Poder:" + getRandNumb);

            sound.PlayOneShot(soundsPowerUps[0], 0.5f); //toca som ganho xp
            powerUpsBtn.gameObject.GetComponent<Image>().sprite = spritesPowerIcon[getRandNumb];
            SelectPowerIconState(getRandNumb);
        }
        else if (tp == TYPE_POWER.SHIELD)
        {
            ShieldActive();
        }
        else if (tp == TYPE_POWER.ARROWS)
        {
            ArrowsActive();
        }
        else if (tp == TYPE_POWER.BOMB)
        {
            BombActive();
        }
        else if (tp == TYPE_POWER.PLUSXP)
        {
            PlusXpActive();
        }
        else if (tp == TYPE_POWER.LAZER)
        {
            PlayLazer();
        }
        else if (tp == TYPE_POWER.INACTIVE)
        {
            //TEM QUE ESPERAR NOVAMENTE FICA ATIVO BOTAO POWER UP
            sound.PlayOneShot(soundsPowerUps[2], 0.5f); //toca som botao inativo
            Debug.Log("Botao inativo!!!");
        }
    }

    public void SelectPowerIconState(int p_power)
    {
        if (p_power == 0)
        {
            
            tp = TYPE_POWER.BOMB;
        }
        else if (p_power == 1)
        {
            tp = TYPE_POWER.PLUSXP;
        }
        else if (p_power == 2)
        {
            //Debug.Log("SHIELD ACESSO");
            tp = TYPE_POWER.ARROWS;
        }
        else if (p_power == 3)
        {
            tp = TYPE_POWER.SHIELD;
        }
        else if (p_power == 4)
        {
            tp = TYPE_POWER.LAZER;
        }
    }

    private void ShieldActive()
    {
        sound.PlayOneShot(soundsPowerUps[3], 0.5f);
        Debug.Log("SHIELD");
        Instantiate(GmPowerUps[0], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        powerUpsBtn.gameObject.GetComponent<Image>().sprite = spritePowerUpInactive;
        tp = TYPE_POWER.INACTIVE;
        StartCoroutine(ReturnBtn());
    }

    private void ArrowsActive()
    {
        sound.PlayOneShot(soundsPowerUps[1]); //toca som ganho xp
        Debug.Log("ARROWS");
        Instantiate(GmPowerUps[1], new Vector3(gameObject.transform.position.x - 0.7f, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
        Instantiate(GmPowerUps[2], new Vector3(gameObject.transform.position.x + 0.7f, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
        Instantiate(GmPowerUps[3], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1f, gameObject.transform.position.z), Quaternion.identity);
        Instantiate(GmPowerUps[4], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1f, gameObject.transform.position.z), Quaternion.identity);

        if (auxPowerUps < 4) //vc pode usar 5 vezes as flechas
        {
            auxPowerUps++;
        }
        else if (auxPowerUps >= 4) //usou ultima flecha 
        {
            auxPowerUps = 0;
            powerUpsBtn.gameObject.GetComponent<Image>().sprite = spritePowerUpInactive;
            tp = TYPE_POWER.INACTIVE;
            StartCoroutine(ReturnBtn());
        }
    }

    private void BombActive()
    {
        Debug.Log("BOMB");
        Instantiate(GmPowerUps[5], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

        if (auxPowerUps < 2)
        {
            auxPowerUps++;
        }
        else if (auxPowerUps >= 2)
        {
            auxPowerUps = 0;
            powerUpsBtn.gameObject.GetComponent<Image>().sprite = spritePowerUpInactive;
            tp = TYPE_POWER.INACTIVE;
            StartCoroutine(ReturnBtn());
        }
    }

    //aumenta ganho de xp a cada inimigo derrotado
    private void PlusXpActive()
    {
        sound.PlayOneShot(soundsPowerUps[3], 0.5f);
        Debug.Log("XP");
        GmPlayer.gameObject.GetComponent<Test2>().xp = 0.4f;
        imgPlusXp.gameObject.SetActive(true); //mostra imagem que esta ganhando mais XP a cada inimigo derrotado
        powerUpsBtn.gameObject.GetComponent<Image>().sprite = spritePowerUpInactive;
        tp = TYPE_POWER.INACTIVE;
        StartCoroutine(ReturnBtn());
        StartCoroutine(ReturnNormalXp());
    }

    private void PlayLazer()
    {
        sound.PlayOneShot(soundsPowerUps[3], 0.5f);
        Debug.Log("LAZER");
        powerUpsBtn.gameObject.GetComponent<Image>().sprite = spritePowerUpInactive;
        tp = TYPE_POWER.INACTIVE;
        
        Instantiate(GmPowerUps[6], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1f, gameObject.transform.position.z), Quaternion.identity);
        Instantiate(GmPowerUps[7], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1f, gameObject.transform.position.z), Quaternion.identity);

        StartCoroutine(ReturnBtn());
    }

    IEnumerator ReturnBtn()
    {
        yield return new WaitForSeconds(25); //Destroy escudo depois de um tempo

        powerUpsBtn.gameObject.GetComponent<Image>().sprite = spritePowerUpNormal;
        tp = TYPE_POWER.NORMAL;
    }

    IEnumerator ReturnNormalXp()
    {
        yield return new WaitForSeconds(10); //Destroy escudo depois de um tempo

        imgPlusXp.gameObject.SetActive(false);
        GmPlayer.gameObject.GetComponent<Test2>().xp = 0.1f;
    }

    //funcao que apertando C no jogo podemos testar para verificar power-up esta funcionando
    public void InstantiatePowerUpsTests()
    {
        //ARROWS
        /*Instantiate(GmPowerUps[1], new Vector3(GmPlayer.gameObject.transform.position.x - 0.7f, GmPlayer.gameObject.transform.position.y, GmPlayer.gameObject.transform.position.z), Quaternion.identity);
        Instantiate(GmPowerUps[2], new Vector3(GmPlayer.gameObject.transform.position.x + 0.7f, GmPlayer.gameObject.transform.position.y, GmPlayer.gameObject.transform.position.z), Quaternion.identity);
        Instantiate(GmPowerUps[3], new Vector3(GmPlayer.gameObject.transform.position.x, GmPlayer.gameObject.transform.position.y + 1f, GmPlayer.gameObject.transform.position.z), Quaternion.identity);
        Instantiate(GmPowerUps[4], new Vector3(GmPlayer.gameObject.transform.position.x, GmPlayer.gameObject.transform.position.y - 1f, GmPlayer.gameObject.transform.position.z), Quaternion.identity);*/

        //BOMB
        //Instantiate(GmPowerUps[5], new Vector3(GmPlayer.gameObject.transform.position.x, GmPlayer.gameObject.transform.position.y, GmPlayer.gameObject.transform.position.z), Quaternion.identity);

        //+XP
        //PlusXpActive();

        //SHIELD
        //ShieldActive();

        //LAZER
        PlayLazer();
    }
}
