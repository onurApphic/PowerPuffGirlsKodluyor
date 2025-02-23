﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PathCreation;

public class SpawnController : MonoBehaviour
{

    public Transform spawnLeft;
    public Transform spawnRight;
    public Transform spawnRotation;
    public PathCreator pathCreator;
    private Dictionary<string, Item> levelItemsClone;
    private bool canSpawn = true;
    private Coroutine SpawnCoroutine;
    private float spacing;
    private int frequency = 10;

    
    private void OnEnable()
    {
        EventManager.OnLevelFailed.AddListener(() => canSpawn = false);
        EventManager.OnLevelSuccesed.AddListener(() => canSpawn = false);
        EventManager.OnLevelFailed.AddListener(StopSpawn);
        EventManager.OnLevelSuccesed.AddListener(StopSpawn);
    }
    private void OnDisable()
    {
        EventManager.OnLevelFailed.RemoveListener(() => canSpawn = false);
        EventManager.OnLevelSuccesed.RemoveListener(() => canSpawn = false);
        EventManager.OnLevelFailed.RemoveListener(StopSpawn);
        EventManager.OnLevelSuccesed.RemoveListener(StopSpawn);
    }
    private void Start()
    {        
        OrderManager.Instance.SetLevelItems();
        ResetClone();
        SpawnLevelItems();        
    }
    private void Spawn()
    {        
        StopAllCoroutines();
        SpawnCoroutine = StartCoroutine(SpawnCo());
    }

    private void ResetClone()
    {
        levelItemsClone = new Dictionary<string, Item>(OrderManager.Instance.levelItems);
    }
    IEnumerator SpawnCo()
    {
        while (canSpawn)
        {
            Vector3 randomPos = new Vector3(Random.Range(spawnLeft.position.x, spawnRight.position.x),
                spawnLeft.position.y + Random.Range(0.5f, 1f),
                Random.Range(spawnLeft.position.z, spawnRight.position.z));

            int random = Random.Range(0, levelItemsClone.Count);
            Instantiate(levelItemsClone.Values.ElementAt(random).itemPrefab.gameObject, randomPos, spawnRotation.rotation);


            levelItemsClone.Remove(levelItemsClone.Keys.ElementAt(random));
            if (levelItemsClone.Count == 0) ResetClone();

            yield return new WaitForSeconds(LevelManager.Instance.CurrentLevel.spawnDelay);
        }
    }

    private void StopSpawn()
    {
        StopCoroutine(SpawnCoroutine);
    }

    private void SpawnLevelItems()
    {
        spacing = pathCreator.path.length / frequency;

        for (int i = 0; i < frequency; i++)
        {
            Vector3 pos = pathCreator.path.GetPointAtDistance(spacing * i) + Vector3.up*0.25f;

            int random = Random.Range(0, levelItemsClone.Count);
            Instantiate(levelItemsClone.Values.ElementAt(random).itemPrefab.gameObject, pos, spawnRotation.rotation);
            levelItemsClone.Remove(levelItemsClone.Keys.ElementAt(random));
            if (levelItemsClone.Count == 0) ResetClone();            
        }

        Spawn();
    }
}
