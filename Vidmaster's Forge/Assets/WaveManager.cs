using Kitbashery.Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class WaveManager : MonoBehaviour
{
    public Spawner[] spawners;

    public void OnWave()
    {
        StartCoroutine(OnWaveQueue());
    }
    public IEnumerator OnWaveQueue()
    {

        foreach (var item in spawners)
        {
            item.canSpawn = false;
        }

        yield return new WaitForSeconds(10);

        foreach (var item in spawners)
        {
            item.canSpawn = true;
        }


    }


}


