using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // expose instance

    [Header("Clips Library")]
    public AudioClip tankEngineIdle; // tank engine running / idle
    public AudioClip tankEngineMoving; // tank engine moving
    public AudioClip tankBarrel; // tank barrel moving
    public AudioClip powerChangeClick; // click on power change
    public AudioClip tankFire; // tank fires
    public AudioClip environmentExplosion; // explosion on impact
    public AudioClip tankHitExplosion; // explosion on impact
    public AudioClip crateHeal;
    public AudioClip cratePickUp;
    public AudioClip uiClick; // click on UI
    public AudioClip jetpack;

    [Header("Announcer")]
    public AudioClip tankHitAnnouncer;
    public AudioClip tankNearHitAccouncer;
    public AudioClip crateInboundAnnouncer;
    public AudioClip fireAtWillAnnouncer;



    [Header("Global Sources")]
    [SerializeField] public AudioSource musicSource; // in game music
    [SerializeField] public AudioSource sfxSource; 
    [SerializeField] private AudioSource uiSource; // Non-spatial
    [SerializeField] private AudioSource announcerSource; // Non-spatial

    void Awake() {
        Instance = this;

        // start playing game music... on awake ?
        // musicSource.playOnAwake = gameObject;
    }

    public void PlayUI(AudioClip clip) {
        uiSource.PlayOneShot(clip);
    }

    public void PlayTankHit()
    {
        sfxSource.PlayOneShot(tankHitExplosion);
    }

    public void PlayTankHitNotDead()
    {
        sfxSource.PlayOneShot(tankNearHitAccouncer);
    }

    public void PlayEnvironmentHit()
    {
        sfxSource.PlayOneShot(environmentExplosion);
    }

    // Announcer clips
    public void PlayFireAtWillAnnouncer()
    {
        announcerSource.PlayOneShot(fireAtWillAnnouncer);
    }
    public void PlayTargetNeutralizedAnnouncer()
    {
        announcerSource.PlayOneShot(tankHitAnnouncer);
    }

    public void PlayArtilleryInbound()
    {
        announcerSource.PlayOneShot(tankNearHitAccouncer);
    }
    public void PlayCrateInbound()
    {
        announcerSource.PlayOneShot(crateInboundAnnouncer);
    }

    public void PlayHealCrateSFX()
    {
        sfxSource.PlayOneShot(crateHeal);
    }

    public void PlayCratePickUp()
    {
        sfxSource.PlayOneShot(cratePickUp);
    }
}
