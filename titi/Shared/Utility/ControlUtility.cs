using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Xml;
using System;
using System.Collections.Generic;

namespace Shared.Utility
{
    public class ControlUtility
    {
        public static string language = "vn";

        /// <summary>
        /// This method is used to Permision.
        /// </summary>
        public static void UserAccessRight(int userPid, Control control)
        {
            //string strCmdCtr = "Select ControlName From TblGNRDefineUIControl ";
            //strCmdCtr += string.Format("Where UICode = '{0}'", control.Name);
            //DataTable dtCtr = Shared.DataBaseUtility.DataBaseAccess.SearchCommandTextDataTable(strCmdCtr);

            ////search permision of group
            //string commandText = "SELECT	COUNT(*) FROM	TblGNRAccessGroup G " +
            //                     " INNER JOIN TblGNRAccessGroupUser GU ON GU.GroupPid = G.Pid " +
            //                     " INNER JOIN TblBOMUser U ON U.Pid = GU.UserPid AND U.EmployeePid =" + SharedObject.UserInfo.UserPid +
            //                     " INNER JOIN TblBOMCodeMaster C ON C.Code = G.[Role] AND C.Code = 1 AND C.[Group] =" + Shared.Utility.ConstantClass.GROUP_ROLE;
            //int count = DBConvert.ParseInt(DataBaseAccess.ExecuteScalarCommandText(commandText).ToString());

            //for (int i = 0; i < dtCtr.Rows.Count; i++)
            //{
            //  if (SharedObject.UserInfo.UserPid != ConstantClass.UserAddmin && count == 0)
            //    AccessControl(dtCtr.Rows[i]["ControlName"].ToString(), control, false);
            //}

            //string storeName = "spGNRGroupUIControl_Access";

            //DBParameter[] inputParam = new DBParameter[2];
            //inputParam[0] = new DBParameter("@EmpPid", DbType.Int32, userPid);
            //inputParam[1] = new DBParameter("@UICode", DbType.AnsiString, 128, control.Name);

            //DataTable dtCompUser = Shared.DataBaseUtility.DataBaseAccess.SearchStoreProcedureDataTable(storeName, inputParam);
            //if(dtCompUser != null){
            //  for (int i = 0; i < dtCompUser.Rows.Count; i++)
            //  {
            //    AccessControl(dtCompUser.Rows[i]["ControlName"].ToString(), control, true);
            //  }
            //}
        }

        /// <summary>
        /// Recursive to Visible or Invisible a control and some chile its controls
        /// </summary>
        /// <param name="strControlName"></param>
        /// <param name="control"></param>
        /// <param name="bAccess"></param>
        public static void AccessControl(string strControlName, Control control, bool bAccess)
        {
            foreach (Control ctr in control.Controls)
            {
                if (string.Compare(strControlName, ctr.Name, true) == 0)
                {
                    ctr.Visible = bAccess;
                }
                else
                {
                    if (ctr.Controls.Count > 0)
                    {
                        AccessControl(strControlName, ctr, bAccess);
                    }
                }
            }
        }

        public static void LoadCombobox(ComboBox cmb, DataTable dtSoure, string columnValue, string columnText)
        {
            if (dtSoure != null)
            {
                DataTable dt = dtSoure.Clone();
                dt.Merge(dtSoure);
                DataRow row = dt.NewRow();
                dt.Rows.InsertAt(row, 0);
                cmb.DataSource = dt;
                cmb.DisplayMember = columnText;
                cmb.ValueMember = columnValue;
            }
        }

        public static string GetSelectedValueCombobox(ComboBox cmb)
        {
            string value = string.Empty;
            try
            {
                value = cmb.SelectedValue.ToString();
            }
            catch { }
            return value;
        }

        /// <summary>
        /// Disable all controls in view, except control in conEnables
        /// </summary>
        /// <param name="container">View need disable</param>
        /// <param name="conEnables">Array of control enable</param>
        public static void LockControl(Control container, params Control[] conEnables)
        {
            foreach (Control control in container.Controls)
            {
                LockControlRecursive(control, conEnables);
            }
        }

        /// <summary>
        /// Recursive disable controls
        /// </summary>
        /// <param name="control"></param>
        /// <param name="conEnables"></param>
        public static void LockControlRecursive(Control control, params Control[] conEnables)
        {
            foreach (Control conTrack in control.Controls)
            {
                bool isAllow = false;
                foreach (Control conEnable in conEnables)
                {
                    if (conEnable.Name == conTrack.Name)
                    {
                        conEnable.Enabled = true;
                        isAllow = true;
                        break;
                    }
                }
                if (!isAllow)
                {
                    if (conTrack.Controls.Count > 0)
                        LockControlRecursive(conTrack, conEnables);
                    else if (conTrack.GetType() != typeof(Label))
                        conTrack.Enabled = false;
                }
            }
        }
    }
}