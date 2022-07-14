using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Phatra.Core.AdoNet;
using Phatra.Core.Logging;

namespace Phatra.Core.Managers
{
    public class IfisManager : BaseManager, IIfisManager
    {

        private readonly ILogger _logger;

        public IfisManager(ILogger logger)
        {
            _logger = logger;
        }

        public string UpdateStockInIFIS(IDbTransaction transaction, string inRefNo)
        {
            string UpdateType;
            string Ac_No;
            string StockSym;
            string Stock_Type;
            decimal Quantity;
            decimal Cost_Price;
            string TTF_Flag;
            decimal Buy_CR;
            decimal Sell_CR;
            decimal Credit_Line;
            decimal Upper_Credit;
            string Flag;
            string Ref_No;
            string Result;
            string sResult = "OK";

            using (var db = DatabaseFactory.CreateDefaultDatabase())
            {
                var query = db.StoredProcedure("[dbo].[USP_Front_UpdateStockAndCredit]")
                        .WithParameter("@Ref_NO", inRefNo);

                if (transaction != null)
                {
                    query.InTransaction(transaction);
                }

                var list = query.AsEnumerable().ToList();

                list.ForEach(c =>
                {
                    UpdateType = c.Update_Type;

                    if (UpdateType.CompareTo("P") == 0)
                    {
                        Ac_No = c.AC_NO;
                        StockSym = c.Sec_Code;
                        Stock_Type = c.Stock_Type;
                        Quantity = Convert.ToDecimal(c.Quantity_Update);
                        Cost_Price = Convert.ToDecimal(c.Cost_Price);
                        TTF_Flag = c.TTF_ID;
                        Ref_No = c.Upd_NO;

                        Result = Store_Up_Position(transaction, Ac_No, StockSym, Stock_Type, Quantity, Cost_Price, TTF_Flag, Ref_No);
                    }
                    else
                    {
                        Ac_No = c.AC_NO;
                        Buy_CR = Convert.ToDecimal(c.Buy_CR_Update);
                        Sell_CR = Convert.ToDecimal(c.Sell_CR_Update);
                        Credit_Line = Convert.ToDecimal(c.CR_Line_Update);
                        Upper_Credit = 0;
                        Flag = c.Trading_Flag;
                        Ref_No = c.Upd_NO;

                        Result = Store_Up_Credit_Maintain(transaction, Ac_No, Buy_CR, Sell_CR, Credit_Line, Upper_Credit, Flag, Ref_No);
                    }

                    if (Result != "OK") sResult = Result;
                });
            }

            return sResult;
        }

        public string UpdateStockInIFIS(string inRefNo)
        {
            return UpdateStockInIFIS(null, inRefNo);
        }

        public string Store_Up_Position(IDbTransaction transaction, string acNo, string stockSym, string stockType, decimal quantity, decimal costPrice, string ttfFlag, string refNo)
        {
            using (var db = DatabaseFactory.CreateDefaultDatabase())
            {
                //SqlParameter err = new SqlParameter("@Err", System.Data.SqlDbType.Int);
                //err.Direction = System.Data.ParameterDirection.Output;

                //SqlParameter msg = new SqlParameter("@Msg", System.Data.SqlDbType.VarChar, 255);
                //err.Direction = System.Data.ParameterDirection.Output;

                var query = db.StoredProcedure("SQLTRADESERVER.IFIS.dbo.USP_IFIS_SendPositionToIfis")
                    .WithParameter("@AC_NO", acNo)
                    .WithParameter("@Sec_Code", stockSym)
                    .WithParameter("@Stock_Type", stockType)
                    .WithParameter("@Quantity", quantity)
                    .WithParameter("@Cost_Price", costPrice)
                    .WithParameter("@TTF_Flag", ttfFlag)
                    .WithParameter("@Ref_No", refNo);

                IDbDataParameter err = query.CreateParameter();
                err.ParameterName = "@Err";
                err.DbType = DbType.Int16;
                err.Direction = ParameterDirection.Output;

                IDbDataParameter msg = query.CreateParameter();
                msg.ParameterName = "@Msg";
                msg.DbType = DbType.String;
                msg.Size = 255;
                msg.Direction = ParameterDirection.Output;

                query.WithParameter(err)
                    .WithParameter(msg);

                if (transaction != null)
                {
                    query.InTransaction(transaction);
                }
                query.AsNonQuery();
            }

            return "OK";
        }

