using HUDHealthcarePortal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using System.Configuration;
using HUDHealthcarePortal.Core;
using HUDHealthcarePortal.BusinessService;
using HUDHealthcarePortal.BusinessService.Interface;

namespace HUDHealthCarePortal.Controllers
{
    [Authorize]
    public class ExcelUploadController : Controller
    {
        IUploadDataManager uploadDataMgr;

        public ExcelUploadController() : this(new UploadDataManager())
        {

        }

        public ExcelUploadController(IUploadDataManager uploadDataManager)
        {
            uploadDataMgr = uploadDataManager;
        }

        [HttpGet]
        public ActionResult ExcelUploadView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Importexcel()
        {
            OleDbConnection excelConnection = null;
            Session["Confirmation"] = null;
            if (Request.Files["FileUpload1"].ContentLength > 0)
            {
                try
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName);
                    string sPathUploadingFileLocation = Request.Files["FileUpload1"].FileName;
                    string sFileName = System.IO.Path.GetFileName(Request.Files["FileUpload1"].FileName);
                    string sPathStoring = @"C:\inetpub\wwwroot\Uploaded files\" + sFileName;
                    int iLenderID;
                    int iUserID;

                    if (System.IO.File.Exists(sPathStoring))
                        System.IO.File.Delete(sPathStoring);

                    //
                    ////////////////////////////////////////Initial Excel Upload///////////////////////////////////////
                    //
                    Request.Files["FileUpload1"].SaveAs(sPathStoring);
                    string sqlConnectionString = ConfigurationManager.ConnectionStrings["Connection_HCP_Intermediate_db"].ConnectionString;

                    //Create connection string to Excel work book
                    //string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sPathUploadingFileLocation + ";Extended Properties=Excel 12.0;Persist Security Info=True";
                    string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sPathStoring + ";Extended Properties=Excel 12.0;Persist Security Info=True";

                    //Create Connection to Excel work book
                    excelConnection = new OleDbConnection(excelConnectionString);

                    //Create OleDbCommand to fetch data from Excel
                    OleDbCommand cmd = new OleDbCommand("Select [Project Name],[Servicer Name],[Lender ID],[Period Ending],[Months In Period],[FHA Number],[Resident Patient Days],[Total beds operated],[Operating Cash],[Investments],[Reserve for Replacement Escrow Balance],[Accounts Receivable],[Current Assets],[Current Liabilities],[Total Revenues],[Rent/Lease Expense],[Depreciation Expense],[Amortization Expense],[Total Expenses],[Net Income],[FHA Insured Mortgage Principal Payment],[FHA Insured Mortgage Interest Expense],[Mortgage Insurance Premium (MIP)] from [Sheet1$]", excelConnection);

                    OleDbDataAdapter adp = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.SelectCommand = cmd;
                    adp.Fill(dt);
                    dt.Columns.Add("UserID");

                    foreach (DataRow dRow in dt.Rows)
                    {
                        dRow["UserID"] = UserPrincipal.Current.UserId;
                    }

                    iLenderID = Convert.ToInt32(dt.Rows[1]["Lender ID"]);
                    if(UserPrincipal.Current.IsInRole("Lender") && UserPrincipal.Current.LenderId != iLenderID)
                    {
                        Session["Confirmation"] = "You are not allowed to upload file for another lender.";
                        return RedirectToAction("ExcelUploadView", "ExcelUpload");
                    }                    

                    excelConnection.Open();

                    using (SqlBulkCopy sqlBulk = new SqlBulkCopy(sqlConnectionString))
                    {
                        // Set up the column mappings by ordinal.
                        SqlBulkCopyColumnMapping column0 =
                            new SqlBulkCopyColumnMapping("Project Name", "ProjectName");
                        sqlBulk.ColumnMappings.Add(column0);

                        SqlBulkCopyColumnMapping column1 =
                            new SqlBulkCopyColumnMapping("Servicer Name", "ServiceName");
                        sqlBulk.ColumnMappings.Add(column1);

                        SqlBulkCopyColumnMapping column2 =
                            new SqlBulkCopyColumnMapping("Lender ID", "LenderID");
                        sqlBulk.ColumnMappings.Add(column2);

                        SqlBulkCopyColumnMapping column3 =
                            new SqlBulkCopyColumnMapping("Period Ending", "PeriodEnding");
                        sqlBulk.ColumnMappings.Add(column3);

                        SqlBulkCopyColumnMapping column4 =
                            new SqlBulkCopyColumnMapping("Months In Period", "MonthsInPeriod");
                        sqlBulk.ColumnMappings.Add(column4);

                        SqlBulkCopyColumnMapping column5 =
                            new SqlBulkCopyColumnMapping("FHA Number", "FHANumber");
                        sqlBulk.ColumnMappings.Add(column5);

                        SqlBulkCopyColumnMapping column6 =
                            new SqlBulkCopyColumnMapping("Resident Patient Days", "ResidentPatientDays");
                        sqlBulk.ColumnMappings.Add(column6);

                        SqlBulkCopyColumnMapping column7 =
                            new SqlBulkCopyColumnMapping("Total beds operated", "TotalBedsoperated");
                        sqlBulk.ColumnMappings.Add(column7);

                        SqlBulkCopyColumnMapping column8 =
                            new SqlBulkCopyColumnMapping("Operating Cash", "OperatingCash");
                        sqlBulk.ColumnMappings.Add(column8);

                        SqlBulkCopyColumnMapping column9 =
                            new SqlBulkCopyColumnMapping("Investments", "Investments");
                        sqlBulk.ColumnMappings.Add(column9);

                        SqlBulkCopyColumnMapping column10 =
                            new SqlBulkCopyColumnMapping("Reserve for Replacement Escrow Balance", "ReserveForReplacementEscrowBalance");
                        sqlBulk.ColumnMappings.Add(column10);

                        SqlBulkCopyColumnMapping column11 =
                            new SqlBulkCopyColumnMapping("Accounts Receivable", "AccountsReceivable");
                        sqlBulk.ColumnMappings.Add(column11);

                        SqlBulkCopyColumnMapping column12 =
                            new SqlBulkCopyColumnMapping("Current Assets", "CurrentAssets");
                        sqlBulk.ColumnMappings.Add(column12);

                        SqlBulkCopyColumnMapping column13 =
                            new SqlBulkCopyColumnMapping("Current Liabilities", "CurrentLiabilities");
                        sqlBulk.ColumnMappings.Add(column13);

                        SqlBulkCopyColumnMapping column14 =
                            new SqlBulkCopyColumnMapping("Total Revenues", "TotalRevenues");
                        sqlBulk.ColumnMappings.Add(column14);

                        SqlBulkCopyColumnMapping column15 =
                            new SqlBulkCopyColumnMapping(@"Rent/Lease Expense", "RentLeaseExpense");
                        sqlBulk.ColumnMappings.Add(column15);

                        SqlBulkCopyColumnMapping column16 =
                            new SqlBulkCopyColumnMapping("Depreciation Expense", "DepreciationExpense");
                        sqlBulk.ColumnMappings.Add(column16);

                        SqlBulkCopyColumnMapping column17 =
                            new SqlBulkCopyColumnMapping("Amortization Expense", "AmortizationExpense");
                        sqlBulk.ColumnMappings.Add(column17);

                        SqlBulkCopyColumnMapping column18 =
                            new SqlBulkCopyColumnMapping("Total Expenses", "TotalExpenses");
                        sqlBulk.ColumnMappings.Add(column18);

                        SqlBulkCopyColumnMapping column19 =
                            new SqlBulkCopyColumnMapping("Net Income", "NetIncome");
                        sqlBulk.ColumnMappings.Add(column19);

                        SqlBulkCopyColumnMapping column20 =
                            new SqlBulkCopyColumnMapping("FHA Insured Mortgage Principal Payment", "FHAInsuredMortgagePrincipalPayment");
                        sqlBulk.ColumnMappings.Add(column20);

                        SqlBulkCopyColumnMapping column21 =
                            new SqlBulkCopyColumnMapping("FHA Insured Mortgage Interest Expense", "FHAInsuredMortgageInterestExpense");
                        sqlBulk.ColumnMappings.Add(column21);

                        SqlBulkCopyColumnMapping column22 =
                            new SqlBulkCopyColumnMapping("Mortgage Insurance Premium (MIP)", "MortgageInsurancePremium");
                        sqlBulk.ColumnMappings.Add(column22);

                        SqlBulkCopyColumnMapping column23 =
                            new SqlBulkCopyColumnMapping("UserID", "UserID");
                        sqlBulk.ColumnMappings.Add(column23);

                        //Give your Destination table name
                        sqlBulk.DestinationTableName = "Lender_DataUpload_Intermediate";
                        sqlBulk.WriteToServer(dt);
                    }

                    //
                    ////////////////////////////////////////Initial Excel Upload///////////////////////////////////////
                    //                
                    string sConnection_HCP_Live_db = ConfigurationManager.ConnectionStrings["Connection_HCP_Live_db"].ConnectionString;
                    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(sConnection_HCP_Live_db);
                    System.Data.SqlClient.SqlCommand command1 = new System.Data.SqlClient.SqlCommand();
                    iUserID = UserPrincipal.Current.UserId;
                    conn.Open();

                    command1.Connection = conn;
                    command1.CommandText = "usp_HCP_Verify_PropertyID_iRems";
                    command1.CommandType = CommandType.StoredProcedure;

                    // Add the input parameter and set its properties.
                    System.Data.SqlClient.SqlParameter parameter = new System.Data.SqlClient.SqlParameter();

                    //Lender ID
                    parameter.ParameterName = "@LenderID";
                    parameter.SqlDbType = SqlDbType.Int;
                    parameter.Direction = ParameterDirection.Input;

                    //Add the parameter to the Parameters collection - User Name.
                    command1.Parameters.AddWithValue("LenderID", iLenderID);

                    //User ID
                    parameter.ParameterName = "@UserID";
                    parameter.SqlDbType = SqlDbType.Int;
                    parameter.Direction = ParameterDirection.Input;

                    // Add the parameter to the Parameters collection - Password.
                    command1.Parameters.AddWithValue("UserID", iUserID);
                    command1.ExecuteNonQuery();

                    uploadDataMgr.CalcAndUpdateUploadedData();

                    excelConnection.Close();
                    Session["Confirmation"] = "File Uploaded Successfully (" + sFileName + ")";
                }
                catch (Exception ex)
                {
                    Session["Confirmation"] = "Encountered Problem while uploading file";
                    string errorMessage = ex.GetBaseException().Message;
                    //throw new InvalidOperationException("error while uploading", ex);
                }
                finally
                {
                    if (excelConnection != null)
                        excelConnection.Dispose();
                }
            }

            return RedirectToAction("ExcelUploadView", "ExcelUpload");
        }


    }
}