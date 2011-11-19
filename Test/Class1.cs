using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void RemoveStringArray()
        {
            var l1 = new List<string>() { "a", "b", "c", "d", "f" };
            var l2 = new List<string>() { "d", "c", "b" };
            List<string> lNew = l1.Except(l2, StringComparer.OrdinalIgnoreCase).ToList();

            Console.WriteLine(" L1 List ");
            foreach (var variable in l1)
            {
                Console.WriteLine(variable);
            }


            Console.WriteLine(" L2 List ");
            foreach (var variable in l2)
            {
                Console.WriteLine(variable);
            }


            Console.WriteLine(lNew.Count);

            //Intersect saertoebi, Except - gansxvaveba
            
        }
    }
}
