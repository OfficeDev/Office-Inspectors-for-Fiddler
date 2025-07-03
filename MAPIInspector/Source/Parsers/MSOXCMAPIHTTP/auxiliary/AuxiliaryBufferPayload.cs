using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the payload data contains auxiliary information. It is defined in section 3.1.4.1.2 of MS-OXCRPC.
    /// </summary>
    public class AuxiliaryBufferPayload : BaseStructure
    {
        /// <summary>
        /// An AUX_HEADER structure that provides information about the auxiliary block structures that follow it.
        /// </summary>
        public AUX_HEADER AUXHEADER;

        /// <summary>
        /// An object that constitute the auxiliary buffer payload data.
        /// </summary>
        public object AuxiliaryBlock;

        /// <summary>
        /// Parse the auxiliary buffer payload of session.
        /// </summary>
        /// <param name="s">A stream of auxiliary buffer payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            AUXHEADER = new AUX_HEADER();
            AUXHEADER.Parse(s);
            AuxiliaryBlockType_1 type1;
            AuxiliaryBlockType_2 type2;

            if (AUXHEADER.Version == PayloadDataVersion.AUX_VERSION_1)
            {
                type1 = (AuxiliaryBlockType_1)AUXHEADER.Type;

                switch (type1)
                {
                    case AuxiliaryBlockType_1.AUX_TYPE_ENDPOINT_CAPABILITIES:
                        {
                            AUX_ENDPOINT_CAPABILITIES auxiliaryBlock = new AUX_ENDPOINT_CAPABILITIES();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_CLIENT_CONNECTION_INFO:
                        {
                            AUX_CLIENT_CONNECTION_INFO auxiliaryBlock = new AUX_CLIENT_CONNECTION_INFO();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_PROTOCOL_DEVICE_IDENTIFICATION:
                        {
                            AUX_PROTOCOL_DEVICE_IDENTIFICATION auxiliaryBlock = new AUX_PROTOCOL_DEVICE_IDENTIFICATION();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_SERVER_SESSION_INFO:
                        {
                            AUX_SERVER_SESSION_INFO auxiliaryBlock = new AUX_SERVER_SESSION_INFO();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_CLIENT_CONTROL:
                        {
                            AUX_CLIENT_CONTROL auxiliaryBlock = new AUX_CLIENT_CONTROL();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_EXORGINFO:
                        {
                            AUX_EXORGINFO auxiliaryBlock = new AUX_EXORGINFO();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_OSVERSIONINFO:
                        {
                            AUX_OSVERSIONINFO auxiliaryBlock = new AUX_OSVERSIONINFO();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_ACCOUNTINFO:
                        {
                            AUX_PERF_ACCOUNTINFO auxiliaryBlock = new AUX_PERF_ACCOUNTINFO();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_BG_DEFGC_SUCCESS:
                        {
                            AUX_PERF_DEFGC_SUCCESS auxiliaryBlock = new AUX_PERF_DEFGC_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_BG_DEFMDB_SUCCESS:
                        {
                            AUX_PERF_DEFMDB_SUCCESS auxiliaryBlock = new AUX_PERF_DEFMDB_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_BG_FAILURE:
                        {
                            AUX_PERF_FAILURE auxiliaryBlock = new AUX_PERF_FAILURE();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_BG_GC_SUCCESS:
                        {
                            AUX_PERF_GC_SUCCESS auxiliaryBlock = new AUX_PERF_GC_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_BG_MDB_SUCCESS:
                        {
                            AUX_PERF_MDB_SUCCESS auxiliaryBlock = new AUX_PERF_MDB_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_CLIENTINFO:
                        {
                            AUX_PERF_CLIENTINFO auxiliaryBlock = new AUX_PERF_CLIENTINFO();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_DEFGC_SUCCESS:
                        {
                            AUX_PERF_DEFGC_SUCCESS auxiliaryBlock = new AUX_PERF_DEFGC_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_DEFMDB_SUCCESS:
                        {
                            AUX_PERF_DEFMDB_SUCCESS auxiliaryBlock = new AUX_PERF_DEFMDB_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FAILURE:
                        {
                            AUX_PERF_FAILURE auxiliaryBlock = new AUX_PERF_FAILURE();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FG_DEFGC_SUCCESS:
                        {
                            AUX_PERF_DEFGC_SUCCESS auxiliaryBlock = new AUX_PERF_DEFGC_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FG_DEFMDB_SUCCESS:
                        {
                            AUX_PERF_DEFMDB_SUCCESS auxiliaryBlock = new AUX_PERF_DEFMDB_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FG_FAILURE:
                        {
                            AUX_PERF_FAILURE auxiliaryBlock = new AUX_PERF_FAILURE();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FG_GC_SUCCESS:
                        {
                            AUX_PERF_GC_SUCCESS auxiliaryBlock = new AUX_PERF_GC_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FG_MDB_SUCCESS:
                        {
                            AUX_PERF_MDB_SUCCESS auxiliaryBlock = new AUX_PERF_MDB_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_GC_SUCCESS:
                        {
                            AUX_PERF_GC_SUCCESS auxiliaryBlock = new AUX_PERF_GC_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_MDB_SUCCESS:
                        {
                            AUX_PERF_MDB_SUCCESS auxiliaryBlock = new AUX_PERF_MDB_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_PROCESSINFO:
                        {
                            AUX_PERF_PROCESSINFO auxiliaryBlock = new AUX_PERF_PROCESSINFO();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_REQUESTID:
                        {
                            AUX_PERF_REQUESTID auxiliaryBlock = new AUX_PERF_REQUESTID();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_SERVERINFO:
                        {
                            AUX_PERF_SERVERINFO auxiliaryBlock = new AUX_PERF_SERVERINFO();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_SESSIONINFO:
                        {
                            AUX_PERF_SESSIONINFO auxiliaryBlock = new AUX_PERF_SESSIONINFO();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    default:
                        {
                            AnnotatedBytes auxiliaryBlock = new AnnotatedBytes(AUXHEADER.Size - 4);
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                }
            }
            else if (AUXHEADER.Version == PayloadDataVersion.AUX_VERSION_2)
            {
                type2 = (AuxiliaryBlockType_2)AUXHEADER.Type;
                switch (type2)
                {
                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_BG_FAILURE:
                        {
                            AUX_PERF_FAILURE_V2 auxiliaryBlock = new AUX_PERF_FAILURE_V2();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_BG_GC_SUCCESS:
                        {
                            AUX_PERF_GC_SUCCESS_V2 auxiliaryBlock = new AUX_PERF_GC_SUCCESS_V2();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_BG_MDB_SUCCESS:
                        {
                            AUX_PERF_MDB_SUCCESS_V2 auxiliaryBlock = new AUX_PERF_MDB_SUCCESS_V2();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_FAILURE:
                        {
                            AUX_PERF_FAILURE_V2 auxiliaryBlock = new AUX_PERF_FAILURE_V2();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_FG_FAILURE:
                        {
                            AUX_PERF_FAILURE_V2 auxiliaryBlock = new AUX_PERF_FAILURE_V2();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_FG_GC_SUCCESS:
                        {
                            AUX_PERF_GC_SUCCESS_V2 auxiliaryBlock = new AUX_PERF_GC_SUCCESS_V2();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_FG_MDB_SUCCESS:
                        {
                            AUX_PERF_MDB_SUCCESS_V2 auxiliaryBlock = new AUX_PERF_MDB_SUCCESS_V2();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_GC_SUCCESS:
                        {
                            AUX_PERF_GC_SUCCESS_V2 auxiliaryBlock = new AUX_PERF_GC_SUCCESS_V2();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_MDB_SUCCESS:
                        {
                            AUX_PERF_MDB_SUCCESS_V2 auxiliaryBlock = new AUX_PERF_MDB_SUCCESS_V2();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_PROCESSINFO:
                        {
                            AUX_PERF_PROCESSINFO auxiliaryBlock = new AUX_PERF_PROCESSINFO();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_SESSIONINFO:
                        {
                            AUX_PERF_SESSIONINFO_V2 auxiliaryBlock = new AUX_PERF_SESSIONINFO_V2();
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }

                    default:
                        {
                            AnnotatedBytes auxiliaryBlock = new AnnotatedBytes(AUXHEADER.Size - 4);
                            auxiliaryBlock.Parse(s);
                            AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                }
            }
            else
            {
                AnnotatedBytes auxiliaryBlock = new AnnotatedBytes(AUXHEADER.Size - 4);
                auxiliaryBlock.Parse(s);
                AuxiliaryBlock = auxiliaryBlock;
            }
        }
    }
}