using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> 
{
    public GameState state;
    public Player player;
    public int startingPlatform;
    public float xSpawnOffset;
    public float minYspawnPos;
    public float maxYspawnPos;
    public PlatForm[] platformPrefabs;
    public CollectableItem[] collectableItems;

    private PlatForm m_lastPlatformSpawned;
    private List<int> m_platformLandedIds;
    private float m_halfCamSixeX;
    private int m_score;

    public PlatForm LastPlatformSpawned { get => m_lastPlatformSpawned; set => m_lastPlatformSpawned = value; }
    public List<int> PlatformLandedIds { get => m_platformLandedIds; set => m_platformLandedIds = value; }
    public int Score { get => m_score; }

    public override void Awake()
    {
        MakeSingleton(false);
        m_platformLandedIds = new List<int>();
        m_halfCamSixeX = Helper.Get2DCamSize().x / 2;
    }

    public override void Start()
    {
        base.Start();
        state = GameState.Starting;
        Invoke("PlatformInit", 0.5f);
        if (AudioController.Ins)
        {
            AudioController.Ins.PlayBackgroundMusic();
        }
        if (GUIManager.Ins)
        {
            GUIManager.Ins.ShowMainmenu(true);
            GUIManager.Ins.ShowGameplay(false);
        }
    }

    public void PlayGame()
    {
        if (GUIManager.Ins)
        {
            //SceneManager.LoadSceneAsync();
            GUIManager.Ins.ShowGameplay(true);
            GUIManager.Ins.ShowMainmenu(false);
        }
        Invoke("PlayGameIvk", 1f);
    }

    private void PlayGameIvk()
    {
        state = GameState.Playing;
        if(player)
        {
            player.Jump();
        }
    }

    private void PlatformInit()
    {
        m_lastPlatformSpawned = player.PlatformLanded;
        for(int i=0; i< startingPlatform;i++)
        {
            SpawnPlatform();
        }
    }

    public bool IsPlatformLanded(int id)
    {
        if(m_platformLandedIds == null || m_platformLandedIds.Count <=0) return false;
        return m_platformLandedIds.Contains(id);
    }

    public void SpawnPlatform()
    {
        if (!player || platformPrefabs == null || platformPrefabs.Length<=0) return;
        float spawnPosX = Random.Range(
            -(m_halfCamSixeX - xSpawnOffset), (m_halfCamSixeX - xSpawnOffset));
        float distBetweenPlat = Random.Range(minYspawnPos, maxYspawnPos);
        float spawnPosY = m_lastPlatformSpawned.transform.position.y+distBetweenPlat;
        Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY,0f);

        int randIdx = Random.Range(0, platformPrefabs.Length);
        var platformPrefab = platformPrefabs[randIdx];
        if (!platformPrefab) return;

        var platformClone = Instantiate(platformPrefab, spawnPos, Quaternion.identity);
        platformClone.Id = m_lastPlatformSpawned.Id + 1;
        m_lastPlatformSpawned = platformClone;
    }

    public void SpawnCollectable(Transform spawnPoint)
    {
        if (collectableItems==null || collectableItems.Length<=0 || state != GameState.Playing) return;
        int randIdx = Random.Range(0, collectableItems.Length);
        var collectItem = collectableItems[randIdx];
        if(collectItem == null) return;
        float randCheck = Random.Range(0f, 1f);
        if (randCheck <= collectItem.spawnRate && collectItem.collectablePrefab)
        {
            var cClone= Instantiate(collectItem.collectablePrefab, spawnPoint.position, Quaternion.identity);
            cClone.transform.SetParent(spawnPoint);
        }
    }

    public void AddScore(int scoreToAdd)
    {
        if (state!= GameState.Playing) return;
        m_score += scoreToAdd;
        Pref.bestScore = m_score;
        if(GUIManager.Ins)
        {
            GUIManager.Ins.UpdateScore(m_score);
        }
    }
}
