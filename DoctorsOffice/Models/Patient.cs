using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace DoctorsOffice.Models
{
    public class Patient
    {
        private string _description;
        private DateTime _dueDate;
        private bool _completed;
        private int _id;

        public Patient (string description, DateTime dueDate, bool completed = false, int id = 0)
        {
            _description = description;
            _dueDate = dueDate;
            _completed = completed;
            _id = id;
        }

        public string GetDescription()
        {
            return _description;
        }

        public void SetDescription(string newDescription)
        {
            _description = newDescription;
        }

        public string GetDueDate()
        {
            var dueDateToString = _dueDate.ToString("D");
            return dueDateToString;
        }

        public void SetDueDate(DateTime newDueDate)
        {
            _dueDate = newDueDate;
        }

        public bool GetCompleted()
        {
            return _completed;
        }

        public void SetCompleted(bool newCompleted)
        {
            _completed = newCompleted;
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
                string patientDescription = rdr.GetString(1);
                DateTime patientDueDate = rdr.GetDateTime(2);
                bool patientCompleted = rdr.GetBoolean(3);
                Patient newPatient = new Patient(patientDescription, patientDueDate, patientCompleted, patientId);
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
            DateTime patientDueDate = new DateTime(1999, 12, 24);
            bool patientCompleted = false;
            while(rdr.Read())
            {
                patientId = rdr.GetInt32(0);
                patientName = rdr.GetString(1);
                patientDueDate = rdr.GetDateTime(2);
                patientCompleted = rdr.GetBoolean(3);
            }
            Patient newPatient = new Patient(patientName, patientDueDate, patientCompleted, patientId);
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
                bool descriptionEquality = this.GetDescription() == newPatient.GetDescription();
                bool dueDateEquality = this.GetDueDate() == newPatient.GetDueDate();
                bool completedEquality = this.GetCompleted() == newPatient.GetCompleted();
                return (idEquality && descriptionEquality && dueDateEquality && completedEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO patients (description, dueDate, completed) VALUES (@description, @dueDate, @completed);";
            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@description";
            description.Value = this._description;
            cmd.Parameters.Add(description);
            MySqlParameter dueDate = new MySqlParameter();
            dueDate.ParameterName = "@dueDate";
            dueDate.Value = this._dueDate;
            cmd.Parameters.Add(dueDate);
            MySqlParameter completed = new MySqlParameter();
            completed.ParameterName = "@completed";
            completed.Value = this._completed;
            cmd.Parameters.Add(completed);
            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Edit(string newDescription, DateTime newDueDate, bool newCompleted)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE patients SET description = @newDescription, dueDate = @newDueDate, completed = @newCompleted WHERE id = @searchId;";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);
            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@newDescription";
            description.Value = newDescription;
            cmd.Parameters.Add(description);
            MySqlParameter dueDate = new MySqlParameter();
            dueDate.ParameterName = "@newDueDate";
            dueDate.Value = newDueDate;
            cmd.Parameters.Add(dueDate);
            MySqlParameter completed = new MySqlParameter();
            completed.ParameterName = "@newCompleted";
            completed.Value = newCompleted;
            cmd.Parameters.Add(completed);
            cmd.ExecuteNonQuery();
            _description = newDescription;
            _dueDate = newDueDate;
            _completed = newCompleted;
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
            cmd.CommandText = @"DELETE FROM patients WHERE id = @PatientId; DELETE FROM doctors_patients WHERE patient_id = @PatientId;";
            MySqlParameter patientIdParameter = new MySqlParameter();
            patientIdParameter.ParameterName = "@PatientId";
            patientIdParameter.Value = this.GetId();
            cmd.Parameters.Add(patientIdParameter);
            cmd.ExecuteNonQuery();
            if (conn != null)
            {
              conn.Close();
            }
        }

        public List<Doctor> GetCategories()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT doctors.* FROM patients
                JOIN doctors_patients ON (patients.id = doctors_patients.patient_id)
                JOIN doctors ON (doctors_patients.doctor_id = doctors.id)
                WHERE patients.id = @patientId;";
            MySqlParameter patientIdParameter = new MySqlParameter();
            patientIdParameter.ParameterName = "@patientId";
            patientIdParameter.Value = _id;
            cmd.Parameters.Add(patientIdParameter);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Doctor> doctors = new List<Doctor> {};
            while(rdr.Read())
            {
                int thisDoctorId = rdr.GetInt32(0);
                string doctorName = rdr.GetString(1);
                Doctor foundDoctor = new Doctor(doctorName, thisDoctorId);
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
            cmd.CommandText = @"INSERT INTO doctors_patients (doctor_id, patient_id) VALUES (@DoctorId, @PatientId);";
            MySqlParameter doctor_id = new MySqlParameter();
            doctor_id.ParameterName = "@DoctorId";
            doctor_id.Value = newDoctor.GetId();
            cmd.Parameters.Add(doctor_id);
            MySqlParameter patient_id = new MySqlParameter();
            patient_id.ParameterName = "@PatientId";
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
