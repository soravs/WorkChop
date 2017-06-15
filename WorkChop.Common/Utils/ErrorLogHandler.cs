using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WorkChop.DataModel.Models;
using WorkChop.DataModel.Repository;

namespace WorkChop.Common.Utils
{
    public class ErrorLogHandler
    {
        private readonly UnitOfWork _unitOfwork;
        public ErrorLogHandler()
        {
            _unitOfwork = new UnitOfWork();
        }
        public void SaveException(Exception ex,string methodName)
        {
            ErrorLog errorLog = new ErrorLog()
            {
                Method = methodName,
                ErrorDateTime = DateTime.Now,
                HelpLink = ex.HelpLink,
                InnerException = ex.InnerException == null ? string.Empty : ex.InnerException.Message,
                Message = ex.Message,
                Source = ex.Source,
                StackTrace = ex.StackTrace,
                Exception = ex.ToString(),
                ExceptionId = new Guid(),
                UserId="",
            };
            _unitOfwork.ErrorLogRepository.Add(errorLog);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }
    }
}
