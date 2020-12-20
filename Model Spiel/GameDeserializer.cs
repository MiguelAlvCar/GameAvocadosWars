
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using BasicElements;
using System.Collections;
using Ninject;
using Ninject.Parameters;
using BasicElements.AbstractClasses;
using System.Runtime.Serialization.Formatters.Binary;

namespace ModelGame
{
    public class GameDeserializer: IDeserializer
    {
        public IModelGame Deserialize(Stream fileStream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Game gam = (Game)formatter.Deserialize(fileStream);
            fileStream.Close();
            return gam;
        }
    }
}
