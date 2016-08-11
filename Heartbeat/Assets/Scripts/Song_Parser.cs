using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

public class Song_Parser
{

    private string filePath;

    private const float sampleLengthDefault = 15.0f;

    public struct Metadata
    {
        public bool valid;

        public string title;
        public string subtitle;
        public string artist;
        public string bannerPath;
        public string backgroundPath;
        public string musicPath;
        public float offset;
        public float sampleStart;
        public float sampleLength;
        public float bpm;

        public NoteData beginner;
        public bool beginnerExists;
        public NoteData easy;
        public bool easyExists;
        public NoteData medium;
        public bool mediumExists;
        public NoteData hard;
        public bool hardExists;
        public NoteData challenge;
        public bool challengeExists;
    }

    public struct NoteData
    {
        public List<List<Notes>> bars;
    }

    public struct Notes
    {
        public bool left;
        public bool right;
        public bool up;
        public bool down;
    }

    public enum difficulties { beginner, easy, medium, hard, challenge };

    public Metadata Parse(string newFilePath)
    {
        filePath = newFilePath;

        if (IsNullOrWhiteSpace(filePath))
        {
            //Error
            Metadata tempMeta = new Metadata();
            tempMeta.valid = false;
            return tempMeta;
        }

        bool inNotes = false;

        Metadata songData = new Metadata();
        //Initialise Metadata
        //If it encounters any major errors during parsing, this is set to false and the song cannot be selected
        songData.valid = true;
        songData.beginnerExists = false;
        songData.easyExists = false;
        songData.mediumExists = false;
        songData.hardExists = false;
        songData.challengeExists = false;

        //Collect the data from the sm file
        List<string> fileData = File.ReadAllLines(filePath).ToList<string>();

        string fileDir = Path.GetDirectoryName(filePath);
        if (!fileDir.EndsWith("\\") && !fileDir.EndsWith("/"))
        {
            fileDir += "\\";
        }

        for (int i = 0; i < fileData.Count; i++)
        {
            string line = fileData[i].Trim();

            if (line.StartsWith("//"))
            {
                continue;
            }
            else if (line.StartsWith("#"))
            {
                string key = line.Substring(0, line.IndexOf(':')).Trim('#').Trim(':');

                switch (key.ToUpper())
                {
                    case "TITLE":
                        songData.title = line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                        break;
                    case "SUBTITLE":
                        songData.subtitle = line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                        break;
                    case "ARTIST":
                        songData.artist = line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                        break;
                    case "BANNER":
                        songData.bannerPath = fileDir + line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                        break;
                    case "BACKGROUND":
                        songData.backgroundPath = fileDir + line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                        break;
                    case "MUSIC":
                        songData.musicPath = fileDir + line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                        if (IsNullOrWhiteSpace(songData.musicPath) || !File.Exists(songData.musicPath))
                        {
                            //No music file found!
                            songData.musicPath = null;
                            songData.valid = false;
                        }
                        break;
                    case "OFFSET":
                        if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out songData.offset))
                        {
                            //Error Parsing
                            songData.offset = 0.0f;
                        }
                        break;
                    case "SAMPLESTART":
                        if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out songData.sampleStart))
                        {
                            //Error Parsing
                            songData.sampleStart = 0.0f;
                        }
                        break;
                    case "SAMPLELENGTH":
                        if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out songData.sampleLength))
                        {
                            //Error Parsing
                            songData.sampleLength = sampleLengthDefault;
                        }
                        break;
                    case "DISPLAYBPM":
                        if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out songData.bpm) || songData.bpm <= 0)
                        {
                            //Error Parsing - BPM not valid
                            songData.valid = false;
                            songData.bpm = 0.0f;
                        }
                        break;
                    case "NOTES":
                        inNotes = true;
                        break;
                    default:
                        break;
                }
            }

            if (inNotes)
            {
                //Skip dance-double for now
                if (line.ToLower().Contains("dance-double"))
                {
                    for(int j = i; j < fileData.Count; j++)
                    {
                        if (fileData[j].Contains(";"))
                        {
                            i = j - 1;
                            break;
                        }
                    }
                }

                //Check if it's a difficulty
                if (line.ToLower().Contains("beginner") ||
                    line.ToLower().Contains("easy") ||
                    line.ToLower().Contains("medium") ||
                    line.ToLower().Contains("hard") ||
                    line.ToLower().Contains("challenge"))
                {
                    string difficulty = line.Trim().Trim(':');

                    List<string> noteChart = new List<string>();
                    for (int j = i; j < fileData.Count; j++)
                    {
                        string noteLine = fileData[j].Trim();
                        if (noteLine.EndsWith(";"))
                        {
                            i = j - 1;
                            break;
                        }
                        else
                        {
                            noteChart.Add(noteLine);
                        }
                    }

                    switch (difficulty.ToLower().Trim())
                    {
                        case "beginner":
                            songData.beginnerExists = true;
                            songData.beginner = ParseNotes(noteChart);
                            break;
                        case "easy":
                            songData.easyExists = true;
                            songData.easy = ParseNotes(noteChart);
                            break;
                        case "medium":
                            songData.mediumExists = true;
                            songData.medium = ParseNotes(noteChart);
                            break;
                        case "hard":
                            songData.hardExists = true;
                            songData.hard = ParseNotes(noteChart);
                            break;
                        case "challenge":
                            songData.challengeExists = true;
                            songData.challenge = ParseNotes(noteChart);
                            break;
                    }
                }
                if (line.EndsWith(";"))
                {
                    inNotes = false;
                }
            }
        }

        return songData;
    }

    private NoteData ParseNotes(List<string> notes)
    {
        NoteData noteData = new NoteData();
        noteData.bars = new List<List<Notes>>();

        List<Notes> bar = new List<Notes>();
        for(int i = 0; i < notes.Count; i++)
        {
            string line = notes[i].Trim();

            if (line.StartsWith(";"))
            {
                break;
            }

            if (line.EndsWith(","))
            {
                noteData.bars.Add(bar);
                bar = new List<Notes>();
            }
            else if (line.EndsWith(":"))
            {
                continue;
            }
            else if (line.Length >= 4)
            {
                Notes note = new Notes();
                note.left = false;
                note.down = false;
                note.up = false;
                note.right = false;

                
                if (line[0] != '0')
                {
                    note.left = true;
                }
                if (line[1] != '0')
                {
                    note.down = true;
                }
                if (line[2] != '0')
                {
                    note.up = true;
                }
                if (line[3] != '0')
                {
                    note.right = true;
                }

                bar.Add(note);
            }
        }

        return noteData;
    }


    //Because Unity doesnt have this fucking method like what the fuck
    public static bool IsNullOrWhiteSpace(string value)
    {
        if (value != null)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }
        }
        return true;
    }
}
