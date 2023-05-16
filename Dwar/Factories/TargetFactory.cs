using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Dwar.Factories
{
    public class TargetFactory
    {
        public static IEnumerable<Target> ParseDistinct(string text)
        {
            return GetDistinctTargets(ParseTargets(text));
        }
        public static IEnumerable<Target> Parse(string text)
        {
            return ParseTargets(text);
        }
        private static IEnumerable<Target> ParseTargets(string text)
        {
            var targerts = new List<Target>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(text);

            if (xml.LastChild == null)
                return targerts;

            foreach (XmlNode xnList in xml.LastChild.ChildNodes)
            {
                foreach (XmlNode xn in xnList)
                {
                    string name = "";
                    int id = 0;
                    int fightId = 0;
                    string pic = "";
                    foreach (XmlAttribute attribute in xn.Attributes!)
                    {
                        if (attribute.Name == "name")
                            name = attribute.Value;
                        if (attribute.Name == "id"|| attribute.Name == "num")
                            id = Convert.ToInt32(attribute.Value);
                        if (attribute.Name == "pic")
                            pic = attribute.Value;
                        if (attribute.Name == "fight_id" || attribute.Name == "farming")
                            fightId = Convert.ToInt32(attribute.Value);
                    }
                    if (id != 0)
                        targerts.Add(new Target(id, name, fightId));

                }
            }

            return targerts;
        }

        private static IEnumerable<Target> GetDistinctTargets(IEnumerable<Target> targets)
        {
            var distinctTargets = new List<Target>();
            foreach (var target in targets)
            {
                if (distinctTargets.Any(x => x.Name == target.Name) == false)
                    distinctTargets.Add(target);
            }
            return distinctTargets;
        }
    }
}
