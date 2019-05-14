using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoctorsOffice.Models;
using System.Collections.Generic;
using System;

namespace DoctorsOffice.Tests
{
    [TestClass]
    public class PatientTest : IDisposable
    {

        public void Dispose()
        {
          Patient.ClearAll();
          Category.ClearAll();
        }

        public PatientTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=to_do_list_test;";
        }

        [TestMethod]
        public void PatientConstructor_CreatesInstanceOfPatient_Patient()
        {
            DateTime dueDate =  new DateTime(1999, 12, 24);
            Patient newPatient = new Patient("test", dueDate);
            Assert.AreEqual(typeof(Patient), newPatient.GetType());
        }

        [TestMethod]
        public void GetDescription_ReturnsDescription_String()
        {
            //Arrange
            DateTime dueDate =  new DateTime(1999, 12, 24);
            string description = "Walk the dog.";
            Patient newPatient = new Patient(description, dueDate);

            //Act
            string result = newPatient.GetDescription();

            //Assert
            Assert.AreEqual(description, result);
        }

        [TestMethod]
        public void SetDescription_SetDescription_String()
        {
            //Arrange
            DateTime dueDate =  new DateTime(1999, 12, 24);
            string description = "Walk the dog.";
            Patient newPatient = new Patient("test", dueDate);

            //Act
            string updatedDescription = "Do the dishes";
            newPatient.SetDescription(updatedDescription);
            string result = newPatient.GetDescription();

            //Assert
            Assert.AreEqual(updatedDescription, result);
        }

        [TestMethod]
        public void GetCompleted_ReturnsCompleted_Boolean()
        {
            //Arrange
            DateTime dueDate =  new DateTime(1999, 12, 24);
            bool completed = false;
            Patient newPatient = new Patient("test", dueDate, completed);

            //Act
            bool result = newPatient.GetCompleted();

            //Assert
            Assert.AreEqual(completed, result);
        }

        [TestMethod]
        public void SetCompleted_SetCompleted_Boolean()
        {
            //Arrange
            DateTime dueDate =  new DateTime(1999, 12, 24);
            Patient newPatient = new Patient("test", dueDate);

            //Act
            bool updatedCompleted = true;
            newPatient.SetCompleted(updatedCompleted);
            bool result = newPatient.GetCompleted();

            //Assert
            Assert.AreEqual(updatedCompleted, result);
        }

        [TestMethod]
        public void GetAll_ReturnsEmptyList_PatientList()
        {
            //Arrange
            List<Patient> newList = new List<Patient> { };

            //Act
            List<Patient> result = Patient.GetAll();

            //Assert
            CollectionAssert.AreEqual(newList, result);
        }

        [TestMethod]
        public void GetAll_ReturnsPatients_PatientList()
        {
            //Arrange
            DateTime dueDate =  new DateTime(1999, 12, 24);
            string description01 = "Walk the dog";
            string description02 = "Wash the dishes";
            Patient newPatient1 = new Patient(description01, dueDate);
            newPatient1.Save();
            Patient newPatient2 = new Patient(description02, dueDate);
            newPatient2.Save();
            List<Patient> newList = new List<Patient> { newPatient1, newPatient2 };

            //Act
            List<Patient> result = Patient.GetAll();

            //Assert
            CollectionAssert.AreEqual(newList, result);
        }

        [TestMethod]
        public void Find_ReturnsCorrectPatientFromDatabase_Patient()
        {
            //Arrange
            DateTime dueDate =  new DateTime(1999, 12, 24);
            Patient testPatient = new Patient("Mow the lawn", dueDate);
            testPatient.Save();

            //Act
            Patient foundPatient = Patient.Find(testPatient.GetId());

            //Assert
            Assert.AreEqual(testPatient, foundPatient);
        }

