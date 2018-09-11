/**
 * $File: JCS_ScreenSettings.cs $
 * $Date: 2018-09-08 15:15:28 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{
    public delegate void OnScreenResize();

    /// <summary>
    /// Screen related settings.
    /// </summary>
    public class JCS_ScreenSettings
        : JCS_Settings<JCS_ScreenSettings>
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        public OnScreenResize onScreenResize = null;


#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_ScreenManager) **")]

        public float REAL_SCREEN_WIDTH = 0.0f;

        public float REAL_SCREEN_HEIGHT = 0.0f;
#endif


        [Header("** Check Variables (JCS_ScreenSettings) **")]

        [Tooltip("When the application start, what's the screen width?")]
        public int STARTING_SCREEN_WIDTH = 0;

        [Tooltip("When the application start, what's the screen height?")]
        public int STARTING_SCREEN_HEIGHT = 0;

        [Tooltip("Store the camera orthographic size value over scene.")]
        public float ORTHOGRAPHIC_SIZE = 10.0f;

        [Tooltip("Store the camera filed of view value over scene.")]
        public float FIELD_OF_VIEW = 90.0f;

        [Tooltip("Current screen width.")]
        public float CURRENT_SCREEN_WIDTH = 0.0f;

        [Tooltip("Current screen height.")]
        public float CURRENT_SCREEN_HEIGHT = 0.0f;

        [Tooltip("Previous screen width.")]
        public float PREV_SCREEN_WIDTH = 0.0f;

        [Tooltip("Previous screen height.")]
        public float PREV_SCREEN_HEIGHT = 0.0f;


        [Header("- Resize UI (JCS_ScreenSettings)")]

        [Tooltip("Record down the previous 'mWScale' value.")]
        public float PREV_W_SCALE = 1.0f;

        [Tooltip("Record down the previous 'mHScale' value.")]
        public float PREV_H_SCALE = 1.0f;


        [Header("** Initialize Variables (JCS_ScreenSettings) **")]

        [Tooltip("Resize the screen/window to certain aspect when " +
            "the application starts. Aspect ratio can be set at " +
            "'JCS_ScreenManager'.")]
        public bool RESIZE_TO_ASPECT_WHEN_APP_STARTS = true;


        [Header("** Runtime Variables (JCS_ScreenSettings) **")]

        [Tooltip("Type of the screen handle.")]
        public JCS_ScreenType SCREEN_TYPE = JCS_ScreenType.RESIZABLE;

        [Tooltip("Target aspect ratio screen width.")]
        [SerializeField]
        public int ASPECT_RATIO_SCREEN_WIDTH = 16;

        [Tooltip("Target aspect ratio screen height.")]
        [SerializeField]
        public int ASPECT_RATIO_SCREEN_HEIGHT = 9;


        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void Awake()
        {
            instance = CheckSingleton(instance, this);

            // This will only run once at the time when 
            // the application is starts.
            if (!JCS_ApplicationSettings.instance.APPLICATION_STARTS)
            {
                if (RESIZE_TO_ASPECT_WHEN_APP_STARTS)
                {
                    // Force resize screen/window to certain aspect
                    // ratio once.
                    ForceAspectScreenOnce();
                }

                // Record down the starting screen width and screen height.
                STARTING_SCREEN_WIDTH = Screen.width;
                STARTING_SCREEN_HEIGHT = Screen.height;
            }
        }

        private void Start()
        {
            // NOTE(jenchieh): Here is the execution order implementation.
            // 'APPLICATION_STARTS' will be true after the first scene's 
            // main game loop is runs. 
            if (JCS_ApplicationSettings.instance.APPLICATION_STARTS)
            {
                Camera cam = JCS_Camera.main.GetCamera();
                cam.fieldOfView = FIELD_OF_VIEW;
                cam.orthographicSize = ORTHOGRAPHIC_SIZE;
            }
        }

        private void LateUpdate()
        {
#if (UNITY_EDITOR)
            REAL_SCREEN_WIDTH = Screen.width;
            REAL_SCREEN_HEIGHT = Screen.height;
#endif

            DoScreenType();
        }

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        /// <summary>
        /// Return width of the blackspace on the screen, if any 
        /// after resizing the screen.
        /// </summary>
        /// <returns></returns>
        public float BlackspaceWidth()
        {
            return Screen.width - STARTING_SCREEN_WIDTH;
        }

        /// <summary>
        /// Return height of the blackspace on the screen, if any 
        /// after resizing the screen.
        /// </summary>
        /// <returns></returns>
        public float BlackspaceHeight()
        {
            return Screen.height - STARTING_SCREEN_HEIGHT;
        }

        /// <summary>
        /// Get the visible of the screen width.
        /// </summary>
        /// <returns></returns>
        public float VisibleScreenWidth()
        {
            return Screen.width - BlackspaceWidth();
        }

        /// <summary>
        /// Get the size of the screen height.
        /// </summary>
        /// <returns></returns>
        public float VisibleScreenHeight()
        {
            return Screen.height - BlackspaceHeight();
        }

        //----------------------
        // Protected Functions

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected override void TransferData(JCS_ScreenSettings _old, JCS_ScreenSettings _new)
        {
            _new.SCREEN_TYPE = _old.SCREEN_TYPE;

            _new.ORTHOGRAPHIC_SIZE = _old.ORTHOGRAPHIC_SIZE;
            _new.FIELD_OF_VIEW = _old.FIELD_OF_VIEW;

            _new.STARTING_SCREEN_WIDTH = _old.STARTING_SCREEN_WIDTH;
            _new.STARTING_SCREEN_HEIGHT = _old.STARTING_SCREEN_HEIGHT;

            _new.PREV_W_SCALE = _old.PREV_W_SCALE;
            _new.PREV_H_SCALE = _old.PREV_H_SCALE;
        }

        //----------------------
        // Private Functions

        /// <summary>
        /// Make the screen in certain aspect ratio.
        /// </summary>
        private void ForceAspectScreenOnce()
        {
            int width = Screen.width;
            int height = Screen.height;

            // update the height
            float heightAccordingToWidth = width / ASPECT_RATIO_SCREEN_WIDTH * ASPECT_RATIO_SCREEN_HEIGHT;
            Screen.SetResolution(width, (int)Mathf.Round(heightAccordingToWidth), false, 0);

            // update the width
            float widthAccordingToHeight = height / ASPECT_RATIO_SCREEN_HEIGHT * ASPECT_RATIO_SCREEN_WIDTH;
            Screen.SetResolution((int)Mathf.Round(widthAccordingToHeight), height, false, 0);
        }

        /// <summary>
        /// Do the task base on the screen type handle.
        /// </summary>
        private void DoScreenType()
        {
            switch (SCREEN_TYPE)
            {
                case JCS_ScreenType.FORCE_ASPECT:
                    DoFoceAspectScreen();
                    break;
                case JCS_ScreenType.RESIZABLE:
                    DoResizableScreen();
                    break;
            }
        }

        /// <summary>
        /// Force aspect window.
        /// 
        /// SOURCE: https://gamedev.stackexchange.com/questions/86707/how-to-lock-aspect-ratio-when-resizing-game-window-in-unity
        /// AUTHOR: Entity in JavaScript.
        /// Modefied: Jen-Chieh Shen to C#.
        /// </summary>
        private void DoFoceAspectScreen()
        {
            if (Screen.fullScreen)
                return;

            JCS_ScreenSettings ss = JCS_ScreenSettings.instance;

            int width = Screen.width;
            int height = Screen.height;

            // if the user is changing the width
            if (PREV_SCREEN_WIDTH != width)
            {
                // update the height
                float heightAccordingToWidth = width / ss.ASPECT_RATIO_SCREEN_WIDTH * ss.ASPECT_RATIO_SCREEN_HEIGHT;
                Screen.SetResolution(width, (int)Mathf.Round(heightAccordingToWidth), false, 0);
            }

            // if the user is changing the height
            if (PREV_SCREEN_HEIGHT != height)
            {
                // update the width
                float widthAccordingToHeight = height / ss.ASPECT_RATIO_SCREEN_HEIGHT * ss.ASPECT_RATIO_SCREEN_WIDTH;
                Screen.SetResolution((int)Mathf.Round(widthAccordingToHeight), height, false, 0);
            }

            this.PREV_SCREEN_WIDTH = width;
            this.PREV_SCREEN_HEIGHT = height;
        }

        /// <summary>
        /// Do the resizable window.
        /// </summary>
        private void DoResizableScreen()
        {
            int screenWidth = Screen.width;
            int screenHeight = Screen.height;

            if (CURRENT_SCREEN_WIDTH == screenWidth &&
                CURRENT_SCREEN_HEIGHT == screenHeight)
                return;

            if (PREV_SCREEN_WIDTH == 0.0f || PREV_SCREEN_HEIGHT == 0.0f)
            {
                // If zero, set to the same value.
                PREV_SCREEN_WIDTH = screenWidth;
                PREV_SCREEN_HEIGHT = screenHeight;
            }
            else
            {
                // Record previous screen info.
                PREV_SCREEN_WIDTH = CURRENT_SCREEN_WIDTH;
                PREV_SCREEN_HEIGHT = CURRENT_SCREEN_HEIGHT;
            }

            // Update current screen info.
            CURRENT_SCREEN_WIDTH = screenWidth;
            CURRENT_SCREEN_HEIGHT = screenHeight;

            // Do callback.
            if (onScreenResize != null)
                onScreenResize.Invoke();
        }
    }
}
