using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour 
{
    public enum ARROW_POSITION
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public ARROW_POSITION AP;

    public GameObject Player;

    private int moveSpeed = 5; //velocidade da flecha

    #region sound
    public AudioSource sound;
    public AudioClip soundArrow;
    private int auxSound;
    #endregion

    // Use this for initialization
    void Start()
    {
        //para ver se som esta habilitado ou nao
        auxSound = PlayerPrefs.GetInt("SaveSound");

        if (auxSound == 1)
        {
            sound.mute = true;
        }

        StartCoroutine(DestroyArrow());

        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        ArrowMove();
    }

    public void ArrowMove()
    {
        //verifica para que lado a flecha deve ir
        if (AP == ARROW_POSITION.LEFT)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else if (AP == ARROW_POSITION.RIGHT)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else if (AP == ARROW_POSITION.UP)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
        else if (AP == ARROW_POSITION.DOWN)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
    }

    //testa colisao do player com o inimigo
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Entrou Aqui");
        if (col.gameObject.tag == "Enemy")
        {
            sound.PlayOneShot(soundArrow, 0.5f);
            //incrementar valor xp player
            Player.gameObject.GetComponent<Test2>().SliderController(0.2f);

            Destroy(col.gameObject); //destroi inimigo 
            Destroy(this.gameObject); //destroi flecha se acertou
        }
    }

    //destroi a flecha se nao colidir com inimigo em 1 seg
    IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(1);

        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }
}
