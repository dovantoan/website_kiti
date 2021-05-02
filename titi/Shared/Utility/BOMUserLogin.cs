using System;
namespace Shared
{
    public class BOMUserLogin
    {
        #region Fields
        private string userName;
        private int userPid;
        private string empCode;
        private string empName;
        private string department;
        private string loginDate;
        #endregion Fields

        #region Constructors
        public BOMUserLogin()
        {
            this.userName = string.Empty;
            this.userPid = int.MinValue;
            this.empCode = string.Empty;
            this.empName = string.Empty;
            this.loginDate = string.Empty;
            this.department = string.Empty;

        }
        #endregion Constructors

        #region Properties
        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        public int UserPid
        {
            get { return this.userPid; }
            set { this.userPid = value; }
        }

        public string EmpName
        {
            get { return this.empName; }
            set { this.empName = value; }
        }

        public string EmpCode
        {
            get { return this.empCode; }
            set { this.empCode = value; }
        }

        public string LoginDate
        {
            get { return this.loginDate; }
            set { this.loginDate = value; }
        }

        public string Department
        {
            get { return this.department; }
            set { this.department = value; }
        }

        #endregion Properties
    }
}