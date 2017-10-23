/**
 * $File: JCS_ButtonSelectionGroupController.cs $
 * $Date: 2017-10-07 14:58:41 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{
    /// <summary>
    /// Control the 'JCS_ButtonSelectionGroup' class with certain ket input, 
    /// without that class this class is meaningless.
    /// </summary>
    [RequireComponent(typeof(JCS_ButtonSelectionGroup))]
    public class JCS_ButtonSelectionGroupController
        : MonoBehaviour
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/
        private JCS_ButtonSelectionGroup mButtonSelectionGroup = null;


        [Header("** Runtime Variables (JCS_ButtonSelectionGroupController) **")]

        [Tooltip("Active key listener?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("JCSUnity key would not work with game pause, ignore it?")]
        [SerializeField]
        private bool mIgnoreGamePause = false;

        [Tooltip("")]
        [SerializeField]
        private JCS_KeyActionType mKeyActionType = JCS_KeyActionType.KEY_DOWN;


        [Header("- Keyboard Settings (JCS_ButtonSelectionGroupController)")]

        [Tooltip("Key for next selection. (Keyboard)")]
        [SerializeField]
        private KeyCode mMNext = KeyCode.None;

        [Tooltip("Key for previous selection. (Keyboard)")]
        [SerializeField]
        private KeyCode mMPrev = KeyCode.None;

        [Tooltip("Okay for this selection. (Keyboard)")]
        [SerializeField]
        private KeyCode mMOkay = KeyCode.None;

        [Header("- Full Control Settings / Keyboard (JCS_ButtonSelectionGroupController)")]

        [Tooltip("Up select key.")]
        [SerializeField]
        private KeyCode mMUp = KeyCode.None;

        [Tooltip("Down select key.")]
        [SerializeField]
        private KeyCode mMDown = KeyCode.None;

        [Tooltip("Right select key.")]
        [SerializeField]
        private KeyCode mMRight = KeyCode.None;

        [Tooltip("Left select key.")]
        [SerializeField]
        private KeyCode mMLeft = KeyCode.None;


        [Header("- Game Pad Settings (JCS_ButtonSelectionGroupController)")]

        [Tooltip("Which device we are listening to?")]
        [SerializeField]
        private JCS_JoystickIndex mGamePadId = JCS_JoystickIndex.FROM_ALL_JOYSTICK;

        [Tooltip("Next selection. (Game Pad)")]
        [SerializeField]
        private JCS_JoystickButton mJNext = JCS_JoystickButton.NONE;

        [Tooltip("Previous selection. (Game Pad)")]
        [SerializeField]
        private JCS_JoystickButton mJPrev = JCS_JoystickButton.NONE;


        [Tooltip("Okay for this selection. (Game Pad)")]
        [SerializeField]
        private JCS_JoystickButton mJOkay = JCS_JoystickButton.NONE;


        [Header("- Full Control Settings / Game Pad (JCS_ButtonSelectionGroupController)")]

        [Tooltip("Up select key.")]
        [SerializeField]
        private JCS_JoystickButton mJUp = JCS_JoystickButton.NONE;

        [Tooltip("Down select key.")]
        [SerializeField]
        private JCS_JoystickButton mJDown = JCS_JoystickButton.NONE;

        [Tooltip("Right select key.")]
        [SerializeField]
        private JCS_JoystickButton mJRight = JCS_JoystickButton.NONE;

        [Tooltip("Left select key.")]
        [SerializeField]
        private JCS_JoystickButton mJLeft = JCS_JoystickButton.NONE;


        [Header("- Audio Settings (JCS_ButtonSelectionGroupController)")]

        [Tooltip("Sound when next button is pressed.")]
        [SerializeField]
        private AudioClip mNextSound = null;

        [Tooltip("Sound when prev button is pressed.")]
        [SerializeField]
        private AudioClip mPrevSound = null;

        [Tooltip("Sound when okay button is pressed.")]
        [SerializeField]
        private AudioClip mOkaySound = null;

        [Tooltip("Sound when up button is pressed.")]
        [SerializeField]
        private AudioClip mUpSound = null;

        [Tooltip("Sound when down button is pressed.")]
        [SerializeField]
        private AudioClip mDownSound = null;

        [Tooltip("Sound when right button is pressed.")]
        [SerializeField]
        private AudioClip mRightSound = null;

        [Tooltip("Sound when left button is pressed.")]
        [SerializeField]
        private AudioClip mLeftSound = null;


        [Tooltip("Sound volumne.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mNextSoundVolume = 1.0f;

        [Tooltip("Sound volumne.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mPrevSoundVolume = 1.0f;

        [Tooltip("Sound volumne.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mOkaySoundVolume = 1.0f;

        [Tooltip("Sound volumne.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mUpSoundVolume = 1.0f;

        [Tooltip("Sound volumne.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mDownSoundVolume = 1.0f;

        [Tooltip("Sound volumne.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mRightSoundVolume = 1.0f;

        [Tooltip("Sound volumne.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mLeftSoundVolume = 1.0f;

        [Tooltip("Method to play the sound.")]
        [SerializeField]
        private JCS_SoundMethod mNextSoundMethod = JCS_SoundMethod.PLAY_SOUND;

        [Tooltip("Method to play the sound.")]
        [SerializeField]
        private JCS_SoundMethod mPrevSoundMethod = JCS_SoundMethod.PLAY_SOUND;

        [Tooltip("Method to play the sound.")]
        [SerializeField]
        private JCS_SoundMethod mOkaySoundMethod = JCS_SoundMethod.PLAY_SOUND;

        [Tooltip("Method to play the sound.")]
        [SerializeField]
        private JCS_SoundMethod mUpSoundMethod = JCS_SoundMethod.PLAY_SOUND;

        [Tooltip("Method to play the sound.")]
        [SerializeField]
        private JCS_SoundMethod mDownSoundMethod = JCS_SoundMethod.PLAY_SOUND;

        [Tooltip("Method to play the sound.")]
        [SerializeField]
        private JCS_SoundMethod mRightSoundMethod = JCS_SoundMethod.PLAY_SOUND;

        [Tooltip("Method to play the sound.")]
        [SerializeField]
        private JCS_SoundMethod mLeftSoundMethod = JCS_SoundMethod.PLAY_SOUND;


        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public bool IgnoreGamePause { get { return this.mIgnoreGamePause; } set { this.mIgnoreGamePause = value; } }

        public AudioClip NextSound { get { return this.mNextSound; } set { this.mNextSound = value; } }
        public AudioClip PrevSound { get { return this.mPrevSound; } set { this.mPrevSound = value; } }
        public AudioClip OkaySound { get { return this.mOkaySound; } set { this.mOkaySound = value; } }
        public AudioClip UpSound { get { return this.mUpSound; } set { this.mUpSound = value; } }
        public AudioClip DownSound { get { return this.mDownSound; } set { this.mDownSound = value; } }
        public AudioClip RightSound { get { return this.mRightSound; } set { this.mRightSound = value; } }
        public AudioClip LeftSound { get { return this.mLeftSound; } set { this.mLeftSound = value; } }

        public float NextSoundVolume { get { return this.mNextSoundVolume; } set { this.mNextSoundVolume = value; } }
        public float PrevSoundVolume { get { return this.mPrevSoundVolume; } set { this.mPrevSoundVolume = value; } }
        public float OkaySoundVolume { get { return this.mOkaySoundVolume; } set { this.mOkaySoundVolume = value; } }
        public float UpSoundVolume { get { return this.mUpSoundVolume; } set { this.mUpSoundVolume = value; } }
        public float DownSoundVolume { get { return this.mDownSoundVolume; } set { this.mDownSoundVolume = value; } }
        public float RightSoundVolume { get { return this.mRightSoundVolume; } set { this.mRightSoundVolume = value; } }
        public float LeftSoundVolume { get { return this.mLeftSoundVolume; } set { this.mLeftSoundVolume = value; } }

        public JCS_SoundMethod NextSoundMethod { get { return this.mNextSoundMethod; } set { this.mNextSoundMethod = value; } }
        public JCS_SoundMethod PrevSoundMethod { get { return this.mPrevSoundMethod; } set { this.mPrevSoundMethod = value; } }
        public JCS_SoundMethod OkaySoundMethod { get { return this.mOkaySoundMethod; } set { this.mOkaySoundMethod = value; } }
        public JCS_SoundMethod UpSoundMethod { get { return this.mUpSoundMethod; } set { this.mUpSoundMethod = value; } }
        public JCS_SoundMethod DownSoundMethod { get { return this.mDownSoundMethod; } set { this.mDownSoundMethod = value; } }
        public JCS_SoundMethod RightSoundMethod { get { return this.mRightSoundMethod; } set { this.mRightSoundMethod = value; } }
        public JCS_SoundMethod LeftSoundMethod { get { return this.mLeftSoundMethod; } set { this.mLeftSoundMethod = value; } }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void Awake()
        {
            this.mButtonSelectionGroup = this.GetComponent<JCS_ButtonSelectionGroup>();
        }

        private void Update()
        {
            if (!mActive)
                return;

            /* Basic Control (One Dimensional) */
            {
                if (ActiveNext())
                {
                    mButtonSelectionGroup.NextSelection();
                    PlayNextSound();
                }
                if (ActivePrev())
                {
                    mButtonSelectionGroup.PrevSelection();
                    PlayPrevSound();
                }
            }

            if (ActiveOkay())
            {
                mButtonSelectionGroup.OkaySelection();
                PlayOkaySound();
            }

            /* Full Contol (Two Dimensional) */
            {
                if (ActiveUp())
                {
                    mButtonSelectionGroup.UpSelection();
                    PlayUpSound();
                }
                if (ActiveDown())
                {
                    mButtonSelectionGroup.DownSelection();
                    PlayDownSound();
                }
                if (ActiveRight())
                {
                    mButtonSelectionGroup.RightSelection();
                    PlayRightSound();
                }
                if (ActiveLeft())
                {
                    mButtonSelectionGroup.LeftSelection();
                    PlayLeftSound();
                }
            }
        }

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Active next selection? (Key Depends)
        /// </summary>
        /// <returns></returns>
        private bool ActiveNext()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMNext, mIgnoreGamePause) || 
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJNext, mIgnoreGamePause);
        }

        /// <summary>
        /// Active previous selection? (Key Depends)
        /// </summary>
        /// <returns></returns>
        private bool ActivePrev()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMPrev, mIgnoreGamePause) || 
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJPrev, mIgnoreGamePause);
        }

        /// <summary>
        /// Active okay selection? (Key Depends)
        /// </summary>
        /// <returns></returns>
        private bool ActiveOkay()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMOkay, mIgnoreGamePause) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJOkay, mIgnoreGamePause);
        }

        /// <summary>
        /// Acitve up selection.
        /// </summary>
        /// <returns></returns>
        private bool ActiveUp()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMUp, mIgnoreGamePause) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJUp, mIgnoreGamePause);
        }

        /// <summary>
        /// Acitve up selection.
        /// </summary>
        /// <returns></returns>
        private bool ActiveDown()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMDown, mIgnoreGamePause) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJDown, mIgnoreGamePause);
        }

        /// <summary>
        /// Acitve up selection.
        /// </summary>
        /// <returns></returns>
        private bool ActiveRight()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMRight, mIgnoreGamePause) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJRight, mIgnoreGamePause);
        }

        /// <summary>
        /// Acitve up selection.
        /// </summary>
        /// <returns></returns>
        private bool ActiveLeft()
        {
            return JCS_Input.GetKeyByAction(mKeyActionType, mMLeft, mIgnoreGamePause) ||
                JCS_Input.GetJoystickKeyByAction(mKeyActionType, mGamePadId, mJLeft, mIgnoreGamePause);
        }

        /* Play the next sound. */
        private void PlayNextSound()
        {
            if (mNextSound == null)
                return;

            JCS_SoundManager.instance.GetGlobalSoundPlayer().
                PlayOneShotByMethod(mNextSound, mNextSoundMethod, mNextSoundVolume);
        }

        /* Play the prev sound. */
        private void PlayPrevSound()
        {
            if (mPrevSound == null)
                return;

            JCS_SoundManager.instance.GetGlobalSoundPlayer().
                PlayOneShotByMethod(mPrevSound, mPrevSoundMethod, mPrevSoundVolume);
        }

        /* Play the okay sound. */
        private void PlayOkaySound()
        {
            if (mOkaySound == null)
                return;

            JCS_SoundManager.instance.GetGlobalSoundPlayer().
                PlayOneShotByMethod(mOkaySound, mOkaySoundMethod, mOkaySoundVolume);
        }

        /* Play the up sound. */
        private void PlayUpSound()
        {
            if (mUpSound == null)
                return;

            JCS_SoundManager.instance.GetGlobalSoundPlayer().
                PlayOneShotByMethod(mUpSound, mUpSoundMethod, mUpSoundVolume);
        }

        /* Play the down sound. */
        private void PlayDownSound()
        {
            if (mDownSound == null)
                return;

            JCS_SoundManager.instance.GetGlobalSoundPlayer().
                PlayOneShotByMethod(mDownSound, mDownSoundMethod, mDownSoundVolume);
        }

        /* Play the right sound. */
        private void PlayRightSound()
        {
            if (mRightSound == null)
                return;

            JCS_SoundManager.instance.GetGlobalSoundPlayer().
                PlayOneShotByMethod(mRightSound, mRightSoundMethod, mRightSoundVolume);
        }

        /* Play the left sound. */
        private void PlayLeftSound()
        {
            if (mLeftSound == null)
                return;

            JCS_SoundManager.instance.GetGlobalSoundPlayer().
                PlayOneShotByMethod(mLeftSound, mLeftSoundMethod, mLeftSoundVolume);
        }
    }
}
