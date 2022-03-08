using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language
{
    Ukrainian,
    English,
    Russian
}

public class LanguageManager : MonoBehaviour
{
    public static Language Language { get; private set; }
}
