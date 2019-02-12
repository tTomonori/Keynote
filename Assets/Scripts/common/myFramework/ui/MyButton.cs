﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : MyBehaviour {
    //押された時のメッセージ
    [SerializeField] protected string mPushedMessage;
    [SerializeField] protected string mGroup;
    /// <summary>
    /// falseなら押されても処理を実行しない
    /// </summary>
    [SerializeField] public bool mActivated = true;
    public Arg mParameters=new Arg();
    //押されているか
    private bool isPushed = false;
    //押された時に大きさを変化させるbehaviour
    private MyBehaviour mContentsObject;
    protected MyBehaviour mContents{
        get{
            if (mContentsObject == null) uniteChildren();
            return mContentsObject;
        }
    }
    private void uniteChildren(){
        mContentsObject = MyBehaviour.create<MyBehaviour>();
        mContents.name = "buttonContents";
        mContents.transform.SetParent(this.transform, false);
        //子要素を一つのbehaviourにまとめる
        foreach(Transform tChild in GetComponentsInChildren<Transform>()){
            tChild.SetParent(mContents.transform, true);
        }
    }
    private void OnMouseDown(){
        isPushed = true;
        mContents.scaleBy(new Vector3(-0.1f, -0.1f, 0), 0.1f);
    }
    private void OnMouseExit(){
        if (!isPushed) return;
        isPushed = false;
        mContents.scaleBy(new Vector3(0.1f, 0.1f, 0), 0.1f);
    }
    private void OnMouseUp(){
        if (!isPushed) return;
        isPushed = false;
        mContents.scaleBy(new Vector3(0.1f, 0.1f, 0), 0.1f);
        if (mActivated)
            push();
    }
    /// <summary>
    /// ボタンを押されたことにする
    /// </summary>
    public void push(){
        pushed();
        string tMessage = (mPushedMessage == "") ? name + "Pushed" : mPushedMessage;
        Subject.sendMessage(new Message(tMessage, mParameters, mGroup));
    }
    /// <summary>
    /// ボタンが押された時の処理
    /// </summary>
    protected virtual void pushed(){}
}
