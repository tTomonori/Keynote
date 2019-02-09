﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

static public class DataFolder {
    static public string path{
        get{
            if (Debug.isDebugBuild)
                return Application.dataPath + "/../data";
            else
                return Application.dataPath + "/../../data";
        }
    }
    //楽曲リスト取得
    static public Arg loadListData(){
        return new Arg(MyJson.deserializeFile(DataFolder.path + "/list.json"));
    }
    //譜面データ取得
    static public MusicScoreFileData loadScoreData(string aFileName){
        return new MusicScoreFileData(new Arg(MyJson.deserializeFile(DataFolder.path + "/score/" + aFileName + ".json")));
    }
    //譜面データ存在確認
    static public bool existScoreData(string aFileName){
        return File.Exists(DataFolder.path + "/score/" + aFileName + ".json");
    }
    //音声取得
    static public AudioClip loadMusic(string aFileName){
        WWW tW = new WWW("file:///" + DataFolder.path + "/music/" + aFileName);
        while (!tW.isDone) { }//読み込み完了まで待機
        return tW.GetAudioClip();
    }
    //音声存在確認
    static public bool existMusic(string aFileName){
        return File.Exists(DataFolder.path + "/music/" + aFileName);
    }
    //サムネイル取得
    static public Sprite loadThumbnail(string aFileName){
        if (aFileName == "")
            return Resources.Load<Sprite>("sprites/default/defaultThumbnail");
        return SpriteLoader.load(DataFolder.path + "/thumbnail/" + aFileName);
    }
    //サムネイル取得(非同期)
    static public void loadThumbnailAsync(string aFileName,Action<Sprite> aRes){
        if (aFileName == ""){
            aRes(Resources.Load<Sprite>("sprites/default/defaultThumbnail"));
            return;
        }
        SpriteLoader.loadAsync(DataFolder.path + "/thumbnail/" + aFileName, aRes);
    }
    //背景取得
    static public Sprite loadBackImage(string aFileName){
        Resources.Load<Sprite>("sprites/star/star");
        WWW tW = new WWW("file:///" + DataFolder.path + "/back/" + aFileName);
        while (!tW.isDone) { }//読み込み完了まで待機
        return Sprite.Create(tW.texture, new Rect(new Vector2(0, 0), tW.texture.texelSize), new Vector2(0.5f, 0.5f));
    }
}
