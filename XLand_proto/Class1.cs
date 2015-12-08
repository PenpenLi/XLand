using SuperProto;

namespace XLand_proto
{
    [System.Serializable]
    public class TestProto : BaseProto
    {
        public string data;

        public TestProto()
        {
            type = PROTO_TYPE.C2S;
        }
    }

    [System.Serializable]
    public class TestProto2 : BaseProto
    {
        public string data;

        public TestProto2()
        {
            type = PROTO_TYPE.S2C;
        }
    }
}
