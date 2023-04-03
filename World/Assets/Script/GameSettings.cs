using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private static string _settingsFilename = "settings.txt";
    private static bool _inverseWheelZoom;
    private static bool _verticalInverted;
    private static float _sensitivity;
    private static float _effectsVolume;
    private static float _musicVolume;
    private static bool _allSoundsDisabled;
    private static bool _gameTimerEnabled;
    private static bool _coinDistanceEnabled;
    private static bool _directionHintsEnabled;
    private static bool _coinTimeoutEnabled;
    private static bool _staminaEnabled;
    private static List<LeaderRecord> _leaderRecords;
    public static List<LeaderRecord> LeaderRecords { get => _leaderRecords; }
    public static bool InverseWheelZoom
    {
        get => _inverseWheelZoom;
        set
        {
            _inverseWheelZoom = value;
            SaveSettings();
        }
    }
    public static bool VerticalInverted
    {
        get => _verticalInverted;
        set
        {
            _verticalInverted = value;
            SaveSettings();
        }
    }
    public static float Sensitivity
    {
        get => _sensitivity;
        set
        {
            _sensitivity = value;
            SaveSettings();
        }
    }
    public static float EffectsVolume
    {
        get => _effectsVolume;
        set
        {
            _effectsVolume = value;
            SaveSettings();
        }
    }
    public static float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            SaveSettings();
        }
    }
    public static bool AllSoundsDisabled
    {
        get => _allSoundsDisabled;
        set
        {
            _allSoundsDisabled = value;
            SaveSettings();
        }
    }
    public static bool GameTimerEnabled
    {
        get => _gameTimerEnabled;
        set
        {
            _gameTimerEnabled = value;
            SaveSettings();
        }
    }
    public static bool CoinDistanceEnabled
    {
        get => _coinDistanceEnabled;
        set
        {
            _coinDistanceEnabled = value;
            SaveSettings();
        }
    }
    public static bool DirectionHintsEnabled
    {
        get => _directionHintsEnabled;
        set
        {
            _directionHintsEnabled = value;
            SaveSettings();
        }
    }
    public static bool CoinTimeoutEnabled
    {
        get => _coinTimeoutEnabled;
        set
        {
            _coinTimeoutEnabled = value;
            SaveSettings();
        }
    }
    public static bool StaminaEnabled
    {
        get => _staminaEnabled;
        set
        {
            _staminaEnabled = value;
            SaveSettings();
        }
    }

    private static int EnableDisplays()
    {
        int displays = 0;
        if (GameTimerEnabled) ++displays;
        if (CoinDistanceEnabled) ++displays;
        if (DirectionHintsEnabled) ++displays;
        if (CoinTimeoutEnabled) ++displays;
        if (StaminaEnabled) ++displays;
        return displays;
    }

    public static GameDifficulty Difficulty
    {
        get {
            int displays = EnableDisplays();
            if (displays < 2) return GameDifficulty.High;
            if (displays < 4) return GameDifficulty.Normal;
            return GameDifficulty.Easy;
        }
       
    }
    public static int CoinValue
    {
        get {
            return EnableDisplays() switch
            {
                0 => 50,
                1 => 40,
                2 => 30,
                3 => 25,
                4 => 15,
                _ => 10,
            };
        }

    }

    public static void RestoreDefaults()
    {
        _inverseWheelZoom = false;
        _verticalInverted = false;
        _sensitivity = 0.5f;
        _effectsVolume = 0.5f;
        _musicVolume = 0.5f;
        _allSoundsDisabled = false;
        _gameTimerEnabled = true;
        _coinDistanceEnabled = true;
        _directionHintsEnabled = true;
        _coinTimeoutEnabled = true;
        _staminaEnabled = true;
    }

    private static void SaveSettings()
    {
        //_leaderRecords= new List<LeaderRecord>();
        //_leaderRecords.Add(new() { Name = "Stepan",Score = 200});
        //_leaderRecords.Add(new() { Name = "Ybica", Score = 250 });
        //_leaderRecords.Add(new() { Name = "Stas", Score = 190 });

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(_inverseWheelZoom).Append("\n")
            .Append(_verticalInverted).Append("\n")
            .Append(_sensitivity).Append("\n")
            .Append(_gameTimerEnabled).Append("\n")
            .Append(_coinDistanceEnabled).Append("\n")
            .Append(_directionHintsEnabled).Append("\n")
            .Append(_coinTimeoutEnabled).Append("\n")
            .Append(_staminaEnabled).Append("\n")
            .Append(_effectsVolume).Append("\n")
            .Append(_musicVolume).Append("\n")
            .Append(_allSoundsDisabled).Append("\n");

        foreach(var recod in _leaderRecords)
        {
            stringBuilder.Append(recod.Name).Append(";").Append(recod.Score).Append('\n');
        }

        System.IO.File.WriteAllText(_settingsFilename, stringBuilder.ToString());
    }

    public static void LoadSettigns()
    {
        try
        {
            string content = System.IO.File.ReadAllText(_settingsFilename);
            string[] lines = content.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
            _inverseWheelZoom = Convert.ToBoolean(lines[0]);
            _verticalInverted = Convert.ToBoolean(lines[1]);
            _sensitivity = Convert.ToSingle(lines[2]);
            _gameTimerEnabled = Convert.ToBoolean(lines[3]);
            _coinDistanceEnabled = Convert.ToBoolean(lines[4]);
            _directionHintsEnabled = Convert.ToBoolean(lines[5]);
            _coinTimeoutEnabled = Convert.ToBoolean(lines[6]);
            _staminaEnabled = Convert.ToBoolean(lines[7]);
            _effectsVolume = Convert.ToSingle(lines[8]);
            _musicVolume = Convert.ToSingle(lines[9]);
            _allSoundsDisabled = Convert.ToBoolean(lines[10]);

            _leaderRecords = new();
            for(int i=11;i<lines.Length;i++) {
                try
                {
                    _leaderRecords.Add(LeaderRecord.Parse(lines[i]));
                }catch(System.ArgumentException e)
                {
                    Debug.Log(e.Message);
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public enum GameDifficulty
    {
        Easy = 0,
        Normal = 1,
        High = 2
    }

    public class LeaderRecord
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public static LeaderRecord Parse(string text)
        {
            string[] parts = text.Split(';',2);
            try
            {
                return new()
                {
                    Name = parts[0],
                    Score = int.Parse(parts[1])
                };
            }
            catch {
                throw new System.ArgumentException($"Invalid string {text}");
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
