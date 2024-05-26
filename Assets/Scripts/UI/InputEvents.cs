using UnityEngine.Events;

namespace GMatch3.UI
{
    public static class InputEvents
    {
        public static UnityAction PlayEvent;
        public static UnityAction ExitEvent;
        public static UnityAction MenuEvent;
        public static UnityAction<int> LoadLevelEvent;
        public static UnityAction RestartLevelEvent;
        public static UnityAction CompletedLevelEvent;
    }
}
