using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {


    public Vector3 moveEnemy;
    public int moveSpeed = 1;

    public int enemyLevel;

    public int minEnemyLvl = 1; //para controlar level no inimigo, min e max
    public int maxEnemyLvl = 4;

    public Sprite[] spritesDirection;//alguns inimigos precisamos definir sprites de direcao esq ou dir 


	// Use this for initialization
	void Start () {

        if (spritesDirection.Length > 0) //se precisa definir sprites de posicao
        {
            if (this.gameObject.transform.position.x < -3)
                this.gameObject.GetComponent<SpriteRenderer>().sprite = spritesDirection[0];
            else
                this.gameObject.GetComponent<SpriteRenderer>().sprite = spritesDirection[1];
        }
            

        //enemyLevel = Random.Range(minEnemyLvl, maxEnemyLvl);
        this.gameObject.GetComponentInChildren<TextMesh>().text = enemyLevel.ToString();

        StartCoroutine(DestroyEnemy());
	}
	
	// Update is called once per frame
	void Update () {
        MoveEnemy();
	}

    public void MoveEnemy()
    {
        //transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        transform.Translate(moveEnemy * moveSpeed * Time.deltaTime);
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(10); //Destroy inimigos depois d 10seg

        if(this.gameObject != null)
            Destroy(this.gameObject);
    }
}
