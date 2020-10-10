using UnityEngine;

namespace CC
{
    public class CCLauncher : MonoBehaviour
    {
        private static CCLauncher instance;
        private CCLaunchScreen loadScreen;
        //public GameObject loadScreenRes;
        //public float launchDelayTime;
        public GameObject FirstUI;

        public static CCLauncher GetInstance()
        {
            return instance;
        }

        void Awake()
        {
            if (CCGame.IsRunning())
            {
                CCGame.Begin();
                return;
            }

            DontDestroyOnLoad(gameObject);
            instance = this;

            InitDefaulSet();
            CCGame.Init();
            //ShowLoadScreen();
        }

        void InitDefaulSet()
        {
            Application.targetFrameRate = AnimManager.FPS;
        }

        //void ShowLoadScreen()
        //{
        //    if (loadScreenRes == null) return;

        //    loadScreen = Instantiate(loadScreenRes).GetOrAddComponent<CCLaunchScreen>();
        //    loadScreen.transform.SetParent(GuiSystem.MaskHelper.transform, false);
        //    loadScreen.delayTime = launchDelayTime;
        //}
    }
}