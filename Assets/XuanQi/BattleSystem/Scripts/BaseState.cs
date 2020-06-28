using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Battle
{
    public enum StateID
    {
        NullState = 0,
        WeakState,
        NormalState,
        FinalState
    }
    public enum TransitionID
    {
        NullTransition = 0,
        Normal2Weak,
        Weak2Normal,
        Normal2Final,
        Weak2Final
    }
    public abstract class BaseState 
    {
        Dictionary<TransitionID, StateID> map = new Dictionary<TransitionID, StateID>();
        protected StateID _stateID;
        public StateID ID { get { return _stateID; } }
        /// <summary>
        /// 添加转换
        /// </summary>
        /// <param name="transitionID"></param>
        /// <param name="stateID"></param>
        public void AddTransition(TransitionID transitionID, StateID stateID)
        {
            if(transitionID == TransitionID.NullTransition||stateID == StateID.NullState)
            {
                Debug.Log("无效的状态切换！");
                return;
            }
            if(stateID==ID)
            {
                Debug.Log("目标状态是自身！");
                return;
            }
            map.Add(transitionID,stateID);
        }
        /// <summary>
        /// 删去转换
        /// </summary>
        /// <param name="transitionID"></param>
        public void DeleteTransition(TransitionID transitionID)
        {
            if(transitionID == TransitionID.NullTransition)
            {
                Debug.Log("该转换为空！");
                return;
            }
            if(map.ContainsKey(transitionID))
                map.Remove(transitionID);
            else
                Debug.Log("不存在该转换！");
            return;
        }
        /// <summary>
        /// 获得转换对应的状态
        /// </summary>
        /// <param name="transitionID"></param>
        /// <returns></returns>
        public StateID GetOutputState(TransitionID transitionID)
        {
            if(map.ContainsKey(transitionID))
            {
                return map[transitionID];
            }
            return StateID.NullState;
        }
        /// <summary>
        /// 判断是否切换为其他状态
        /// </summary>
        public virtual void Reason() { }
        /// <summary>
        /// 正常执行
        /// </summary>
        public virtual void Act() { }
        /// <summary>
        /// 离开状态前调用
        /// </summary>
        public virtual void DoBeforeLeaving(){}
        /// <summary>
        /// 进入状态时调用
        /// </summary>
        public virtual void DoBeforeEnterState() {}
    }
    public abstract class FSMSystem
    {
        private List<BaseState> states ;
        private BaseState _currentState;
        private StateID _currentID;
        public BaseState CurrentState { get { return _currentState; } }
        public StateID CurrentID { get { return _currentID; } }
        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="state"></param>
        public void AddState(BaseState state)
        {
            if(state == null)
            {
                Debug.LogError("无法添加空状态");
                return;
            }
            if(states.Count==0)
            {
                states.Add(state);
                _currentID = state.ID;
                _currentState = state;
            }
            foreach(BaseState state1 in states)
            {
                if(state1.ID == state.ID)
                {
                    Debug.LogError("已存在状态" + state.ID);
                    return;
                }
            }
        }
        /// <summary>
        /// 删除状态
        /// </summary>
        /// <param name="id"></param>
        public void DeleteState(StateID id)
        {
            if(id==StateID.NullState)
            {
                Debug.LogError("你不能删除一个不存在的状态");
                return;
            }
            foreach(BaseState state in states)
            {
                if(state.ID == id)
                {
                    states.Remove(state);
                    return;
                }
            }
            Debug.LogError("未在列表中找到状态");
            return;
        }
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="transitionID"></param>
        public void PerformTransition(TransitionID transitionID)
        {
            if(transitionID == TransitionID.NullTransition)
            {
                Debug.LogError("转换为空！");
                return;
            }
            StateID stateID = CurrentState.GetOutputState(transitionID);
            if(stateID == StateID.NullState)
            {
                Debug.LogError("转换 " + transitionID.ToString() + " 为空！");
                return;
            }
            foreach(BaseState state in states )
            {
                if(state.ID == stateID)
                {
                    _currentState.DoBeforeLeaving();
                    _currentState = state;
                    _currentID = stateID;
                    _currentState.DoBeforeEnterState();
                }
            }
        }
    }

}
