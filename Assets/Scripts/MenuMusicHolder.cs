using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicHolder : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip[] AudioSongsTab;
    int oldIndex = 0;

    void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("GameMusic");
        if (objects.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        AudioSongsTab = Resources.LoadAll<AudioClip>("Music/InGameAlbum");
    }

    void Update()
    {
        if(!audioSource.isPlaying)
        {
            int number = Random.Range(0, AudioSongsTab.Length - 1);
            while( oldIndex == number)
            {
                number = Random.Range(0, AudioSongsTab.Length - 1);
            }
            audioSource.clip = AudioSongsTab[number];
            audioSource.Play();
            oldIndex = number;
        }
    }
}
