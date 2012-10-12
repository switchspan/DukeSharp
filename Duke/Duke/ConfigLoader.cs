using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using Duke.Cleaners;
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
            XElement xml = XElement.Load(file);

            // Get the threshold
            double threshold =
                xml.Elements("schema").Descendants("threshold").Select(x => double.Parse(x.Value)).FirstOrDefault();
            cfg.Threshold = threshold;

            // Get all of the properties
            IEnumerable<XElement> xmlProperties = from s in xml.Elements("schema")
                                                  from p in s.Descendants("property")
                                                  select p;

            foreach (XElement xElement in xmlProperties)
            {
                string propName = xElement.Descendants("name").First().Value;
                var property = new Property(propName);

                // Check to see if this is an id property
                XAttribute xAttribute = xElement.Attribute("type");
                if (xAttribute != null)
                {
                    string id = xAttribute.Value;
                    if (id != null && id == "id")
                    {
                        property.IsIdProperty = true;
                    }
                }
                else
                {
                    string comparatorName = xElement.Descendants("comparator").FirstOrDefault().Value;
                    property.Comparator = GetComparatorFromString(comparatorName);
                    property.LowProbability = xElement.Descendants("low").Select(x => double.Parse(x.Value)).FirstOrDefault();
                    property.HighProbability = xElement.Descendants("high").Select(x => double.Parse(x.Value)).FirstOrDefault();
                    properties.Add(property);
                }
            }

            cfg.SetProperties(properties);

            //// Get the datasources
            //XPathNodeIterator dsi = xpn.Select("/duke/*[not(self::schema)]");

            //while (dsi.MoveNext())
            //{
            //    if (dsi.Current != null && xpi.Current.Name == "csv")
            //    {
            //        var datasource = GetCsvDataSourceFromXml(dsi, xpn);
            //    }
            //}
            var dataSources = from d in xml.Elements()
                              where d.Name != "schema"
                              select d;

            foreach (var dataSource in dataSources)
            {
                if (dataSource.Name == "csv")
                {
                    var csvDs = new CsvDataSource();
                    var csvParams = GetParametersTable(dataSource);
                    csvDs.File = csvParams["input-file"].ToString();
                    csvDs.HasHeader = (csvParams["header-line"].ToString().ToLower() == "true");
                    var skipLines = 0;

                    csvDs.SkipLines = Int32.TryParse(csvParams["skip-lines"].ToString(), out skipLines) ? skipLines : 0;
                    csvDs.FileEncoding = GetTextEncodingFromString(csvParams["encoding"].ToString());

                    var cols = GetDataSourceColumns(dataSource);
                    foreach (var column in cols)
                    {
                        csvDs.AddColumn(column);
                    }

                }
            }
            

            return cfg;
        }

        private static Hashtable GetParametersTable(XElement dataSource)
        {
            var paramTable = new Hashtable();

            // get all of the parameters
            var csvParams = from p in dataSource.Elements()
                            where p.Name == "param"
                            select p;

            foreach (var csvParam in csvParams)
            {
                paramTable.Add((string)csvParam.Attributes("name").FirstOrDefault(), (string)csvParam.Attributes("value").FirstOrDefault());
            }

            return paramTable;
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

        /// <summary>
        /// Gets the data source columns.
        /// </summary>
        /// <param name="dataSourceXml">The data source XML.</param>
        /// <returns></returns>
        private static List<Column> GetDataSourceColumns(XElement dataSourceXml)
        {
            var cols = dataSourceXml.Elements("column")
            .Select(x => new
            {
                name = (string)x.Attributes("name").FirstOrDefault(),
                cleaner = (string)x.Attributes("cleaner").FirstOrDefault(),
                property = (string)x.Attributes("property").FirstOrDefault(),
                prefix = (string)x.Attributes("prefix").FirstOrDefault()

            });

            return cols.Select(col => new Column(col.name, col.property, col.prefix, GetCleanerFromString(col.cleaner))).ToList();
        }


        /// <summary>
        /// Gets the comparator from string.
        /// </summary>
        /// <param name="comparatorName">Name of the comparator.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the cleaner from string.
        /// </summary>
        /// <param name="cleanerName">Name of the cleaner.</param>
        /// <returns></returns>
        private static ICleaner GetCleanerFromString(string cleanerName)
        {
            // strip the java namespacing from the string
            string cleaner = cleanerName.Trim().ToLower().Replace("no.priv.garshol.duke.cleaners.", "");

            switch (cleaner)
            {
                case "digitsonlycleaner":
                    return new DigitsOnlyCleaner();

                case "familycommagivencleaner":
                    return new FamilyCommaGivenCleaner();

                case "lowercasenormalizecleaner":
                    return new LowerCaseNormalizeCleaner();

                case "mappingfilecleaner":
                    return new MappingFileCleaner();

                case "nocleaningcleaner":
                    return new NoCleaningCleaner();

                case "norwegianaddresscleaner":
                    return new NorwegianAddressCleaner();

                case "norwegiancompanynamecleaner":
                    return new NorwegianCompanyNameCleaner();

                case "personnamecleaner":
                    return new PersonNameCleaner();

                case "regexpcleaner":
                    return new RegexpCleaner();

                case "trimcleaner":
                    return new TrimCleaner();

                
                default: // we don't know what type of comparator this is, so return null.
                    return null;
            }
        }

        private static Encoding GetTextEncodingFromString(string encoding)
        {
            switch (encoding.ToLower().Trim())
            {
                case "ascii":
                    return Encoding.ASCII;
                case "utf8":
                    return Encoding.UTF8;
                case "utf7":
                    return Encoding.UTF7;
                case "unicode":
                    return Encoding.Unicode;
                default:
                    return Encoding.Default;
            }
        }
    }
}