using System;
using UnityEngine;

namespace Scripts.Data.ValueObject
{
    [Serializable]
    public struct TileData
    {
        public int id;
        public Vector3 position;
        public string character;
        public int[] children;
    }
}