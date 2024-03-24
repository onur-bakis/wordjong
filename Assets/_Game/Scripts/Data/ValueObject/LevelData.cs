using System;

namespace Scripts.Data.ValueObject
{
    [Serializable]
    public struct LevelData
    {
        public string title;
        public TileData[] tiles;
    }
}