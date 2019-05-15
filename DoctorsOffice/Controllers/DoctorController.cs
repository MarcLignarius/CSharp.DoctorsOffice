using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using DoctorsOffice.Models;

namespace DoctorsOffice.Controllers
{
    public class DoctorsController : Controller
    {

        [HttpGet("/doctors")]
        public ActionResult Index()
        {
            List<Doctor> allDoctors = Doctor.GetAll();
            return View(allDoctors);
        }

        [HttpPost("/doctors")]
        public ActionResult Create(string doctorName, string doctorSpecialty)
        {
            Doctor newDoctor = new Doctor(doctorName, doctorSpecialty);
            newDoctor.Save();
            List<Doctor> allDoctors = Doctor.GetAll();
            return View("Index", allDoctors);
        }

        [HttpGet("/doctors/new")]
        public ActionResult New()
        {
          return View();
        }

        [HttpGet("/doctors/{id}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Doctor selectedDoctor = Doctor.Find(id);
            List<Patient> doctorPatients = selectedDoctor.GetPatients();
            List<Patient> allPatients = Patient.GetAll();
            model.Add("doctor", selectedDoctor);
            model.Add("doctorPatients", doctorPatients);
            model.Add("allPatients", allPatients);
            return View(model);
        }

        [HttpPost("/doctors/{doctorId}/patients/new")]
        public ActionResult AddPatient(int doctorId, int patientId)
        {
            Doctor doctor = Doctor.Find(doctorId);
            Patient patient = Patient.Find(patientId);
            doctor.AddPatient(patient);
            return RedirectToAction("Show",  new { id = doctorId });
        }

        // [HttpGet("/doctors")]
        // public ActionResult Index()
        // {
        //     List<Doctor> allDoctors = Doctor.GetAll();
        //     return View(allDoctors);
        // }
        //
        //
    }
}
