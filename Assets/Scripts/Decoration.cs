using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoration
{
    private int _position;
    private bool _occupation;
    private int _layer;

    public Decoration(int position, bool occupation, int layer)
    {
        _position = position;
        _occupation = occupation;
        _layer = layer;
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

    public int Layer
    {
        get => _layer;
        set => _layer = value;
    }

    public override string ToString()
    {
        return $"{nameof(Position)}: {Position}, {nameof(Occupation)}: {Occupation}, {nameof(Layer)}: {Layer}";
    }
}