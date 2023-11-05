using ConsoleRoguelike.CoreModule;
using ConsoleRoguelike.GameObjects;

namespace ConsoleRoguelike.GameScene
{
    internal interface IReadOnlyScene
    {
        public event Action<SceneLayer, Vector2Int, GameObject> GameObjectChangedPosition;
        public event Action<SceneLayer, GameObject> GameObjectAdded;
        public event Action<SceneLayer, Vector2Int> GameObjectRemoved;

        public int LayersCount { get; }

        public SceneLayer GetLayer(int layerNumber, bool createIfNotExist = true);
        public int GetLayerNumber(SceneLayer layer);
        public GameObject GetHighestGameObjectOnPosition(Vector2Int position);
        public SceneLayer GetHighestLayer();
        public List<SceneLayer> GetLayersThatHigher(SceneLayer layer);
        public List<SceneLayer> GetLayersAscending();
        public GameObject GetBottomLeftGameObject();
    }
}
