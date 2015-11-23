using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using xy3d.tstd.lib.superTween;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleHeroHpBarUnit
    {
        private const float hpChangeTime = 1;//血条变化需要时间

        public Vector3 pos = new Vector3();
        public Vector3 scale = new Vector3(1, 1, 1);
        public Quaternion rotation = Quaternion.Euler(0, 0, 0);  
        private Matrix4x4 matrix = new Matrix4x4();

        private Vector4 posVec = new Vector4();
        private Vector4 stateInfoVec = new Vector4();
        private Vector4 fixVec = new Vector4();

        private Vector4 scaleVec4 = new Vector4(1, 1, 1, 1);

        public bool show = false;

        private float alpha = 0;

        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        private int state;

        public int State
        {
            get { return state; }
            set { state = value; }
        }

        private bool isChange = true;

        public bool IsChange
        {
            get { return isChange; }
            set { isChange = value; }
        }

        public float angerUFix;
        public float angerXFix;

//        private float angerNum;

        public float hpUFix;
        public float hpXFix;

//        private float targetHp;
        private float _hp;
        private float maxHp;

        private GameObject go;

        public BattleHeroHpBarUnit()
        {

        }

        public float Hp
        {
            get
            {
                return _hp;
            }
            set
            {
                hpUFix = (value / maxHp - 1) * BattleHeroHpBar.hpBarWidth / BattleHeroHpBar.TEXTURE_WIDTH;
                hpXFix = (value / maxHp - 1) * BattleHeroHpBar.hpBarWidth;

                _hp = value;
            }
        }

        public void SetHp(float _targetHp)
        {
            show = true;
            Alpha = 1;
            int id = SuperTween.Instance.To(Hp, _targetHp, hpChangeTime, UpdateHp, UpdateHpEnd);
            SuperTween.Instance.SetTag(id, "battle_tag");
        }

        private void UpdateHpEnd()
        {
            show = false;
        }

        private void UpdateHp(float value)
        {
            Hp = value;
        }

        public void Init(float _nowhp, float _maxHp, int _nowAnger, GameObject _go)
        {
            maxHp = _maxHp;
            Hp = _nowhp;
            go = _go;
            UpdateAnger(_nowAnger);
        }

        public Vector4 GetPositionsVec()
        {
           
            if (go != null)
            {
                posVec.x = go.transform.position.x;
                posVec.y = go.transform.position.y + (1.2f * Math.Abs(go.transform.localScale.x));
                posVec.z = go.transform.position.z;
                posVec.w = 1;
            }
            return posVec;
        }

        public Vector4 GetStateInfoVec()
        {
            float tempAlpha = Alpha;
            if ((!BattleHeroHpBar.Instance.showForce) && (!show))
            {
                tempAlpha = 0;
            }
            stateInfoVec.x = tempAlpha;
            stateInfoVec.y = State;
            return stateInfoVec;
        }

        public Matrix4x4 GetMatrix()
        {
            
            if (go != null)
            {
                //rotation = Quaternion.Euler(go.transform.eulerAngles.x, go.transform.eulerAngles.y, go.transform.eulerAngles.z);
                scale.x = Math.Abs(go.transform.localScale.x);
                scale.y =  Math.Abs(go.transform.localScale.y);
                scale.z =  Math.Abs(go.transform.localScale.z);
                matrix.SetTRS(pos, rotation, scale);
            }
            return matrix;
        }

        public Vector4 GetFixVec()
        {
            if (go != null)
            {
                fixVec.x = hpUFix;
                fixVec.y = hpXFix;
                fixVec.z = angerUFix;
                fixVec.w = angerXFix;
            }
            return fixVec;
        }

        public Vector4 GetScaleFix()
        {
            if (go != null)
            {
                scaleVec4.x = Math.Abs(go.transform.localScale.x);
                scaleVec4.y = Math.Abs(go.transform.localScale.y);
                scaleVec4.z = Math.Abs(go.transform.localScale.z);
                scaleVec4.w = 1;
            }
            return scaleVec4;
        }

        public void resetHp(float _nowHp, float _maxHp)
        {
            maxHp = _maxHp;
            Hp = _nowHp;
        }

        public void UpdateAnger(float value)
        {
            angerUFix = (value / 10 - 1) * BattleHeroHpBar.angerBarWidth / BattleHeroHpBar.TEXTURE_WIDTH;
            angerXFix = (value / 10 - 1) * BattleHeroHpBar.angerBarWidth;

//            angerNum = value;
        }
    }
}