using System;

namespace Nexify_Test.Libary
{
    /// <summary>
    /// Remoting 服務提供函式
    /// </summary>
    [Serializable]
    public class RServiceProvider
    {
        public RServiceProvider()
        {
        }

        /// <summary>
        /// 回傳結果 true-執行成功, false-執行失敗
        /// (預設值為false)
        /// </summary>
        bool result = false;
        public bool Result
        {
            get { return result; }
            set { result = value; }
        }
        
        /// <summary>
        /// 回傳代號
        /// </summary>
        string returnCode = string.Empty;
        public string ReturnCode
        {
            get { return returnCode; }
            set { returnCode = value; }
        }

        /// <summary>
        /// 回傳訊息
        /// </summary>
        string returnMessage = string.Empty;
        public string ReturnMessage
        {
            get { return returnMessage; }
            set { returnMessage = value; }
        }
        
        /// <summary>
        /// 回傳資料
        /// </summary>
        Object returnData;
        public Object ReturnData
        {
            get { return returnData; }
            set { returnData = value; }
        }

        /// <summary>
        /// 回傳資料
        /// </summary>
        Object returnSecondData;
        public Object ReturnSecondData
        {
            get { return returnSecondData; }
            set { returnSecondData = value; }
        }
        
    }
}