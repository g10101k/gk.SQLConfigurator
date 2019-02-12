/*
 *  "gk.SQLConfigurator", Excel add-in that allows you to fill / edit / delete SQL table data.
 *
 *  Copyright (C) 2015-2019  Igor Tyulyakov aka g10101k, g101k. Contacts: <g101k@mail.ru>
 *  
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0
 *
 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 */
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using gk.SQLConfigurator;
using System.Drawing;

namespace ForTest
{
    class Program
    {
        public static Bitmap Icon;
        public static string ImageBuffer
        {
            get
            {
                string imageBuffer = null;

                if (Icon != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Icon.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        imageBuffer = Convert.ToBase64String(ms.ToArray());
                        return imageBuffer;
                    }
                }

                return imageBuffer;
            }
            set
            {
                if (value == null)
                {
                    Icon = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(value)))
                    {
                        Icon = new Bitmap(ms);
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            try
            {
                Icon = (Bitmap)Image.FromFile(args[0], true);

            }
            catch (System.IO.FileNotFoundException) {}
            Console.WriteLine(ImageBuffer);
            Console.ReadLine();
        }
    }


    [Serializable]
    public class Conteiner : List<object>
    {

    }
}
/*
 * 
using System;
using System.IO;
using System.Xml.Serialization;
 
namespace Serialization
{
    // класс и его члены объявлены как public
    [Serializable]
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
 
        // стандартный конструктор без параметров
        public Person()
        { }
 
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // объект для сериализации
            Person person = new Person("Tom", 29);
            Console.WriteLine("Объект создан");
 
            // передаем в конструктор тип класса
            XmlSerializer formatter = new XmlSerializer(typeof(Person));
 
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("persons.xml", FileMode.OpenOrCreate))
            {
               formatter.Serialize(fs, person);
 
                Console.WriteLine("Объект сериализован");
            }
 
            // десериализация
            using (FileStream fs = new FileStream("persons.xml", FileMode.OpenOrCreate))
            {
                Person newPerson = (Person)formatter.Deserialize(fs);
 
                Console.WriteLine("Объект десериализован");
                Console.WriteLine("Имя: {0} --- Возраст: {1}", newPerson.Name, newPerson.Age);
            }
 
            Console.ReadLine();
        }
    }
}*/
