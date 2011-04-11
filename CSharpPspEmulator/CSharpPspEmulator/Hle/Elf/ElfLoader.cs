using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using CSharpPspEmulator.Utils;

namespace CSharpPspEmulator.Hle.Elf
{
    public class ElfLoader
    {
        // http://stackoverflow.com/questions/3863191/loading-binary-data-into-a-structure
        // http://stackoverflow.com/questions/4159184/c-read-structures-from-binary-file
        // 

        public enum Type : ushort { Executable = 0x0002, Prx = 0xFFA0 }
        public enum Machine : ushort { ALLEGREX = 8 }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct Header
        {
		    // e_ident 16 bytes.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] 
		    public byte[] Magic;
            public byte Class;                         ///
            public byte Data;                          ///
            public byte Idver;                         ///

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public byte[] Pad;                         /// Padding.

            public Type Type;                          /// Identifies object file type
            public Machine Machine;                    /// Architecture build
            public uint Version;                       /// Object file version
            public uint EntryPoint;                    /// Virtual address of code entry. Module EntryPoint (PC)
            public uint ProgramHeaderOffset;           /// Program header table's file offset in bytes
            public uint SectionHeaderOffset;           /// Section header table's file offset in bytes
            public uint Flags;                         /// Processor specific flags
            public ushort Ehsize;                      /// ELF header size in bytes

		    // Program Header.
            public ushort ProgramHeaderEntrySize;      /// Program header size (all the same size)
            public ushort ProgramHeaderCount;          /// Number of program headers

		    // Section Header.
            public ushort SectionHeaderEntrySize;      /// Section header size (all the same size)
            public ushort SectionHeaderCount;          /// Number of section headers
            public ushort SectionHeaderStringTable;    /// Section header table index of the entry associated with the section name string table

		    // Check the size of the struct.
		    //static assert(this.sizeof == 52);
	    }

        public ElfLoader()
        {
        }

        public void Load(Stream Stream)
        {
            Header Header = Stream.ReadStruct<Header>();
            if (!Header.Magic.SequenceEqual(new byte[] { 0x7F, (byte)'E', (byte)'L', (byte)'F' })) throw(new Exception("Invalid Header"));
            if (Header.Type != Type.Executable) throw(new Exception("Unsupported Header.Type"));
            if (Header.Machine != Machine.ALLEGREX) throw(new Exception("Unsupported Header.Machine"));

            Console.WriteLine("{0,8:X}", Header.EntryPoint);
        }
    }
}
