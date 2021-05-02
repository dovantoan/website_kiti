using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Shared.Utility
{
    public class CreateDataSet
    {

        public static DataSet WorkDeadLine()
        {
            DataSet ds = new DataSet();
            DataTable dtWorkOderInfomation = new DataTable("dtWorkOderInfomation");
            dtWorkOderInfomation.Columns.Add("ItemCode", typeof(System.String));
            dtWorkOderInfomation.Columns.Add("Revision", typeof(System.Int32));
            dtWorkOderInfomation.Columns.Add("NameEN", typeof(System.String));
            dtWorkOderInfomation.Columns.Add("NameVN", typeof(System.String));
            dtWorkOderInfomation.Columns.Add("Qty", typeof(System.Int32));
            dtWorkOderInfomation.Columns.Add("RowState", typeof(System.Int32));
            ds.Tables.Add(dtWorkOderInfomation);

            DataTable dtMCHDeadline = new DataTable("dtMCHDeadline");
            dtMCHDeadline.Columns.Add("Pid", typeof(System.Int64));
            dtMCHDeadline.Columns.Add("ItemCode", typeof(System.String));
            dtMCHDeadline.Columns.Add("Revision", typeof(System.Int32));
            dtMCHDeadline.Columns.Add("Deadline", typeof(System.DateTime));
            dtMCHDeadline.Columns.Add("Qty", typeof(System.Int32));
            dtMCHDeadline.Columns.Add("RowState", typeof(System.Int32));
            ds.Tables.Add(dtMCHDeadline);

            DataTable tdASSDeadline = new DataTable("dtASSDeadline");
            tdASSDeadline.Columns.Add("Pid", typeof(System.Int64));
            tdASSDeadline.Columns.Add("ItemCode", typeof(System.String));
            tdASSDeadline.Columns.Add("Revision", typeof(System.Int32));
            tdASSDeadline.Columns.Add("Deadline", typeof(System.DateTime));
            tdASSDeadline.Columns.Add("Qty", typeof(System.Int32));
            tdASSDeadline.Columns.Add("RowState", typeof(System.Int32));
            ds.Tables.Add(tdASSDeadline);

            DataTable tdSUBDeadline = new DataTable("dtSUBDeadline");
            tdSUBDeadline.Columns.Add("Pid", typeof(System.Int64));
            tdSUBDeadline.Columns.Add("ItemCode", typeof(System.String));
            tdSUBDeadline.Columns.Add("Revision", typeof(System.Int32));
            tdSUBDeadline.Columns.Add("Deadline", typeof(System.DateTime));
            tdSUBDeadline.Columns.Add("Qty", typeof(System.Int32));
            tdSUBDeadline.Columns.Add("RowState", typeof(System.Int32));
            ds.Tables.Add(tdSUBDeadline);

            DataTable tdPACDeadline = new DataTable("dtPACDeadline");
            tdPACDeadline.Columns.Add("Pid", typeof(System.Int64));
            tdPACDeadline.Columns.Add("ItemCode", typeof(System.String));
            tdPACDeadline.Columns.Add("Revision", typeof(System.Int32));
            tdPACDeadline.Columns.Add("Deadline", typeof(System.DateTime));
            tdPACDeadline.Columns.Add("Qty", typeof(System.Int32));
            tdPACDeadline.Columns.Add("RowState", typeof(System.Int32));
            ds.Tables.Add(tdPACDeadline);

            DataRelation dtWorkOderInfomation_dtMCHDeadline = new DataRelation("dtWorkOderInfomation_dtMCHDeadline", new DataColumn[] { dtWorkOderInfomation.Columns["ItemCode"], dtWorkOderInfomation.Columns["Revision"] }, new DataColumn[] { dtMCHDeadline.Columns["ItemCode"], dtMCHDeadline.Columns["Revision"] }, false);

            DataRelation dtWorkOderInfomation_tdASSDeadline = new DataRelation("dtWorkOderInfomation_tdASSDeadline", new DataColumn[] { dtWorkOderInfomation.Columns["ItemCode"], dtWorkOderInfomation.Columns["Revision"] }, new DataColumn[] { tdASSDeadline.Columns["ItemCode"], tdASSDeadline.Columns["Revision"] }, false);

            DataRelation dtWorkOderInfomation_tdSUBDeadline = new DataRelation("dtWorkOderInfomation_tdSUBDeadline", new DataColumn[] { dtWorkOderInfomation.Columns["ItemCode"], dtWorkOderInfomation.Columns["Revision"] }, new DataColumn[] { tdSUBDeadline.Columns["ItemCode"], tdSUBDeadline.Columns["Revision"] }, false);

            DataRelation dtWorkOderInfomation_tdPACDeadline = new DataRelation("dtWorkOderInfomation_tdPACDeadline", new DataColumn[] { dtWorkOderInfomation.Columns["ItemCode"], dtWorkOderInfomation.Columns["Revision"] }, new DataColumn[] { tdPACDeadline.Columns["ItemCode"], tdPACDeadline.Columns["Revision"] }, false);

            ds.Relations.Add(dtWorkOderInfomation_dtMCHDeadline);
            ds.Relations.Add(dtWorkOderInfomation_tdASSDeadline);
            ds.Relations.Add(dtWorkOderInfomation_tdSUBDeadline);
            ds.Relations.Add(dtWorkOderInfomation_tdPACDeadline);

            return ds;
        }

    }
}
