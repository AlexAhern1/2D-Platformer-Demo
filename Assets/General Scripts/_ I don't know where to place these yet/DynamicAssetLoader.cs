using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game
{
    public static class DynamicAssetLoader
    {
        public static void LoadGameObject<T1, T2>(AssetReference reference, T1 t1, T2 t2, Action<GameObject, T1, T2> onLoadCallback)
        {
            AsyncOperationHandle<GameObject> asyncHandler = reference.LoadAssetAsync<GameObject>();
            asyncHandler.Completed += (asyncHandler) =>
            {
                if (asyncHandler.Status == AsyncOperationStatus.Succeeded)
                {
                    Logger.Log($"Asset ({asyncHandler.Result.name}) successfully loaded!", Logger.Addressables, MoreColors.Emerald);
                    onLoadCallback(asyncHandler.Result, t1, t2);
                }
                else
                {
                    Logger.Warn("Failed to load asset!", Logger.Addressables, MoreColors.Ruby);
                }
            };
        }

        public static void LoadGameObject<T>(AssetReference reference, Action<T> onLoadCallback) where T : MonoBehaviour
        {
            AsyncOperationHandle<GameObject> asyncHandler = reference.LoadAssetAsync<GameObject>();
            asyncHandler.Completed += (asyncHandler) =>
            {
                if (asyncHandler.Status == AsyncOperationStatus.Succeeded)
                {
                    T result = asyncHandler.Result.GetComponent<T>();
                    if (result == null)
                    {
                        Logger.Warn($"Asset loaded but component {typeof(T)} was not found!", Logger.Addressables, MoreColors.Ruby);
                        return;
                    }
                    Logger.Log($"Asset ({result}) successfully loaded!", Logger.Addressables, MoreColors.Emerald);
                    onLoadCallback(result);
                }
                else
                {
                    Logger.Warn("Failed to load asset!", Logger.Addressables, MoreColors.Ruby);
                }
            };
        }
    }
}