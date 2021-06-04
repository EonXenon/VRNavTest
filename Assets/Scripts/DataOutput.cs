using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class DataInOut
{

    public struct GameConfig
    {
        public int rayCount;
        public bool disableSSR;
        public float dragRotationSensitivity;
        public float directionalContinuousRotationSpeed;
        public float paddlingTranslationSpeed;
        public float dragNGoDefaultDistance;
        public int interpolatedInputFrames;
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
                        case "disablessr":
                            config.disableSSR = bool.Parse(temp[1]);
                            continue;
                        case "raycount":
                            config.rayCount = int.Parse(temp[1]);
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
                        case "interpolatedinputframes":
                            config.interpolatedInputFrames = int.Parse(temp[1]);
                            continue;
                        default:
                            Debug.Log("Could not read: " + line);
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
