using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;

namespace BlockParserTests
{
    [TestClass]
    public class BlockTTests
    {
        private struct TestStruct
        {
            public int A;
            public short B;
        }

        [TestMethod]
        public void BlockT_SetAndGetData_Works()
        {
            var block = new BlockT<int>();
            block.Data = 42;
            Assert.AreEqual(42, block.Data);
            block.Data = 99;
            Assert.AreEqual(99, block.Data);
        }

        [TestMethod]
        public void BlockT_Create_SetsProperties()
        {
            var block = BlockT<int>.Create(7, 4, 10);
            Assert.AreEqual(7, block.Data);
            Assert.AreEqual(4, block.Size);
            Assert.AreEqual(10, block.Offset);
        }

        [TestMethod]
        public void BlockT_EmptyT_ReturnsEmptyBlock()
        {
            var empty = BlockT<int>.EmptyT<int>();
            Assert.IsNotNull(empty);
            Assert.AreEqual(default(int), empty.Data);
        }

        [TestMethod]
        public void BlockT_Parse_ReadsData()
        {
            // Arrange
            var bytes = BitConverter.GetBytes(0x12345678);
            var parser = new BinaryParser(bytes);
            var block = BlockT<int>.Parse(parser);
            Assert.AreEqual(0x12345678, block.Data);
        }

        // TODO: Implement this test with a more realistic scenario when we need it
        //[TestMethod]
        //public void BlockT_ParseU_ReadsAndConverts()
        //{
        //    var testStruct = new TestStruct { A = 0x11223344, B = 0x5566 };
        //    int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(TestStruct));
        //    byte[] bytes = new byte[size];
        //    IntPtr ptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
        //    try
        //    {
        //        System.Runtime.InteropServices.Marshal.StructureToPtr(testStruct, ptr, false);
        //        System.Runtime.InteropServices.Marshal.Copy(ptr, bytes, 0, size);
        //    }
        //    finally
        //    {
        //        System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);
        //    }
        //    var parser = new BinaryParser(bytes);
        //    var block = BlockT<long>.Parse<TestStruct>(parser);
        //    Assert.AreEqual(Convert.ToInt64(testStruct.A), block.Data);
        //}

        [TestMethod]
        public void BlockT_ParseT_ReadsAndConverts()
        {
            var testStruct = new TestStruct { A = 0x11223344, B = 0x5566 };
            int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(TestStruct));
            byte[] bytes = new byte[size];
            IntPtr ptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            try
            {
                System.Runtime.InteropServices.Marshal.StructureToPtr(testStruct, ptr, false);
                System.Runtime.InteropServices.Marshal.Copy(ptr, bytes, 0, size);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);
            }
            var parser = new BinaryParser(bytes);
            var block = BlockT<TestStruct>.Parse(parser);
            Assert.AreEqual(testStruct, block.Data);
        }
    }
}
