using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace EstacionamientoUDCI.App_Code
{
    public class DataObjectClass
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EstacionamientoConnStr"].ToString());

        //obtiene todas la carreras
        public DataTable SelectCareers()
        {
            DataTable dt = new DataTable();

            SqlCommand sqlCmd;
            SqlDataReader dr;

            try
            {
                conn.Open();
                string query = @"SELECT id, descripcion FROM CARRERAS";
                sqlCmd = new SqlCommand(query, conn);
                dr = sqlCmd.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return dt;
        }
        //obtiene todos los alumnos
        public DataTable SelectAlumns()
        {
            DataTable dt = new DataTable();

            SqlCommand sqlCmd;
            SqlDataReader dr;

            try
            {
                conn.Open();
                string query = @"SELECT A.matricula, A.nombre, A.apellidos, A.carrera AS idCarrera, B.descripcion AS carrera, A.email, A.rol 
                                 FROM ALUMNOS AS A
                                 INNER JOIN CARRERAS AS B ON A.carrera = B.id 
                                 WHERE A.matricula <> 'admin' ORDER BY A.matricula";
                sqlCmd = new SqlCommand(query, conn);
                dr = sqlCmd.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return dt;
        }
        //actualiza los datos de un alumno
        public void UpdateAlumn(string nombre, string apellidos, string email, string rol, string idCarrera, string matricula)
        {
            try
            {
                conn.Open();
                if (conn.State.ToString() == "Open")
                {
                    string query = @"UPDATE ALUMNOS SET nombre = @nombre, apellidos = @apellidos, carrera = @carrera, email = @email,
                                     rol = @rol WHERE matricula = @matricula";

                    SqlCommand sqlCmd = new SqlCommand(query, conn);
                    sqlCmd.Parameters.Add("@nombre", SqlDbType.NVarChar, 50).Value = nombre;
                    sqlCmd.Parameters.Add("@apellidos", SqlDbType.NVarChar, 50).Value = apellidos;
                    sqlCmd.Parameters.Add("@carrera", SqlDbType.NVarChar, 50).Value = idCarrera;
                    sqlCmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    sqlCmd.Parameters.Add("@rol", SqlDbType.NVarChar, 25).Value = rol;
                    sqlCmd.Parameters.Add("@matricula", SqlDbType.VarChar, 25).Value = matricula;
                    sqlCmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
                if (conn != null) conn.Dispose();
                conn = null;
            }
        }
        //obtiene todos cajones del estacionamiento
        public DataTable SelectParkingBoxes()
        {
            DataTable dt = new DataTable();

            SqlCommand sqlCmd;
            SqlDataReader dr;

            try
            {
                conn.Open();
                string query = @"SELECT A.alias, 
                                CASE A.turno 
	                                WHEN 'M' THEN 'Matutino' ELSE 'Vespertino' 
                                END AS turno, 
                                CASE A.ocupado
	                                WHEN 0 THEN 'No' ELSE 'Si' 
                                END AS ocupado,
                                CASE CONVERT(varchar, A.auto)
	                                WHEN '0' THEN 'N/A' ELSE 
	                                (
		                                (SELECT modelo FROM MODELOS_AUTO WHERE id = B.modelo) + ' (' + 
		                                (SELECT matricula + '/' + nombre + ' ' + apellidos FROM ALUMNOS WHERE matricula = B.alumno) + ')'
	                                )
                                END AS auto
                                FROM CAJONES AS A
                                LEFT JOIN AUTOS AS B ON A.auto = B.id
                                ORDER BY A.alias";
                sqlCmd = new SqlCommand(query, conn);
                dr = sqlCmd.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return dt;
        }
        //obtiene el auto del alumno indicado
        public DataTable SelectCar(string matricula)
        {
            DataTable dt = new DataTable();

            SqlCommand sqlCmd;
            SqlDataReader dr;

            try
            {
                conn.Open();
                string query = @"SELECT A.id, B.marca, C.modelo, A.ano, A.color 
                                FROM AUTOS AS A
                                INNER JOIN MARCAS_AUTO AS B ON A.marca = B.id
                                INNER JOIN MODELOS_AUTO AS C ON A.modelo = C.id
                                WHERE alumno = @matricula";
                sqlCmd = new SqlCommand(query, conn);
                sqlCmd.Parameters.Add("@matricula", SqlDbType.NVarChar, 25).Value = matricula;
                dr = sqlCmd.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return dt;
        }
        //obtiene si un auto existe para el alumno indicado
        public int CheckCar(string matricula)
        {
            int result = -1;
            SqlCommand sqlCmd;
            
            try
            {
                conn.Open();
                string query = @"SELECT COUNT(*)
                                 FROM AUTOS
                                 WHERE alumno = @matricula";
                sqlCmd = new SqlCommand(query, conn);
                sqlCmd.Parameters.Add("@matricula", SqlDbType.NVarChar, 25).Value = matricula;
                result = Convert.ToInt32(sqlCmd.ExecuteScalar());
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return result;
        }
        //obtiene si un cajon existe con el nombre indicado
        public int CheckParkingBox(string alias)
        {
            int result = -1;
            SqlCommand sqlCmd;

            try
            {
                conn.Open();
                string query = @"SELECT COUNT(*)
                                 FROM CAJONES
                                 WHERE alias = @alias";
                sqlCmd = new SqlCommand(query, conn);
                sqlCmd.Parameters.Add("@alias", SqlDbType.NVarChar, 25).Value = alias;
                result = Convert.ToInt32(sqlCmd.ExecuteScalar());
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return result;
        }
        //obtiene si un cajon esta reservado/asignado para el alumno indicado
        public int CheckAssignedParkingBox(string matricula)
        {
            int result = -1;
            SqlCommand sqlCmd;

            try
            {
                conn.Open();
                string query = @"SELECT COUNT(*)
                                FROM CAJONES
                                WHERE auto = (SELECT id
                                FROM AUTOS AS A
                                WHERE A.alumno = @matricula)";
                sqlCmd = new SqlCommand(query, conn);
                sqlCmd.Parameters.Add("@matricula", SqlDbType.NVarChar, 25).Value = matricula;
                result = Convert.ToInt32(sqlCmd.ExecuteScalar());
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return result;
        }
        //obtiene el numero de cajones registrados
        public int CheckParkingBoxesRegistered()
        {
            int result = -1;
            SqlCommand sqlCmd;

            try
            {
                conn.Open();
                string query = @"SELECT COUNT(*) FROM CAJONES";
                sqlCmd = new SqlCommand(query, conn);
                result = Convert.ToInt32(sqlCmd.ExecuteScalar());
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return result;
        }
        //obtiene el siguiente cajon disponible
        public DataTable GetNextAvailableParking(string turno)
        {
            DataTable dt = new DataTable();

            SqlCommand sqlCmd;
            SqlDataReader dr;

            try
            {
                conn.Open();
                string query = @"SELECT TOP 1 * 
                                FROM CAJONES
                                WHERE turno = @turno AND ocupado = 0
                                ORDER BY alias";
                sqlCmd = new SqlCommand(query, conn);
                sqlCmd.Parameters.Add("@turno", SqlDbType.Char).Value = turno;
                dr = sqlCmd.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return dt;
        }
        //reserva el cajon indicado para el turno indicado
        public int ReserveParkingBox(string cajon, string turno)
        {
            int result = -1;
            try
            {
                conn.Open();
                if (conn.State.ToString() == "Open")
                {
                    string query = @"UPDATE CAJONES SET ocupado = 1 WHERE alias = @cajon AND turno = @turno";

                    SqlCommand sqlCmd = new SqlCommand(query, conn);
                    sqlCmd.Parameters.Add("@cajon", SqlDbType.NVarChar, 25).Value = cajon;
                    sqlCmd.Parameters.Add("@turno", SqlDbType.Char).Value = turno;
                    result = sqlCmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
                if (conn != null) conn.Dispose();
                conn = null;
            }
            return result;
        }
        //registra pago
        public void RegisterPayment(string matricula, string cajon, string turno, string cicloEscolar)
        {
            try
            {
                conn.Open();
                if (conn.State.ToString() == "Open")
                {
                    string query = @"INSERT INTO PAGOS(alumno,cajon,turno,ciclo_escolar,monto,pagado,estatus)
                                    VALUES
                                    (@matricula,@cajon,@turno,@cicloEscolar,@monto,@pagado,@estatus)";

                    SqlCommand sqlCmd = new SqlCommand(query, conn);
                    sqlCmd.Parameters.Add("@matricula", SqlDbType.VarChar, 25).Value = matricula;
                    sqlCmd.Parameters.Add("@cajon", SqlDbType.NVarChar, 25).Value = cajon;
                    sqlCmd.Parameters.Add("@turno", SqlDbType.Char).Value = turno;
                    sqlCmd.Parameters.Add("@cicloEscolar", SqlDbType.VarChar, 25).Value = cicloEscolar;
                    sqlCmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = 400;
                    sqlCmd.Parameters.Add("@pagado", SqlDbType.Bit).Value = 0;
                    sqlCmd.Parameters.Add("@estatus", SqlDbType.VarChar, 25).Value = "pendiente";
                    sqlCmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
                if (conn != null) conn.Dispose();
                conn = null;
            }
        }
        //obtiene las solicitudes pendientes
        public DataTable SelectPendingRequests()
        {
            DataTable dt = new DataTable();

            SqlCommand sqlCmd;
            SqlDataReader dr;

            try
            {
                conn.Open();
                string query = @"SELECT A.id, B.matricula, B.nombre + ' ' + B.apellidos AS alumno, A.cajon + ' (' + A.turno + ')' AS cajon, A.estatus
                                FROM PAGOS AS A
                                INNER JOIN ALUMNOS AS B ON A.alumno = B.matricula
                                WHERE A.estatus NOT IN('pagado','cancelado')";
                sqlCmd = new SqlCommand(query, conn);
                dr = sqlCmd.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return dt;
        }
        //actualiza la solicitud de estacionamiento indicada segun el estatus
        public void UpdateParkingRequest(int id, string currentStatus)
        {
            string nextStatus = currentStatus == "pendiente" ? "aprobado" : "pagado";

            try
            {
                conn.Open();
                if (conn.State.ToString() == "Open")
                {
                    string query = @"";

                    if (nextStatus == "pagado")
                        query = @"UPDATE PAGOS SET pagado = 1, fecha_pago = GETDATE(), estatus = @estatus WHERE id = @id";
                    else
                        query = @"UPDATE PAGOS SET estatus = @estatus WHERE id = @id";

                    SqlCommand sqlCmd = new SqlCommand(query, conn);
                    sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCmd.Parameters.Add("@estatus", SqlDbType.VarChar, 25).Value = nextStatus;
                    sqlCmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
                if (conn != null) conn.Dispose();
                conn = null;
            }
        }
        //cancela la solicitud indicada
        public void CancelParkingRequest(int id)
        {
            try
            {
                conn.Open();
                if (conn.State.ToString() == "Open")
                {
                    string query = @"UPDATE PAGOS SET estatus = @estatus WHERE id = @id";

                    SqlCommand sqlCmd = new SqlCommand(query, conn);
                    sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    sqlCmd.Parameters.Add("@estatus", SqlDbType.VarChar, 25).Value = "cancelado";
                    sqlCmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
                if (conn != null) conn.Dispose();
                conn = null;
            }
        }
        //actualiza el cajon reservado con el auto indicado
        public void UpdateParkingBox_RegisterCar(string cajon, string turno, int idAuto)
        {
            try
            {
                conn.Open();
                if (conn.State.ToString() == "Open")
                {
                    string query = @"UPDATE CAJONES SET auto = @auto WHERE alias = @cajon AND turno = @turno";

                    SqlCommand sqlCmd = new SqlCommand(query, conn);
                    sqlCmd.Parameters.Add("@auto", SqlDbType.Int).Value = idAuto;
                    sqlCmd.Parameters.Add("@cajon", SqlDbType.VarChar, 25).Value = cajon;
                    sqlCmd.Parameters.Add("@turno", SqlDbType.Char).Value = turno;
                    sqlCmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
                if (conn != null) conn.Dispose();
                conn = null;
            }
        }
        //obtiene los detalles de un pago
        public DataTable SelectPaymentDetail(int idPago)
        {
            DataTable dt = new DataTable();

            SqlCommand sqlCmd;
            SqlDataReader dr;

            try
            {
                conn.Open();
                string query = @"SELECT A.id AS folio, B.matricula, B.nombre + ' ' + B.apellidos AS nombre,
                                CASE A.turno WHEN 'M' THEN 'Matutino' ELSE 'Vespertino' END AS turno, 
                                (SELECT marca FROM MARCAS_AUTO WHERE id = C.marca) AS marca,
                                (SELECT modelo FROM MODELOS_AUTO WHERE id = C.modelo) AS modelo,
                                C.ano, C.color, A.cajon, A.monto
                                FROM PAGOS AS A
                                INNER JOIN ALUMNOS AS B ON A.alumno = B.matricula
                                INNER JOIN AUTOS AS C ON A.alumno = C.alumno
                                WHERE A.id = @id";
                sqlCmd = new SqlCommand(query, conn);
                sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = idPago;
                dr = sqlCmd.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return dt;
        }
        //obtiene los pagos pendientes
        public DataTable SelectPendingPayments()
        {
            DataTable dt = new DataTable();

            SqlCommand sqlCmd;
            SqlDataReader dr;

            try
            {
                conn.Open();
                string query = @"SELECT A.id AS folio, B.matricula, B.nombre + ' ' + B.apellidos AS nombre,
                                CASE A.turno WHEN 'M' THEN 'Matutino' ELSE 'Vespertino' END AS turno, 
                                (SELECT marca FROM MARCAS_AUTO WHERE id = C.marca) AS marca,
                                (SELECT modelo FROM MODELOS_AUTO WHERE id = C.modelo) AS modelo,
                                C.ano, C.color, A.cajon, A.monto
                                FROM PAGOS AS A
                                INNER JOIN ALUMNOS AS B ON A.alumno = B.matricula
                                INNER JOIN AUTOS AS C ON A.alumno = C.alumno
								WHERE A.estatus = 'aprobado'";
                sqlCmd = new SqlCommand(query, conn);
                dr = sqlCmd.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return dt;
        }
        //reinicia/desactiva todos los ciclos escolares
        public void ResetScholarCycles()
        {
            try
            {
                conn.Open();
                if (conn.State.ToString() == "Open")
                {
                    string query = @"UPDATE CICLOS_ESCOLARES SET estatus = 0";

                    SqlCommand sqlCmd = new SqlCommand(query, conn);
                    sqlCmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
                if (conn != null) conn.Dispose();
                conn = null;
            }
        }
        public int CheckScholarCycle(string cicloEscolar)
        {
            int result = -1;
            SqlCommand sqlCmd;

            try
            {
                conn.Open();
                string query = @"SELECT COUNT(*)
                                 FROM CICLOS_ESCOLARES
                                 WHERE ciclo = @ciclo";
                sqlCmd = new SqlCommand(query, conn);
                sqlCmd.Parameters.Add("@ciclo", SqlDbType.NVarChar, 25).Value = cicloEscolar;
                result = Convert.ToInt32(sqlCmd.ExecuteScalar());
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return result;
        }
        //registra ciclo escolar
        public void RegisterScholarCycle(string ciclo)
        {
            try
            {
                conn.Open();
                if (conn.State.ToString() == "Open")
                {
                    string query = @"INSERT INTO CICLOS_ESCOLARES(ciclo,estatus)
                                    VALUES
                                    (@ciclo,@estatus)";

                    SqlCommand sqlCmd = new SqlCommand(query, conn);
                    sqlCmd.Parameters.Add("@ciclo", SqlDbType.VarChar, 25).Value = ciclo;
                    sqlCmd.Parameters.Add("@estatus", SqlDbType.Bit).Value = 1;
                    sqlCmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
                if (conn != null) conn.Dispose();
                conn = null;
            }
        }
        //obtiene el ciclo escolar activo
        public DataTable SelectScholarCycle_Active()
        {
            DataTable dt = new DataTable();

            SqlCommand sqlCmd;
            SqlDataReader dr;

            try
            {
                conn.Open();
                string query = @"SELECT * FROM CICLOS_ESCOLARES WHERE estatus = 1";
                sqlCmd = new SqlCommand(query, conn);
                dr = sqlCmd.ExecuteReader();
                dt.Load(dr);
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return dt;
        }
        //obtiene si un usuario tiene cajon reservado para el ciclo escolar indicado
        public int CheckIfReserved(string matricula, string ciclo)
        {
            int result = -1;
            SqlCommand sqlCmd;

            try
            {
                conn.Open();
                string query = @"SELECT COUNT(*) 
                                FROM PAGOS
                                WHERE alumno = @matricula AND ciclo_escolar = @ciclo AND estatus IN('pendiente','aprobado','pagado')";
                sqlCmd = new SqlCommand(query, conn);
                sqlCmd.Parameters.Add("@matricula", SqlDbType.NVarChar, 25).Value = matricula;
                sqlCmd.Parameters.Add("@ciclo", SqlDbType.VarChar, 25).Value = ciclo;
                result = Convert.ToInt32(sqlCmd.ExecuteScalar());
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return result;
        }
        //libera todos los cajones de estacionamiento
        public void FreeParkingBoxes()
        {
            SqlCommand sqlCmd;

            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string query = @"UPDATE CAJONES SET ocupado = 0, auto = 0";

                    sqlCmd = new SqlCommand(query, conn);
                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
            }
        }
        //libera el cajon del turno indicado
        public void FreeParkingBox(string cajon, string turno)
        {
            SqlCommand sqlCmd;

            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string query = @"UPDATE CAJONES SET ocupado = 0, auto = 0 WHERE alias = @cajon AND turno = @turno";

                    sqlCmd = new SqlCommand(query, conn);
                    sqlCmd.Parameters.Add("@cajon", SqlDbType.VarChar, 25).Value = cajon;
                    sqlCmd.Parameters.Add("@turno", SqlDbType.Char, 1).Value = turno;
                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
            }
        }
        //obtiene si hay registros pendientes para el ciclo escolar indicado
        public int CheckPendingPayments_Requests(string cicloEscolar)
        {
            int result = -1;
            SqlCommand sqlCmd;

            try
            {
                conn.Open();
                string query = @"SELECT COUNT(*) 
                                FROM PAGOS
                                WHERE ciclo_escolar = @ciclo AND estatus IN('pendiente','aprobado')";
                sqlCmd = new SqlCommand(query, conn);
                sqlCmd.Parameters.Add("@ciclo", SqlDbType.VarChar, 25).Value = cicloEscolar;
                result = Convert.ToInt32(sqlCmd.ExecuteScalar());
            }
            catch (Exception)
            {

            }
            finally { if (conn.State == ConnectionState.Open) { conn.Close(); } }

            return result;
        }
    }
}