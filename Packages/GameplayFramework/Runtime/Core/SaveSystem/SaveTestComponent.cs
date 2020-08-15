using System.Collections;
using System.Collections.Generic;
using System.Text;
using TowerJump.Save;
using UnityEngine;

[System.Serializable]
public struct test
{
    public string a;
    public string c;

    public test(string a = "",string c = "") : this()
    {
        this.a = a;
        this.c = c;
    }
}

[SaveClass]
public class SaveTestComponent : MonoBehaviour
{
    [Save]
    public int bla = 69;

    [Save] public int blabla { get; set; } = 6;
    [Save] public test yalalla = new test("a","b");
}
