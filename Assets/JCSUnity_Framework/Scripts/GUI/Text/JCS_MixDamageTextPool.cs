﻿/**
 * $File: JCS_MixDamageTextPool.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{
    // Function pointer.
    public delegate int[] DamageTextSpawnerFP(int minDamage, int maxDamage, Vector2 pos, int hit, int percentOfCritical);


    public class JCS_MixDamageTextPool
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public enum DamageTextType
        {
            NORMAL,
            CRITICAL,
            GET_DAMAGE,
            HEAL
        };

        //----------------------
        // Private Variables

        [Header("NOTE: Plz put the whole set of Damage Text here!")]
        [SerializeField] private JCS_DamageTextPool mCritDamageTextPool = null;
        [SerializeField] private JCS_DamageTextPool mNormalDamageTextPool = null;
        [SerializeField] private JCS_DamageTextPool mGetDamageDamageTextPool = null;
        [SerializeField] private JCS_DamageTextPool mHealDamageTextPool = null;

        [Header("** Runtime Variables **")]
        [SerializeField] private float mSpacingPerText = 1;
        [SerializeField] private float mTimePerSpawn = 0.1f;
        private float mTimePerSpawnTimer = 0;

        [Header("** Sound (Audio can be set at the Text Pool) **\n N/A")]
        [Space(10)]

        [Header("** Zigge Right Left Effect (In Sequence)**")]
        [SerializeField] private bool mZiggeEffect = true;
        [SerializeField] private float mRightAlign = 1;
        [SerializeField] private float mLeftAlign = 1;


        private JCS_Vector<int> mSequenceThread = null;
        // Data we need to let Sequence Thread process!
        private JCS_Vector<int[]> mSequenceDamageData = null;
        private JCS_Vector<Vector2[]> mSequencePosData = null;
        private JCS_Vector<DamageTextType[]> mSequenceTypeData = null;

        // Utililty
        private JCS_Vector<float> mSequenceSpanwTimer = null;
        // IMPORTANT: index of number we want to call to spawn the damage text!
        private JCS_Vector<int> mSequenceSpawnCount = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        private JCS_DamageTextPool GetCriticalDamageTextPool() { return this.mCritDamageTextPool; }
        private JCS_DamageTextPool GetNormralDamageTextPool() { return this.mNormalDamageTextPool; }
        private JCS_DamageTextPool GetGetDamageDamageTextPool() { return this.mGetDamageDamageTextPool; }
        private JCS_DamageTextPool GetHealDamageTextPoll() { return this.mHealDamageTextPool; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {

            // spawn all the sequence
            mSequenceThread = new JCS_Vector<int>();
            mSequenceDamageData = new JCS_Vector<int[]>();
            mSequencePosData = new JCS_Vector<Vector2[]>();
            mSequenceTypeData = new JCS_Vector<DamageTextType[]>();
            mSequenceSpanwTimer = new JCS_Vector<float>();
            mSequenceSpawnCount = new JCS_Vector<int>();
        }

        private void Start()
        {
            // set to global!
            JCS_GameManager.instance.SetMixDamageTextPool(this);
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            ProccessSequences();
        }

        private void Test()
        {
            if (JCS_Input.GetKeyDown(KeyCode.N))
            {
                // Testing helper function so spawn sequence of damage
                // Showing u can get the damage from ite
                int[] damages = DamageTextSpawnerSimple(0, 1, new Vector2(0, 0), 6, 60);
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        
        /// <summary>
        /// Default Damaget Text Spawner with the defualt Random Algorithm!
        /// </summary>
        public int[] DamageTextSpawnerSimple(int minDamage, int maxDamage, Vector2 pos, int hit, int percentOfCritical)
        {
            return DamageTextSpawnerSimple(minDamage, maxDamage, pos, hit, percentOfCritical, JCS_UsefualFunctions.JCS_IntRange);
        }
        /// <summary>
        /// Helper function to spawn the damaget text
        /// so scripter will not have to do all the drug code over and
        /// over agian!
        /// </summary>
        /// <param name="minDamage"> minimum of damage we produce </param>
        /// <param name="maxDamage"> maximum of damage we produce </param>
        /// <param name="pos"> position of damage text we spawn </param>
        /// <param name="hit"> how many damage text we spawn </param>
        /// <param name="percentOfCritical"> how many percentage will be critical instead of normal damage text </param>
        /// <returns> data we produced </returns>
        public int[] DamageTextSpawnerSimple(int minDamage, int maxDamage, Vector2 pos, int hit, int percentOfCritical, JCS_Range algorithm)
        {
            if (minDamage > maxDamage)
            {
                JCS_GameErrors.JcsErrors("JCS_MixDamageTextPool", -1, "min damage cannot be higher or equal to the max damage!");
                return null;
            }

            if (minDamage < 0 || maxDamage < 0)
            {
                JCS_GameErrors.JcsErrors("JCS_MixDamageTextPool", -1, "Min or Max damage cannot be lower than 0!");
                return null;
            }

            if (percentOfCritical < 0 || percentOfCritical > 100)
            {
                JCS_GameErrors.JcsErrors("JCS_MixDamageTextPool", -1, "Percent Of Critical should within range of 0 ~ 100...");
                return null;
            }

            if (hit <= 0)
            {
                JCS_GameErrors.JcsErrors("JCS_MixDamageTextPool", -1, "Hit count should not be equal or lower than 0!");
                return null;
            }


            int[] damages = new int[hit];
            DamageTextType[] types = new DamageTextType[hit];

            for (int index = 0;
                index < hit;
                ++index)
            {
                int dm = Random.Range(minDamage, maxDamage);
                damages[index] = dm;

                bool isCritical = (algorithm(0, 100) < percentOfCritical);

                if (isCritical)
                    types[index] = DamageTextType.CRITICAL;
                else
                    types[index] = DamageTextType.NORMAL;
            }

            SpawnDamageTextsFromPoolByType(damages, pos, types);

            // return the damages we just create!
            return damages;
        }

        /// <summary>
        /// Spawn a Damaget Text form Pool by the corresponding type
        /// </summary>
        /// <param name="damage"> value of the damage text </param>
        /// <param name="pos"> position damage text </param>
        /// <param name="type"> type of the damage text (Default: Normal Damage Text) </param>
        public void SpawnDamageTextFromPoolByType(int damage, Vector2 pos, DamageTextType type = DamageTextType.NORMAL)
        {
            JCS_DamageTextPool dtp = GetDamageTextPoolByType(type);

            if (dtp != null)
                dtp.SpawnDamageTextFromPool(damage, pos);
        }
        public void SpawnDamageTextsFromPoolByType(int[] damage, Vector2 pos, DamageTextType[] types)
        {
            Vector2[] poses = new Vector2[damage.Length];
            for (int index = 0;
                index < poses.Length;
                ++index)
            {
                poses[index] = pos;
            }
            SpawnDamageTextsFromPoolByType(damage, poses, types);
        }
        public void SpawnDamageTextsFromPoolByType(int[] damage, Vector2[] pos, DamageTextType[] types)
        {
            if (damage.Length != pos.Length || 
                damage.Length !=types.Length)
            {
                JCS_GameErrors.JcsErrors("JCS_MixDamageTextPool", -1, "Wrong triple pair size!");
                return;
            }

            if (mZiggeEffect)
            {
                for (int count = 0;
                    count < pos.Length;
                    ++count)
                {
                    if ((count % 2) == 0)
                        pos[count].x += mRightAlign;
                    else
                        pos[count].x -= mLeftAlign;

                    pos[count].y += mSpacingPerText * count;
                }
            }

            // add thread!
            mSequenceThread.push(mSequenceThread.length);

            // update data to memory
            mSequenceDamageData.push(damage);
            mSequencePosData.push(pos);
            mSequenceTypeData.push(types);

            // simply add a timer!
            mSequenceSpanwTimer.push(0);

            // always start with the first index
            mSequenceSpawnCount.push(0);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private JCS_DamageTextPool GetDamageTextPoolByType(DamageTextType type)
        {

            switch (type)
            {
                case DamageTextType.NORMAL:
                    return this.GetNormralDamageTextPool();
                case DamageTextType.CRITICAL:
                    return this.GetCriticalDamageTextPool();
                case DamageTextType.GET_DAMAGE:
                    return this.GetGetDamageDamageTextPool();
                case DamageTextType.HEAL:
                    return this.GetHealDamageTextPoll();
            }

            return null;
        }

        /// <summary>
        /// The actual thread is doing here.
        /// </summary>
        /// <param name="processIndex"> the unique id of the thread </param>
        /// <param name="damage"> memory data thread needed </param>
        /// <param name="pos"> memory data thread needed </param>
        /// <param name="timer"> memory data thread needed </param>
        private void Sequence(int processIndex, int[] damage, Vector2[] pos, DamageTextType[] types, float timer)
        {
            float newTimer = timer;

            newTimer += Time.deltaTime;

            if (mTimePerSpawn < newTimer)
            {
                int count = mSequenceSpawnCount.at(processIndex);
                if (count == damage.Length)
                {
                    // done the sequence, do delete the process(thread)
                    EndProcessSequence(processIndex);
                    return;
                }

                // spawn that specific damage text!
                SpawnDamageTextFromPoolByType(damage[count], pos[count], types[count]);

                ++count;
                // update new count, in order 
                // to spawn next damage text
                mSequenceSpawnCount.set(processIndex, count);
                newTimer = 0;
            }


            // update timer
            mSequenceSpanwTimer.set(processIndex, newTimer);
        }
        private void ProccessSequences()
        {
            for (int process = 0;
                process < mSequenceThread.length;
                ++process)
            {
                // pass in all the data wee need in order to process the data
                Sequence(process,
                    mSequenceDamageData.at(process),
                    mSequencePosData.at(process),
                    mSequenceTypeData.at(process),
                    mSequenceSpanwTimer.at(process));
            }
        }
        private void EndProcessSequence(int processIndex)
        {
            mSequenceThread.slice(processIndex);
            mSequenceDamageData.slice(processIndex);
            mSequencePosData.slice(processIndex);
            mSequenceTypeData.slice(processIndex);
            mSequenceSpanwTimer.slice(processIndex);
            mSequenceSpawnCount.slice(processIndex);
        }

    }
}
