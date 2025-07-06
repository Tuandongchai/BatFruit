using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio")]
public class AudioSO : ScriptableObject
{
    [Header("Audio")]
    [SerializeField] private AudioClip swap, match, spawSFruit, explosion, fireball, rocket, rubikEffect, rainbown;
    [SerializeField] private AudioClip inGameMusicBG;

    public AudioClip Swap => swap;
    public AudioClip Match => match;

    public AudioClip SpawSFruit => spawSFruit;
    public AudioClip Explosion => explosion;
    public AudioClip Fireball => fireball;
    public AudioClip Rocket => rocket;

    public AudioClip RubikEffect => rubikEffect;
    public AudioClip Rainbow => rainbown;
    public AudioClip InGameMusicBG => inGameMusicBG;

    
}
