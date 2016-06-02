using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;

namespace FtpServer
{
    /// <summary>
    /// 暂时只支持两级的配置信息，如：user/logininfo
    /// </summary>
    public class UserConfigReadWrite
    {
        private string configName = "user";

        public string ConfigName
        {
            get { return configName; }
            set { configName = value; }
        }

        private string filePath;

        public string FilePath
        {
            get { return this.filePath; }
            set { this.filePath = value.ToString(); }
        }
        
        public UserConfigReadWrite()
        {
            this.filePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "user.xml";
        }

        public UserConfigReadWrite(string path)
        {
            this.filePath = path;
        }

        public XmlNode GetSubNode(string node, string name)
        {
            XmlDocument doc = null;
            if (File.Exists(filePath))
            {
                doc = new XmlDocument();
                doc.Load(filePath);
            }
            else
            {
                doc = CreateXmlFile(ConfigName, node);
            }
            XmlNode n = doc.SelectSingleNode(ConfigName + "/" + node);
            if (n != null)
            {
                XmlNodeList nodelist = n.ChildNodes;
                foreach (XmlNode xn in nodelist)
                {
                    if (xn.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return xn;
                    }
                }
            }
            return null;
        }

        public bool SetSubNode(string node, XmlNode subNode)
        {
            XmlDocument doc = null;
            if (File.Exists(filePath))
            {
                doc = new XmlDocument();
                doc.Load(filePath);
            }
            else
            {
                doc = CreateXmlFile(ConfigName, node);
            }
            XmlNode n = doc.SelectSingleNode(ConfigName + "/" + node);
            if (n != null)
            {
                XmlNodeList nodelist = n.ChildNodes;
                if (nodelist.Count > 0)
                {
                    int index = -1;
                    for (int i = 0; i < nodelist.Count; i++)
                    {
                        if (nodelist[i].Name.Equals(subNode.Name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index > -1)
                    {
                        n.ReplaceChild(subNode, nodelist[index]);
                    }
                    else
                    {
                        n.AppendChild(subNode);
                    }
                }
                else
                {
                    n.AppendChild(subNode);
                }
            }
            else
            {
                XmlNode root = doc.SelectSingleNode(ConfigName);
                XmlElement newNode = doc.CreateElement(node);
                root.AppendChild(newNode);
                newNode.AppendChild(subNode);
            }
            try
            {
                doc.Save(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetAttribute(string node, string name)
        {
            string value = string.Empty;
            XmlDocument doc = null;
            if (File.Exists(filePath))
            {
                doc = new XmlDocument();
                doc.Load(filePath);
            }
            else
            {
                doc = CreateXmlFile(ConfigName, node);
            }
            XmlNode n = doc.SelectSingleNode(ConfigName + "/" + node);
            if (n != null)
            {
                XmlAttributeCollection attrs = n.Attributes;
                foreach (XmlAttribute xa in attrs)
                {
                    if (xa.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        value = xa.Value;
                        break;
                    }
                }
            }
            return value;
        }

        public bool SetAttribute(string node, string name, string value)
        {
            XmlDocument doc = null;
            if (File.Exists(filePath))
            {
                doc = new XmlDocument();
                doc.Load(filePath);
            }
            else
            {
                doc = CreateXmlFile(ConfigName, node);
            }
            XmlNode n = doc.SelectSingleNode(ConfigName + "/" + node);
            if (n != null)
            {
                XmlAttributeCollection attrs = n.Attributes;
                if (attrs.Count > 0)
                {
                    bool find = false;
                    foreach (XmlAttribute xa in attrs)
                    {
                        if (xa.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            find = true;
                            xa.Value = value;
                            break;
                        }
                    }
                    if (!find)
                    {
                        XmlAttribute attr = doc.CreateAttribute(name);
                        attr.Value = value;
                        n.Attributes.Append(attr);
                    }
                }
                else
                {
                    XmlAttribute attr = doc.CreateAttribute(name);
                    attr.Value = value;
                    n.Attributes.Append(attr);
                }
            }
            else
            {
                XmlNode root = doc.SelectSingleNode(ConfigName);
                XmlElement newNode = doc.CreateElement(node);
                root.AppendChild(newNode);
                XmlAttribute attr = doc.CreateAttribute(name);
                attr.Value = value;
                newNode.Attributes.Append(attr);
            }
            try
            {
                doc.Save(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetInnerText(string node)
        {
            XmlDocument doc = null;
            if (File.Exists(filePath))
            {
                doc = new XmlDocument();
                doc.Load(filePath);
            }
            else
            {
                doc = CreateXmlFile(ConfigName, node);
            }
            XmlNode n = doc.SelectSingleNode(ConfigName + "/" + node);
            if (n != null)
            {
                return n.InnerText;
            }
            return string.Empty;
        }

        public string GetInnerXml(string node)
        {
            XmlDocument doc = null;
            if (File.Exists(filePath))
            {
                doc = new XmlDocument();
                doc.Load(filePath);
            }
            else
            {
                doc = CreateXmlFile(ConfigName, node);
            }
            XmlNode n = doc.SelectSingleNode(ConfigName + "/" + node);
            if (n != null)
            {
                return n.InnerXml;
            }
            return string.Empty;
        }

        public bool SetInnerText(string node, string text)
        {
            XmlDocument doc = null;
            if (File.Exists(filePath))
            {
                doc = new XmlDocument();
                doc.Load(filePath);
            }
            else
            {
                doc = CreateXmlFile(ConfigName, node);
            }
            XmlNode nod = doc.SelectSingleNode(ConfigName + "/" + node);
            if (nod != null)
            {
                nod.InnerText = text;
            }
            else
            {
                XmlNode root = doc.SelectSingleNode(ConfigName);
                XmlElement xe = doc.CreateElement(node);
                xe.InnerText = text;
                root.AppendChild(xe);
            }
            try
            {
                doc.Save(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SetInnerXml(string node, string text)
        {
            XmlDocument doc = null;
            if (File.Exists(filePath))
            {
                doc = new XmlDocument();
                doc.Load(filePath);
            }
            else
            {
                doc = CreateXmlFile(ConfigName, node);
            }
            XmlNode nod = doc.SelectSingleNode(ConfigName + "/" + node);
            if (nod != null)
            {
                nod.InnerXml = text;
            }
            else
            {
                XmlNode root = doc.SelectSingleNode(ConfigName);
                XmlElement xe = doc.CreateElement(node);
                xe.InnerXml = text;
                root.AppendChild(xe);
            }
            try
            {
                doc.Save(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public XmlDocument CreateXmlFile(string configName, string nodeName, string nodeValue = null, Dictionary<string, string> attributes = null)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<" + configName + "></" + configName + ">");
            XmlNode root = doc.SelectSingleNode(configName);
            XmlElement node = doc.CreateElement(nodeName);
            if (nodeValue != null)
            {
                node.InnerText = nodeValue;
            }
            if (attributes != null)
            {
                foreach (string key in attributes.Keys)
                {
                    node.SetAttribute(key, attributes[key]);
                }
            }
            root.AppendChild(node);
            try
            {
                doc.Save(this.filePath);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return doc;
        }
    }
}
