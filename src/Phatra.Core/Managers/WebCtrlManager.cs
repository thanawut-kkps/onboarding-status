using System;
using Phatra.Core.AdoNet;
using Phatra.Core.Logging;

namespace Phatra.Core.Managers
{
    public class WebCtrlManager : BaseManager, IWebCtrlManager
    {
        private static WebCtrlManager _webCtrlManager;

        public static WebCtrlManager Instance => _webCtrlManager ?? (_webCtrlManager = new WebCtrlManager());

        public bool HasPagePrivilage(string url)
        {
            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                var query = db.StoredProcedure("dbo.USP_IsPriv")
                                .WithParameter("@Url", url);
                var a = query.AsScalar<string>();

                if (a != "Y")
                {
                    return false;
                }
            }
            return true;
        }

        public string GetNoPrivilegePage()
        {
            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                var query = db.StoredProcedure("dbo.USP_Gen_Url")
                            .WithParameter("@Apps_Code", "WebCtrl")
                            .WithParameter("@Page_Name", "NoPriv.aspx");

                return query.AsScalar<string>();
            }
        }

        public string GetMenuStep(string url)
        {
            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                var query = db.StoredProcedure("dbo.USP_get_MenuStep")
                            .WithParameter("@Url", url);

                return query.AsScalar<string>();
            }
        }

        public string GetPageLabel(string url)
        {
            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                var query = db.StoredProcedure("dbo.USP_get_Label")
                            .WithParameter("@Url", url);

                return query.AsScalar<string>();
            }
        }

        public bool IsProduction()
        {
            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                var strRestult = db.Sql("SELECT [dbo].[UFN_Util_IsProdServer]()").AsScalar<string>();
                return strRestult != "N";
            }

        }

        public bool IsUat()
        {
            bool isUat = true;

            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                var strRestult = db.Sql("SELECT [dbo].[UFN_Util_IsUAT2Server]()").AsScalar<string>();
                if (strRestult == "N")
                {
                    strRestult = db.Sql("SELECT [dbo].[UFN_Util_IsUATServer]()").AsScalar<string>();
                    if (strRestult == "N")
                    {
                        isUat = false;
                    }
                }
            }
            return isUat;
        }

        public string GetCurrentUserFullName()
        {
            string strRestult;
            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                strRestult = db.StoredProcedure("[dbo].[USP_Get_CurrentUserFullName]")
                                .AsScalar<string>();
            }
            return strRestult;
        }

        public void USP_Init_WorkFlow_State(int? pageID, string refNO, int? stateID, string sTo1, string sCc1, string sBcc1, string sj1, string sj2, string sj3, string ms1, string ms2, string ms3, string attachedFile)
        {
            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                try
                {
                    db.StoredProcedure("[dbo].[USP_Init_WorkFlow_State]")
                        .WithParameter("@Page_ID", pageID)
                        .WithParameter("@Ref_NO", refNO)
                        .WithParameter("@State_ID", stateID)
                        .WithParameter("@sTo1", sTo1)
                        .WithParameter("@sCc1", sCc1)
                        .WithParameter("@sBcc1", sBcc1)
                        .WithParameter("@Sj1", sj1)
                        .WithParameter("@Sj2", sj2)
                        .WithParameter("@Sj3", sj3)
                        .WithParameter("@Ms1", ms1)
                        .WithParameter("@Ms2", ms2)
                        .WithParameter("@Ms3", ms3)
                        .WithParameter("@Attached_File", attachedFile)
                        .AsNonQuery();
                }
                catch (Exception ex)
                {
                    if (ex.Message != "OK")
                    {
                        Logger.Error($"{ex.ToString()}");
                        //throw;
                    }
                }
            }
        }

        public void USP_Init_WorkFlow_State(int? pageID, string refNO)
        {
            USP_Init_WorkFlow_State(pageID, refNO, 100, "", "", "", "", "", "", "", "", "", "");
        }

        public void USP_Init_WorkFlow_State(int? pageID, string refNO, int? stateID)
        {
            USP_Init_WorkFlow_State(pageID, refNO, stateID, "", "", "", "", "", "", "", "", "", "");
        }

        public void USP_Change_WorkFlow_State(int? pageID, string refNO, int? stateID, string sTo1, string sCc1, string sBcc1, string sj1, string sj2, string sj3, string ms1, string ms2, string ms3, string attachedFile)
        {
            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                try
                {
                    db.StoredProcedure("[dbo].[USP_Change_WorkFlow_State]")
                        .WithParameter("@Page_ID", pageID)
                        .WithParameter("@Ref_NO", refNO)
                        .WithParameter("@State_ID", stateID)
                        .WithParameter("@sTo1", sTo1)
                        .WithParameter("@sCc1", sCc1)
                        .WithParameter("@sBcc1", sBcc1)
                        .WithParameter("@Sj1", sj1)
                        .WithParameter("@Sj2", sj2)
                        .WithParameter("@Sj3", sj3)
                        .WithParameter("@Ms1", ms1)
                        .WithParameter("@Ms2", ms2)
                        .WithParameter("@Ms3", ms3)
                        .WithParameter("@Attached_File", attachedFile)
                        .AsNonQuery();
                }
                catch (Exception ex)
                {
                    if (ex.Message != "OK")
                    {
                        Logger.Error($"{ex.ToString()}");
                        //throw;
                    }
                }
            }
        }

        public void USP_Change_WorkFlow_State(int? pageID, string refNO, int? stateID)
        {
            USP_Change_WorkFlow_State(pageID, refNO, stateID, "", "", "", "", "", "", "", "", "", "");
        }

        public int GetPageId(string appsCode, string PageName)
        {
            int pageId = 0;
            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                var appsRoots = db.StoredProcedure("[dbo].[USP_Get_AppsRoot]")
                                .WithParameter("@Apps_Code", appsCode)
                                .AsScalar<string>();

                pageId = db.StoredProcedure("[dbo].[USP_Get_PageID]")
                        .WithParameter("@Url", appsRoots + PageName)
                        .AsScalar<int>();
            }
            return pageId;
        }

        public string USP_Get_ServerName()
        {
            string item;
            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                item = db.StoredProcedure("[dbo].[USP_Get_ServerName]")
                                .AsScalar<string>();
            }
            return item;
        }

        public string USP_Util_GetEnvionmentName()
        {
            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                return db.StoredProcedure("[dbo].[USP_Util_GetEnvionmentName]")
                         .AsScalar<string>();
            }
        }

        public DateTime USP_Get_PreWorkDate(DateTime? curDate, int? numberOfDate)
        {
            DateTime item;
            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                item = db.StoredProcedure("[dbo].[USP_Get_PreWorkDate]")
                    .WithParameter("@CurDate", curDate)
                    .WithParameter("@No_of_Day", numberOfDate)
                    .AsScalar<DateTime>();
            }
            return item;
        }

        public string USP_IsInApplGroup(string applGroup)
        {
            string item;
            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                item = db.StoredProcedure("[dbo].[USP_IsInApplGroup]")
                    .WithParameter("@Appl_Group", applGroup)
                    .AsScalar<string>();
            }
            return item;
        }

        public string USP_Get_UserName()
        {
            string item;
            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                item = db.StoredProcedure("[dbo].[USP_Get_UserName]")
                    .AsScalar<string>();
            }
            return item;
        }

        public string USP_Get_UserFullName(string loginName)
        {
            string item;
            using (var db = DatabaseFactory.CreateWebCtrlDatabase())
            {
                item = db.StoredProcedure("[dbo].[USP_Get_UserFullName]")
                     .WithParameter("@Login_Name", loginName)
                    .AsScalar<string>();
            }
            return item;
        }
    }

    public interface IWebCtrlManager
    {
        bool HasPagePrivilage(string url);
        string GetNoPrivilegePage();
        string GetMenuStep(string url);
        string GetPageLabel(string url);
        bool IsProduction();
        bool IsUat();
        string GetCurrentUserFullName();

        void USP_Init_WorkFlow_State(int? pageID, string refNO, int? stateID,
            string sTo1, string sCc1, string sBcc1,
            string sj1, string sj2, string sj3,
            string ms1, string ms2, string ms3, string attachedFile);
        void USP_Init_WorkFlow_State(int? pageID, string refNO);
        void USP_Init_WorkFlow_State(int? pageID, string refNO, int? stateID);

        void USP_Change_WorkFlow_State(int? pageID, string refNO, int? stateID, string sTo1, string sCc1, string sBcc1, string sj1, string sj2, string sj3, string ms1, string ms2, string ms3, string attachedFile);
        void USP_Change_WorkFlow_State(int? pageID, string refNO, int? stateID);

        int GetPageId(string appsCode, string PageName);

        string USP_Get_ServerName();
        string USP_Util_GetEnvionmentName();

        DateTime USP_Get_PreWorkDate(DateTime? curDate, int? numberOfDate);
        string USP_IsInApplGroup(string applGroup);
        string USP_Get_UserName();
        string USP_Get_UserFullName(string loginName);
    }
}
