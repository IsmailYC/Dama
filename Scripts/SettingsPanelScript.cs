using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelScript : MonoBehaviour {
    public Button[] typeButtons;
    public Button[] levelButtons;

    int level, type;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        SetPanel();
    }

    void SetPanel()
    {
        level = PlayerPrefsManager.GetLevel();
        type = PlayerPrefsManager.GetPieceType();
        levelButtons[level].interactable = false;
        typeButtons[type].interactable = false;
    }

    public void ChangePieceType(int t)
    {
        typeButtons[type].interactable = true;
        type = t;
        typeButtons[type].interactable = false;
    }

    public void ChangeGameLevel(int l)
    {
        levelButtons[level].interactable = true;
        level = l;
        levelButtons[level].interactable = false;
    }

    public void CancelSettings()
    {
        gameObject.SetActive(false);
    }

    public void ApplySettings()
    {
        PlayerPrefsManager.SetLevel(level);
        PlayerPrefsManager.SetPieceType(type);
        gameObject.SetActive(false);
    }
}
