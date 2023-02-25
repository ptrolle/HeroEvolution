using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour 
{
    public enum BOMB_POWER
    {
        NORMAL,
        EXPLODE
    }

    public BOMB_POWER BP;

    public GameObject Player;

    public Sprite[] spritesBomb;
    private bool changeSprites;

    float timeChangeSprite = 0;

    #region sound
    public AudioSource sound;
    public AudioClip soundBomb;
    private int auxSound;
    #endregion

	// Use this for initialization
	void Start () {
        //para ver se som esta habilitado ou nao
        auxSound = PlayerPrefs.GetInt("SaveSound");

        if (auxSound == 1)
        {
            sound.mute = true;
        }

        changeSprites = false;
        StartCoroutine(UsingBomb());

        BP = BOMB_POWER.NORMAL;

        Player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (BP == BOMB_POWER.NORMAL)
        {
            timeChangeSprite += Time.deltaTime;
            //Debug.Log(test);
            if (timeChangeSprite >= 0.5f)
            {
                timeChangeSprite = 0;
                if (changeSprites)
                    gameObject.GetComponent<SpriteRenderer>().sprite = spritesBomb[0];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = spritesBomb[1];

                changeSprites = !changeSprites;
            }
        }
        else if (BP == BOMB_POWER.EXPLODE)
        {
            Debug.Log("Entrou Aqui EXPLODE");
            gameObject.GetComponent<SpriteRenderer>().sprite = spritesBomb[2];
        }
	}

    //testa colisao da bomba com inimigos que estao dentro no raio de explosao
    void OnTriggerStay2D(Collider2D p_collider)
    {
        if (p_collider.gameObject.tag == "Enemy" && BP == BOMB_POWER.EXPLODE) //apenas destroy inimigos que estao dentro do raio de explsao no momento que explode
        {
            Debug.Log("HORA DA EXPLOSAO"+p_collider.gameObject.name);
            Destroy(p_collider.gameObject);
            //incrementar valor xp player
            Player.gameObject.GetComponent<Test2>().SliderController(0.3f); //bomba da mais xp de que outros powerups
        }
    }

    IEnumerator UsingBomb()
    {
        yield return new WaitForSeconds(6); //Destroy escudo depois de um tempo

        BP = BOMB_POWER.EXPLODE;
        sound.PlayOneShot(soundBomb, 0.5f);
        Destroy(this.gameObject, 0.2f);
    }
}
