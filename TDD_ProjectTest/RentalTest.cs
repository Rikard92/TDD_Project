using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using TDD_Project;

namespace TDD_ProjectTest
{
    [TestFixture]
    public class RentalTest
    {

        private Rentals sut;

        private IDatateTime idtMock;

        [SetUp]
        public void RentalTestSetup()
        {
            idtMock = Substitute.For<IDatateTime>();
            

            sut = new Rentals(idtMock);
        }

        [Test]
        public void CantRentIfLate()
        {

            idtMock.Now().Returns(new DateTime(2010,10,10));

            sut.AddRental("Rambo", "1992-11-01");

            idtMock.Now().Returns(new DateTime(2010, 10, 10).AddDays(4));

            Assert.Throws<DueDateExpiredExeption>(() => sut.AddRental("Rambo2", "1992-11-01"));

        }
    }
}
