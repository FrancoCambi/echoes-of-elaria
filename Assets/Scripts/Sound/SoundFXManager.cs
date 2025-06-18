using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    private static SoundFXManager instance; 

    public static SoundFXManager Instance
    {
        get
        {
            instance = instance != null ? instance : FindAnyObjectByType<SoundFXManager>();
            return instance;
        }
    }

    [SerializeField] private AudioSource soundFXObject;

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume = 1f, bool randomPitch = true)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        if (randomPitch)
        {
            float rand = Random.Range(0.9f, 1.1f);
            SoundMixerManager.Instance.SetSoundFXPitch(rand);

        }
        else
        {
            SoundMixerManager.Instance.SetSoundFXPitch(1);
        }

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClips, Transform spawnTransform, float volume = 1f)
    {
        int rand = Random.Range(0, audioClips.Length);

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClips[rand];

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
}
