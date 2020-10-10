namespace CC
{
    namespace JsonData
    {
        public class ReflectData
        {
            public string name = "ReflectData";
        }

        public class ExchangeData : ReflectData
        {
            public string productID;
            public string purchaseDate;
            public string isSuccess;
        }

        public class ShareData : ReflectData
        {
            public string isSuccess;
        }

        public class VideoData : ReflectData
        {
            public string tag;
            public string isSuccess;
        }
    }
}