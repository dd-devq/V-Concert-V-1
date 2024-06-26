using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Interaction;

public class ActivatorManager : SingletonMono<ActivatorManager>
{
    private List<Activator> _activators = new();
    private Dictionary<NoteName, int> _pitchNameDict = new();

    public List<Activator> Activators
    {
        get => _activators;
        set => _activators = value;
    }

    public void Awake()
    {
        GetListActivators();
    }

    private void Start()
    {
        foreach (Activator activator in _activators)
        {
            var endZone = activator.EndZone;
            var distance = Vector3.Distance(activator.StartZone.transform.position,
                activator.EndZone.transform.position);
            var direction = (activator.EndZone.transform.position - activator.StartZone.transform.position).normalized;
            endZone.transform.position = activator.transform.position + direction * distance * 0.5f;
            //endZone.SetActive(false);
        }
    }

    private void GetListActivators()
    {
        for (var i = 0; i < 2; i++)
        {
            var child = transform.GetChild(i);
            _activators.Add(child.GetComponent<Activator>());
        }

        for (var i = 0; i < _activators.Count; i++)
        {
            _activators[i].ZoneIndex = i;
        }
    }

    private Activator GetActivatorByIndex(int index)
    {
        for (var i = 0; i < _activators.Count; i++)
        {
            if (_activators[i].ZoneIndex == index)
                return _activators[i];
        }

        return null;
    }

    private void AddPitchesName(List<Melanchall.DryWetMidi.Interaction.Note> listNotes)
    {
        double interval = 0;
        //Get Pitch Name that match condition
        for (var i = 0; i < listNotes.Count; i++)
        {
            var note = listNotes[i];
            if (i == 0 || i == listNotes.Count - 1)
            {
                var metricTimeSpan =
                    TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.Midifile.GetTempoMap());
                double noteTimeMidi = (double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                                     (double)metricTimeSpan.Milliseconds / 1000f;
                interval = noteTimeMidi;
                if (!_pitchNameDict.ContainsKey(note.NoteName))
                {
                    _pitchNameDict.Add(note.NoteName, 1);
                }
                else _pitchNameDict[note.NoteName] += 1;
            }
            else
            {
                var metricTimeSpan =
                    TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.Midifile.GetTempoMap());
                double noteTimeMidi = (double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                                     (double)metricTimeSpan.Milliseconds / 1000f;
                if (noteTimeMidi - interval >= Define.NoteInterval)
                {
                    interval = noteTimeMidi;
                    if (!_pitchNameDict.ContainsKey(note.NoteName))
                    {
                        _pitchNameDict.Add(note.NoteName, 1);
                    }
                    else _pitchNameDict[note.NoteName] += 1;
                }
            }
        }
    }

    private void DividePitchesIntoZones()
    {
        //Divide pitches to zones
        int countOfPitch = _pitchNameDict.Count;
        int PitchPerZone = 0;
        int index = 0;
        switch (countOfPitch % Define.NumOfActivators)
        {
            case 0:
                PitchPerZone = countOfPitch / Define.NumOfActivators;
                for (var i = 0; i < _pitchNameDict.Count; i += PitchPerZone)
                {
                    foreach (var zone in _activators)
                    {
                        if (zone.ZoneIndex == i / PitchPerZone)
                        {
                            for (var j = 0; j < PitchPerZone; j++)
                            {
                                zone.Pitches.Add(_pitchNameDict.ElementAt(i + j).Key);
                            }
                        }
                    }
                }

                break;
            case 1:
                PitchPerZone = (countOfPitch - 1) / Define.NumOfActivators;
                index = 0;
                for (var i = 0; i < _pitchNameDict.Count; i++)
                {
                    if (i <= PitchPerZone)
                    {
                        index = 0;
                    }

                    if (i > PitchPerZone && index <= PitchPerZone * 2)
                    {
                        index = 1;
                    }

                    if (i > PitchPerZone * 2 && i <= PitchPerZone * 3)
                    {
                        index = 2;
                    }

                    if (i > PitchPerZone * 3 && i <= PitchPerZone * 4)
                    {
                        index = 3;
                    }

                    GetActivatorByIndex(index).Pitches.Add(_pitchNameDict.ElementAt(i).Key);
                }

                break;
            case 2:
                PitchPerZone = (countOfPitch - 2) / Define.NumOfActivators;
                index = 0;
                for (var i = 0; i < _pitchNameDict.Count; i++)
                {
                    if (i <= PitchPerZone)
                    {
                        index = 0;
                    }

                    if (i > PitchPerZone && index <= PitchPerZone * 2 + 1)
                    {
                        index = 1;
                    }

                    if (i > PitchPerZone * 2 + 1 && i <= PitchPerZone * 3 + 1)
                    {
                        index = 2;
                    }

                    if (i > PitchPerZone * 3 + 1 && i <= PitchPerZone * 4 + 1)
                    {
                        index = 3;
                    }

                    GetActivatorByIndex(index).Pitches.Add(_pitchNameDict.ElementAt(i).Key);
                }

                break;
            case 3:
                PitchPerZone = (countOfPitch - 3) / Define.NumOfActivators;
                index = 0;
                for (var i = 0; i < _pitchNameDict.Count; i++)
                {
                    if (i <= PitchPerZone)
                    {
                        index = 0;
                    }

                    if (i > PitchPerZone && index <= PitchPerZone * 2 + 1)
                    {
                        index = 1;
                    }

                    if (i > PitchPerZone * 2 + 1 && i <= PitchPerZone * 3 + 2)
                    {
                        index = 2;
                    }

                    if (i > PitchPerZone * 3 + 2 && i <= PitchPerZone * 4 + 2)
                    {
                        index = 3;
                    }

                    GetActivatorByIndex(index).Pitches.Add(_pitchNameDict.ElementAt(i).Key);
                }

                break;
        }
    }

    public void SetSpawnedTimes(List<Melanchall.DryWetMidi.Interaction.Note> listNotes)
    {
        AddPitchesName(listNotes);
        //sort dictionary
        List<KeyValuePair<NoteName, int>> sortedList = _pitchNameDict.ToList();
        sortedList.Sort((x, y) => x.Value.CompareTo(y.Value));
        _pitchNameDict = sortedList.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        DividePitchesIntoZones();

        double interval = 0;
        for (var i = 0; i < listNotes.Count; i++)
        {
            var note = listNotes[i];
            if (i == 0 || i == listNotes.Count - 1)
            {
                foreach (var activator in _activators)
                {
                    if (activator.Pitches.Contains(note.NoteName))
                    {
                        var metricTimeSpan =
                            TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.Midifile.GetTempoMap());
                        double noteTimeMidi = (double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                                             (double)metricTimeSpan.Milliseconds / 1000f;
                        activator.NoteTimeMidi.Add(noteTimeMidi);
                        interval = noteTimeMidi;
                    }
                }
            }
            else
            {
                var metricTimeSpan =
                    TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.Midifile.GetTempoMap());
                double noteTimeMidi = (double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                                     (double)metricTimeSpan.Milliseconds / 1000f;
                if (noteTimeMidi - interval >= Define.NoteInterval)
                {
                    foreach (var activator in _activators)
                    {
                        if (activator.Pitches.Contains(note.NoteName))
                        {
                            activator.NoteTimeMidi.Add(noteTimeMidi);
                            interval = noteTimeMidi;
                        }
                    }
                }
            }
        }
    }
}