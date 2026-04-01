using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public Animator animator;
    public GameObject starEffect;
    public AudioClip petSound;
    public AudioSource shouting;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void BeHappy()
    {
        animator.CrossFade("Comforted", 0);
        GameManager.Instance.hasPetTheDog = true;
        GameManager.Instance.SetContentIndix();
        Destroy(starEffect);
        GameManager.Instance.PlaySound(petSound);
        Invoke("CanControllLuna", 1.75f);
    }
    public void CanControllLuna()
    {
        GameManager.Instance.canControlLuna = true;
    }

    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        shouting.volume = volume;
    }
}
