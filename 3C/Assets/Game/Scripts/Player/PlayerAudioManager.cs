using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _footStepSfx;

    [SerializeField]
    private AudioSource _landingSfx;

    [SerializeField]
    private AudioSource _punchSfx;

    [SerializeField]
    private AudioSource _glideSfx;

    private void PlayFootStepSfx()
    {

        _footStepSfx.volume = Random.Range(0.8f, 1f);

        _footStepSfx.pitch = Random.Range(0.5f, 2.5f);

        _footStepSfx.Play();
    }

    private void PlayLandingSfx()
    {
        _landingSfx.Play();
    }

    private void PunchSfx()
    {
        _punchSfx.volume = Random.Range(0.8f, 1f);

        _punchSfx.pitch = Random.Range(0.8f, 1.5f);

        _punchSfx.Play();
    }

    public void PlayGlideSfx()
    {
        _glideSfx.volume = Random.Range(0.2f, 0.8f);

        _glideSfx.pitch = Random.Range(0.3f, 0.7f);

        _glideSfx.Play();
    }

    public void StopGlideSfx()
    {
        _glideSfx.Stop();
    }
}
