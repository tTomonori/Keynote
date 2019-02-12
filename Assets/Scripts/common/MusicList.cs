﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

static public class MusicList {
    //曲リストデータ
    static private MusicListFileData mData;
    //最後にプレイした曲のindex
    static public int mLastPlayIndex{
        get { return mData.lastPlayIndex; }
    }
    //最後にプレイした難易度
    static public ScoreDifficult mLastPlayDifficult{
        get { return mData.lastPlayDifficult; }
    }
    //最後に遊んだ情報を記録
    static public void updateLastPlay(int aIndex,ScoreDifficult aDifficult){
        mData.lastPlayIndex = aIndex;
        mData.lastPlayDifficult = aDifficult;
        mData.save();
    }
    static MusicList(){
        mData = DataFolder.loadListData();
    }
    static public MusicListFileData.MusicListElement get(int aNum){
        int tNum = aNum % mData.length;
        if (tNum < 0) tNum += mData.length;
        return mData.getData(tNum);
    }
}