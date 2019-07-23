using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSSHTTPandWOPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;
    using System.IO.Compression;
    using System.Xml.Serialization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Reflection;

  

    /// <summary>
    /// 2.6.14 This class is used to represent a JCID
    /// </summary>
    public class JCID : BaseStructure
    {
        /// <summary>
        /// Gets or sets an unsigned integer that specifies the type of object
        /// </summary> 
        public ushort Index;

        /// <summary>
        /// Gets or sets the IsBinary value that specifies whether the object contains encryption data transmitted over the File Synchronization via SOAP over HTTP Protocol.
        /// </summary>
        [BitAttribute(1)]
        public byte A;

        /// <summary>
        /// Gets or sets the IsPropertySet value that specifies whether the object contains a property set. 
        /// </summary>
        [BitAttribute(1)]
        public byte B;

        /// <summary>
        /// Gets or sets a value of IsGraphNode field.
        /// </summary>
        [BitAttribute(1)]
        public byte C;

        /// <summary>
        /// Gets or sets the IsFileData value that specifies whether the object is a file data object.
        /// </summary>
        [BitAttribute(1)]
        public byte D;

        /// <summary>
        /// Gets or sets the IsReadOnly value that specifies whether the object's data MUST NOT be changed when the object is revised.
        /// </summary>
        [BitAttribute(1)]
        public byte E;

        /// <summary>
        /// Gets or sets the value of Reserved field.
        /// </summary>
        [BitAttribute(11)]
        public ushort Reserved;

        /// <summary>
        /// Parse the JCID structure.
        /// </summary>
        /// <param name="s">A stream containing JCID structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            int index = 0;
            uint temp = ReadUint();
            this.Index = (ushort)GetBits(temp, index, 16);
            index = index + 16;
            this.A = (byte)GetBits(temp, index, 1);
            index = index + 1;
            this.B = (byte)GetBits(temp, index, 1);
            index = index + 1;
            this.C = (byte)GetBits(temp, index, 1);
            index = index + 1;
            this.D = (byte)GetBits(temp, index, 1);
            index = index + 1;
            this.E = (byte)GetBits(temp, index, 1);
            index = index + 1;
            this.Reserved = (ushort)GetBits(temp, index, 11);
        }
    }

    /// <summary>
    /// This class is used to represent the CompactID structrue.
    /// </summary>
    public class CompactID:BaseStructure
    {
        /// <summary>
        /// Gets or sets an unsigned integer that specifies the value of the ExtendedGUID.n field.
        /// </summary>        
        public byte N;

        /// <summary>
        /// Gets or sets an unsigned integer that specifies the index in the global identification table. 
        /// </summary>
        [BitAttribute(16)]
        public uint GuidIndex;

        /// <summary>
        /// Parse the CompactID structure.
        /// </summary>
        /// <param name="s">A stream containing CompactID structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            int index = 0;
            uint temp = ReadUint();
            this.N = (byte)GetBits(temp, index, 8);
            index = index + 8;
            this.GuidIndex = (uint)GetBits(temp, index, 24);            
        }
    }

    /// <summary>
    /// This class is used to represent a ObjectSpaceObjectStreamHeader.
    /// </summary>
    public class ObjectSpaceObjectStreamHeader : BaseStructure
    {
        /// <summary>
        /// Gets or sets an unsigned integer that specifies the number of CompactID structures.
        /// </summary> 
        [BitAttribute(16)]
        public uint Count;       

        /// <summary>
        /// Gets or sets the Reserved field.
        /// </summary>
        [BitAttribute(6)]
        public byte Reserved;

        /// <summary>
        /// Gets or sets the ExtendedStreamsPresent field.
        /// </summary>
        [BitAttribute(1)]
        public byte A;

        /// <summary>
        /// Gets or sets the OsidStreamNotPresent field.
        /// </summary>
        [BitAttribute(1)]
        public byte B;

        /// <summary>
        /// Parse the ObjectSpaceObjectStreamHeader structure.
        /// </summary>
        /// <param name="s">A stream containing ObjectSpaceObjectStreamHeader structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            int index = 0;
            uint temp = ReadUint();
            this.Count = (ushort)GetBits(temp, index, 24);
            index = index + 24;
            this.Reserved=(byte)GetBits(temp, index, 6);
            index = index + 6;
            this.A = (byte)GetBits(temp, index, 1);
            index = index + 1;
            this.B = (byte)GetBits(temp, index, 1);            
        }
    }

    /// <summary>
    /// This class is used to represent a PropertyID.
    /// </summary>
    public class PropertyID:BaseStructure
    {
        /// <summary>
        /// Gets or sets the value of id field.
        /// </summary>
        [BitAttribute(18)]
        public uint Id;

        /// <summary>
        /// Gets or sets the value of type field.
        /// </summary>
        [BitAttribute(5)]
        public byte Type;

        /// <summary>
        /// Gets or sets the value of boolValue field.
        /// </summary>
        [BitAttribute(1)]
        public byte BoolValue;

        /// <summary>
        /// Parse the PropertyID structure.
        /// </summary>
        /// <param name="s">A stream containing JCID structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            int index = 0;
            uint temp = ReadUint();
            this.Id= (uint)GetBits(temp, index, 26);
            index += 26;
            this.Type= (byte)GetBits(temp, index, 5);
            index += 5;
            this.BoolValue= (byte)GetBits(temp, index, 1);
        }
    }

    /// <summary>
    /// This class is used to represent a PropertySet.
    /// </summary>
    public class PropertySet:BaseStructure
    {
        /// <summary>
        /// Gets or sets an unsigned integer that specifies the number of properties in this PropertySet structure.
        /// </summary>
        public ushort CProperties;

        /// <summary>
        /// Gets or sets the value of rgPrids.
        /// </summary>
        public PropertyID[] RgPrids;

        /// <summary>
        /// Gets or sets the value of rgData field.
        /// </summary>
        public object[] RgData;

        /// <summary>
        /// Parse the PropertySet structure.
        /// </summary>
        /// <param name="s">A stream containing PropertySet structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);            
            this.CProperties = ReadUshort();
            this.RgPrids = new PropertyID[this.CProperties];
            this.RgData = new object[this.CProperties];
            List<PropertyID> tempPropertyIDList = new List<PropertyID>();
            if (this.CProperties>0)
            {
                ulong tempCount = CProperties;                
                do
                {
                    PropertyID tempPropertyID = new PropertyID();
                    tempPropertyID.Parse(s);
                    tempPropertyIDList.Add(tempPropertyID);
                    tempCount--;
                } while (tempCount > 0);
                this.RgPrids = tempPropertyIDList.ToArray();
            }
            List<object> tempRgDataList = new List<object>();
            if (this.RgPrids!=null)
            {
                foreach (PropertyID propertyID in this.RgPrids)
                {
                    object tempRgData =new object();
                    switch ((rgDataType)propertyID.Type)
                    {
                        case rgDataType.NoData:
                        case rgDataType.Bool:
                        case rgDataType.ObjectID:
                        case rgDataType.ContextID:
                        case rgDataType.ObjectSpaceID:
                            tempRgData = new NoData();
                            ((NoData)tempRgData).Parse(s);
                            break;
                        case rgDataType.ArrayOfObjectIDs:
                        case rgDataType.ArrayOfObjectSpaceIDs:
                        case rgDataType.ArrayOfContextIDs:
                            tempRgData = new NumberOfComactIDs();
                            ((NumberOfComactIDs)tempRgData).Parse(s);
                            break;
                        case rgDataType.OneByteOfData:
                            tempRgData = new OneByteOfData();
                            ((OneByteOfData)tempRgData).Parse(s);
                            break;
                        case rgDataType.TwoBytesOfData:
                            tempRgData = new TwoBytesOfData();
                            ((TwoBytesOfData)tempRgData).Parse(s);
                            break;
                        case rgDataType.FourBytesOfData:
                            tempRgData = new FourBytesOfData();
                            ((FourBytesOfData)tempRgData).Parse(s);
                            break;
                        case rgDataType.EightBytesOfData:
                            tempRgData = new EightBytesOfData();
                            ((EightBytesOfData)tempRgData).Parse(s);
                            break;
                        case rgDataType.FourBytesOfLengthFollowedByData:
                            tempRgData = new PrtFourBytesOfLengthFollowedByData();
                            ((PrtFourBytesOfLengthFollowedByData)tempRgData).Parse(s);
                            break;
                        case rgDataType.ArrayOfPropertyValues:
                            tempRgData = new PrtArrayOfPropertyValues();
                            ((PrtArrayOfPropertyValues)tempRgData).Parse(s);
                            break;
                        case rgDataType.PropertySet:
                            tempRgData = new PropertySet();
                            ((PropertySet)tempRgData).Parse(s);
                            break;
                        default:
                            break;
                    }
                    if (tempRgData != null)
                    {
                        tempRgDataList.Add(tempRgData);
                    }
                }
                this.RgData = tempRgDataList.ToArray();
            }
        }        
    }

    /// <summary>
    /// The types of properties.
    /// </summary>
    public enum rgDataType : uint
    {
        /// <summary>
        /// The property contains no data.
        /// </summary>
        NoData = 0x1,
        /// <summary>
        /// The property is a Boolean value specified by boolValue.
        /// </summary>
        Bool = 0x2,
        /// <summary>
        /// The property contains 1 byte of data in the PropertySet.rgData stream field.
        /// </summary>
        OneByteOfData = 0x3,
        /// <summary>
        /// The property contains 2 bytes of data in the PropertySet.rgData stream field.
        /// </summary>
        TwoBytesOfData = 0x4,
        /// <summary>
        /// The property contains 4 bytes of data in the PropertySet.rgData stream field.
        /// </summary>
        FourBytesOfData = 0x5,
        /// <summary>
        /// The property contains 8 bytes of data in the PropertySet.rgData stream field.
        /// </summary>
        EightBytesOfData = 0x6,
        /// <summary>
        /// The property contains a prtFourBytesOfLengthFollowedByData in the PropertySet.rgData stream field.
        /// </summary>
        FourBytesOfLengthFollowedByData = 0x7,
        /// <summary>
        /// The property contains one CompactID in the ObjectSpaceObjectPropSet.OIDs.body stream field.
        /// </summary>
        ObjectID = 0x8,
        /// <summary>
        /// The property contains an array of CompactID structures in the ObjectSpaceObjectPropSet.OSIDs.body stream field. 
        /// </summary>
        ArrayOfObjectIDs = 0x9,
        /// <summary>
        /// The property contains one CompactID structure in the ObjectSpaceObjectPropSet.OSIDs.body stream field.
        /// </summary>
        ObjectSpaceID = 0xA,
        /// <summary>
        /// The property contains an array of CompactID structures in the ObjectSpaceObjectPropSet.OSIDs.body stream field. 
        /// </summary>
        ArrayOfObjectSpaceIDs = 0xB,
        /// <summary>
        /// The property contains one CompactID in the ObjectSpaceObjectPropSet.ContextIDs.body stream field.
        /// </summary>
        ContextID = 0xC,
        /// <summary>
        /// The property contains an array of CompactID structures in the ObjectSpaceObjectPropSet.ContextIDs.body stream field. 
        /// </summary>
        ArrayOfContextIDs = 0xD,
        /// <summary>
        /// The property contains a prtArrayOfPropertyValues structure in the PropertySet.rgData stream field.
        /// </summary>
        ArrayOfPropertyValues = 0x10,
        /// <summary>
        /// The property contains a child PropertySet structure in the PropertySet.rgData stream field of the parent PropertySet.
        /// </summary>
        PropertySet = 0x11
    }

    /// <summary>
    /// This class is used to represent the property contains no data.
    /// </summary>
    public class NoData : BaseStructure
    {
        /// <summary>
        /// Parse the NoData structure.
        /// </summary>
        /// <param name="s">A stream containing NoData structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
        }
    }

    /// <summary>
    /// This class is used to represent the property contains 1 byte of data in the PropertySet.rgData stream field.
    /// </summary>
    public class OneByteOfData : BaseStructure
    {
        /// <summary>
        /// Gets or sets the data of property.
        /// </summary>
        public byte Data;

        /// <summary>
        /// Parse the OneByteOfData structure.
        /// </summary>
        /// <param name="s">A stream containing OneByteOfData structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Data = ReadByte();
        }
    }

    /// <summary>
    /// This class is used to represent the property contains 2 bytes of data in the PropertySet.rgData stream field.
    /// </summary>
    public class TwoBytesOfData : BaseStructure
    {
        /// <summary>
        /// Gets or sets the data of property.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Parse the TwoBytesOfData structure.
        /// </summary>
        /// <param name="s">A stream containing TwoBytesOfData structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Data = new byte[2];
            this.Data = ReadBytes(2);
        }
    }

    /// <summary>
    /// This class is used to represent the property contains 4 bytes of data in the PropertySet.rgData stream field.
    /// </summary>
    public class FourBytesOfData : BaseStructure
    {
        /// <summary>
        ///  Gets or sets the data of property.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Parse the FourBytesOfData structure.
        /// </summary>
        /// <param name="s">A stream containing FourBytesOfData structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Data = new byte[4];
            this.Data = ReadBytes(4);
        }
    }

    /// <summary>
    /// This class is used to represent the property contains 8 bytes of data in the PropertySet.rgData stream field.
    /// </summary>
    public class EightBytesOfData : BaseStructure
    {
        /// <summary>
        /// Gets or sets the data of property.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Parse the EightBytesOfData structure.
        /// </summary>
        /// <param name="s">A stream containing EightBytesOfData structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Data = new byte[8];
            this.Data = ReadBytes(8);
        }
    }

    /// <summary>
    /// This class is used to represent the prtFourBytesOfLengthFollowedByData.
    /// </summary>
    public class PrtFourBytesOfLengthFollowedByData : BaseStructure
    {
        /// <summary>
        /// Gets or sets an unsigned integer that specifies the size, in bytes, of the Data field.
        /// </summary>
        public uint cb;

        /// <summary>
        /// Gets or sets the value of Data field.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Parse the PrtFourBytesOfLengthFollowedByData structure.
        /// </summary>
        /// <param name="s">A stream containing PrtFourBytesOfLengthFollowedByData structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            int index = 0;
            this.cb = ReadUint();
            index += 4;
            this.Data = new byte[this.cb];
            for (int i = 0; i < this.cb; i++)
            {
                this.Data[i] = ReadByte();
            }
        }
    }

    /// <summary>
    /// The class is used to represent the number of the array.
    /// </summary>
    public class NumberOfComactIDs : BaseStructure
    {
        /// <summary>
        /// Gets or sets the number of array.
        /// </summary>
        public uint Number;
        /// <summary>
        /// A stream containing ArrayNumber structure.
        /// </summary>
        /// <param name="s">A stream containing JCID structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);            
            this.Number = ReadUint();
        }

    }

    /// <summary>
    /// The class is used to represent the prtArrayOfPropertyValues . 
    /// </summary>
    public class PrtArrayOfPropertyValues : BaseStructure
    {
        /// <summary>
        /// Gets or sets an unsigned integer that specifies the number of properties in Data.
        /// </summary>
        public uint CProperties;

        /// <summary>
        /// Gets or sets the value of prid field.
        /// </summary>
        public PropertyID Prid;

        /// <summary>
        /// Gets or sets the value of Data field.
        /// </summary>
        public PropertySet[] Data;
        
        /// <summary>
        /// This method is used to deserialize the prtArrayOfPropertyValues from the specified byte array and start index.
        /// </summary>
        /// <param name="s">A stream containing PrtArrayOfPropertyValues structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);            
            this.CProperties = ReadUint();        
            if (this.CProperties> 0)
            {
                this.Prid = new PropertyID();
                this.Prid.Parse(s);
                this.Data = new PropertySet[this.CProperties];
                List<PropertySet> tempDataList = new List<PropertySet>();
                for (int i = 0; i < this.CProperties; i++)
                {
                    this.Data[i] = new PropertySet();
                    this.Data[i].Parse(s);
                }
            }
        }
    }

    /// <summary>
    /// This class is used to represent a ObjectSpaceObjectStreamOfContextIDs.
    /// </summary>
    public class ObjectSpaceObjectStreamOfContextIDs : BaseStructure
    {
        /// <summary>
        /// Gets or sets value of header field.
        /// </summary>
        public ObjectSpaceObjectStreamHeader Header;
        /// <summary>
        /// Gets or sets the value of body field.
        /// </summary>
        public CompactID[] Body;

        /// <summary>
        /// Parse the ObjectSpaceObjectStreamOfContextIDs structure.
        /// </summary>
        /// <param name="s">A stream containing ObjectSpaceObjectStreamOfContextIDs structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Header = new ObjectSpaceObjectStreamHeader();
            this.Header.Parse(s);  
            List<CompactID> tempCompactIDList = new List<CompactID>();
            if (this.Header.Count > 0)
            {
                ulong tempCompactIDCount = this.Header.Count;                
                do
                {
                    CompactID tempCompactID = new CompactID();
                    tempCompactID.Parse(s);
                    tempCompactIDList.Add(tempCompactID);
                    tempCompactIDCount--;
                } while (tempCompactIDCount > 0);
                this.Body = tempCompactIDList.ToArray();
            }
        }
    }

    /// <summary>
    /// This class is used to represent a ObjectSpaceObjectStreamOfOSIDs.
    /// </summary>
    public class ObjectSpaceObjectStreamOfOSIDs : BaseStructure
    {
        /// <summary>
        /// Gets or sets the value of header field.
        /// </summary>
        public ObjectSpaceObjectStreamHeader Header;

        /// <summary>
        /// Gets or sets the value of body field.
        /// </summary>
        public CompactID[] Body;

        /// <summary>
        /// Parse the ObjectSpaceObjectStreamOfOSIDs structure.
        /// </summary>
        /// <param name="s">A stream containing ObjectSpaceObjectStreamOfOSIDs structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Header = new ObjectSpaceObjectStreamHeader();
            this.Header.Parse(s);
            List<CompactID> tempCompactIDList = new List<CompactID>();
            if (this.Header.Count > 0)
            {
                ulong tempCompactIDCount = this.Header.Count;                
                do
                {
                    CompactID tempCompactID = new CompactID();
                    tempCompactID.Parse(s);
                    tempCompactIDList.Add(tempCompactID);
                    tempCompactIDCount--;
                } while (tempCompactIDCount > 0);
                this.Body = tempCompactIDList.ToArray();
            }
        }
    }

    /// <summary>
    /// This class is used to represent a ObjectSpaceObjectStreamOfOIDs.
    /// </summary>
    public class ObjectSpaceObjectStreamOfOIDs:BaseStructure
    {
        /// <summary>
        /// Gets or sets an ObjectSpaceObjectStreamHeader that specifies the number of elements in the body field and whether the ObjectSpaceObjectPropSet structure contains an OSIDs field and ContextIDs field. 
        /// </summary>
        public ObjectSpaceObjectStreamHeader Header;
        /// <summary>
        /// Gets or sets an array of CompactID structures.
        /// </summary>
        public CompactID[] Body;

        /// <summary>
        /// Parse the ObjectSpaceObjectStreamOfOIDs structure.
        /// </summary>
        /// <param name="s">A stream containing ObjectSpaceObjectStreamOfOIDs structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Header = new ObjectSpaceObjectStreamHeader();
            this.Header.Parse(s);
            List<CompactID> tempCompactIDList = new List<CompactID>();
            if (this.Header.Count > 0)
            {
                ulong tempCompactIDCount = this.Header.Count;                
                do
                {
                    CompactID tempCompactID = new CompactID();
                    tempCompactID.Parse(s);
                    tempCompactIDList.Add(tempCompactID);
                    tempCompactIDCount--;
                } while (tempCompactIDCount> 0);
                this.Body = tempCompactIDList.ToArray();
            }
        }
    }

    /// <summary>
    /// This class is used to represent a ObjectSpaceObjectPropSet.
    /// </summary>
    public class ObjectSpaceObjectPropSet:BaseStructure
    {
        /// <summary>
        /// Gets or sets an ObjectSpaceObjectStreamOfOIDs that specifies the count and list of objects that are referenced by this ObjectSpaceObjectPropSet.
        /// </summary>
        public ObjectSpaceObjectStreamOfOIDs OIDs;
        /// <summary>
        /// Gets or sets The value of OSIDs.
        /// </summary>
        public ObjectSpaceObjectStreamOfOSIDs OSIDs;
        /// Gets or sets the value of ContextIDs field.
        /// </summary>
        public ObjectSpaceObjectStreamOfContextIDs ContextIDs;
        /// <summary>
        /// Gets or sets the value of body field.
        /// </summary>
        public PropertySet Body;
        /// <summary>
        /// Gets or sets the value of padding field.
        /// </summary>
        public byte[] Padding;
        /// <summary>
        /// Parse the ObjectSpaceObjectPropSet structure.
        /// </summary>
        /// <param name="s">A stream containing ObjectSpaceObjectPropSet structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            long startIndex = s.Position;
            this.OIDs = new ObjectSpaceObjectStreamOfOIDs();
            this.OIDs.Parse(s);
            if (this.OIDs.Header.B==0)
            {
                this.OSIDs = new ObjectSpaceObjectStreamOfOSIDs();
                this.OSIDs.Parse(s);
            }
            if (this.OIDs.Header.B == 0 && this.OSIDs.Header.A == 1)
            {
                this.ContextIDs = new ObjectSpaceObjectStreamOfContextIDs();
                this.ContextIDs.Parse(s);
            }        
            
            this.Body = new PropertySet();
            this.Body.Parse(s);
            
            long paddingLen = 8-(s.Position -startIndex)%8;
            if (paddingLen < 8)
            {
                this.Padding = new byte[paddingLen];                
                for (int i = 0; i < paddingLen; i++)
                {
                    byte temp = ReadByte();
                    //The size of the padding field is the number of bytes necessary to ensure the total size of ObjectSpaceObjectPropSet structure is a multiple of 8. 
                    //If the byte read in sequence for padding is 0, then assign to Padding field.
                    if (temp==0)
                    {
                        this.Padding[i] = temp;
                    }
                    // If the byte read in sequence for padding is not 0, then assgin 0 to Padding field. s.Position backward 1.
                    else
                    {
                        this.Padding[i] = 0;
                        s.Position -= 1;
                    }
                }
            }
        }
    }
}
