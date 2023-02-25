using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour 
{

    #region sound
    public AudioSource sound;
    private int auxSound;
    private bool soundController = true;
    #endregion

    private bool pause = false;

    public Button btnPause;
    public Button btnSound;
    public Sprite soundOn;
    public Sprite soundOff;
    public Canvas pauseCanvas;

    public Slider slLoad;
    public Text txtProgress;

	// Use this for initialization
	void Start () {
        //verifica se o som esta habilitado
        auxSound = PlayerPrefs.GetInt("SaveSound");

        if (auxSound == 0)
        {
            sound.Play();
            btnSound.gameObject.GetComponent<Image>().sprite = soundOn;
        }
        else
        {
            btnSound.gameObject.GetComponent<Image>().sprite = soundOff;
            sound.mute = true; //deixa mudo nenhum som irá tocar
        }

	}

    //funcao controla pause no game
    public void PauseController()
    {
        Time.timeScale = 0;
        pauseCanvas.gameObject.SetActive(true);
        
    }

    public void SoundController()
    {
        Debug.Log("entrou aqui");
        soundController = !soundController;
        if (soundController)
        {
            Debug.Log("soundON");
            sound.Play();
            sound.mute = false; 
            btnSound.gameObject.GetComponent<Image>().sprite = soundOn;
            SaveDatas.SaveSound(0);
        }
        else
        {
            Debug.Log("soundOFF");
            sound.mute = true; //deixa mudo nenhum som irá tocar
            btnSound.gameObject.GetComponent<Image>().sprite = soundOff;
            SaveDatas.SaveSound(1);
        }
    }

    public void ReturnMainMenu()
    {
        StartCoroutine(ProgressScene());
        //SceneManager.LoadScene("MainMenu");
    }

    IEnumerator ProgressScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("MainMenu");
        slLoad.gameObject.SetActive(true);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.8f);
            slLoad.value = progress;
            txtProgress.text = (progress * 100f).ToString("##.00") + "%";
            yield return null;
        }

        //Time.timeScale = 1; //volta ao normal sem pause
        
    }
}
