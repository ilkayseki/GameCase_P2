using System;
using System.Collections;
using DIG.UIExpansion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

public class LoadingManager : MonoBehaviour
{
    
    [SerializeField] GameObject _loadingPanel;
    [SerializeField] ProgresBar _progressBar;
    [SerializeField] TextMeshProUGUI _loadingTxt;
    [SerializeField] float _totalDuration = 5f;

    public static event Action OnLoadingPanelClosed;

    [SerializeField] ColorData colorData;

    private void Awake()
    {
        OnEnableLoadingPanel();
        SetRandomLoadingPanel();
    }
    
    private void OnEnableLoadingPanel()
    {
        _loadingPanel.SetActive(true);
    }

    private void SetRandomLoadingPanel()
    {
        _loadingPanel.GetComponent<Image>().color = colorData.colors[Random.Range(0,colorData.colors.Count)];
    }


    void Start()
    {
        StartLoading();
    }

    public void StartLoading()
    {
        StartCoroutine(LoadData());
    }

    IEnumerator LoadData()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _totalDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / _totalDuration);
            _progressBar.Value = progress*100;
            _loadingTxt.text = "Loading... " + (int)(progress * 100) + "%";
            yield return null;
        }

        _loadingPanel.SetActive(false);
        OnLoadingPanelClosed?.Invoke();
    }
}