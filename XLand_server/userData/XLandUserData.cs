using SuperServer.userManager;

namespace XLand_server.userData
{
    [System.Serializable]
    class XLandUserData : UserData
    {
        public ValueData<int> money = new ValueData<int>();
        public DicValueData<int> cards = new DicValueData<int>();

        public XLandUserData():base()
        {
            money.data = 986;

            cards.Add(1, 2);
            cards.Add(2, 4);
            cards.Add(5, 10);
        }

    }
}
