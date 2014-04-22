using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda
{
    public static class XMLSerialization
    {
        public static bool LoadXML<T>(out T obj, string path)
        {
            obj = default(T);

            try
            {
                using (var stream = TitleContainer.OpenStream(path))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    obj = (T)serializer.Deserialize(stream);

                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: Failed loading {0} with exception: ({1})", path, ex.Message);
            }

            return false;
        }
    }
}
