using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD_Project
{
    public class VideoStore : IVideoStore
    {
        public List<Movie> Stock;

        public List<Customer> Customers;

        private IRentals Rentals;

        public VideoStore(IRentals Ir)
        {
            Rentals = Ir;
            Stock = new List<Movie>();
            Customers = new List<Customer>();
        }

        public void RentMovie(string customermane, string movietile)
        {
            Customer thec = Customers.Find(x => x.Customer_name.Equals(customermane));

            if (thec == null)
            {
                throw new CustomerDoseNotExistExeption();
            }

            Movie them = Stock.FirstOrDefault(y => y.Title.Equals(movietile));

            if (them == null)
            {
                throw new MovieDoseNotExistExeption();
            }

            List<Rental> custrentals = Rentals.GetRentalsFor(thec.socialSecurityNumber);

            if (custrentals.Count == 3)
            {
                throw new NoMoreThan3Exeption();
            }

            foreach (Rental r in custrentals)
            {
                if (r.Movie_Title.Equals(movietile))
                {
                    throw new No2CoppiesForRenting();
                }
            }


            Rentals.AddRental(movietile, thec.socialSecurityNumber);
        }

        public bool isRented(string customermane, string movietile)
        {
            RentMovie(customermane, movietile);

            return true;
        }

        public bool hasTwoRentedMovies(string customermane)
        {
            Customer thec = Customers.Find(x => x.Customer_name.Equals(customermane));

            var custrentals = Rentals.GetRentalsFor(thec.socialSecurityNumber);

            if (custrentals.Count == 2)
            {
                return true;
            }

            return false;
        }


        public void RegisterCustomer(string name, string socialSecurityNumber)
        {
            string[] split = socialSecurityNumber.Split('-');
            if (split.Length < 3 && split.Length > 3)
            {
                throw new IncorectSocialSecurityNumberExeption();
            }
            else if (split.Length == 3)
            {
                if (split[0].Length != 4 || split[1].Length != 2 || split[2].Length != 2)
                {
                    throw new IncorectSocialSecurityNumberExeption();
                }
            }


            if (Customers.Contains(new Customer {socialSecurityNumber = socialSecurityNumber, Customer_name = name}))
            {
                throw new OnlyOneKindCustomerAlowedExeption();
            }

            Customers.Add(new Customer()
            {
                socialSecurityNumber = socialSecurityNumber,
                Customer_name = name
            });
        }

        public void AddMovie(Movie movie)
        {
            if (movie.Title.Equals(""))
            {
                throw new TitleIsEmptyExeption();
            }

            List<Movie> max3 = Stock.FindAll(x => x.Title.Equals(movie.Title));
            if (max3.Count >= 3)
            {
                throw new OnlyThreeMoviesAlowedExeption();
            }

            Stock.Add(movie);
        }

        public Rental GetRental(string customername, string movieTitle)
        {
            Customer thec = Customers.Find(x => x.Customer_name.Equals(customername));

            Rental theR = Rentals.GetRentalForMovie(movieTitle, thec.socialSecurityNumber);

            return theR;
        }

        public List<Customer> GetCustomers()
        {
            return Customers;
        }

        public void ReturnMovie(string movieTitle, string socialSecurityNumber)
        {
            throw new NotImplementedException();
        }

        public List<Rental> GetRentalsBySSN(string socialSecurityNumber)
        {
            List<Rental> rents = Rentals.GetRentalsFor(socialSecurityNumber);

            return rents;
        }
    }
}