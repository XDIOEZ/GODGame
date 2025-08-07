using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ResManager : SingletonAutoMono<ResManager>
{
    [Header("统一的 Addressables 标签")]
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

    [Button("手动同步初始化资源")]
    public void LoadResourcesSync()
    {
        LoadedCount = 0;

        if (ADBLabels == null || ADBLabels.Count == 0)
        {
            ADBLabels = new List<string> { "Prefab", "TileBase" };
        }

        // 加载所有资源类型
        SyncLoadAssetsByLabels<GameObject>(ADBLabels, AllPrefabs);
        SyncLoadAssetsByLabels<TileBase>(ADBLabels, tileBaseDict);

        isLoadFinish = true;
        // Debug.Log($"资源加载完成，共加载 {LoadedCount} 个资源");
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
            Debug.LogError($"同步加载 {typeof(T).Name} 失败");
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

        // Debug.Log($"{typeof(T).Name} 资源加载完成，数量：{assets.Count}");
    }

    // 外部获取资源接口
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
