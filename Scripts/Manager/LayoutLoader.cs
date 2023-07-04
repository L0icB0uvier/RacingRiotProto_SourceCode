using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Manager
{
    public class LayoutLoader : MonoBehaviour
    {
        public static LayoutLoader Instance;
        
        [SerializeField] 
        private LayoutSettings _layoutSettings;

        public UnityAction OnLayoutReady;

        private AsyncOperationHandle<SceneInstance> _loadHandle;
        private SceneInstance _currentSceneInstance;
        private SceneInstance _previousSceneInstance;
        
        private LayoutSO _currentLayout;
        private LayoutSO _previousLayout;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameplayManager.Instance.OnPrepareRound += OnPrepareGame;
        }

        private void OnPrepareGame()
        {
            LoadNewLayout();
        }
        
        [ContextMenu("Load Layout")]
        public void LoadNewLayout()
        {
            StartCoroutine(LoadNewLayoutCoroutine());
        }

        private IEnumerator LoadNewLayoutCoroutine()
        {
            Debug.Log("Loading Layout.");
            var layout = GetNewLayout();
            
            if (layout == _currentLayout)
            {
                LayoutReady();
                yield break;
            }
            
            yield return LoadLayout(layout);

            if (_previousLayout != null)
            {
                yield return UnloadPreviousLayout();
            }
            
            LayoutReady();
        }

        private LayoutSO GetNewLayout()
        {
            HashSet<LayoutSO> availableLayouts = new HashSet<LayoutSO>();

            if (_layoutSettings.Layouts.Length == 1)
            {
                return _layoutSettings.Layouts[0];
            }
            
            foreach (var layout in _layoutSettings.Layouts)
            {
                if (layout == _currentLayout) continue;
                availableLayouts.Add(layout);
            }

            var selectedLayout = GetRandomLayout(availableLayouts.ToArray());
            return selectedLayout;
        }
        
        private LayoutSO GetRandomLayout(LayoutSO[] _availableLayouts)
        {
            var randomIndex = Random.Range(0, _availableLayouts.Length);
            return _availableLayouts[randomIndex];
        }
        
        private IEnumerator LoadLayout(LayoutSO _layout)
        {
            Debug.Log($"Loading {_layout.name}.");

            if (_currentLayout != null)
            {
                _previousLayout = _currentLayout;
            }
            
            _currentLayout = _layout;
            _loadHandle = _layout.SceneAssetRef.LoadSceneAsync(LoadSceneMode.Additive);
            while (_loadHandle.IsDone == false)
            {
                yield return null;
            }

            _previousSceneInstance = _currentSceneInstance;
            
            _currentSceneInstance = _loadHandle.Result;
            Debug.Log($"{_layout.name} loaded.");
        }

        private void LayoutReady()
        {
            Debug.Log("Layout Ready.");
            OnLayoutReady?.Invoke();
        }

        private IEnumerator UnloadPreviousLayout()
        {
            Debug.Log($"Unloading previous scene {_previousSceneInstance.Scene.name}.");

            var unloadingHandle = _previousLayout.SceneAssetRef.UnLoadScene();
            
            while (unloadingHandle.IsDone == false)
            {
                yield return null;
            }
        }
    }
}