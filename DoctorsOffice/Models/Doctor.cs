using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace DoctorsOffice.Models
{
    public class Doctor
    {
        private string _name;
        private string _specialty;
        private int _id;

        public Doctor(string name, string specialty, int id = 0)
        {
            _name = name;
            _specialty = specialty;
            _id = id;
        }

        public override bool Equals(System.Object otherDoctor)
        {
            if (!(otherDoctor is Doctor))
            {
                return false;
            }
            else
            {
                Doctor newDoctor = (Doctor) otherDoctor;
                bool idEquality = this.GetId().Equals(newDoctor.GetId());
                bool nameEquality = this.GetName().Equals(newDoctor.GetName());
                bool specialtyEquality = this.GetSpecialty().Equals(newDoctor.GetSpecialty());
                return (idEquality && nameEquality && specialtyEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetId().GetHashCode();
        }

        public string GetName()
        {
            return _name;
        }

        public string GetSpecialty()
        {
            return _specialty;
        }

        public int GetId()
        {
            return _id;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO doctors (name, specialty) VALUES (@name, @specialty);";
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);
            MySqlParameter specialty = new MySqlParameter();
            specialty.ParameterName = "@specialty";
            specialty.Value = this._specialty;
            cmd.Parameters.Add(specialty);
            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

        }

        public List<Patient> GetPatients()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT patients.* FROM doctors
                JOIN doctors_patients ON (doctors.id = doctors_patients.doctorId)
                JOIN patients ON (doctors_patients.patientid = patientId)
                WHERE doctors.id = @doctorId;";
            MySqlParameter doctorIdParameter = new MySqlParameter();
            doctorIdParameter.ParameterName = "@doctorId";
            doctorIdParameter.Value = _id;
            cmd.Parameters.Add(doctorIdParameter);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Patient> patients = new List<Patient>{};
            while(rdr.Read())
            {
                int patientId = rdr.GetInt32(0);
                string patientName = rdr.GetString(1);
                DateTime patientBirthDate = rdr.GetDateTime(2);
                Patient newPatient = new Patient(patientName, patientBirthDate, patientId);
                patients.Add(newPatient);
            }
            conn.Close();
            if (conn != null)
            {
              conn.Dispose();
            }
            return patients;
        }

        public static List<Doctor> GetAll()
        {
            List<Doctor> allDoctors = new List<Doctor> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM doctors;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int doctorId = rdr.GetInt32(0);
                string doctorName = rdr.GetString(1);
                string doctorSpecialty = rdr.GetString(2);
                Doctor newDoctor = new Doctor(doctorName, doctorSpecialty, doctorId);
                allDoctors.Add(newDoctor);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allDoctors;
        }

        public static Doctor Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM doctors WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int doctorId = 0;
            string doctorName = "";
            string doctorSpecialty = "";
            while(rdr.Read())
            {
              doctorId = rdr.GetInt32(0);
              doctorName = rdr.GetString(1);
              doctorSpecialty = rdr.GetString(2);
            }
            Doctor newDoctor = new Doctor(doctorName, doctorSpecialty, doctorId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newDoctor;
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM doctors;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM doctors WHERE id = @DoctorId; DELETE FROM doctors_patients WHERE doctorid = @DoctorId;", conn);
            MySqlParameter doctorIdParameter = new MySqlParameter();
            doctorIdParameter.ParameterName = "@DoctorId";
            doctorIdParameter.Value = this.GetId();
            cmd.Parameters.Add(doctorIdParameter);
            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }

        public void AddPatient(Patient newPatient)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO doctors_patients (doctorId, patientId) VALUES (@doctorId, @patientId);";
            MySqlParameter doctorId = new MySqlParameter();
            doctorId.ParameterName = "@doctorId";
            doctorId.Value = _id;
            cmd.Parameters.Add(doctorId);
            MySqlParameter patientId = new MySqlParameter();
            patientId.ParameterName = "@patientId";
            patientId.Value = newPatient.GetId();
            cmd.Parameters.Add(patientId);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    }
}
