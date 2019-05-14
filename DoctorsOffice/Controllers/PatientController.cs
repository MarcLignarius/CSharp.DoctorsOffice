using Microsoft.AspNetCore.Mvc;
using DoctorsOffice.Models;
using System.Collections.Generic;
using System;

namespace DoctorsOffice.Controllers
{
    public class PatientsController : Controller
    {

        [HttpGet("/patients")]
        public ActionResult Index()
        {
            List<Patient> allPatients = Patient.GetAll();
            return View(allPatients);
        }

        [HttpPost("/patients")]
        public ActionResult Create(string description, DateTime dueDate)
        {
            Patient newPatient = new Patient(description, dueDate);
            newPatient.Save();
            List<Patient> allPatients = Patient.GetAll();
            return View("Index", allPatients);
        }

        [HttpGet("/patients/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpGet("/patients/{id}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Patient selectedPatient = Patient.Find(id);
            List<Doctor> patientDoctors = selectedPatient.GetDoctors();
            List<Doctor> allDoctors = Doctor.GetAll();
            model.Add("selectedPatient", selectedPatient);
            model.Add("patientDoctors", patientDoctors);
            model.Add("allDoctors", allDoctors);
            return View(model);
        }

        [HttpPost("/patients/{patientId}/doctors/new")]
        public ActionResult AddDoctor(int patientId, int doctorId)
        {
            Patient patient = Patient.Find(patientId);
            Doctor doctor = Doctor.Find(doctorId);
            patient.AddDoctor(doctor);
            return RedirectToAction("Show",  new { id = patientId });
        }

        // [HttpPost("/doctors/{doctorId}/patients/{patientId}")]
        // public ActionResult Update(int doctorId, int patientId, string newDescription, DateTime newDueDate, bool newCompleted)
        // {
        //     Patient patient = Patient.Find(patientId);
        //     patient.Edit(newDescription, newDueDate, newCompleted);
        //     Dictionary<string, object> model = new Dictionary<string, object>();
        //     Doctor doctor = Doctor.Find(doctorId);
        //     model.Add("doctor", doctor);
        //     model.Add("patient", patient);
        //     return View("Show", model);
        // }

        // [HttpPost("/doctors/{doctorId}/patients/{patientId}/delete")]
        // public ActionResult Delete(int doctorId, int patientId)
        // {
        //     Patient patient = Patient.Find(patientId);
        //     patient.Delete();
        //     Dictionary<string, object> model = new Dictionary<string, object>();
        //     Doctor foundDoctor = Doctor.Find(doctorId);
        //     List<Patient> doctorPatients = foundDoctor.GetPatients();
        //     model.Add("patients", doctorPatients);
        //     model.Add("doctor", foundDoctor);
        //     return View(model);
        // }
        //
        // [HttpGet("/doctors/{doctorId}/patients/{patientId}/edit")]
        // public ActionResult Edit(int doctorId, int patientId)
        // {
        //     Dictionary<string, object> model = new Dictionary<string, object>();
        //     Doctor doctor = Doctor.Find(doctorId);
        //     model.Add("doctor", doctor);
        //     Patient patient = Patient.Find(patientId);
        //     model.Add("patient", patient);
        //     return View(model);
        // }
        //
        // [HttpPost("/doctors/{doctorId}/patients/{patientId}")]
        // public ActionResult Update(int doctorId, int patientId, string newDescription)
        // {
        //     Patient patient = Patient.Find(patientId);
        //     patient.Edit(newDescription);
        //     Dictionary<string, object> model = new Dictionary<string, object>();
        //     Doctor doctor = Doctor.Find(doctorId);
        //     model.Add("doctor", doctor);
        //     model.Add("patient", patient);
        //     return View("Show", model);
        // }

    }
}
