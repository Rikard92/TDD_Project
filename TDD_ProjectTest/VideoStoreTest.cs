using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using TDD_Project;

namespace TDD_ProjectTest
{
    [TestFixture]
    public class VideoStoreTest
    {
        private Rentals Rental;

        private VideoStore sut;

        private IDatateTime idtMock;

        [SetUp]
        public void VideoStoreSetup()
        {
            idtMock = Substitute.For<IDatateTime>();

            Rental = new Rentals(idtMock);

            sut = new VideoStore(Rental);
        }

        [Test]
        public void TitleIsNotAlowedEmpty()
        {
            Movie m = new Movie()
            {
                Director = "",
                Title = "",
                Genre = ""
            };

            Assert.Throws<TitleIsEmptyExeption>(() => sut.AddMovie(m));
        }

        [Test]
        public void ForthNotAlowed()
        {
            Movie m1 = new Movie()
            {
                Director = "Director",
                Title = "MovieTitle",
                Genre = "MovieGenre"
            };
            sut.AddMovie(m1);
            Movie m2 = new Movie()
            {
                Director = "Director",
                Title = "MovieTitle",
                Genre = "MovieGenre"
            };
            sut.AddMovie(m2);
            Movie m3 = new Movie()
            {
                Director = "Director",
                Title = "MovieTitle",
                Genre = "MovieGenre"
            };
            sut.AddMovie(m3);
            Movie m4 = new Movie()
            {
                Director = "Director",
                Title = "MovieTitle",
                Genre = "MovieGenre"
            };

            Assert.Throws<OnlyThreeMoviesAlowedExeption>(() => sut.AddMovie(m4));
        }

        [Test]
        public void NoTwoCustomers()
        {
            
            sut.RegisterCustomer("Riakrd Persson", "1992-11-01");

            Assert.Throws<OnlyOneKindCustomerAlowedExeption>(() => sut.RegisterCustomer("Riakrd Persson", "1992-11-01"));
        }

        [Test]
        public void ProperSocNum()
        {

            Assert.Throws<IncorectSocialSecurityNumberExeption>(() => sut.RegisterCustomer("Riakrd Persson", "1992-110-1"));
        }

        [Test]
        public void RentNonExistingMovie()
        {
            sut.RegisterCustomer("Riakrd Persson", "1992-11-01");

            Assert.Throws<MovieDoseNotExistExeption>(() => sut.RentMovie("Riakrd Persson", "Movietitle"));
        }

        [Test]
        public void MustBeRegisterd()
        {
            Movie m = new Movie()
            {
                Director = "Director",
                Title = "MovieTitle",
                Genre = "MovieGenre"
            };
            sut.AddMovie(m);

            Assert.Throws<CustomerDoseNotExistExeption>(() => sut.RentMovie("Riakrd Persson", "MovieTitle"));
        }
        

        //IRentaltests

        [Test]
        public void AddARental()
        {

            sut.RegisterCustomer("Riakrd Persson", "1992-11-01");
            
            sut.AddMovie(new Movie()
            {
                Director = "Director",
                Title = "MovieTitle",
                Genre = "MovieGenre"
            });

            //sut.RentMovie("Riakrd Persson", "MovieTitle");

            Assert.AreEqual(true, sut.isRented("Riakrd Persson", "MovieTitle"));
        }

        [Test]
        public void GetThreeDays()
        {
            idtMock.Now().Returns(new DateTime(2010, 10, 10));

            DateTime theTime = idtMock.Now().AddDays(3);

            sut.RegisterCustomer("Riakrd Persson", "1992-11-01");

            sut.AddMovie(new Movie()
            {
                Director = "Director",
                Title = "MovieTitle",
                Genre = "MovieGenre"
            });

            sut.RentMovie("Riakrd Persson", "MovieTitle");

            Assert.AreEqual(theTime, sut.GetRental("Riakrd Persson", "MovieTitle").Due_Date);
        }
        
        [Test]
        public void GetRentalsBySocialSecuratyNumber()
        {
            sut.RegisterCustomer("Riakrd Persson", "1992-11-01");

            sut.AddMovie(new Movie()
            {
                Director = "Director",
                Title = "MovieTitle",
                Genre = "MovieGenre"
            });

            sut.RentMovie("Riakrd Persson", "MovieTitle");


        }

        [Test]
        public void RentMoreThanOne()
        {
            sut.RegisterCustomer("Riakrd Persson", "1992-11-01");

            sut.AddMovie(new Movie()
            {
                Director = "Director",

                Title = "MovieTitle",
                Genre = "MovieGenre"
            });
            sut.AddMovie(new Movie()
            {
                Director = "Director2",
                Title = "MovieTitle2",
                Genre = "MovieGenre2"
            });

            sut.RentMovie("Riakrd Persson", "MovieTitle");
            sut.RentMovie("Riakrd Persson", "MovieTitle2");

            Assert.AreEqual(true, sut.hasTwoRentedMovies("Riakrd Persson"));

        }

        [Test]
        public void CantRentMoreThan3()
        {

            sut.RegisterCustomer("Riakrd Persson", "1992-11-01");

            sut.AddMovie(new Movie()
            {
                Director = "Director",
                Title = "MovieTitle",
                Genre = "MovieGenre"
            });
            sut.AddMovie(new Movie()
            {
                Director = "Director2",
                Title = "MovieTitle2",
                Genre = "MovieGenre2"
            });
            sut.AddMovie(new Movie()
            {
                Director = "Director3",
                Title = "MovieTitle3",
                Genre = "MovieGenre3"
            });
            sut.AddMovie(new Movie()
            {
                Director = "Director4",
                Title = "MovieTitle4",
                Genre = "MovieGenre4"
            });

            sut.RentMovie("Riakrd Persson", "MovieTitle");
            sut.RentMovie("Riakrd Persson", "MovieTitle2");
            sut.RentMovie("Riakrd Persson", "MovieTitle3");

            Assert.Throws<NoMoreThan3Exeption>(() => sut.RentMovie("Riakrd Persson", "MovieTitle4"));
        }

        [Test]
        public void NoTwoCopies()
        {
            sut.RegisterCustomer("Riakrd Persson", "1992-11-01");

            sut.AddMovie(new Movie()
            {
                Director = "Director",

                Title = "MovieTitle",
                Genre = "MovieGenre"
            });
            sut.RentMovie("Riakrd Persson", "MovieTitle");

            Assert.Throws<No2CoppiesForRenting>(() => sut.RentMovie("Riakrd Persson", "MovieTitle"));

        }



    }
}
