using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Phatra.Core.Exceptions
{
    /// <summary>
    /// Base class สำหรับ exception ที่จะโยนออกไปในโปรแกรมเพื่อที่จะระบุว่าเป็น error แบบไหนมี message ของการ error คืออะไร
    /// </summary>
    /// <remarks>
    ///     1.วิธีใช้ให้ทำการ inherit BaseMessageException
    ///     2.สร้าง constructor 2 อัน อันที่ไม่มีรับ parameter แล้วอันที่รับ parameter ที่เป็น string array
    ///     3.ให้ทำการ override property XmlPath เพื่อระบุว่า xml ที่เก็บ exception message อยู่ที่ไหน
    ///     ดูตัวอย่างได้จาก ไฟล์ Example Exception.cs และตัวอย่างไฟล์ xml ได้ที่ Example Exception.xml
    /// </remarks>
    public abstract class BaseMessageException : Exception
    {
        private bool _loadMessageByLanguageFile;

        protected virtual bool LoadMessageByLanguageFile
        {
            get { return _loadMessageByLanguageFile; }
            set { _loadMessageByLanguageFile = value; }
        }

        /// <summary>
        /// Property ที่เก็บ error message
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// Property ที่เก็บ error message อีกอันในกรณีที่ต้องการแสดงมากกว่า 1 message
        /// </summary>
        public string OtherErrorMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor ไม่มีการส่ง parameter ที่จะมา concat กับ error message
        /// </summary>
        public BaseMessageException()
            : this(string.Empty)
        {

        }

        /// <summary>
        /// Constructor ที่มีการส่ง parameter ที่จะมาแสดงกับ error message
        /// </summary>
        /// <param name="arg">parameter ที่จะ concat กับ error message</param>
        public BaseMessageException(Exception innerException, params string[] arg)
            : base(ConvertToMessage(arg), innerException)
        {
            AttachError(arg);
        }

        /// <summary>
        /// Constructor ที่มีการส่ง parameter ที่จะมาแสดงกับ error message
        /// </summary>
        /// <param name="arg">parameter ที่จะ concat กับ error message</param>
        public BaseMessageException(params string[] arg)
            : base(ConvertToMessage(arg))
        {
            AttachError(arg);
        }

        private static string ConvertToMessage(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                return string.Join("|", args);
            }

            return string.Empty;
        }

        /// <summary>
        /// Property ที่บอกว่าตอนนี้ folder ของ dll ที่กำลัง execute นั้นอยู่ที่ไหน
        /// </summary>
        protected string ExecutingPath
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", ""); }
        }

        /// <summary>
        /// Property folder xml ที่เก็บ error message ไว้
        /// </summary>
        protected abstract string XmlPath
        {
            get;// { return Path.Combine(this.ExecutingPath, "Exception.xml"); }
        }

        /// <summary>
        /// method สำหรับนำ parameter มา concat กับ error message
        /// </summary>
        /// <param name="additionalContents">parameter ที่จะ concat กับ error message</param>
        /// <example>
        /// 
        ///     ตัวอย่างไฟล์ xml message
        ///     <?xml version="1.0" encoding="utf-8" ?>
        ///     <exceptions>
        ///         <exceptionGroup name="PFS.Business.BusinessException.Provident">
        ///             <exception name="InvalidColumnCurrencyTextFileException">
        ///              <message>Column '{0}' มีข้อมูลจำนวนหน่วยเงิน '{1}' ไม่ถูกต้องตามรูปแบบ '{2}'</message>
        ///              <otherMessage></otherMessage>
        ///            </exception>
        ///         </exceptionGroup>
        ///         
        ///         <exceptionGroup name="PFS.Business.BusinessException.User">
        ///             <exception name="InvalidIdForPasswordRecoveryException">
        ///                 <message>รหัสสำหรับใช้ในการตรวจสอบ ไม่ถูกต้อง</message>
        ///                 <otherMessage></otherMessage>
        ///             </exception>
        ///         </exceptionGroup>
        ///     </exceptions>
        /// 
        /// </example>
        private void AttachError(string[] additionalContents)
        {
            string xmlPath = XmlPath;
            if (LoadMessageByLanguageFile && Thread.CurrentThread.CurrentUICulture.Name != string.Empty)
            {
                xmlPath = Path.Combine(Path.GetDirectoryName(XmlPath), Path.GetFileNameWithoutExtension(XmlPath) + "." + Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName + Path.GetExtension(XmlPath));
            }
            //--- ทำการหาว่ามีไฟล์ xml ที่เก็บ message อยู่หรือไม่
            if (!File.Exists(xmlPath)) throw new FileNotFoundException("Xml Exception file not found " + xmlPath);

            Type exType = GetType();
            XElement root = XElement.Load(xmlPath);

            //--- ในไฟล์ xml message จะต้องมี root ที่ขึ้นต้นด้วยคำว่า exceptionGroup ซึ่งจะใช้ Namespace ที่ class exception นั้นอยู่เป็นตัวแยก exception group
            var exGroup = from exGrp in root.Elements("exceptionGroup") where (string)exGrp.Attribute("name") == exType.Namespace select exGrp;
            //--- ถ้าไม่มี root node exceptionGroup ให้ฟ้องว่าไม่เจอ node exceptionGroup
            if (exGroup.Count() == 0) throw new XmlException("Xml Exception file error Node <exceptionGroup name='" + exType.Namespace + "'> not found.");

            //--- ในไฟล์ xml message จะต้องมี node ที่ชื่อว่า exception และมี attribute name เป็นชื่อ class exception
            var exception = from ex in exGroup.Elements("exception") where (string)ex.Attribute("name") == exType.Name select ex;
            if ((additionalContents == null || additionalContents.Length == 0) && exception.Count() == 0)
            {
                //--- ถ้าไม่เจอจะฟ้องว่าไม่เจอข้อมูล
                throw new XmlException("Xml Exception file error Node <exceptionGroup name='" + exType.Namespace + "'><exception name='" + exType.Name + "'> not found.");
            }

            string message = string.Empty;
            string otherMessage = string.Empty;

            //--- ทำการหา node ที่เก็บ message และ other message
            var nodeMessage = from msg in exception.Elements("message") select msg;
            var nodeOtherMessage = from msg in exception.Elements("otherMessage") select msg;
            message = (nodeMessage.Count() == 0 ? string.Empty : nodeMessage.First().Value);
            otherMessage = (nodeOtherMessage.Count() == 0 ? string.Empty : nodeOtherMessage.First().Value);

            //--- ถ้ามีการส่ง parameter ที่ใช้ในการ concat message
            if (additionalContents != null)
            {
                if (message == string.Empty)
                {
                    if (additionalContents.Length == 1)
                    {
                        message = additionalContents[0];
                    }
                }
                else
                {
                    for (int i = 0; i < additionalContents.Length; i++)
                    {
                        message = message.Replace("{" + i + "}", additionalContents[i]);
                        otherMessage = otherMessage.Replace("{" + i + "}", additionalContents[i]);
                    }
                }
            }
            ErrorMessage = message;
            OtherErrorMessage = otherMessage;
        }
    }
}
