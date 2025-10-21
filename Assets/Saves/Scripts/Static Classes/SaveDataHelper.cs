using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.SaveData
{
    public static class SaveDataHelper
    {
        static char quoteMark = '"';
        static char leftBrace = '{';
        static char rightBrace = '}';

        public static string UpdateSaveDataFormatting<T>(string profileJsonData) where T : new()
        {
            string defaultJsonData = JsonUtility.ToJson(new T());

            //1) create 2 bijections
            string trimmedProfileJsonData = TrimJson(profileJsonData);
            string trimmedDefaultJsonData = TrimJson(defaultJsonData);

            Dictionary<string, string> profileBijection = GetBijection(trimmedProfileJsonData);
            Dictionary<string, string> defaultBijection  = GetBijection(trimmedDefaultJsonData);

            Dictionary<string, string> tunedProfileBijection = TuneBijection(profileBijection);
            Dictionary<string, string> tunedDefaultBijection = TuneBijection(defaultBijection);

            //2) comparing bijections and updating the profjile's bijection
            Dictionary<string, string> updatedBijection = GetUpdatedSavedBijection(tunedProfileBijection, tunedDefaultBijection);

            //3) constructing a json string with the updated profile's bijection
            string jsonString = ConstructJsonString(updatedBijection);

            return jsonString;
        }

        static Dictionary<string, string> GetUpdatedSavedBijection(Dictionary<string, string> profileBijection, Dictionary<string, string> defaultBijection)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            List<string> savedKeys = profileBijection.Keys.ToList();
            List<string> defaultKeys = defaultBijection.Keys.ToList();

            for (int i = 0; i < savedKeys.Count; i++)
            {
                string savedKey = savedKeys[i];
                string savedValue = profileBijection[savedKey];

                if (defaultKeys.Contains(savedKey))
                {
                    //Debug.Log($"<color=lime>CONTAINED IN DEFAULT KEYS : {savedKey}</color>");
                    result.Add(savedKey, savedValue);
                    defaultKeys.Remove(savedKey);
                }
                else
                {
                    //Debug.Log($"<color=yellow>MISSING IN DEFAULT KEYS : {savedKey}</color>");
                }
            }

            for (int i = 0; i < defaultKeys.Count; i++)
            {
                string defaultKey = defaultKeys[i];
                string defaultValue = defaultBijection[defaultKey];
                //Debug.Log($"<color=cyan>MISSING IN SAVED KEYS : {defaultKey} = {defaultValue}</color>");

                result.Add(defaultKey, defaultValue);
            }

            return result;
        }

        static string ConstructJsonString(Dictionary<string, string> bijection)
        {
            List<string> bijectionKeys = bijection.Keys.ToList();

            Dictionary<string, Dictionary<string, List<string>>> arrayClasses = new Dictionary<string, Dictionary<string, List<string>>>();

            for (int i = 0; i < bijectionKeys.Count; i++)
            {
                string key = bijectionKeys[i];
                string value = bijection[key];

                //check if the key has the '[' character
                if (key.Contains('['))
                {
                    //get the entire ancestry
                    int leftBraceIndex = key.IndexOf('[');
                    int rightBraceIndex = key.IndexOf("]");

                    string fullValueKey = key.Substring(0, leftBraceIndex);
                    string valueName = key.Substring(rightBraceIndex + 2);
                    int arrayIndex = int.Parse(key.Substring(leftBraceIndex + 1, rightBraceIndex - leftBraceIndex - 1));

                    string[] ancestry = fullValueKey.Split(".");

                    string arrayName = ancestry[ancestry.Length - 1];
                    string className = fullValueKey.Substring(0, fullValueKey.IndexOf(arrayName) - 1);

                    //check if the class name exists within the arrayClasses dictionary
                    if (!arrayClasses.ContainsKey(className))
                    {
                        arrayClasses.Add(className, new Dictionary<string, List<string>>());
                        arrayClasses[className].Add(arrayName, new List<string>());
                    }
                    else if (!arrayClasses[className].ContainsKey(arrayName))
                    {
                        arrayClasses[className].Add(arrayName, new List<string>());
                    }

                    //check with the length of the array
                    string valueToAdd = $"{Enquote(valueName)}:{value}";
                    //Debug.Log($"<color=magenta>{valueToAdd}</color>");
                    if (arrayClasses[className][arrayName].Count == arrayIndex)
                    {
                        arrayClasses[className][arrayName].Add(valueToAdd);
                    }
                    else
                    {
                        arrayClasses[className][arrayName][arrayIndex] += $",{valueToAdd}";
                    }
                }
            }

            //loop through bijection
            Dictionary<string, string> reducedBijection = new Dictionary<string, string>();

            for (int i = 0; i < bijectionKeys.Count; i++)
            {
                string key = bijectionKeys[i];

                if (key.Contains('['))
                {
                    //get the name of the class with the array (full parent)
                    int leftBraceIndex = key.IndexOf('[');

                    string arrayFullName = key.Substring(0, leftBraceIndex);
                    string[] ancestry = arrayFullName.Split(".");

                    string arrayName = ancestry[ancestry.Length - 1];
                    string className = string.Join('.', arrayFullName).Substring(0, arrayFullName.IndexOf(arrayName) - 1);

                    if (!reducedBijection.ContainsKey(arrayFullName))
                    {
                        string arrayString = "[";

                        List<string> chosenList = arrayClasses[className][arrayName];
                        for (int j = 0; j < chosenList.Count; j++)
                        {
                            string arrayData = chosenList[j];
                            string stringToAdd = "{" + $"{arrayData}" + "},";
                            arrayString += stringToAdd;
                        }
                        arrayString = arrayString.Remove(arrayString.Length - 1) + "]";
                        //Debug.Log("TOP");
                        //Debug.Log(key);
                        //Debug.Log(arrayFullName);
                        //Debug.Log(arrayString);
                        reducedBijection.Add(arrayFullName, arrayString);
                    }
                }
                else if (!(reducedBijection.ContainsKey(key) && bijection[key] == "[]"))
                {
                    //Debug.Log("BOTTOM");
                    //Debug.Log(key);
                    //Debug.Log(bijection[key]);
                    reducedBijection.Add(key, bijection[key]);
                }
            }

            string resultString = "";
            int nestLevel = 0;
            bool constructing = true;
            //do this recursively based on the number of dots of the bijection keys

            List<string> reducedBijectionKeys = reducedBijection.Keys.ToList();
            List<string> usedKeys = new List<string>();
            Dictionary<string, string> construction = new Dictionary<string, string>();

            for (int i = 0; i < reducedBijectionKeys.Count ; i++)
            {
                usedKeys.Add("");
            }

            while (constructing)
            {
                Dictionary<string, Dictionary<string, string>> hierarchy = new Dictionary<string, Dictionary<string, string>>();

                //process the reducec bijection and select the deepest nested classes first.
                //Debug.Log($"<color=orange>NEST LEVEL : {nestLevel}</color>");
                for (int i = 0; i < reducedBijectionKeys.Count; i++)
                {
                    string currentKey = reducedBijectionKeys[i];
                    string fullKey;

                    if (nestLevel == 0)
                    {
                        fullKey = currentKey;
                    }
                    else
                    {
                        fullKey = $"{usedKeys[i]}.{currentKey}";
                    }

                    //Debug.Log($"<color=yellow>{currentKey}</color>   <color=orange>{fullKey}</Color>   <color=yellow>{bijection[fullKey]}</color>");

                    //depending on what the nest level is, we can identify what is the main class of subsequent classes.
                    List<string> classByRank = currentKey.Split('.').ToList();
                    string firstClass = classByRank[0];
                    

                    if (!hierarchy.Keys.Contains(firstClass))
                    {
                        hierarchy.Add(firstClass, new Dictionary<string, string>());
                    }
                    classByRank.Remove(firstClass);
                    string remainingClasses = string.Join(".", classByRank);

                    if (!remainingClasses.Contains('.'))
                    {
                        //Debug.Log($"<color=cyan>Added to hierarchy - {firstClass} - {remainingClasses} : {reducedBijection[fullKey]}</color>");
                        hierarchy[firstClass].Add(remainingClasses, reducedBijection[fullKey]);
                    }

                    if (usedKeys[i] == "")
                    {
                        usedKeys[i] += firstClass;
                    }
                    else
                    {
                        usedKeys[i] += $".{firstClass}";
                    }
                }

                if (nestLevel == 1)
                {
                    Dictionary<string, string> childConstruction = ConstructNestedJsonString(hierarchy);
                    Dictionary<string, string[]> childParentClasses = GetChildParentClasses(usedKeys, nestLevel);

                    //loop through the child construction backwards.
                    List<string> childKeys = childConstruction.Keys.Reverse().ToList();

                    foreach (string childKey in childKeys)
                    {
                        string[] parents = childParentClasses[childKey];

                        string parentKey = parents[0];
                        string constructionData = $"{childKey}:{childConstruction[childKey]}";

                        if (construction[parentKey] == "{}")
                        {
                            construction[parentKey] = construction[parentKey].Insert(1, constructionData);
                        }
                        else
                        {
                            construction[parentKey] = construction[parentKey].Insert(1, constructionData + ",");
                        }
                    }
                }
                else if (nestLevel > 1)
                {
                    Dictionary<string, string> childConstruction = ConstructNestedJsonString(hierarchy);
                    Dictionary<string, string[]> childParentClasses = GetChildParentClasses(usedKeys, nestLevel);

                    int insertionIndex;
                    //loop through the child construction backwards.
                    List<string> childKeys = childConstruction.Keys.Reverse().ToList();

                    foreach (string childKey in childKeys)
                    {
                        string[] parents = childParentClasses[childKey];
                        string jsonConstruction = $"{childKey}:{childConstruction[childKey]}";

                        string parentKey = parents[0];
                        string immediateParentKey = parents[nestLevel - 1];

                        string targetClass = $"{immediateParentKey}:{leftBrace}";
                        string json = construction[parentKey];
                        insertionIndex = json.IndexOf(targetClass) - 1;

                        if (json.Substring(insertionIndex + targetClass.Length, 2) == "{}")
                        {
                            construction[parentKey] = json.Insert(insertionIndex + targetClass.Length + 1, jsonConstruction);
                            //showdict(construction);
                            //Debug.Log("<color=red>----------</color>");
                        }
                        else
                        {
                            construction[parentKey] = json.Insert(insertionIndex + targetClass.Length + 1, jsonConstruction + ",");
                            //showdict(construction);
                            //Debug.Log("<color=cyan>----------</color>");
                        }
                    }
                }
                else
                {
                    construction = ConstructNestedJsonString(hierarchy);
                }

                //preparing the fields for next iteration.
                List<string> newKeys = new List<string>();
                List<int> removalIndices = new List<int>();

                for (int i = 0; i < reducedBijectionKeys.Count; i++)
                {
                    List<string> classes = reducedBijectionKeys[i].Split('.').ToList();
                    if (classes.Count == 2)
                    {
                        removalIndices.Add(i);
                        continue;
                    }
                    else if (hierarchy.Keys.Contains(classes[0]))
                    {
                        classes.Remove(classes[0]);
                        string reducedString = string.Join(".", classes);
                        newKeys.Add(reducedString);
                    }
                }

                for (int i = usedKeys.Count - 1; i >= 0; i--)
                {
                    if (removalIndices.Contains(i))
                    {
                        usedKeys.RemoveAt(i);
                    }
                }

                //set currentKeys
                reducedBijectionKeys = newKeys;

                //increment nest level
                nestLevel++;

                if (reducedBijectionKeys.Count == 0 || nestLevel > 300)
                {
                    constructing = false;
                    if (reducedBijectionKeys.Count > 0)
                    {
                        Debug.LogWarning("FORCED BREAK OUT OF WHILE LOOP");
                    }
                }
            }

            //showdict(construction);

            foreach (string parentKey in construction.Keys)
            {
                resultString += $",{parentKey}:{construction[parentKey]}";
            }
            string finalJsonString = $"{leftBrace}{resultString.Substring(1).Replace('|', ',')}{rightBrace}".Replace('<', '{').Replace('>', '}');
            return finalJsonString;
        }

        static Dictionary<string, string[]> GetChildParentClasses(List<string> usedKeys, int nestLevel)
        {
            //create a dictionary where the key is the current class value of the parent class
            Dictionary<string, string[]> childClasses = new Dictionary<string, string[]>();
            for (int i = 0; i < usedKeys.Count; i++)
            {
                string[] parentChildClass = usedKeys[i].Split('.');
                string childName = Enquote(parentChildClass[nestLevel]);
                //Debug.Log($"{usedKeys[i]}, {childName}, {nestLevel}");

                string[] parents = parentChildClass.Select(parent => Enquote(parent)).Where(parent => parent != childName).ToArray();
                if (!childClasses.ContainsKey(childName))
                {
                    //Debug.Log($"{childName} {parents}");

                    childClasses.Add(childName, parents);
                }
            }
            return childClasses;
        }

        static Dictionary<string, string> ConstructNestedJsonString(Dictionary<string, Dictionary<string, string>> hierarchy)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (string key in hierarchy.Keys)
            {
                //Debug.Log($"<color=white>{key}</color>");

                string jsonClassString = ConstructPrimitiveOrClassArrayJson(hierarchy[key]);
                //Debug.Log($"<color=orange>{jsonClassString}</color>");
                result.Add(Enquote(key), jsonClassString);
            }
            return result;
        }

        static string ConstructPrimitiveOrClassArrayJson(Dictionary<string, string> fields)
        {
            string baseString = ""; //$"{Enquote(className)}:";
            string primitives = "{}";

            foreach (string field in fields.Keys)
            {
                if (fields[field].Contains("[{"))
                {
                    string pairingString = $"{Enquote(field)}:{fields[field]},";
                    primitives = primitives.Insert(primitives.Length - 1, pairingString);
                    //Debug.Log(primitives);
                }
                else
                {
                    string pairingString = $"{Enquote(field)}:{fields[field]},";
                    primitives = primitives.Insert(primitives.IndexOf('}'), pairingString);
                }

            }
            if (primitives != "{}")
            {
                baseString = $"{baseString}{primitives}";
                baseString = baseString.Remove(baseString.Length - 2);
                baseString = $"{baseString}{rightBrace}";
            }
            else
            {
                baseString = "{}";
            }
            return baseString;
        }

        static string Enquote(string str)
        {
            if (str.Contains(quoteMark)) return str;
            return $"{quoteMark}{str}{quoteMark}";
        }

        private static Dictionary<string, string> GetBijection(string jsonString, int iteration = 0, string prefix = "", Dictionary<string, string> bijection = null)
        {
            if (bijection == null) { bijection = new Dictionary<string, string>(); }

            iteration++;
            Dictionary<string, string> nestedBijection = GetKeyAndValuePairs(jsonString);

            //check the values of the current dictionary and see if (certain condition) is not met. in that case, run this method on the values
            foreach (string bijectionKey in nestedBijection.Keys)
            {
                string fullKey = (prefix == "") ? bijectionKey : $"{prefix}.{bijectionKey}";

                string bijectionValue = TrimJson(nestedBijection[bijectionKey]);
                string debugString = $"{bijectionKey} -> {bijectionValue}";

                //1) check if there are arrays of classes.
                if (bijectionValue.Contains("[{") && bijectionValue.Contains("}]"))
                {
                    //if there are, reformat them slightly.

                    bijectionValue = FormatStringWithClassArray(bijectionValue);
                    //Debug.Log($"<color=cyan> ({prefix}) FORMATTED STRING WITH CLASS ARRAY(S) - {bijectionValue}</color>");
                }

                //2) check if the value has any instances of '{' or '}'. (ideally, it should have EITHER: both or neither. it cannot have 1 and not the other.
                if (bijectionValue.Contains('{') && bijectionValue.Contains('}'))
                {
                    //Debug.Log($"<color=yellow> ({iteration}) NEED TO NEST FURTHER - {debugString}</color>");
                    GetBijection(TrimJson(bijectionValue), iteration, fullKey, bijection);
                }
                else if (!(bijectionValue.Contains('{') || bijectionValue.Contains('}')))
                {
                    //Debug.Log($"<color=lime> ({fullKey}) NO NEED TO NEST FURTHER (base step) - {debugString}</color>");
                    bijection.Add(fullKey, bijectionValue);
                }
                else
                {
                    Debug.LogError($"<color=red>ERROR - {debugString} HAS ONE BRACE AND NOT THE OTHER</color>");
                }
            }

            return bijection;
        }

        private static Dictionary<string, string> GetKeyAndValuePairs(string jsonString)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            int leftBraceCount = 0;
            int rightBraceCount = 0;

            int leftBraceIndex = 0;
            int rightBraceIndex = 0;

            int keyIndex = 0;

            string key = "";

            //loop through jsonString
            for (int i = 0; i < jsonString.Length; i++)
            {
                char letter = jsonString[i];

                //keep track of left and right brace count
                if (letter == '{') { leftBraceCount++; leftBraceIndex = i; }
                else if (letter == '}') { rightBraceCount++; rightBraceIndex = i; }

                //check if the first left brace is found
                if (leftBraceCount == 1 && rightBraceCount == 0 && leftBraceIndex == i)
                {
                    //key identified
                    key = jsonString.Substring(keyIndex, i - keyIndex);
                    //Debug.Log($"<color=lime>{key} ({i}, {letter})</color>");
                }

                //check if left and right braces match.
                else if (leftBraceCount == rightBraceCount && rightBraceIndex == i && i > 0)
                {
                    //value identified
                    //Debug.Log($"<color=cyan>({i}, {letter})</color>");
                    string value = jsonString.Substring(keyIndex + key.Length, i - keyIndex - key.Length + 1);
                    //Debug.Log($"<color=cyan>{value}</color>");

                    leftBraceCount = 0;
                    rightBraceCount = 0;

                    keyIndex = i + 2;

                    result.Add(key, value);
                }
            }

            return result;
        }

        private static string TrimJson(string fullJson)
        {
            //Debug.Log(fullJson);
            //check 
            int leftBracesb = 0;
            int rightBracesb = 0;
            for (int i = 0; i < fullJson.Length; i++)
            {
                char c = fullJson[i];
                if (c == '{') { leftBracesb++; }
                else if (c == '}') { rightBracesb++; }
            }
            //Debug.Log($"BEFORE TRIMMING: left - {leftBracesb}, right - {rightBracesb}");

            if (fullJson[0] == '{')
            {
                fullJson = fullJson.Substring(0, fullJson.Length - 1).TrimStart('{');
            }

            int leftBracesa = 0;
            int rightBracesa = 0;
            for (int i = 0; i < fullJson.Length; i++)
            {
                char c = fullJson[i];
                if (c == '{') { leftBracesa++; }
                else if (c == '}') { rightBracesa++; }
            }
            //Debug.Log($"AFTER TRIMMING: left - {leftBracesa}, right - {rightBracesa}");
            return fullJson;
        }

        private static string FormatStringWithClassArray(string value)
        {
            string result = "";
            bool insideClassArray = false;

            for (int i = 0; i < value.Length; i++)
            {
                char letter = value[i];

                if (insideClassArray)
                {
                    if (letter == '{') { result += '<'; }
                    else if (letter == '}') { result += '>'; }
                    else { result += letter; }
                }
                else
                {
                    result += letter;
                }

                if (!insideClassArray && letter == '[')
                {
                    insideClassArray = true;
                }
                else if (insideClassArray && letter == ']')
                {
                    insideClassArray = false;
                }
            }

            return result;
        }

        private static Dictionary<string, string> TuneBijection(Dictionary<string, string> bijection)
        {
            Dictionary<string, string> tunedBijection = new Dictionary<string, string>();

            foreach (string key in bijection.Keys)
            {
                string trimmedKey = key.Replace('"', '?').Replace("?", "").Replace(":", "");
                string value = bijection[key];

                if (value.Contains("[") && value.Contains("]"))
                {
                    value = ModifyArrayString(value);
                }

                string[] primitives = value.Split(',');

                foreach (string primitive in primitives)
                {
                    string immediateKey = primitive.Substring(0, primitive.IndexOf(':')).Trim('"');
                    string trueValue = primitive.Substring(primitive.IndexOf(':') + 1, primitive.Length - primitive.IndexOf(':') - 1);

                    string trueKey = string.Join('.', trimmedKey, immediateKey);

                    tunedBijection.Add(trueKey, trueValue);
                }
            }

            return tunedBijection;
        }

        private static string ModifyArrayString(string value)
        {
            string result = "";
            bool inArray = false;
            foreach (char letter in value)
            {
                if (!inArray && letter == '[') { inArray = true; }
                else if (inArray && letter == ']') { inArray = false; }

                if (inArray && letter == ',') { result += '|'; }
                else { result += letter; }
            }

            return result;
        }
    }
}