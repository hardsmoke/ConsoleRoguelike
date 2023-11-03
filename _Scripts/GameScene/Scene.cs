namespace ConsoleRoguelike.GameScene
{
    using GameObjects;
    using System;

    internal class Scene : IReadOnlyScene
    {
        public event Action<SceneLayer, Vector2Int, GameObject> GameObjectChangedPosition;
        public event Action<SceneLayer, GameObject> GameObjectAdded;
        public event Action<SceneLayer, Vector2Int> GameObjectRemoved;

        private Dictionary<int, SceneLayer> _sceneLayers = new Dictionary<int, SceneLayer>();
        public List<SceneLayer> Layers => _sceneLayers.Values.ToList();

        public int LayersCount => _sceneLayers.Count();

        public void AddObjectToScene(GameObject obj, int layerNumber)
        {
            AddObjectsToScene(new List<GameObject>() { obj }, layerNumber);
        }

        public void AddObjectsToScene(List<GameObject> objs, int layerNumber)
        {
            TryCreateLayer(layerNumber, out SceneLayer layer);

            for (int i = 0; i < objs.Count; i++)
            {
                layer.AddGameObject(objs[i]);
            }
        }

        public SceneLayer GetLayer(int layerNumber, bool createIfNotExist = true)
        {
            _sceneLayers.TryGetValue(layerNumber, out SceneLayer layer);

            if (layer == null && createIfNotExist)
            {
                TryCreateLayer(layerNumber, out layer);
            }

            return layer;
        }

        public int GetLayerNumber(SceneLayer layer)
        {
            return _sceneLayers.FirstOrDefault(x => x.Value == layer).Key;
        }

        public GameObject GetHighestGameObjectOnPosition(Vector2Int position)
        {
            List<SceneLayer> layersDescending = GetLayersAscending();
            layersDescending.Reverse();
            for (int i = 0; i < layersDescending.Count; i++)
            {
                GameObject gameObjectAtPosition = layersDescending[i].GetGameObjectsOnPosition(position).FirstOrDefault();
                if (gameObjectAtPosition != null)
                {
                    return gameObjectAtPosition;
                }
            }

            return null;
        }

        public SceneLayer GetHighestLayer()
        {
            List<int> keysList = _sceneLayers.Keys.OrderBy(layerNumber => layerNumber).ToList();
            return _sceneLayers[keysList.Last()];
        }

        public List<SceneLayer> GetLayersThatHigher(SceneLayer layer)
        {
            List<int> keysList = _sceneLayers.Keys.OrderBy(layerNumber => layerNumber).ToList();
            int layerNumber = GetLayerNumber(layer);

            List<int> lowestKeysList = keysList.Where(i => i > layerNumber).ToList();
            List<SceneLayer> lowestLayers = new List<SceneLayer>();
            for (int i = 0; i < lowestKeysList.Count; i++)
            {
                lowestLayers.Add(_sceneLayers[lowestKeysList[i]]);
            }

            return lowestLayers;
        }

        public List<SceneLayer> GetLayersAscending()
        {
            List<SceneLayer> layers = new List<SceneLayer>();

            List<int> keysList = _sceneLayers.Keys.OrderBy(layerNumber => layerNumber).ToList();
            for (int i = 0; i < keysList.Count; i++)
            {
                layers.Add(_sceneLayers[keysList[i]]);
            }

            return layers;
        }

        public bool TryCreateLayer(int layerNumber, out SceneLayer layer)
        {
            if (_sceneLayers.ContainsKey(layerNumber) == false)
            {
                layer = new SceneLayer();
                layer.GameObjectChangedPosition += OnGameObjectChangedPosition;
                layer.GameObjectAdded += OnGameObjectAdded;
                layer.GameObjectRemoved += OnGameObjectRemoved;
                _sceneLayers.Add(layerNumber, layer);
                return true;
            }
            else
            {
                layer = _sceneLayers[layerNumber];
            }

            return false;
        }

        public GameObject GetBottomLeftGameObject()
        {
            List<SceneLayer> layers = Layers;
            GameObject? bottomLeftGameObject = Layers.Count > 0 ? layers[0].GetBottomLeftGameObject() : null;

            for (int i = 1; i < layers.Count; i++)
            {
                GameObject bottomLeftLayerGameObject = layers[i].GetBottomLeftGameObject();
                if (bottomLeftLayerGameObject != null &&
                    Vector2Int.IsFirstHigherSecond(bottomLeftLayerGameObject.Position, bottomLeftGameObject.Position, true) &&
                    Vector2Int.IsFirstLefterSecond(bottomLeftLayerGameObject.Position, bottomLeftGameObject.Position, true))
                {
                    bottomLeftGameObject = bottomLeftLayerGameObject;
                }
            }

            return bottomLeftGameObject;
        }

        private void OnGameObjectAdded(SceneLayer layer, GameObject gameObject)
        {
            GameObjectAdded?.Invoke(layer, gameObject);
        }

        private void OnGameObjectRemoved(SceneLayer layer, Vector2Int position)
        {
            GameObjectRemoved?.Invoke(layer, position);
        }

        private void OnGameObjectChangedPosition(SceneLayer layer, Vector2Int previousPosition, GameObject gameObject)
        {
            GameObjectChangedPosition?.Invoke(layer, previousPosition, gameObject);
        }
    }
}
