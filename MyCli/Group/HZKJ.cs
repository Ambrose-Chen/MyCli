using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MyCli.Group
{
    class HZKJ : IGroup
    {
        static HZKJ()
        {
            SqlServerUrl = @"10.168.96.150\SQL2008R2";
            DataBase = "PSFSYS";
            User = "YIQI";
            Passwd = "szyiqi";
        }
        private static string SqlServerUrl { get; set; }
        private static string DataBase { get; set; }
        private static string User { get; set; }
        private static string Passwd { get; set; }
        private static string ConnectionString => $@"Data Source={SqlServerUrl};Initial Catalog={DataBase};uid={User};password={Passwd}";
        private static DataTable Dt { get; set; }
        public List<string> Commands() =>
             new List<string> { "unlock", "showtables", "conf", "setconf", "startdate", "enddate" };


        public void Run(string[] Command)
        {
            switch (Command[0])
            {
                case "showtables":
                    if (Command.Length > 1)
                        Showtables(Command[1]);
                    else
                        Showtables(string.Empty);
                    break;
                case "unlock":
                    Unlock(Command[1]);
                    break;
                case "conf":
                    Conf();
                    break;
                case "setconf":
                    SetConf();
                    break;
                case "startdate":
                    StartDate();
                    break;
                case "enddate":
                    EndDate();
                    break;
            }
        }
        public void Unlock(string keypin)
        {
            string sql = $"update PIUSERLOGON set PASSWORDERRORCOUNT=0,LOCKENDDATE='2013-04-04' where ID=(select ID from PIUSER where KEYPIN='{keypin}')";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                try
                {
                    using (SqlCommand com = new SqlCommand(sql, con))
                    {
                        int n = com.ExecuteNonQuery();
                        if (n > 0)
                            Console.WriteLine($"keypin: {keypin}, unlock Success");
                        else
                            Console.WriteLine($"unlock Failed: {keypin} does not exist");
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        public void Showtables(string tab)
        {
            string sql = $@"
                SELECT  obj.name AS tablename,
                        col.colorder AS num ,
                        col.name AS colname ,
                        ISNULL(ep.[value], '') AS colc ,
                        t.name AS datatype ,
                        col.length AS length ,
                        ISNULL(COLUMNPROPERTY(col.id, col.name, 'Scale'), 0) AS scale ,
                        CASE WHEN COLUMNPROPERTY(col.id, col.name, 'IsIdentity') = 1 THEN '1'
                             ELSE '0'
                        END AS isidentity ,
                        CASE WHEN EXISTS ( SELECT   1
                                           FROM     dbo.sysindexes si
                                                    INNER JOIN dbo.sysindexkeys sik ON si.id = sik.id
                                                                              AND si.indid = sik.indid
                                                    INNER JOIN dbo.syscolumns sc ON sc.id = sik.id
                                                                              AND sc.colid = sik.colid
                                                    INNER JOIN dbo.sysobjects so ON so.name = si.name
                                                                              AND so.xtype = 'PK'
                                           WHERE    sc.id = col.id
                                                    AND sc.colid = col.colid ) THEN '1'
                             ELSE '0'
                        END AS prikey ,
                        CASE WHEN col.isnullable = 1 THEN '1'
                             ELSE '0'
                        END AS accessnull ,
                        ISNULL(comm.text, '') AS defaults
                FROM    dbo.syscolumns col
                        LEFT  JOIN dbo.systypes t ON col.xtype = t.xusertype
                        inner JOIN dbo.sysobjects obj ON col.id = obj.id
                                                         AND obj.xtype = 'U'
                                                         AND obj.status >= 0
                        LEFT  JOIN dbo.syscomments comm ON col.cdefault = comm.id
                        LEFT  JOIN sys.extended_properties ep ON col.id = ep.major_id
                                                                      AND col.colid = ep.minor_id
                                                                      AND ep.name = 'MS_Description'
                        LEFT  JOIN sys.extended_properties epTwo ON obj.id = epTwo.major_id
                                                                         AND epTwo.minor_id = 0
                                                                         AND epTwo.name = 'MS_Description'
                WHERE   obj.name in (select name from sys.tables where name like '%{tab}%')
                ORDER BY obj.name,col.colorder
                ";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                    da.Fill(ds, "ds");
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Console.WriteLine($"{dr[0]}\t{dr[1]}\t{dr[2]}\t{dr[3]}\t{dr[4]}\t{dr[5]}\t{dr[6]}\t{dr[7]}\t{dr[8]}\t{dr[9]}\t{dr[10]}");
                        }
                    }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public void Conf() => Console.WriteLine($"ConnectionString: {ConnectionString}");
        public void SetConf()
        {
            foreach (KeyValue<string, string> kv in Common.FormatCommand())
            {
                switch (kv.Key)
                {
                    case "d":
                        DataBase = kv.Value;
                        break;
                    case "s":
                        SqlServerUrl = kv.Value;
                        break;
                    case "u":
                        User = kv.Value;
                        break;
                    case "p":
                        Passwd = kv.Value;
                        break;
                }
            }
        }
        public void StartDate()
        {
            Console.WriteLine(DateTime.Parse("2017-04-10").ToString("d"));
        }
        public void EndDate()
        {
            Console.WriteLine(DateTime.Parse("2018-08-31").ToString("d"));
        }
    }
}
