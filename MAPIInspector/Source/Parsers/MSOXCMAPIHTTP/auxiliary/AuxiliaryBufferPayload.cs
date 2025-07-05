using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the payload data contains auxiliary information. It is defined in section 3.1.4.1.2 of MS-OXCRPC.
    /// </summary>
    public class AuxiliaryBufferPayload : Block
    {
        /// <summary>
        /// An AUX_HEADER structure that provides information about the auxiliary block structures that follow it.
        /// </summary>
        public AUX_HEADER AUXHEADER;

        /// <summary>
        /// An object that constitute the auxiliary buffer payload data.
        /// </summary>
        public Block AuxiliaryBlock;

        /// <summary>
        /// Parse the auxiliary buffer payload of session.
        /// </summary>
        protected override void Parse()
        {
            AUXHEADER = Parse<AUX_HEADER>();

            if (AUXHEADER.Version == PayloadDataVersion.AUX_VERSION_1)
            {
                switch (AUXHEADER.Type1.Data)
                {
                    case AuxiliaryBlockType_1.AUX_TYPE_ENDPOINT_CAPABILITIES:
                        {
                            AuxiliaryBlock = Parse<AUX_ENDPOINT_CAPABILITIES>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_CLIENT_CONNECTION_INFO:
                        {
                            AuxiliaryBlock = Parse<AUX_CLIENT_CONNECTION_INFO>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_PROTOCOL_DEVICE_IDENTIFICATION:
                        {
                            AuxiliaryBlock = Parse<AUX_PROTOCOL_DEVICE_IDENTIFICATION>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_SERVER_SESSION_INFO:
                        {
                            AuxiliaryBlock = Parse<AUX_SERVER_SESSION_INFO>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_CLIENT_CONTROL:
                        {
                            AuxiliaryBlock = Parse<AUX_CLIENT_CONTROL>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_EXORGINFO:
                        {
                            AuxiliaryBlock = Parse<AUX_EXORGINFO>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_OSVERSIONINFO:
                        {
                            AuxiliaryBlock = Parse<AUX_OSVERSIONINFO>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_ACCOUNTINFO:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_ACCOUNTINFO>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_BG_DEFGC_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_DEFGC_SUCCESS>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_BG_DEFMDB_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_DEFMDB_SUCCESS>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_BG_FAILURE:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_FAILURE>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_BG_GC_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_GC_SUCCESS>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_BG_MDB_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_MDB_SUCCESS>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_CLIENTINFO:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_CLIENTINFO>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_DEFGC_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_DEFGC_SUCCESS>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_DEFMDB_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_DEFMDB_SUCCESS>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FAILURE:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_FAILURE>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FG_DEFGC_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_DEFGC_SUCCESS>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FG_DEFMDB_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_DEFMDB_SUCCESS>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FG_FAILURE:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_FAILURE>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FG_GC_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_GC_SUCCESS>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FG_MDB_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_MDB_SUCCESS>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_GC_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_GC_SUCCESS>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_MDB_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_MDB_SUCCESS>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_PROCESSINFO:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_PROCESSINFO>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_REQUESTID:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_REQUESTID>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_SERVERINFO:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_SERVERINFO>();
                            break;
                        }

                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_SESSIONINFO:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_SESSIONINFO>();
                            break;
                        }

                    default:
                        {
                            AuxiliaryBlock = ParseBytes(AUXHEADER._Size - 4);
                            break;
                        }
                }
            }
            else if (AUXHEADER.Version == PayloadDataVersion.AUX_VERSION_2)
            {
                switch (AUXHEADER.Type2.Data)
                {
                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_BG_FAILURE:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_FAILURE_V2>();
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_BG_GC_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_GC_SUCCESS_V2>();
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_BG_MDB_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_MDB_SUCCESS_V2>();
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_FAILURE:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_FAILURE_V2>();
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_FG_FAILURE:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_FAILURE_V2>();
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_FG_GC_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_GC_SUCCESS_V2>();
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_FG_MDB_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_MDB_SUCCESS_V2>();
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_GC_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_GC_SUCCESS_V2>();
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_MDB_SUCCESS:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_MDB_SUCCESS_V2>();
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_PROCESSINFO:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_PROCESSINFO>();
                            break;
                        }

                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_SESSIONINFO:
                        {
                            AuxiliaryBlock = Parse<AUX_PERF_SESSIONINFO_V2>();
                            break;
                        }

                    default:
                        {
                            AuxiliaryBlock = ParseBytes(AUXHEADER._Size - 4);
                            break;
                        }
                }
            }
            else
            {
                AuxiliaryBlock = ParseBytes(AUXHEADER._Size - 4);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("AuxiliaryBufferPayload");
            AddChild(AUXHEADER, "AUXHEADER");
            if (AuxiliaryBlock is BlockBytes bb)
            {
                AddChildBytes(bb, "AuxiliaryBlock");
            }
            else
            {
                AddChild(AuxiliaryBlock, "AuxiliaryBlock");
            }
        }
    }
}