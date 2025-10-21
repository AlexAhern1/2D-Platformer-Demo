using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Logger : MonoBehaviour
    {
        [SerializeField] private bool _ignoreLoggingCalls;

        [Header("Logging classes")]
        [Separator]

        [Header("Controller")]
        [SerializeField] private bool _allowControllerLogs;

        [Header("Player")]
        [SerializeField] private bool _allowMovementLogs;
        [SerializeField] private bool _allowCombatLogs;
        [SerializeField] private bool _allowCollectingLogs;

        [Header("World")]
        [SerializeField] private bool _allowSceneLoadingLogs;
        [SerializeField] private bool _allowLevelTransitionLogs;
        [SerializeField] private bool _allowPlayerSpawningLogs;

        [Header("Stats")]
        [SerializeField] private bool _allowStatModificationLogs;

        [Header("Enemies")]
        [SerializeField] private bool _allowEnemySpawningLogs;
        [SerializeField] private bool _allowEnemyDespawningLogs;
        [SerializeField] private bool _allowEnemyDeathLogs;
        [SerializeField] private bool _allowEnemyCombatLogs;
        [SerializeField] private bool _allowEnemyErrorLogs;
        [SerializeField] private bool _allowEnemyAIStateLogs;

        [Header("Load/Save")]
        [SerializeField] private bool _allowLoadingDataLogs;
        [SerializeField] private bool _allowSavingDataLogs;

        [Header("Asynchronous Operations")]
        [SerializeField] private bool _allowAddressableLogs;

        [Header("Refactoring")]
        [SerializeField] private bool _allowRefactoringLogs;

        [Header("FX")]
        [SerializeField] private bool _allowAudioLogs;

        [Header("Error messages")]
        [SerializeField] private bool _ignoreErrorMessages;

        //controller logs
        public static bool Controller => _instance._allowControllerLogs;

        //player logs
        public static bool Movement => _instance._allowMovementLogs;
        public static bool Combat => _instance._allowCombatLogs;
        public static bool Collecting => _instance._allowCollectingLogs;

        //world logs
        public static bool SceneLoading => _instance._allowSceneLoadingLogs;
        public static bool LevelTransition => _instance._allowLevelTransitionLogs;
        public static bool PlayerSpawning => _instance._allowPlayerSpawningLogs;

        //stats logs
        public static bool StatModification => _instance._allowStatModificationLogs;

        //enemies logs
        public static bool EnemySpawning => _instance._allowEnemySpawningLogs;
        public static bool EnemyDespawning => _instance._allowEnemyDespawningLogs;
        public static bool EnemyDeath => _instance._allowEnemyDeathLogs;
        public static bool EnemyCombat => _instance._allowEnemyCombatLogs;
        public static bool EnemyError => _instance._allowEnemyErrorLogs;
        public static bool EnemyAIState => _instance._allowEnemyAIStateLogs;

        //load/save logs
        public static bool DataLoading => _instance._allowLoadingDataLogs;
        public static bool DataSaving => _instance._allowSavingDataLogs;

        //async logs
        public static bool Addressables => _instance._allowAddressableLogs;

        //refactoring logs
        public static bool Refactoring => _instance._allowRefactoringLogs;

        //audio logs
        public static bool Audio => _instance._allowAudioLogs;

        public static bool Active => _instance != null;

        private static Logger _instance;

        private List<LineSegment> _lineSegments = new List<LineSegment>();
        private List<Circle> _circles = new List<Circle>();

        private void Awake() { if (_instance == null) _instance = this; }

        public static void Log(object obj)
        {
            if (_instance == null) { LogWithoutInstance(obj, default, Debug.Log); return; }
            _instance.LogMessage(obj, Debug.Log);
        }

        public static void Log(object obj, Color color = default)
        {
            if (_instance == null) { LogWithoutInstance(obj, color, Debug.Log); return; }
            string hexString = GetHexString(color);
            _instance.LogMessage(obj, Debug.Log, hexString);
        }

        private static void LogWithoutInstance(object obj, Color color, Action<string> logAction)
        {
            string hexString = GetHexString(color);
            logAction($"<color={hexString}>{obj}</color>");
        }

        public static void Log(object obj, bool logGroup, Color color = default)
        {
            if (!logGroup) return;
            Log(obj, color);
        }

        public static void LogRaw(object obj, Color color = default)
        {
            LogWithoutInstance(obj, color, Debug.Log);
        }

        public static void LogCollection(IEnumerable<object> objs, Color color = default)
        {
            if (_instance == null) { Debug.LogWarning("Logger not found!"); return; }
            string hexString = GetHexString(color);
            foreach (object obj in objs)
            {
                _instance.LogMessage(obj, Debug.Log, hexString);
            }
        }

        public static void LogCollection(IEnumerable<object> objs, bool logGroup, Color color = default)
        {
            if (!logGroup) return;
            LogCollection(objs, color);
        }

        public static void Warn(object obj)
        {
            if (_instance == null) { LogWithoutInstance(obj, default, Debug.LogWarning); return; }
            _instance.LogMessage(obj, Debug.LogWarning);
        }

        public static void Warn(object obj, Color color = default)
        {
            if (_instance == null) { Debug.LogWarning("Logger not found!"); return; }
            string hexString = GetHexString(color);
            _instance.LogMessage(obj, Debug.LogWarning, hexString);
        }

        public static void Warn(object obj, bool logGroup, Color color)
        {
            if (!logGroup) return;
            Warn(obj, color);
        }

        public static void Error(object obj, bool logGroup, Color color)
        {
            if (!logGroup && _instance._ignoreErrorMessages) return;
            Error(obj, color);
        }

        public static void Error(object obj, Color color = default)
        {
            if (color == default) color = MoreColors.BrightRed;

            if (_instance == null) { Debug.LogWarning("Logger not found!"); return; }
            string hexString = GetHexString(color);
            _instance.LogMessage(obj, Debug.LogError, hexString);
        }

        public static void DrawLine(Vector3 start, Vector3 end, float duration, Color color)
        {
            LineSegment line = new(start, end, duration, color);
            _instance._lineSegments.Add(line);
        }

        public static void DrawBox(Bounds bounds, float duration, Color color)
        {
            DrawBox(bounds.center, bounds.size, duration, color);
        }

        public static void DrawBox(Bounds bounds, Vector3 offset, float duration, Color color)
        {
            DrawBox(bounds.center + offset, bounds.size, duration, color);
        }

        public static void DrawBox(Vector2 start, Vector2 size, float duration, Color color)
        {
            if (_instance == null) { Debug.LogWarning("Logger not found!"); return; }

            //starts from top-left and goes anti-clockwise
            LineSegment line1 = new LineSegment(new Vector2(start.x - size.x, start.y + size.y),
                                                new Vector2(start.x - size.x, start.y - size.y),
                                                duration, color);
            _instance._lineSegments.Add(line1);

            LineSegment line2 = new LineSegment(new Vector2(start.x - size.x, start.y - size.y),
                                                new Vector2(start.x + size.x, start.y - size.y),
                                                duration, color);
            _instance._lineSegments.Add(line2);


            LineSegment line3 = new LineSegment(new Vector2(start.x + size.x, start.y - size.y),
                                                new Vector2(start.x + size.x, start.y + size.y),
                                                duration, color);
            _instance._lineSegments.Add(line3);

            LineSegment line4 = new LineSegment(new Vector2(start.x + size.x, start.y + size.y),
                                                new Vector2(start.x - size.x, start.y + size.y),
                                                duration, color);
            _instance._lineSegments.Add(line4);
        }

        public static void DrawCircle(Vector2 center, float radius, float duration, Color color)
        {
            if (_instance == null) { Debug.LogWarning("Logger not found!"); return; }

            Circle circle = new(center, radius, duration, color);
            _instance._circles.Add(circle);
        }

        public static void DrawBoxTriggers(Bounds bounds, float duration, LayerMask layermask)
        {
            DrawBoxTriggers(bounds.center, bounds.extents, duration, layermask);
        }

        public static void DrawBoxTriggers(Bounds bounds, Vector3 offset, float duration, LayerMask layermask, Color sourceColor, Color detectedColor)
        {
            DrawBoxTriggers(bounds.center + offset, bounds.size, duration, layermask, sourceColor, detectedColor);
        }

        public static void DrawBoxTriggers(Bounds bounds, float duration, LayerMask layermask, Color sourceColor, Color detectedColor)
        {
            DrawBoxTriggers(bounds.center, bounds.size, duration, layermask, sourceColor, detectedColor);
        }

        public static void DrawBoxTriggers(Vector2 center, Vector2 size, float duration, LayerMask layermask)
        {
            DrawBoxTriggers(center, size, duration, layermask, Color.green, Color.yellow);
        }

        public static void DrawBoxTriggers(Vector2 center, Vector2 extends, float duration, LayerMask layerMask, Color sourceColor, Color detectedColor)
        {
            DrawBox(center, extends, duration, sourceColor);

            RaycastHit2D[] hits = Physics2D.BoxCastAll(center, extends, 0f, Vector2.zero, 0f, layerMask);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == null) continue;
                DrawBox(hit.collider.bounds, duration, detectedColor);
            }
        }

        private void LogMessage(object obj, Action<string> logAction, string hexColor = "#CBCBCB")
        {
            if (_ignoreLoggingCalls) return;
            string message = GetString(obj);
            logAction($"<color={hexColor}>{message}</color>");
        }

        private string GetString(object obj)
        {
            if (obj is null)
            {
                return $"<color=red>Null</color>";
            }
            else
            {
                return obj.ToString();
            }
        }

        private static string GetHexString(Color color)
        {
            if (color.r == 0 && color.g == 0 && color.b == 0)
            {
                color.r = 1;
                color.g = 1;
                color.b = 1;
            }

            string rHex = Convert.ToString((int)(color.r * 255), 16);
            string gHex = Convert.ToString((int)(color.g * 255), 16);
            string bHex = Convert.ToString((int)(color.b * 255), 16);

            string hexString = "#" + (rHex.Length == 1 ? rHex + "0" : rHex[..2])
                                   + (gHex.Length == 1 ? gHex + "0" : gHex[..2])
                                   + (bHex.Length == 1 ? bHex + "0" : bHex[..2]);

            return hexString;
        }

        private void OnDrawGizmos()
        {
            for (int i = _lineSegments.Count - 1; i >= 0; i--)
            {
                LineSegment lineSegment = _lineSegments[i];

                Gizmos.color = lineSegment.Color;
                Gizmos.DrawLine(lineSegment.Start, lineSegment.End);

                if (Time.time >= lineSegment.EndTime)
                {
                    _lineSegments.Remove(lineSegment);
                }
            }

            for (int i = _circles.Count - 1; i >= 0; i--)
            {
                Circle circle = _circles[i];

                Gizmos.color = circle.Color;
                Gizmos.DrawWireSphere(circle.Center, circle.Radius);

                if (Time.time >= circle.EndTime)
                {
                    _circles.Remove(circle);
                }
            }
        }
    }

    public struct LineSegment
    {
        public Vector2 Start;
        public Vector2 End;
        public Color Color;

        public float EndTime;

        public LineSegment(Vector2 start, Vector2 end, float duration, Color color)
        {
            Start = start;
            End = end;
            Color = color;

            EndTime = Time.time + duration;
        }
    }

    public struct Circle
    {
        public Vector2 Center;
        public float Radius;
        public Color Color;

        public float EndTime;

        public Circle(Vector2 center, float radius, float duration, Color color)
        {
            Center = center;
            Radius = radius;
            Color = color;

            EndTime = Time.time + duration;
        }
    }
}