﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MyBehaviour {
    [SerializeField] private TextMesh mTitle;
    [SerializeField] private HideButton mEditButton;
    public void set(string aTitle,Arg aParameters){
        mTitle.text = aTitle;
        mEditButton.mParameters = aParameters;
    }
    private void OnMouseOver(){
        mEditButton.positionZ = -1; 
    }
    private void OnMouseExit(){
        mEditButton.positionZ = 1;
    }
}
