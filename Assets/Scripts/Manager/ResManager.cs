using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ResManager : SingletonAutoMono<ResManager>
{
    [Header("ͳһ�� Addressables ��ǩ")]
    public List<string> ADBLabels = new List<string> { "Prefab", "TileBase" };

    [ShowInInspector]
    public Dictionary<string, GameObject> AllPrefabs = new();
    [ShowInInspector]
    public Dictionary<string, TileBase> tileBaseDict = new();

    public int LoadedCount = 0;
    public bool isLoadFinish = false;

    new void Awake()
    {
        base.Awake();
        LoadResourcesSync();
    }

    [Button("�ֶ�ͬ����ʼ����Դ")]
    public void LoadResourcesSync()
    {
        LoadedCount = 0;

        if (ADBLabels == null || ADBLabels.Count == 0)
        {
            ADBLabels = new List<string> { "Prefab", "TileBase" };
        }

        // ����������Դ����
        SyncLoadAssetsByLabels<GameObject>(ADBLabels, AllPrefabs);
        SyncLoadAssetsByLabels<TileBase>(ADBLabels, tileBaseDict);

        isLoadFinish = true;
        // Debug.Log($"��Դ������ɣ������� {LoadedCount} ����Դ");
    }

    private void SyncLoadAssetsByLabels<T>(List<string> labels, IDictionary<string, T> dict)
    {
        if (labels == null || labels.Count == 0) return;

        var handle = Addressables.LoadAssetsAsync<T>(
            labels,
            null,
            Addressables.MergeMode.Union);

        var assets = handle.WaitForCompletion();

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"ͬ������ {typeof(T).Name} ʧ��");
            return;
        }

        foreach (var asset in assets)
        {
            if (asset == null) continue;

            string key = asset switch
            {
                GameObject go => go.name,
                TileBase tile => tile.name,
                _ => asset.ToString()
            };

            dict[key] = asset;
            LoadedCount++;
        }

        // Debug.Log($"{typeof(T).Name} ��Դ������ɣ�������{assets.Count}");
    }

    // �ⲿ��ȡ��Դ�ӿ�
    public GameObject GetPrefab(string name)
    {
        AllPrefabs.TryGetValue(name, out var go);
        return go;
    }

    public TileBase GetTileBase(string name)
    {
        tileBaseDict.TryGetValue(name, out var tile);
        return tile;
    }
}
