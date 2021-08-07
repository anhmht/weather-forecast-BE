﻿using System;

namespace GloboWeather.WeatherManagement.Application.Helpers.Common
{
    public class Constants
    {
        public const string SemiColonStringSeparator = ";";
        //public const string CommaStringSeparator = ",";
    }

    public class LookupNameSpace
    {
        public const string ActionType = "ACTION_TYPE";
        public const string ActionMethod = "ACTION_METHOD";
        public const string ActionAreaType = "ACTION_AREA_TYPE";
        public const string ScenarioActionType = "SCENARIO_ACTION_TYPE";
        public const string Position = "POSITION";
    }

    public class StorageContainer
    {
        public const string Image = "images";
        public const string Thumbnail = "thumbnails";
        public const string Temp = "temps";
        public const string Event = "events";
        public const string Post = "posts";
        public const string User = "users";
        public const string Scenarios = "scenarios";
    }

    public class EventStatus
    {
        public static Guid Publish = Guid.Parse("B0788D2F-8003-43C1-92A4-EDC76A7C5DDE");
        public static Guid Private = Guid.Parse("FE98F549-E790-4E9F-AA16-18C2292A2EE9");
        public static Guid Pending = Guid.Parse("BF3F3002-7E53-441E-8B76-F6280BE284AA");
        public static Guid Draft = Guid.Parse("6313179F-7837-473A-A4D5-A5571B43E6A6");
    }
}
