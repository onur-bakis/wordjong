using UnityEngine.Events;

namespace Scripts.Signals
{
    public class GameUISignals
    {
        public UnityAction onSubmit = delegate { };
        public UnityAction onUndo = delegate { };
    }
}