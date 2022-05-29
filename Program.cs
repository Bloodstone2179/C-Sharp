using VideoLibrary;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using MediaToolkit;
using MediaToolkit.Util;
using MediaToolkit.Options;
using MediaToolkit.Model;
using CryMediaAPI.Audio;
using System;




public class program {
    public static string YouTubeVideoLink = "";
    public static string AppData = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    public static string folder = "/Music Player/";
    public static List<string> pathAndFileName = new List<string>();
    public static string choice = "";
    
    public static void Load()
    {
        string[] SaveFile = File.ReadAllLines(AppData + folder + "SavedPlaylist.txt");
        foreach (string line in SaveFile)
        {
            pathAndFileName.Add(line);
        }
        

    }
    public static void Save()
    {
        
        File.WriteAllLines(AppData + @"/SavedPlaylist.txt", pathAndFileName);//path and the file to write to // the data to write to said file
    }
    
    public static void Main()  {
        Load();
        WhatToDo();
    }

    public static void Download( string link)
    {
        

        //downloading part
        var youTube = YouTube.Default;
        var video = youTube.GetVideo(link);
        var fullName = video.FullName;
        var title = video.Title;


        byte[] bytes = video.GetBytes();
        var stream = video.Stream();
        //string user = Environment.UserName;
        
        
        var Folder = @"\Music Player\Player\"; // where theses files are stored
        var path = AppData + Folder;

        Console.WriteLine(path);
        File.WriteAllBytes(path + fullName, bytes);
        Console.WriteLine("File downloaded and saved in the directory of " + path);
        
        string fullNameFile = path + fullName;
        Console.WriteLine("File Name " + fullNameFile);
        Convertfile(fullName, path, title);
        

    }
    public static void Convertfile(string InputFile, string path,string name)
    {
        Console.WriteLine("Conversion in progress");
        
        var Folder = @"\Music Player\";
        var path_ = AppData + Folder;
        var localAppData = Path.Combine(path_, @"Resources\FFMpeg\bin");
        var FFMPEG_PATH = Path.Combine(localAppData, "ffmpeg.exe");
        var inputFile = new MediaFile { Filename =  path + InputFile};
        var outputFile = new MediaFile { Filename = path + name + ".mp3"};
        
        Console.WriteLine("PATH_: " + path);

        var conversionOptions = new ConversionOptions
        {
            //VideoAspectRatio = VideoAspectRatio.R16_9,
            //VideoSize = VideoSize.Hd1080,
            AudioSampleRate = AudioSampleRate.Hz44100
        };

        using (var engine = new Engine(FFMPEG_PATH))
        {
            
            engine.Convert(inputFile, outputFile, conversionOptions);
            //engine.Convert(inputFile, outputFile);
            Console.WriteLine("Conversion Complete");
            File.Delete(Path.Combine(path, InputFile));
            var file =  outputFile.Filename;
            pathAndFileName.Add(file);
            //Play(path, outputFile.Filename);
            WhatToDo();
           // Save();
        }
    }
    public static void WhatToDo()
    {
        Console.WriteLine("What do You Want to do Now");
        Console.WriteLine("(Play) a song in the playlist, (See) the playlist, (Download) and the song to the playlist, (Stop) the song");
        choice = Console.ReadLine().ToString();
        if(choice == "See" || choice == "see")
        {
            foreach (string song in pathAndFileName)
            {
                Console.WriteLine(pathAndFileName.IndexOf(song) + ". ," + song);
            }
            WhatToDo();
        }
        if(choice == "Play" || choice == "play")
        {
            if(pathAndFileName.Count() == 0)
            {
                Console.WriteLine("Playlist is empty");
                WhatToDo();
            }
            foreach(string song in pathAndFileName)
            {
                Console.WriteLine(pathAndFileName.IndexOf(song) + ". ," + song);
            }
            Console.WriteLine("Choose the song number to play: ");
            var Song = Console.ReadLine().ToString();
            int songIndex = int.Parse(Song);
            Play(pathAndFileName[songIndex]);
        }
        if(choice == "Download" || choice == "download")
        {
            Console.WriteLine("Enter the Youtube Video URL: ");
            string YouTubeVideoLink = Console.ReadLine().ToString();
            Download(YouTubeVideoLink);
        }
    }
    public static void Play(string file)
    {
        //Save();
        var fileName = Path.Combine(AppData, file);
        var player = new AudioPlayer(fileName);
        player.Play();
        WhatToDo();
        if(choice == "Stop" || choice == "stop")
        {
            player.Dispose();
            WhatToDo();
        }

    }



}

