using System;

namespace Nexify_Test.Libary
{
    /// <summary>
    /// Remoting �A�ȴ��Ѩ禡
    /// </summary>
    [Serializable]
    public class RServiceProvider
    {
        public RServiceProvider()
        {
        }

        /// <summary>
        /// �^�ǵ��G true-���榨�\, false-���楢��
        /// (�w�]�Ȭ�false)
        /// </summary>
        bool result = false;
        public bool Result
        {
            get { return result; }
            set { result = value; }
        }
        
        /// <summary>
        /// �^�ǥN��
        /// </summary>
        string returnCode = string.Empty;
        public string ReturnCode
        {
            get { return returnCode; }
            set { returnCode = value; }
        }

        /// <summary>
        /// �^�ǰT��
        /// </summary>
        string returnMessage = string.Empty;
        public string ReturnMessage
        {
            get { return returnMessage; }
            set { returnMessage = value; }
        }
        
        /// <summary>
        /// �^�Ǹ��
        /// </summary>
        Object returnData;
        public Object ReturnData
        {
            get { return returnData; }
            set { returnData = value; }
        }

        /// <summary>
        /// �^�Ǹ��
        /// </summary>
        Object returnSecondData;
        public Object ReturnSecondData
        {
            get { return returnSecondData; }
            set { returnSecondData = value; }
        }
        
    }
}