using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using DoctorsOffice.Models;

namespace DoctorsOffice.Tests
{
    [TestClass]
    public class DoctorTests : IDisposable
    {
        public DoctorTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=to_do_list_test;";
        }

        // [TestMethod]
        // public void GetAll_CategoriesEmptyAtFirst_0()
        // {
        //     //Arrange, Act
        //     int result = Doctor.GetAll().Count;
        //
        //     //Assert
        //     Assert.AreEqual(0, result);
        // }
        //
        // [TestMethod]
        // public void Equals_ReturnsTrueIfNamesAreTheSame_Doctor()
        // {
        //     //Arrange, Act
        //     Doctor firstDoctor = new Doctor("Household chores");
        //     Doctor secondDoctor = new Doctor("Household chores");
        //
        //     //Assert
        //     Assert.AreEqual(firstDoctor, secondDoctor);
        // }
        //
        // [TestMethod]
        // public void Save_SavesDoctorToDatabase_DoctorList()
        // {
        //     //Arrange
        //     Doctor testDoctor = new Doctor("Household chores");
        //     testDoctor.Save();
        //
        //     //Act
        //     List<Doctor> result = Doctor.GetAll();
        //     List<Doctor> testList = new List<Doctor>{testDoctor};
        //
        //     //Assert
        //     CollectionAssert.AreEqual(testList, result);
        // }
        //
        // [TestMethod]
        // public void Save_DatabaseAssignsIdToDoctor_Id()
        // {
        //     //Arrange
        //     Doctor testDoctor = new Doctor("Household chores");
        //     testDoctor.Save();
        //
        //     //Act
        //     Doctor savedDoctor = Doctor.GetAll()[0];
        //
        //     int result = savedDoctor.GetId();
        //     int testId = testDoctor.GetId();
        //
        //     //Assert
        //     Assert.AreEqual(testId, result);
        // }
        //
        // [TestMethod]
        // public void Find_FindsDoctorInDatabase_Doctor()
        // {
        //     //Arrange
        //     Doctor testDoctor = new Doctor("Household chores");
        //     testDoctor.Save();
        //
        //     //Act
        //     Doctor foundDoctor = Doctor.Find(testDoctor.GetId());
        //
        //     //Assert
        //     Assert.AreEqual(testDoctor, foundDoctor);
        // }
        //
        [TestMethod]
        public void GetPatients_ReturnsAllDoctorPatients_PatientList()
        {
            //Arrange
            DateTime patientDueDate =  new DateTime(1999, 12, 24);
            Doctor testDoctor = new Doctor("Household chores");
            testDoctor.Save();
            Patient testPatient1 = new Patient("Mow the lawn", patientDueDate);
            testPatient1.Save();
            Patient testPatient2 = new Patient("Buy plane ticket", patientDueDate);
            testPatient2.Save();

            //Act
            testDoctor.AddPatient(testPatient1);
            List<Patient> savedPatients = testDoctor.GetPatients();
            List<Patient> testList = new List<Patient> {testPatient1};

            //Assert
            CollectionAssert.AreEqual(testList, savedPatients);
        }

        [TestMethod]
        public void Delete_DeletesDoctorAssociationsFromDatabase_DoctorList()
        {
            //Arrange
            DateTime patientDueDate =  new DateTime(1999, 12, 24);
            Patient testPatient = new Patient("Mow the lawn", patientDueDate);
            testPatient.Save();
            string testName = "Home stuff";
            Doctor testDoctor = new Doctor(testName);
            testDoctor.Save();

            //Act
            testDoctor.AddPatient(testPatient);
            testDoctor.Delete();
            List<Doctor> resultPatientCategories = testPatient.GetCategories();
            List<Doctor> testPatientCategories = new List<Doctor> {};

            //Assert
            CollectionAssert.AreEqual(testPatientCategories, resultPatientCategories);
        }

        [TestMethod]
        public void Test_AddPatient_AddsPatientToDoctor()
        {
            //Arrange
            Doctor testDoctor = new Doctor("Household chores");
            testDoctor.Save();
            DateTime patientDueDate =  new DateTime(1999, 12, 24);
            Patient testPatient = new Patient("Mow the lawn", patientDueDate);
            testPatient.Save();
            Patient testPatient2 = new Patient("Water the garden", patientDueDate);
            testPatient2.Save();

            //Act
            testDoctor.AddPatient(testPatient);
            testDoctor.AddPatient(testPatient2);
            List<Patient> result = testDoctor.GetPatients();
            List<Patient> testList = new List<Patient>{testPatient, testPatient2};

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        public void Dispose()
        {
            Patient.ClearAll();
            Doctor.ClearAll();
        }

    }
}
