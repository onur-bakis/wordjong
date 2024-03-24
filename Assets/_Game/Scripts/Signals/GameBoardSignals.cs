using Scripts.Keys;
using UnityEngine.Events;

namespace Scripts.Signals
{
    public class GameBoardSignals
    {
        public UnityAction<bool> onUndoAvailable = delegate { };
        public UnityAction<WordCheckParams> onCurrentWordChange = delegate { };
        public UnityAction<int> onTopScoreChange = delegate { };
        public UnityAction<string> onNewWordAdded = delegate { };

    }
}