using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MyOfficeApi.Models;

namespace MyOfficeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AccountController(IConfiguration config)
        {
            _config = config;
        }

        // GET: api/Account
        [HttpGet]
        public IActionResult GetAccounts()
        {
            List<Account> list = new List<Account>();

            string connStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = "SELECT ACPD_SID, ACPD_Cname, ACPD_Ename, ACPD_Email FROM MyOffice_ACPD";

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Account
                    {
                        AccountId = reader["ACPD_SID"].ToString(),
                        ChineseName = reader["ACPD_Cname"].ToString(),
                        EnglishName = reader["ACPD_Ename"].ToString(),
                        Email = reader["ACPD_Email"].ToString()
                    });
                }
            }

            return Ok(list);
        }

        // GET: api/Account/{id}
        [HttpGet("{id}")]
        public IActionResult GetAccountById(string id)
        {
            Account account = null;

            string connStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = @"SELECT ACPD_SID, ACPD_Cname, ACPD_Ename, ACPD_Email
                               FROM MyOffice_ACPD
                               WHERE ACPD_SID = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    account = new Account
                    {
                        AccountId = reader["ACPD_SID"].ToString(),
                        ChineseName = reader["ACPD_Cname"].ToString(),
                        EnglishName = reader["ACPD_Ename"].ToString(),
                        Email = reader["ACPD_Email"].ToString()
                    };
                }
            }

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // POST: api/Account
        [HttpPost]
        public IActionResult CreateAccount(Account account)
        {
            string connStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = @"INSERT INTO MyOffice_ACPD
                              (ACPD_SID, ACPD_Cname, ACPD_Ename, ACPD_Email)
                              VALUES
                              (@id, @cname, @ename, @email)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", account.AccountId);
                cmd.Parameters.AddWithValue("@cname", account.ChineseName);
                cmd.Parameters.AddWithValue("@ename", account.EnglishName);
                cmd.Parameters.AddWithValue("@email", account.Email);

                cmd.ExecuteNonQuery();
            }

            return StatusCode(201, "新增成功");
        }

        // PUT: api/Account/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateAccount(string id, Account account)
        {
            string connStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = @"UPDATE MyOffice_ACPD
                               SET ACPD_Cname = @cname,
                                   ACPD_Ename = @ename,
                                   ACPD_Email = @email
                               WHERE ACPD_SID = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@cname", account.ChineseName);
                cmd.Parameters.AddWithValue("@ename", account.EnglishName);
                cmd.Parameters.AddWithValue("@email", account.Email);

                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    return NotFound();
                }
            }

            return Ok("更新成功");
        }

        // DELETE: api/Account/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(string id)
        {
            string connStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = "DELETE FROM MyOffice_ACPD WHERE ACPD_SID = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", id);

                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    return NotFound();
                }
            }

            return NoContent();
        }
    }
}