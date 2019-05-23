using GruppoCap.Core;

namespace GruppoCap.Logger.Log4Net
{
    public class RevoAdoNetAppender : log4net.Appender.AdoNetAppender
    {
        public RevoAdoNetAppender()
        {
            base.ConnectionString = Ambient.CurrentApplicationConnectionString();
        }
    }
}
