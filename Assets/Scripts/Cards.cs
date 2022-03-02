using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores value of specific cards
/// </summary>
[System.Serializable]
public class Cards
{
    /// <summary>
    /// Stores the actual sprite of the card
    /// </summary>
    public GameObject face;

    /// <summary>
    /// Stores the suit of the card
    /// </summary>
    public Suits suit;

    /// <summary>
    /// Stors the value of the card
    /// </summary>
    public Values value;
}

/// <summary>
/// Sets the suit of the card
/// </summary>
public enum Suits
{
    Club,
    Diamond,
    Heart,
    Spade
}

/// <summary>
/// Sets the value of the card
/// </summary>
public enum Values
{
    Ace,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King
}