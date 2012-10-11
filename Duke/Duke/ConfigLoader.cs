using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Duke.Comparators;
using Duke.Datasources;

namespace Duke
{
    /// <summary>
    /// Can read XML configuration files and return a fully set up configuration.
    /// </summary>
    public class ConfigLoader
    {
        //Note that if file starts with 'classpath:' the resource is looked
        // up on the classpath instead.

        public static Configuration Load(string file)
        {
            var cfg = new Configuration();
            var properties = new List<Property>();

            // Get the appropriate nodes using Linq to XML
            var xml = XElement.Load(file);

            // Get the threshold
            var threshold = xml.Elements("schema").Descendants("threshold").Select(x => double.Parse(x.Value)).First();
            cfg.Threshold = threshold;

            // Get all of the properties
            var xmlProperties = from s in xml.Elements("schema")
                             from p in s.Descendants("property")
                             select p;

            foreach (var xElement in xmlProperties)
            {
                var propName = xElement.Descendants("name").First().Value;
                var property = new Property(propName);
                
                // Check to see if this is an id property
                var xAttribute = xElement.Attribute("type");
                if (xAttribute != null)
                {
                    var id = xAttribute.Value;
                    if (id != null && id == "id")
                    {
                        property.IsIdProperty = true;
                    }
                }

                var comparatorName = xElement.Descendants("comparator").First().Value;
                property.Comparator = GetComparatorFromString(comparatorName);
                property.LowProbability = xElement.Descendants("low").Select(x => double.Parse(x.Value)).First();
                property.HighProbability = xElement.Descendants("high").Select(x => double.Parse(x.Value)).First();
                properties.Add(property);
            }

            cfg.SetProperties(properties);

            // Get the appropriate nodes using XPath...
            //var xpd = new XPathDocument(file);
            //XPathNavigator xpn = xpd.CreateNavigator();
            //XPathNodeIterator xpi = xpn.Select("/duke/schema/*");

            //var properties = new List<Property>();

            //while (xpi.MoveNext()) // each schema node
            //{
            //    if (xpi.Current != null && xpi.Current.Name == "threshold")
            //    {
            //        cfg.Threshold = Double.Parse(xpi.Current.Value);
            //    }

            //    if (xpi.Current != null && xpi.Current.Name == "property")
            //    {
            //        properties.Add(GetPropertyFromXml(xpi, xpn));
            //    }
            //}

            //cfg.SetProperties(properties);

            //// Get the datasources
            //XPathNodeIterator dsi = xpn.Select("/duke/*[not(self::schema)]");

            //while (dsi.MoveNext())
            //{
            //    if (dsi.Current != null && xpi.Current.Name == "csv")
            //    {
            //        var datasource = GetCsvDataSourceFromXml(dsi, xpn);
            //    }
            //}

            return cfg;
        }

        private static Property GetPropertyFromXml(XPathNodeIterator xpi, XPathNavigator xpn)
        {
            if (xpi.Current != null && xpi.Current.Name == "property")
            {
                var prop = new Property("UNDEFINED");
                string type;

                type = xpi.Current.GetAttribute("type", xpn.NamespaceURI);
                prop.IsIdProperty = (!String.IsNullOrEmpty(type) && type.ToLower() == "id");

                XPathNodeIterator propChild = xpi.Current.SelectChildren(XPathNodeType.Element);
                while (propChild.MoveNext())
                {
                    if (propChild.Current != null)
                    {
                        string elementName = propChild.Current.Name;
                        string elementValue = propChild.Current.Value;

                        switch (elementName)
                        {
                            case "name":
                                prop.Name = elementValue.Trim();
                                break;
                            case "low":
                                prop.LowProbability = Double.Parse(elementValue);
                                break;
                            case "high":
                                prop.HighProbability = Double.Parse(elementValue);
                                break;
                            case "comparator":
                                prop.Comparator = GetComparatorFromString(elementValue);
                                break;
                        }
                    }
                }

                return (prop.Name != "UNDEFINED") ? prop : null;
            }

            return null;
        }

        private static CsvDataSource GetCsvDataSourceFromXml(XPathNodeIterator xpi, XPathNavigator xpn)
        {
            return null;
        }

        private static IComparator GetComparatorFromString(string comparatorName)
        {
            // strip the java namespacing from the string
            string comparator = comparatorName.Trim().ToLower().Replace("no.priv.garshol.duke.comparators.", "");

            switch (comparator)
            {
                case "jarowinklertokenized":
                    return new JaroWinklerTokenized();

                case "jarowinkler":
                    return new JaroWinklerTokenized();

                case "exactcomparator":
                    return new ExactComparator();

                case "dicecoefficientcomparator":
                    return new DiceCoefficientComparator();

                case "differentcomparator":
                    return new DifferentComparator();

                case "jaccardindexcomparator":
                    return new JaccardIndexComparator();

                case "levenshtein":
                    return new Levenshtein();

                case "metaphonecomparator":
                    return new MetaphoneComparator();

                case "numericcomparator":
                    return new NumericComparator();

                case "personnamecomparator":
                    return new PersonNameComparator();

                case "soundexcomparator":
                    return new SoundexComparator();

                case "weightedlevenshtein":
                    return new WeightedLevenshtein();
                default: // we don't know what type of comparator this is, so return null.
                    return null;
            }
        }
    }
}