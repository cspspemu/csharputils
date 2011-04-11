using CSharpPspEmulator.Hle.Elf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace CSharpPspEmulatorTests
{
    [TestClass]
    public class ElfLoaderTest
    {
        [TestMethod]
        public void LoadTest()
        {
            ElfLoader target = new ElfLoader(); // TODO: Inicializar en un valor adecuado
            target.Load(new FileStream("../../../Demos/minifire.elf", FileMode.Open));
        }
    }
}
