using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class DataInOut
{

    public struct GameConfig
    {
        public float targetFramerate;
        public float maximumResolutionPercentage;
        public float minimumResolutionPercentage;
        public float safetyMarginPercentage;
        public float dragRotationSensitivity;
        public float directionalContinuousRotationSpeed;
        public float paddlingTranslationSpeed;
        public float dragNGoDefaultDistance;
    }

    public static GameConfig config;

    public static string sessionID;

    public static async Task Write(string s)
    {
        using StreamWriter file = new StreamWriter("results_" + sessionID + ".csv", append: true);
        await file.WriteLineAsync(s);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void ReadINIConfig()
    {

        sessionID = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);

        try
        {
            using (StreamReader sr = new StreamReader("config.ini"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] temp = line.Split('=');
                    switch (temp[0].Trim(' ').ToLower())
                    {
                        case "targetframerate":
                            config.targetFramerate = float.Parse(temp[1]);
                            continue;
                        case "maximumresolutionpercentage":
                            config.maximumResolutionPercentage = float.Parse(temp[1]);
                            continue;
                        case "minimumresolutionpercentage":
                            config.minimumResolutionPercentage = float.Parse(temp[1]);
                            continue;
                        case "safetymarginpercentage":
                            config.safetyMarginPercentage = float.Parse(temp[1]);
                            continue;
                        case "dragrotationsensitivity":
                            config.dragRotationSensitivity = float.Parse(temp[1]);
                            continue;
                        case "directionalcontinuousrotationspeed":
                            config.directionalContinuousRotationSpeed = float.Parse(temp[1]);
                            continue;
                        case "paddlingtranslationspeed":
                            config.paddlingTranslationSpeed = float.Parse(temp[1]);
                            continue;
                        case "dragngodefaultdistance":
                            config.dragNGoDefaultDistance = float.Parse(temp[1]);
                            continue;
                        case "overrideid":
                            sessionID = temp[1].Trim(' ').ToLower();
                            continue;
                        default:
                            continue;
                    }
                }
            }
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Debug.LogError("DataInOut: " + e.Message);
        }
    }
}
