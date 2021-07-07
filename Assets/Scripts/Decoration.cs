using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoration
{
    private string _name;
    private int _position;
    private bool _occupation;

    public Decoration(string name, int position, bool occupation)
    {
        _name = name;
        _position = position;
        _occupation = occupation;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public int Position
    {
        get => _position;
        set => _position = value;
    }

    public bool Occupation
    {
        get => _occupation;
        set => _occupation = value;
    }
}