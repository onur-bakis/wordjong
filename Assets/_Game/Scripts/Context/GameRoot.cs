using strange.extensions.context.impl;

namespace Scripts.Context
{
    public class GameRoot : ContextView
    {
        public static GameRoot Instance;
    
        private void Awake()
        {
            Instance = this;
            context = new GameContext(this);
        }
    }
}