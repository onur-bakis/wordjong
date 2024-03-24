using Scripts.Keys;
using UnityEngine.Events;

namespace Scripts.Context.Signals
{
    public class CoreGameSignals
    {
        public UnityAction<byte> onPlayGameInitialize = delegate { };
        public UnityAction<LevelFinishParams> onLevelFinished = delegate { };
        public UnityAction onTapToContinue = delegate { };
        public UnityAction onPlay = delegate { };
        public UnityAction onReset = delegate { };
    }
}