using UnityEngine;

namespace Tools
{
    public static class ScreenSizes
    {
        private static bool _initialized;

        public static float TopOffset { get; private set; }
        public static float BottomOffset { get; private set; }
        public static float ScreenHeight { get; private set; }
        public static float ScreenWidth { get; private set; }
        
    #if UNITY_EDITOR
        private const float TEST_TOP_OFFSET = 0;
        private const float TEST_BOTTOM_OFFSET = 0;
    #endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            if (_initialized) return;

            Rect safeArea = Screen.safeArea;

    #if UNITY_EDITOR
            ScreenHeight = safeArea.height;
            ScreenWidth = safeArea.width;
    #else
            ScreenHeight = Screen.currentResolution.height;
            ScreenWidth = Screen.currentResolution.width;
    #endif
            
    #if UNITY_EDITOR
            safeArea.height = safeArea.height - TEST_TOP_OFFSET - TEST_BOTTOM_OFFSET;
            safeArea.center = new Vector2(safeArea.x, safeArea.center.y + TEST_BOTTOM_OFFSET);
    #endif
            
            TopOffset = ScreenHeight - safeArea.yMax;
            BottomOffset = safeArea.yMin;
            
            _initialized = true;
        }
    }
}