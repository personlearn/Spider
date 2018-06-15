using Spider.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Spider.Service
{
    internal class SaveToXml
    {
        /// <summary>
        /// 创建XML文档
        /// </summary>
        /// <param name="name">根节点名称</param>
        /// <param name="type">根节点的一个属性值</param>
        /// <returns>XmlDocument对象</returns>     
        public static XmlDocument CreateXmlDocument(string name, string type)
        {
            XmlDocument doc;
            try
            {
                doc = new XmlDocument();
                doc.LoadXml("<" + name + "/>");
                var rootEle = doc.DocumentElement;
                rootEle?.SetAttribute("type", type);
            }
            catch (Exception er)
            {
                throw new Exception(er.ToString());
            }
            return doc;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// <returns>string</returns>
        public static string Read(string path, string node, string attribute)
        {
            var value = "";
            try
            {
                var doc = new XmlDocument();
                doc.Load(path);
                var xn = doc.SelectSingleNode(node);
                if (xn != null && xn.Attributes != null)
                    value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch (Exception er)
            {
                throw new Exception(er.ToString());
            }
            return value;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void Insert(string path, string node, string element, string attribute, string value)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(path);
                var xn = doc.SelectSingleNode(node);
                if (element.Equals(""))
                {
                    if (!attribute.Equals(""))
                    {
                        var xe = (XmlElement)xn;
                        xe?.SetAttribute(attribute, value);
                        //xe?.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    var xe = doc.CreateElement(element);
                    if (attribute.Equals(""))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    xn?.AppendChild(xe);
                }
                doc.Save(path);
            }
            catch (Exception er)
            {
                throw new Exception(er.ToString());
            }
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void Update(string path, string node, string attribute, string value)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(path);
                var xn = doc.SelectSingleNode(node);
                var xe = (XmlElement)xn;
                if (attribute.Equals(""))
                {
                    if (xe != null) xe.InnerText = value;
                }
                else
                {
                    xe?.SetAttribute(attribute, value);
                }
                doc.Save(path);
            }
            catch (Exception er)
            {
                throw new Exception(er.ToString());
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        /// <returns></returns>
        public static void Delete(string path, string node, string attribute)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(path);
                var xn = doc.SelectSingleNode(node);
                var xe = (XmlElement)xn;
                if (attribute.Equals(""))
                {
                    xn?.ParentNode?.RemoveChild(xn);
                }
                else
                {
                    xe?.RemoveAttribute(attribute);
                }
                doc.Save(path);
            }
            catch (Exception er)
            {
                throw new Exception(er.ToString());
            }
        }

        /// <summary>
        /// 获得xml文件中指定节点的节点数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static string GetNodeInfoByNodeName(string path, string nodeName)
        {
            var xmlString = string.Empty;
            try
            {
                var xml = new XmlDocument();
                xml.Load(path);
                var root = xml.DocumentElement;
                if (root == null) return xmlString;
                var node = root.SelectSingleNode("//" + nodeName);
                if (node != null)
                {
                    xmlString = node.InnerText;
                }
            }
            catch (Exception er)
            {
                throw new Exception(er.ToString());
            }
            return xmlString;
        }

        /// <summary>  
        /// 功能:读取指定节点的指定属性值     
        /// </summary>
        /// <param name="path"></param>
        /// <param name="strNode">节点名称</param>  
        /// <param name="strAttribute">此节点的属性</param>  
        /// <returns></returns>  
        public string GetXmlNodeAttributeValue(string path, string strNode, string strAttribute)
        {
            var strReturn = "";
            try
            {
                var xml = new XmlDocument();
                xml.Load(path);
                //根据指定路径获取节点  
                var xmlNode = xml.SelectSingleNode(strNode);
                if (xmlNode != null)
                {
                    //获取节点的属性，并循环取出需要的属性值  
                    var xmlAttr = xmlNode.Attributes;
                    if (xmlAttr == null) return strReturn;
                    for (var i = 0; i < xmlAttr.Count; i++)
                    {
                        if (xmlAttr.Item(i).Name != strAttribute) continue;
                        strReturn = xmlAttr.Item(i).Value;
                        break;
                    }
                }
            }
            catch (XmlException xmle)
            {
                throw new Exception(xmle.Message);
            }
            return strReturn;
        }

        /// <summary>  
        /// 功能:设置节点的属性值     
        /// </summary>
        /// <param name="path"></param>
        /// <param name="xmlNodePath">节点名称</param>  
        /// <param name="xmlNodeAttribute">属性名称</param>  
        /// <param name="xmlNodeAttributeValue">属性值</param>  
        public void SetXmlNodeAttributeValue(string path, string xmlNodePath, string xmlNodeAttribute, string xmlNodeAttributeValue)
        {
            try
            {
                var xml = new XmlDocument();
                xml.Load(path);
                //可以批量为符合条件的节点的属性付值  
                var xmlNode = xml.SelectNodes(xmlNodePath);
                if (xmlNode == null) return;
                foreach (var xmlAttr in from XmlNode xn in xmlNode select xn.Attributes)
                {
                    if (xmlAttr == null) return;
                    for (var i = 0; i < xmlAttr.Count; i++)
                    {
                        if (xmlAttr.Item(i).Name != xmlNodeAttribute) continue;
                        xmlAttr.Item(i).Value = xmlNodeAttributeValue;
                        break;
                    }
                }

            }
            catch (XmlException xmle)
            {
                throw new Exception(xmle.Message);
            }
        }

        /// <summary>
        /// 读取XML资源中的指定节点内容
        /// </summary>
        /// <param name="source">XML资源</param>
        /// <param name="xmlType">XML资源类型：文件，字符串</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns>节点内容</returns>
        public static object GetNodeValue(string source, XmlType xmlType, string nodeName)
        {
            var xd = new XmlDocument();
            if (xmlType == XmlType.FILE)
            {
                xd.Load(source);
            }
            else
            {
                xd.LoadXml(source);
            }
            var xe = xd.DocumentElement;
            XmlNode xn = null;
            if (xe != null)
            {
                xn = xe.SelectSingleNode("//" + nodeName);

            }
            return xn.InnerText;
        }

        /// <summary>
        /// 更新XML文件中的指定节点内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeValue">更新内容</param>
        /// <returns>更新是否成功</returns>
        public static bool UpdateNode(string filePath, string nodeName, string nodeValue)
        {
            try
            {
                bool flag;
                var xd = new XmlDocument();
                xd.Load(filePath);
                var xe = xd.DocumentElement;
                if (xe == null) return false;
                var xn = xe.SelectSingleNode("//" + nodeName);
                if (xn != null)
                {
                    xn.InnerText = nodeValue;
                    flag = true;
                }
                else
                {
                    flag = false;
                }
                return flag;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 将对象转化为xml，并写入指定路径的xml文件中
        /// </summary>
        /// <typeparam name="T">C#对象名</typeparam>
        /// <param name="item">对象实例</param>
        /// <param name="path">路径</param>
        /// <param name="jjdbh">标号</param>
        /// <param name="ends">结束符号（整个xml的路径类似如下：C:\xmltest\201111send.xml，其中path=C:\xmltest,jjdbh=201111,ends=send）</param>
        /// <returns></returns>
        public static void WriteXml<T>(T item, string path, string jjdbh, string ends)
        {
            if (string.IsNullOrEmpty(ends))
            {
                //默认为发送
                ends = "send";
            }
            //控制写入文件的次数
            var i = 0;
            //获取当前对象的类型，也可以使用反射typeof(对象名)
            var serializer = new XmlSerializer(item.GetType());
            //xml的路径组合
            object[] obj = { path, "\\", jjdbh, ends, ".xml" };
            var xmlPath = string.Concat(obj);
            while (true)
            {
                try
                {
                    //用filestream方式创建文件不会出现“文件正在占用中，用File.create”则不行
                    var fs = System.IO.File.Create(xmlPath);
                    fs.Close();
                    TextWriter writer = new StreamWriter(xmlPath, false, Encoding.UTF8);
                    var xml = new XmlSerializerNamespaces();
                    xml.Add(string.Empty, string.Empty);
                    serializer.Serialize(writer, item, xml);
                    writer.Flush();
                    writer.Close();
                    break;
                }
                catch (Exception)
                {
                    if (i < 5)
                    {
                        i++;
                        continue;
                    }
                    break;
                }
            }
            //return SerializeToXmlStr<T>(item, true);
        }

        /// <summary>  
        /// 向一个已经存在的父节点中插入一个子节点  
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parentNodePath">父节点</param>
        /// <param name="childnodename">子节点名称</param>  
        public void AddChildNode(string path, string parentNodePath, string childnodename)
        {
            try
            {
                var xml = new XmlDocument();
                xml.Load(path);
                var parentXmlNode = xml.SelectSingleNode(parentNodePath);
                XmlNode childXmlNode = xml.CreateElement(childnodename);
                if ((parentXmlNode) != null)
                {
                    //如果此节点存在    
                    parentXmlNode.AppendChild(childXmlNode);
                }
                else
                {
                    //如果不存在就放父节点添加  
                    xml.SelectSingleNode(path).AppendChild(childXmlNode);
                }

            }
            catch (XmlException xmle)
            {
                throw new Exception(xmle.Message);
            }
        }

    }
}
