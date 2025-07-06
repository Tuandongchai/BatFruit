using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Vector3 currentScale;
    [SerializeField] private Button musicToggle, sfxToggle;
    [SerializeField] private Vector3 musicButtonOnPos, musicButtonOffPos;
    [SerializeField] private Vector3 sfxButtonOnPos, sfxButtonOffPos;


    private void OnEnable()
    {
        EnableAnimation();
        /*musicToggle.onClick.AddListener(()=>MusicButtonAnim());
        sfxToggle.onClick.AddListener(()=> SFXButtonAnim());*/
    }
    private void EnableAnimation()
    {
        gameObject.transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, currentScale, 0.5f)
            .setEase(LeanTweenType.easeOutBack);
        if (PlayerPrefs.GetInt("Music")==1)
        {
            musicToggle.GetComponent<RectTransform>().anchoredPosition = musicButtonOnPos;
        }
        else
            musicToggle.GetComponent<RectTransform>().anchoredPosition = musicButtonOffPos;

        if (PlayerPrefs.GetInt("SFX") == 1)
        {
            sfxToggle.GetComponent<RectTransform>().anchoredPosition = sfxButtonOnPos;
        }
        else
            sfxToggle.GetComponent<RectTransform>().anchoredPosition = sfxButtonOffPos;

    }
    private void OnDisable()
    {
        
    }
    public void DisableAnimation()
    {
        LeanTween.scale(gameObject, Vector3.one * 1.2f, 0.2f)
            .setEase(LeanTweenType.easeOutBack)
            .setOnComplete(() =>
            {
                LeanTween.scale(gameObject, Vector3.zero, 0.5f)
                         .setEase(LeanTweenType.easeInBack)
                         .setOnComplete(() => gameObject.SetActive(false));
            });
    }

    public void MusicButtonAnim()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            StatsManager.Instance.ToggleMusic();
            //LeanTween.moveLocal(musicToggle.gameObject, musicButtonOffPos ,0.5f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.value(
                musicToggle.gameObject,
                musicToggle.GetComponent<RectTransform>().anchoredPosition,
                new Vector2(musicButtonOffPos.x, musicButtonOffPos.y), 
                0.3f
                    ).setOnUpdate((Vector2 val) => {
                        musicToggle.GetComponent<RectTransform>().anchoredPosition = val;
                    }).setEase(LeanTweenType.easeInOutQuad);
            AudioManager.Instance.StopMusic();

        }
        else if(PlayerPrefs.GetInt("Music") == 0)
        {
            StatsManager.Instance.ToggleMusic();
            //LeanTween.moveLocal(musicToggle.gameObject, musicButtonOnPos, 0.5f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.value(
                musicToggle.gameObject,
                musicToggle.GetComponent<RectTransform>().anchoredPosition,
                new Vector2(musicButtonOnPos.x, musicButtonOnPos.y),
                0.3f
                    ).setOnUpdate((Vector2 val) => {
                        musicToggle.GetComponent<RectTransform>().anchoredPosition = val;
                    }).setEase(LeanTweenType.easeInOutQuad);
            AudioManager.Instance.PlayMusic();
        }
    }
    public void SFXButtonAnim()
    {
        if (PlayerPrefs.GetInt("SFX") == 1)
        {
            StatsManager.Instance.ToggleSFX();
            //LeanTween.moveLocal(musicToggle.gameObject, musicButtonOffPos ,0.5f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.value(
                sfxToggle.gameObject,
                sfxToggle.GetComponent<RectTransform>().anchoredPosition,
                new Vector2(sfxButtonOffPos.x, sfxButtonOffPos.y),
                0.3f
                    ).setOnUpdate((Vector2 val) => {
                        sfxToggle.GetComponent<RectTransform>().anchoredPosition = val;
                    }).setEase(LeanTweenType.easeInOutQuad);

        }
        else if(PlayerPrefs.GetInt("SFX") == 0)
        {
            StatsManager.Instance.ToggleSFX();
            //LeanTween.moveLocal(musicToggle.gameObject, musicButtonOffPos ,0.5f).setEase(LeanTweenType.easeInOutQuad);
            LeanTween.value(
                sfxToggle.gameObject,
                sfxToggle.GetComponent<RectTransform>().anchoredPosition,
                new Vector2(sfxButtonOnPos.x, sfxButtonOnPos.y),
                0.3f
                    ).setOnUpdate((Vector2 val) => {
                        sfxToggle.GetComponent<RectTransform>().anchoredPosition = val;
                    }).setEase(LeanTweenType.easeInOutQuad);
        }
    }
}
