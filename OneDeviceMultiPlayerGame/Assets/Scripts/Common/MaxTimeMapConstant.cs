using System;
using System.Collections.Generic;
public class MaxTimeMapConstant {

    public static List<string> MAX_TIMES = new List<string>() {
        "5",
        "10",
        "15",
        "20",
        "30",
        "40",
        "50",
        "60",
    };

    public static string DEFAULT_MAX_TIME = "15";

    public static int DEFAULT_MAX_TIME_INDEX = MAX_TIMES.FindIndex(maxTime => maxTime.Equals(DEFAULT_MAX_TIME));
}