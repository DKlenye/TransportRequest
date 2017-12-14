using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Intranet.Utils
{
    public class EmailSender
    {
        private readonly SqlConnection connection;

        private const string recipients = "naumov@polymir.by; pashkevich@polymir.by; e.smirnov@polymir.by; logist@polymir.by; fomina@polymir.by; dydik@polymir.by; sokolov@polymir.by";
        private const string headerTpl = "Поступила новая заявка № {0}";
        private const string bodyTpl = "Поступила заявка № {0}, созданная пользователем {1} {2} по направлению - {3}. Для просмотра заявки перейдите по ссылке http://tr.lan.naftan.by/International/Details/{0}";

        public EmailSender()
        {
            connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["mail"].ConnectionString);
        }

        public void Send(int requestNumber, string userName, string userTelephone, string delivery)
        {

            Execute(
                String.Format(headerTpl,requestNumber),
                String.Format(bodyTpl,requestNumber,userName,userTelephone,delivery)
                );
        }

        private void Execute(string header, string body)
        {
            var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_send_dbmail";
            command.Parameters.Add(new SqlParameter("@recipients", recipients));
            command.Parameters.Add(new SqlParameter("@subject", header));
            command.Parameters.Add(new SqlParameter("@body", body));
            command.Parameters.Add(new SqlParameter("@body_format", "TEXT"));
            command.Parameters.Add(new SqlParameter("@profile_name", "Transport"));

            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                connection.Close();
            }

        }

    }
}