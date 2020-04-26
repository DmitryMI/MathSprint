using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Assets.Scripts.MathTrials.Exercises;
using UnityEngine;
using Object = System.Object;

namespace Assets.Scripts.MathTrials
{
    public static class ExercisesLoader
    {
        public class XmlParserException : Exception
        {
            public XmlParserException(string message) : base(message)
            {

            }
        }

        public struct XmlParsedData
        {
            public string Text { get; set; }
            public string ImageName { get; set; }
            public string Answer { get; set; }
        }

        public static XmlParsedData ParseXml(string xml)
        {
            XmlParsedData data = new XmlParsedData();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            XmlNodeList rootNodes = xmlDocument.GetElementsByTagName("Exercise");

            if (rootNodes == null || rootNodes.Count == 0)
            {
                throw new XmlParserException("Root element must be named \"Exercise\"");
            }

            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Text");
            if (nodeList == null || nodeList.Count == 0)
            {
                throw new XmlParserException("Node \"Text\" does not exist");
            }
            XmlNode node = nodeList[0];
            data.Text = node.InnerText;

            nodeList = xmlDocument.GetElementsByTagName("Image");
            if (nodeList == null || nodeList.Count == 0)
            {
                throw new XmlParserException("Node \"Image\" does not exist");
            }
            node = nodeList[0];
            data.ImageName = node.InnerText;

            nodeList = xmlDocument.GetElementsByTagName("Answer");
            if (nodeList == null || nodeList.Count == 0)
            {
                throw new XmlParserException("Node \"Answer\" does not exist");
            }
            node = nodeList[0];
            data.Answer = node.InnerText;

            return data;
        }

        private static Sprite FindSpriteByName(string name, UnityEngine.Object[] resources)
        {
            foreach (var resource in resources)
            {
                if (resource is Sprite sprite)
                {
                    if (sprite.name.Equals(name))
                    {
                        return sprite;
                    }
                }
            }

            return null;
        }

        private static IExercise CreateExercise(XmlParsedData xmlParsedData, string name, UnityEngine.Object[] resources)
        {
            Sprite sprite = FindSpriteByName(xmlParsedData.ImageName, resources);
            if (sprite == null)
            {
                Debug.LogError($"Image \"{xmlParsedData.ImageName}\" was not found");
            }
            SimpleExercise exercise = new SimpleExercise(name, xmlParsedData.Text, sprite, xmlParsedData.Answer);
            return exercise;
        }

        private static void ProcessTextAsset(TextAsset textAsset, UnityEngine.Object[] resources,
            List<IExercise> exercises)
        {
            try
            {
                string xmlText = textAsset.text;
                XmlParsedData xmlParsedData = ParseXml(xmlText);
                IExercise exercise = CreateExercise(xmlParsedData, textAsset.name, resources);
                exercises.Add(exercise);
            }
            catch (XmlParserException ex)
            {
                Debug.LogError($"Error with processing text asset \"{textAsset.name}\": {ex.Message}");
            }
        }

        public static IExercise[] LoadFromResources()
        {
            List<IExercise> result = new List<IExercise>();

            UnityEngine.Object[] resources = Resources.LoadAll("Exercises");

            foreach (var resource in resources)
            {
                if (resource is TextAsset textAsset)
                {
                    ProcessTextAsset(textAsset, resources, result);
                }
            }

            return result.ToArray();
        }
    }
}
