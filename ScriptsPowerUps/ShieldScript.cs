using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour 
{
    public GameObject Player;

    #region sound
    public AudioSource sound;
    public AudioClip soundShield;
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
        
        StartCoroutine(UsingShield());

        Player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Player.transform.localPosition;
	}

    //testa colisao do player com o inimigo
    void OnTriggerEnter2D(Collider2D p_collider)
    {
        if (p_collider.gameObject.tag == "Enemy")
        {
            sound.PlayOneShot(soundShield, 0.5f);
            Destroy(p_collider.gameObject);
            //incrementar valor xp player
            Player.gameObject.GetComponent<Test2>().SliderController(0.1f);
        }
    }

    IEnumerator UsingShield()
    {
        yield return new WaitForSeconds(4); //Destroy escudo depois de um tempo

        Destroy(this.gameObject);
    }
}
