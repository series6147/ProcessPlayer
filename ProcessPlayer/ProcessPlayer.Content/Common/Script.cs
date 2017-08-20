using Newtonsoft.Json;
using ProcessPlayer.Data.Common;
using ProcessPlayer.Data.Expressions;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace ProcessPlayer.Content.Common
{
    public class Script : ProcessContent
    {
        #region private variables

        private readonly JsonParser _parser = new JsonParser();
        private XmlDocument _doc;

        #endregion

        #region private methods

        private void parse(string json)
        {
            var sbOut = new StringBuilder();

            using (var errOut = new StringWriter(sbOut))
            {
                _parser.Construct(json, errOut);
                _parser.TopElement();
            }
        }

        #endregion

        #region programming methods

        public XmlNode createNode(string name)
        {
            return _doc.CreateNode(XmlNodeType.Element, name, null);
        }

        public bool deleteNode(XmlNode node)
        {
            if (node != null && node.ParentNode != null)
            {
                node.ParentNode.RemoveChild(node);

                return true;
            }
            return false;
        }

        public XmlNode getAttribute(XmlNode node, string name)
        {
            return node != null ? node.SelectSingleNode(name) : null;
        }

        public XmlNodeList getAttributes(XmlNode node)
        {
            return node != null ? node.ChildNodes : null;
        }

        public string getAttributeValue(XmlNode node)
        {
            return node != null ? node.Value : null;
        }

        public string getInnerText(XmlNode node)
        {
            return node != null ? node.InnerText : null;
        }

        public XmlNode getObjectById(string id)
        {
            return _doc.SelectSingleNode(string.Format("//*[ID='{0}']", id));
        }

        public void load(string path)
        {
            if (File.Exists(path))
            {
                string json;

                using (var reader = File.OpenText(path))
                {
                    json = reader.ReadToEnd();
                }

                _doc = JsonConvert.DeserializeXmlNode(json, string.Empty, true);
            }
            else
                throw new Exception(string.Format("File '{0}' does not exists.", path));
        }

        public void save(string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeXmlNode(_doc, Newtonsoft.Json.Formatting.Indented).Replace("\\r\\n", "\r\n"));
        }

        public XmlNode serialize(object value)
        {
            var json = JsonConvert.SerializeObject(value);

            return _doc.ImportNode(JsonConvert.DeserializeXmlNode(json).DocumentElement, true);
        }

        public XmlNode serialize(object value, string name)
        {
            var json = JsonConvert.SerializeObject(value);

            return _doc.ImportNode(JsonConvert.DeserializeXmlNode(json, name).DocumentElement, true);
        }

        public void setAttributeValue(XmlNode node, object value)
        {
            if (node != null)
            {
                if (value.IsComplex())
                {
                    node.RemoveAll();

                    XmlNode externalNode;

                    externalNode = serialize(value);
                    externalNode = _doc.ImportNode(externalNode, true);

                    node.AppendChild(externalNode);
                }
                else
                    node.InnerText = (value ?? string.Empty).ToString();
            }
        }

        public void tryParse()
        {
            parse(JsonConvert.SerializeXmlNode(_doc));
        }

        #endregion
    }
}
