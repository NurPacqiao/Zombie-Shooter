using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;
    public AudioSource reloadingSoundAK74;
    public AudioSource reloadingSoundUzi;

    public AudioSource emptyMagazineSoundAK74;


    public AudioClip AK74Shot;
    public AudioClip UziShot;

    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;

    public AudioSource zombieChannel;
    public AudioSource zombieChannel2;

    public AudioSource playerChannel;

    public AudioClip playerHurt;
    public AudioClip playerDeath;


    public AudioClip gameOverMusic;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }



    public void PlayShootingSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.AK74:
            {
                    ShootingChannel.PlayOneShot(AK74Shot);
                    break;
            }
            case Weapon.WeaponModel.Uzi:
            {
                    //play uzi sound
                    ShootingChannel.PlayOneShot(UziShot);
                    break;
            }


        }
    }


    public void PlayReloadSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.AK74:
                {
                    reloadingSoundAK74 .Play();
                    break;
                }
            case Weapon.WeaponModel.Uzi:
                {
                    //play uzi sound
                    reloadingSoundUzi.Play();
                    break;
                }


        }
    }

}
