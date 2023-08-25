using System.Collections.Generic;
using Modules.GameFlowModule;
using SpawnModule;
using TMPro;
using UnityEngine;
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
    private Button _toggleScreenButton;
    
    [SerializeField] 
    private Button _applyButton;
    
    [SerializeField] 
    private TMP_Dropdown _spawnKeyDropdown;

    [SerializeField] 
    private Slider _intervalSlider;

    [SerializeField] 
    private TextMeshProUGUI _intervalSliderText;

    [SerializeField] 
    private TMP_InputField _intervalInputField;
    
    [SerializeField] 
    private List<SpawnController> _spawnControllers;
    

    private PauseService _pauseService;

    public bool IsOpen { get; private set; }
    private void OnEnable()
    {
        _toggleScreenButton.onClick.AddListener(OnToggleScreen);
        _applyButton.onClick.AddListener(OnApplySettings);
        _intervalInputField.onValueChanged.AddListener(OnTrySetIntervalValue);
        _intervalSlider.onValueChanged.AddListener(OnSetIntervalValue);
    }
    
    private void OnDisable()
    {
        _toggleScreenButton.onClick.RemoveListener(OnToggleScreen);
        _applyButton.onClick.RemoveListener(OnApplySettings);
        _intervalInputField.onValueChanged.RemoveListener(OnTrySetIntervalValue);
        _intervalSlider.onValueChanged.RemoveListener(OnSetIntervalValue);
    }

    private void Start()
    {
        _pauseService = ServiceLocator.Instance.Get<PauseService>();
        foreach (var controller in _spawnControllers)
        {
            _spawnKeyDropdown.options.Add(new TMP_Dropdown.OptionData(controller.Key));
        }
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
    
    private void OnSetIntervalValue(float sliderValue)
    {
        _intervalSlider.value = sliderValue;
        _intervalSliderText.text = sliderValue.ToString();
    }

    private void OnApplySettings()
    {
        SpawnController foundController = _spawnControllers.Find(controller => controller.Key == _spawnKeyDropdown.options[_spawnKeyDropdown.value].text);
        foundController.SetInterval(_intervalSlider.value);        
    }
    
    private void OnToggleScreen()
    {
        DoScreenAnimation();
        IsOpen = !IsOpen;
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
