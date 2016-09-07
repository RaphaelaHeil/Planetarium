using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Planetarium
{
    /// <summary>
    /// Parses an XmlFile and creates Luminaries from it.
    /// </summary>
    class LuminaryCreator
    {
        /// <summary>
        /// Creates a list of luminaries from the given xml-file.
        /// </summary>
        /// <param name="filename">Xml-Filename</param>
        /// <param name="shape">Shape contained by the luminary and used to draw/represent it.</param>
        /// <returns>List of parsed luminaries</returns>
        public static List<Luminary> Create(String filename, Shape shape)
        {
            List<Luminary> luminaries = new List<Luminary>();

            XDocument doc = XDocument.Load(filename); //load file
            IEnumerable<XElement> primaries = doc.Elements().Elements(); //get children

            float Scale = float.Parse(primaries.ElementAt(0).Value); //first "universal" child = scale, same for all luminaries

            for (int i = 1; i < primaries.Count(); i++) //iterate over the rest of the children and parse them
            {
                luminaries.Add(parseLuminary(primaries.ElementAt(i), shape, Scale, new Vector3(0,0,0))); //first child = sun -> located in the center of the solar system
            }
            return luminaries;
        }

        /// <summary>
        /// Parses a single luminary and recursively all its children.
        /// </summary>
        /// <param name="lumen">XElement to parse</param>
        /// <param name="shape">Shape contained by the luminary and used to draw/represent it.</param>
        /// <param name="scale">universe scale</param>
        /// <param name="primaryPosition">this luminary's parent position</param>
        /// <returns></returns>
        private static Luminary parseLuminary(XElement lumen, Shape shape, float scale, Vector3 primaryPosition)
        {
            Luminary l = new Luminary(shape);

            l.Scaling = scale;
            l.Name = lumen.Element("name").Value;
            l.Info = lumen.Element("info").Value;
            l.CalculateCoords(primaryPosition, float.Parse(lumen.Element("primaryDistance").Value), float.Parse(lumen.Element("diameter").Value) );
            l.CalculateSpeed(float.Parse(lumen.Element("rotationSpeed").Value));
            l.Illuminated = Boolean.Parse(lumen.Element("illuminated").Value);
            l.IsSatellite = Boolean.Parse(lumen.Element("isSatellite").Value);
            if (lumen.Element("texture") == null)
            {
                l.AddTextures("Sun_Map.jpg", "sun_normal.png", "sun_b.jpg"); //for now: set the sun textures as default
            }
            else
            {
                l.AddTextures(lumen.Element("texture").Value, lumen.Element("normal").Value, lumen.Element("bump").Value);
            }
    
            if (!l.IsSatellite) //is not a satellite -> has children that need to be parsed as well
            {
                IEnumerable<XElement> children = lumen.Elements("luminaries").Elements();
                foreach (var child in children)
                {
                    l.add(parseLuminary(child, shape, scale, l.Coords+new Vector3(l.Diameter*0.5f))); //translate from the surface of the primary, not the center 
                }
            }
            return l;
        }
    }
}
