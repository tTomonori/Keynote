﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MusicScore : MyBehaviour {
    //Bar同士の間隔
    static float kBarInterval = 9f;
    //現在のQNを表示する位置を弄る
    static float kScoreOffset = 1f;
    //譜面のscale
    static float kScale = 0.3f;
    //小節のオブジェクト
    private List<Bar> mBars;
    //現在の中心のBar
    public int mCurrentBarNum{
        get { return Mathf.FloorToInt((positionY - kScoreOffset) / kBarInterval / kScale); }
    }
    //現在のQN
    public float mCurrentQuarterBeat{
        get { return (positionY - kScoreOffset) / kBarInterval / kScale * (MusicScoreData.mRhythm * 4); }
        set { positionY = quarterBeatToScorePosition(value); }
    }
    //音声を流し始める時のpositionY
    public float mStartMusicPosition{
        get { return quarterBeatToScorePosition(MusicScoreData.mStartPlayMusicTime.mQuarterBeat); }
    }
    //QNを譜面のY座標に変換
    public float quarterBeatToScorePosition(float aQuarterBeat){
        return aQuarterBeat / (MusicScoreData.mRhythm * 4) * kBarInterval * kScale + kScoreOffset;
    }
    void Awake () {
        this.name = "score";
        this.transform.localScale = new Vector3(kScale, kScale, 1f);
        mBars = new List<Bar>();
	}
    private void Start(){
        for (int i = mCurrentBarNum - 4; i < mCurrentBarNum + 5;i++){
            mBars.Add(createBar(new KeyTime(i)));
        }
    }
    void Update () {
        updateBars();
	}
    //引数のQNに合わせてobjectの位置を調整
    public void show(KeyTime aTime){
        mCurrentQuarterBeat = aTime.mQuarterBeat;
    }
    //引数のQNに合わせてobjectの位置を調整
    public void show(float aQuarterBeat){
        mCurrentQuarterBeat = aQuarterBeat;
    }
    //小節生成
    private Bar createBar(KeyTime aBarNum){
        List<Arg> tNotes = MusicScoreData.getNotesInBar(aBarNum);
        Bar tBar = MyBehaviour.createObjectFromPrefab<Bar>("score/bar" + MusicScoreData.mRhythm.ToString());
        tBar.mTime = aBarNum;
        //音符追加
        foreach(Arg tNoteData in tNotes){
            tBar.addNote(tNoteData);
        }
        //位置調整
        tBar.transform.parent = gameObject.transform;
        tBar.transform.localPosition = new Vector3(0, -tBar.mTime.mBarNum * kBarInterval, 0);
        tBar.transform.localScale = new Vector3(1f, 1f, 1f);
        tBar.name = "bar:" + aBarNum.mBarNum;
        return tBar;
    }
    //現在のPositionYに応じてBarを削除・生成する
    private void updateBars(){
        int tCurrentNum = mCurrentBarNum;
        int tTopNum = tCurrentNum - 5;
        int tTailNum = tCurrentNum + 5;
        //はみ出たBar削除
        List<Bar> tDeleted = new List<Bar>();
        foreach (Bar tBar in mBars){
            int tNum = tBar.mTime.mBarNum;
            if (tTopNum <= tNum && tNum <= tTailNum) continue;
            tDeleted.Add(tBar);
        }
        foreach (Bar tBar in tDeleted){
            mBars.Remove(tBar);
            tBar.delete();
        }
        //新しくBar生成
        if(mBars.Count==0){
            mBars.Add(createBar(new KeyTime(tTopNum)));
        }
        int tCreatedTop = mBars.First<Bar>().mTime.mBarNum;
        int tCreatedTail = mBars.Last<Bar>().mTime.mBarNum;
        for (int i = tCreatedTop - 1; i >= tTopNum; i--){
            mBars.Insert(0, createBar(new KeyTime(i)));
        }
        for (int i = tCreatedTail + 1; i <= tTailNum; i++){
            mBars.Add(createBar(new KeyTime(i)));
        }
    }
    //音声の再生位置に合わせてpozitionを変更
    public void adjustPozitionToMusicTime(float aMusicTime){
        show(MusicScoreData.musicTimeToQuarterBeat(aMusicTime));
    }
}
