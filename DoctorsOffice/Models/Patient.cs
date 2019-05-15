using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace DoctorsOffice.Models
{
    public class Patient
    {
        private string _name;
        private DateTime _birthDate;
        private int _id;

        public Patient (string name, DateTime birthDate, int id = 0)
        {
            _name = name;
            _birthDate = birthDate;
            _id = id;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string newName)
        {
            _name = newName;
        }

        public string GetBirthDate()
        {
            var birthDateToString = _birthDate.ToString("D");
            return birthDateToString;
        }

        public void SetBirthDate(DateTime newBirthDate)
        {
            _birthDate = newBirthDate;
        }

        public int GetId()
        {
            return _id;
        }

        public static List<Patient> GetAll()
        {
            List<Patient> allPatients = new List<Patient> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM patients;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int patientId = rdr.GetInt32(0);
                string patientName = rdr.GetString(1);
                DateTime patientBirthDate = rdr.GetDateTime(2);
                Patient newPatient = new Patient(patientName, patientBirthDate, patientId);
                allPatients.Add(newPatient);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allPatients;
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM patients;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Patient Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM patients WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int patientId = 0;
            string patientName = "";
            DateTime patientBirthDate = new DateTime(1999, 12, 24);
            while(rdr.Read())
            {
                patientId = rdr.GetInt32(0);
                patientName = rdr.GetString(1);
                patientBirthDate = rdr.GetDateTime(2);
            }
            Patient newPatient = new Patient(patientName, patientBirthDate, patientId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newPatient;
        }

        public override bool Equals(System.Object otherPatient)
        {
            if (!(otherPatient is Patient))
            {
                return false;
            }
            else
            {
                Patient newPatient = (Patient) otherPatient;
                bool idEquality = this.GetId() == newPatient.GetId();
                bool nameEquality = this.GetName() == newPatient.GetName();
                bool birthDateEquality = this.GetBirthDate() == newPatient.GetBirthDate();

                return (idEquality && nameEquality && birthDateEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO patients (name, birth_date) VALUES (@name, @birth_date);";
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);
            MySqlParameter birthDate = new MySqlParameter();
            birthDate.ParameterName = "@birth_date";
            birthDate.Value = this._birthDate;
            cmd.Parameters.Add(birthDate);
            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Edit(string newName, DateTime newBirthDate)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE patients SET name = @new_name, birthDate = @new_birth_date WHERE id = @search_id;";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@search_id";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@new_name";
            name.Value = newName;
            cmd.Parameters.Add(name);
            MySqlParameter birthDate = new MySqlParameter();
            birthDate.ParameterName = "@new_birth_date";
            birthDate.Value = newBirthDate;
            cmd.Parameters.Add(birthDate);
            cmd.ExecuteNonQuery();
            _name = newName;
            _birthDate = newBirthDate;
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
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM patients WHERE id = @patient_id; DELETE FROM doctors_patients WHERE patient_id = @patient_id;";
            MySqlParameter patientIdParameter = new MySqlParameter();
            patientIdParameter.ParameterName = "@patient_id";
            patientIdParameter.Value = this.GetId();
            cmd.Parameters.Add(patientIdParameter);
            cmd.ExecuteNonQuery();
            if (conn != null)
            {
              conn.Close();
            }
        }

        public List<Doctor> GetDoctors()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT doctors.* FROM patients
                JOIN doctors_patients ON (patients.id = doctors_patients.patient_id)
                JOIN doctors ON (doctors_patients.doctor_id = doctors.id)
                WHERE patients.id = @patient_id;";
            MySqlParameter patientIdParameter = new MySqlParameter();
            patientIdParameter.ParameterName = "@patient_id";
            patientIdParameter.Value = _id;
            cmd.Parameters.Add(patientIdParameter);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Doctor> doctors = new List<Doctor> {};
            while(rdr.Read())
            {
                int doctorId = rdr.GetInt32(0);
                string doctorName = rdr.GetString(1);
                string doctorSpecialty = rdr.GetString(2);
                Doctor foundDoctor = new Doctor(doctorName, doctorSpecialty, doctorId);
                doctors.Add(foundDoctor);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return doctors;
        }

        public void AddDoctor(Doctor newDoctor)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO doctors_patients (doctor_id, patient_id) VALUES (@doctor_id, @patient_id);";
            MySqlParameter doctor_id = new MySqlParameter();
            doctor_id.ParameterName = "@doctor_id";
            doctor_id.Value = newDoctor.GetId();
            cmd.Parameters.Add(doctor_id);
            MySqlParameter patient_id = new MySqlParameter();
            patient_id.ParameterName = "@patient_id";
            patient_id.Value = _id;
            cmd.Parameters.Add(patient_id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
