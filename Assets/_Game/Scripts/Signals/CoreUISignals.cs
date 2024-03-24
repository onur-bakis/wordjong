using Scripts.Enums;
using UnityEngine.Events;

namespace Scripts.Context.Signals
{
    public class CoreUISignals 
    {
        public UnityAction<UIPanelTypes> onOpenPanel = delegate { };
    }
}