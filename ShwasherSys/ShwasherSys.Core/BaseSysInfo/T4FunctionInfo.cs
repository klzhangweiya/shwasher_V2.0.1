using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Abp.Extensions;

namespace ShwasherSys.BaseSysInfo
{
    public class T4Function
    {
        //private static readonly string ConnectionString = WebConfigurationManager.ConnectionStrings["Default"].ToString();
        private static readonly string ConnectionString = @"Server=120.25.240.119; Database=DBwasher2019; User Id=sa;Password=syfsz,./;";
        //private static readonly string ConnectionString = @"Server=47.112.30.247,50681; Database=DBwasher2019; User Id=sa;Password=Iwb2017";
        public static T4FunctionInfo GetRootFunction()
        {
            List<T4FunctionInfo> loFuns = GetAllFunctions();
            T4FunctionInfo loRootFun = loFuns.FirstOrDefault(a => a.Id == 1);
            if (loRootFun != null)
            {
                loFuns.Remove(loRootFun);
                loRootFun.Children = GetChildFunctions(loRootFun.FunctionNo, loFuns);
            }
            return loRootFun;
        }

        public static T4FunctionInfo GetRootFunctionWithoutBtn()
        {
            List<T4FunctionInfo> loFuns = GetAllFunctions(false);
            T4FunctionInfo loRootFun = loFuns.FirstOrDefault(a => a.Id == 1);
            if (loRootFun != null)
            {
                loFuns.Remove(loRootFun);
                loRootFun.Children = GetChildFunctions(loRootFun.FunctionNo, loFuns);
            }
            return loRootFun;
        }

        private static List<T4FunctionInfo> GetChildFunctions(string pcPraentNo, List<T4FunctionInfo> poFunctions)
        {
            List<T4FunctionInfo> loFuns = new List<T4FunctionInfo>();
            var loParentFuns = poFunctions.Where(a => a.ParentNo == pcPraentNo).ToList();
            if (loParentFuns.Any())
            {
                foreach (var f in loParentFuns)
                {
                    f.Children = GetChildFunctions(f.FunctionNo, poFunctions);
                    loFuns.Add(f);
                }
            }
            return loFuns;
        }

        public static List<T4FunctionInfo> GetAllFunctions(bool hasBtn = true)
        {
            using (var loSqlConn = new SqlConnection(ConnectionString))
            {
                using (loSqlConn.CreateCommand())
                {
                    loSqlConn.Open();
                    string lcSql =
                        "SELECT [Id],[FunctionNo],[ParentNo],[FunctionName],[PermissionName],[FunctionType],[FunctionPath],[Action],[Controller],[Url],[Icon],[Sort],[Depth]  FROM [Sys_Functions] WHERE [IsDeleted] = 0 ";
                    if (!hasBtn)
                    {
                        lcSql += " AND [FunctionType] !=2 ";
                    }
                    lcSql += " ORDER BY[Depth],[Sort]";
                    SqlDataAdapter loDataAdapter = new SqlDataAdapter(lcSql, loSqlConn);
                    DataSet loDataSet = new DataSet();      // 创建DataSet
                    loDataAdapter.Fill(loDataSet, "Function");
                    DataTable loTable = loDataSet.Tables["Function"];
                    var loFuns = new List<T4FunctionInfo>();
                    foreach (DataRow row in loTable.Rows)
                    {
                        string url = row["Url"] + "", controller = row["Controller"] + "", action = row["Action"] + "";
                        if (url.IsNullOrEmpty() && !controller.IsNullOrEmpty() && !action.IsNullOrEmpty())
                        {
                            url = "/" + row["Controller"] + "/" + row["Action"];
                        }
                        loFuns.Add(new T4FunctionInfo()
                        {

                            Id = (int)row["Id"],
                            FunctionNo = row["FunctionNo"] + "",
                            ParentNo = row["ParentNo"] + "",
                            PermissionName = row["PermissionName"] + "",
                            FunctionName = row["FunctionName"] + "",
                            FunctionType = row["FunctionType"] + "",
                            FunctionPath = row["FunctionPath"] + "",
                            Action = action,
                            Controller = controller,
                            Url = url,
                            Icon = row["Icon"] + "",
                            Sort = (int)row["Sort"],
                            Depth = (int)row["Depth"]

                        });
                    }
                    return loFuns;
                }
            }
        }
    }

    public class T4FunctionInfo
    {
        public int Id { get; set; }
        public string FunctionNo { get; set; }
        public string ParentNo { get; set; }
        public string FunctionName { get; set; }
        public string PermissionName { get; set; }
        public string FunctionType { get; set; }
        public string FunctionPath { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int Sort { get; set; }
        public int Depth { get; set; }
        public List<T4FunctionInfo> Children { get; set; }

    }
}