        [TestMethod]
        public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Patient()
        {
            // Arrange, Act
            DateTime dueDate =  new DateTime(1999, 12, 24);
            Patient firstPatient = new Patient("Mow the lawn", dueDate);
            Patient secondPatient = new Patient("Mow the lawn", dueDate);

            // Assert
            Assert.AreEqual(firstPatient, secondPatient);
        }

        [TestMethod]
        public void Save_SavesToDatabase_PatientList()
        {
            //Arrange
            DateTime dueDate =  new DateTime(1999, 12, 24);
            Patient testPatient = new Patient("Mow the lawn", dueDate);

            //Act
            testPatient.Save();
            List<Patient> result = Patient.GetAll();
            List<Patient> testList = new List<Patient>{testPatient};

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_Id()
        {
            //Arrange
            DateTime dueDate =  new DateTime(1999, 12, 24);
            Patient testPatient = new Patient("Mow the lawn", dueDate);

            //Act
            testPatient.Save();
            Patient savedPatient = Patient.GetAll()[0];

            int result = savedPatient.GetId();
            int testId = testPatient.GetId();

            //Assert
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Edit_UpdatesPatientInDatabase_String()
        {
            //Arrange
            DateTime dueDate =  new DateTime(1999, 12, 24);
            bool completed = false;
            int id = 0;
            Patient testPatient = new Patient("Walk the Dog", dueDate);
            testPatient.Save();
            string newDescription = "Mow the lawn";
            DateTime newDueDate = new DateTime(2019, 12, 24);
            bool newCompleted = true;
            int newId = 1;

            //Act
            testPatient.Edit(newDescription, newDueDate, newCompleted);
            string result = Patient.Find(testPatient.GetId()).GetDescription();

            //Assert
            Assert.AreEqual(newDescription, result);
        }

        [TestMethod]
        public void GetCategories_ReturnsAllPatientCategories_CategoryList()
        {
            //Arrange
            DateTime dueDate =  new DateTime(1999, 12, 24);
            Patient testPatient = new Patient("Mow the lawn", dueDate);
            testPatient.Save();
            Category testCategory1 = new Category("Home stuff");
            testCategory1.Save();
            Category testCategory2 = new Category("Work stuff");
            testCategory2.Save();

            //Act
            testPatient.AddCategory(testCategory1);
            List<Category> result = testPatient.GetCategories();
            List<Category> testList = new List<Category> {testCategory1};

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void AddCategory_AddsCategoryToPatient_CategoryList()
        {
            //Arrange
            DateTime dueDate =  new DateTime(1999, 12, 24);
            Patient testPatient = new Patient("Mow the lawn", dueDate);
            testPatient.Save();
            Category testCategory = new Category("Home stuff");
            testCategory.Save();

            //Act
            testPatient.AddCategory(testCategory);

            List<Category> result = testPatient.GetCategories();
            List<Category> testList = new List<Category>{testCategory};

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Delete_DeletesPatientAssociationsFromDatabase_PatientList()
        {
            //Arrange
            DateTime dueDate =  new DateTime(1999, 12, 24);
            Category testCategory = new Category("Home stuff");
            testCategory.Save();
            string testDescription = "Mow the lawn";
            Patient testPatient = new Patient(testDescription, dueDate);
            testPatient.Save();

            //Act
            testPatient.AddCategory(testCategory);
            testPatient.Delete();
            List<Patient> resultCategoryPatients = testCategory.GetPatients();
            List<Patient> testCategoryPatients = new List<Patient> {};

            //Assert
            CollectionAssert.AreEqual(testCategoryPatients, resultCategoryPatients);
        }
    }
}

// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using DoctorsOffice.Models;
// using System.Collections.Generic;
// using System;
//
// namespace DoctorsOffice.Tests
// {
//     [TestClass]
//     public class PatientTest : IDisposable
//     {
//         public void Dispose()
//         {
//             Patient.ClearAll();
//         }
//
//         public PatientTest()
//         {
//             DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=to_do_list_test;";
//         }
//
//         [TestMethod]
//         public void PatientConstructor_CreatesInstanceOfPatient_Patient()
//         {
//             //Arrange
//             DateTime itemDueDate =  new DateTime(1999, 12, 24);
//             Patient newPatient = new Patient("test", itemDueDate, 1);
//
//             //Assert
//             Assert.AreEqual(typeof(Patient), newPatient.GetType());
//         }
//
//         [TestMethod]
//         public void GetDescription_ReturnsDescription_String()
//         {
//             //Arrange
//             string description = "Walk the dog.";
//             DateTime itemDueDate =  new DateTime(1999, 12, 24);
//             Patient newPatient = new Patient(description, itemDueDate, 1);
//
//             //Act
//             string result = newPatient.GetDescription();
//
//             //Assert
//             Assert.AreEqual(description, result);
//         }
//
//         [TestMethod]
//         public void SetDescription_SetDescription_String()
//         {
//             //Arrange
//             string description = "Walk the dog.";
//             DateTime itemDueDate =  new DateTime(1999, 12, 24);
//             Patient newPatient = new Patient(description, itemDueDate, 1);
//
//             //Act
//             string updatedDescription = "Do the dishes";
//             newPatient.SetDescription(updatedDescription);
//             string result = newPatient.GetDescription();
//
//             //Assert
//             Assert.AreEqual(updatedDescription, result);
//         }
//
//         [TestMethod]
//         public void GetAll_ReturnsEmptyListFromDatabase_PatientList()
//         {
//             //Arrange
//             List<Patient> newList = new List<Patient> { };
//
//             //Act
//             List<Patient> result = Patient.GetAll();
//
//             //Assert
//             CollectionAssert.AreEqual(newList, result);
//         }
//
//         [TestMethod]
//         public void GetAll_ReturnsPatients_PatientList()
//         {
//             //Arrange
//             string description01 = "Walk the dog";
//             string description02 = "Wash the dishes";
//             DateTime itemDueDate =  new DateTime(1999, 12, 24);
//             Patient newPatient1 = new Patient(description01, itemDueDate, 1);
//             newPatient1.Save();
//             Patient newPatient2 = new Patient(description02, itemDueDate, 1);
//             newPatient2.Save();
//             List<Patient> newList = new List<Patient> { newPatient1, newPatient2 };
//
//             //Act
//             List<Patient> result = Patient.GetAll();
//
//             //Assert
//             CollectionAssert.AreEqual(newList, result);
//         }
//
//         [TestMethod]
//         public void Save_AssignsIdToObject_Id()
//         {
//             //Arrange
//             DateTime itemDueDate =  new DateTime(1999, 12, 24);
//             Patient testPatient = new Patient("Mow the lawn", itemDueDate, 1);
//
//             //Act
//             testPatient.Save();
//             Patient savedPatient = Patient.GetAll()[0];
//
//             int result = savedPatient.GetId();
//             int testId = testPatient.GetId();
//
//             //Assert
//             Assert.AreEqual(testId, result);
//         }
//
//         /*[TestMethod]
//         public void GetId_PatientsInstantiateWithAnIdAndGetterReturns_Int()
//         {
//             //Arrange
//             string description = "Walk the dog.";
//             Patient newPatient = new Patient(description, 1);
//
//             //Act
//             int result = newPatient.GetId();
//
//             //Assert
//             Assert.AreEqual(1, result);
//         }*/
//
//         [TestMethod]
//         public void Find_ReturnsCorrectPatientFromDatabase_Patient()
//         {
//             //Arrange
//             DateTime itemDueDate =  new DateTime(1999, 12, 24);
//             Patient testPatient = new Patient("Mow the lawn", itemDueDate, 1);
//             testPatient.Save();
//
//             //Act
//             Patient foundPatient = Patient.Find(testPatient.GetId());
//
//             //Assert
//             Assert.AreEqual(testPatient, foundPatient);
//         }
//
//         [TestMethod]
//         public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Patient()
//         {
//             // Arrange, Act
//             DateTime itemDueDate =  new DateTime(1999, 12, 24);
//             Patient firstPatient = new Patient("Mow the lawn", itemDueDate, 1);
//             Patient secondPatient = new Patient("Mow the lawn", itemDueDate, 1);
//
//             // Assert
//             Assert.AreEqual(firstPatient, secondPatient);
//         }
//
//         [TestMethod]
//         public void Save_SavesToDatabase_PatientList()
//         {
//             //Arrange
//             DateTime itemDueDate =  new DateTime(1999, 12, 24);
//             Patient testPatient = new Patient("Mow the lawn", itemDueDate, 1);
//
//             //Act
//             testPatient.Save();
//             List<Patient> result = Patient.GetAll();
//             List<Patient> testList = new List<Patient>{testPatient};
//
//             //Assert
//             CollectionAssert.AreEqual(testList, result);
//         }
//
//         [TestMethod]
//         public void Edit_UpdatesPatientInDatabase_String()
//         {
//             //Arrange
//             DateTime itemDueDate =  new DateTime(1999, 12, 24);
//             string firstDescription = "Walk the Dog";
//             Patient testPatient = new Patient(firstDescription, itemDueDate, 1);
//             testPatient.Save();
//             string secondDescription = "Mow the lawn";
//
//             //Act
//             testPatient.Edit(secondDescription);
//             string result = Patient.Find(testPatient.GetId()).GetDescription();
//
//             //Assert
//             Assert.AreEqual(secondDescription, result);
//         }
//
//         [TestMethod]
//         public void Delete_DeletesPatientFromDatabase_List()
//         {
//             //Arrange
//             DateTime itemDueDate =  new DateTime(1999, 12, 24);
//             Category newCategory = new Category("Home Tasks");
//             Patient newPatient1 = new Patient("Walk the dog.", itemDueDate, newCategory.GetId());
//             Patient newPatient2 = new Patient("Clean the room", itemDueDate, newCategory.GetId());
//             newPatient1.Save();
//             newPatient2.Save();
//             //Act
//             newPatient1.Delete();
//             List<Patient> result = Patient.GetAll();
//             List<Patient> newList = new List<Patient> { newPatient2 };
//             //Assert
//             CollectionAssert.AreEqual(newList, result);
//         }
//
//         [TestMethod]
//       public void GetCategories_ReturnsAllPatientCategories_CategoryList()
//       {
//           //Arrange
//           DateTime itemDueDate =  new DateTime(1999, 12, 24);
//           Patient testPatient = new Patient("Mow the lawn", itemDueDate);
//           testPatient.Save();
//           Category testCategory1 = new Category("Home stuff");
//           testCategory1.Save();
//           Category testCategory2 = new Category("Work stuff");
//           testCategory2.Save();
//
//           //Act
//           testPatient.AddCategory(testCategory1);
//           List<Category> result = testPatient.GetCategories();
//           List<Category> testList = new List<Category> {testCategory1};
//
//           //Assert
//           CollectionAssert.AreEqual(testList, result);
//       }
//
//
//
//         [TestMethod]
//         public void Delete_DeletesPatientAssociationsFromDatabase_PatientList()
//         {
//             //Arrange
//             DateTime itemDueDate =  new DateTime(1999, 12, 24);
//             Category testCategory = new Category("Home stuff");
//             testCategory.Save();
//             string testDescription = "Mow the lawn";
//             Patient testPatient = new Patient(testDescription, itemDueDate);
//             testPatient.Save();
//
//             //Act
//             testPatient.AddCategory(testCategory);
//             testPatient.Delete();
//             List<Patient> resultCategoryPatients = testCategory.GetPatients();
//             List<Patient> testCategoryPatients = new List<Patient> {};
//
//             //Assert
//             CollectionAssert.AreEqual(testCategoryPatients, resultCategoryPatients);
//         }
//     }
// }
