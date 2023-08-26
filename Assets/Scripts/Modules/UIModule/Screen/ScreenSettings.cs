using System.Collections.Generic;
using Modules.GameFlowModule;
using SpawnModule;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UtilityModule.Debug;
using UtilityModule.Service;

public class ScreenSettings : MonoBehaviour
{
    private readonly int OPEN_ANIMATION = Animator.StringToHash("SettingsDescend");
    private readonly int CLOSE_ANIMATION = Animator.StringToHash("SettingsAscend");
    
    [SerializeField] 
    private Animator _animator;

    [SerializeField] 
    private GameObject _fadeScreen;
    
    [SerializeField] 
    private Button _toggleScreenButton;
    
    [SerializeField] 
    private Button _applySpawnSettingsButton;
    [SerializeField] 
    private Button _applyCrowdSettingsButton;
    
    [SerializeField] 
    private TMP_Dropdown _spawnKeyDropdown;

    [SerializeField] 
    private Slider _intervalSlider;

    [SerializeField] 
    private TextMeshProUGUI _intervalSliderText;

    [SerializeField] 
    private TMP_InputField _intervalInputField;
    
    [SerializeField] 
    private Slider _crowdThresholdSlider;

    [SerializeField] 
    private TextMeshProUGUI _crowdThresholdText;

    [SerializeField] 
    private TMP_InputField _crowdThresholdInputField;
    
    [SerializeField] 
    private List<SpawnController> _spawnControllers;
    

    private PauseService _pauseService;

    public bool IsOpen { get; private set; }
    private void OnEnable()
    {
        _toggleScreenButton.onClick.AddListener(OnToggleScreen);
        _applySpawnSettingsButton.onClick.AddListener(OnApplySpawnSettings);
        _applyCrowdSettingsButton.onClick.AddListener(OnApplyCrowdSettings);
        _intervalInputField.onValueChanged.AddListener(OnTrySetIntervalValue);
        _intervalSlider.onValueChanged.AddListener(OnSetIntervalValue);
        _crowdThresholdInputField.onValueChanged.AddListener(OnTrySetCrowdValue);
        _crowdThresholdSlider.onValueChanged.AddListener(OnSetCrowdValue);
    }
    
    private void OnDisable()
    {
        _toggleScreenButton.onClick.RemoveListener(OnToggleScreen);
        _applySpawnSettingsButton.onClick.RemoveListener(OnApplySpawnSettings);
        _applyCrowdSettingsButton.onClick.RemoveListener(OnApplyCrowdSettings);
        _intervalInputField.onValueChanged.RemoveListener(OnTrySetIntervalValue);
        _intervalSlider.onValueChanged.RemoveListener(OnSetIntervalValue);
        _crowdThresholdInputField.onValueChanged.RemoveListener(OnTrySetCrowdValue);
        _crowdThresholdSlider.onValueChanged.RemoveListener(OnSetCrowdValue);
    }

    private void Start()
    {
        _pauseService = ServiceLocator.Instance.Get<PauseService>();
        foreach (var controller in _spawnControllers)
        {
            _spawnKeyDropdown.options.Add(new TMP_Dropdown.OptionData(controller.Key));
        }

        _intervalSliderText.text = _intervalSlider.value.ToString();
        _crowdThresholdText.text = _crowdThresholdSlider.value.ToString();
    }
    
    private void OnTrySetIntervalValue(string inputValue)
    {
        if (!float.TryParse(inputValue, out float value))
        {
            return;
        }
        if (value >= _intervalSlider.minValue && value <= _intervalSlider.maxValue)
        {
            OnSetIntervalValue(value);
        }
        else
        {
            _intervalInputField.text = default;
        }
    }

    private void OnTrySetCrowdValue(string inputValue)
    {
        if (!int.TryParse(inputValue, out int value))
        {
            return;
        }
        if (value >= _crowdThresholdSlider.minValue && value <= _crowdThresholdSlider.maxValue)
        {
            OnSetCrowdValue(value);
        }
        else
        {
            _intervalInputField.text = default;
        }
    }

    private void OnSetIntervalValue(float sliderValue)
    {
        _intervalSlider.value = sliderValue;
        _intervalSliderText.text = sliderValue.ToString();
    }

    private void OnSetCrowdValue(float sliderValue)
    {
        _crowdThresholdSlider.value = sliderValue;
        _crowdThresholdText.text = sliderValue.ToString();
    }

    private void OnApplySpawnSettings()
    {
        SpawnController foundController = _spawnControllers.Find(controller => controller.Key == _spawnKeyDropdown.options[_spawnKeyDropdown.value].text);
        foundController.SetInterval(_intervalSlider.value);
    }
    
    private void OnApplyCrowdSettings()
    {
        foreach (var controller in _spawnControllers)
        {
            controller.SetMaxPopulation((int)_crowdThresholdSlider.value);
        }
    }
    
    private void OnToggleScreen()
    {
        DoScreenAnimation();
        IsOpen = !IsOpen;
        _fadeScreen.SetActive(IsOpen);
        if (IsOpen)
        {
            _pauseService.PauseGameByTimeScale();
        }
        else
        {
            _pauseService.UnPauseGameByTimeScale();
        }
    }

    private void DoScreenAnimation()
    {
        _animator.Play(IsOpen ? CLOSE_ANIMATION : OPEN_ANIMATION);
    }
}
