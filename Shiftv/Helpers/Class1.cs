﻿//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using Microsoft.Media.WebVTT;

//namespace Shiftv.Helpers
//{
//    public static class WebVttParser
//    {
//        static readonly char[] cueHeaderSeparator;
//        static readonly char[] timeStampSeparator;
//        const string noteIdentifier = "NOTE";
//        const string bodyIdentifier = "WEBVTT";
//        const string cueIdentifier = "-->";
//        const string cueSettingVertical = "vertical";
//        const string cueSettingVerticalGrowingLeft = "rl";
//        const string cueSettingVerticalGrowingRight = "lr";
//        const string cueSettingLinePosition = "line";
//        const string cueSettingTextPosition = "position";
//        const string cueSettingSize = "size";
//        const string cueSettingAlignment = "align";
//        const string cueSettingAlignmentStart = "start";
//        const string cueSettingAlignmentMiddle = "middle";
//        const string cueSettingAlignmentEnd = "end";
//        const string cueSettingAlignmentLeft = "left";
//        const string cueSettingAlignmentRight = "right";

//        static WebVttParser()
//        {
//            cueHeaderSeparator = new char[] { ' ' };
//            timeStampSeparator = new char[] { ':', '.' };
//        }

//        public static WebVTTDocument ParseDocument(string content, TimeSpan startTime, TimeSpan endTime)
//        {
//            using (var reader = new StringReader(content))
//            {
//                return ParseDocument(reader, startTime);
//            }
//        }

//        private static WebVTTDocument ParseDocument(StringReader reader, TimeSpan timeOffset)
//        {
//            var blockReader = new WebVtt(reader);
//            if (!blockReader.ReadBlock().StartsWith(bodyIdentifier)) throw new Exception("Invalid WebVTT start identifier");

//            var result = new WebVTTDocument();
//            WebVTTCue previousCue = null;
//            while (true)
//            {
//                var block = blockReader.ReadBlock();
//                if (block == null) break;
//                else if (!block.StartsWith(noteIdentifier)) // ignore comments
//                {
//                    // cue found
//                    var cue = ParseCue(block, timeOffset);
//                    if (previousCue != null && cue.Begin < previousCue.Begin) throw new Exception("Invalid WebVTT cue start time");
//                    if (cue.End <= cue.Begin) throw new Exception("Invalid WebVTT cue end time");
//                    result.Cues.Add(cue);
//                    previousCue = cue;
//                }
//            }
//            return result;
//        }

//        private static WebVTTCue ParseCue(string block, TimeSpan timeOffset)
//        {
//            var cueReader = new StringReader(block);

//            WebVTTCue result = new WebVTTCue();
//            var line = cueReader.ReadLine();
//            if (!line.Contains(cueIdentifier))
//            {
//                result.StyleClass = line;
//                line = cueReader.ReadLine();
//            }

//            var parts = line.Split(cueHeaderSeparator, StringSplitOptions.RemoveEmptyEntries);
//            if (parts[1] != cueIdentifier) throw new Exception("Invalid WebVTT cue time separator");
//            result.Begin = ParseTimeStamp(parts[0]).Add(timeOffset);
//            result.End = ParseTimeStamp(parts[2]).Add(timeOffset);
//            if (parts.Length >= 3)
//            {
//                var settingsDictionary = parts.Skip(3)
//                    .Select(p => p.Split(':'))
//                    .ToDictionary(i => i[0], i => i[1]);
//                result.Settings = ParseCueSettings(settingsDictionary);
//            }

//            StringBuilder cueContent = new StringBuilder();
//            do
//            {
//                line = cueReader.ReadLine();
//                if (line == null) break;
//                if (line.Contains(cueIdentifier)) throw new Exception("Invalid WebVTT cue content");
//                if (cueContent.Length > 0) cueContent.AppendLine();
//                cueContent.Append(line);

//            } while (true);
//            //  result.Content = ParseCueContent(cueContent.ToString(), timeOffset);

//            return result;
//        }
//        private static WebVTTCueSettings ParseCueSettings(IDictionary<string, string> values)
//        {
//            var result = new WebVTTCueSettings();
//            foreach (var item in values)
//            {
//                switch (item.Key)
//                {
//                    case cueSettingVertical:
//                        switch (item.Value)
//                        {
//                            case cueSettingVerticalGrowingLeft:
//                                result.WritingMode = WebVTTWritingMode.VerticalGrowingLeft;
//                                break;
//                            case cueSettingVerticalGrowingRight:
//                                result.WritingMode = WebVTTWritingMode.VerticalGrowingRight;
//                                break;
//                            default:
//                                throw new Exception("Invalid WebVTT cue vertical text setting");
//                        }
//                        break;
//                    case cueSettingLinePosition:
//                        if (item.Value.EndsWith("%"))
//                        {
//                            result.LinePosition = int.Parse(item.Value.TrimEnd('%'));
//                            result.SnapToLines = false;
//                        }
//                        else
//                        {
//                            result.LinePosition = int.Parse(item.Value);
//                            result.SnapToLines = true;
//                        }
//                        break;
//                    case cueSettingTextPosition:
//                        result.TextPosition = int.Parse(item.Value.TrimEnd('%'));
//                        break;
//                    case cueSettingSize:
//                        result.Size = int.Parse(item.Value.TrimEnd('%'));
//                        break;
//                    case cueSettingAlignment:
//                        switch (item.Value)
//                        {
//                            case cueSettingAlignmentStart:
//                                result.Alignment = WebVTTAlignment.Start;
//                                break;
//                            case cueSettingAlignmentMiddle:
//                                result.Alignment = WebVTTAlignment.Middle;
//                                break;
//                            case cueSettingAlignmentEnd:
//                                result.Alignment = WebVTTAlignment.End;
//                                break;
//                            case cueSettingAlignmentLeft:
//                                result.Alignment = WebVTTAlignment.Left;
//                                break;
//                            case cueSettingAlignmentRight:
//                                result.Alignment = WebVTTAlignment.Right;
//                                break;
//                            default:
//                                throw new Exception("Invalid WebVTT cue align setting");
//                        }
//                        break;
//                }
//            }
//            return result;
//        }

//        private static TimeSpan ParseTimeStamp(string source)
//        {
//            var parts = source.Split(timeStampSeparator).Reverse().ToArray();
//            int hours = parts.Length > 3 ? int.Parse(parts[3]) : 0;
//            int minutes = int.Parse(parts[2]);
//            int seconds = int.Parse(parts[1]);
//            int milliseconds = int.Parse(parts[0]);
//            return new TimeSpan(0, hours, minutes, seconds, milliseconds);
//        }
//    }

//}