        public string Store_Up_Credit_Maintain(IDbTransaction transaction, string acNo, decimal buyCr, decimal sellCr, decimal creditLine, decimal upperCredit, string flag, string refNo)
        {
            using (var db = DatabaseFactory.CreateDefaultDatabase())
            {
                var query = db.StoredProcedure("SQLTRADESERVER.IFIS.dbo.USP_IFIS_SendCreditToIfis");

                var err = query.CreateParameter("@Err", DbType.Int16);
                err.Direction = ParameterDirection.Output;

                var msg = query.CreateParameter("@Msg", DbType.String);
                msg.Size = 255;
                msg.Direction = ParameterDirection.Output;

                query.WithParameter("@AC_NO", acNo)
                    .WithParameter("@Buy_CR_Avail", buyCr)
                    .WithParameter("@Sell_CR_Avail", sellCr)
                    .WithParameter("@Credit_Line", creditLine)
                    .WithParameter("@Upper_Credit", upperCredit)
                    .WithParameter("@Flag", flag)
                    .WithParameter("@Ref_No", refNo)
                    .WithParameter(err)
                    .WithParameter(msg);

                if (transaction != null)
                {
                    query.InTransaction(transaction);
                }
                query.AsNonQuery();
            }
            return "OK";
        }

        public string UpdateCashInIFIS(string inDocNo, string inUpdateType, decimal? inSendSbl)
        {
            var results = "OK";
            using (var db = DatabaseFactory.CreateDefaultDatabase())
            {
                try
                {
                    var list = db.StoredProcedure("[dbo].[USP_Front_UpdateCashAndCredit]")
                                .WithParameter("@Doc_NO", inDocNo)
                                .WithParameter("@Update_Type", inUpdateType)
                                .WithParameter("@Send_Sbl_Amt", inSendSbl)
                                .WithNoTimeout()
                                .AsEnumerable<USP_Front_UpdateCashAndCredit>()
                                .ToList();

                    if (list != null && list.Count > 0)
                    {

                        foreach (var item in list)
                        {
                            var result = "OK";
                            if (item.Update_Type != "P")
                            {
                                result = Store_Up_Credit_Maintain(null, item.AC_NO, item.Buy_CR_Update, item.Sell_CR_Update, item.CR_Line_Update, 0, item.Trading_Flag, item.Upd_NO);
                            }
                            if (result != "OK")
                            {
                                results = result;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"{ex.ToString()}");
                    throw;
                }
            }

            return results;
        }

        public string USP_IFIS_ClearMaintainFromBack()
        {
            var results = "OK";
            using (var db = DatabaseFactory.CreateDefaultDatabase())
            {
                var query = db.StoredProcedure("SQLTRADESERVER.IFIS.dbo.USP_IFIS_ClearMaintainFromBack");

                IDbDataParameter err = query.CreateParameter();
                err.ParameterName = "@Err";
                err.DbType = DbType.Int16;
                err.Direction = ParameterDirection.Output;

                IDbDataParameter msg = query.CreateParameter();
                msg.ParameterName = "@Msg";
                msg.DbType = DbType.String;
                msg.Size = 255;
                msg.Direction = ParameterDirection.Output;

                query.WithParameter(err)
                    .WithParameter(msg);

                query.AsNonQuery();

                var strMsg = (string)msg.Value;
                if (strMsg != "OK")
                {
                    throw new Exception(strMsg);
                }
            }
            return results;
        }

        public class USP_Front_UpdateCashAndCredit
        {
            public string Upd_NO { get; set; }
            public string AC_NO { get; set; }
            public decimal Buy_CR_Update { get; set; }
            public decimal Sell_CR_Update { get; set; }
            public decimal CR_Line_Update { get; set; }
            public string Sec_Code { get; set; }
            public decimal Quantity_Update { get; set; }
            public string TTF_ID { get; set; }
            public string Update_Type { get; set; }
            public decimal Cost_Price { get; set; }
            public string Trading_Flag { get; set; }
            public string Stock_Type { get; set; }
        }

    }

    public interface IIfisManager
    {
        //string UpdateStockInIFIS(IDbTransaction transaction, string inRefNo);

        string UpdateStockInIFIS(string inRefNo);

        string UpdateCashInIFIS(string inDocNo, string inUpdateType, decimal? inSendSbl);

        string USP_IFIS_ClearMaintainFromBack();

        string Store_Up_Position(IDbTransaction transaction, string acNo, string stockSym, string stockType, decimal quantity, decimal costPrice, string ttfFlag, string refNo);

        string Store_Up_Credit_Maintain(IDbTransaction transaction, string acNo, decimal buyCr, decimal sellCr, decimal creditLine, decimal upperCredit, string flag, string refNo);
    }
}
