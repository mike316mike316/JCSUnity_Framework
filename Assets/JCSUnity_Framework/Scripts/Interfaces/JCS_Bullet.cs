﻿/**
 * $File: JCS_Bullet.cs $
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

    public abstract class JCS_Bullet
        : MonoBehaviour
    {
        [SerializeField] private float mMoveSpeed = 10.0f;

        public float MoveSpeed { get { return this.mMoveSpeed; } set { this.mMoveSpeed = value; } }
    }
}
