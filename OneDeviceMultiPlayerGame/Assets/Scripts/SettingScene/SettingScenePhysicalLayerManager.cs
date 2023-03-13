using UnityEngine;
using System.Collections;

public class SettingScenePhysicalLayerManager : MonoBehaviour {
    [SerializeField] private AudioClip settingSE;
    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void setting() {
        audioSource.PlayOneShot(settingSE);
    }

}