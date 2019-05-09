using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsManager
{
    const string depth = "Depth";
    const string level = "Level";
    const string pieceType = "Piece Type";
    const string userName = "User Name";

    public static void SetLevel(int l)
    {
        PlayerPrefs.SetInt(level, l);
    }

    public static int GetLevel()
    {
        if (PlayerPrefs.HasKey(level))
            return PlayerPrefs.GetInt(level);
        else
            return 0;
    }

    public static int GetDepth()
    {
        int l = GetLevel();
        switch(l)
        {
            case 0:
                return 4;
            case 1:
                return 6;
            case 2:
                return 8;
        }
        return 2;
    }

    public static void SetPieceType(int t)
    {
        PlayerPrefs.SetInt(pieceType, t);
    }

    public static int GetPieceType()
    {
        if (PlayerPrefs.HasKey(pieceType))
        {
            return PlayerPrefs.GetInt(pieceType);
        }
        else
            return 0;
    }

    public static void SetUserName(string name)
    {
        PlayerPrefs.SetString(userName, name);
    }

    public static string GetUserName()
    {
        if (PlayerPrefs.HasKey(userName))
        {
            return PlayerPrefs.GetString(userName);
        }
        else
            return "";
    }
}