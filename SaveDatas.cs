using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDatas : MonoBehaviour 
{

    public static void SaveLvl(int p_lvlPlayer)
    {
        PlayerPrefs.SetInt("LvlPlayer", p_lvlPlayer);
    }

    public static void SaveLockNumber(int p_nextBackgroundNumb)
    {
        PlayerPrefs.SetInt("NextBackgroundNumb", p_nextBackgroundNumb);
    }

    public static void SaveEvolution(int p_evolution, int p_currentEvolution)
    {
        PlayerPrefs.SetInt("NextEvolution", p_evolution);
        PlayerPrefs.SetInt("CurrentEvolution", p_currentEvolution);
    }

    public static void SavePowerUps(int p_powerUps, int p_powerUpsAvailables)
    {
        PlayerPrefs.SetInt("BtnPowerUpsActive", p_powerUps);
        PlayerPrefs.SetInt("PowerUpsAvailables", p_powerUpsAvailables);
    }

    public static void SaveStageEnemies(int p_stage, int p_minLvlEnemy, int p_maxLvlEnemy, int p_minEnemyStage, int p_maxEnemyStage, int p_auxEnemyLvl)
    {
        PlayerPrefs.SetInt("StageNumb", p_stage);
        PlayerPrefs.SetInt("MinLvlEnemy", p_minLvlEnemy);
        PlayerPrefs.SetInt("MaxLvlEnemy", p_maxLvlEnemy);
        PlayerPrefs.SetInt("MinEnemyStage", p_minEnemyStage);
        PlayerPrefs.SetInt("MaxEnemyStage", p_maxEnemyStage);
        PlayerPrefs.SetInt("AuxLvlEnemy", p_auxEnemyLvl);
    }

    public static void SaveSpawnRate(float p_spawnRate, float p_controlSpawn)
    {
        PlayerPrefs.SetFloat("SpawnRate", p_spawnRate);
        PlayerPrefs.SetFloat("ControlSpawn", p_controlSpawn);
    }

    public static void SaveSound(int p_sound)
    {
        PlayerPrefs.SetInt("SaveSound", p_sound);
    }
}
