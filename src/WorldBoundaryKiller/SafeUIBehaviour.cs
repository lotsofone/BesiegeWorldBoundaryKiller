using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using UnityEngine.SceneManagement;

namespace WorldBoundaryKiller
{

    //v0.3.4
    abstract class SafeUIBehaviour : MonoBehaviour
    {
        public bool showGUI = true;
        public Rect windowRect;
        public int windowID { get; protected set; } = ModUtility.GetWindowId();
        public string windowName;
        GameObject background;
        protected virtual void Awake()
        {
            windowRect = InitialWindowRect();
            if (windowRect.x > Screen.width - 10) windowRect.x = Screen.width - 80;
            if (windowRect.y > Screen.height - 10) windowRect.y = Screen.height - 40;
            windowName = InitialWindowName();
            this.background = new GameObject("UIBackGround");
            background.transform.parent = gameObject.transform;
            background.layer = 13;
            background.AddComponent<BoxCollider>();
        }
        void OnGUI()
        {
            if (GameObject.Find("HUD Cam") == null) return;

            if (ShouldShowGUI())
            {
                if (!background.activeSelf)
                    background.SetActive(true);
                Camera hudCamera = GameObject.Find("HUD Cam").GetComponent<Camera>();
                Vector3 leftTop = hudCamera.ScreenPointToRay(new Vector3(windowRect.xMin, hudCamera.pixelHeight - windowRect.yMin, 0)).origin;
                Vector3 rightButtom = hudCamera.ScreenPointToRay(new Vector3(windowRect.xMax, hudCamera.pixelHeight - windowRect.yMax, 0)).origin;

                Vector3 pos = (leftTop + rightButtom) / 2; pos.z += 0.3f;
                background.transform.position = pos;
                Vector3 sca = rightButtom - leftTop; sca.z = 0.1f;
                sca.x = Mathf.Abs(sca.x); sca.y = Mathf.Abs(sca.y);
                background.transform.localScale = sca;



                this.windowRect = GUILayout.Window(this.windowID, this.windowRect, new GUI.WindowFunction(this.WindowContent), this.windowName);
            }
            else
            {
                if (background.activeSelf)
                    background.SetActive(false);
            }
        }
        public bool IsPlaying()
        {
            List<string> scene = new List<string> { "INITIALISER", "TITLE SCREEN", "LevelSelect", "LevelSelect1", "LevelSelect2", "LevelSelect3" };

            if (SceneManager.GetActiveScene().isLoaded)
            {

                if (scene.Exists(match => match == SceneManager.GetActiveScene().name))
                {
                    return false;
                }
            }
            return true;
        }
        protected virtual void OnDisable()
        {
            background.SetActive(false);
        }
        protected virtual void OnDestroy()
        {
            GameObject.Destroy(this.background);
        }
        protected abstract void WindowContent(int windowID);
        public abstract bool ShouldShowGUI();
        public abstract string InitialWindowName();
        public abstract Rect InitialWindowRect();
    }

}
